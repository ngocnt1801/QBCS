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

        // GET: Import
        public ActionResult Index()
        {
            int userId = ((UserViewModel)Session["user"]).Id;
            var model = importService.GetListImport(userId);
            TempData["active"] = "Import";
            return View(model);
        }

        public ActionResult GetResult(int importId)
        {
            var result = importService.GetImportResult(importId);
            TempData["active"] = "Import";

            if (result.Status != (int)StatusEnum.Done)
            {
                return View(result);
            }
            TempData["NewestCount"] = result.NumberOfSuccess;
            return RedirectToAction("CourseDetail", "Course", new { courseId = result.CourseId});
        }

        [ValidateInput(false)]
        public ActionResult EditQuestion(QuestionTempViewModel model)
        {
            importService.UpdateQuestionTemp(model);
            return RedirectToAction("GetResult", new { importId = model.ImportId });
        }

        public ActionResult GetQuestionTemp(int tempId)
        {
            var questiontemp = importService.GetQuestionTemp(tempId);
            TempData["active"] = "Import";
            return View(questiontemp);
        }

        public ActionResult AddToBank(int importId)
        {
            Task.Factory.StartNew(() => {
                importService.ImportToBank(importId);
            });
            TempData["Message"] = "Your questions are processing";
            TempData["Status"] = ToastrEnum.Info;
            return RedirectToAction("Index", "Home");
        }
    
        public ActionResult Cancel(int importId)
        {
            importService.Cancel(importId);
            TempData["Message"] = "Your import is canceled";
            TempData["Status"] = ToastrEnum.Success;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(int questionId, int importId)
        {
            importService.UpdateQuestionTempStatus(questionId, (int)StatusEnum.Delete);
            return RedirectToAction("GetResult", new { importId = importId });
        }

        public ActionResult Skip(int questionId, int importId)
        {
            importService.UpdateQuestionTempStatus(questionId, (int)StatusEnum.Success);
            return RedirectToAction("GetResult", new { importId = importId });
        }
    }
}