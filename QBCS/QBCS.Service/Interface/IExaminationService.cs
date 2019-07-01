using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface IExaminationService
    {
        GenerateExamViewModel GenerateExamination(GenerateExamViewModel exam);
        List<ExaminationViewModel> GetAllExam();
        List<ExaminationViewModel> GetExamByExamGroup(string groupExam);
        ExaminationViewModel GetExanById(int examId);
        string GetExamCode();
        void DisableEaxam(int examId);
        List<QuestionInExamViewModel> GetExaminationHistoryQuestionsInCourse(int courseId);
    }
}
