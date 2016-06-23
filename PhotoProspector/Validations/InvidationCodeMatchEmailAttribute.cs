using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Autofac;
using PhotoProspector.Helpers;
using PhotoProspector.Models;
using PhotoProspector.Services;
using PhotoProspector.ViewModels;

namespace PhotoProspector.Validations
{
    public class InvidationCodeMatchEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var viewModel = validationContext.ObjectInstance;
            MailAddress emailAddress = null;

            try
            {
                if (viewModel is SignUpViewModel)
                {
                    emailAddress = new MailAddress(((SignUpViewModel)viewModel).MSEmail);
                }
                else if (viewModel is DeleteUserViewModel)
                {
                    emailAddress = new MailAddress(((DeleteUserViewModel)viewModel).MSEmail);
                }
                else if (viewModel is CustomerSignUpViewModel)
                {
                    emailAddress = new MailAddress(((CustomerSignUpViewModel)viewModel).MSEmail);
                }
            }
            catch
            {
                return new ValidationResult("Email is not correct");
            }

            var userService = PhotoConstants.IoCContainer.Resolve<IUserService>();
            var invitationCodeStatus = userService.CheckInvitationCode(emailAddress.User, value as string);

            switch (invitationCodeStatus)
            {
                case InvitationCodeStatus.Matched:
                    return ValidationResult.Success;
                case InvitationCodeStatus.NotMatch:
                    return new ValidationResult("Invitation Code is not macthed with your email.");
                case InvitationCodeStatus.NotRegister:
                    return new ValidationResult("This email address haven't register for invitation code.");
            }

            return ValidationResult.Success;
        }
    }
}
