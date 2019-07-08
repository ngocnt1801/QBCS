﻿
using Newtonsoft.Json;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Attributes
{

    public class LogAttribute : ActionFilterAttribute
    {
        public string Message { get; set; }
        public string Action { get; set; }
        public string TargetName { get; set; }
        public string ObjectParamName { get; set; }
        public string IdParamName { get; set; }
        public string LocParamName { get; set; }
        public string LevelParamName { get; set; }
        public string CateParamName { get; set; }
        public string Fullname { get; set; }
        public string UserCode { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }

        private ILogService logService;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            logService = new LogService();

            LogViewModel logModel = new LogViewModel();

            var user = (UserViewModel)HttpContext.Current.Session["user"];
            int? userId = null;
            if (user != null)
            {
                userId = user.Id;
            }

            ILogService logger = new LogService();
            CourseService courseService = new CourseService();
            LearningOutcomeService learningOutcomeService = new LearningOutcomeService();
            LevelService levelService = new LevelService();
            int? targetId = null;
            QuestionViewModel oldQuestionModel = new QuestionViewModel();
            QuestionViewModel newQuestionModel = new QuestionViewModel();
            IQuestionService questionService = new QuestionService();
            int[] ids;

            if (IdParamName != null && filterContext.ActionParameters.ContainsKey(IdParamName))
            {
                targetId = filterContext.ActionParameters[IdParamName] as Int32?;
            }
            else if (ObjectParamName != null && filterContext.ActionParameters.ContainsKey(ObjectParamName))
            {
                newQuestionModel = filterContext.ActionParameters[ObjectParamName] as QuestionViewModel;
                targetId = newQuestionModel.GetType().GetProperty(IdParamName).GetValue(newQuestionModel, null) as Int32?;
                if (Action.ToLower().Equals("update") && TargetName.ToLower().Equals("question"))
                {
                    oldQuestionModel = questionService.GetQuestionById(newQuestionModel.Id);
                }

            }

            if (Action.ToLower().Equals("update") && TargetName.ToLower().Equals("question"))
            {
                QuestionViewModel questionViewModel = new QuestionViewModel();
                oldQuestionModel.QuestionContent = WebUtility.HtmlDecode(oldQuestionModel.QuestionContent);
                for (int i = 0; i < oldQuestionModel.Options.Count; i++)
                {
                    oldQuestionModel.Options[i].OptionContent = WebUtility.HtmlDecode(oldQuestionModel.Options[i].OptionContent);
                }
                logModel.OldValue = JsonConvert.SerializeObject(oldQuestionModel);

                if (newQuestionModel.QuestionContent != "")
                {
                    newQuestionModel.QuestionCode = oldQuestionModel.QuestionCode != null ? oldQuestionModel.QuestionCode.ToString() : "";
                    newQuestionModel.CourseId = oldQuestionModel.CourseId;
                    newQuestionModel.Image = oldQuestionModel.Image;
                    newQuestionModel.CourseName = oldQuestionModel.CourseName;
                    newQuestionModel.LearningOutcomeName = oldQuestionModel.LearningOutcomeName;
                    newQuestionModel.LevelName = oldQuestionModel.LevelName;
                    newQuestionModel.QuestionContent = WebUtility.HtmlDecode(newQuestionModel.QuestionContent);
                    for (int i = 0; i < newQuestionModel.Options.Count; i++)
                    {
                        newQuestionModel.Options[i].OptionContent = WebUtility.HtmlDecode(newQuestionModel.Options[i].OptionContent);
                    }
                    logModel.NewValue = JsonConvert.SerializeObject(newQuestionModel);
                }
            }
            else if (Action.ToLower().Equals("move") && TargetName.ToLower().Equals("question"))
            {

                int categoryId = 0;
                int learningOutComeId = 0;
                int levelId = 0;
                CourseViewModel courseView = new CourseViewModel();
                LearningOutcomeViewModel learningOutcomeViewModel = new LearningOutcomeViewModel();
                LevelViewModel levelViewModel = new LevelViewModel();

                if (filterContext.ActionParameters.ContainsKey(IdParamName))
                {
                    ids = filterContext.ActionParameters[IdParamName] as int[];
                    categoryId = (int)filterContext.ActionParameters[CateParamName];
                    learningOutComeId = (int)filterContext.ActionParameters[LocParamName];
                    levelId = (int)filterContext.ActionParameters[LevelParamName];
                    if (ids != null)
                    {
                        foreach (var item in ids)
                        {
                            if (item != 0)
                            {
                                targetId = item;
                                oldQuestionModel = questionService.GetQuestionById((int)targetId);
                                newQuestionModel.LearningOutcomeId = learningOutComeId;
                                newQuestionModel.LevelId = levelId;
                                newQuestionModel.CategoryId = categoryId;
                                newQuestionModel.CourseId = oldQuestionModel.CourseId;

                                courseView = courseService.GetCourseById(oldQuestionModel.CourseId);
                                newQuestionModel.CourseName = courseView.Name;
                                learningOutcomeViewModel = learningOutcomeService.GetLearingOutcomeById(learningOutComeId);
                                newQuestionModel.LearningOutcomeName = learningOutcomeViewModel.Name;
                                levelViewModel = levelService.GetLevelById(levelId);
                                newQuestionModel.LevelName = levelViewModel.Name;
                                //newQues.CourseName = oldValue.CourseName;
                                //newQues.LearningOutcomeName = oldValue.LearningOutcomeName;
                                //newQues.LevelName = oldValue.LevelName;
                                newQuestionModel.QuestionContent = oldQuestionModel.QuestionContent;
                                newQuestionModel.Id = oldQuestionModel.Id;
                                newQuestionModel.QuestionCode = oldQuestionModel.QuestionCode;
                                newQuestionModel.Options = oldQuestionModel.Options;

                                #region log to db
                                logModel.OldValue = JsonConvert.SerializeObject(oldQuestionModel);
                                logModel.NewValue = JsonConvert.SerializeObject(newQuestionModel);
                                logModel.UserId = userId;
                                logModel.TargetId = targetId;
                                logModel.Action = Action;
                                logModel.TargetName = TargetName;
                                logModel.LogDate = DateTime.Now;
                                logModel.Message = Message;
                                logModel.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                                logModel.Method = filterContext.ActionDescriptor.ActionName;
                                logger.Log(logModel);
                                #endregion
                            }

                        }
                    }

                }
            }
            else
            {
                logModel.OldValue = OldValue;
                logModel.NewValue = NewValue;
                if (newQuestionModel != null && !String.IsNullOrWhiteSpace(NewValue))
                {
                    for (int i = 0; i < newQuestionModel.Options.Count; i++)
                    {
                        newQuestionModel.Options[i].OptionContent = WebUtility.HtmlDecode(newQuestionModel.Options[i].OptionContent);
                    }
                    logModel.NewValue = JsonConvert.SerializeObject(newQuestionModel);
                }

            }

            #region comment
            /*if (!Action.ToLower().Equals("move"))
            {
                logModel.UserId = userId;
                logModel.TargetId = targetId;
                logModel.Action = Action;
                logModel.TargetName = TargetName;
                logModel.LogDate = DateTime.Now;
                logModel.Message = Message;
                logModel.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                logModel.Method = filterContext.ActionDescriptor.ActionName;

            }
            else
            {
                logModel.UserId = userId;
                logModel.TargetId = targetId;
                logModel.Action = Action;
                logModel.TargetName = TargetName;
                logModel.LogDate = DateTime.Now;
                logModel.Message = Message;
                logModel.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                logModel.Method = filterContext.ActionDescriptor.ActionName;
                logModel.Fullname = Fullname;
                logModel.UserCode = UserCode;
            }*/
            #endregion

            logService.Log(logModel);
            logModel.UserId = userId;
            logModel.TargetId = targetId;
            logModel.Action = Action;
            logModel.TargetName = TargetName;
            logModel.LogDate = DateTime.Now;
            logModel.Message = Message;
            logModel.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            logModel.Method = filterContext.ActionDescriptor.ActionName;
            logModel.Fullname = Fullname;
            logModel.UserCode = UserCode;
            logger.Log(logModel);

        }

        private static LearningOutcomeViewModel GetLocView()
        {
            return new LearningOutcomeViewModel();
        }
    }

}