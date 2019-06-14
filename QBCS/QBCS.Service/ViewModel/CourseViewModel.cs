using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DepartmentId { get; set; }
        public int DefaultNumberOfQuestion { get; set; }
        public List<TopicViewModel> Topic { get; set; }
        public List<LearningOutcomeViewModel> LearningOutcome { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public bool IsDisable { get; set; }

    }
}
