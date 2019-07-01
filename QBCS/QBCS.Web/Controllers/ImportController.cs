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
        [Feature(FeatureType.Page, "List Imports", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(ImportService), nameof(ImportService.GetListImport))]
        public ActionResult Index()
        {
            int userId = ((UserViewModel)Session["user"]).Id;
            var model = importService.GetListImport(userId);
            return View(model);
        }


        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Import Result", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(ImportService), nameof(ImportService.GetImportResult))]
        public ActionResult GetResult(int importId)
        {
            var result = importService.GetImportResult(importId);
          
            if (result.Status != (int)StatusEnum.Done)
            {
                return View(result);
            }
            TempData["NewestCount"] = result.NumberOfSuccess;
            return RedirectToAction("CourseDetail", "Course", new { courseId = result.CourseId});
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Edit Question Import", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(ImportService), nameof(ImportService.UpdateQuestionTemp))]
        [ValidateInput(false)]
        public ActionResult EditQuestion(QuestionTempViewModel model)
        {
            importService.UpdateQuestionTemp(model);
            return RedirectToAction("GetResult", new { importId = model.ImportId });
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Get Question Import", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(ImportService), nameof(ImportService.GetQuestionTemp))]
        public ActionResult GetQuestionTemp(int tempId)
        {
            var questiontemp = importService.GetQuestionTemp(tempId);
            return View(questiontemp);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Add Question to Bank", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(ImportService), nameof(ImportService.ImportToBank))]
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
        //stpm: dependency declare
        [Dependency(typeof(ImportService), nameof(ImportService.Cancel))]
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
        //stpm: dependency declare
        [Dependency(typeof(ImportService), nameof(ImportService.UpdateQuestionTempStatus))]
        public ActionResult Delete(int questionId, int importId)
        {
            importService.UpdateQuestionTempStatus(questionId, (int)StatusEnum.Delete);
            return RedirectToAction("GetResult", new { importId = importId });
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Accept Invalid Question", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(ImportService), nameof(ImportService.UpdateQuestionTempStatus))]
        public ActionResult Skip(int questionId, int importId)
        {
            importService.UpdateQuestionTempStatus(questionId, (int)StatusEnum.Success);
            return RedirectToAction("GetResult", new { importId = importId });
        }
    }
}