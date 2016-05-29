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
            builder.Add("client_id", ConfigurationManager.AppSettings["pi:MSAAppId"]);
            builder.Add("response_type", "code");
            builder.Add("redirect_uri", redirectUrl);
            builder.Add("state", "personal");
            builder.Add("scope", ConfigurationManager.AppSettings["pi:MSAScopes"]);

            return ConfigurationManager.AppSettings["pi:MSAAuthService"] + builder.ToString();
        }
    }
}
