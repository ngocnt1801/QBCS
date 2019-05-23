using QBCS.Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QBCS.Web.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Code { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public RoleEnum Role { get; set; }
        public string Email { get; set; }
    }
}