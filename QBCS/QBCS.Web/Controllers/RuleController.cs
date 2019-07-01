﻿using QBCS.Service.Implement;
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
        public JsonResult UpdateAllRule(List<RuleAjaxHandleViewModel> rules)
        {
            var result = ruleService.UpdateRule(rules);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}