﻿using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using QBCS.Web.SignalRHub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    [CheckSession]
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

        //All role
       
        public JsonResult GetNotification()
        {
            List<NotificationViewModel> notificationList = null;
            var user = (UserViewModel)Session["user"];
            if (user != null)
            {
                notificationList = notificationService.GetNotifyImportResult(user.Id);
            }
            return Json(notificationList, JsonRequestBehavior.AllowGet);
        }

        //All role
        [LogAction(Action = "Notification", Message = "Mark Read All Notification", Method = "GET")]
        public JsonResult ReadAll()
        {
            int userId = ((UserViewModel)Session["user"]).Id;
            notificationService.MarkAllAsRead(userId);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}