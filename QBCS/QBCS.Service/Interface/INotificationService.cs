using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{

    public interface INotificationService
    {
        List<NotificationViewModel> GetNotifyImportResult(int userId);
        void RegisterNotification(OnChangeEventHandler eventHandler);
        void MarkAllAsRead(int userId);
    }
}
