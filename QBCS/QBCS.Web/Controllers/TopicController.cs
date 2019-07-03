﻿using AuthLib.Module;
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
    public class TopicController : Controller
    {
        private ITopicService topicService;
        private ICourseService courseService;
        public TopicController()
        {
            topicService = new TopicService();
            courseService = new CourseService();
        }
        // GET: Topic
        public ActionResult Index()
        {
            var list = topicService.GetAllTopic();
            return View(list);
        }
        public ActionResult Add()
        {
            var topic = new TopicViewModel();
            return View(topic);
        }
        [HttpPost]
        public JsonResult Add(TopicViewModel topic)
        {
            bool result = false;
            result = topicService.AddTopic(topic);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(int id)
        {
            var result = topicService.GetTopicById(id);
            return View(result);
        }
        [HttpPost]
        public JsonResult Edit(TopicViewModel topic)
        {
            bool result = false;
            result = topicService.UpdateTopic(topic);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Lecturer
        //stpm: feature declare
        [Feature(FeatureType.Page, "Get All Course API", "QBCS", protectType: ProtectType.Authorized)]
        public JsonResult LoadCourse()
        {
            var result = courseService.GetAllCourses();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateDisable(int itemId)
        {
            var update = topicService.UpdateDisable(itemId);
            return RedirectToAction("Index");
        }
    }
}