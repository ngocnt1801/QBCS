using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class SyllabusPartialViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public int CourseId { get; set; }
    }
}
