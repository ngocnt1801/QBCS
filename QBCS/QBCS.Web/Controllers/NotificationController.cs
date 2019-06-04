using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.SignalRHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    public class NotificationController : Controller
    {
        private INotificationService notificationService;
        public NotificationController()
        {
            notificationService = new NotificationService();
            notificationService.RegisterNotification(ImportResultHub.ImportStatus_OnChange);
        }
        // GET: Notification
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetNotification()
        {
            int count = 0;
            var user = (UserViewModel)Session["user"];
            if (user != null)
            {
                count = notificationService.GetNotifyImportResult(user.Id, ImportResultHub.ImportStatus_OnChange);
            }
            return Json(count, JsonRequestBehavior.AllowGet);
        }
    }
}