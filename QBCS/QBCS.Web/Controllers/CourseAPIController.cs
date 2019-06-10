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

        [HttpGet]
        [ActionName("searchCourse")]
        public List<CourseViewModel> searchCourse(string courseCode)
        {
            string[] courseName = {"PRX301","PRJ321","PRF291" };
            List<CourseViewModel> courseViewModels = new List<CourseViewModel>();
            for (int i = 0; i < 3; i++)
            {
                CourseViewModel courseViewModel = new CourseViewModel()
                {
                    Id = i,
                    Code = courseName[i],
                };
                courseViewModels.Add(courseViewModel);
            }
            return courseViewModels;
        }
    }
}
