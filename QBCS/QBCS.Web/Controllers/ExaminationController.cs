using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace QBCS.Web.Controllers
{
    
    public class ExaminationController : Controller
    {
        private const string COMMENT_SWITCH_CATEGORY_LINE = "// question: 0  name: Switch category to $course$/{0}/{1}/{2}";
        private const string CATEGORY_LINE = "$CATEGORY: $course$/{0}/{1}/{2}";
        private const string QUESTION_COMMENT = "// question: {0}  name: {1}";
        private const string QUESTION_TITLE = "::{0}::{1}{";
        private ITopicService topicService;
        private ILearningOutcomeService learningOutcomeService;
        private IExaminationService examinationService;
        private IPartOfExamService partOfExamService;
        public ExaminationController()
        {
            topicService = new TopicService();
            learningOutcomeService = new LearningOutcomeService();
            examinationService = new ExaminationService();
            partOfExamService = new PartOfExamService();
        }
        public ActionResult GenerateExam(int courseId)
        {
            List<TopicViewModel> topicViewModels = topicService.GetTopicByCourseId(courseId);
            List<LearningOutcomeViewModel> learningOutcomeViewModels = learningOutcomeService.GetLearningOutcomeByCourseId(courseId);
            ListTopicLearningOutcomeViewModel listTopicLearningOutcomeViewModel = new ListTopicLearningOutcomeViewModel()
            {
                Topics = topicViewModels,
                LearningOutcomes = learningOutcomeViewModels
            };
            return View(listTopicLearningOutcomeViewModel);
        }
        public ActionResult GenerateExaminaton(GenerateExamViewModel exam)
        {
            GenerateExamViewModel examination = examinationService.GenerateExamination(exam);
            return View(examination);
        }

        public ActionResult ViewGeneratedExamination(int examinationId)
        {
            List<PartOfExamViewModel> partOfExams = partOfExamService.GetPartOfExamByExamId(examinationId);
            return View(partOfExams);
        }
        public HttpResponseMessage ExportExamination(int examinationId, string fileExtension)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            List<PartOfExamViewModel> partOfExams = partOfExamService.GetPartOfExamByExamId(examinationId);
            if (fileExtension.Equals("xml"))
            {

            } else
            {
                string filePath = "/Files/";
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + filePath + "/export.txt";
                using (StreamWriter writer = new StreamWriter(fullPath))
                {
                    foreach (var part in partOfExams)
                    {
                        string switchCategoryLine = String.Format(COMMENT_SWITCH_CATEGORY_LINE,"XML" , part.Topic.Name, part.Question.First().Level.Name);
                        writer.WriteLine(switchCategoryLine);
                        string categoryLine = String.Format(CATEGORY_LINE, "XML", part.Topic.Name, part.Question.First().Level.Name);
                        writer.WriteLine(categoryLine);
                        writer.WriteLine();
                        writer.WriteLine();
                        for (int i = 0; i < part.Question.Count; i++)
                        {
                            QuestionViewModel question = part.Question[i];
                            if (i != 0)
                            {
                                if(question.LevelId != part.Question[i - 1].LevelId)
                                {
                                    switchCategoryLine = String.Format(COMMENT_SWITCH_CATEGORY_LINE, "XML", part.Topic.Name, question.Level.Name);
                                    writer.WriteLine(switchCategoryLine);
                                    categoryLine = String.Format(CATEGORY_LINE, "XML", part.Topic.Name, question.Level.Name);
                                    writer.WriteLine(categoryLine);
                                    writer.WriteLine();
                                    writer.WriteLine();
                                }
                            }
                            string questionComment = String.Format(QUESTION_COMMENT, question.Id, question.QuestionCode);
                            writer.WriteLine(questionComment);
                            string questionTitle = String.Format(QUESTION_TITLE, question.QuestionCode, question.QuestionContent);
                            writer.WriteLine(questionTitle);
                        }
                    }
                }
            }
            return result;
        }
    }
}