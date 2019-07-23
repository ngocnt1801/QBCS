using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class GetQuestionsDatatableViewModel
    {
        public List<QuestionViewModel> Questions { get; set; }
        public int totalCount { get; set; }
        public int filteredCount { get; set; }
    }
}
