using QBCS.Entity;
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
        // GET: Course
        public ActionResult Index(int userId)
        {
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

        public ActionResult GetAllCourse()
        {
            List<CourseViewModel> courses = courseService.GetAllCourses();
            TempData["CreateExam"] = true;
            return View("Staff_ListCourse", courses);
        }


        public ActionResult GetAllCourseForHistory()
        {
            List<CourseViewModel> courses = courseService.GetAllCourses();
            TempData["ViewHistory"] = true;
            return View("Staff_ListCourse", courses);
        }

        public ActionResult GetCourseByNameOrId(string searchValue)
        {
            List<CourseViewModel> courses = courseService.SearchCourseByNameOrCode(searchValue);
            return View("Staff_ListCourse", courses);
        }

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
        public ActionResult CourseStatistic()
        {
            int userId = ((UserViewModel)Session["user"]).Id;
            var result = courseService.GetAllCourseStat();
            TempData["active"] = "Statistics";
            return View(result);
        }
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