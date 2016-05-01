using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PhotoProspector.Helpers;
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

        [Required]
        public string alias { get; set; }

        [DefaultValue("")]
        [EqualValue(PhotoConstants.cInvitationCode, ErrorMessage = "Invitation Code is not correct.")]
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
