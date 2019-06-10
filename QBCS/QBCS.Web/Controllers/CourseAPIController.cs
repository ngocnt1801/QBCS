using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QBCS.Web.Controllers
{
    public class CourseAPIController : ApiController
    {

        private ICourseService courseService;

        public CourseAPIController()
        {
            courseService = new CourseService();
        }

        [HttpGet]
        [ActionName("searchCourse")]
        public List<CourseViewModel> searchCourse(string courseCode)
        {
            List<CourseViewModel> courseViewModels = courseService.SearchCourseByNameOrCode(courseCode);
            return courseViewModels;
        }
    }
}
