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
        GenerateExamViewModel GenerateExamination(GenerateExamViewModel exam, string fullname = "", string usercode = "");
        List<ExaminationViewModel> GetAllExam();
        List<ExaminationViewModel> GetExamByExamGroup(string groupExam);
        ExaminationViewModel GetExanById(int examId);
        string GetExamCode();
        void DisableEaxam(int examId);
        List<QuestionInExamViewModel> GetExaminationHistoryQuestionsInCourse(int courseId);
        string GetExamGroup();
        void ResetPriorityAndFrequency(string groupExam);
        string ReplaceQuestionInExam(int questionId, string fullname, string usercode);
        GenerateExamViewModel SaveQuestionsToExam(List<string> questionCode, int courseId, int semeterId);
    }
}
