
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
            if (IdParamName != null && filterContext.ActionParameters.ContainsKey(IdParamName))
            {
                targetId = filterContext.ActionParameters[IdParamName] as Int32?;
            }
            else if (ObjectParamName != null && filterContext.ActionParameters.ContainsKey(ObjectParamName))
            {
                var obj = filterContext.ActionParameters[ObjectParamName];
                targetId = obj.GetType().GetProperty(IdParamName).GetValue(obj, null) as Int32?;
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
                TargetId = targetId
            });

        }


    }
}