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
        public int AmountQuestion { get; set; }
        public List<LearningOutcomeViewModel> LearingOutcomes { get; set; }
        public List<LearingOutcomeInExamination> LearingOutcomesInExam { get; set; }
        public int CourseId { get; set; }
    }
}
