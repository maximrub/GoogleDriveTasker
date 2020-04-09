using System.Threading.Tasks;

namespace GoogleDriveTasker.Interfaces.DriveTasks
{
    public interface ITasksRunner
    {
        Task ExecuteAsync();
    }
}