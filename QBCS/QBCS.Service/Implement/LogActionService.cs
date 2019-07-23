using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.Utilities;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                UserCode = model.UserCode,
                Ip = model.Ip
            };

            unitOfWork.Repository<LogAction>().Insert(entity);
            unitOfWork.SaveChanges();
        }
        public GetLogActionViewModel GetLogAction(string search, int start, int length)
        {
            //comment
            DateUltilities dateProcess = new DateUltilities();
            var result = new GetLogActionViewModel();
            var listLog = unitOfWork.Repository<LogAction>()
                .GetAll()
                .OrderByDescending(l => l.Date.Value);
            result.totalCount = listLog.Count();
            var query = listLog
                .Where(a => a.Action.Contains(search) || ((DateTime)a.Date).ToString().Contains(search) || a.Ip.Contains(search) || a.Fullname.Contains(search));
            result.filteredCount = query.Count();
            var logs = query.Skip(start).Take(length).ToList();
            result.Logs = logs
                .Select(l => new LogViewModel()
                {
                    Id = l.Id,
                    UserId = l.UserId.HasValue ? l.UserId.Value : 0,
                    TargetId = l.TargetId.HasValue ? l.TargetId.Value : 0,
                    TargetName = l.TargetName,
                    Fullname = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Fullname : l.Fullname,
                    UserCode = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Code : l.UserCode,
                    Route = l.Controller + "/" + l.Action,
                    Message = l.Message,
                    LogDate = l.Date.Value,
                    Ip = l.Ip,
                    Method = l.Method,
                    TimeAgo = dateProcess.TimeAgo(l.Date.Value)

                })
                .ToList();
            
            return result;
        }
    }
}
