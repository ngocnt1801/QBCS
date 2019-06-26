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
using Newtonsoft.Json;

namespace QBCS.Service.Implement
{
    public class LogService : ILogService
    {
        private IUnitOfWork unitOfWork;

        public LogService()
        {
            unitOfWork = new UnitOfWork();
        }

        public IEnumerable<LogViewModel> GetAllActivities()
        {
            var listLog = unitOfWork.Repository<Log>().GetAll().OrderByDescending(l => l.Date).ToList();

            return listLog.Select(l => new LogViewModel()
            {
                Id = l.Id,
                TargetId = l.TargetId,
                Fullname = unitOfWork.Repository<User>().GetById(l.UserId.Value).Fullname,
                Message = (l.Action + " " + l.TargetName).ToLowerInvariant(),
                LogDate = l.Date.Value

            });
        }
        public List<LogViewModel> GetAllActivitiesByTargetId(int targetId)
        {
            List<LogViewModel> list = new List<LogViewModel>();
            List<Log> listLog = unitOfWork.Repository<Log>().GetAll().Where(t => t.TargetId == targetId).ToList(); ;
            foreach (var item in listLog)
            {
                LogViewModel logViewModel = new LogViewModel()
                {
                    Id = item.Id,
                    TargetId = item.TargetId,
                    Fullname = unitOfWork.Repository<User>().GetById(item.UserId.Value).Fullname,
                    Message = (item.Action + " " + item.TargetName).ToLowerInvariant(),
                    LogDate = item.Date.Value
                };
                list.Add(logViewModel);
            }
            return list;
        }
    public IEnumerable<LogViewModel> GetActivitiesById(int id)
    {
        var logById = unitOfWork.Repository<Log>().GetById(id);

        List<LogViewModel> list = new List<LogViewModel>();
        Question oldValue = JsonConvert.DeserializeObject<Question>(logById.OldValue);
        Question newValue = JsonConvert.DeserializeObject<Question>(logById.NewValue);
        QuestionViewModel questionViewModelOld = new QuestionViewModel();
        QuestionViewModel questionViewModelNew = new QuestionViewModel();
        questionViewModelOld = ParseEntityToModel(oldValue);
        questionViewModelNew = ParseEntityToModel(newValue);



        LogViewModel model = new LogViewModel()
        {
            TargetId = logById.TargetId,
            Fullname = unitOfWork.Repository<User>().GetById(logById.UserId.Value).Fullname,
            Message = (logById.Action + " " + logById.TargetName).ToLowerInvariant(),
            LogDate = logById.Date.Value,
            OldValue = questionViewModelOld.ToString(),
            NewValue = questionViewModelNew.ToString(),
            QuestionOld = questionViewModelOld,
            QuestionNew = questionViewModelNew

        };

        list.Add(model);
        return list;

    }

    public QuestionViewModel ParseEntityToModel(Question question)
    {

        List<OptionViewModel> optionViewModels = new List<OptionViewModel>();
        foreach (var option in question.Options)
        {

            OptionViewModel optionViewModel = new OptionViewModel()
            {
                Id = option.Id,
                OptionContent = option.OptionContent,
                IsCorrect = (bool)option.IsCorrect
            };
            optionViewModels.Add(optionViewModel);
        }
        QuestionViewModel questionViewModel = new ViewModel.QuestionViewModel()
        {
            Id = question.Id,
            QuestionContent = question.QuestionContent,
            Options = optionViewModels
        };
        if (question.CourseId != null)
        {
            questionViewModel.CourseId = (int)question.CourseId;
        }
        if (question.LevelId != null)
        {
            questionViewModel.LevelId = (int)question.LevelId;
        }
        if (question.LearningOutcomeId != null)
        {
            questionViewModel.LearningOutcomeId = (int)question.LearningOutcomeId;
        }

        return questionViewModel;
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

