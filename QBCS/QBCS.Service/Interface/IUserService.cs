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
    }
}
