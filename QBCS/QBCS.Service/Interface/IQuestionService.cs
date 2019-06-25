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
        //List<Question> GetQuestionsByContent(string questionContent);
        //List<QuestionViewModel> GetQuestionPaging(string searchInput, int take, int skip);
        //List<Question> GetQuestionSearchBar(string searchInput, int take, int skip);
        bool UpdateQuestion(QuestionViewModel question);
        QuestionViewModel GetQuestionById(int id);//Return the QuestionViewModel
        List<QuestionViewModel> GetQuestionByQuestionId(int questionId); //Return the list of QuestionViewModel
        List<QuestionViewModel> GetQuestionsByCourse(int CourseId);
        List<QuestionViewModel> GetAllQuestionByCourseId(int courseId); //Change Model
        List<QuestionViewModel> GetAllQuestions();
        List<QuestionViewModel> CheckDuplicated();
        int GetMinFreQuencyByTopicAndLevel(int topicId, int levelId);
        int GetMinFreQuencyByLearningOutcome(int learningOutcomeId, int levelId);
        bool InsertQuestion(HttpPostedFileBase questionFile, int userId, int courseId, bool checkCate, bool checkHTML);
        List<QuestionViewModel> GetQuestionList(int? courseId, int? categoryId, int? learningoutcomeId, int? topicId, int? levelId);
        void ToggleDisable(int id);
        void UpdateCategory(int[] questionIds, int? categoyrId, int? learningOutcomeId, int? levelId);
    }
}
