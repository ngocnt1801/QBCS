using QBCS.Service.Implement;
using QBCS.Service.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService courseService;
        public CourseController()
        {
            courseService = new CourseService();
        }
        // GET: Course
        public ActionResult Index()
        {
            var list = courseService.GetAllCourses();
            
            return View(list);
            
        }
        
    }
}