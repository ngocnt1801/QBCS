using QBCS.Entity;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QBCS.Service.Interface
{
    public interface IQuestionService
    {
        bool Add(QuestionViewModel question);
        List<Question> GetQuestionsByContent(string questionContent);
        List<Question> GetQuestionSearchBar(string searchInput);
        bool UpdateQuestion(QuestionViewModel question);
        QuestionViewModel GetQuestionById(int id);//Return the QuestionViewModel
        List<QuestionViewModel> GetQuestionByQuestionId(int questionId); //Return the list of QuestionViewModel
        List<QuestionViewModel> GetQuestionsByCourse(int CourseId);
        List<QuestionViewModel> GetAllQuestionByCourseId(int courseId); //Change Model
        List<QuestionViewModel> GetAllQuestions();
        List<QuestionViewModel> CheckDuplicated();
        int GetMinFreQuencyByTopicAndLevel(int topicId, int levelId);
        int GetMinFreQuencyByLearningOutcome(int learningOutcomeId, int levelId);
        bool InsertQuestion(HttpPostedFileBase questionFile, int userId, int courseId);
    }
}
