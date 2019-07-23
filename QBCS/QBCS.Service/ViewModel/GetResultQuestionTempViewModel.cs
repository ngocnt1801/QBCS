using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class GetResultQuestionTempViewModel
    {
        public List<QuestionTempViewModel> Questions { get; set; }
        public int totalCount { get; set; }
    }
}
