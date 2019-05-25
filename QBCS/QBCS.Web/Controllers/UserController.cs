using QBCS.Service.Implement;
using QBCS.Service.Interface;
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
        private ICourseService courseService;
        public UserController()
        {
            userService = new UserService();
            courseService = new CourseService();
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
            return RedirectToAction("Details", "User", new { userId = userId });
        }

        public ActionResult AddCourse(int courseId, int userId)
        {
            userService.AddUserCourse(courseId, userId);
            return RedirectToAction("Details", "User", new { userId = userId });
        }
        public ActionResult Details(int userId)
        {
            var item = userService.GetUserById(userId);
            var listAvailable = courseService.GetAvailableCourse(userId);
            var model = new UserDetailViewModel()
            {
                User = item,
                AvailableToAddCourses = listAvailable
            };
            return View(model);
        }
    }
}