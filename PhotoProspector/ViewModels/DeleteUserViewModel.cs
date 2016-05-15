using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using PhotoProspector.Validations;

namespace PhotoProspector.ViewModels
{
    public class DeleteUserViewModel
    {
        public DeleteUserViewModel() : this(DeleteUserStatus.Initial, "") { }

        public DeleteUserViewModel(DeleteUserStatus status = DeleteUserStatus.Initial, string errorMessage = "")
        {
            Status = status;
            ErrorMessage = errorMessage;
        }

        public string alias { get; set; }

        [Required]
        [DisplayName("Email")]
        public string MSEmail
        {
            get
            {
                return alias + "@microsoft.com";
            }
            set
            {
                var mail = new MailAddress(value);
                alias = mail.User;
            }
        }

        [DefaultValue("")]
        [InvidationCodeMatchEmail]
        [DisplayName("Code")]
        public string SignUpCode { get; set; }

        public DeleteUserStatus Status { get; private set; }

        public string ErrorMessage { get; private set; }

        public enum DeleteUserStatus
        {
            Initial = 1,
            DeleteUserSucceed = 2,
            DeleteUserFailed = 3
        }
    }
}
