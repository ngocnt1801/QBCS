using QBCS.Service.Implement;
using QBCS.Service.ViewModel;
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
            
            return View(list);
        }

        public ActionResult Disable(int userId)
        {
            userService.DisableUser(userId);

            return RedirectToAction("Index");
        }

        public ActionResult Update(UserViewModel model)
        {
            userService.UpdateUserInfo(model);

            return RedirectToAction("Index");
        }

        public ActionResult DeleteCourse(int userId, int courseId)
        {
            userService.RemoveUserCourse(courseId, userId);
            return RedirectToAction("Details", "Details", new { id = userId });
        }

        public ActionResult AddCourse(int courseId, int userId)
        {
            userService.AddUserCourse(courseId, userId);
            return RedirectToAction("Details", "Details", new { id = userId });
        }
    }
}