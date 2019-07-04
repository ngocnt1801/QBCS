using QBCS.Service.Enum;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface IUserService
    {
        UserViewModel Login(String username, String password);
        bool AddUser(UserViewModel user);
        bool UpdateUserInfo(UserViewModel user);
        bool DisableUser(int userId);
        bool EnableUser(int userId);
        bool AddUserCourse(int courseId, int userId);
        bool RemoveUserCourse(int courseId, int userId);
        List<UserViewModel> GetAllUser();
        UserViewModel GetUserById(int id);
        List<UserViewModel> GetUserByNameAndRoleId(string name, int id);

        UserViewModel GetUser(string code);
    }
}
