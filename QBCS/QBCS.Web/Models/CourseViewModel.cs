using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QBCS.Web.Models
{
    public class CourseViewModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int DepartmentId { get; set; }
        public int? DefaultNumberOfQuestion { get; set; }
    }
}