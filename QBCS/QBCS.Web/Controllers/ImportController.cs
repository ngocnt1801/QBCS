using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using System;
using System.Collections.Generic;
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
            return View();
        }

        public ActionResult GetResult(int importId)
        {
            var result = importService.GetImportResult(importId);
            if (result.Status != (int)StatusEnum.Done)
            {
                return View(result);
            }
            TempData["NewestCount"] = result.NumberOfSuccess;
            return RedirectToAction("GetListQuestion", "Question", new { courseId = result.CourseId});
        }

        public ActionResult EditQuestion(QuestionTempViewModel model)
        {
            importService.UpdateQuestionTemp(model);
            return RedirectToAction("GetResult", new { importId = model.ImportId });
        }

        public ActionResult GetQuestionTemp(int tempId)
        {
            var questiontemp = importService.GetQuestionTemp(tempId);
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
    }
}