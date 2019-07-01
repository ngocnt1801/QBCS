using AuthLib.Module;
using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
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
        [Feature(FeatureType.Page, "Activities", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(LogService), nameof(LogService.GetAllActivitiesByUserId))]
        public ActionResult Index(int id)
        {
            List<LogViewModel> logViews = new List<LogViewModel>();
            var user = (UserViewModel)Session["user"];
            //var model = logService.GetAllActivities();
            var model = logService.GetAllActivitiesByUserId(id, user);
            return View(model);
        }

        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Get All Activities", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(LogService), nameof(LogService.GetAllActivities))]
        public ActionResult GetAllActivities()
        {
            List<LogViewModel> logViews = new List<LogViewModel>();
            var user = (UserViewModel)Session["user"];
            //var model = logService.GetAllActivities();
            var model = logService.GetAllActivities();
            return View("Index", model);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Activity Detail", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(LogService), nameof(LogService.GetQuestionImportByTargetId))]
        [Dependency(typeof(LogService), nameof(LogService.GetAllActivitiesByTargetId))]
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
            return View("Index", logViews);
        }
        //Lecturer
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Compare Question History", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(LogService), nameof(LogService.GetActivitiesById))]
        public ActionResult GetUpdateActivityById (int id)
        {
            var model = logService.GetActivitiesById(id);
            return View("GetUpdateActivity", model);
        }

        //Lecturer
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Activity Question LifeCycle", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(LogService), nameof(LogService.GetListQuestionImportByTargetId))]
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
            return View("GetListActivity", list);
        }
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Question History In Examination", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(QuestionService), nameof(QuestionService.GetQuestionHistory))]
        public ActionResult GetExaminationHistory(int id)
        {
            var result = questionService.GetQuestionHistory(id);
            return View(result);
        }
    }
}