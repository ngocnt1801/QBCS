using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Configuration;
using System.Data.SqlClient;

namespace QBCS.Web.SignalRHub
{
    [HubName("importResultHub")]
    public class ImportResultHub : Hub
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["QBCSContext"].ToString();

        public void Hello()
        {
            Clients.All.hello();
        }

        [HubMethodName("sendNotification")]
        public static void SendNotification()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ImportResultHub>();
            
            context.Clients.All.updateNotification();

        }

        public static void ImportStatus_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SendNotification();
            }
        }

    }
}