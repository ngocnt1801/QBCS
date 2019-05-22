using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class UserDetailViewModel
    {
        public UserViewModel User { get; set; }
        public List<CourseViewModel> AvailableToAddCourses { get; set; }
    }
}
