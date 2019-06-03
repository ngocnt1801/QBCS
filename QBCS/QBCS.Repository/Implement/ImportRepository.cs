using QBCS.Entity;
using QBCS.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Repository.Implement
{
    public class ImportRepository : Repository<Import>, IImportRepository 
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["QBCSContext"].ConnectionString;
        private readonly bool NOT_SEEN = false;

        public ImportRepository(DbContext context) : base(context)
        {

        }

        public int GetNotifyImportResult(int userId)
        {
            int count = 0;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(Id) AS 'Count' FROM [dbo].[Import] Where UserId=@userId AND Seen=@seen";
                using (var command = new SqlCommand(query, connection))
                {

                    command.Notification = null;
                    var dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(ImportStatus_OnChange);

                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@seen", NOT_SEEN);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        count = (int)reader["Count"];
                    }

                }
            }

            return count;
        }

        private void ImportStatus_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                //CustomerHub.SendMessages();
            }
        }
    }
}
