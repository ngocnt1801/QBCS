using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ExaminationController()
        {
            topicService = new TopicService();
            learningOutcomeService = new LearningOutcomeService();
            examinationService = new ExaminationService();
            partOfExamService = new PartOfExamService();
        }
        public ActionResult GenerateExam(int courseId)
        {
            List<TopicViewModel> topicViewModels = topicService.GetTopicByCourseId(courseId);
            List<LearningOutcomeViewModel> learningOutcomeViewModels = learningOutcomeService.GetLearningOutcomeByCourseId(courseId);
            ListTopicLearningOutcomeViewModel listTopicLearningOutcomeViewModel = new ListTopicLearningOutcomeViewModel()
            {
                Topics = topicViewModels,
                LearningOutcomes = learningOutcomeViewModels
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