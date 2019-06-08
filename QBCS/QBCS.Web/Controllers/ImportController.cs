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
            return View(result);
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

            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}