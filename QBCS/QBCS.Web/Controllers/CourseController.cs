using AuthLib.Module;
using QBCS.Entity;
using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    [CheckSession]
    public class CourseController : Controller
    {
        private ICourseService courseService;
        private ICategoryService categoryService;
        public CourseController()
        {
            courseService = new CourseService();
            categoryService = new CategoryService();
        }

        [Feature(FeatureType.SideBar, "List all course by user", "QBCS", protectType: ProtectType.Authorized, ShortName = "Course", InternalId = (int)SideBarEnum.CourseByUser)]
        // GET: Course
        public ActionResult Index()
        {
            var user = ((UserViewModel)Session["user"]);
            int userId = user != null ? user.Id : 0;
            var list = courseService.GetAllCoursesByUserId(userId);
            TempData["active"] = "Course";
            return View(list);
        }

        public ActionResult Staff_Index()
        {
            var list = courseService.GetCourseByDisable();
            return View(list);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(CourseViewModel model)
        {
            courseService.AddNewCourse(model);
            return RedirectToAction("Staff_Index");
        }
        public ActionResult Edit(int itemId)
        {
            var result = courseService.GetCourseById(itemId);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(CourseViewModel model)
        {
            var result = courseService.UpdateCourse(model);
            if (result)
            {
                return RedirectToAction("Detail","Course", new { itemId = model.Id});
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }
            
        }
        public ActionResult GetCoursesByName(string name)
        {
            List<CourseViewModel> result = new List<CourseViewModel>();

            List<Course> courses = courseService.GetCoursesByName(name);
            foreach (Course course in courses)
            {
                CourseViewModel courseViewModel = new CourseViewModel
                {
                    Name = course.Name,
                    Code = course.Code,
                    DefaultNumberOfQuestion = course.DefaultNumberOfQuestion.Value
                };
                result.Add(courseViewModel);
            }
            return View("ListCourse", result);
        }
        public ActionResult Detail(int itemId)
        {
            var result = courseService.GetDetailCourseById(itemId);
            return View(result);
        }
        public ActionResult UpdateDisable(int itemId)
        {
            //int itemId = 0;
            //int userId = 0;
            //try
            //{
            //    string[] split = itemAndUserId.Split('_');
            //    itemId = int.Parse(split[0]);
            //    userId = int.Parse(split[1]);
            //}
            //catch (NotFiniteNumberException)
            //{

            //}
            var update = courseService.UpdateDisable(itemId);
            return RedirectToAction("Staff_Index");
        }

        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "All Courses For Generate", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult GetAllCourse()
        {
            List<CourseViewModel> courses = courseService.GetAllCourses();
            TempData["CreateExam"] = true;
            TempData["active"] = "Examination";
            return View("Staff_ListCourse", courses);
        }

        //Staff
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "All Courses For History", "QBCS", protectType: ProtectType.Authorized, ShortName = "Course", InternalId = (int)SideBarEnum.AllCourseHistory)]
        public ActionResult GetAllCourseForHistory()
        {
            List<CourseViewModel> courses = courseService.GetAllCourses();
            TempData["ViewHistory"] = true;
            TempData["active"] = "Course";
            return View("Staff_ListCourse", courses);
        }

        public ActionResult GetCourseByNameOrId(string searchValue)
        {
            List<CourseViewModel> courses = courseService.SearchCourseByNameOrCode(searchValue);
            return View("Staff_ListCourse", courses);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Course Detail", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(QuestionController), nameof(QuestionController.GetQuestions))]
        [Dependency(typeof(QuestionController), nameof(QuestionController.ToggleDisable))]
        [Dependency(typeof(QuestionController), nameof(QuestionController.UpdateCategory))]
        public ActionResult CourseDetail(int courseId)
        {
            List<CategoryViewModel> categories = categoryService.GetListCategories(courseId);
            var model = new CourseViewModel
            {
                Id = courseId,
                Categories = categories
            };
            TempData["active"] = "Course";
            return View(model);
        }

        //Lecturer
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "All Course Statistic", "QBCS", protectType: ProtectType.Authorized, ShortName = "Statistic", InternalId = (int)SideBarEnum.AllStatistic)]
        //stpm: dependency declare
        [Dependency(typeof(CourseController), nameof(CourseController.GetCourseDetailStat))]
        public ActionResult CourseStatistic()
        {
            var result = courseService.GetAllCourseStat(null);
            TempData["active"] = "Statistic";
            return View(result);
        }

        [Feature(FeatureType.SideBar, "Course Statistic By User", "QBCS", protectType: ProtectType.Authorized, ShortName = "Statistic", InternalId = (int)SideBarEnum.StatisticByUser)]
        //stpm: dependency declare
        [Dependency(typeof(CourseController), nameof(CourseController.GetCourseDetailStat))]
        public ActionResult CourseStatisticByUser()
        {
            var user = (UserViewModel)Session["user"];
            int userId = user != null ? user.Id : 0;
            var result = courseService.GetAllCourseStat(userId);
            TempData["active"] = "Statistic";
            return View("CourseStatistic", result);
        }

        //Lecturer
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Course Detail Statistic", "QBCS", protectType: ProtectType.Authorized)]
        public JsonResult GetCourseDetailStat(int courseId)
        {
            var result = courseService.GetCourseStatDetailByCourseId(courseId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CourseDetailWithoutId()
        {
            List<CategoryViewModel> categories = categoryService.GetAllCategories();
            var model = new CourseViewModel
            {
                Categories = categories
            };
            return View(model);
        }
    }
}