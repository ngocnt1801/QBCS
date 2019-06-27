using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class CategoryViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public List<LearningOutcomeViewModel> LearningOutcomes { get; set; }
        public int QuestionCount { get; set; }
    }
}
