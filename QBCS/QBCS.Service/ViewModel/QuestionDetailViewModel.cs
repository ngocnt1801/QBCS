using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class QuestionDetailViewModel
    {
        public QuestionViewModel Question { get; set; }

        public List<LevelViewModel> Levels { get; set; }

        public List<LearningOutcomeViewModel> LearningOutcomes { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}
