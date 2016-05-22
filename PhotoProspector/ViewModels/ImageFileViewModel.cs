namespace PhotoProspector.ViewModels
{
    public class ImageFileViewModel
    {
        public ImageFileViewModel()
        {
            Url = "";
            Filename = "";
        }

        public ImageFileViewModel(string url, string filename)
        {
            Url = url;
            Filename = filename;
        }
        public string Url { get; set; }
        public string Filename { get; set; }
    }
}
