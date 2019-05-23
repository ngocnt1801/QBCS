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
            return View();
        }

        [HttpPost]
        public ActionResult Add(QBCS.Service.ViewModel.QuestionViewModel model)
        {
            questionService.Add(model);

            return RedirectToAction("AddQuestion", "Home");
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

        [Route("course/{courseCode}/question/new")]
        public ActionResult AddQuestion(QuestionViewModel model, string courseCode)
        {

            return View();
        }

        // GET: Question
        public ActionResult GetQuestionsByContent(string content)
        {
            List<QuestionViewModel> result = new List<QuestionViewModel>();

            List<Question> questions = questionService.GetQuestionsByContent(content);
            foreach (Question question in questions)
            {
                List<Option> op = optionService.GetOptionsByQuestion(question.Id);
                QuestionViewModel questionViewModel = new QuestionViewModel
                {
                    Question = question,
                    Options = op
                };
                result.Add(questionViewModel);
            }
            return View("ListQuestion", result);
        }
    }
}