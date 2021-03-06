﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Web;
using PhotoProspector.Models;
using PhotoProspector.Validations;

namespace PhotoProspector.ViewModels
{
    public class SignUpViewModel : Person
    {
        [Required]
        public new HttpPostedFileBase photoPath { get; set; }

        [Required]
        [EmailWithDomain("microsoft.com")]
        [DisplayName("Email")]
        public string MSEmail
        {
            get
            {
                return alias + "@microsoft.com";
            }
            set
            {
                try
                {
                    var mail = new MailAddress(value);
                    alias = mail.User;
                    Email = value;
                }
                catch
                {
                    alias = "";
                }
            }
        }

        public SignUpViewModel() : this(SignUpStatus.Initial, "")
        {
            IsCustomer = false;
        }

        public SignUpViewModel(SignUpStatus status = SignUpStatus.Initial, string errorMessage = "")
        {
            Status = status;
            ErrorMessage = errorMessage;
            base.photoPath = title = specialty = team = favoritesport = "";
        }

        [DefaultValue("")]
        [Required]
        [InvidationCodeMatchEmail]
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