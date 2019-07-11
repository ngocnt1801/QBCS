using AuthLib.Module;
using QBCS.Service.Enum;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    [CheckSession]
    public class ScriptController : Controller
    {
        private IScriptService scriptService;

        public ScriptController()
        {
            scriptService = new ScriptService();
        }

        // GET: Script
        [Feature(FeatureType.SideBar
            , "Show Edit Scripts Page"
            , "QBCS", protectType: ProtectType.Authorized
            , ShortName = "ShowScript"
            , InternalId = 16)]
        public ActionResult Index()
        {
            return View();
        }

        [Feature(FeatureType.Page
            , "Run Scripts"
            , "QBCS", protectType: ProtectType.Authorized
            , ShortName = "RunScript")]
        public ActionResult Run(string raw)
        {
            scriptService.RunScirpt(raw);
            TempData["Message"] = "Script run successfully";
            TempData["Status"] = ToastrEnum.Success;
            return RedirectToAction("Index");
        }
    }
}