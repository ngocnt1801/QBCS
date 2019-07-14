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
    public class LogActionController : Controller
    {
        private ILogActionService logActionService;
        public LogActionController()
        {
            logActionService = new LogActionService();
        }
        // GET: LogAction
        
        public ActionResult Index()
        {
           
            var user = (UserViewModel)Session["user"];
            int userId = user != null ? user.Id : 0;
            var list = logActionService.GetLogAction();
            return View(list);
        }
    }
}