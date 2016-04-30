using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using PhotoProspector.Models;
using PhotoProspector.Validations;

namespace PhotoProspector.ViewModels
{
    public class SignUpViewModel : Person
    {
        [Required]
        public new HttpPostedFileBase photoPath { get; set; }

        [DefaultValue("")]
        [EqualValue("abc", ErrorMessage = "Invitation Code is not correct.")]
        [DisplayName("Code")]
        public string SignUpCode { get; set; }
    }
}