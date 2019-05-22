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
        private UserService userService;
        public DetailsController()
        {
            userService = new UserService();
        }
        // GET: Details
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            var item = userService.GetUserById(id);
            if (item.Role == "3")
            {
                item.Role = "Staff";
            }
            if (item.Role == "2")
            {
                item.Role = "Lecturer";
            }
            else
            {
                item.Role = "Admin";
            }
            return View(item);
        }
    }
}