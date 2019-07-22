using AuthLib.Module;
using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{

    [CheckSession]
    public class ExaminationController : Controller
    {
        private ITopicService topicService;
        private ILearningOutcomeService learningOutcomeService;
        private IExaminationService examinationService;
        private IPartOfExamService partOfExamService;
        private ICategoryService categoryService;
        private ISemesterService semesterService;
        private ILogService logService;
        private ICourseService courseService;
        private IQuestionService questionService;
        public ExaminationController()
        {
            topicService = new TopicService();
            learningOutcomeService = new LearningOutcomeService();
            examinationService = new ExaminationService();
            partOfExamService = new PartOfExamService();
            categoryService = new CategoryService();
            semesterService = new SemesterService();
            logService = new LogService();
            courseService = new CourseService();
            questionService = new QuestionService();
        }
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Config to Generate", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Examination", Message = "Generate Examination", Method = "GET")]
        public ActionResult GenerateExam(int courseId)
        {
            List<LearningOutcomeViewModel> learningOutcomeViewModels = learningOutcomeService.GetLearningOutcomeByCourseId(courseId);
            List<CategoryViewModel> categoryViewModels = categoryService.GetCategoriesByCourseId(courseId);
            List<SemesterViewModel> semester = semesterService.GetAllSemester();
            ListLearningOutcomeViewModel listTopicLearningOutcomeViewModel = new ListLearningOutcomeViewModel()
            {
                LearningOutcomes = learningOutcomeViewModels,
                Categories = categoryViewModels,
                Semester = semester, 
                CourseId = courseId
            };
            TempData["active"] = "Examination";
            return View(listTopicLearningOutcomeViewModel);
        }
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Generate Examination", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Examination", Message = "Generate Examination", Method = "GET")]
        public ActionResult GenerateExaminaton(GenerateExamViewModel exam)
        {
            GenerateExamViewModel examination = examinationService.GenerateExamination(exam, fullname: User.Get(u => u.FullName), usercode: User.Get(u => u.Code));     
            TempData["active"] = "Examination";
            return View(examination);
        }

        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Review Examination", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Examination", Message = "View Generate Examination", Method = "GET")]
        public ActionResult ViewGeneratedExamination(string examGroup)
        {
            List<ExaminationViewModel> exams = examinationService.GetExamByExamGroup(examGroup);
            TempData["active"] = "Examination";
            return View(exams);
        }
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "All Examinations", "QBCS", protectType: ProtectType.Authorized, ShortName = "Examination", InternalId = (int)SideBarEnum.AllExamination)]
        [LogAction(Action = "Examination", Message = "View All Examination", Method = "GET")]
        public ActionResult GetAllExamination()
        {
            List<ExaminationViewModel> exams = examinationService.GetAllExam();
            TempData["active"] = "Examination";
            return View("ListExamination",exams);
        }
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Examination Detail", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Examination", Message = "View Detail Examination", Method = "GET")]
        public ActionResult DetailExam(int examId)
        {
            ExaminationViewModel exam = examinationService.GetExanById(examId);
            TempData["active"] = "Examination";
            return View(exam);
        }
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Disable Examination", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Examination", Message = "Disable Examination", Method = "GET")]
        public ActionResult DisableExam(int examId)
        {
            examinationService.DisableEaxam(examId);
            logService.LogManually("Disable", "Examination", targetId: examId, controller: "Examination", method: "DisableExam", fullname: User.Get(u => u.FullName), usercode: User.Get(u => u.Code));
            return RedirectToAction("GetAllExamination", "Examination");
        }
        //Staff

        //stpm: feature declare
        [Feature(FeatureType.Page, "Get Aging Question", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Examination", Message = "Get History Course in Examination", Method = "GET")]
        public ActionResult GetHistoryCourse(int courseId)
        {
            var listQuestion = examinationService.GetExaminationHistoryQuestionsInCourse(courseId);
            TempData["active"] = "Course";
            return View(listQuestion);
        }

        //[Feature(FeatureType.Page, "Back To Generate Exam View", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Examination", Message = "Generate Examination", Method = "GET")]
        public ActionResult BackToGenerate(int courseId, string groupExam)
        {
            TempData["active"] = "Examination";
            examinationService.ResetPriorityAndFrequency(groupExam);
            return RedirectToAction("GenerateExam", "Examination",  new { courseId = courseId });
        }

        //[Feature(FeatureType.Page, "Replace Question In Exam", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult DeleteQuestionInExam(int questionId)
        {
            string groupExam = examinationService.ReplaceQuestionInExam(questionId, fullname: User.Get(u => u.FullName), usercode: User.Get(u => u.Code));
            return RedirectToAction("ViewGeneratedExamination", "Examination", new { examGroup  = groupExam});
        }
        public ActionResult CreateExamManually(int courseId)
        {
            List<CategoryViewModel> categories = categoryService.GetListCategories(courseId);
            List<SemesterViewModel> semesters = semesterService.GetAllSemester();
            var model = courseService.GetCourseById(courseId);
            model.Categories = categories;
            model.Semester = semesters;
            return View(model);
        }
        public ActionResult GetQuestions(int? courseId, int? categoryId, int? learningoutcomeId, int? topicId, int? levelId)
        {
            var result = questionService.GetQuestionList(courseId, categoryId, learningoutcomeId, topicId, levelId);
            return PartialView("ListQuestionCreateExamManually", result);
        }
        public ActionResult SaveQuestionToExam(List<string> questionCode, int courseId, int semeterId)
        {
            GenerateExamViewModel exam = examinationService.SaveQuestionsToExam(questionCode, courseId, semeterId);
            return View("GenerateExaminaton", exam);
        }

    }
}