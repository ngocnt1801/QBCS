﻿using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface IUserService
    {
        List<UserViewModel> GetAllUser();
        UserViewModel GetUserById(int id);
    }
}
