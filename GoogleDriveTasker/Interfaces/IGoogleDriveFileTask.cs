using System.Threading.Tasks;
using GoogleDriveTasker.Entities;

namespace GoogleDriveTasker.Interfaces
{
    public interface IGoogleDriveFileTask
    {
        Task ExecuteAsync(GoogleDriveFile googleDriveFile);
    }
}