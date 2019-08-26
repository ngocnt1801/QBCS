﻿using AuthLib.Module;
using Newtonsoft.Json;
using QBCS.Service.Enum;
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
        private IUserService userService;
        private ICategoryService categoryService;
        private ILogService logService;

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
            userService = new UserService();
            categoryService = new CategoryService();
            logService = new LogService();
        }

        // GET: Question
        public ActionResult Index()
        {
            var questions = questionService.GetAllQuestions();
            return View("ListQuestion", questions);
        }

        [HttpPost]
        [Log(Action = "Create", TargetName = "Question", ObjectParamName = "ques", IdParamName = "Id")]
        [LogAction(Action = "Question", Message = "Add Question", Method = "POST")]
        public ActionResult Add(QuestionViewModel model)
        {
            questionService.Add(model);

            return RedirectToAction("AddQuestion", new { courseId = model.CourseId });
        }

        // GET: Question
        [LogAction(Action = "Question", Message = "Get Question By CourseId", Method = "GET")]
        public ActionResult GetListQuestion(int courseId)
        {
            List<QuestionViewModel> ListQuestion = questionService.GetQuestionsByCourse(courseId);
            return View("ListQuestion", ListQuestion);
        }

        [LogAction(Action = "Question", Message = "Add Question in Course", Method = "GET")]
        public ActionResult AddQuestion(int courseId)
        {
            var question = new QuestionViewModel();
            question.CourseId = courseId;
            return View(question);
        }

        //GET: Question
        [LogAction(Action = "Question", Message = "Get Question", Method = "GET")]
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
        [LogAction(Action = "Question", Message = "View Question Detail", Method = "GET")]
        public ActionResult GetQuestionDetail(int id)
        {
            QuestionViewModel qvm = questionService.GetQuestionById(id);

            List<LevelViewModel> levels = levelService.GetLevel();

            List<LearningOutcomeViewModel> learningOutcomes = learningOutcomeService.GetLearningOutcomeByCourseId(qvm.CourseId);

            List<CategoryViewModel> categories = categoryService.GetCategoriesByCourseId(qvm.CourseId);

            QuestionDetailViewModel qdvm = new QuestionDetailViewModel()
            {
                Question = qvm,
                Levels = levels,
                LearningOutcomes = learningOutcomes,
                Categories = categories
            };
            TempData["active"] = "Course";
            return View("EditQuestion", qdvm);
        }


        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Update Question", "QBCS", protectType: ProtectType.Authorized)]
        [ValidateInput(false)]
        [Log(Action = "Update", TargetName = "Question", ObjectParamName = "ques", IdParamName = "Id")]
        [LogAction(Action = "Question", Message = "Update Question", Method = "GET")]
        public ActionResult UpdateQuestion(QuestionViewModel ques)
        {
            QuestionDetailViewModel questionDetailViewModel = new QuestionDetailViewModel();
            if (!ques.QuestionContent.Trim().Equals("[html]") || (ques.ImagesInput != null && ques.ImagesInput.Count() > 0))
            {
                bool result = questionService.UpdateQuestion(ques);
                // bool optionResult = optionService.UpdateOptions(ques.Options);
                ViewBag.Modal = "#success-modal";
                return RedirectToAction("CourseDetail", "Course", new { courseId = ques.CourseId });
            }
            if (ques.QuestionContent.Trim().Equals("[html]"))
            {
                ModelState.AddModelError(string.Empty, "Please enter Question Content");
            }

            foreach (var item in ques.Options)
            {
                if (item.OptionContent.Trim().Equals("[html]"))
                {
                    ModelState.AddModelError(string.Empty, "Please enter Option Content");
                }
            }

            //ModelState.AddModelError(string.Empty, "Question Content is required");
            //ModelState.AddModelError(string.Empty, "Option Content is required");
            List<LevelViewModel> levels = levelService.GetLevel();
            List<LearningOutcomeViewModel> learningOutcomes = learningOutcomeService.GetLearningOutcomeByCourseId(ques.CourseId);
            questionDetailViewModel = new QuestionDetailViewModel()
            {
                Question = ques,
                Levels = levels,
                LearningOutcomes = learningOutcomes
            };


            return View("EditQuestion", questionDetailViewModel);

        }

        [HttpPost]
        public JsonResult UpdateQuestionWithTextBox(EditQuestionTextboxViewModel vm)
        {
            try
            {
                var conversion = questionService.TableStringToListQuestion(vm.Table, "");
                if (conversion.Count > 0)
                {
                    if ((conversion.FirstOrDefault().QuestionContent == null && conversion.FirstOrDefault().Images.Count == 0) || conversion.FirstOrDefault().Options.Count == 0)
                    {
                        return Json(new { error = true }, JsonRequestBehavior.AllowGet);
                    }
                    var question = new QuestionViewModel()
                    {
                        Id = (int)vm.QuestionId,
                        //ImportId = (int)vm.ImportId,
                        QuestionContent = conversion.FirstOrDefault().QuestionContent.Replace("\r", ""),
                        Images = conversion.FirstOrDefault().Images.Select(i => new ImageViewModel()
                        {
                            Source = i.Source
                        }).ToList(),
                        Options = conversion.FirstOrDefault().Options.Select(o => new OptionViewModel()
                        {
                            OptionContent = o.OptionContent,
                            Images = o.Images,
                            IsCorrect = o.IsCorrect
                        }).ToList(),
                        LearningOutcomeId = vm.LearningOutcomeId != null ? (int)vm.LearningOutcomeId : 0,
                        CategoryId = vm.CategoryId != null ? (int)vm.CategoryId : 0,
                        LevelId = vm.LevelId != null ? (int)vm.LevelId : 0,

                    };
                    var oldQuestion = questionService.GetQuestionById((int)vm.QuestionId);
                    var user = (UserViewModel)Session["user"];
                    logService.LogFullManually("UpdateQuestionWithTextBox", "Question",
                                                vm.QuestionId, null, "QuestionController", "POST", user.Fullname, "",
                                                JsonConvert.SerializeObject(question), JsonConvert.SerializeObject(oldQuestion));
                    questionService.UpdateQuestionWithTextbox(question);
                    TempData["Modal"] = "#success-modal";
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { error = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetCategoriesInCourse(int id)
        {
            List<CategoryViewModel> categories = categoryService.GetCategoriesByCourseId(id);
            return Json(categories, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLevels()
        {
            List<LevelViewModel> levels = levelService.GetLevel();
            return Json(levels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLearningOutcomesInCourse(int id)
        {
            List<LearningOutcomeViewModel> learningOutcomes = learningOutcomeService.GetLearningOutcomeByCourseId(id);
            return Json(learningOutcomes, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EditQuestionWithTextbox(int id)
        {
            var question = questionService.GetQuestionById(id);
            return View("EditQuestionWithTextbox", question);
        }

        //lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Import File", "QBCS", protectType: ProtectType.Authorized)]
        [HttpPost]
        [LogAction(Action = "Question", Message = "Import File", Method = "POST")]
        public ActionResult ImportFile(HttpPostedFileBase questionFile, int courseId, int? owner = null, bool checkCate = false, bool checkHTML = false, string prefix = "", bool checkSemantic = false)
        {
            var user = (UserViewModel)Session["user"];

            int check = -1;
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            if (questionFile.ContentLength > 0)
            {
                if (owner != null && owner != 0)
                {
                    var ownerUser = userService.GetUserById(owner.Value);
                    if (ownerUser != null)
                    {
                        check = questionService.InsertQuestion(questionFile, user.Id, courseId, checkCate, checkHTML, ownerUser.Id, ownerUser.Fullname, prefix, checkSemantic);
                        if (check > -1)
                        {
                            ViewBag.Modal = "#success-modal";
                            TempData["CourseId"] = courseId;
                            TempData["OwnereName"] = ownerUser.Fullname;
                            //return Json("OK");
                        }
                        else
                        {
                            ViewBag.Message = "Cannot Import File!";
                            ViewBag.Status = ToastrEnum.Error;
                            //return Json("Error");
                        }

                        //notify 

                    }
                    else
                    {
                        ViewBag.Message = "Cannot Import File!";
                        ViewBag.Status = ToastrEnum.Error;
                        //return Json("Error");
                    }
                }
                else
                {
                    check = questionService.InsertQuestion(questionFile, user.Id, courseId, checkCate, checkHTML, user.Id, user.Fullname, prefix, checkSemantic);
                    if (check == -1)
                    {
                        ViewBag.Message = "File has wrong format. Please check again!";
                        ViewBag.Status = ToastrEnum.Error;
                        return Json("Error");
                    }
                }

            }

            return Json(check);
            //return RedirectToAction("Index", "Home");
        }

        //lecturer
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Import Manually", "QBCS", protectType: ProtectType.Authorized)]
        [HttpPost]
        [LogAction(Action = "Question", Message = "Import File Text area", Method = "POST")]
        public JsonResult ImportTextarea(Textarea textarea)
        {
            var user = (UserViewModel)Session["user"];
            bool check = true;
            if (textarea.Table != null && !textarea.Table.Equals(""))
            {
                if (String.IsNullOrWhiteSpace(textarea.OwnerName))
                {
                    textarea.OwnerName = user != null ? user.Fullname : User.Get(u => u.FullName);
                }

                check = questionService.InsertQuestionWithTableString(textarea.Table, user.Id, textarea.CourseId, textarea.Prefix, textarea.OwnerName,textarea.CheckSemantic);

            }
            else
            {
                ViewBag.Message = "You cannot import empty file";
                ViewBag.Status = ToastrEnum.Error;
            }
            //if (table != null && !table.Equals(""))
            //{
            //    check = questionService.InsertQuestionWithTableString("", user.Id, courseId);
            //}


            //notify 
            ViewBag.Message = "You import successfully";
            ViewBag.Status = ToastrEnum.Success;

            return Json(check, JsonRequestBehavior.AllowGet);
        }

        [Feature(FeatureType.BusinessLogic, "Get All Course By User for import", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Course", Message = "Load All Course", Method = "GET")]
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

        [LogAction(Action = "Question", Message = "Get Question By Question Id", Method = "GET")]
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
        [LogAction(Action = "Questions", Message = "View Question Detail", Method = "GET")]
        public ActionResult GetQuestions(int? courseId, int? categoryId, int? learningoutcomeId, int? topicId, int? levelId)
        {
            var result = questionService.GetQuestionList(courseId, categoryId, learningoutcomeId, topicId, levelId);
            if (courseId == 0 || courseId == null)
            {
                return PartialView("Staff_ListQuestion", result);
            }
            return PartialView("ListQuestion", result);
        }

        public JsonResult GetQuestionsDatatable(int? courseId, int? categoryId, int? learningoutcomeId, int? topicId, int? levelId, int draw, int start, int length)
        {
            var search = Request["search[value]"] != null ? Request["search[value]"].ToLower() : "";
            var data = questionService.GetQuestionList(courseId, categoryId, learningoutcomeId, topicId, levelId, search, start, length);
            var result = Json(new { draw = draw, recordsFiltered = data.filteredCount, recordsTotal = data.totalCount, data = data.Questions, success = true }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Disable Question", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Question", Message = "Update File", Method = "GET")]
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
        [LogAction(Action = "Question", Message = "Move Question", Method = "GET")]
        public ActionResult UpdateCategory(int[] ids, int? categoryId, int? learningOutcomeId, int? levelId)
        {
            questionService.UpdateCategory(ids, categoryId, learningOutcomeId, levelId);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQuestionByImportIdAndType(int importId, string type, int draw, int start, int length)
        {
            var search = Request["search[value]"] != null ? Request["search[value]"].ToLower() : "";
            var data = questionService.GetQuestionTempByImportId(importId, type, search, start, length);
            var result = Json(new { draw = draw, recordsFiltered = data.filteredCount, recordsTotal = data.totalCount, data = data.Questions, success = true }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }

        public ActionResult CheckBankResult(int courseId)
        {
            var course = courseService.GetCourseById(courseId);
            TempData["course"] = course.Name + " - " + course.Code;
            return View(courseId);
        }

        public JsonResult GetQuestionByCourseIdAndType(int courseId, string type, int draw, int start, int length)
        {
            var search = Request["search[value]"] != null ? Request["search[value]"].ToLower() : "";
            var data = questionService.GetQuestionByCourseId(courseId, type, search, start, length);
            var result = Json(new { draw = draw, recordsFiltered = data.filteredCount, recordsTotal = data.totalCount, data = data.Questions, success = true }, JsonRequestBehavior.AllowGet);
            result.MaxJsonLength = int.MaxValue;
            return result;
        }

        public ActionResult Delete(int questionId, string url)
        {
            questionService.UpdateQuestionStatus(questionId, (int)StatusEnum.Deleted);
            if (String.IsNullOrWhiteSpace(url))
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Redirect(url);
            }

            //return RedirectToAction("GetResult", new { importId = importId });
        }

        public ActionResult Skip(int questionId, string url)
        {
            questionService.UpdateQuestionStatus(questionId, (int)StatusEnum.Success);
            if (String.IsNullOrWhiteSpace(url))
            {
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Redirect(url);
            }
            //return RedirectToAction("GetResult", new { importId = importId });
        }

        public ActionResult GetDuplicatedDetail(int id)
        {
            var model = questionService.GetDuplicatedDetail(id);

            return View(model);
        }

        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Question History In Examination", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Examination", Message = "View Examination History Activity", Method = "GET")]
        public ActionResult GetExaminationHistory(int id)
        {
            var result = questionService.GetQuestionHistory(id);
            TempData["active"] = "Course";
            return View(result);
        }
    }

    public class Textarea
    {
        public string Table { get; set; }
        public int CourseId { get; set; }
        public string OwnerName { get; set; }
        public string Prefix { get; set; }
        public bool CheckSemantic { get; set; }
    }
}