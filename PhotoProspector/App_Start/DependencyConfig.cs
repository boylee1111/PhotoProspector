using Autofac;
using Autofac.Integration.Mvc;
using System.Web.Mvc;

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

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
