using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Exchange.WebServices.Data;

namespace PhotoProspector.Services
{
    public interface IMessageService
    {
        void SendInvitationCode(string recipient);
    }

    class MessageService : IMessageService
    {
        private readonly IUserService userService;

        public MessageService(IUserService userService)
        {
            this.userService = userService;
        }

        public void SendInvitationCode(string recipient)
        {
            string alias = recipient.Split('@')[0];
            string code = "";
            string sender = "photo@ericyi.msftonlinerepro.com";

            Random rand = new Random();
            code = rand.Next(1000, 9999).ToString();

#if !DEBUG
            userService.InsertInvitationCodeSQL(alias, code);
#endif
            try
            {
                ServicePointManager.ServerCertificateValidationCallback = CertificateValidationCallBack;
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013);
                service.Credentials = new WebCredentials("EWSAccount", "qwe123~");

                service.Url = new Uri("https://mail.ericyi.msftonlinerepro.com/EWS/Exchange.asmx");


                service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, sender);

                EmailMessage message = new EmailMessage(service);

                message.Subject = "Photo Inspector Invitation Code";
                string body = "Welcome to Photo Inspector Website, with this website you can know your colleagues better! Your Invitation Code is: " + code + ".";

                message.Body = body;

                message.ToRecipients.Add(recipient);

                message.Send();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static bool CertificateValidationCallBack(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            // If the certificate is a valid, signed certificate, return true.
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            // If there are errors in the certificate chain, look at each error to determine the cause.
            if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) != 0)
            {
                if (chain != null && chain.ChainStatus != null)
                {
                    foreach (X509ChainStatus status in chain.ChainStatus)
                    {
                        if ((certificate.Subject == certificate.Issuer) &&
                           (status.Status == X509ChainStatusFlags.UntrustedRoot))
                        {
                            // Self-signed certificates with an untrusted root are valid. 
                            continue;
                        }
                        else
                        {
                            if (status.Status != X509ChainStatusFlags.NoError)
                            {
                                // If there are any other errors in the certificate chain, the certificate is invalid,
                                // so the method returns false.
                                return false;
                            }
                        }
                    }
                }

                // When processing reaches this line, the only errors in the certificate chain are 
                // untrusted root errors for self-signed certificates. These certificates are valid
                // for default Exchange server installations, so return true.
                return true;
            }
            else
            {
                // In all other cases, return false.
                return false;
            }
        }
    }
}
