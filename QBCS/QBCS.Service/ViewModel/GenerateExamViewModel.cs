using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.ViewModel
{
    public class GenerateExamViewModel
    {
        public int CourseId { get; set; }
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
        public int TotalExam { get; set; }
        public bool IsEnough { get; set; }
        public bool IsManually { get; set; }
        public bool IsSyllabusHaveLO { get; set; }
        public string GroupExam { get; set; }
        public List<LearingOutcomeInExamination> LearningOutcomeInExam { get; set; }
        public List<SyllabusPartialViewModel> Syllabus { get; set; }
        public void CalculateGrade()
        {
            if (TotalQuestion == 0)
            {
                OrdinaryGradeCalculate = 0;
                OrdinaryGrade = 0;
                ExcellentGradeCalculate = 0;
                return;
            }
            if (EasyQuestionGenerrate == 0)
            {
                OrdinaryGradeCalculate = 0;
            }
            else
            {
                OrdinaryGradeCalculate = (int)Math.Round(((EasyQuestionGenerrate * 1.0) / TotalQuestion) * 100);
            }

            if (MediumQuestionGenerrate == 0)
            {
                GoodGradeCalculate = 0 + OrdinaryGradeCalculate;
            }
            else
            {
                GoodGradeCalculate = (int)Math.Round(((MediumQuestionGenerrate * 0.6) / TotalQuestion) * 100) + OrdinaryGradeCalculate;
            }
            ExcellentGradeCalculate = (int)Math.Round(((MediumQuestionGenerrate * 1.0) / TotalQuestion) * 100) + OrdinaryGradeCalculate;

        }
    }
}
