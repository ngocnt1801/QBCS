using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{
    public class NotificationService : INotificationService
    {
        private IUnitOfWork unitOfWork;

        public NotificationService()
        {
            unitOfWork = new UnitOfWork();
        }
        public int GetNotifyImportResult(int userId)
        {
            int count = 0;
            count = unitOfWork.ImportRepository().GetNotifyImportResult(userId);
            return count;
        }
    }
}
