using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Enum;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System.Collections.Generic;
using System.Linq;

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
                Id = course.Id,
                Name = course.Name,
                Code = course.Code,
                DefaultNumberOfQuestion = course.DefaultNumberOfQuestion.HasValue ? (int)course.DefaultNumberOfQuestion : 0
            };
            return result;
        }
        public List<CourseViewModel> GetAllCourses()
        {
            var course = unitOfWork.Repository<Course>().GetAll().Select(c => new CourseViewModel
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name
            });

            return course.ToList();
        }
        public List<CourseViewModel> GetCoursesVMByNameAndCode(string name)
        {
            var course = unitOfWork.Repository<Course>().GetAll().Where(c => c.Name.Contains(name) || c.Code.Contains(name)).Select(c => new CourseViewModel
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name
            });

            return course.ToList();
        }
        public List<CourseViewModel> GetAllCoursesWithDetail()
        {
            var course = unitOfWork.Repository<Course>().GetAll().Where(c => c.IsDisable == false).Select(c => new CourseViewModel
            {
                Id = c.Id,
                Code = c.Code,
                Name = c.Name,
                LearningOutcome = c.LearningOutcomes.Select(lo => new LearningOutcomeViewModel {
                    Id = lo.Id,
                    Name = lo.Name
                }).ToList()
            });

            return course.ToList();
        }
        public List<CourseViewModel> GetAllCoursesWithDetailById(int userId)
        {
            var course = unitOfWork.Repository<CourseOfUser>().GetAll().Where(c => c.User.IsDisable == false && c.UserId == userId).Select(c => new CourseViewModel
            {
                Id = (int)c.CourseId,
                Code = c.Course.Code,
                Name = c.Course.Name,
                LearningOutcome = c.Course.LearningOutcomes.Select(lo => new LearningOutcomeViewModel
                {
                    Id = lo.Id,
                    Name = lo.Name
                }).ToList()
            });

            return course.ToList();
        }
        public List<CourseViewModel> GetCourseByDisable()
        {
            var entity = unitOfWork.Repository<Course>().GetAll().Where(c => c.IsDisable == false);
            var course = entity.Select(c => new CourseViewModel
            {
                CourseId = c.Id,
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
        public List<CourseViewModel> GetAllCoursesByUserId(int? id)
        {
            if (id != null)
            {
                var user = unitOfWork.Repository<User>().GetById(id);
                if (user != null)
                {
                    var courses = user.CourseOfUsers.Select(c => new CourseViewModel
                    {
                        Id = c.Id,
                        CourseId = c.CourseId.Value,
                        Name = c.Course.Name,
                        Code = c.Course.Code,
                        IsDisable = c.Course.IsDisable.HasValue && c.Course.IsDisable.Value
                    }).Where(c => c.IsDisable == false).ToList();
                    return courses;
                }
                else
                {
                    return new List<CourseViewModel>();
                }
            }
            else
            {
                var courses = unitOfWork.Repository<Course>().GetAll().Select(c => new CourseViewModel
                {
                    Id = 0,
                    CourseId = c.Id,
                    Name = c.Name,
                    Code = c.Code,
                    IsDisable = c.IsDisable.HasValue && c.IsDisable.Value
                }).Where(c => c.IsDisable == false).ToList();
                return courses;
            }
            
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
        public CourseViewModel GetDetailCourseById(int id)
        {
            var course = unitOfWork.Repository<Course>().GetById(id);
            var listTopic = new List<TopicViewModel>();
            var listLearningOutcome = new List<LearningOutcomeViewModel>();
            foreach (LearningOutcome learningOutcome in course.LearningOutcomes)
            {
                var learningOutcomeVM = new LearningOutcomeViewModel()
                {
                    Id = learningOutcome.Id,
                    Code = learningOutcome.Code,
                    Name = learningOutcome.Name
                };
                listLearningOutcome.Add(learningOutcomeVM);
            }
            var courseVM = new CourseViewModel()
            {
                CourseId = id,
                Code = course.Code,
                Name = course.Name,
                DefaultNumberOfQuestion = course.DefaultNumberOfQuestion.HasValue ? (int)course.DefaultNumberOfQuestion : 0,
                Topic = listTopic,
                LearningOutcome = listLearningOutcome,
            };
            return courseVM;
        }

        public List<CourseViewModel> SearchCourseByNameOrCode(string searchContent)
        {
            List<Course> courses = unitOfWork.Repository<Course>().GetAll().Where(c => (c.Name.ToLower()).Contains(searchContent.ToLower()) || (c.Code.ToLower()).Contains(searchContent.ToLower())).ToList();
            List<CourseViewModel> courseViewModels = new List<CourseViewModel>();
            foreach (var course in courses)
            {
                CourseViewModel courseViewModel = new CourseViewModel()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Code = course.Code
                };
                courseViewModels.Add(courseViewModel);
            }
            return courseViewModels;
        }
        public List<CourseStatViewModel> GetAllCourseStat(int? id)
        {
            List<CourseStatViewModel> courses = null;
            if (id == null)
            {
                courses = unitOfWork.Repository<Course>().GetAll()
                .Select(c => new CourseStatViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Code = c.Code,
                    IsDisable = c.IsDisable.HasValue && c.IsDisable.Value
                }).Where(c => c.IsDisable == false).ToList();
            }
            else
            {
                courses = unitOfWork.Repository<CourseOfUser>().GetAll()
                    .Where(c => c.UserId == id)
                    .Select(c => new CourseStatViewModel
                    {
                        Id = c.CourseId.Value,
                        Name = c.Course.Name,
                        Code = c.Course.Code,
                        IsDisable = c.Course.IsDisable.HasValue && c.Course.IsDisable.Value
                    }).ToList();
            }
            
            foreach (var course in courses)
            {
                var questions = unitOfWork.Repository<Question>().GetAll().Where(q => q.CourseId == course.Id);
                course.Easy = questions.Where(q => q.LevelId == (int)LevelEnum.Easy).Count();
                course.Medium = questions.Where(q => q.LevelId == (int)LevelEnum.Medium).Count();
                course.Hard = questions.Where(q => q.LevelId == (int)LevelEnum.Hard).Count();
            }
            return courses;
        }
        public CourseStatDetailViewModel GetCourseStatDetailByIdAndType(int id, string type)
        {
            var courseDetail = new CourseStatDetailViewModel();
            var questions = unitOfWork.Repository<Question>().GetAll();

            switch (type)
            {
                case "c":
                    var courseQuestions = questions.Where(q => q.CourseId == id);
                    courseDetail = new CourseStatDetailViewModel()
                    {
                        Type = "Course",
                        Easy = courseQuestions.Where(q => q.LevelId == (int)LevelEnum.Easy).Count(),
                        Medium = courseQuestions.Where(q => q.LevelId == (int)LevelEnum.Medium).Count(),
                        Hard = courseQuestions.Where(q => q.LevelId == (int)LevelEnum.Hard).Count()
                    };
                    break;
                case "lo":
                    var learningOutcomeQuestions = questions.Where(q => q.LearningOutcomeId == id);
                    courseDetail = new CourseStatDetailViewModel()
                    {
                        Type = "Learning Outcome",
                        Easy = learningOutcomeQuestions.Where(q => q.LevelId == (int)LevelEnum.Easy).Count(),
                        Medium = learningOutcomeQuestions.Where(q => q.LevelId == (int)LevelEnum.Medium).Count(),
                        Hard = learningOutcomeQuestions.Where(q => q.LevelId == (int)LevelEnum.Hard).Count()
                    };
                    break;
            }

            return WarnCourse(courseDetail);
        }
        private CourseStatDetailViewModel WarnCourse(CourseStatDetailViewModel detail)
        {
            switch (detail.Type)
            {
                case "Course":

                    break;
                case "Learning Outcome":

                    break;
            }
            return detail;
        }
        public bool UpdateCourse(CourseViewModel course)
        {
            try
            {
                var entity = unitOfWork.Repository<Course>().GetById(course.Id);
                entity.Code = course.Code;
                entity.Name = course.Name;
                unitOfWork.SaveChanges();
                return true;
            }
            catch (System.Exception)
            {

                return false;
            }
        }
    }
}
