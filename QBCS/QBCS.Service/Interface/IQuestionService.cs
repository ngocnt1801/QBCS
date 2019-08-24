﻿using QBCS.Entity;
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
        bool UpdateQuestion(QuestionViewModel question);
        void UpdateQuestionWithTextbox(QuestionViewModel question);
        QuestionViewModel GetQuestionById(int id);//Return the QuestionViewModel
        List<QuestionViewModel> GetQuestionByQuestionId(int questionId); //Return the list of QuestionViewModel
        List<QuestionViewModel> GetQuestionsByCourse(int CourseId);
        List<QuestionViewModel> GetAllQuestionByCourseId(int courseId); //Change Model
        List<QuestionViewModel> GetAllQuestions();
        List<QuestionViewModel> CheckDuplicated();        
        int GetMinFreQuencyByLearningOutcome(int learningOutcomeId, int levelId);
        int InsertQuestion(HttpPostedFileBase questionFile, int userId, int courseId, bool checkCate, bool checkHTML,int ownerId, string ownerName, string prefix="", bool ccheckSemantic = true);
        List<QuestionViewModel> GetQuestionList(int? courseId, int? categoryId, int? learningoutcomeId, int? topicId, int? levelId);
        GetQuestionsDatatableViewModel GetQuestionList(int? courseId, int? categoryId, int? learningoutcomeId, int? topicId, int? levelId, string search, int start, int length);
        void ToggleDisable(int id);
        void UpdateCategory(int[] questionIds, int? categoyrId, int? learningOutcomeId, int? levelId);
        bool InsertQuestionWithTableString(string table, int userId, int courseId,string prefix, string ownerName, bool checkSemantic = false);
        int GetCountOfListQuestionByLearningOutcomeAndId(int learningOutcomeId, int levelId);
        QuestionHistoryViewModel GetQuestionHistory(int id);
        List<QuestionTmpModel> TableStringToListQuestion(string table, string prefix);
        QuestionViewModel GetQuestionByQuestionCode(string questionCode);
        GetResultQuestionTempViewModel GetQuestionTempByImportId(int importId, string type, string search, int start, int length);
        GetResultQuestionTempViewModel GetQuestionByCourseId(int courseId, string type, string search, int start, int length);
        void CheckImageInQuestion(List<QuestionTemp> tempQuestions);
        QuestionTempViewModel GetQuestionTempById(int id);
        void UpdateQuestionStatus(int questionId, int status);
        QuestionTempViewModel GetDuplicatedDetail(int questionId);


    }
}
