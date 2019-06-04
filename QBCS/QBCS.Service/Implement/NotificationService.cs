using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System.Collections.Generic;
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
        public List<NotificationViewModel> GetNotifyImportResult(int userId)
        {
            var notificationList = unitOfWork.Repository<Import>().GetAll().Where(im => im.UserId == userId && im.Seen.HasValue && !im.Seen.Value)
                                                                            .Select(im => new NotificationViewModel {
                                                                                ImportId = im.Id,
                                                                                Message = "Your import questions have already checked, click to see result!"
                                                                            })
                                                                            .ToList();
            return notificationList;
        }

        public void RegisterNotification(OnChangeEventHandler eventHandler)
        {
            unitOfWork.ImportRepository().RegisterNotificationImportResult(eventHandler);
        }
    }
}
