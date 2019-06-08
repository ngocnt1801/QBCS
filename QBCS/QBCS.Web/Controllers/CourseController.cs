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
        public CourseController()
        {
            courseService = new CourseService();
        }
        // GET: Course
        public ActionResult Index(int userId)
        {           
            var list = courseService.GetAllCoursesByUserId(userId); 
            return View(list);         
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
    }
}