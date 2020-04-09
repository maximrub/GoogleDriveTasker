using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using GoogleDriveTasker.Entities;
using GoogleDriveTasker.Interfaces;
using GoogleDriveTasker.Interfaces.DriveTasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using File = Google.Apis.Drive.v3.Data.File;

namespace GoogleDriveTasker.Services
{
    public class TasksRunner : ITasksRunner
    {
        private readonly ILogger<TasksRunner> _logger;
        private readonly IAuthenticator _authenticator;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly IGoogleDriveFileTask _googleDriveFileTask;

        public TasksRunner(ILogger<TasksRunner> logger, IAuthenticator authenticator, IConfiguration configuration, IMemoryCache cache, IGoogleDriveFileTask googleDriveFileTask)
        {
            _logger = logger;
            _authenticator = authenticator;
            _configuration = configuration;
            _cache = cache;
            _googleDriveFileTask = googleDriveFileTask;
        }

        public async Task ExecuteAsync()
        {
            UserCredential credentials = await _authenticator.AuthorizeAsync(DriveService.Scope.Drive);

            // Create Drive API service.
            DriveService service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credentials,
                ApplicationName = _configuration["ApplicationName"]
            });

            // Define parameters of request.
            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 5;
            listRequest.Q = "trashed = false and mimeType != 'application/vnd.google-apps.folder'";
            listRequest.Fields = "nextPageToken, files(id, name, kind, md5Checksum, originalFilename, size, mimeType, parents)";

            FileList fileList = null;

            do
            {
                listRequest.PageToken = fileList?.NextPageToken;
                fileList = await listRequest.ExecuteAsync();
                if (fileList?.Files == null)
                {
                    continue;
                }

                List<Task> tasks = new List<Task>();

                foreach (File file in fileList.Files)
                {
                    string fullName = await GetFileFullNameAsync(file, service);
                    GoogleDriveFile googleDriveFile = new GoogleDriveFile(file.Id, file.Name, fullName, file.Md5Checksum, file.Size ?? 0);
                    tasks.Add(_googleDriveFileTask.ExecuteAsync(googleDriveFile));
                }

                await Task.WhenAll(tasks);
            } while (fileList != null && !string.IsNullOrEmpty(fileList.NextPageToken));
        }

        private async Task<string> GetFileFullNameAsync(File file, DriveService service)
        {
            if (file.Parents == null || !file.Parents.Any())
            {
                // stop when we get to the root ('My Drive' folder)
                return string.Empty;
            }

            string parentId = file.Parents[0];

            File parent = await _cache.GetOrCreateAsync<File>(key: parentId, async entry =>
            {
                FilesResource.GetRequest getRequest = service.Files.Get(parentId);
                getRequest.Fields = "id, name, parents";
                File item = await getRequest.ExecuteAsync();
                entry.SetSize(1);
                return item;
            });

            return Path.Combine(await GetFileFullNameAsync(parent, service), file.Name);
        }
    }
}