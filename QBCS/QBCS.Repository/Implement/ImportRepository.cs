using QBCS.Entity;
using QBCS.Repository.Interface;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;

namespace QBCS.Repository.Implement
{
    public class ImportRepository : Repository<Import>, IImportRepository 
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QBCSContext"].ConnectionString;

        public ImportRepository(DbContext context) : base(context)
        {

        }

        public void RegisterNotificationImportResult(OnChangeEventHandler eventHandler)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Id, Status, Seen FROM [dbo].[Import]";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Notification = null;
                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(eventHandler);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }

        }
    }
}
