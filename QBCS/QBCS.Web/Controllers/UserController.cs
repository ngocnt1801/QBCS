using QBCS.Service.Implement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    public class UserController : Controller
    {
        private UserService userService;
        public UserController()
        {
            userService = new UserService();
        }
        // GET: User
        public ActionResult Index()
        {
            var list = userService.GetAllUser();
            foreach (var item in list)
            {
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
            }
            return View(list);
        }
    }
}