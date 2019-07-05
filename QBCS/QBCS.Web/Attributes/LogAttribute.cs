
using Newtonsoft.Json;
using QBCS.Entity;
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
            int userId = user != null ? user.Id : 0;

            int? targetId = null;
            QuestionViewModel oldQuestionModel = new QuestionViewModel();
            QuestionViewModel newQuestionModel = new QuestionViewModel();
            IQuestionService questionService = new QuestionService();
          
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
                    //newQues.LearningOutcomeId = oldValue.LearningOutcomeId;
                    //newQues.LevelId = oldValue.LevelId;
                    newQuestionModel.QuestionContent = WebUtility.HtmlDecode(newQuestionModel.QuestionContent);
                    for (int i = 0; i < newQuestionModel.Options.Count; i++)
                    {
                        newQuestionModel.Options[i].OptionContent = WebUtility.HtmlDecode(newQuestionModel.Options[i].OptionContent);
                    }
                    logModel.NewValue = JsonConvert.SerializeObject(newQuestionModel);
                }
            }
            else
            {
                logModel.OldValue = OldValue;
                logModel.NewValue = NewValue;
            }

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

            logService.Log(logModel);

        }

    }
    
}