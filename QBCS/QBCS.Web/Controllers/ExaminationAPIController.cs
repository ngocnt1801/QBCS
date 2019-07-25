
using AuthLib.Module;
using DuplicateQuestion.Entity;
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.Utilities;
using QBCS.Service.ViewModel;
using QBCS.Web.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web.Mvc;
using System.Xml;

namespace QBCS.Web.Controllers
{
    [CheckSession]
    public class ExaminationAPIController : Controller
    {
        private const string COMMENT_SWITCH_CATEGORY_LINE = "// question: 0  name: Switch category to $course$/{0}/{1}/{2}";
        private const string CATEGORY_LINE = "$CATEGORY: $course$/{0}/{1}/{2}";
        private const string QUESTION_COMMENT = "// question: {0}  name: {1}";
        //private const string QUESTION_TITLE = "::{0}::[html]{1}";
        private const string QUESTION_TITLE = "::{0}::{1}";
        private const string OPTION_TRUE = "={0}";
        private const string OPTION_FALSE = "~{0}";
        private const string XML_COMMENT_CATEGORY = " question: 0";
        private const string XML_QUESTION_TAG = "question";
        private const string XML_CATEGORY_TAG = "category";
        private const string XML_TYPE_ATTR_NAME = "type";
        private const string XML_CATEGORY_ATTR_VALUE = "category";
        private const string XML_TEXT_TAG = "text";
        private const string XML_SWITCH_CATEGORY = "$course$/{0}/{1}/{2}";
        private const string XML_MULTICHOICE_ATTR_VALUE = "multichoice";
        private const string XML_COMMENT_QUESTION = " question: {0}";
        private const string XML_NAME_TAG = "name";
        private const string XML_QUESTIONTEXT_TAG = "questiontext";
        private const string XML_GENRALFEEDBACK_TAG = "generalfeedback";
        private const string XML_FORMAT_ATTR_NAME = "format";
        private const string XML_HTML_ATTR_VALUE = "html";
        private const string SpecialChars = @"<>&";
        private const string XML_DEFAULTGRADE_TAG = "defaultgrade";
        private const double DEFAULTGRADE_VALUE = 1;
        private const string XML_PENALTY_TAG = "penalty";
        private const double PENALTY_VALUE = 0.3333333;
        private const string XML_HIDDEN_TAG = "hidden";
        private const int HIDDEN_VALUE = 0;
        private const string XML_SINGLE_TAG = "single";
        private const bool SINGLE_VALUE = true;
        private const string XML_SHUFFLEANSWERS_TAG = "shuffleanswers";
        private const bool SHUFFLEANSWERS_VALUE = true;
        private const string XML_ANSWERNUMBERING_TAG = "answernumbering";
        private const string ANSWERNUMBERING_VALUE = "abc";
        private const string XML_CORRECTFEEDBACK_TAG = "correctfeedback";
        private const string XML_PARTIALLYCORRECTFEEDBACK_TAG = "partiallycorrectfeedback";
        private const string XML_INCORRECTFEEDBACK_TAG = "incorrectfeedback";
        private const string XML_ANSWER_TAG = "answer";
        private const string XML_FRACTION_ATTR_NAME = "fraction";
        private const int XML_CORRECT_FRACTION_ATTR_VALUE = 100;
        private const int XML_INCORRECT_FRACTION_ATTR_VALUE = 0;
        private const string XML_FEEDBACK_TAG = "feedback";
        private const string XML_FILE_TAG = "file";
        private const string XML_NAME_ATTR_NAME = "name";
        private const string XML_PATH_ATTR_NAME = "path";
        private const string XML_ENCODING_ATTR_NAME = "encoding";
        private const string XML_ENCODING_ATTR_VALUE = "base64";
        private const string XML_PATH_ATTR_VALUE = "/";
        private const string XML_NAME_ATTR_VALUE = "Image00613.bmp";
        private const string HTML_IMAGE_TAG = "<img src='@@PLUGINFILE@@/{0}' alt='' role='presentation' style=''>";

        private IPartOfExamService partOfExamService;
        private IExaminationService examinationService;
        private ILogService logService;
        private IQuestionService questionService;

        public ExaminationAPIController()
        {
            partOfExamService = new PartOfExamService();
            examinationService = new ExaminationService();
            logService = new LogService();
            questionService = new QuestionService();

        }

        //Staff

        [HttpGet]
        [ActionName("export")]
        //stpm: feature declare
        [Feature(FeatureType.Page, "Export Examination", "QBCS", protectType: ProtectType.Authorized)]
        public FileResult ExportExamination(int examinationId, string fileExtension)
        {

            logService.LogManually("Export", "Examination", targetId: examinationId, controller: "ExaminationAPI", method: "ExportExamination", fullname: User.Get(u => u.FullName), usercode: User.Get(u => u.Code));

            ExaminationViewModel exam = examinationService.GetExanById(examinationId);
            string fileName = exam.Course.Code + "_" + exam.ExamGroup + "_" + exam.ExamCode + "_" + DateTime.Now.ToString("yyyyMMdd");
            List<QuestionInExamViewModel> questions = new List<QuestionInExamViewModel>();
            foreach (var part in exam.PartOfExam)
            {
                questions = questions.Concat(part.Question).ToList();
            }
            if (fileExtension.ToLower().Equals("xml"))
            {
                byte[] byteArray = ExportToXMLFile(questions);
                return File(byteArray, System.Net.Mime.MediaTypeNames.Application.Octet, fileName + ".xml");

            }
            else
            {
                byte[] byteArray = ExportToGIFTFile(questions);
                return File(byteArray, System.Net.Mime.MediaTypeNames.Application.Octet, fileName + ".txt");
            }
        }

        [HttpGet]
        [ActionName("exportAll")]
        //[Feature(FeatureType.Page, "Export Group Examination ", "QBCS", protectType: ProtectType.Authorized)]
        public FileResult ExportGroupExamination(string examGroup, string fileExtension)
        {
            string zipFileName = null;
            List<ExaminationViewModel> exams = examinationService.GetExamByExamGroup(examGroup);
            //the output bytes of the zip file
            byte[] fileBytes = null;
            //create working memory stream of zip file
            using (MemoryStream memoryStream = new MemoryStream())
            {
                //create a zip file
                using (ZipArchive zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    //file xml
                    if (fileExtension.ToLower().Equals("xml"))
                    {
                        foreach (var exam in exams)
                        {
                            string fileName = exam.Course.Code + "_" + exam.ExamGroup + "_" + exam.ExamCode + "_" + DateTime.Now.ToString("yyyyMMdd");
                            List<QuestionInExamViewModel> questions = new List<QuestionInExamViewModel>();
                            foreach (var part in exam.PartOfExam)
                            {
                                questions = questions.Concat(part.Question).ToList();
                            }
                            if (zipFileName == null)
                            {
                                zipFileName = exam.Course.Code + "_" + exam.ExamGroup + "_" + DateTime.Now.ToString("yyyyMMdd") + "_XML";
                            }
                            //add item to zip
                            ZipArchiveEntry zipItem = zip.CreateEntry(fileName + ".xml");

                            //stream for item

                            byte[] itemBytes = ExportToXMLFile(questions);
                            using (Stream entryStream = zipItem.Open())
                            {
                                Stream itemStream = new MemoryStream(itemBytes);
                                itemStream.CopyTo(entryStream);
                            }
                        }
                        // file GIFT
                    }
                    else
                    {
                        foreach (var exam in exams)
                        {
                            string fileName = exam.Course.Code + "_" + exam.ExamGroup + "_" + exam.ExamCode + "_" + DateTime.Now.ToString("yyyyMMdd");
                            List<QuestionInExamViewModel> questions = new List<QuestionInExamViewModel>();
                            foreach (var part in exam.PartOfExam)
                            {
                                questions = questions.Concat(part.Question).ToList();
                            }
                            if (zipFileName == null)
                            {
                                zipFileName = exam.Course.Code + "_" + exam.ExamGroup + "_" + DateTime.Now.ToString("yyyyMMdd") + "_GIFT";
                            }
                            //add item to zip
                            ZipArchiveEntry zipItem = zip.CreateEntry(fileName + ".txt");
                            //stream for item
                            byte[] itemBytes = ExportToGIFTFile(questions);
                            using (Stream entryStream = zipItem.Open())
                            {
                                Stream itemStream = new MemoryStream(itemBytes);
                                itemStream.CopyTo(entryStream);
                            }
                        }
                    }

                }
                fileBytes = memoryStream.ToArray();
            }
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, zipFileName + ".zip");
        }

        public FileResult ExportBank(int[] loId, string extension, bool? getCategory = false)
        {
            var result = new List<QuestionViewModel>();
            if (loId != null)
            {
                foreach (var id in loId)
                {
                    var questionsByLo = questionService.GetQuestionList(null, null, id, null, null);
                    if (questionsByLo != null && questionsByLo.Count > 0)
                    {
                        result.AddRange(questionsByLo);
                    }
                }

            }
            List<QuestionInExamViewModel> questions = new List<QuestionInExamViewModel>();
            for (int i = 0; i < result.Count; i++)
            {
                QuestionInExamViewModel question = new QuestionInExamViewModel
                {
                    Id = result[i].Id,
                    QuestionCode = result[i].Code,
                    QuestionContent = result[i].QuestionContent,
                    Image = result[i].Image,
                    Options = result[i].Options,
                    Category = new CategoryViewModel
                    {
                        Id = result[i].CategoryId,
                        Name = result[i].Category
                    },
                    LearningOutcomeName = result[i].LearningOutcomeName,
                    Level = new LevelViewModel
                    {
                        Id = result[i].LevelId,
                        Name = ((LevelEnum)result[i].LevelId).ToString()
                    }

                };
                questions.Add(question);
            }

            if (extension.ToLower().Equals("xml"))
            {
                byte[] byteArray = ExportToXMLFile(questions, getCategory);
                return File(byteArray, System.Net.Mime.MediaTypeNames.Application.Octet, "test" + ".xml");
            }
            else
            {
                byte[] byteArray = ExportToGIFTFile(questions, getCategory);
                return File(byteArray, System.Net.Mime.MediaTypeNames.Application.Octet, "test" + ".txt");
            }

        }

        private byte[] ExportToXMLFile(List<QuestionInExamViewModel> questions, bool? getCategory = false)
        {
            byte[] result = null;
            int count = 0;
            using (MemoryStream stream = new MemoryStream())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(stream, System.Text.Encoding.UTF8);
                xmlWriter.Formatting = Formatting.Indented;
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("quiz");

                //CATEGORY

                string switchCategory = "";

                for (int i = 0; i < questions.Count; i++)
                {
                    QuestionInExamViewModel question = questions[i];

                    #region category
                    if ((i == 0 || (question.LevelId != questions[i - 1].LevelId || !question.Category.Name.Equals(questions[i - 1].Category))) && (getCategory.Value))
                    {
                        xmlWriter.WriteComment(XML_COMMENT_CATEGORY);
                        xmlWriter.WriteStartElement(XML_QUESTION_TAG);
                        xmlWriter.WriteAttributeString(XML_TYPE_ATTR_NAME, XML_CATEGORY_ATTR_VALUE);
                        xmlWriter.WriteStartElement(XML_CATEGORY_TAG);
                        switchCategory = String.Format(XML_SWITCH_CATEGORY, question.Category.Name, question.LearningOutcomeName, question.Level.Name);
                        xmlWriter.WriteElementString(XML_TEXT_TAG, switchCategory);
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();
                    }
                    #endregion

                    string questionComment = String.Format(XML_COMMENT_QUESTION, question.Id);
                    xmlWriter.WriteComment(questionComment);
                    xmlWriter.WriteStartElement(XML_QUESTION_TAG);

                    xmlWriter.WriteAttributeString(XML_TYPE_ATTR_NAME, XML_MULTICHOICE_ATTR_VALUE);
                    //name tag
                    xmlWriter.WriteStartElement(XML_NAME_TAG);
                    xmlWriter.WriteElementString(XML_TEXT_TAG, question.QuestionCode);
                    xmlWriter.WriteEndElement();
                    //questiontext tag
                    if (string.IsNullOrEmpty(question.Image))
                    {
                        xmlWriter.WriteStartElement(XML_QUESTIONTEXT_TAG);
                        xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                        xmlWriter.WriteStartElement(XML_TEXT_TAG);
                        string questionContentEncode = StringUtilities.FormatStringExportXML(question.QuestionContent).Trim();
                        if (questionContentEncode.IndexOfAny(SpecialChars.ToCharArray()) != -1)
                        {
                            xmlWriter.WriteCData(questionContentEncode);
                        }
                        else
                        {
                            xmlWriter.WriteString(questionContentEncode);
                        }
                        xmlWriter.WriteEndElement();
                    }
                    else
                    {
                        //questiontext tag
                        xmlWriter.WriteStartElement(XML_QUESTIONTEXT_TAG);
                        xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                        xmlWriter.WriteStartElement(XML_TEXT_TAG);
                        string imageName = "Image" + count++ + ".png";
                        string questionContentEncode = StringUtilities.FormatStringExportXML(question.QuestionContent).Trim() + String.Format(HTML_IMAGE_TAG, imageName);
                        if (questionContentEncode.IndexOfAny(SpecialChars.ToCharArray()) != -1)
                        {
                            xmlWriter.WriteCData(questionContentEncode);
                        }
                        else
                        {
                            xmlWriter.WriteString(questionContentEncode);
                        }
                        xmlWriter.WriteEndElement();

                        //Image tag
                        xmlWriter.WriteStartElement(XML_FILE_TAG);
                        xmlWriter.WriteAttributeString(XML_NAME_ATTR_NAME, imageName);
                        xmlWriter.WriteAttributeString(XML_PATH_ATTR_NAME, XML_PATH_ATTR_VALUE);
                        xmlWriter.WriteAttributeString(XML_ENCODING_ATTR_NAME, XML_ENCODING_ATTR_VALUE);
                        xmlWriter.WriteString(question.Image);
                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement();
                    //generalfeedback tag
                    xmlWriter.WriteStartElement(XML_GENRALFEEDBACK_TAG);
                    xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                    xmlWriter.WriteStartElement(XML_TEXT_TAG);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    //defaultgrade tag
                    xmlWriter.WriteStartElement(XML_DEFAULTGRADE_TAG);
                    xmlWriter.WriteString(String.Format("{0:0.0000000}", DEFAULTGRADE_VALUE.ToString()));
                    xmlWriter.WriteEndElement();
                    //penalty tag
                    xmlWriter.WriteStartElement(XML_PENALTY_TAG);
                    xmlWriter.WriteString(PENALTY_VALUE.ToString());
                    xmlWriter.WriteEndElement();
                    //hidden tag
                    xmlWriter.WriteStartElement(XML_HIDDEN_TAG);
                    xmlWriter.WriteString(HIDDEN_VALUE.ToString());
                    xmlWriter.WriteEndElement();
                    //single tag
                    xmlWriter.WriteStartElement(XML_SINGLE_TAG);
                    xmlWriter.WriteString(SINGLE_VALUE.ToString());
                    xmlWriter.WriteEndElement();
                    //shuffleanswers tag
                    xmlWriter.WriteStartElement(XML_SHUFFLEANSWERS_TAG);
                    xmlWriter.WriteString(SHUFFLEANSWERS_VALUE.ToString());
                    xmlWriter.WriteEndElement();
                    //answernumbering tag
                    xmlWriter.WriteStartElement(XML_ANSWERNUMBERING_TAG);
                    xmlWriter.WriteString(ANSWERNUMBERING_VALUE.ToString());
                    xmlWriter.WriteEndElement();
                    //correctfeedback tag
                    xmlWriter.WriteStartElement(XML_CORRECTFEEDBACK_TAG);
                    xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                    xmlWriter.WriteStartElement(XML_TEXT_TAG);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    //partiallycorrectfeedback tag
                    xmlWriter.WriteStartElement(XML_PARTIALLYCORRECTFEEDBACK_TAG);
                    xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                    xmlWriter.WriteStartElement(XML_TEXT_TAG);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    //incorrectfeedback tag
                    xmlWriter.WriteStartElement(XML_INCORRECTFEEDBACK_TAG);
                    xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                    xmlWriter.WriteStartElement(XML_TEXT_TAG);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                    // answer tag
                    foreach (var option in question.Options)
                    {
                        if (option.IsCorrect)
                        {
                            xmlWriter.WriteStartElement(XML_ANSWER_TAG);
                            xmlWriter.WriteAttributeString(XML_FRACTION_ATTR_NAME, XML_CORRECT_FRACTION_ATTR_VALUE.ToString());
                            xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);

                            if (string.IsNullOrEmpty(option.Image))
                            {
                                //text atg
                                xmlWriter.WriteStartElement(XML_TEXT_TAG);
                                string optionContentEncode = StringUtilities.FormatStringExportXML(option.OptionContent).Trim();
                                if (optionContentEncode.IndexOfAny(SpecialChars.ToCharArray()) != -1)
                                {
                                    xmlWriter.WriteCData(optionContentEncode);
                                }
                                else
                                {
                                    xmlWriter.WriteString(optionContentEncode);
                                }
                                xmlWriter.WriteEndElement();
                            }
                            else
                            {
                                string imageName = "Image" + count++ + ".png";
                                // text atg
                                xmlWriter.WriteStartElement(XML_TEXT_TAG);
                                string optionContentEncode = StringUtilities.FormatStringExportXML(option.OptionContent).Trim() + String.Format(HTML_IMAGE_TAG, imageName);
                                if (optionContentEncode.IndexOfAny(SpecialChars.ToCharArray()) != -1)
                                {
                                    xmlWriter.WriteCData(optionContentEncode);
                                }
                                else
                                {
                                    xmlWriter.WriteString(optionContentEncode);
                                }
                                xmlWriter.WriteEndElement();

                                //Image tag
                                xmlWriter.WriteStartElement(XML_FILE_TAG);
                                xmlWriter.WriteAttributeString(XML_NAME_ATTR_NAME, imageName);
                                xmlWriter.WriteAttributeString(XML_PATH_ATTR_NAME, XML_PATH_ATTR_VALUE);
                                xmlWriter.WriteAttributeString(XML_ENCODING_ATTR_NAME, XML_ENCODING_ATTR_VALUE);
                                xmlWriter.WriteString(option.Image);
                                xmlWriter.WriteEndElement();
                            }
                            //feedback tag
                            xmlWriter.WriteStartElement(XML_FEEDBACK_TAG);
                            xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                            xmlWriter.WriteStartElement(XML_TEXT_TAG);
                            xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();

                            xmlWriter.WriteEndElement();
                        }
                        else
                        {
                            xmlWriter.WriteStartElement(XML_ANSWER_TAG);
                            xmlWriter.WriteAttributeString(XML_FRACTION_ATTR_NAME, XML_INCORRECT_FRACTION_ATTR_VALUE.ToString());
                            xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                            if (string.IsNullOrEmpty(option.Image))
                            {
                                //text atg
                                xmlWriter.WriteStartElement(XML_TEXT_TAG);
                                string optionContentEncode = StringUtilities.FormatStringExportXML(option.OptionContent).Trim();
                                if (optionContentEncode.IndexOfAny(SpecialChars.ToCharArray()) != -1)
                                {
                                    xmlWriter.WriteCData(optionContentEncode);
                                }
                                else
                                {
                                    xmlWriter.WriteString(optionContentEncode);
                                }
                                xmlWriter.WriteEndElement();
                            }
                            else
                            {
                                string imageName = "Image" + count++ + ".png";
                                // text atg
                                xmlWriter.WriteStartElement(XML_TEXT_TAG);
                                string optionContentEncode = StringUtilities.FormatStringExportXML(option.OptionContent).Trim() + String.Format(HTML_IMAGE_TAG, imageName);
                                if (optionContentEncode.IndexOfAny(SpecialChars.ToCharArray()) != -1)
                                {
                                    xmlWriter.WriteCData(optionContentEncode);
                                }
                                else
                                {
                                    xmlWriter.WriteString(optionContentEncode);
                                }
                                xmlWriter.WriteEndElement();

                                //Image tag
                                xmlWriter.WriteStartElement(XML_FILE_TAG);
                                xmlWriter.WriteAttributeString(XML_NAME_ATTR_NAME, imageName);
                                xmlWriter.WriteAttributeString(XML_PATH_ATTR_NAME, XML_PATH_ATTR_VALUE);
                                xmlWriter.WriteAttributeString(XML_ENCODING_ATTR_NAME, XML_ENCODING_ATTR_VALUE);
                                xmlWriter.WriteString(option.Image);
                                xmlWriter.WriteEndElement();
                            }
                            //feedback tag
                            xmlWriter.WriteStartElement(XML_FEEDBACK_TAG);
                            xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                            xmlWriter.WriteStartElement(XML_TEXT_TAG);
                            xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();

                            xmlWriter.WriteEndElement();
                        }
                    }

                    xmlWriter.WriteEndElement();
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
                // Convert the memory stream to an array of bytes.
                result = stream.ToArray();

                xmlWriter.Close();
            }
            return result;
        }

        private byte[] ExportToGIFTFile(List<QuestionInExamViewModel> questions, bool? getCategory = false)
        {
            byte[] result = null;
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);

                //CATEGORY
                string switchCategoryLine = "";
                string categoryLine = "";

                for (int i = 0; i < questions.Count; i++)
                {
                    QuestionInExamViewModel question = questions[i];

                    //CATEGORY
                    if ((i == 0 || (question.LevelId != questions[i - 1].LevelId || !question.Category.Name.Equals(questions[i - 1].Category))) && (getCategory.Value))
                    {
                        switchCategoryLine = String.Format(COMMENT_SWITCH_CATEGORY_LINE, question.Category.Name, question.LearningOutcomeName, question.Level.Name);
                        writer.WriteLine(switchCategoryLine);
                        categoryLine = String.Format(CATEGORY_LINE, question.Category.Name, question.LearningOutcomeName, question.Level.Name);
                        writer.WriteLine(categoryLine);
                        writer.WriteLine();
                        writer.WriteLine();
                    }

                    string questionComment = String.Format(QUESTION_COMMENT, question.Id, question.QuestionCode);
                    writer.WriteLine(questionComment);
                    string questionTitle = String.Format(QUESTION_TITLE, question.QuestionCode, StringUtilities.FormatStringExportGIFT(question.QuestionContent).Trim()) + "{";
                    writer.Write(questionTitle);
                    writer.WriteLine();
                    foreach (var option in question.Options)
                    {
                        if (option.IsCorrect == true)
                        {
                            string optionString = String.Format(OPTION_TRUE, StringUtilities.FormatStringExportGIFT(option.OptionContent).Trim());
                            writer.Write(optionString);
                            writer.WriteLine();
                        }
                        else
                        {
                            string optionString = String.Format(OPTION_FALSE, StringUtilities.FormatStringExportGIFT(option.OptionContent).Trim());
                            writer.Write(optionString);
                            writer.WriteLine();
                        }
                    }
                    writer.WriteLine("}");
                    writer.WriteLine();
                }
                writer.Flush();
                result = stream.ToArray();

                writer.Close();
            }
            return result;
        }

    }

}
