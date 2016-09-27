using Autofac;
using Common.Managers;
using PwTransferApp.Models.Identity;
using PwTransferApp.Providers;

namespace PwTransferApp.ContainerConfig
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof (TransferClientModelProvider)).AsSelf().InstancePerLifetimeScope();
            builder.RegisterType(typeof (RegistrationManager))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType(typeof (UserSearcher)).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType(typeof (TransferManager)).AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}