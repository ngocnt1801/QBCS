using QBCS.Entity;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Interface
{
    public interface IQuestionService
    {
        bool Add(QuestionViewModel question);
        List<QuestionViewModel> GetQuestionsByCourse(int CourseId);

        QuestionViewModel GetQuestionById(int id);

        bool UpdateQuestion(QuestionViewModel question);
    }
}
