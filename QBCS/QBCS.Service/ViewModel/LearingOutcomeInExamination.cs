using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    class LearingOutcomeInExamination
    {
        public int Id { get; set; }
        public int EasyQuestion { get; set; }
        public int MediumQuestion { get; set; }
        public int HardQuestion { get; set; }
        public int TotalEasyQuestionInTopic { get; set; }
        public int TotalMediumQuestionInTopic { get; set; }
        public int TotalHardQuestionInTopic { get; set; }
        public LearingOutcomeInExamination()
        {
            EasyQuestion = 0;
            MediumQuestion = 0;
            HardQuestion = 0;
        }
    }
}
