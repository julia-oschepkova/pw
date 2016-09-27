using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof (PwTransferApp.Startup))]

namespace PwTransferApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}