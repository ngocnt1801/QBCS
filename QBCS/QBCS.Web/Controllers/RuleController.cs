using AuthLib.Module;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    public class RuleController : Controller
    {
        // GET: Rule
        private IRuleService ruleService;
        public RuleController()
        {
            ruleService = new RuleService();
        }
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "Get All Rules", "QBCS", protectType: ProtectType.Authorized)]
        public ActionResult Index()
        {
            List<RuleViewModel> listRule = ruleService.getAllRule();
            List<List<RuleViewModel>> result = new List<List<RuleViewModel>>();
            result.Add(listRule.Where(r => r.GroupType == 1).ToList());
            result.Add(listRule.Where(r => r.GroupType == 2).ToList());
            result.Add(listRule.Where(r => r.GroupType == 3).ToList());
            result.Add(listRule.Where(r => r.GroupType == 4).ToList());
            return View(result);
        }
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Get Rule Detail", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(RuleController), nameof(RuleController.UpdateAllRule))]
        public ActionResult Edit()
        {
            List<RuleViewModel> listRule = ruleService.getAllRule();
            List<List<RuleViewModel>> result = new List<List<RuleViewModel>>();
            result.Add(listRule.Where(r => r.GroupType == 1).ToList());
            result.Add(listRule.Where(r => r.GroupType == 2).ToList());
            result.Add(listRule.Where(r => r.GroupType == 3).ToList());
            result.Add(listRule.Where(r => r.GroupType == 4).ToList());
            return View(result);
        }
        //Staff

        [HttpPost]
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Update Rule", "QBCS", protectType: ProtectType.Authorized)]
        public JsonResult UpdateAllRule(List<RuleAjaxHandleViewModel> rules)
        {
            var result = ruleService.UpdateRule(rules);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}