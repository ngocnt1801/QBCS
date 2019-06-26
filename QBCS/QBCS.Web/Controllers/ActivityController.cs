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

        public ActivityController()
        {
            logService = new LogService();
        }

        // GET: Activity
        public ActionResult Index(int targetId)
        {
            List<LogViewModel> logViews = new List<LogViewModel>();
            var user = (UserViewModel)Session["user"];
            //var model = logService.GetAllActivities();
            var model = logService.GetAllActivitiesByTargetId(targetId);
            return View(model);
        }
        public ActionResult GetListTargetByID(int id)
        {
            var list = logService.GetActivitiesById(id);
            return View("GetListActivity", list);
        }
    }
}