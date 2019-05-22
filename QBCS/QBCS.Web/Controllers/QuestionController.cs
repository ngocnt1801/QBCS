using QBCS.Entity;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Web.Models;
using System.Collections.Generic;
using System.Web.Mvc;



namespace QBCS.Web.Controllers
{
    public class QuestionController : Controller
    {
        private IQuestionService questionService;
        private IOptionService optionService;
        private ITopicService topicService;
        private ILevelService levelService;
        private ILearningOutcomeService lo;

        public QuestionController()
        {
            questionService = new QuestionService();
            optionService = new OptionService();
            topicService = new TopicService();
            levelService = new LevelService();
            lo = new LearningOutcomeService();
        }

        // GET: Question
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(QBCS.Service.ViewModel.QuestionViewModel model)
        {
            questionService.Add(model);

            return RedirectToAction("AddQuestion", new { courseId = model.CourseId});
        }

        // GET: Question
        public ActionResult GetListQuestion(int id)
        {
            List<QuestionViewModel> ListQuestion = new List<QuestionViewModel>();

            List<Question> Questions = questionService.GetQuestionsByCourse(id);
            foreach (Question ques in Questions )
            {
                List<Option> op = optionService.GetOptionsByQuestion(ques.Id);
                QuestionViewModel qvm = new QuestionViewModel
                {
                    Question = ques,
                    Options = op
                };
                ListQuestion.Add(qvm);
            }
            return View("ListQuestion", ListQuestion);
        }

        public ActionResult AddQuestion(int courseId)
        {
            var question = new QBCS.Service.ViewModel.QuestionViewModel();
            question.CourseId = courseId;
            return View(question);
        }

        public ActionResult GetQuestionDetail (int id)
        {
            Question ques = questionService.GetQuestionById(id);
            List<Option> op = optionService.GetOptionsByQuestion(ques.Id);
            QuestionViewModel qvm = new QuestionViewModel
            {
                Question = ques,
                Options = op
            };
            List<Topic> topics = topicService.GetTopicByCourseId(ques.CourseId);

            List<Level> levels = levelService.GetLevelByCourse(ques.CourseId);

            List<LearningOutcome> learningOutcomes = lo.GetLearningOutcomeByCourseId(ques.CourseId);

            QuestionDetailViewModel qdvm = new QuestionDetailViewModel()
            {
                QuestionViewModel = qvm,
                Topics = topics,
                Levels = levels,
                LearningOutcomes = learningOutcomes
            };

            return View("EditQuestion", qdvm);
        }

        public ActionResult UpdateQuestion(Question ques)
        {
            bool result = questionService.UpdateQuestion(ques);

            return RedirectToAction("GetQuestionDetail", new {id = ques.Id });
        }

    }
}