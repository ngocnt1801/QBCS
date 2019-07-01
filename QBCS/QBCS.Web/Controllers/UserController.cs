using QBCS.Service.Enum;
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
    [CheckSession]
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

            var role = ((UserViewModel)Session["user"]).Role;
            if (role == RoleEnum.Admin)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

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

        //lecturer
        public JsonResult GetLecturer(string term)
        {
            List<string> lecturerName = new List<string>();
            var result = userService.GetUserByNameAndRoleId(term, (int)RoleEnum.Lecturer);
            foreach(var lec in result)
            {
                lecturerName.Add(lec.Fullname);
            }
            return Json(lecturerName, JsonRequestBehavior.AllowGet);
        }
    }
}