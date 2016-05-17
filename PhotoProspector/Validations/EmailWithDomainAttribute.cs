using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;

namespace PhotoProspector.Validations
{
    public class EmailWithDomainAttribute : ValidationAttribute
    {
        private List<string> acceptDomains;

        public EmailWithDomainAttribute(string domain)
        {
            acceptDomains = new List<string>();
            acceptDomains.Add(domain);
        }

        public EmailWithDomainAttribute(IEnumerable<string> domains)
        {
            if (domains != null)
            {
                acceptDomains = new List<string>(domains);
            }
            else
            {
                acceptDomains = new List<string>();
            }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var email = new MailAddress(value as string);
                var host = email.Host;
                if (acceptDomains.Any(d => d == host))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    string accecptDomainString = string.Join(",", acceptDomains);
                    return new ValidationResult("The value only accepts email with domain " + acceptDomains + ".");
                }
            }
            catch
            {
                return new ValidationResult("The value must be email.");
            }
        }
    }
}
