
using Newtonsoft.Json;
using QBCS.Entity;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
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
      

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LogViewModel logModel = new LogViewModel();
            ILogService logger = new LogService();
            CourseService courseService = new CourseService();
            LearningOutcomeService learningOutcomeService = new LearningOutcomeService();
            LevelService levelService = new LevelService();
            int userId = ((UserViewModel)HttpContext.Current.Session["user"]).Id;
            int? targetId = null;
            QuestionViewModel oldValue = new QuestionViewModel();
            QuestionViewModel newQues = new QuestionViewModel();
            IQuestionService questionService = new QuestionService();
            int[] ids;

            if (IdParamName != null && filterContext.ActionParameters.ContainsKey(IdParamName))
            {
                targetId = filterContext.ActionParameters[IdParamName] as Int32?;
            }
            else if (ObjectParamName != null && filterContext.ActionParameters.ContainsKey(ObjectParamName))
            {
                newQues = filterContext.ActionParameters[ObjectParamName] as QuestionViewModel;
                targetId = newQues.GetType().GetProperty(IdParamName).GetValue(newQues, null) as Int32?;
                if (Action.ToLower().Equals("update") && TargetName.ToLower().Equals("question"))
                {
                    oldValue = questionService.GetQuestionById(newQues.Id);
                }
               

            }

            if (Action.ToLower().Equals("update") && TargetName.ToLower().Equals("question"))
            {
                QuestionViewModel questionViewModel = new QuestionViewModel();
                oldValue.QuestionContent = WebUtility.HtmlDecode(oldValue.QuestionContent);
                for (int i = 0; i < oldValue.Options.Count; i++)
                {
                    oldValue.Options[i].OptionContent = WebUtility.HtmlDecode(oldValue.Options[i].OptionContent);
                }
                logModel.OldValue = JsonConvert.SerializeObject(oldValue);

                if (newQues.QuestionContent != "")
                {
                    newQues.QuestionCode = oldValue.QuestionCode != null ? oldValue.QuestionCode.ToString() : "";
                    newQues.CourseId = oldValue.CourseId;
                    newQues.LearningOutcomeId = oldValue.LearningOutcomeId;
                    newQues.LevelId = oldValue.LevelId;
                    newQues.CourseName = oldValue.CourseName;
                    newQues.LearningOutcomeName = oldValue.LearningOutcomeName;
                    newQues.LevelName = oldValue.LevelName;
                    newQues.QuestionContent = WebUtility.HtmlDecode(newQues.QuestionContent);
                    for (int i = 0; i < newQues.Options.Count; i++)
                    {
                        newQues.Options[i].OptionContent = WebUtility.HtmlDecode(newQues.Options[i].OptionContent);
                    }
                    logModel.NewValue = JsonConvert.SerializeObject(newQues);
                }
                
            }
            if (Action.ToLower().Equals("move") && TargetName.ToLower().Equals("question"))
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
                                oldValue = questionService.GetQuestionById((int)targetId);
                                newQues.LearningOutcomeId = learningOutComeId;
                                newQues.LevelId = levelId;
                                newQues.CategoryId = categoryId;
                                newQues.CourseId = oldValue.CourseId;

                                courseView = courseService.GetCourseById(oldValue.CourseId);
                                newQues.CourseName = courseView.Name;
                                learningOutcomeViewModel = learningOutcomeService.GetLearingOutcomeById(learningOutComeId);
                                newQues.LearningOutcomeName = learningOutcomeViewModel.Name;
                                levelViewModel = levelService.GetLevelById(levelId);
                                newQues.LevelName = levelViewModel.Name;
                                //newQues.CourseName = oldValue.CourseName;
                                //newQues.LearningOutcomeName = oldValue.LearningOutcomeName;
                                //newQues.LevelName = oldValue.LevelName;
                                newQues.QuestionContent = oldValue.QuestionContent;
                                newQues.Id = oldValue.Id;
                                newQues.QuestionCode = oldValue.QuestionCode;
                                newQues.Options = oldValue.Options;
                                logModel.OldValue = JsonConvert.SerializeObject(oldValue);
                                logModel.NewValue = JsonConvert.SerializeObject(newQues);
                                logModel.UserId = userId;
                                logModel.TargetId = targetId;
                                logModel.Action = Action;
                                logModel.TargetName = TargetName;
                                logModel.LogDate = DateTime.Now;
                                logModel.Message = Message;
                                logModel.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                                logModel.Method = filterContext.ActionDescriptor.ActionName;
                                logger.Log(logModel);
                            }

                        }
                    }

                }




            }

            if (!Action.ToLower().Equals("move"))
            {
                logModel.UserId = userId;
                logModel.TargetId = targetId;
                logModel.Action = Action;
                logModel.TargetName = TargetName;
                logModel.LogDate = DateTime.Now;
                logModel.Message = Message;
                logModel.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                logModel.Method = filterContext.ActionDescriptor.ActionName;



                logger.Log(logModel);
            }
           
            //logger.Log(new LogViewModel
            //{
            //    UserId = userId,
            //    LogDate = DateTime.Now,
            //    Message = Message,
            //    Action = Action,
            //    TargetName = TargetName,
            //    Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
            //    Method = filterContext.ActionDescriptor.ActionName,
            //    TargetId = targetId,
            //    OldValue = jsonOldValue,
            //    NewValue = jsonNewValue
            //});

        }

        private static LearningOutcomeViewModel GetLocView()
        {
            return new LearningOutcomeViewModel();
        }
    }

}