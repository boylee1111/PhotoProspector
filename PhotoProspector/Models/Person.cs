using System.ComponentModel;

namespace PhotoProspector.Models
{
    public class Person
    {
        [DisplayName("Name")]
        public string displayname { get; set; }

        [DisplayName("Avatar")]
        public string photoPath { get; set; }

        [DisplayName("Alias")]
        public string alias { get; set; }

        [DisplayName("Title")]
        public string title { get; set; }

        [DisplayName("Specialty")]
        public string specialty { get; set; }

        [DisplayName("Team")]
        public string team { get; set; }

        [DisplayName("Favorite Sport")]
        public string favoritesport { get; set; }

    }
}
