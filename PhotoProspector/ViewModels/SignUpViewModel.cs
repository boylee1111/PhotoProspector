using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using PhotoProspector.Helpers;
using PhotoProspector.Models;
using PhotoProspector.Validations;

namespace PhotoProspector.ViewModels
{
    public class SignUpViewModel : Person
    {
        public SignUpViewModel() : this(SignUpStatus.Initial, "") { }

        public SignUpViewModel(SignUpStatus status = SignUpStatus.Initial, string errorMessage = "")
        {
            Status = status;
            ErrorMessage = errorMessage;
            base.photoPath = title = specialty = team = favoritesport = "";
        }

        [Required]
        public new HttpPostedFileBase photoPath { get; set; }

        [DefaultValue("")]
        [EqualValue(PhotoConstants.cInvitationCode, ErrorMessage = "Invitation Code is not correct.")]
        [DisplayName("Code")]
        public string SignUpCode { get; set; }

        public SignUpStatus Status { get; private set; }

        public string ErrorMessage { get; private set; }

        public enum SignUpStatus
        {
            Initial = 1,
            SignUpSucceed = 2,
            SignUpFailed = 3
        }
    }
}