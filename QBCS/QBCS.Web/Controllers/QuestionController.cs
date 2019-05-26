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

        public QuestionController()
        {
            questionService = new QuestionService();
            optionService = new OptionService();
        }

        // GET: Question
        public ActionResult Index()
        {           
            var questions = questionService.GetAllQuestions();
            return View("ListQuestion", questions);
        }

        [HttpPost]
        public ActionResult Add(QBCS.Service.ViewModel.QuestionViewModel model)
        {
            questionService.Add(model);

            return RedirectToAction("AddQuestion", new { courseId = model.CourseId});
        }

        // GET: Question
        public ActionResult GetListQuestion(int courseId)
        {
            List<QuestionViewModel> ListQuestion = new List<QuestionViewModel>();

            List<Question> Questions = questionService.GetQuestionsByCourse(courseId);
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
    }
}