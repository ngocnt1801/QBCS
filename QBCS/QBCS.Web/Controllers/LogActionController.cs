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
        public JsonResult GetLogAction(int? draw, int? start, int? length)
        {
            var search = Request["search[value]"] != null ? Request["search[value]"].ToLower() : "";
            var entities = logActionService.GetLogAction();
            var recordTotal = entities.Count();
            var result = new List<LogViewModel>();

            result = entities.Where(a => a.Action.Contains(search) || a.LogDate.ToString("dd/M/yyyy", CultureInfo.InvariantCulture).Contains(search) || a.Ip.Contains(search)).ToList();
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
    }
}