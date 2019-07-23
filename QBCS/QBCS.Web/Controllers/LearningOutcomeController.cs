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
        [LogAction(Action = "LOC", Message = "Get LOC", Method = "GET")]
        public ActionResult Index()
        {
            var list = learningOutcomeService.GetAllLearningOutcome();
            return View(list);
        }
        [LogAction(Action = "LOC", Message = "Add LOC", Method = "GET")]
        public ActionResult Add(int courseId)
        {
            var learningOutcome = new LearningOutcomeViewModel()
            {
                CourseId = courseId
            };
            return View(learningOutcome);
        }
        [HttpPost]
        [LogAction(Action = "LOC", Message = "Add LOC", Method = "POST")]
        public ActionResult Add(LearningOutcomeViewModel learningOutcome)
        {
            var courseId = learningOutcomeService.AddLearningOutcome(learningOutcome);
            //if(courseId == 0)
            //{
            //    return RedirectToAction("Index", "Error");
            //}
            //return RedirectToAction("Detail","Course", new { itemId = courseId });
            return RedirectToAction("Category","Course", new { courseId = learningOutcome.CourseId });
        }
        [LogAction(Action = "LOC", Message = "Edit LOC", Method = "GET")]
        public ActionResult Edit(int id)
        {
            var result = learningOutcomeService.GetLearningOutcomeById(id);
            return View(result);
        }
        [HttpPost]
        [LogAction(Action = "LOC", Message = "Edit LOC", Method = "POST")]
        public ActionResult Edit(LearningOutcomeViewModel learningoutcome)
        {
            var courseId = learningOutcomeService.UpdateLearningOutcome(learningoutcome);

            //if (courseId == 0)
            //{
            //    return RedirectToAction("Index", "Error");
            //}
            //return RedirectToAction("Detail", "Course", new { itemId = courseId });
            return RedirectToAction("Category", "Course", new { courseId = learningoutcome.CourseId });
        }
        [LogAction(Action = "LOC", Message = "Get LOC", Method = "GET")]
        public JsonResult LoadCourse()
        {
            var result = courseService.GetAllCourses();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [LogAction(Action = "Course", Message = "Search Course Live", Method = "GET")]
        public JsonResult LoadCourseLive(string term)
        {
            var result = courseService.GetCoursesVMByNameAndCode(term);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [LogAction(Action = "LOC", Message = "Update LOC", Method = "GET")]
        public ActionResult UpdateDisable(int itemId)
        {
            var update = learningOutcomeService.UpdateDisable(itemId);
            return RedirectToAction("Detail","Course", new { itemId = update});
        }

        [LogAction(Action = "Course", Message = "View Course Active", Method = "GET")]
        public JsonResult LoadCourseActive()
        {
            var result = courseService.GetCourseByDisable();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}