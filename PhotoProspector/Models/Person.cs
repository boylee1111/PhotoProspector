using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PhotoProspector.Models
{
    public class Person
    {
        [Required]
        [DisplayName("Name")]
        public virtual string displayname { get; set; }

        [Required]
        [DisplayName("Avatar")]
        public virtual string photoPath { get; set; }

        [DisplayName("Alias")]
        public virtual string alias { get; set; }

        [DisplayName("Title")]
        [DefaultValue("")]
        public virtual string title { get; set; }

        [DisplayName("Specialty")]
        [DefaultValue("")]
        public virtual string specialty { get; set; }

        [DisplayName("Team")]
        [DefaultValue("")]
        public virtual string team { get; set; }

        [DisplayName("Favorite Sport")]
        [DefaultValue("")]
        public virtual string favoritesport { get; set; }

    }
}
