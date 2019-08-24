using AuthLib.Module;
using QBCS.Entity;
using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using System.Collections.Generic;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    [CheckSession]
    public class CourseController : Controller
    {
        private ICourseService courseService;
        private ICategoryService categoryService;
        private ILearningOutcomeService learningOutcomeService;
        private ISyllabusService syllabusService;
        private IExaminationService examinationService;
        public CourseController()
        {
            courseService = new CourseService();
            categoryService = new CategoryService();
            learningOutcomeService = new LearningOutcomeService();
            syllabusService = new SyllabusService();
            examinationService = new ExaminationService();
        }

        [Feature(FeatureType.SideBar, "List all course by user", "QBCS", protectType: ProtectType.Authorized, ShortName = "Course", InternalId = (int)SideBarEnum.CourseByUser)]
        // GET: Course
        [LogAction(Action = "Courses", Message = "Get All Courses", Method = "GET")]
        public ActionResult Index()
        {
            var user = ((UserViewModel)Session["user"]);
            List<CourseViewModel> list = new List<CourseViewModel>();
            if (user != null)
            {
                list = courseService.GetAllCoursesByUserId(user.Id);
            }
            else
            {
                list = courseService.GetAllCoursesByUserId(null);
            }

          
            TempData["active"] = "Course";
            return View(list);
        }

        public ActionResult Staff_Index()
        {
            var list = courseService.GetCourseByDisable();
            return View(list);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [LogAction(Action = "Courses", Message = "Add Course", Method = "POST")]
        public ActionResult Add(CourseViewModel model)
        {
            courseService.AddNewCourse(model);
            return RedirectToAction("Staff_Index");
        }
        public ActionResult Edit(int itemId)
        {
            var result = courseService.GetCourseById(itemId);
            return View(result);
        }
        [HttpPost]
        [LogAction(Action = "Courses", Message = "Edit Course", Method = "POST")]
        public ActionResult Edit(CourseViewModel model)
        {
            var result = courseService.UpdateCourse(model);
            if (result)
            {
                return RedirectToAction("Detail", "Course", new { itemId = model.Id });
            }
            else
            {
                return RedirectToAction("Index", "Error");
            }

        }
        [LogAction(Action = "Courses", Message = "Get Course", Method = "GET")]
        public ActionResult GetCoursesByName(string name)
        {
            List<CourseViewModel> result = new List<CourseViewModel>();

            List<Course> courses = courseService.GetCoursesByName(name);
            foreach (Course course in courses)
            {
                CourseViewModel courseViewModel = new CourseViewModel
                {
                    Name = course.Name,
                    Code = course.Code,
                    DefaultNumberOfQuestion = course.DefaultNumberOfQuestion.Value
                };
                result.Add(courseViewModel);
            }
            return View("ListCourse", result);
        }

        [LogAction(Action = "Courses", Message = "Get Course Detail", Method = "GET")]
        public ActionResult Detail(int itemId)
        {
            var result = courseService.GetDetailCourseById(itemId);
            return View(result);
        }

        [LogAction(Action = "Courses", Message = "Update Disable Course", Method = "GET")]
        public ActionResult UpdateDisable(int itemId)
        {
            //int itemId = 0;
            //int userId = 0;
            //try
            //{
            //    string[] split = itemAndUserId.Split('_');
            //    itemId = int.Parse(split[0]);
            //    userId = int.Parse(split[1]);
            //}
            //catch (NotFiniteNumberException)
            //{

            //}
            var update = courseService.UpdateDisable(itemId);
            return RedirectToAction("Staff_Index");
        }

        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "All Courses For Generate", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Courses", Message = "View Courses", Method = "GET")]
        public ActionResult GetAllCourse()
        {
            List<CourseViewModel> courses = courseService.GetAllCourses();
            TempData["CreateExam"] = true;
            TempData["active"] = "Examination";
            return View("Staff_ListCourse", courses);
        }

        //Staff
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "All Courses For History", "QBCS", protectType: ProtectType.Authorized, ShortName = "Examination's Questions", InternalId = (int)SideBarEnum.AllCourseHistory)]
        [LogAction(Action = "Courses", Message = "View All Course", Method = "GET")]
        public ActionResult GetAllCourseForHistory()
        {
            List<CourseViewModel> courses = courseService.GetAllCourses();
            TempData["ViewHistory"] = true;
            TempData["active"] = "Course";
            return View("Staff_ListCourse", courses);
        }

        public ActionResult GetCourseByNameOrId(string searchValue)
        {
            List<CourseViewModel> courses = courseService.SearchCourseByNameOrCode(searchValue);
            return View("Staff_ListCourse", courses);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Course Detail", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(QuestionController), nameof(QuestionController.GetQuestions))]
        [Dependency(typeof(QuestionController), nameof(QuestionController.ToggleDisable))]
        [Dependency(typeof(QuestionController), nameof(QuestionController.UpdateCategory))]
        [LogAction(Action = "Courses", Message = "Get Course Detail", Method = "GET")]
        public ActionResult CourseDetail(int courseId)
        {
            List<CategoryViewModel> categories = categoryService.GetListCategories(courseId);
            var model = courseService.GetCourseById(courseId);
            model.LearningOutcome = learningOutcomeService.GetLearningOutcomeByCourseId(courseId);
            model.Categories = categories;
            TempData["active"] = "Course";
            return View(model);
        }

        //Lecturer
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "All Course Statistic", "QBCS", protectType: ProtectType.Authorized, ShortName = "Statistic All Courses", InternalId = (int)SideBarEnum.AllStatistic)]
        //stpm: dependency declare
        [Dependency(typeof(CourseController), nameof(CourseController.GetCourseDetailStat))]
        [LogAction(Action = "Courses", Message = "View Course Statistic", Method = "GET")]
        public ActionResult CourseStatistic()
        {

            var result = courseService.GetAllCoursesWithDetail();
            TempData["active"] = "Statistic";
            return View(result);
        }

        [Feature(FeatureType.SideBar, "Course Statistic By User", "QBCS", protectType: ProtectType.Authorized, ShortName = "Statistic", InternalId = (int)SideBarEnum.StatisticByUser)]
        //stpm: dependency declare
        [Dependency(typeof(CourseController), nameof(CourseController.GetCourseDetailStat))]
        [LogAction(Action = "Courses", Message = "View Course Statistic", Method = "GET")]
        public ActionResult CourseStatisticByUser()
        {
            var user = (UserViewModel)Session["user"];
            int userId = user != null ? user.Id : 0;
            var result = courseService.GetAllCoursesWithDetailById(userId);
            TempData["active"] = "Statistic";
            return View("CourseStatistic", result);
        }

        //Lecturer
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Course Detail Statistic", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Courses", Message = "View Course Statistic Detail", Method = "GET")]
        public ActionResult GetCourseDetailStat(int id, string type)
        {
            var result = courseService.GetCourseStatDetailByIdAndType(id, type);
            return PartialView("CourseDetailStatistic", result);
        }

        [LogAction(Action = "Courses", Message = "Get Course Statistic", Method = "GET")]
        public ActionResult Staff_CourseStatistic()
        {

            var result = courseService.GetAllCoursesWithDetail();
            TempData["active"] = "Staff_Statistic";
            return View(result);
        }

        public ActionResult GetStaffCourseDetailStat(int id)
        {
            var result = examinationService.GetExamStat(id);
            return PartialView("Staff_CourseDetailStatistic", result);
        }

        public ActionResult CourseDetailWithoutId()
        {
            List<CategoryViewModel> categories = categoryService.GetAllCategories();
            var model = new CourseViewModel
            {
                Categories = categories
            };
            return View(model);
        }

        [LogAction(Action = "Syllabus", Message = "Get Syllabus", Method = "GET")]
        public ActionResult Syllabus(int courseId)
        {
            var model = syllabusService.GetSyllabusPartials(courseId);
            var course = courseService.GetCourseById(courseId);
            course.Syllabus = model;
            return View(course);
        }

        [LogAction(Action = "Syllabus", Message = "Get Syllabus", Method = "POST")]
        public ActionResult CreateSyllabus(SyllabusPartialViewModel model)
        {
            syllabusService.AddSyllabusPartial(model);
            return RedirectToAction("Syllabus", new { courseId = model.CourseId });
        }

        [LogAction(Action = "Syllabus", Message = "Update Syllabus", Method = "POST")]
        public ActionResult UpdateSyllabus(SyllabusPartialViewModel model)
        {
            syllabusService.UpdateSyllabusPartial(model);
            return RedirectToAction("Syllabus", new { courseId = model.CourseId });
        }

        [LogAction(Action = "Syllabus", Message = "Delete Syllabus", Method = "POST")]
        public ActionResult DeleteSyllabus(int id, int courseId)
        {
            syllabusService.DeleteSyllabusPartial(id);
            return RedirectToAction("Syllabus", new { courseId = courseId });
        }

        [LogAction(Action = "LOC", Message = "Get LOC", Method = "GET")]
        public ActionResult GetLearningOutcomes(int syllabusId)
        {
            var model = syllabusService.GetLearningOutcomes(syllabusId);
            model.AddRange(syllabusService.GetLearningOutcomes(null));
            ViewBag.Syl = syllabusId;
            return PartialView(model);
        }

        [LogAction(Action = "LOC", Message = "Add LOC to Syllabus", Method = "POST")]
        public ActionResult AddLOCtoSyllabus(int locId, int syllabusId)
        {
            syllabusService.ChangeSyllabusPartial(locId, syllabusId);
            var model = syllabusService.GetLearningOutcomes(syllabusId);
            model.AddRange(syllabusService.GetLearningOutcomes(null));
            ViewBag.Syl = syllabusId;
            return PartialView("GetLearningOutcomes", model);
        }

        [LogAction(Action = "LOC", Message = "Delete LOC", Method = "GET")]
        public ActionResult DeleteLOC(int locId, int syllabusId)
        {
            syllabusService.ChangeSyllabusPartial(locId, null);
            var model = syllabusService.GetLearningOutcomes(syllabusId);
            model.AddRange(syllabusService.GetLearningOutcomes(null));
            ViewBag.Syl = syllabusId;
            return PartialView("GetLearningOutcomes", model);
        }

         
        public ActionResult UpdateTotalQuestion(int courseId, int total)
        {
            courseService.UpdateTotalQuesiton(courseId, total);
            return RedirectToAction("Syllabus", new { courseId = courseId });
        }

        public ActionResult Category(int courseId)
        {
            var categories = categoryService.GetCategoriesByCourseId(courseId);
            var course = courseService.GetDetailCourseById(courseId);
            course.Categories = categories;
            return View(course);
        }

        [LogAction(Action = "Category", Message = "Create category", Method = "POST")]
        public ActionResult CreateCategory(CategoryViewModel model)
        {
            categoryService.AddCategory(model);
            return RedirectToAction("Category", new { courseId = model.CourseId });
        }

        [LogAction(Action = "Category", Message = "Delete category", Method = "POST")]
        public ActionResult DeleteCategory(int categoryId, int courseId)
        {
            categoryService.DeleteCategory(categoryId);
            return RedirectToAction("Category", new { courseId = courseId });
        }

        [LogAction(Action = "Category", Message = "Update category", Method = "POST")]
        public ActionResult UpdateCategory(CategoryViewModel model)
        {
            categoryService.UpdateCategory(model);
            return RedirectToAction("Category", new { courseId = model.CourseId });
        }

        [LogAction(Action = "Category", Message = "Disable category", Method = "POST")]
        public ActionResult DisableCategory(int categoryId)
        {
            categoryService.DisableCategory(categoryId);
            return Json("OK");
        }
    }
}