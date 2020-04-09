using System.Threading.Tasks;

namespace GoogleDriveTasker.Interfaces
{
    public interface ITasksRunner
    {
        Task ExecuteAsync();
    }
}