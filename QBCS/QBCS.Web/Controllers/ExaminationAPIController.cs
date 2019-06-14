﻿
using QBCS.Service.Implement;
using QBCS.Service.Interface;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace QBCS.Web.Controllers
{
    public class ExaminationAPIController : ApiController
    {
        private const string COMMENT_SWITCH_CATEGORY_LINE = "// question: 0  name: Switch category to $course$/{0}/{1}/{2}";
        private const string CATEGORY_LINE = "$CATEGORY: $course$/{0}/{1}/{2}";
        private const string QUESTION_COMMENT = "// question: {0}  name: {1}";
        private const string QUESTION_TITLE = "::{0}::[html]{1}";
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
        private IPartOfExamService partOfExamService;
        public ExaminationAPIController()
        {
            partOfExamService = new PartOfExamService();
        }
        [HttpGet]
        [ActionName("export")]
        public HttpResponseMessage ExportExamination(int examinationId, string fileExtension)
        {
            List<PartOfExamViewModel> partOfExams = partOfExamService.GetPartOfExamByExamId(examinationId);
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
            if (fileExtension.ToLower().Equals("xml"))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    XmlTextWriter xmlWriter = new XmlTextWriter(stream, System.Text.Encoding.ASCII);
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("quiz");
                    foreach (var part in partOfExams)
                    {
                        xmlWriter.WriteComment(XML_COMMENT_CATEGORY);
                        xmlWriter.WriteStartElement(XML_QUESTION_TAG);
                        xmlWriter.WriteAttributeString(XML_TYPE_ATTR_NAME, XML_CATEGORY_ATTR_VALUE);
                        xmlWriter.WriteStartElement(XML_CATEGORY_TAG);
                        string switchCategory = String.Format(XML_SWITCH_CATEGORY, "XML", part.Topic.Name, part.Question.First().Level.Name);
                        xmlWriter.WriteElementString(XML_TEXT_TAG, switchCategory);
                        xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();

                        for (int i = 0; i < part.Question.Count; i++)
                        {
                            QuestionViewModel question = part.Question[i];
                            if (i != 0)
                            {
                                if (question.LevelId != part.Question[i - 1].LevelId)
                                {
                                    xmlWriter.WriteComment(XML_COMMENT_CATEGORY);
                                    xmlWriter.WriteStartElement(XML_QUESTION_TAG);
                                    xmlWriter.WriteAttributeString(XML_TYPE_ATTR_NAME, XML_CATEGORY_ATTR_VALUE);
                                    xmlWriter.WriteStartElement(XML_CATEGORY_TAG);
                                    switchCategory = String.Format(XML_SWITCH_CATEGORY, "XML", part.Topic.Name, question.Level.Name);
                                    xmlWriter.WriteElementString(XML_TEXT_TAG, switchCategory);
                                    xmlWriter.WriteEndElement();
                                    xmlWriter.WriteEndElement();
                                }
                            }
                            string questionComment = String.Format(XML_COMMENT_QUESTION, question.Id);
                            xmlWriter.WriteComment(questionComment);
                            xmlWriter.WriteStartElement(XML_QUESTION_TAG);

                            xmlWriter.WriteAttributeString(XML_TYPE_ATTR_NAME, XML_MULTICHOICE_ATTR_VALUE);
                            //name tag
                            xmlWriter.WriteStartElement(XML_NAME_TAG);
                            xmlWriter.WriteElementString(XML_TEXT_TAG, question.QuestionCode);
                            xmlWriter.WriteEndElement();
                            //questiontext tag
                            xmlWriter.WriteStartElement(XML_QUESTIONTEXT_TAG);
                            xmlWriter.WriteStartElement(XML_TEXT_TAG);
                            if (question.QuestionContent.IndexOfAny(SpecialChars.ToCharArray()) != -1)
                            {
                                xmlWriter.WriteCData(question.QuestionContent);
                            }
                            else
                            {
                                xmlWriter.WriteString(question.QuestionContent);
                            }
                            xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();
                            //generalfeedback tag
                            xmlWriter.WriteStartElement(XML_GENRALFEEDBACK_TAG);
                            xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                            xmlWriter.WriteStartElement(XML_TEXT_TAG);
                            xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();
                            //defaultgrade tag
                            xmlWriter.WriteStartElement(XML_DEFAULTGRADE_TAG);
                            xmlWriter.WriteString(DEFAULTGRADE_VALUE.ToString());
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
                                if(option.IsCorrect)
                                {
                                    xmlWriter.WriteStartElement(XML_ANSWER_TAG);
                                    xmlWriter.WriteAttributeString(XML_FRACTION_ATTR_NAME, XML_CORRECT_FRACTION_ATTR_VALUE.ToString());
                                    xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                                    //text atg
                                    xmlWriter.WriteStartElement(XML_TEXT_TAG);
                                    if (option.OptionContent.IndexOfAny(SpecialChars.ToCharArray()) != -1)
                                    {
                                        xmlWriter.WriteCData(option.OptionContent);
                                    }
                                    else
                                    {
                                        xmlWriter.WriteString(option.OptionContent);
                                    }
                                    xmlWriter.WriteEndElement();
                                    //feedback tag
                                    xmlWriter.WriteStartElement(XML_FEEDBACK_TAG);
                                    xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                                    xmlWriter.WriteStartElement(XML_TEXT_TAG);
                                    xmlWriter.WriteEndElement();
                                    xmlWriter.WriteEndElement();

                                    xmlWriter.WriteEndElement();
                                } else
                                {
                                    xmlWriter.WriteStartElement(XML_ANSWER_TAG);
                                    xmlWriter.WriteAttributeString(XML_FRACTION_ATTR_NAME, XML_INCORRECT_FRACTION_ATTR_VALUE.ToString());
                                    xmlWriter.WriteAttributeString(XML_FORMAT_ATTR_NAME, XML_HTML_ATTR_VALUE);
                                    //text atg
                                    xmlWriter.WriteStartElement(XML_TEXT_TAG);
                                    if (option.OptionContent.IndexOfAny(SpecialChars.ToCharArray()) != -1)
                                    {
                                        xmlWriter.WriteCData(option.OptionContent);
                                    }
                                    else
                                    {
                                        xmlWriter.WriteString(option.OptionContent);
                                    }
                                    xmlWriter.WriteEndElement();
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
                    }
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();
                    // Convert the memory stream to an array of bytes.
                    byte[] byteArray = stream.ToArray();
                    // Send the XML file to the web browser for download.
                    httpResponseMessage.StatusCode = HttpStatusCode.OK;
                    httpResponseMessage.Content = new ByteArrayContent(byteArray);
                    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = "ExaminationExport.xml";
                    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    xmlWriter.Close();
                }
            }
            else
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    StreamWriter writer = new StreamWriter(stream);
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
                            writer.WriteLine(HttpUtility.HtmlEncode(questionTitle));
                            foreach (var option in question.Options)
                            {
                                if (option.IsCorrect == true)
                                {
                                    string optionString = String.Format(OPTION_TRUE, option.OptionContent);
                                    writer.WriteLine(HttpUtility.HtmlEncode(optionString));
                                }
                                else
                                {
                                    string optionString = String.Format(OPTION_FALSE, option.OptionContent);
                                    writer.WriteLine(HttpUtility.HtmlEncode(optionString));
                                }
                            }
                            writer.WriteLine("}");
                            writer.WriteLine();
                        }
                    }
                    writer.Flush();
                    byte[] byteArray = stream.ToArray();
                    httpResponseMessage.StatusCode = HttpStatusCode.OK;
                    httpResponseMessage.Content = new ByteArrayContent(byteArray);
                    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                    httpResponseMessage.Content.Headers.ContentDisposition.FileName = "ExaminationExport.txt";
                    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                    writer.Close();
                }


                //string data = "";
                //foreach (var part in partOfExams)
                //{
                //    string switchCategoryLine = String.Format(COMMENT_SWITCH_CATEGORY_LINE, "XML", part.Topic.Name, part.Question.First().Level.Name);
                //    data += switchCategoryLine + Environment.NewLine;
                //    string categoryLine = String.Format(CATEGORY_LINE, "XML", part.Topic.Name, part.Question.First().Level.Name);
                //    data += categoryLine + Environment.NewLine;
                //    data += Environment.NewLine;
                //    data += Environment.NewLine;
                //    for (int i = 0; i < part.Question.Count; i++)
                //    {
                //        QuestionViewModel question = part.Question[i];
                //        if (i != 0)
                //        {
                //            if (question.LevelId != part.Question[i - 1].LevelId)
                //            {
                //                switchCategoryLine = String.Format(COMMENT_SWITCH_CATEGORY_LINE, "XML", part.Topic.Name, question.Level.Name);
                //                data += switchCategoryLine + Environment.NewLine;
                //                categoryLine = String.Format(CATEGORY_LINE, "XML", part.Topic.Name, question.Level.Name);
                //                data += switchCategoryLine + Environment.NewLine;
                //                data += Environment.NewLine;
                //                data += Environment.NewLine;
                //            }
                //        }
                //        string questionComment = String.Format(QUESTION_COMMENT, question.Id, question.QuestionCode);
                //        data += questionComment + Environment.NewLine;
                //        string questionTitle = String.Format(QUESTION_TITLE, question.QuestionCode, question.QuestionContent) + "{";
                //        data += questionTitle + Environment.NewLine;
                //        foreach (var option in question.Options)
                //        {
                //            if (option.IsCorrect == true)
                //            {
                //                string optionString = String.Format(OPTION_TRUE, option.OptionContent);
                //                data += optionString + Environment.NewLine;
                //            }
                //            else
                //            {
                //                string optionString = String.Format(OPTION_FALSE, option.OptionContent);
                //                data += optionString + Environment.NewLine;
                //            }
                //        }
                //        data += "}" + Environment.NewLine;
                //        data += Environment.NewLine;
                //    }
                //}
                //var byteArray = Encoding.ASCII.GetBytes(data);
                //var stream = new MemoryStream(byteArray);

                //httpResponseMessage.StatusCode = HttpStatusCode.OK;
                //httpResponseMessage.Content = new StreamContent(stream);
                //httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                //httpResponseMessage.Content.Headers.ContentDisposition.FileName = "ExaminationExport.txt";
                //httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            }
            return httpResponseMessage;
        }
    }

}