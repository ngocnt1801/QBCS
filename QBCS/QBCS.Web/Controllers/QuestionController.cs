using AuthLib.Module;
using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    [CheckSession]
    public class QuestionController : Controller
    {
        private IQuestionService questionService;
        private IOptionService optionService;
        private ITopicService topicService;
        private ILevelService levelService;
        private ILearningOutcomeService learningOutcomeService;
        private IExaminationService examinationService;
        private IImportService importService;
        private ICourseService courseService;

        public QuestionController()
        {
            questionService = new QuestionService();
            optionService = new OptionService();
            topicService = new TopicService();
            levelService = new LevelService();
            learningOutcomeService = new LearningOutcomeService();
            examinationService = new ExaminationService();
            importService = new ImportService();
            courseService = new CourseService();
        }

        // GET: Question
        public ActionResult Index()
        {
            var questions = questionService.GetAllQuestions();
            return View("ListQuestion", questions);
        }

        [HttpPost]
        [Log(Action = "Create", TargetName = "Question", ObjectParamName = "ques", IdParamName = "Id")]
        public ActionResult Add(QuestionViewModel model)
        {
            questionService.Add(model);

            return RedirectToAction("AddQuestion", new { courseId = model.CourseId });
        }

        // GET: Question
        public ActionResult GetListQuestion(int courseId)
        {
            List<QuestionViewModel> ListQuestion = questionService.GetQuestionsByCourse(courseId);
            return View("ListQuestion", ListQuestion);
        }

        public ActionResult AddQuestion(int courseId)
        {
            var question = new QuestionViewModel();
            question.CourseId = courseId;
            return View(question);
        }

        //GET: Question
        public ActionResult GetQuestionsByContent(string content)
        {
            List<QuestionViewModel> result = new List<QuestionViewModel>();

            //List<Question> questions = questionService.GetQuestionsByContent(content);
            //foreach (Question question in questions)
            //{
            //    List<Option> op = optionService.GetOptionsByQuestion(question.Id);
            //    QuestionViewModel questionViewModel = new QuestionViewModel
            //    {
            //        Question = question,
            //        Options = op
            //    };
            //    result.Add(questionViewModel);
            //}
            return View("ListQuestion", result);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Get Question Detail", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult GetQuestionDetail(int id)
        {
            QuestionViewModel qvm = questionService.GetQuestionById(id);

            List<LevelViewModel> levels = levelService.GetLevel();

            List<LearningOutcomeViewModel> learningOutcomes = learningOutcomeService.GetLearningOutcomeByCourseId(qvm.CourseId);

            QuestionDetailViewModel qdvm = new QuestionDetailViewModel()
            {
                Question = qvm,
                Levels = levels,
                LearningOutcomes = learningOutcomes
            };
            TempData["active"] = "Course";
            return View("EditQuestion", qdvm);
        }


        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Update Question", "QBCS", protectType: ProtectType.Authorized)]
        [ValidateInput(false)]
        [Log(Action = "Update", TargetName = "Question", ObjectParamName = "ques", IdParamName = "Id")]
        public ActionResult UpdateQuestion(QuestionViewModel ques)
        {
            bool result = questionService.UpdateQuestion(ques);
            // bool optionResult = optionService.UpdateOptions(ques.Options);
            TempData["Modal"] = "#success-modal";
            return RedirectToAction("CourseDetail", "Course", new { courseId = ques.CourseId });
        }

        //lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Import File", "QBCS", protectType: ProtectType.Authorized)]
        [HttpPost]
        public ActionResult ImportFile(
            HttpPostedFileBase questionFile
            , int courseId
            , string ownerName
            , bool checkCate = false
            , bool checkHTML = false
            , string prefix = ""
            , bool allowLogFile = false)
        {
            var user = (UserViewModel)Session["user"];

            bool check = true;
            if (questionFile.ContentLength > 0)
            {
                check = questionService.InsertQuestion(questionFile, user.Id, courseId, checkCate, checkHTML, ownerName, prefix, allowLogFile);
            }

            //notify 
            TempData["Modal"] = "#success-modal";
            TempData["CourseId"] = courseId;
            TempData["OwnereName"] = ownerName;

            return RedirectToAction("Index", "Home");
        }

        //lecturer
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Import Manually", "QBCS", protectType: ProtectType.Authorized)]
        [HttpPost]
        public JsonResult ImportTextarea(Textarea textarea)
        {
            var user = (UserViewModel)Session["user"];
            bool check = true;
            if (textarea.Table != null && !textarea.Table.Equals(""))
            {
                check = questionService.InsertQuestionWithTableString(textarea.Table, user.Id, textarea.CourseId);
            }
            //if (table != null && !table.Equals(""))
            //{
            //    check = questionService.InsertQuestionWithTableString("", user.Id, courseId);
            //}


            //notify 
            TempData["Message"] = "You import successfully";
            TempData["Status"] = ToastrEnum.Success;

            return Json(check, JsonRequestBehavior.AllowGet);
        }

        [Feature(FeatureType.BusinessLogic, "Get All Course By User for import", "QBCS", protectType: ProtectType.Authorized)]
        public JsonResult LoadCourseAjax()
        {
            var user = (UserViewModel)Session["user"];
            int userId = user != null ? user.Id : 0;
            var result = courseService.GetAllCoursesByUserId(userId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPartialView(bool? isDuplicate)
        {
            var questions = questionService.CheckDuplicated();
            //All
            if (!isDuplicate.HasValue)
            {

            }
            else if (isDuplicate.Value) // wrong
            {
                questions = questions.Where(q => q.IsDuplicated).ToList();
            }
            else // right
            {
                questions = questions.Where(q => !q.IsDuplicated).ToList();
            }
            return PartialView("_AllQuestion", questions);
        }

        public ActionResult GetQuestionByQuestionId(int? questionId)
        {
            //var content = JsonConvert.DeserializeObject<QuestionViewModel>(question);
            //var questions = questionService.GetQuestionByQuestionId(questionId.HasValue ? questionId.Value : 0);
            var question = questionService.GetQuestionById(questionId.Value);
            //question.DuplicatedQuestion = question;
            return View("ReviewQuestion", question);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Get List Question By Category", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(QuestionController), nameof(QuestionController.ToggleDisable))]
        [Dependency(typeof(QuestionController), nameof(QuestionController.UpdateCategory))]
        public ActionResult GetQuestions(int? courseId, int? categoryId, int? learningoutcomeId, int? topicId, int? levelId)
        {
            var result = questionService.GetQuestionList(courseId, categoryId, learningoutcomeId, topicId, levelId);
            if (courseId == 0 || courseId == null)
            {
                return PartialView("Staff_ListQuestion", result);
            }
            return PartialView("ListQuestion", result);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Disable Question", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult ToggleDisable(int id, int? courseId, int? categoryId, int? learningoutcomeId, int? topicId, int? levelId)
        {
            questionService.ToggleDisable(id);

            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Move Questions", "QBCS", protectType: ProtectType.Authorized)]
        [ValidateInput(false)]
        [Log(Action = "Move", TargetName = "Question", ObjectParamName = "ques", IdParamName = "ids", CateParamName = "categoryId", LocParamName = "learningOutcomeId", LevelParamName = "levelId")]
        public ActionResult UpdateCategory(int[] ids, int? categoryId, int? learningOutcomeId, int? levelId)
        {
            questionService.UpdateCategory(ids, categoryId, learningOutcomeId, levelId);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }

    public class Textarea
    {
        public string Table { get; set; }
        public int CourseId { get; set; }
    }
}