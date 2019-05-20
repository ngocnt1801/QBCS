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
    }
}
