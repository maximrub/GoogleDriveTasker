using System.Threading.Tasks;
using GoogleDriveTasker.Entities;

namespace GoogleDriveTasker.Interfaces.DriveTasks
{
    public interface IGoogleDriveFileTask
    {
        string Name { get; }
        Task ExecuteAsync(GoogleDriveFile googleDriveFile);
    }
}