using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class ExaminationViewModel
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public CourseViewModel Course { get; set; }
        public DateTime GeneratedDate { get; set; }
        public int NumberOfEasy { get; set; }
        public int NumberOfMedium { get; set; }
        public int NumberOfHard { get; set; }
        public string ExamCode { get; set; }
        public List<PartOfExamViewModel> PartOfExam { get; set; }        
    }
}
