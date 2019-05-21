﻿using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QBCS.Service.ViewModel;
using QBCS.Repository.Interface;
using QBCS.Repository.Implement;
using QBCS.Entity;

namespace QBCS.Service.Implement
{
    public class UserService : IUserService
    {
        private IUnitOfWork unitOfWork;
        public UserService()
        {
            unitOfWork = new UnitOfWork();
        }
        public List<UserViewModel> GetAllUser()
        {
            var list = unitOfWork.Repository<User>().GetAll().Select(c => new UserViewModel
            {
                Id = c.Id,
                UserCode = c.Code,              
                Fullname = c.Fullname,
                Username = c.Username,
                Password = c.Password,
                Role = c.RoleId.ToString(),
                Email = c.Email
            });
            
            return list.ToList();
        }
        public UserViewModel GetUserById(int id)
        {
            var user = unitOfWork.Repository<User>().GetById(id);
            var userViewModel = new UserViewModel
            {
                Id = user.Id,
                UserCode = user.Code,
                Fullname = user.Fullname,
                Username = user.Username,
                Password = user.Password,
                Role = user.RoleId.ToString(),
                Email = user.Email
            }; 
            return userViewModel;
        }
    }
}
