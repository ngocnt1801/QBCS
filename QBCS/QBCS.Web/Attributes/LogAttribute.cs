
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

        private ILogService logService;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            logService = new LogService();

            LogViewModel logModel = new LogViewModel();

            var user = (UserViewModel)HttpContext.Current.Session["user"];
            int userId = user != null ? user.Id : 0;

            int? targetId = null;
            QuestionViewModel oldValue = new QuestionViewModel();
            QuestionViewModel newQues = new QuestionViewModel();
            IQuestionService questionService = new QuestionService();
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
                    newQues.QuestionCode = oldValue.QuestionCode;
                    newQues.QuestionContent = WebUtility.HtmlDecode(newQues.QuestionContent);
                    for (int i = 0; i < newQues.Options.Count; i++)
                    {
                        newQues.Options[i].OptionContent = WebUtility.HtmlDecode(newQues.Options[i].OptionContent);
                    }
                    logModel.NewValue = JsonConvert.SerializeObject(newQues);
                }
            }

            logModel.UserId = userId;
            logModel.TargetId = targetId;
            logModel.Action = Action;
            logModel.TargetName = TargetName;
            logModel.LogDate = DateTime.Now;
            logModel.Message = Message;
            logModel.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            logModel.Method = filterContext.ActionDescriptor.ActionName;

            logService.Log(logModel);

        }

    }
    
}