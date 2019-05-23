using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    public class UserController : Controller
    {
        private IUserService iUserService;

        public UserController()
        {
            iUserService = new UserService();
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddUser()
        {
            return View();
        }
        public ActionResult AddUserToDB(QBCS.Service.ViewModel.UserViewModel user)
        {
            iUserService.AddUser(user);

            return View("AddQuestion");
        }

    }
}