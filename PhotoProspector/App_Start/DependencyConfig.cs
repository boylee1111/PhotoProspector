using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using PhotoProspector.Services;

namespace PhotoProspector.App_Start
{
    public class DependencyConfig
    {
        public static void RegisterComponents()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(typeof(MvcApplication).Assembly)
                .PropertiesAutowired();
            //builder.Register(x => new UnitOfWork()).As<IUnitOfWork>().InstancePerRequest();
            //builder.RegisterType<PersonRepository>().As<IPersonRepository>().InstancePerRequest();
            builder.RegisterType<ImageService>().As<IImageService>().SingleInstance();
            builder.RegisterType<FileService>().As<IFileService>().SingleInstance();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
