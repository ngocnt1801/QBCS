using Microsoft.Owin;
using Owin;
using System.Configuration;

[assembly: OwinStartup(typeof(QBCS.Web.Startup))]

namespace QBCS.Web
{
    public class Startup
    {
        string connectionString = ConfigurationManager.ConnectionStrings["QBCSContext"].ConnectionString;
        public void Configuration(IAppBuilder app)
        {
            System.Data.SqlClient.SqlDependency.Start(connectionString);
            app.MapSignalR();
            app.UseStpmAuthentication(ConfigurationManager.AppSettings);
            app.UseStpmModuleManager(
                assemblies: new[] {
                    typeof(Controllers.HomeController).Assembly
                },
                appData: ConfigurationManager.AppSettings
            );
            app.UseStpmApiCaller(ConfigurationManager.AppSettings);
            app.UseStpmSidebar(appData: ConfigurationManager.AppSettings);
        }
    }
}
