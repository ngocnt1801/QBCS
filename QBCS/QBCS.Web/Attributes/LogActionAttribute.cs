using System;
using System.Collections.Generic;
using System.Linq;
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
        public string LocParamName { get; set; }
        public string LevelParamName { get; set; }
        public string CateParamName { get; set; }
        public string CourseParamName { get; set; }
        private ILogActionService logActionService;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            logActionService = new LogActionService();
            LogViewModel logModel = new LogViewModel();
            int? targetId = null;
            int? cateId = null;
            int? locId = null;
            int? courseId = null;
            int? levelId = null;
            string targetName = "";
            QuestionTempViewModel newQuestionModel = new QuestionTempViewModel();
            string newQuestion = "";

            var user = (UserViewModel)HttpContext.Current.Session["user"];
            int? userId = null;
            if (user != null)
            {
                userId = user.Id;
            }
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
            if (CateParamName != null && filterContext.ActionParameters.ContainsKey(CateParamName))
            {
                cateId = filterContext.ActionParameters[CateParamName] as Int32?;   
            }
            if (LocParamName != null && filterContext.ActionParameters.ContainsKey(LocParamName))
            {
                locId = filterContext.ActionParameters[LocParamName] as Int32?;
            }
            if (LevelParamName != null && filterContext.ActionParameters.ContainsKey(LevelParamName))
            {
                levelId = filterContext.ActionParameters[LevelParamName] as Int32?;
            }
            if (CourseParamName != null && filterContext.ActionParameters.ContainsKey(CourseParamName))
            {
                courseId = filterContext.ActionParameters[CourseParamName] as Int32?;
            }
            if (ObjectParamName != null && filterContext.ActionParameters.ContainsKey(ObjectParamName))
            {
                newQuestionModel = filterContext.ActionParameters[ObjectParamName] as QuestionTempViewModel;
                logModel.NewValue = JsonConvert.SerializeObject(newQuestionModel);
            }
            ILogActionService logger = new LogActionService();
            if (Action != null)
            {
                
                logModel.UserId = userId;
                logModel.Action = Action;
                logModel.TargetId = targetId;
                logModel.TargetName = targetName != null ? targetName.ToString() : "";
                logModel.LogDate = DateTime.Now;
                logModel.Message = Message != null ? Message.ToString() : "";
                logModel.Controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                logModel.Method = filterContext.ActionDescriptor.ActionName;
                logModel.Fullname = Fullname;
                logModel.UserCode = UserCode;
                logModel.CategoryId = (int)cateId;
                logModel.CourseId = (int)courseId;
                logModel.LearningOutcomeId = (int)locId;
                logModel.LevelId = (int)levelId;
                logger.LogAction(logModel);
            }
        }
    }
}