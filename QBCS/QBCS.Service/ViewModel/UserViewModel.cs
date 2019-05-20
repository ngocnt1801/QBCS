using QBCS.Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Code { get; set; }
        public RoleEnum Role { get; set; }
    }
}
