using System.Configuration;
using PhotoProspector.Helpers;

namespace PhotoProspector.Services
{
    public interface IOneDrivePhotoService
    {
        string GenerateOneDrivePersonalAuthUrl(string redirectUrl);
        string GenerateOneDriveForBizAuthUrl(string redirectUrl);
    }

    class OneDrivePhotoService : IOneDrivePhotoService
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string GenerateOneDriveForBizAuthUrl(string redirectUrl)
        {
            QueryStringBuilder builder = new QueryStringBuilder();
            builder.Add("client_id", ConfigurationManager.AppSettings["pi:AADAppId"]);
            builder.Add("response_type", "code");
            builder.Add("redirect_uri", redirectUrl);
            builder.Add("state", "business");

            return ConfigurationManager.AppSettings["pi:AADAuthService"] + builder.ToString();
        }

        public string GenerateOneDrivePersonalAuthUrl(string redirectUrl)
        {
            QueryStringBuilder builder = new QueryStringBuilder();
            logger.Info("Building URL String start");
            builder.Add("client_id", ConfigurationManager.AppSettings["pi:MSAAppId"]);
            logger.Info("MSAAppID is: " + ConfigurationManager.AppSettings["pi:MSAAppId"]);
            builder.Add("response_type", "code");
            logger.Info("response_type is: code.");
            builder.Add("redirect_uri", redirectUrl);
            logger.Info("redirect_uri is: " + redirectUrl);
            builder.Add("state", "personal");
            logger.Info("state is: personal");
            builder.Add("scope", ConfigurationManager.AppSettings["pi:MSAScopes"]);
            logger.Info("scope is: " + ConfigurationManager.AppSettings["pi:MSAScopes"]);

            return ConfigurationManager.AppSettings["pi:MSAAuthService"] + builder.ToString();
        }
    }
}
