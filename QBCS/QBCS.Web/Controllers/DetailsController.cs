using QBCS.Service.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    public class DetailsController : Controller
    {
        private UserService service;
        public DetailsController()
        {
            service = new UserService();
        }
        // GET: Details
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            var item = service.GetUserById(id);
           
            return View(item);
        }
    }
}