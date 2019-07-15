using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;

namespace QBCS.Web.Attributes
{
    public class LogActionAttribute : ActionFilterAttribute
    {
        public string Message { get; set; }
        public string Action { get; set; }
        public string TargetId { get; set; }
        public string TargetName { get; set; }
        public string ObjectParamName { get; set; }
        public string IdParamName { get; set; }
        public string Fullname { get; set; }
        public string UserCode { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string Method { get; set; }
      
        private ILogActionService logActionService;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            logActionService = new LogActionService();
            LogViewModel logModel = new LogViewModel();
            int? targetId = null;
            string method = "";
            string ip = "";
            string fullname = "";
            string userCode = "";
            string targetName = "";
            QuestionTempViewModel newQuestionModel = new QuestionTempViewModel();
          

            var user = (UserViewModel)HttpContext.Current.Session["user"];
            int? userId = null;
            if (user != null)
            {
                userId = user.Id;
                fullname = filterContext.HttpContext.User.Get(u => u.FullName);
                userCode = filterContext.HttpContext.User.Get(u => u.Code);
            }
            //ip = System.Web.HttpContext.Current.Request.UserHostAddress;
            string strHostName = "";
            strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            //ip = addr[3].ToString();


            if (IdParamName != null && filterContext.ActionParameters.ContainsKey(IdParamName))
            {
                targetId = filterContext.ActionParameters[IdParamName] as Int32?;
                targetName = "Question";

            }
            if (TargetId != null && filterContext.ActionParameters.ContainsKey(TargetId))
            {
                targetId = filterContext.ActionParameters[TargetId] as Int32?;
                targetName = "Import";
            }
           
           
            
            ILogActionService logger = new LogActionService();
            if (Action != null)
            {
                
                logModel.UserId = userId;
                logModel.TargetId = targetId;
                logModel.TargetName = targetName != null ? targetName.ToString() : "";
                logModel.LogDate = DateTime.Now;
                logModel.Message = Message != null ? Message.ToString() : "";
                logModel.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                logModel.Action = filterContext.ActionDescriptor.ActionName;
                logModel.Fullname = fullname;
                logModel.UserCode = userCode;
                logModel.Method = Method != null ? Method.ToString() : "";
                logModel.Ip = ip;
                logger.LogAction(logModel);
            }
        }
    }
}