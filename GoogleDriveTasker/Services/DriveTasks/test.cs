using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleDriveTasker.Entities;
using GoogleDriveTasker.Interfaces;

namespace GoogleDriveTasker.Services
{
    public class Test1 : IGoogleDriveFileTask
    {
        public Test1()
        {
        }

        public Task ExecuteAsync(GoogleDriveFile googleDriveFile)
        {
            return Task.CompletedTask;
        }
    }

    public class Test2 : IGoogleDriveFileTask
    {
        public Test2()
        {
        }

        public Task ExecuteAsync(GoogleDriveFile googleDriveFile)
        {
            return Task.CompletedTask;
        }
    }
}