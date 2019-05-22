using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{ 
    public class CourseService : ICourseService
    {
        private IUnitOfWork unitOfWork;
        public CourseService()
        {
            unitOfWork = new UnitOfWork();
        }
        public List<CourseViewModel> GetAllCourses()
        {
            
            var course = unitOfWork.Repository<Course>().GetAll().Select(c => new CourseViewModel {
                Code = c.Code,
                Name = c.Name
            });
            
           
            return course.ToList();
        }

        public List<CourseViewModel> GetAvailableCourse(int userId)
        {
            var listCurrentCourse = unitOfWork.Repository<CourseOfUser>().GetAll().Where(uc => uc.UserId == userId).Select(uc => uc.CourseId).ToList();
            var listAvailable = unitOfWork.Repository<Course>().GetAll()
                                                                .Where(c => !listCurrentCourse.Contains(c.Id))
                                                                .Select(c => new CourseViewModel
                                                                {
                                                                    Id = c.Id,
                                                                    Code = c.Code,
                                                                    Name = c.Name
                                                                }).ToList();

            return listAvailable;
        }
        public List<CourseViewModel> GetAllCoursesByUserId(int id)
        {
            var courseId = 0;
            var user = unitOfWork.Repository<User>().GetById(id);
            var courses = user.CourseOfUsers.Select(c => new CourseViewModel
            {
                Id = c.Id,
                CodeId = (int)c.CourseId,
                Name = c.Course.Name,
                Code = c.Course.Code
            });
           
           
           
            return courses.ToList();
        }
    }
}
