using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleDriveTasker.Entities;
using GoogleDriveTasker.Interfaces.DriveTasks;
using Microsoft.Extensions.Logging;

namespace GoogleDriveTasker.Services
{
    public class GoogleDriveFileTasksComposite : IGoogleDriveFileTask
    {
        private readonly IEnumerable<IGoogleDriveFileTask> _googleDriveFileTasks;
        private readonly ILogger<GoogleDriveFileTasksComposite> _logger;

        public GoogleDriveFileTasksComposite(IEnumerable<IGoogleDriveFileTask> googleDriveFileTasks, ILogger<GoogleDriveFileTasksComposite> logger)
        {
            _googleDriveFileTasks = googleDriveFileTasks;
            _logger = logger;
        }

        public string Name => "GoogleDriveFileTasksComposite";

        public async Task ExecuteAsync(GoogleDriveFile googleDriveFile)
        {
            foreach (IGoogleDriveFileTask task in _googleDriveFileTasks)
            {
                try
                {
                    _logger.LogInformation($"Starting executing '{task.Name}' on file '{googleDriveFile.FullName}'");
                    await task.ExecuteAsync(googleDriveFile);
                    _logger.LogInformation($"Completed executing '{task.Name}' on file '{googleDriveFile.FullName}'");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred while executing '{task.Name}' on file '{googleDriveFile.FullName}'");
                }
                
            }
        }
    }
}