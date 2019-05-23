using QBCS.Entity;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService iCourseService;
        
        public CourseController()
        {
            iCourseService = new CourseService();
        }

        // GET: Course
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCoursesByName(string name)
        {
            List<CourseViewModel> result = new List<CourseViewModel>();

            List<Course> courses = iCourseService.GetCoursesByName(name);
            foreach (Course course in courses)
            {
                CourseViewModel courseViewModel = new CourseViewModel
                {
                    Name = course.Name,
                    Code = course.Code,
                    DefaultNumberOfQuestion = course.DefaultNumberOfQuestion
                };
                result.Add(courseViewModel);
            }
            return View("ListCourse", result);
        }
    }
}