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
    public class LogService : ILogService
    {
        private IUnitOfWork unitOfWork;

        public LogService()
        {
            unitOfWork = new UnitOfWork();
        }

        public void Log(LogViewModel model)
        {
            Log entity = new Log
            {
                Message = model.Message,
                Date = model.LogDate,
                UserId = model.UserId,
                Action = model.Action,
                TargetName = model.TargetName,
                Controller = model.Controller,
                Method = model.Method,
                OldValue = model.OldValue,
                NewValue = model.NewValue,
                TargetId = model.TargetId
            };

            unitOfWork.Repository<Log>().Insert(entity);
            unitOfWork.SaveChanges();
        }
    }
}
