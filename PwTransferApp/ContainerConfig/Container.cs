using System;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;

namespace PwTransferApp.ContainerConfig
{
    public static class Container
    {
        static Container()
        {
            Init();
        }

        public static IContainer NativeContainer { get; private set; }

        public static void Init()
        {
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            builder.RegisterModule(new RepositoryModule());
            builder.RegisterModule(new EfModule());
            builder.RegisterModule(new ApplicationModule());

            NativeContainer = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(NativeContainer);
        }

        public static T Resolve<T>()
        {
            return (T) Resolve(typeof (T));
        }

        public static object Resolve(Type type)
        {
            using (var lifetimeScope = NativeContainer.BeginLifetimeScope())
            {
                return lifetimeScope.Resolve(type);
            }
        }
    }
}