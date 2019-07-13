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
    public class LogActionService : ILogActionService
    {
        private IUnitOfWork unitOfWork;
        public LogActionService()
        {
            unitOfWork = new UnitOfWork();
        }
        public void LogAction(LogViewModel model)
        {
            LogAction entity = new LogAction
            {
                Message = model.Message,
                Date = model.LogDate,
                UserId = model.UserId,
                Action = model.Action,         
                Controller = model.Controller,
                Method = model.Method,
                Status = model.Status,
                OldValue = model.OldValue,
                NewValue = model.NewValue,
                TargetId = model.TargetId,
                Fullname = model.Fullname,
                UserCode = model.UserCode
            };

            unitOfWork.Repository<LogAction>().Insert(entity);
            unitOfWork.SaveChanges();
        }
    }
}
