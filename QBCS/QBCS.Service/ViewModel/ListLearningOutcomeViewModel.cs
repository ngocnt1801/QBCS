using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class ListLearningOutcomeViewModel
    {
        public List<LearningOutcomeViewModel> LearningOutcomes { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public List<SemesterViewModel> Semester { get; set; }
    }
}
