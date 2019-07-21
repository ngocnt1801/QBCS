using AuthLib.Module;
using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.Utilities;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        [LogAction(Action = "Activities", Message = "View All Activities", Method = "GET")]
        public ActionResult Index()
        {
            //List<LogViewModel> logViews = new List<LogViewModel>();
            //var user = (UserViewModel)Session["user"];
            //int userId = user != null ? user.Id : 0;
            //var model = logService.GetAllActivitiesByUserId(userId);
            //TempData["active"] = "Activity";
            //return View(model);
            ViewBag.isUser = true;
            return View();
        }

        //Staff
        //stpm: feature declare
        //[Feature(FeatureType.SideBar, "Get All Activities", "QBCS", protectType: ProtectType.Authorized, ShortName = "All Activities", InternalId = (int)SideBarEnum.AllActivity)]
        //[LogAction(Action = "Activities", Message = "View All Activities", Method = "GET")]
        public ActionResult GetAllActivities()
        {
            //List<LogViewModel> logViews = new List<LogViewModel>();
            //var model = logService.GetAllActivities();
            //TempData["active"] = "Activity";
            //return View("Index", model);

            return View("Index");
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Activity Detail", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Activities", Message = "View Activity Detail by Id", Method = "GET")]
        public ActionResult GetLogByQuestionID(int targetId, int importId)
        {
            //List<LogViewModel> logViews = new List<LogViewModel>();
            //LogViewModel logModel = new LogViewModel();
            //var user = (UserViewModel)Session["user"];
            ////var model = logService.GetAllActivities();

            //logModel = logService.GetQuestionImportByTargetId(importId);
            //logViews = logService.GetAllActivitiesByTargetId(targetId);
            //logViews.Add(logModel);
            ////logViews = logService.GetAllActivitiesByUserId(user.Id, user);
            //TempData["active"] = "Activity";

            //return View("Index", logViews);
            ViewBag.targetId = targetId;
            ViewBag.importId = importId;
            return View("Index");
        }

        public JsonResult GetActivityDatatable(int? importId, int? targetId, int? userId, int? draw, int? start, int? length)
        {
            var search = Request["search[value]"] != null ? Request["search[value]"].ToLower() : "";
            //Get all
            if (importId == null && targetId == null && userId == null)
            {
                var entities = logService.GetAllActivities();
                TempData["active"] = "Activity";
                var recordTotal = entities.Count();
                var result = new List<LogViewModel>();

                result = entities.Where(a => a.Action.Contains(search) || a.LogDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture).Contains(search)).ToList();
                foreach (var a in entities)
                {
                    var user = VietnameseToEnglish.SwitchCharFromVietnameseToEnglish(a.Fullname).ToLower();
                    if (user.Contains(search) || a.Fullname.Contains(search))
                    {
                        result.Add(a);
                    }
                }
                var recordFiltered = result.Count();
                if (length != null && length >= 0)
                {
                    result = result.Skip(start != null ? (int)start : 0).Take((int)length).ToList();
                }
                else
                {
                    result = result.ToList();
                }
                return Json(new { draw = draw, recordsFiltered = recordFiltered, recordsTotal = recordTotal, data = result, success = true }, JsonRequestBehavior.AllowGet);
            }
            //Get Log by QuestionId
            else if (importId != null && targetId != null)
            {
                var entities = logService.GetAllActivitiesByTargetId((int)targetId);
                LogViewModel logModel = new LogViewModel();

                logModel = logService.GetQuestionImportByTargetId((int)importId);
                entities.Add(logModel);
                TempData["active"] = "Activity";
                var recordTotal = entities.Count();
                var result = new List<LogViewModel>();

                result = entities.Where(a => a.Action.Contains(search) || a.LogDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture).Contains(search)).ToList();
                foreach (var a in entities)
                {
                    var user = VietnameseToEnglish.SwitchCharFromVietnameseToEnglish(a.Fullname).ToLower();
                    if (user.Contains(search) || a.Fullname.Contains(search))
                    {
                        result.Add(a);
                    }
                }
                var recordFiltered = result.Count();
                if (length != null && length >= 0)
                {
                    result = result.Skip(start != null ? (int)start : 0).Take((int)length).ToList();
                }
                else
                {
                    result = result.ToList();
                }
                return Json(new { draw = draw, recordsFiltered = recordFiltered, recordsTotal = recordTotal, data = result, success = true }, JsonRequestBehavior.AllowGet);
            } 
            //Index/Get Activity by User
            else if(userId != null)
            {
                var entities = logService.GetAllActivitiesByUserId((int)userId);
                TempData["active"] = "Activity";
                var recordTotal = entities.Count();
                var result = new List<LogViewModel>();

                result = entities.Where(a => a.Action.Contains(search) || a.LogDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture).Contains(search)).ToList();
                foreach (var a in entities)
                {
                    var user = VietnameseToEnglish.SwitchCharFromVietnameseToEnglish(a.Fullname).ToLower();
                    if (user.Contains(search) || a.Fullname.Contains(search))
                    {
                        result.Add(a);
                    }
                }
                var recordFiltered = result.Count();
                if (length != null && length >= 0)
                {
                    result = result.Skip(start != null ? (int)start : 0).Take((int)length).ToList();
                }
                else
                {
                    result = result.ToList();
                }
                return Json(new { draw = draw, recordsFiltered = recordFiltered, recordsTotal = recordTotal, data = result, success = true }, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        //Lecturer
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Compare Question History", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Activities", Message = "View All Update Question Activity", Method = "GET")]
        public ActionResult GetUpdateActivityById (int id)
        {
            var model = logService.GetActivitiesById(id);
            TempData["active"] = "Activity";
            return View("GetUpdateActivity", model);
        }

        //Lecturer
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Compare Question Move History", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Activities", Message = "View All Update Question Activity", Method = "GET")]
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
        [LogAction(Action = "Examination", Message = "View Examination History Activity", Method = "GET")]
        public ActionResult GetExaminationHistory(int id)
        {
            var result = questionService.GetQuestionHistory(id);
            TempData["active"] = "Course";
            return View(result);
        }
    }
}