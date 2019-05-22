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
    public class DetailsController : Controller
    {
        private IUserService userService;
        private ICourseService courseService;
        public DetailsController()
        {
            userService = new UserService();
            courseService = new CourseService();
        }
        // GET: Details
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
           var item = userService.GetUserById(id);
           var listAvailable = courseService.GetAvailableCourse(id);
            var model = new UserDetailViewModel()
            {
                User = item,
                AvailableToAddCourses = listAvailable
            };
            return View(model);
        }
    }
}