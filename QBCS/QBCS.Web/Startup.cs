using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using QBCS.Service.Implement;
using QBCS.Web.SignalRHub;

[assembly: OwinStartup(typeof(QBCS.Web.Startup))]

namespace QBCS.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            app.UseStpmAuthentication(ConfigurationManager.AppSettings);
            app.UseStpmModuleManager(
                assemblies: new[] {
                    typeof(Controllers.HomeController).Assembly
                },
                appData: ConfigurationManager.AppSettings
            );
            app.UseStpmApiCaller(ConfigurationManager.AppSettings);
        }
    }
}
