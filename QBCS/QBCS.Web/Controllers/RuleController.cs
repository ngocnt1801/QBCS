using AuthLib.Module;
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
    public class RuleController : Controller
    {
        // GET: Rule
        private IRuleService ruleService;
        private ILogService logService;
        public RuleController()
        {
            ruleService = new RuleService();
            logService = new LogService();
        }
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.SideBar, "Get All Rules", "QBCS", protectType: ProtectType.Authorized, ShortName = "Rule", InternalId = (int)SideBarEnum.AllRule)]
        [LogAction(Action = "Rule", Message = "Get All Rules", Method = "GET")]
        public ActionResult Index()
        {
            List<RuleViewModel> listRule = ruleService.getAllRule();
            List<List<RuleViewModel>> result = new List<List<RuleViewModel>>();
            result.Add(listRule.Where(r => r.GroupType == 1).ToList());
            result.Add(listRule.Where(r => r.GroupType == 2).ToList());
            result.Add(listRule.Where(r => r.GroupType == 3).ToList());
            result.Add(listRule.Where(r => r.GroupType == 4).ToList());
            TempData["active"] = "Rule";
            return View(result);
        }
        //Staff
        //stpm: feature declare
        [Feature(FeatureType.Page, "Get Rule Detail", "QBCS", protectType: ProtectType.Authorized)]
        //stpm: dependency declare
        [Dependency(typeof(RuleController), nameof(RuleController.UpdateAllRule))]
        [LogAction(Action = "Rule", Message = "Edit Rule", Method = "GET")]
        public ActionResult Edit()
        {
            List<RuleViewModel> listRule = ruleService.getAllRule();
            List<List<RuleViewModel>> result = new List<List<RuleViewModel>>();
            result.Add(listRule.Where(r => r.GroupType == 1).ToList());
            result.Add(listRule.Where(r => r.GroupType == 2).ToList());
            result.Add(listRule.Where(r => r.GroupType == 3).ToList());
            result.Add(listRule.Where(r => r.GroupType == 4).ToList());
            TempData["active"] = "Rule";
            return View(result);
        }
        //Staff

        [HttpPost]
        //stpm: feature declare
        [Feature(FeatureType.BusinessLogic, "Update Rule", "QBCS", protectType: ProtectType.Authorized)]

        [LogAction(Action = "Rule", Message = "Update Rule", Method = "POST")]
        
        public JsonResult UpdateAllRule(List<RuleAjaxHandleViewModel> rules)
        {
           
            string newValue = "";
            string oldValue = "";
            List<RuleViewModel> oldRules = ruleService.getAllRule();

            oldValue = JsonConvert.SerializeObject(oldRules);
            newValue = JsonConvert.SerializeObject(rules);
            var result = ruleService.UpdateRule(rules);
            // logService.LogManually("Update", "Rule", controller: "Rule", method: "UpdateAllRule", fullname: User.Get(u => u.FullName), usercode: User.Get(u => u.Code));
            logService.LogFullManually("Update", "Rule", controller: "Rule",
                        method: "UpdateAllRule", fullname: User.Get(u => u.FullName), 
                        usercode: User.Get(u => u.Code),
                        newValue: newValue,
                        oldValue: oldValue);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}