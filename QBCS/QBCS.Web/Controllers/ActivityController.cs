using QBCS.Service.Implement;
using QBCS.Service.Interface;
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
        public ActionResult Index()
        {
            var model = logService.GetAllActivities();
            return View(model);
        }
    }
}