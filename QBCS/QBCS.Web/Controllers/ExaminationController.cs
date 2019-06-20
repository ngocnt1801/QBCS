using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
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

    public class ExaminationController : Controller
    {
        private ITopicService topicService;
        private ILearningOutcomeService learningOutcomeService;
        private IExaminationService examinationService;
        private IPartOfExamService partOfExamService;
        private ICategoryService categoryService;
        public ExaminationController()
        {
            topicService = new TopicService();
            learningOutcomeService = new LearningOutcomeService();
            examinationService = new ExaminationService();
            partOfExamService = new PartOfExamService();
            categoryService = new CategoryService();
        }
        public ActionResult GenerateExam(int courseId)
        {
            List<TopicViewModel> topicViewModels = topicService.GetTopicByCourseId(courseId);
            List<LearningOutcomeViewModel> learningOutcomeViewModels = learningOutcomeService.GetLearningOutcomeByCourseId(courseId);
            List<CategoryViewModel> categoryViewModels = categoryService.GetCategoriesByCourseId(courseId);
            ListTopicLearningOutcomeViewModel listTopicLearningOutcomeViewModel = new ListTopicLearningOutcomeViewModel()
            {
                Topics = topicViewModels,
                LearningOutcomes = learningOutcomeViewModels,
                Categories = categoryViewModels
            };
            return View(listTopicLearningOutcomeViewModel);
        }
        public ActionResult GenerateExaminaton(GenerateExamViewModel exam)
        {
            GenerateExamViewModel examination = examinationService.GenerateExamination(exam);
            return View(examination);
        }

        public ActionResult ViewGeneratedExamination(int examinationId)
        {
            List<PartOfExamViewModel> partOfExams = partOfExamService.GetPartOfExamByExamId(examinationId);
            return View(partOfExams);
        }
    }
}