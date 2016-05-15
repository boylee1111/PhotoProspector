using System.Web;
using Autofac;

namespace PhotoProspector.Helpers
{
    public static class PhotoConstants
    {
        public const string cContainer = "container";
        public const string cInvitationCode = "1234";

        public static IContainer IoCContainer
        {
            get
            {
                return HttpContext.Current.Application[cContainer] as IContainer;
            }

            set
            {
                HttpContext.Current.Application[cContainer] = value;
            }
        }
    }
}
