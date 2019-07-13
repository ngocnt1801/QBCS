using AuthLib.Module;
using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    [CheckSession]
    public class ActivityController : Controller
    {
        private ILogService logService;
        private IQuestionService questionService;

        public ActivityController()
        {
            logService = new LogService();
            questionService = new QuestionService();
        }


        //Lecturer
        // GET: Activity
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "Get Activities by User", "QBCS", protectType: ProtectType.Authorized, ShortName = "Activity", InternalId = (int)SideBarEnum.ActivityByUser)]
        [LogAction(Action = "Activities")]
        public ActionResult Index()
        {
            List<LogViewModel> logViews = new List<LogViewModel>();
            var user = (UserViewModel)Session["user"];
            int userId = user != null ? user.Id : 0;
            var model = logService.GetAllActivitiesByUserId(userId);
            TempData["active"] = "Activity";
            return View(model);
        }

        //Staff
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "Get All Activities", "QBCS", protectType: ProtectType.Authorized, ShortName = "Activity", InternalId = (int)SideBarEnum.AllActivity)]
        [LogAction(Action = "Activities")]
        public ActionResult GetAllActivities()
        {
            List<LogViewModel> logViews = new List<LogViewModel>();
            var model = logService.GetAllActivities();
            TempData["active"] = "Activity";
            return View("Index", model);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Activity Detail", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Import", Message = "Question Activity", TargetId = "targetId", IdParamName = "importId")]
        public ActionResult GetLogByQuestionID(int targetId, int importId)
        {
            List<LogViewModel> logViews = new List<LogViewModel>();
            LogViewModel logModel = new LogViewModel();
            var user = (UserViewModel)Session["user"];
            //var model = logService.GetAllActivities();
           
            logModel = logService.GetQuestionImportByTargetId(importId);
            logViews = logService.GetAllActivitiesByTargetId(targetId);
            logViews.Add(logModel);
            //logViews = logService.GetAllActivitiesByUserId(user.Id, user);
            TempData["active"] = "Activity";
            return View("Index", logViews);
        }
        //Lecturer
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Compare Question History", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Update", Message = "Question Activity", TargetId = "id")]
        public ActionResult GetUpdateActivityById (int id)
        {
            var model = logService.GetActivitiesById(id);
            TempData["active"] = "Activity";
            return View("GetUpdateActivity", model);
        }

        //Lecturer
        //Staff
        //stpm: feature declare
        //[Feature(FeatureType.Page, "Compare Question History", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Move", Message = "Question Activity", TargetId = "id")]
        public ActionResult GetMoveActivityById(int id)
        {
            var model = logService.GetActivitiesById(id);
            TempData["active"] = "Activity";
            return View("GetUpdateActivity", model);
        }

        //Lecturer
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Activity Question LifeCycle", "QBCS", protectType: ProtectType.Authorized)]
        
        public ActionResult GetListTargetByID(int id, int? targetId)
        {
            List<LogViewModel> list = new List<LogViewModel>();
           // var listTemp = logService.GetActivitiesById(id);
           //list.Add(listTemp as LogViewModel);

            if (targetId > 0)
            {
                List<LogViewModel> tempImport = logService.GetListQuestionImportByTargetId((int)targetId);
                if (tempImport != null)
                {
                    list = tempImport;
                }
               
            }
            TempData["active"] = "Activity";
            return View("GetListActivity", list);
        }
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Question History In Examination", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Examination", Message = "History Activity", IdParamName = "id")]
        public ActionResult GetExaminationHistory(int id)
        {
            var result = questionService.GetQuestionHistory(id);
            TempData["active"] = "Course";
            return View(result);
        }
    }
}