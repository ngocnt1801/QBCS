
using Newtonsoft.Json;
using QBCS.Entity;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
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

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userId = ((UserViewModel)HttpContext.Current.Session["user"]).Id;
            int? targetId = null;
            object oldValue = "";
            string jsonOldValue = "";
            string jsonNewValue = "";
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
                oldValue = questionService.GetQuestionById(newQues.Id);

            }
            //else if (Action.Equals("Import"))
            //{
            //    targetId = 
            //}

            QuestionViewModel questionViewModel = new QuestionViewModel();
           
            jsonOldValue = JsonConvert.SerializeObject(oldValue);
            if (newQues.QuestionContent != "")
            {
                jsonNewValue = JsonConvert.SerializeObject(newQues);
            }
            
            ILogService logger = new LogService();
            logger.Log(new LogViewModel
            {
                UserId = userId,
                LogDate = DateTime.Now,
                Message = Message,
                Action = Action,
                TargetName = TargetName,
                Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                Method = filterContext.ActionDescriptor.ActionName,
                TargetId = targetId,
                OldValue = jsonOldValue,
                NewValue = jsonNewValue
            });

        }


    }
}