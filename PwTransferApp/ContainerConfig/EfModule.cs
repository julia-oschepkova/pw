using Autofac;
using Common.DbContext;
using PwTransferApp.Models.Identity;

namespace PwTransferApp.ContainerConfig
{
    public class EfModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof (IdentityDbContextProvider))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
            builder.RegisterType(typeof (DbContextProvider))
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}