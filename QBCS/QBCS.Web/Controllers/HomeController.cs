using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    [RoutePrefix("home")]
    public class HomeController : Controller
    {
        private IUserService userService;

        public HomeController()
        {
            userService = new UserService();
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            var user = (UserViewModel)Session["user"];

            if (user == null)
            {
                return View("Login");
            }
            ViewBag.Name = user.Fullname;
            
            if (user.Role == RoleEnum.Admin)
            {
                return View("Admin");
            } else if (user.Role == RoleEnum.Lecturer)
            {
                return View("Index", user);
            } else
            {
                return View("Staff");
            }
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

        
    }
}
