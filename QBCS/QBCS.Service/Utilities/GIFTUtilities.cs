using HtmlAgilityPack;
using QBCS.Entity;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace QBCS.Service.Utilities
{
    public class GIFTUtilities
    {
        static QuestionTmpModel quesModel = new QuestionTmpModel();
        static OptionTemp optionModel = new OptionTemp();
        static string category = "";
        static string topic = "";
        static string level = "";
        public List<QuestionTmpModel> StripTagsCharArray(StreamReader reader)
        {
            List<QuestionTmpModel> list = new List<QuestionTmpModel>();

            string line = null;
            List<OptionTemp> options = new List<OptionTemp>();
            bool isStartQuestion = false;
            //string destination = "[html]";
            int countCode = 0;
            while ((line = reader.ReadLine()) != null)
            {
                string id = null;
                string question = null;
                string right = null;
                string wrong = null;
                bool isStart = false;
                bool isEnd = false;
                int countRight = 0;
                int countWrong = 0;
                int countCate = 0;
                int countStartCate = 0;
                bool isBlock = false;
                bool isStartCate = false;
                string result = null;
                if (!line.StartsWith("//"))
                {
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(line);
                    string resultTmp = htmlDoc.DocumentNode.InnerHtml;
                    result = WebUtility.HtmlDecode(resultTmp);
                    //result = StringProcess.RemoveTag(result, destination, "");
                    result = StringProcess.RemoveTag(result, @"\=", @"=");
                    result = StringProcess.RemoveTag(result, @"\{", @"{");
                    result = StringProcess.RemoveTag(result, @"\}", @"}");
                    result = StringProcess.RemoveTag(result, @"\#", @"#");
                    result = StringProcess.RemoveTag(result, @"\~", @"~");
                    result = StringProcess.RemoveTag(result, @"\:", @":");
                    result = StringProcess.RemoveTag(result, @"\n", @"<cbr>"); //<crb> replace for \n
                    result = StringProcess.RemoveTag(result, @"\:", @":");
                    result = StringProcess.RemoveTag(result, @"<span lang=" + '"' + "EN" + '"' + ">", "");
                    for (int i = 0; i < result.Length; i++)
                    {
                        char let = result[i];
                        #region start count to track the position
                        if (let == '$' && !isStartQuestion)
                        {
                            category = null;
                            level = null;
                            topic = null;
                            countStartCate++;
                            continue;
                        }
                        if (let == '/' && !isStartQuestion)
                        {

                            isStartCate = true;
                            countCate++;
                            continue;
                        }
                        if (let == ':' && !isStartQuestion)
                        {
                            countCode++;
                            isStart = true;
                            continue;
                        }
                        if (let == '=' && !isBlock && isStartQuestion)
                        {
                            countRight++;
                            countWrong = 0;
                            isBlock = true;
                            continue;
                        }
                        if (let == '~' && !isBlock && isStartQuestion)
                        {
                            countWrong++;
                            countRight = 0;
                            isBlock = true;
                            continue;
                        }
                        if (let == '}' && isStartQuestion)
                        {
                            isEnd = true;
                            isStartQuestion = false;
                            continue;
                        }
                        if (let == '{')
                        {
                            countCode = 0;
                            isStartQuestion = true;
                            continue;
                        }


                        #endregion

                        #region add character to variables
                        if (countStartCate == 3 && isStartCate)
                        {

                            if (countCate == 1)
                            {
                                category += let;
                                continue;
                            }
                            if (countCate == 2)
                            {
                                topic += let;
                                continue;
                            }
                            if (countCate == 3)
                            {
                                level += let;
                                continue;
                            }


                        }
                        if (countCode < 3 && isStart)
                        {

                            id += let;
                            continue;
                        }
                        if (countCode >= 4)
                        {
                            question += let;
                            isStart = false;
                            continue;

                        }
                        if (countRight >= 1 && !isEnd)
                        {

                            right += let;
                            continue;

                        }
                        if (countWrong >= 1 && !isEnd)
                        {
                            wrong += let;
                            continue;
                        }
                        #endregion
                    }
                }
                if (category != null && topic != null && level != null)
                {
                    quesModel.Category = category;
                    quesModel.Topic = topic;
                    quesModel.Level = level;

                }
                if (id != null)
                {
                    quesModel.Code = id;
                    id = null;
                }
                if (question != null)
                {

                    //string destination = "[html]";
                    //question = StringProcess.RemoveTag(question, destination, "");
                    //question = StringProcess.RemoveTag(question, @"\=", @"=");
                    //question = StringProcess.RemoveTag(question, @"\{", @"{");
                    //question = StringProcess.RemoveTag(question, @"\}", @"}");
                    //question = StringProcess.RemoveTag(question, @"\#", @"#");
                    //question = StringProcess.RemoveTag(question, @"\~", @"~");
                    //question = StringProcess.RemoveTag(question, @"\:", @":");
                    //question = question.Replace(@"\:", @":");

                    // question = StringProcess.RemoveTag(question, @"<br/>", Environment.NewLine);
                    if (quesModel.QuestionContent != null)
                    {
                        quesModel.QuestionContent += "<br>" + question;
                    }
                    else
                    {
                        quesModel.QuestionContent = question;
                    }
                    question = null;

                }
                if (right != null)
                {
                    optionModel = new OptionTemp();
                    // right = StringProcess.RemoveTag(right, @"<br/>", Environment.NewLine);
                    //right = StringProcess.RemoveTag(right, @"\=", @"=");
                    //right = StringProcess.RemoveTag(right, @"\{", @"{");
                    //right = StringProcess.RemoveTag(right, @"\}", @"}");
                    //right = StringProcess.RemoveTag(right, @"\#", @"#");
                    //right = StringProcess.RemoveTag(right, @"\~", @"~");
                    //right = StringProcess.RemoveTag(right, @"\:", @":");
                    optionModel.OptionContent = right;
                    optionModel.IsCorrect = true;
                    options.Add(optionModel);
                }
                if (wrong != null)
                {
                    optionModel = new OptionTemp();
                    // wrong = StringProcess.RemoveTag(wrong, @"<br/>", Environment.NewLine);
                    //wrong = StringProcess.RemoveTag(wrong, @"\=", @"=");
                    //wrong = StringProcess.RemoveTag(wrong, @"\{", @"{");
                    //wrong = StringProcess.RemoveTag(wrong, @"\}", @"}");
                    //wrong = StringProcess.RemoveTag(wrong, @"\#", @"#");
                    //wrong = StringProcess.RemoveTag(wrong, @"\~", @"~");
                    //wrong = StringProcess.RemoveTag(wrong, @"\<", @"<");
                    //wrong = StringProcess.RemoveTag(wrong, @"\:", @":");
                    optionModel.OptionContent = wrong;
                    optionModel.IsCorrect = false;
                    options.Add(optionModel);
                }

                if (quesModel.QuestionContent != null && isEnd && quesModel.Code != null)
                {
                    quesModel.Options = options;
                    list.Add(quesModel);
                    quesModel = new QuestionTmpModel();
                    options = new List<OptionTemp>();
                    countCode = 0;
                }
            }
            return list;
        }


        public void RemoveNull(string[] array)
        {
            List<string> list = new List<string>(array);
            for (int index = 0; index < list.Count; index++)
            {
                bool nullOrEmpty = list[index].Contains("\0");
                if (nullOrEmpty)
                {
                    list.RemoveAt(index);
                    --index;
                }
            }
        }
    }
}
