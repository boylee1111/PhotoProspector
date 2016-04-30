namespace PhotoProspector.Services
{
    public interface IFileService
    {
        string filenameHashed(string filename);
    }

    class FileService : IFileService
    {
        public string filenameHashed(string filename)
        {
            var arrays = filename.Split('.');
            string hashedName = arrays[0].GetHashCode() + "." + arrays[arrays.Length - 1];
            return hashedName;
        }
    }
}