using AuthLib.Module;
using Newtonsoft.Json;
using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    [CheckSession]
    public class ImportController : Controller
    {
        private IImportService importService;
        private IQuestionService questionService;
        private ILogService logService;
        private ICourseService courseService;

        public ImportController()
        {
            importService = new ImportService();
            questionService = new QuestionService();
            logService = new LogService();
            courseService = new CourseService();
        }

        //Lecturer
        // GET: Import
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "List User Imports", "QBCS", protectType: ProtectType.Authorized, ShortName = "Import History", InternalId = (int)SideBarEnum.ImportByUser)]
        [LogAction(Action = "Import", Message = "View All Import History", Method = "GET")]
        public ActionResult Index()
        {
            var user = ((UserViewModel)Session["user"]);
            TempData["active"] = "Import History";
            if (user != null)
            {
                var model = importService.GetListImport(user.Id);
                return View(model);
            }
            else
            {
                return View();
            }
        }

        //Lecturer
        // GET: Import
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "List All Imports", "QBCS", protectType: ProtectType.Authorized, ShortName = "All Imports", InternalId = (int)SideBarEnum.AllImport)]
        [LogAction(Action = "Import", Message = "View All Import History", Method = "GET")]
        public ActionResult AllImport()
        {
            var model = importService.GetListImport(null);
            TempData["active"] = "All Imports";
            return View("Index", model);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Import Result", "QBCS", protectType: ProtectType.Authorized)]
        [Dependency(typeof(ImportController), nameof(ImportController.GetPartialTable))]
        [Dependency(typeof(ImportController), nameof(ImportController.Skip))]
        [Dependency(typeof(ImportController), nameof(ImportController.Delete))]
        [LogAction(Action = "Import", Message = "View Detail Import History", Method = "GET")]
        public ActionResult GetResult(int importId)
        {
            var result = importService.GetImportResult(importId);
            TempData["active"] = "All Imports";
            var course = courseService.GetCourseById(result.CourseId);
            TempData["course"] = course.Name + " - " + course.Code;
            if (result.Status != (int)StatusEnum.Done)
            {
                //return View(result);
                return View(new GetResultViewModel() {
                    ImportId = result.Id,
                    totalNumber = result.Questions.Count(),
                    editableNumber = result.Questions.Where(q => q.Status == StatusEnum.Editable).Count(),
                    successNumber = result.Questions.Where(q => q.Status == StatusEnum.Success).Count(),
                    invalidNumber = result.Questions.Where(q => q.Status == StatusEnum.Invalid).Count(),
                    deleteNumber = result.Questions.Where(q => q.Status == StatusEnum.Deleted).Count(),
                    NotInsertNumber = result.Questions.Where(q => q.Status == StatusEnum.Editable || q.Status == StatusEnum.Deleted || q.Status == StatusEnum.Invalid).Count(),
                    RecheckNumber = result.Questions.Where(q => q.Status == StatusEnum.NotCheck).Count()
                });
            }
            TempData["NewestCount"] = result.NumberOfSuccess;
            return RedirectToAction("CourseDetail", "Course", new { courseId = result.CourseId});
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Edit Question Import", "QBCS", protectType: ProtectType.Authorized)]
        [ValidateInput(false)]
        [LogAction(Action = "Question", Message = "Edit Question", Method = "GET")]
        public ActionResult EditQuestion(QuestionTempViewModel model)
        {
            importService.UpdateQuestionTemp(model);
            TempData["active"] = "All Imports";
            return RedirectToAction("GetResult", new { importId = model.ImportId });
        }


        [HttpPost]
        public JsonResult UpdateQuestionTempWithTextBox(EditQuestionTextboxViewModel vm)
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
                    var questionTemp = new QuestionTempViewModel()
                    {
                        Id = (int)vm.QuestionId,
                        ImportId = (int)vm.ImportId,
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
                    };
                    var oldQuestionTemp = questionService.GetQuestionTempById((int)vm.QuestionId);
                    var user = (UserViewModel)Session["user"];
                    logService.LogFullManually("UpdateQuestionTempWithTextBox", "Question",
                                                vm.QuestionId, null, "QuestionController", "POST", user.Fullname, "",
                                                JsonConvert.SerializeObject(questionTemp), JsonConvert.SerializeObject(oldQuestionTemp));
                    importService.UpdateQuestionTempWithTextarea(questionTemp);
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
        public ActionResult EditQuestionTempWithTextbox(int id)
        {
            var questiontemp = importService.GetQuestionTemp(id);
            return View("EditQuestionTempWithTextbox", questiontemp);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Get Question Import", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Question", Message = "Get Question Temp", Method = "GET")]
        public ActionResult GetQuestionTemp(int tempId)
        {
            var questiontemp = importService.GetQuestionTemp(tempId);
            TempData["active"] = "All Import";
            return View(questiontemp);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Add Question to Bank", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Question", Message = "Add Question To Bank", Method = "GET")]
        public ActionResult AddToBank(int importId)
        {
            Task.Factory.StartNew(() => {
                importService.ImportToBank(importId);
            });
            ViewBag.Message = "Your questions are processing";
            ViewBag.Status = ToastrEnum.Info;

            return RedirectToAction("Index", "Home");
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Cancel Import", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Question", Message = "Cancel Import", Method = "GET")]
        public ActionResult Cancel(int importId)
        {
            importService.Cancel(importId);
            ViewBag.Message = "Your import is canceled";
            ViewBag.Status = ToastrEnum.Success;
            return RedirectToAction("Index", "Home");
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Delete Invalid Question", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Question", Message = "Delete Question", Method = "GET")]
        public ActionResult Delete(int questionId, string url)
        {
            importService.UpdateQuestionTempStatus(questionId, (int)StatusEnum.Deleted);
            return Redirect(url);

            //return RedirectToAction("GetResult", new { importId = importId });
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Accept Invalid Question", "QBCS", protectType: ProtectType.Authorized)]
        [LogAction(Action = "Question", Message = "Accept Invalid Question", Method = "GET")]
        public ActionResult Skip(int questionId, string url)
        {
            importService.UpdateQuestionTempStatus(questionId, (int)StatusEnum.Success);
            return Redirect(url);
            //return RedirectToAction("GetResult", new { importId = importId });
        }

        [Feature(FeatureType.Page, "Get multiple compare question", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult GetDuplicatedDetail(int id)
        {
            var model = importService.GetDuplicatedDetail(id);

            return View(model);
        }

        [Feature(FeatureType.Page, "Recovery deleted question", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult Recovery(int tempId, string url)
        {
            importService.RecoveryQuestionTemp(tempId);
            return Redirect(url);
            //return RedirectToAction("GetResult", new { importId = importId });
        }

        [Feature(FeatureType.BusinessLogic, "Get partial editable", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult GetPartialTable(int importId, int status)
        {
            var result = importService.GetListQuestionTempByStatus(importId, status);
            TempData["active"] = "All Imports";
            ViewBag.tableId = "tableEditable";
            return PartialView("_ListQuestionWithDuplicate", result);
        }
    }
}