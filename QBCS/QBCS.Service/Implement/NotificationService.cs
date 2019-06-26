using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Enum;
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
                                                                                Message = im.Status == (int)StatusEnum.Checked ? "Your import questions have already checked, click to see result!" : im.TotalSuccess + " questions were added to bank successfully.",
                                                                                Status = im.Status.Value,
                                                                                UpdatedDate = im.UpdatedDate.HasValue ? im.UpdatedDate.Value.ToString() : ""
                                                                            })
                                                                            .OrderByDescending(im => im.UpdatedDate)
                                                                            .ToList();
            return notificationList;
        }

        public void MarkAllAsRead(int userId)
        {
            var notificationList = unitOfWork.Repository<Import>().GetAll().Where(n => !n.Seen.HasValue || !n.Seen.Value).ToList();
            foreach (var noti in notificationList)
            {
                noti.Seen = true;
                unitOfWork.Repository<Import>().Update(noti);
            }

            unitOfWork.SaveChanges();
        }

        public void RegisterNotification(OnChangeEventHandler eventHandler)
        {
            unitOfWork.ImportRepository().RegisterNotificationImportResult(eventHandler);
        }
    }
}
