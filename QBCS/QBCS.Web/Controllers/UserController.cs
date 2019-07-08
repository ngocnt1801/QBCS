using AuthLib.Module;
using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using System.Collections.Generic;
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
        //Admin
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "All Users", "QBCS", protectType: ProtectType.Authorized, ShortName = "User", InternalId = (int)SideBarEnum.AllUser)]
        public ActionResult Index()
        {
            var list = userService.GetAllUser();
            TempData["active"] = "User";
            return View(list);
        }

        //Admin
        //stpm: feature declare
        [Feature(FeatureType.Page, "Disable User", "QBCS", protectType: ProtectType.Authorized)]
        [Log(Action = "Disable", TargetName = "User", IdParamName = "userId", Fullname = "", UserCode = "")]
        public ActionResult Disable(int userId)
        {
            userService.DisableUser(userId);

            return RedirectToAction("Index");
        }

        //Admin
        //stpm: feature declare
        [Feature(FeatureType.Page, "Enable User", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult Enable(int userId)
        {
            userService.EnableUser(userId);

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

        //Admin
        //stpm: feature declare
        [Feature(FeatureType.Page, "Delete Course From User", "QBCS", protectType: ProtectType.Authorized)]
        [Log(Action = "Delete", TargetName = "Courses of User", Fullname = "", UserCode = "", IdParamName = "userId")]
        public ActionResult DeleteCourse(int userId, int courseId)
        {
            userService.RemoveUserCourse(courseId, userId);
            return RedirectToAction("Details", "User", new { userId = userId });
        }

        //Admin
        //stpm: feature declare
        [Feature(FeatureType.Page, "Add Course To User", "QBCS", protectType: ProtectType.Authorized)]
        [Log(Action = "Add", TargetName = "Courses of User", Fullname = "", UserCode = "", IdParamName = "userId")]
        public ActionResult AddCourse(int courseId, int userId)
        {
            userService.AddUserCourse(courseId, userId);
            return RedirectToAction("Details", "User", new { userId = userId });
        }

        //Admin
        //stpm: feature declare
        [Feature(FeatureType.Page, "Get detail for edit user", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult Details(int userId)
        {
            var item = userService.GetUserById(userId);
            var listAvailable = courseService.GetAvailableCourse(userId);
            var model = new UserDetailViewModel()
            {
                User = item,
                AvailableToAddCourses = listAvailable
            };
            TempData["active"] = "User";
            return View(model);
        }

        //lecturer
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Search Lecturer By Name", "QBCS", protectType: ProtectType.Authorized)]
        public JsonResult GetLecturer(string term)
        {
            List<string> lecturerName = new List<string>();
            var result = userService.GetUserByNameAndRoleId(term, (int)RoleEnum.Lecturer);
            foreach (var lec in result)
            {
                lecturerName.Add(lec.Fullname + " (" + lec.Code + ")");
            }
            return Json(lecturerName, JsonRequestBehavior.AllowGet);
        }
    }
}