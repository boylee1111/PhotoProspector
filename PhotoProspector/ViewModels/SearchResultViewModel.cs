using System.Collections.Generic;

namespace PhotoProspector.ViewModels
{
    public class SearchResultViewModel
    {
        public string Alias { get; set; }
        public IList<ImageFileViewModel> Images { get; set; }
    }
}
