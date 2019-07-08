using QBCS.Entity;
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
        CourseViewModel GetCourseById(int id);
        bool AddNewCourse(CourseViewModel model);
        List<CourseViewModel> GetAllCourses();
        List<CourseViewModel> GetCoursesVMByName(string name);
        List<CourseViewModel> GetAllCoursesWithDetail();
        List<CourseViewModel> GetAllCoursesWithDetailById(int userId);
        List<CourseViewModel> GetAvailableCourse(int userId);
        List<CourseViewModel> GetAllCoursesByUserId(int? id);
        List<Course> GetCoursesByName(string name);
        bool UpdateCourse(CourseViewModel course);
        List<CourseViewModel> SearchCourseByNameOrCode(string searchContent);

        CourseViewModel GetDetailCourseById(int id);

        bool UpdateDisable(int id);
        List<CourseStatViewModel> GetAllCourseStat(int? id);
        CourseStatDetailViewModel GetCourseStatDetailByIdAndType(int id, string type);
        List<CourseViewModel> GetCourseByDisable();
    }
}
