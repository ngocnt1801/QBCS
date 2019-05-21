using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Enum;
using QBCS.Service.Interface;
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
                                                                        && u.Password.ToLower().Equals(password.ToLower()))
                                                    .FirstOrDefault();
            if (user != null)
            {
                return new UserViewModel
                {
                    Id = user.Id,
                    Code = user.Code,
                    Fullname = user.Fullname,
                    Role = (RoleEnum) user.RoleId
                };
            }

            return null;
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
    }
}
