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
    public class LogActionController : Controller
    {
        private ILogActionService logActionService;
        public LogActionController()
        {
            logActionService = new LogActionService();
        }
        // GET: LogAction

        [Feature(FeatureType.SideBar
            , "Log action page"
            , "QBCS", protectType: ProtectType.Authorized
            , ShortName = "Log"
            , InternalId = (int)SideBarEnum.LogAction)]
        public ActionResult Index()
        {

            //var user = (UserViewModel)Session["user"];
            //int userId = user != null ? user.Id : 0;
            //var list = logActionService.GetLogAction();
            //return View(list);
            return View();
        }
        public JsonResult GetLogAction(int draw, int start, int length)
        {
            var search = Request["search[value]"] != null ? Request["search[value]"].ToLower() : "";
            var data = logActionService.GetLogAction(search,start,length);
            return Json(new { draw = draw, recordsFiltered = data.filteredCount, recordsTotal = data.totalCount, data = data.Logs, success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}