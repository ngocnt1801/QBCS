﻿using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{

    public class ExaminationController : Controller
    {
        private ITopicService topicService;
        private ILearningOutcomeService learningOutcomeService;
        private IExaminationService examinationService;
        private IPartOfExamService partOfExamService;
        private ICategoryService categoryService;
        private ISemesterService semesterService;
        public ExaminationController()
        {
            topicService = new TopicService();
            learningOutcomeService = new LearningOutcomeService();
            examinationService = new ExaminationService();
            partOfExamService = new PartOfExamService();
            categoryService = new CategoryService();
            semesterService = new SemesterService();
        }
        public ActionResult GenerateExam(int courseId)
        {
            List<LearningOutcomeViewModel> learningOutcomeViewModels = learningOutcomeService.GetLearningOutcomeByCourseId(courseId);
            List<CategoryViewModel> categoryViewModels = categoryService.GetCategoriesByCourseId(courseId);
            List<SemesterViewModel> semester = semesterService.GetAllSemester();
            ListLearningOutcomeViewModel listTopicLearningOutcomeViewModel = new ListLearningOutcomeViewModel()
            {
                LearningOutcomes = learningOutcomeViewModels,
                Categories = categoryViewModels,
                Semester = semester
            };
            return View(listTopicLearningOutcomeViewModel);
        }
        public ActionResult GenerateExaminaton(GenerateExamViewModel exam)
        {
            GenerateExamViewModel examination = examinationService.GenerateExamination(exam);           
            return View(examination);
        }

        public ActionResult ViewGeneratedExamination(string examGroup)
        {
            List<ExaminationViewModel> exams = examinationService.GetExamByExamGroup(examGroup);
            return View(exams);
        }
        public ActionResult GetAllExamination()
        {
            List<ExaminationViewModel> exams = examinationService.GetAllExam();
            return View("ListExamination",exams);
        }
        public ActionResult DetailExam(int examId)
        {
            ExaminationViewModel exam = examinationService.GetExanById(examId);
            return View(exam);
        }
        public ActionResult DisableExam(int examId)
        {
            examinationService.DisableEaxam(examId);
            return RedirectToAction("GetAllExamination", "Examination");
        }

        public ActionResult GetHistoryCourse(int courseId)
        {
            var listQuestion = examinationService.GetExaminationHistoryQuestionsInCourse(courseId);

            return View(listQuestion);
        }

    }
}