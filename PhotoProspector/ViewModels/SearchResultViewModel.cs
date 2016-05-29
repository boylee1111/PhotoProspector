using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotoProspector.ViewModels
{
    public class SearchResultViewModel
    {
        public string Alias { get; set; }
        public IList<ImageFileViewModel> Images { get; set; }

        public SearchResultViewModel()
        {
            Alias = "";
            Images = new List<ImageFileViewModel>();
        }

        public SearchResultViewModel(string alias, string ImgFolderAbsolutePath, string ImgFolderRelativePath)
        {
            if (!Directory.Exists(ImgFolderAbsolutePath))
            {
                Directory.CreateDirectory(ImgFolderAbsolutePath);
            }

            var imageFiles = new List<ImageFileViewModel>();
            Directory.GetFiles(ImgFolderAbsolutePath).ToList().ForEach(f =>
            {
                var fileNameWithoutPath = Path.GetFileName(f);
                var imagePathUrl = ImgFolderRelativePath + fileNameWithoutPath;
                var imageFileModel = new ImageFileViewModel(imagePathUrl, fileNameWithoutPath);
                imageFiles.Add(imageFileModel);
            });

            Alias = alias;
            Images = imageFiles;
        }
    }
}
