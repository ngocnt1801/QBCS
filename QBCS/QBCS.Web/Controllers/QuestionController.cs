using QBCS.Entity;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
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

            return RedirectToAction("AddQuestion", new { courseId = model.CourseId });
        }

        // GET: Question
        public ActionResult GetListQuestion(int courseId)
        {
            List<QuestionViewModel> listQuestion = new List<QuestionViewModel>();
            listQuestion = questionService.GetAllQuestionByCourseId(courseId);
            
            //List<Question> Questions = questionService.GetQuestionsByCourse(courseId);
            //foreach (Question ques in Questions)
            //{
            //    List<Option> op = optionService.GetOptionsByQuestion(ques.Id);
            //    QuestionViewModel qvm = new QuestionViewModel
            //    {
            //        Question = ques,
            //        Options = op
            //    };
            //    ListQuestion.Add(qvm);
            //}
            return View("ListQuestion", listQuestion);
        }

        public ActionResult AddQuestion(int courseId)
        {
            var question = new QBCS.Service.ViewModel.QuestionViewModel();
            question.CourseId = courseId;
            return View(question);
        }

        [HttpPost]
        public ActionResult ImportFile(HttpPostedFileBase questionFile, int? courseId = 0)
        {

            //if (questionFile.ContentLength > 0)
            //{

            //}
            var questions = questionService.GetAllQuestions();
            return View("ImportResult", questions);
        }
        public ActionResult GetPartialView(bool? isDuplicate)
        {
            var questions = questionService.CheckDuplicated();
            //All
            if (!isDuplicate.HasValue)
            {

            }else if (isDuplicate.Value) // wrong
            {
                questions = questions.Where(q => q.IsDuplicated).ToList();
            } else // right
            {
                questions = questions.Where(q => !q.IsDuplicated).ToList();
            }
            return PartialView("_AllQuestion", questions);
        }
    }
}