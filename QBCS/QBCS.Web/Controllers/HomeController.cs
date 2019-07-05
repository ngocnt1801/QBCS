using AuthLib.Module;
using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using QBCS.Web.SignalRHub;
using System;
using System.Collections.Generic;
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
        [Feature(FeatureType.SideBar
            , "Lecturer Home page"
            , "QBCS", protectType: ProtectType.Authorized
            , ShortName = "Import"
            , InternalId = (int)SideBarEnum.Import)]
        [Dependency(typeof(UserController), nameof(UserController.GetLecturer))]
        public ActionResult Index()
        {
            ViewBag.Title = "Lecturer Page";

            //stpm: get logged in user code
            var userCode = User.Identity.Get(a => a.Code);
            Session["user"] = userService.GetUser(userCode);
            
            ViewBag.Name = "";
            TempData["active"] = "Import";
            return View("Index", null);
        }

        [Feature(FeatureType.SideBar
            , "Staff Home page"
            , "QBCS", protectType: ProtectType.Authorized
            , ShortName = "Home"
            , InternalId = (int)SideBarEnum.Staff)]
        public ActionResult Staff()
        {
            ViewBag.Title = "Staff Page";

            //stpm: get logged in user code
            var userCode = User.Identity.Get(a => a.Code);
            Session["user"] = userService.GetUser(userCode);

            ViewBag.Name = "";
            TempData["active"] = "Home";

            return View("Staff", null);
        }

        [Feature(FeatureType.SideBar
            , "Admin Home page"
            , "QBCS", protectType: ProtectType.Authorized
            , ShortName = "Home"
            , InternalId = (int)SideBarEnum.Admin)]
        public ActionResult Admin()
        {
            ViewBag.Title = "Admin Page";

            //stpm: get logged in user code
            var userCode = User.Identity.Get(a => a.Code);
            Session["user"] = userService.GetUser(userCode);

            ViewBag.Name = "";
            TempData["active"] = "Home";

            return View("Admin", null);
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

        //stpm: feature declare
        [Feature(FeatureType.SideBar
            , "Import - Manually"
            , "QBCS", protectType: ProtectType.Authorized
            , ShortName = "Manually"
            , InternalId = (int)SideBarEnum.Manually)]
        [Dependency(typeof(QuestionController), nameof(QuestionController.ImportTextarea))]
        public ActionResult ImportWithTextArea()
        {
            TempData["active"] = "Manually";
            return View();
        }

        [Feature(FeatureType.SideBar
            , "Import - Word"
            , "QBCS", protectType: ProtectType.Authorized
            , ShortName = "Import MS Word"
            , InternalId = (int)SideBarEnum.ImportMSWord)]
        public ActionResult ImportWord()
        {
            TempData["active"] = "ImportWord";
            var user = (UserViewModel)Session["user"];
            return View(user);
        }
    }
}
