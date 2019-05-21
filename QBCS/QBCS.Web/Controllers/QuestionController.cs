using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    public class QuestionController : Controller
    {
        private IQuestionService questionService;

        public QuestionController()
        {
            questionService = new QuestionService();
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

            return RedirectToAction("AddQuestion", "Home");
        }
    }
}