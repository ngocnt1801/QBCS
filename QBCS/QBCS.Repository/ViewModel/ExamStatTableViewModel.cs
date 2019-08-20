using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Repository.ViewModel
{
    public class ExamStatTableViewModel
    {
        public int QuestionId { get; set; }
        public string GroupExam { get; set; }
        public string QuestionCode { get; set; }
        public int TotalNumber { get; set; }
    }
}
