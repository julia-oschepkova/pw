using System.Reflection;
using Autofac;
using PwTransferApp.Models.Identity;

namespace PwTransferApp.ContainerConfig
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("Common"))
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType(typeof (UserRepository)).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}