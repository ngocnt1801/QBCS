using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class GenerateExamViewModel
    {
        private const double ORDINARY_STUDENT_EASY_PERCENT = 1;
        private const double ORDINARY_STUDENT_MEDIUM_PERCENT = 0.3;
        private const double ORDINARY_STUDENT_HARD_PERCENT = 0;

        private const double GOOD_STUDENT_EASY_PERCENT = 1;
        private const double GOOD_STUDENT_MEDIUM_PERCENT = 0.7;
        private const double GOOD_STUDENT_HARD_PERCENT = 0.3;

        private const double EXCELLENT_STUDENT_EASY_PERCENT = 1;
        private const double EXCELLENT_STUDENT_MEDIUM_PERCENT = 1;
        private const double EXCELLENT_STUDENT_HARD_PERCENT = 0.6;
        public int ExamId { get; set; }
        public int TotalQuestion { get; set; }
        public int EasyPercent { get; set; }
        public int MediumPercent { get; set; }
        public int HardPercent { get; set; }
        public List<string> Topic { get; set; }
        public int EasyQuestion { get; set; }
        public int MediumQuestion { get; set; }
        public int HardQuestion { get; set; }
        public int EasyQuestionGenerrate { get; set; }
        public int MediumQuestionGenerrate { get; set; }
        public int HardQuestionGenerrate { get; set; }
        public int TotalQuestionGenerrate { get; set; }
        public int CategoryId { get; set; }
        public int OrdinaryGrade { get; set; }
        public int GoodGrade { get; set; }
        public int ExcellentGrade { get; set; }
        public string FlagPercent { get; set; }
        public int OrdinaryGradeCalculate { get; set; }
        public int GoodGradeCalculate { get; set; }
        public int ExcellentGradeCalculate { get; set; }
        public void calculateGrade()
        {
            OrdinaryGradeCalculate = (int)Math.Round(((EasyQuestion * 1.0) / TotalQuestion) * 100);
            GoodGradeCalculate = (int)Math.Round(((MediumQuestion * 1.0) / TotalQuestion) * 100) + OrdinaryGradeCalculate;
            ExcellentGradeCalculate = (int)Math.Round(((HardQuestion * 0.5) / TotalQuestion) * 100) + GoodGradeCalculate;
            //double gradePerQuestion = (1 * 1.0) / TotalQuestion;
            //int easyQuestionByPercent = (int)Math.Round(EasyQuestion * ORDINARY_STUDENT_EASY_PERCENT);
            //int mediumQuestionByPercent = (int)Math.Round(MediumQuestion * ORDINARY_STUDENT_MEDIUM_PERCENT);
            //int hardQuestionByPercent = (int)Math.Round(HardQuestion * ORDINARY_STUDENT_HARD_PERCENT);
            //OrdinaryGradeCalculate = (int)Math.Round((easyQuestionByPercent + mediumQuestionByPercent + hardQuestionByPercent) * gradePerQuestion * 100);
            //easyQuestionByPercent = (int)Math.Round(EasyQuestion * GOOD_STUDENT_EASY_PERCENT);
            //mediumQuestionByPercent = (int)Math.Round(MediumQuestion * GOOD_STUDENT_MEDIUM_PERCENT);
            //hardQuestionByPercent = (int)Math.Round(HardQuestion * GOOD_STUDENT_HARD_PERCENT);
            //GoodGradeCalculate = (int)Math.Round((easyQuestionByPercent + mediumQuestionByPercent + hardQuestionByPercent) * gradePerQuestion * 100);
            //easyQuestionByPercent = (int)Math.Round(EasyQuestion * EXCELLENT_STUDENT_EASY_PERCENT);
            //mediumQuestionByPercent = (int)Math.Round(MediumQuestion * EXCELLENT_STUDENT_MEDIUM_PERCENT);
            //hardQuestionByPercent = (int)Math.Round(HardQuestion * EXCELLENT_STUDENT_HARD_PERCENT);
            //ExcellentGradeCalculate = (int)Math.Round((easyQuestionByPercent + mediumQuestionByPercent + hardQuestionByPercent) * gradePerQuestion * 100);

        }
    }
}
