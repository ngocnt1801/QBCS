using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using System.Data.SqlClient;
using System.Linq;

namespace QBCS.Service.Implement
{
    public class NotificationService : INotificationService
    {
        private IUnitOfWork unitOfWork;

        public NotificationService()
        {
            unitOfWork = new UnitOfWork();
        }
        public int GetNotifyImportResult(int userId, OnChangeEventHandler eventHandler)
        {
            int count = 0;
            count = unitOfWork.Repository<Import>().GetAll().Where(im => im.UserId == userId && im.Seen.HasValue && !im.Seen.Value).Count();
            return count;
        }

        public void RegisterNotification(OnChangeEventHandler eventHandler)
        {
            unitOfWork.ImportRepository().RegisterNotificationImportResult(eventHandler);
        }
    }
}
