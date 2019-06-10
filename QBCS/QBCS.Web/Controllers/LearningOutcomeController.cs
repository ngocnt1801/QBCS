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
    public class LearningOutcomeController : Controller
    {
        private ILearningOutcomeService learningOutcomeService;
        public LearningOutcomeController()
        {
            learningOutcomeService = new LearningOutcomeService();
        }
        // GET: LearningOutcome
        public ActionResult Index()
        {
            var list = learningOutcomeService.GetAllLearningOutcome();
            return View(list);
        }
        public ActionResult Add()
        {
            var learningOutcome = new LearningOutcomeViewModel();
            return View(learningOutcome);
        }
        [HttpPost]
        public JsonResult Add(LearningOutcomeViewModel learningOutcome)
        {
            bool result = false;
            result = learningOutcomeService.AddLearningOutcome(learningOutcome);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {
            var result = learningOutcomeService.GetLearningOutcomeById(id);
            return View(result);
        }
        [HttpPost]
        public JsonResult Edit(LearningOutcomeViewModel learningoutcome)
        {
            bool result = false;
            result = learningOutcomeService.UpdateLearningOutcome(learningoutcome);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateDisable(int itemId)
        {
            var update = learningOutcomeService.UpdateDisable(itemId);
            return RedirectToAction("Index");
        }

    }
}