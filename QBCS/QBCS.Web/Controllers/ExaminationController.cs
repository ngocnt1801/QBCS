using QBCS.Service.Implement;
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
        private const string COMMENT_SWITCH_CATEGORY_LINE = "// question: 0  name: Switch category to $course$/{0}/{1}/{2}";
        private const string CATEGORY_LINE = "$CATEGORY: $course$/{0}/{1}/{2}";
        private const string QUESTION_COMMENT = "// question: {0}  name: {1}";
        private const string QUESTION_TITLE = "::{0}::[html]{1}";
        private const string OPTION_TRUE = "={0}";
        private const string OPTION_FALSE = "~{0}";
        private ITopicService topicService;
        private ILearningOutcomeService learningOutcomeService;
        private IExaminationService examinationService;
        private IPartOfExamService partOfExamService;
        private ICategoryService categoryService;
        public ExaminationController()
        {
            topicService = new TopicService();
            learningOutcomeService = new LearningOutcomeService();
            examinationService = new ExaminationService();
            partOfExamService = new PartOfExamService();
            categoryService = new CategoryService();
        }
        public ActionResult GenerateExam(int courseId)
        {
            List<TopicViewModel> topicViewModels = topicService.GetTopicByCourseId(courseId);
            List<LearningOutcomeViewModel> learningOutcomeViewModels = learningOutcomeService.GetLearningOutcomeByCourseId(courseId);
            List<CategoryViewModel> categoryViewModels = categoryService.GetCategoriesByCourseId(courseId);
            ListTopicLearningOutcomeViewModel listTopicLearningOutcomeViewModel = new ListTopicLearningOutcomeViewModel()
            {
                Topics = topicViewModels,
                LearningOutcomes = learningOutcomeViewModels,
                Categories = categoryViewModels
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

            }
            else
            {
                string filePath = "/Files/";
                string fullPath = AppDomain.CurrentDomain.BaseDirectory + filePath + "export.txt";
                string path = AppDomain.CurrentDomain.BaseDirectory + filePath;
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                using (StreamWriter writer = new StreamWriter(Path.Combine(path, "export.txt")))
                {
                    foreach (var part in partOfExams)
                    {
                        string switchCategoryLine = String.Format(COMMENT_SWITCH_CATEGORY_LINE, "XML", part.Topic.Name, part.Question.First().Level.Name);
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
                                if (question.LevelId != part.Question[i - 1].LevelId)
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
                            string questionTitle = String.Format(QUESTION_TITLE, question.QuestionCode, question.QuestionContent) + "{";
                            writer.WriteLine(questionTitle);
                            foreach (var option in question.Options)
                            {
                                if (option.IsCorrect == true)
                                {
                                    string optionString = String.Format(OPTION_TRUE, option.OptionContent);
                                    writer.WriteLine(optionString);
                                }
                                else
                                {
                                    string optionString = String.Format(OPTION_FALSE, option.OptionContent);
                                    writer.WriteLine(optionString);
                                }
                            }
                            writer.WriteLine("}");
                            writer.WriteLine();
                        }
                    }
                }
                MemoryStream responseStream = new MemoryStream();
                Stream fileStream = System.IO.File.Open(fullPath, FileMode.Open);

                fileStream.CopyTo(responseStream);
                fileStream.Close();
                responseStream.Position = 0;
                //var stream = new FileStream(fullPath, FileMode.Open);

                HttpResponseMessage response = new HttpResponseMessage();
                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StreamContent(responseStream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                return response;

                string data = "";
                foreach (var part in partOfExams)
                {
                    string switchCategoryLine = String.Format(COMMENT_SWITCH_CATEGORY_LINE, "XML", part.Topic.Name, part.Question.First().Level.Name);
                    data += switchCategoryLine + Environment.NewLine;
                    string categoryLine = String.Format(CATEGORY_LINE, "XML", part.Topic.Name, part.Question.First().Level.Name);
                    data += categoryLine + Environment.NewLine;
                    data += Environment.NewLine;
                    data += Environment.NewLine;
                    for (int i = 0; i < part.Question.Count; i++)
                    {
                        QuestionViewModel question = part.Question[i];
                        if (i != 0)
                        {
                            if (question.LevelId != part.Question[i - 1].LevelId)
                            {
                                switchCategoryLine = String.Format(COMMENT_SWITCH_CATEGORY_LINE, "XML", part.Topic.Name, question.Level.Name);
                                data += switchCategoryLine + Environment.NewLine;
                                categoryLine = String.Format(CATEGORY_LINE, "XML", part.Topic.Name, question.Level.Name);
                                data += switchCategoryLine + Environment.NewLine;
                                data += Environment.NewLine;
                                data += Environment.NewLine;
                            }
                        }
                        string questionComment = String.Format(QUESTION_COMMENT, question.Id, question.QuestionCode);
                        data += questionComment + Environment.NewLine;
                        string questionTitle = String.Format(QUESTION_TITLE, question.QuestionCode, question.QuestionContent) + "{";
                        data += questionTitle + Environment.NewLine;
                        foreach (var option in question.Options)
                        {
                            if (option.IsCorrect == true)
                            {
                                string optionString = String.Format(OPTION_TRUE, option.OptionContent);
                                data += optionString + Environment.NewLine;
                            }
                            else
                            {
                                string optionString = String.Format(OPTION_FALSE, option.OptionContent);
                                data += optionString + Environment.NewLine;
                            }
                        }
                        data += "}" + Environment.NewLine;
                        data += Environment.NewLine;
                    }
                }
                var byteArray = Encoding.ASCII.GetBytes(data);
                var stream = new MemoryStream(byteArray);

                HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                httpResponseMessage.StatusCode = HttpStatusCode.OK;
                httpResponseMessage.Content = new StreamContent(stream);
                httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                httpResponseMessage.Content.Headers.ContentDisposition.FileName = "export.txt";
                httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                return httpResponseMessage;
                //return File(stream, "text/plain");
            }
            return result;
        }
    }
}