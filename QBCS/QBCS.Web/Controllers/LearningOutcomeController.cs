using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    [CheckSession]
    public class LearningOutcomeController : Controller
    {
        private ILearningOutcomeService learningOutcomeService;
        private ICourseService courseService;
        public LearningOutcomeController()
        {
            learningOutcomeService = new LearningOutcomeService();
            courseService = new CourseService();
        }
        // GET: LearningOutcome
        public ActionResult Index()
        {
            var list = learningOutcomeService.GetAllLearningOutcome();
            return View(list);
        }
        public ActionResult Add(int courseId)
        {
            var learningOutcome = new LearningOutcomeViewModel()
            {
                CourseId = courseId
            };
            return View(learningOutcome);
        }
        [HttpPost]
        public ActionResult Add(LearningOutcomeViewModel learningOutcome)
        {
            var courseId = learningOutcomeService.AddLearningOutcome(learningOutcome);
            if(courseId == 0)
            {
                return RedirectToAction("Index", "Error");
            }
            return RedirectToAction("Detail","Course", new { itemId = courseId });
        }
        public ActionResult Edit(int id)
        {
            var result = learningOutcomeService.GetLearningOutcomeById(id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Edit(LearningOutcomeViewModel learningoutcome)
        {
            var courseId = learningOutcomeService.UpdateLearningOutcome(learningoutcome);

            if (courseId == 0)
            {
                return RedirectToAction("Index", "Error");
            }
            return RedirectToAction("Detail", "Course", new { itemId = courseId });
        }
        public JsonResult LoadCourse()
        {
            var result = courseService.GetAllCourses();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadCourseLive(string term)
        {
            var result = courseService.GetCoursesVMByName(term);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateDisable(int itemId)
        {
            var update = learningOutcomeService.UpdateDisable(itemId);
            return RedirectToAction("Detail","Course", new { itemId = update});
        }
        public JsonResult LoadCourseActive()
        {
            var result = courseService.GetCourseByDisable();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}