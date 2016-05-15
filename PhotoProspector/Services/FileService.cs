using System;
using System.Linq;
using System.Web;

namespace PhotoProspector.Services
{
    public interface IFileService
    {
        string filenameHashed(string filename);
        bool IsImage(HttpPostedFileBase file);
    }

    class FileService : IFileService
    {
        private readonly string[] _imageFileExtensions = { ".jpg", ".png", ".jpeg" };

        public string filenameHashed(string filename)
        {
            var arrays = filename.Split('.');
            string hashedName = arrays[0].GetHashCode() + "." + arrays[arrays.Length - 1];
            return hashedName;
        }

        public bool IsImage(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return false;
            }
            return file.ContentType.Contains("image") ||
                _imageFileExtensions.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}