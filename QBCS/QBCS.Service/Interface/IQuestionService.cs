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
        //List<Question> GetQuestionsByContent(string questionContent);
        List<QuestionViewModel> GetQuestionPaging(string searchInput, int take, int skip);
        //List<Question> GetQuestionSearchBar(string searchInput, int take, int skip);
        bool UpdateQuestion(QuestionViewModel question);
        QuestionViewModel GetQuestionById(int id);
        List<QuestionViewModel> GetQuestionsByCourse(int CourseId);
        List<QuestionViewModel> GetAllQuestionByCourseId(int courseId); //Change Model
        List<QuestionViewModel> GetAllQuestions();
        List<QuestionViewModel> CheckDuplicated();
    }
}
