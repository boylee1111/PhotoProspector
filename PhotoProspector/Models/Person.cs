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
        [DataType(DataType.EmailAddress)]
        [DisplayName("Customer Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

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

        [DisplayName("Favorite Foods")]
        public virtual string FavoriteFoods { get; set; }

        [DisplayName("Interests")]
        public virtual string Interests { get; set; } // e.g. Investing, Fishing, Digital Photography

        [DisplayName("Phone")]
        public virtual string Phone { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "Only less than 400 characters are allowed")]
        [DisplayName("Personal Experiences")] // Like education
        public virtual string PersonalExperiences { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "Only less than 400 characters are allowed")]
        [DisplayName("Worked Companies")]
        public virtual string WorkedCompanies { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "Only less than 400 characters are allowed")]
        [DisplayName("Participated Projects")]
        public virtual string ParticipatedProjects { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "Only less than 400 characters are allowed")]
        [DisplayName("Cooperated Microsoft Employees")]
        public virtual string CooperatedMSEmployees { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "Only less than 400 characters are allowed")]
        [DisplayName("Hornors")]
        public virtual string Hornors { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(400, ErrorMessage = "Only less than 400 characters are allowed")]
        [DisplayName("Additional Information")]
        public virtual string Comment { get; set; } // Any valuable information could be added as comment

        public virtual bool IsCustomer { get; set; }

        public Person()
        {
            IsCustomer = false;
        }
    }
}
