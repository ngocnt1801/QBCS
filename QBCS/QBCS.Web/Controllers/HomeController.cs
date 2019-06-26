using AuthLib.Module;
using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using QBCS.Web.SignalRHub;
using System;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUserService userService;

        public HomeController()
        {
            userService = new UserService();
        }

        //stpm: feature declare
        [Feature(FeatureType.Page, "Home page", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(HomeController), nameof(HomeController.Login))]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            //stpm: get logged in user code
            var userCode = User.Identity.Get(a => a.Code);

            var user = (UserViewModel)Session["user"];
            string viewName = "Login";
            if (user == null)
            {
                return View(viewName);
            }
            ViewBag.Name = user.Fullname;

            if (user.Role == RoleEnum.Admin)
            {
                viewName = "Admin";
            }
            else if (user.Role == RoleEnum.Lecturer)
            {
                viewName = "Index";
            }
            else
            {
                viewName = "Staff";
            }

            return View(viewName, user);
        }


        public ActionResult Login(string username, string password)
        {
            var user = userService.Login(username, password);
            if (user != null)
            {
                Session["user"] = user;
                ViewBag.Name = user.Fullname;
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("LoginFail", "Your username or password is correct");

            return View();

        }
        public ActionResult Logout(string username)
        {

            Session.Clear();

            return RedirectToAction("Index");

        }


    }
}
