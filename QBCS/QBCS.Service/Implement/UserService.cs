using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Enum;
using QBCS.Service.Interface;
using QBCS.Service.Utilities;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{
    public class UserService : IUserService
    {
        private IUnitOfWork unitOfWork;

        public UserService()
        {
            unitOfWork = new UnitOfWork();
        }

        public UserViewModel Login(string username, string password)
        {

            var user = unitOfWork.Repository<User>().GetAll()
                                                    .Where(u => u.Username.ToLower().Equals(username.ToLower())
                                                                        && u.Password.ToLower().Equals(password.ToLower())
                                                                        && !u.IsDisable.Value)
                                                    .FirstOrDefault();
            if (user != null)
            {
                return new UserViewModel
                {
                    Id = user.Id,
                    Code = user.Code,
                    Fullname = user.Fullname,
                    Role = (RoleEnum) user.RoleId,
                    Courses = user.CourseOfUsers.Select(uc => new CourseViewModel {
                        CourseId = uc.CourseId.Value,
                        Code = uc.Course.Code,
                        Name = uc.Course.Name
                    }).ToList()
                };
            }

            return null;
        }

        

        public bool AddUser(UserViewModel user)
        {
            bool result = false;
            Role insertRole = unitOfWork.Repository<Role>().GetAll().Where(r => r.Name == user.Role.ToString()).ToList().FirstOrDefault();

            var checkUser = unitOfWork.Repository<User>().GetAll()
                                                    .Where(u => u.Username.ToLower().Equals(user.Username.ToLower()))
                                                    .FirstOrDefault();

            if (checkUser == null)
            {
                var entity = new User()
                {
                    Code = user.Code,
                    Fullname = user.Fullname,
                    IsDisable = false,
                    Email = user.Email,
                    Username = user.Username,
                    Password = user.Password,
                    RoleId = insertRole.Id
                };

                unitOfWork.Repository<User>().Insert(entity);
                unitOfWork.SaveChanges();
                result = true;
            }

            return result;
        }

        public bool UpdateUserInfo(UserViewModel user)
        {
            
            if (user != null)
            {
                var userEntity = unitOfWork.Repository<User>().GetById(user.Id);
                if (userEntity != null)
                {
                    userEntity.Fullname = user.Fullname;
                    userEntity.Code = user.Code;
                    userEntity.Email = user.Email;

                    unitOfWork.Repository<User>().Update(userEntity);
                    unitOfWork.SaveChanges();
                    return true;
                }
            }

            return false;
        }
        public bool AddUserCourse(int courseId, int userId)
        {
            var user = unitOfWork.Repository<User>().GetById(userId);
            var course = unitOfWork.Repository<Course>().GetById(courseId);

            if (!user.IsDisable.Value && !course.IsDisable.Value)
            {
                var userCourse = new CourseOfUser
                {
                    UserId = userId,
                    CourseId = courseId
                };

                unitOfWork.Repository<CourseOfUser>().Insert(userCourse);
                unitOfWork.SaveChanges();
                return true;
            }

            return false;
        }

        public bool RemoveUserCourse(int courseId, int userId)
        {
            var userCourse = unitOfWork.Repository<CourseOfUser>().GetAll().Where(uc => uc.UserId == userId && uc.CourseId == courseId).FirstOrDefault();
            if (userCourse != null)
            {
                unitOfWork.Repository<CourseOfUser>().Delete(userCourse);
                unitOfWork.SaveChanges();
                return true;
            }

            return false;
        }

        public bool DisableUser(int userId)
        {
            var user = unitOfWork.Repository<User>().GetById(userId);
            if (user != null && !user.IsDisable.Value)
            {
                user.IsDisable = true;
                unitOfWork.Repository<User>().Update(user);
                unitOfWork.SaveChanges();
                return true;
            }

            return false;
        }

        public bool EnableUser(int userId)
        {
            var user = unitOfWork.Repository<User>().GetById(userId);
            if (user != null && user.IsDisable.Value)
            {
                user.IsDisable = false;
                unitOfWork.Repository<User>().Update(user);
                unitOfWork.SaveChanges();
                return true;
            }

            return false;
        }

        public List<UserViewModel> GetAllUser()
        {
            var list = unitOfWork.Repository<User>().GetAll().Select(c => new UserViewModel
            {
                Id = c.Id,
                Code = c.Code,
                Fullname = c.Fullname,
                Username = c.Username,
                Password = c.Password,
                Role = (RoleEnum)c.RoleId,
                Email = c.Email,
                IsDisable = c.IsDisable.Value
            });

            return list.ToList();
        }
        public UserViewModel GetUserById(int id)
        {
            var user = unitOfWork.Repository<User>().GetById(id);
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                Code = user.Code,
                Fullname = user.Fullname,
                Username = user.Username,
                Password = user.Password,
                Role = (RoleEnum)user.RoleId,
                Email = user.Email,
                Courses = unitOfWork.Repository<CourseOfUser>().GetAll().Where(uc => uc.UserId == id).Select(uc => new CourseViewModel
                {
                    Id = uc.Course.Id,
                    Name = uc.Course.Name,
                    Code = uc.Course.Code
                }).ToList()
            };
            return userViewModel;
        }
        public List<UserViewModel> GetUserByNameAndRoleId(string name, int id)
        {
            name = name.ToLower();
            var list = unitOfWork.Repository<User>().GetAll().Where(u => u.RoleId == id).Select(c => new UserViewModel
            {
                Id = c.Id,
                Code = c.Code,
                Fullname = c.Fullname,
                Username = c.Username,
                Password = c.Password,
                Role = (RoleEnum)c.RoleId,
                Email = c.Email,
                IsDisable = c.IsDisable.Value
            }).ToList();
            var result = new List<UserViewModel>();
            foreach(var u in list)
            {
                var fullname = VietnameseToEnglish.SwitchCharFromVietnameseToEnglish(u.Fullname).ToLower();
                var code = VietnameseToEnglish.SwitchCharFromVietnameseToEnglish(u.Code).ToLower();
                if (fullname.Contains(name) || code.Contains(name) || u.Fullname.ToLower().Contains(name) || u.Code.ToLower().Contains(name))
                {
                    result.Add(u);
                }
            }
            return result;
        }

        public UserViewModel GetUser(string code)
        {
            var userViewModel = unitOfWork.Repository<User>()
                             .GetAll()
                             .Where(u => u.Code.ToLower().Equals(code.ToLower()))
                             .Select(u => new UserViewModel
                             {
                                 Id = u.Id,
                                 Code = u.Code,
                                 Fullname = u.Fullname
                                
                             })
                             .FirstOrDefault();
            if (userViewModel != null)
            {
                userViewModel.Courses = unitOfWork.Repository<CourseOfUser>().GetAll().Where(uc => uc.UserId == userViewModel.Id).Select(uc => new CourseViewModel
                {
                    CourseId = uc.Course.Id,
                    Name = uc.Course.Name,
                    Code = uc.Course.Code
                }).ToList();
            }
            return userViewModel;
        }
    }
}
