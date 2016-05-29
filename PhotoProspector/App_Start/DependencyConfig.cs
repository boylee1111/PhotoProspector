using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using PhotoProspector.Helpers;
using PhotoProspector.Services;

namespace PhotoProspector.App_Start
{
    public class DependencyConfig
    {
        public static void RegisterComponents()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly).PropertiesAutowired();
            builder.RegisterType<ImageService>().As<IImageService>().SingleInstance();
            builder.RegisterType<FileService>().As<IFileService>().SingleInstance();
            builder.RegisterType<UserService>().As<IUserService>().SingleInstance();
            builder.RegisterType<MessageService>().As<IMessageService>().SingleInstance();
            builder.RegisterType<TrainingService>().As<ITrainingService>().SingleInstance();
            builder.RegisterType<ScanningService>().As<IScanningService>().SingleInstance();
            builder.RegisterType<OneDrivePhotoService>().As<IOneDrivePhotoService>().SingleInstance();

            PhotoConstants.IoCContainer = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(PhotoConstants.IoCContainer));
        }
    }
}
