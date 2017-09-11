using Autofac;
using Autofac.Integration.WebApi;
using RepositoryLayer.Infrastructure;
using RepositoryLayer.Repository;
using ServiceLayer;
using ServiceLayer.Interfaces;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Autopilot
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Setup the Container Builder
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();


            // Register dependencies in controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<DbFactory>().As< IDbFactory>().InstancePerRequest();      
            
              
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerRequest();


            //to register  a servies and their interfaces by diksha date-11/2017
            builder.RegisterType<UserService>().As<IUserService>().InstancePerRequest();
            builder.RegisterType<TwitterServices>().As<ITwitterServices>().InstancePerRequest();


            // Register your repositories all at once using assembly scanning  by diksha temporary trial 

            //builder.RegisterAssemblyTypes(typeof(UserService).Assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerRequest();
            //builder.RegisterAssemblyTypes(typeof(TwitterServices).Assembly).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerRequest();


            // Register your Web API controllers all at once using assembly scanning
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());


            // OPTIONAL: Register the Autofac filter provider.
            builder.RegisterWebApiFilterProvider(config);

            // OPTIONAL: Register the Autofac model binder provider.
            builder.RegisterWebApiModelBinderProvider();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}
