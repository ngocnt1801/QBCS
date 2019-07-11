using AuthLib.Module;
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

        public ImportController()
        {
            importService = new ImportService();
        }

        //Lecturer
        // GET: Import
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "List User Imports", "QBCS", protectType: ProtectType.Authorized, ShortName = "Import History", InternalId = (int)SideBarEnum.ImportByUser)]
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
        public ActionResult AllImport()
        {
            var model = importService.GetListImport(null);
            TempData["active"] = "All Imports";
            return View("Index", model);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Import Result", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult GetResult(int importId)
        {
            var result = importService.GetImportResult(importId);
            TempData["active"] = "All Imports";

            if (result.Status != (int)StatusEnum.Done)
            {
                //return View(result);
                return View(result.Id);
            }
            TempData["NewestCount"] = result.NumberOfSuccess;
            return RedirectToAction("CourseDetail", "Course", new { courseId = result.CourseId});
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Edit Question Import", "QBCS", protectType: ProtectType.Authorized)]
        [ValidateInput(false)]
        public ActionResult EditQuestion(QuestionTempViewModel model)
        {
            importService.UpdateQuestionTemp(model);
            TempData["active"] = "All Imports";
            return RedirectToAction("GetResult", new { importId = model.ImportId });
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Get Question Import", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult GetQuestionTemp(int tempId)
        {
            var questiontemp = importService.GetQuestionTemp(tempId);
            TempData["active"] = "All Import";
            return View(questiontemp);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Add Question to Bank", "QBCS", protectType: ProtectType.Authorized)]
        [Log(Action = "Save", TargetName = "Question", IdParamName = "importId")]
        public ActionResult AddToBank(int importId)
        {
            Task.Factory.StartNew(() => {
                importService.ImportToBank(importId);
            });
            TempData["Message"] = "Your questions are processing";
            TempData["Status"] = ToastrEnum.Info;

            return RedirectToAction("Index", "Home");
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Cancel Import", "QBCS", protectType: ProtectType.Authorized)]
        [Log(Action = "Cancel", TargetName = "Question", IdParamName = "importId")]
        public ActionResult Cancel(int importId)
        {
            importService.Cancel(importId);
            TempData["Message"] = "Your import is canceled";
            TempData["Status"] = ToastrEnum.Success;
            return RedirectToAction("Index", "Home");
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Delete Invalid Question", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult Delete(int questionId, int importId)
        {
            importService.UpdateQuestionTempStatus(questionId, (int)StatusEnum.Delete);
            return RedirectToAction("GetResult", new { importId = importId });
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Accept Invalid Question", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult Skip(int questionId, int importId)
        {
            importService.UpdateQuestionTempStatus(questionId, (int)StatusEnum.Success);
            return RedirectToAction("GetResult", new { importId = importId });
        }
    }
}