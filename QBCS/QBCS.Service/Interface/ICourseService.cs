using QBCS.Service.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface ICourseService
    {
        List<CourseViewModel> GetAllCourses();
        List<CourseViewModel> GetAvailableCourse(int userId);
        List<CourseViewModel> GetAllCoursesByUserId(int id);
    }
}
