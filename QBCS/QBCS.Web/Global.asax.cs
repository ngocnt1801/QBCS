using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QBCS.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        string connectionString = ConfigurationManager.ConnectionStrings["QBCSContext"].ConnectionString;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SqlDependency.Start(connectionString);
        }

        protected void Application_End()
        {
            SqlDependency.Stop(connectionString);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            if (exception != null)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(exception);
                HttpContext.Current.ClearError();
                Response.Redirect("/Error", false);
            }
        }
    }
}
