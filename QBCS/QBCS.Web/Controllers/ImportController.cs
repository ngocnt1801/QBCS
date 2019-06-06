using QBCS.Service.Implement;
using QBCS.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
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
    }
}