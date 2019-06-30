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

        // GET: Activity
        public ActionResult Index(int id)
        {
            List<LogViewModel> logViews = new List<LogViewModel>();
            var user = (UserViewModel)Session["user"];
            //var model = logService.GetAllActivities();
            var model = logService.GetAllActivitiesByUserId(id);
            return View(model);
        }
        public ActionResult GetLogByQuestionID(int targetId)
        {
            List<LogViewModel> logViews = new List<LogViewModel>();
            var user = (UserViewModel)Session["user"];
            //var model = logService.GetAllActivities();
            var model = logService.GetAllActivitiesByTargetId(targetId);
            return View("Index", model);
        }
        public ActionResult GetListTargetByID(int id)
        {
            var list = logService.GetActivitiesById(id);
            return View("GetListActivity", list);
        }
        public ActionResult GetExaminationHistory(int id)
        {
            var result = questionService.GetQuestionHistory(id);
            return View(result);
        }
    }
}