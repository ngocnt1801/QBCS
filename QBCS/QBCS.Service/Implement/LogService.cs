﻿using Newtonsoft.Json;
using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public bool UpdateLogStatus(int importId)
        {
            bool check = false;
            var import = unitOfWork.Repository<Log>().GetAll().Where(t => t.TargetId == importId);
            foreach (var item in import)
            {
                if (item.Action.Equals("Import"))
                {
                    item.Status = (int)Enum.StatusEnum.Canceled;
                    unitOfWork.Repository<Log>().Update(item);
                }

            }

            return check;
        }

        public GetActivityViewModel GetAllActivities(string search, int start, int length)
        {
            //comment
            var result = new GetActivityViewModel();
            var listLog = unitOfWork.Repository<Log>()
                .GetAll();
            result.totalCount = listLog.Count();
            var searchResult = listLog.Where(a => a.Fullname.Contains(search) || 
            a.Action.Contains(search) || 
            (a.Date.Value.Day + "/" + a.Date.Value.Month + "/" + a.Date.Value.Year + " " + a.Date.Value.Hour + ":" + a.Date.Value.Minute).Contains(search));
            result.filteredCount = searchResult.Count();
            result.Logs = length >= 0 ? 
                searchResult.OrderByDescending(l => l.Date.Value).Skip(start).Take(length).ToList()
                .Select(l => new LogViewModel()
                {
                    Id = l.Id,
                    UserId = l.UserId.HasValue ? l.UserId.Value : 0,
                    TargetId = l.TargetId.HasValue ? l.TargetId.Value : 0,
                    TargetName = l.TargetName,
                    Fullname = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Fullname : l.Fullname,
                    UserCode = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Code : l.UserCode,
                    Action = l.Action,
                    Message = (l.Action + " " + l.TargetName),
                    LogDate = l.Date.Value

                }).ToList()
                :
                searchResult.OrderByDescending(l => l.Date.Value).ToList()
                .Select(l => new LogViewModel()
                {
                    Id = l.Id,
                    UserId = l.UserId.HasValue ? l.UserId.Value : 0,
                    TargetId = l.TargetId.HasValue ? l.TargetId.Value : 0,
                    TargetName = l.TargetName,
                    Fullname = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Fullname : l.Fullname,
                    UserCode = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Code : l.UserCode,
                    Action = l.Action,
                    Message = (l.Action + " " + l.TargetName),
                    LogDate = l.Date.Value

                }).ToList();
            return result;
        }
        public LogViewModel GetQuestionImportByTargetId(int targetId)
        {

            List<Log> listLog = unitOfWork.Repository<Log>().GetAll().Where(t => t.TargetId == targetId).OrderByDescending(t => t.Date).ToList();
            QuestionViewModel questionViewModel = new QuestionViewModel();
            List<QuestionViewModel> listTmp = new List<QuestionViewModel>();
            LogViewModel logViewModel = new LogViewModel();
            string ownerName = "";

            foreach (var item in listLog)
            {
                var import = unitOfWork.Repository<Import>().GetById(targetId);
                if (import.OwnerName != null)
                {
                    ownerName = import.OwnerName;
                }


                if (import.Status == (int)Enum.StatusEnum.Done)
                {
                    foreach (var itemQues in import.Questions)
                    {
                        questionViewModel = ParseEntityToModel(itemQues);
                        if (questionViewModel != null)
                        {
                            listTmp.Add(questionViewModel);
                        }
                    }
                }

                if (listTmp.Count > 0 && item != null)
                {
                    logViewModel = new LogViewModel()
                    {
                        Id = item.Id,
                        UserId = item.UserId.HasValue ? item.UserId.Value : 0,
                        Action = item.Action,
                        TargetId = item.TargetId.HasValue ? item.TargetId.Value : 0,
                        TargetName = item.TargetName,
                        Fullname = item.UserId.HasValue && item.UserId != 0 ? unitOfWork.Repository<User>().GetById(item.UserId.Value).Fullname : item.Fullname,
                        Message = (item.Action + " " + item.TargetName).ToLowerInvariant(),
                        LogDate = item.Date.Value,
                        OwnerName = ownerName,
                        listQuestion = listTmp.ToList()
                    };

                }

            }
            if (logViewModel.LogDate == Convert.ToDateTime("1/1/0001 12:00:00 AM"))
            {
                logViewModel.LogDate = new DateTime();
                
            }
            return logViewModel;
        }
        public List<LogViewModel> GetListQuestionImportByTargetId(int targetId)
        {
            List<LogViewModel> list = new List<LogViewModel>();
            List<Log> listLog = unitOfWork.Repository<Log>().GetAll().Where(t => t.TargetId == targetId).OrderByDescending(t => t.Date).ToList();
            QuestionViewModel questionViewModel = new QuestionViewModel();
            List<QuestionViewModel> listTmp = new List<QuestionViewModel>();
            string ownerName = "";
            string courseCode = "";
            int status = 0;
            foreach (var item in listLog)
            {
                var import = unitOfWork.Repository<Import>().GetById(targetId);
                if (import.OwnerName != null)
                {
                    ownerName = import.OwnerName;
                }
                if (import.CourseId != null)
                {
                    int courseId = (int)import.CourseId;
                    courseCode = unitOfWork.Repository<Course>().GetById(courseId).Code;
                }
                if (import.Status != null)
                {
                    status = (int)import.Status.Value;

                }

                foreach (var itemQues in import.Questions)
                {
                    questionViewModel = ParseEntityToModel(itemQues);
                    if (questionViewModel != null)
                    {
                        listTmp.Add(questionViewModel);
                    }
                }
                if (listTmp.Count > 0 && item != null)
                {
                    LogViewModel logViewModel = new LogViewModel()
                    {
                        Id = item.Id,
                        UserId = item.UserId.HasValue ? item.UserId.Value : 0,
                        TargetId = item.TargetId.HasValue ? item.TargetId.Value : 0,
                        Fullname = item.UserId.HasValue && item.UserId != 0 ? unitOfWork.Repository<User>().GetById(item.UserId.Value).Fullname : item.Fullname,
                        Message = (item.Action + " " + item.TargetName).ToLowerInvariant(),
                        LogDate = item.Date.Value,
                        OwnerName = ownerName,
                        Status = status,
                        CourseCode = courseCode != null ? courseCode.ToString() : "",
                        listQuestion = listTmp.ToList()
                    };
                    list.Add(logViewModel);
                }

            }
            return list;
        }
        public GetActivityViewModel GetAllActivitiesByTargetId(int targetId, string search, int start, int length)
        {
            var result = new GetActivityViewModel();
            var listLog = unitOfWork.Repository<Log>()
                .GetAll().Where(a => a.TargetId == targetId && !a.Action.Equals("Import"));
            result.totalCount = listLog.Count();
            var searchResult = listLog.Where(a => a.Fullname.Contains(search) ||
                        a.Action.Contains(search) ||
                        (a.Date.Value.Day + "/" + a.Date.Value.Month + "/" + a.Date.Value.Year + " " + a.Date.Value.Hour + ":" + a.Date.Value.Minute).Contains(search));
            result.filteredCount = searchResult.Count();
            result.Logs = length >= 0 ?
                searchResult.OrderByDescending(l => l.Date.Value).Skip(start).Take(length).ToList()
                .Select(l => new LogViewModel()
                {
                    Id = l.Id,
                    UserId = l.UserId.HasValue ? l.UserId.Value : 0,
                    TargetId = l.TargetId.HasValue ? l.TargetId.Value : 0,
                    TargetName = l.TargetName,
                    Fullname = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Fullname : l.Fullname,
                    UserCode = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Code : l.UserCode,
                    Action = l.Action,
                    Message = (l.Action + " " + l.TargetName),
                    LogDate = l.Date.Value

                }).ToList()
                :
                searchResult.OrderByDescending(l => l.Date.Value).ToList()
                .Select(l => new LogViewModel()
                {
                    Id = l.Id,
                    UserId = l.UserId.HasValue ? l.UserId.Value : 0,
                    TargetId = l.TargetId.HasValue ? l.TargetId.Value : 0,
                    TargetName = l.TargetName,
                    Fullname = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Fullname : l.Fullname,
                    UserCode = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Code : l.UserCode,
                    Action = l.Action,
                    Message = (l.Action + " " + l.TargetName),
                    LogDate = l.Date.Value

                }).ToList();

            return result;
        }

        public GetActivityViewModel GetAllActivitiesByUserId(int id, string search, int start, int length)
        {
            //List<LogViewModel> list = new List<LogViewModel>();
            //List<Log> listLog = unitOfWork.Repository<Log>().GetAll()
            //    .Where(t => t.UserId == id)
            //    .OrderByDescending(t => t.Date)
            //    .ToList();

            ////string role = "";
            ////if (user.Role == RoleEnum.Lecturer)
            ////{
            ////    role = "Lecturer";
            ////}

            //foreach (var item in listLog)
            //{
            //    string tempId = "";
            //    if (item.NewValue != null && (item.Action == "Update" || item.Action == "Import"))
            //    {
            //        var temp = JsonConvert.DeserializeObject<QuestionViewModel>(item.NewValue);
            //        tempId = temp.QuestionCode;
            //    }

            //    LogViewModel logViewModel = new LogViewModel()
            //    {
            //        Id = item.Id,
            //        UserId = item.UserId.HasValue ? item.UserId.Value : 0,
            //        //UserRole = role,
            //        TargetId = item.TargetId,
            //        TargetName = item.TargetName,
            //        Fullname = item.UserId.HasValue && item.UserId != 0 ? unitOfWork.Repository<User>().GetById(item.UserId.Value).Fullname : item.Fullname,
            //        Action = item.Action,
            //        Message = (item.Action + " " + item.TargetName + " " + tempId).ToLowerInvariant(),
            //        LogDate = item.Date.Value

            //    };
            //    list.Add(logViewModel);
            //}
            //return list;
            var result = new GetActivityViewModel();
            var listLog = unitOfWork.Repository<Log>()
                .GetAll().Where(a => a.UserId == id);
            result.totalCount = listLog.Count();
            var searchResult = listLog.Where(a => a.Fullname.Contains(search) ||
            a.Action.Contains(search) ||
            (a.Date.Value.Day + "/" + a.Date.Value.Month + "/" + a.Date.Value.Year + " " + a.Date.Value.Hour + ":" + a.Date.Value.Minute).Contains(search));
            result.filteredCount = searchResult.Count();
            result.Logs = length >= 0 ?
                searchResult.OrderByDescending(l => l.Date.Value).Skip(start).Take(length).ToList()
                .Select(l => new LogViewModel()
                {
                    Id = l.Id,
                    UserId = l.UserId.HasValue ? l.UserId.Value : 0,
                    TargetId = l.TargetId.HasValue ? l.TargetId.Value : 0,
                    TargetName = l.TargetName,
                    Fullname = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Fullname : l.Fullname,
                    UserCode = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Code : l.UserCode,
                    Action = l.Action,
                    Message = (l.Action + " " + l.TargetName),
                    LogDate = l.Date.Value

                }).ToList()
                :
                searchResult.OrderByDescending(l => l.Date.Value).ToList()
                .Select(l => new LogViewModel()
                {
                    Id = l.Id,
                    UserId = l.UserId.HasValue ? l.UserId.Value : 0,
                    TargetId = l.TargetId.HasValue ? l.TargetId.Value : 0,
                    TargetName = l.TargetName,
                    Fullname = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Fullname : l.Fullname,
                    UserCode = l.UserId.HasValue && l.UserId.Value != 0 ? unitOfWork.Repository<User>().GetById(l.UserId.Value).Code : l.UserCode,
                    Action = l.Action,
                    Message = (l.Action + " " + l.TargetName),
                    LogDate = l.Date.Value

                }).ToList();
            return result;
        }
        public IEnumerable<LogViewModel> GetActivitiesById(int id)
        {
            var logById = unitOfWork.Repository<Log>().GetById(id);

            List<LogViewModel> list = new List<LogViewModel>();
            QuestionViewModel oldValue = JsonConvert.DeserializeObject<QuestionViewModel>(logById.OldValue != null ? logById.OldValue.ToString() : "");
            QuestionViewModel newValue = JsonConvert.DeserializeObject<QuestionViewModel>(logById.NewValue != null ? logById.NewValue.ToString() : "");
            //QuestionViewModel questionViewModelOld = new QuestionViewModel();
            //QuestionViewModel questionViewModelNew = new QuestionViewModel();
            //if (oldValue != null && !oldValue.Equals(""))
            //{
            //    questionViewModelOld = ParseEntityToModel(oldValue);
            //}
            //if (newValue.QuestionContent != null && newValue.Options != null && !oldValue.Equals(""))
            //{
            //    questionViewModelNew = ParseEntityToModel(newValue);

            //}


            LogViewModel model = new LogViewModel()
            {
                TargetId = logById.TargetId,
                UserId = logById.UserId.HasValue ? logById.UserId.Value : 0,
                Fullname = logById.UserId.HasValue && logById.UserId != 0 ? unitOfWork.Repository<User>().GetById(logById.UserId.Value).Fullname : logById.Fullname,
                Message = (logById.Action + " " + logById.TargetName).ToLowerInvariant(),
                LogDate = logById.Date.Value,
                //OldValue = questionViewModelOld.ToString(),
                //NewValue = questionViewModelNew.ToString(),

                QuestionOld = oldValue,
                QuestionNew = newValue

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
                        OptionContent = WebUtility.HtmlDecode(option.OptionContent),
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
                Options = optionViewModels,

            };
            if (question.Image != null)
            {
                questionViewModel.Image = question.Image;
            }
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
            if (question.Course != null)
            {
                questionViewModel.CourseName = question.Course.Name;
            }
            if (question.LearningOutcome != null)
            {
                questionViewModel.LearningOutcomeName = question.LearningOutcome.Name;
            }
            if (question.Level != null)
            {
                questionViewModel.LevelName = question.Level.Name;
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
                Status = model.Status,
                OldValue = model.OldValue,
                NewValue = model.NewValue,
                TargetId = model.TargetId,
                Fullname = model.Fullname,
                UserCode = model.UserCode
            };

            unitOfWork.Repository<Log>().Insert(entity);
            unitOfWork.SaveChanges();
        }


        public void LogManually(string action, string targetName, int? targetId = null, int? userId = null, string controller = "", string method = "", string fullname = "", string usercode = "")
        {
            LogViewModel model = new LogViewModel
            {
                TargetId = targetId,
                UserId = userId,
                LogDate = DateTime.Now,
                TargetName = targetName,
                Action = action,
                Controller = controller,
                Method = method,
                Fullname = fullname,
                Status = (int)Enum.StatusEnum.Checked,
                UserCode = usercode
            };
            Log(model);
        }
        public void LogFullManually(string action, string targetName, int? targetId = null, int? userId = null, string controller = "", string method = "", 
            string fullname = "", string usercode = "", string newValue = "", string oldValue = "")
        {
            LogViewModel model = new LogViewModel
            {
                TargetId = targetId,
                UserId = userId,
                LogDate = DateTime.Now,
                TargetName = targetName,
                Action = action,
                Controller = controller,
                OldValue = oldValue,
                NewValue = newValue,
                Method = method,
                Fullname = fullname,
                Status = (int)Enum.StatusEnum.Checked,
                UserCode = usercode
            };
            Log(model);
        }
    }
}

