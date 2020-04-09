using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleDriveTasker.Entities;
using GoogleDriveTasker.Interfaces;

namespace GoogleDriveTasker.Services
{
    public class GoogleDriveFileTasksComposite : IGoogleDriveFileTask
    {
        private readonly IEnumerable<IGoogleDriveFileTask> _googleDriveFileTasks;

        public GoogleDriveFileTasksComposite(IEnumerable<IGoogleDriveFileTask> googleDriveFileTasks)
        {
            _googleDriveFileTasks = googleDriveFileTasks;
        }

        public async Task ExecuteAsync(GoogleDriveFile googleDriveFile)
        {
            foreach (IGoogleDriveFileTask task in _googleDriveFileTasks)
            {
                await task.ExecuteAsync(googleDriveFile);
            }
        }
    }
}