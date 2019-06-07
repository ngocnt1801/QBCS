﻿using QBCS.Entity;
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
        public CourseViewModel GetCourseById(int id)
        {
            var course = unitOfWork.Repository<Course>().GetById(id);
            var result = new CourseViewModel()
            {
                Name = course.Name,
                Code = course.Code,
                DefaultNumberOfQuestion = (int) course.DefaultNumberOfQuestion
            };
            return result;
        }
        public List<CourseViewModel> GetAllCourses()
        {
            
            var course = unitOfWork.Repository<Course>().GetAll().Where(c => c.IsDisable == false).Select(c => new CourseViewModel {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name
            });
            
           
            return course.ToList();
        }

        public List<CourseViewModel> GetAvailableCourse(int userId)
        {
            var listAvailable = new List<CourseViewModel>();
            if (userId != 0)
            {
               var listCurrentCourse = unitOfWork.Repository<CourseOfUser>().GetAll().Where(uc => uc.UserId == userId).Select(uc => uc.CourseId).ToList();
               listAvailable = unitOfWork.Repository<Course>().GetAll()
                                                                    .Where(c => !listCurrentCourse.Contains(c.Id))
                                                                    .Select(c => new CourseViewModel
                                                                    {
                                                                        Id = c.Id,
                                                                        Code = c.Code,
                                                                        Name = c.Name
                                                                    }).ToList();
            }

            return listAvailable;
        }
        public List<CourseViewModel> GetAllCoursesByUserId(int id)
        {
            
            var user = unitOfWork.Repository<User>().GetById(id);
            var courses = user.CourseOfUsers.Select(c => new CourseViewModel
            {
                Id = c.Id,
                CourseId = (int)c.CourseId,
                Name = c.Course.Name,
                Code = c.Course.Code,
                IsDisable = (bool)c.Course.IsDisable
            }).Where(c => c.IsDisable == false).ToList();
            return courses;
        }
        public List<Course> GetCoursesByName(string name)
        {
            IQueryable<Course> courses = unitOfWork.Repository<Course>().GetAll().Where(c => c.Name.Contains(name));
            List<Course> result = courses.ToList();
            return result;
        }

        public bool UpdateDisable(int id)
        {
            try
            {
                var course = unitOfWork.Repository<Course>().GetById(id);
                course.IsDisable = true;
                unitOfWork.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }
        public bool AddNewCourse(CourseViewModel model)
        {
            var course = new Course()
            {
                Name = model.Name,
                Code = model.Code,
                DefaultNumberOfQuestion = model.DefaultNumberOfQuestion,
                IsDisable = false
            };
            unitOfWork.Repository<Course>().Insert(course);
            unitOfWork.SaveChanges();
            return true;
        }
    }
}
