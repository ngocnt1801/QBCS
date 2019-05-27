using QBCS.Entity;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
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
        public ActionResult Add(QuestionViewModel model)
        {
            questionService.Add(model);

            return RedirectToAction("AddQuestion", new { courseId = model.CourseId});
        }

        // GET: Question
        public ActionResult GetListQuestion(int id)
        {
            List<QuestionViewModel> ListQuestion = questionService.GetQuestionsByCourse(id);


            
            return View("ListQuestion", ListQuestion);
        }

        public ActionResult AddQuestion(int courseId)
        {
            var question = new QuestionViewModel();
            question.CourseId = courseId;
            return View(question);
        }

        // GET: Question
        //public ActionResult GetQuestionsByContent(string content)
        //{
        //    List<QuestionViewModel> result = new List<QuestionViewModel>();

        //    List<Question> questions = questionService.GetQuestionsByContent(content);
        //    foreach (Question question in questions)
        //    {
        //        List<Option> op = optionService.GetOptionsByQuestion(question.Id);
        //        QuestionViewModel questionViewModel = new QuestionViewModel
        //        {
        //            Question = question,
        //            Options = op
        //        };
        //        result.Add(questionViewModel);
        //    }
        //    return View("ListQuestion", result);
        //}

        public ActionResult GetQuestionDetail (int id)
        {
            QuestionViewModel qvm = questionService.GetQuestionById(id);

            List<TopicViewModel> topics = topicService.GetTopicByCourseId(qvm.CourseId);

            List<LevelViewModel> levels = levelService.GetLevel();

            List<LearningOutcomeViewModel> learningOutcomes = lo.GetLearningOutcomeByCourseId(qvm.CourseId);

            QuestionDetailViewModel qdvm = new QuestionDetailViewModel()
            {
                Question = qvm,
                Topics = topics,
                Levels = levels,
                LearningOutcomes = learningOutcomes
            };

            return View("EditQuestion", qdvm);
        }

        public ActionResult UpdateQuestion(QuestionViewModel ques)
        {
            bool result = questionService.UpdateQuestion(ques);

            bool optionResult = optionService.UpdateOptions(ques.Options);

            return RedirectToAction("GetQuestionDetail", new {id = ques.Id });
        }

        public ActionResult GenerateExam()
        {

            return View();
        }

        public ActionResult ViewGeneratedExamination()
        {
            return View();
        }

    }
}