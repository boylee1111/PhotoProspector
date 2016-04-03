using System;

namespace PhotoProspector.Service.Service
{
    public interface IUploadService
    {
        void CleanUpTempFolder();
    }
    class UploadService : IUploadService
    {
        public void CleanUpTempFolder()
        {
            throw new NotImplementedException();
        }
    }
}
