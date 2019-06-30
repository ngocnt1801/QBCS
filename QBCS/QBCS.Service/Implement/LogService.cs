using Newtonsoft.Json;
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
using QBCS.Service.Enum;
using System.Net;

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
            //comment
            var listLog = unitOfWork.Repository<Log>().GetAll().OrderByDescending(l => l.Date).ToList();
          
            return listLog.Select(l => new LogViewModel()
            {
                Id = l.Id,
                UserId = (int)l.UserId,
                TargetId = l.TargetId,
                Fullname = unitOfWork.Repository<User>().GetById(l.UserId.Value).Fullname,
                Action = l.Action,
                Message = (l.Action + " " + l.TargetName).ToLowerInvariant(),
                LogDate = l.Date.Value

            });
        }
        public List<LogViewModel> GetListQuestionImportByTargetId(int targetId)
        {
            List<LogViewModel> list = new List<LogViewModel>();
            List<Log> listLog = unitOfWork.Repository<Log>().GetAll().Where(t => t.TargetId == targetId).OrderByDescending(t => t.Date).ToList();
            QuestionViewModel questionViewModel = new QuestionViewModel();
            List<QuestionViewModel> listTmp = new List<QuestionViewModel>();
            string ownerName = "";
            foreach (var item in listLog)
            {
                var import = unitOfWork.Repository<Import>().GetById(targetId);
                if (import.OwnerName != null)
                {
                    ownerName = import.OwnerName;
                }
               
                foreach (var itemQues in import.Questions)
                {
                    questionViewModel = ParseEntityToModel(itemQues);
                    if (questionViewModel != null)
                    {
                        listTmp.Add(questionViewModel);
                    }
                }
                if (listTmp.Count > 0)
                {
                    LogViewModel logViewModel = new LogViewModel()
                    {
                        Id = item.Id,
                        UserId = (int)item.UserId,
                        TargetId = item.TargetId,
                        Fullname = unitOfWork.Repository<User>().GetById(item.UserId.Value).Fullname,
                        Message = (item.Action + " " + item.TargetName).ToLowerInvariant(),
                        LogDate = item.Date.Value,
                        OwnerName = ownerName,
                        listQuestion = listTmp.ToList()
                    };
                    list.Add(logViewModel);
                }

            }
            return list;
        }
        public List<LogViewModel> GetAllActivitiesByTargetId(int targetId)
        {
            List<LogViewModel> list = new List<LogViewModel>();
            List<Log> listLog = unitOfWork.Repository<Log>().GetAll().Where(t => t.TargetId == targetId).OrderByDescending(t => t.Date).ToList(); ;
            foreach (var item in listLog)
            {
                LogViewModel logViewModel = new LogViewModel()
                {
                    Id = item.Id,
                    UserId = (int)item.UserId,
                    TargetId = item.TargetId,
                    Fullname = unitOfWork.Repository<User>().GetById(item.UserId.Value).Fullname,
                    Message = (item.Action + " " + item.TargetName).ToLowerInvariant(),
                    LogDate = item.Date.Value
                };
                list.Add(logViewModel);
            }
            return list;
        }
        public List<LogViewModel> GetAllActivitiesByUserId(int id, UserViewModel user)
        {
            List<LogViewModel> list = new List<LogViewModel>();
            List<Log> listLog = unitOfWork.Repository<Log>().GetAll().Where(t => t.UserId == id).OrderByDescending(t => t.Date).ToList(); ;
            string role = "";
            if (user.Role == RoleEnum.Lecturer)
            {
                role = "Lecturer";
            }
           
            foreach (var item in listLog)
            {
                string tempId = JsonConvert.DeserializeObject<Question>(item.NewValue).QuestionCode;
                LogViewModel logViewModel = new LogViewModel()
                {
                    Id = item.Id,
                    UserId = (int)item.UserId,   
                    UserRole = role,
                    TargetId = item.TargetId,
                    Fullname = unitOfWork.Repository<User>().GetById(item.UserId.Value).Fullname,
                    Action = item.Action,
                    Message = (item.Action + " " + item.TargetName + " " + tempId).ToLowerInvariant(),
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
            if (oldValue != null)
            {
                questionViewModelOld = ParseEntityToModel(oldValue);
            }
            if (newValue.QuestionContent != null && newValue.Options != null)
            {
                questionViewModelNew = ParseEntityToModel(newValue);
                
            }
            
            LogViewModel model = new LogViewModel()
            {
                TargetId = logById.TargetId,
                UserId = (int)logById.UserId,
                Fullname = unitOfWork.Repository<User>().GetById(logById.UserId.Value).Fullname,
                Message = (logById.Action + " " + logById.TargetName).ToLowerInvariant(),
                LogDate = logById.Date.Value,
                //OldValue = questionViewModelOld.ToString(),
                //NewValue = questionViewModelNew.ToString(),
             
                QuestionOld = questionViewModelOld,
                QuestionNew = questionViewModelNew

            };

            list.Add(model);
            return list;

        }

        public QuestionViewModel ParseEntityToModel(Question question)
        {

            List<OptionViewModel> optionViewModels = new List<OptionViewModel>();
            if (question.Options != null)
            {
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
            }
          
            QuestionViewModel questionViewModel = new ViewModel.QuestionViewModel()
            {
                
                QuestionCode = unitOfWork.Repository<Question>().GetById(question.Id).QuestionCode,
                Id = question.Id,
                QuestionContent = WebUtility.HtmlDecode(question.QuestionContent),
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

        public void LogImport(int importId, int userId)
        {
            LogViewModel model = new LogViewModel
            {
                TargetId = importId,
                LogDate = DateTime.Now,
                TargetName = "Question",
                Action = "Import",
                Controller = "Question",
                Method = "ImportFile"
            };
            Log(model);
        }
    }
}

