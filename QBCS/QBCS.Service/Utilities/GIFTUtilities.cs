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
        string category = "";
        string learningOutcome = "";
        string level = "";
        public List<QuestionTmpModel> StripTagsCharArray(StreamReader reader, bool checkCate)
        {
            List<QuestionTmpModel> list = new List<QuestionTmpModel>();
            string line = null;
            List<OptionTemp> options = new List<OptionTemp>();
            bool isStartQuestion = false;
            //string destination = "[html]";
            int countCode = 0;
            int inLine = 0;
            StringProcess stringProcess = new StringProcess();
            while ((line = reader.ReadLine()) != null)
            {
                string id = null;
                string question = null;
                string right = null;
                string wrong = null;
                string result = null;
                string mark = null;
                int countRight = 0;
                int countWrong = 0;
                int countCate = 0;
                int countStartCate = 0;
                int countElement = 1;
                bool isBlock = false;
                bool isStartCate = false;
                bool isInLine = false;
                bool isStart = false;
                bool isEnd = false;
                //bool isComma = false;
                bool isMultipleChoice = false;
                if (!line.StartsWith("//"))
                {

                    line = stringProcess.RemoveHtmlBrTag(line);
                    HtmlDocument htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(line);           
                    string resultTmp = htmlDoc.DocumentNode.InnerText;
                    result = WebUtility.HtmlDecode(resultTmp);
                    //result = StringProcess.RemoveTag(result, destination, "");

                    //result = StringProcess.RemoveTag(result, @"\=", @"=");
                    //result = StringProcess.RemoveTag(result, @"\{", @"{");
                    //result = StringProcess.RemoveTag(result, @"\}", @"}");
                    //result = StringProcess.RemoveTag(result, @"\#", @"#");
                    //result = StringProcess.RemoveTag(result, @"\~", @"~");
                    //result = StringProcess.RemoveTag(result, @"\:", @":");
                    //result = StringProcess.RemoveTag(result, @"\n", @"<cbr>"); //<crb> replace for \n
                    //result = StringProcess.RemoveTag(result, @"\:", @":");
                    //result = StringProcess.RemoveTag(result, @"<span lang=" + '"' + "EN" + '"' + ">", "");
                    //result = StringProcess.RemoveTag(result, @"#" + @"<span lang=" + '"' + "EN" + '"' + ">", "");
                    //result = StringProcess.RemoveTag(result, @"#", "");
                    result = stringProcess.RemoveHtmlTag(result);
                    for (int i = 0; i < result.Length; i++)
                    {
                        char let = result[i];
                        #region start count to track the position
                        if (let == '$' && !isStartQuestion)
                        {
                            category = null;
                            level = null;
                            learningOutcome = null;
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
                            isInLine = true;
                            continue;
                        }
                        if (inLine == 1)
                        {
                            char tab = (char)9;

                            if (let == '=' || let == '~' || let == tab)
                            {
                                inLine = 2; //{ is end question
                                countCode = 0;
                                isStartQuestion = true;
                               // continue;
                            }
                            else
                            {
                                inLine = 3; // { in question
                            }
                        }
                        if (let == '=' && !isBlock && isStartQuestion && isInLine == false && inLine == 2)
                        {
                            countRight++;
                            countWrong = 0;
                            isBlock = true;
                            isInLine = true;
                            continue;
                        }
                        if (let == '~' && !isBlock && isStartQuestion && isInLine == false && inLine == 2)
                        {
                            countWrong++;
                            countRight = 0;
                            isBlock = true;
                            isInLine = true;
                            continue;
                        }
                        if (let == '}' && isStartQuestion && isInLine == false)
                        {
                            isEnd = true;
                            isStartQuestion = false;
                            continue;
                        }
                        //if (let == '?')
                        //{
                        //    isComma = true;

                        //}
                        if (let == '{' && !isBlock)
                        {
                            //countCode = 0;
                            //isStartQuestion = true;
                            inLine = 1; // not sure to end question
                           
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
                                learningOutcome += let;
                                continue;
                            }
                            if (countCate == 3)
                            {
                                level += let;
                                continue;
                            }


                        }
                        if (countCode <= 3 && countStartCate == 0 && isStart)
                        {

                            id += let;
                            continue;
                        }
                        if (countCode >= 4)
                        {
                            // { char is in question
                            if (inLine == 3)
                            {
                                question += '{';
                                question += let;
                                inLine = 0;
                                isStart = false;
                                continue;
                            }
                           if (inLine == 0)
                            {
                                question += let;
                                inLine = 0;
                                isStart = false;
                                continue;
                            }
                        }
                        if (countRight >= 1 && !isEnd)
                        {

                            right += let;
                            continue;

                        }
                        if (isMultipleChoice == true)
                        {
                          
                            if (let != '%')
                            {
                                mark += let;
                                continue;
                            }
                            
                        }
                        if (mark != null)
                        {
                            if (int.Parse(mark) < 0)
                            {
                                countWrong++;
                                countRight = 0;
                                isBlock = true;
                                isMultipleChoice = false;
                                mark = null;
                                continue;
                            }
                            if (int.Parse(mark) > 0)
                            {
                                countRight++;
                                countWrong = 0;
                                isBlock = true;
                                isMultipleChoice = false;
                                mark = null;
                                continue;
                            }
                        }
                       
                        if (countWrong >= 1 && !isEnd)
                        {
                            if (let == '%' && countElement == 1)
                            {
                                isMultipleChoice = true;
                                continue;
                            }
                            else
                            {
                                wrong += let;
                                countElement++;
                                continue;
                            }
                           
                        }
                        #endregion
                    }
                }
                if (category != null && learningOutcome != null && level != null && checkCate == true)
                {
                    if (checkCate == false)
                    {
                        category = null;
                        learningOutcome = null;
                        level = null;
                        countStartCate = 0;
                        isStartCate = false;
                    }
                    else
                    {
                        quesModel.Category = category;
                        quesModel.LearningOutcome = learningOutcome;
                        quesModel.Level = level;
                        countStartCate = 0;
                        isStartCate = false;
                    }
                   

                }
                if (id != null)
                {
                    quesModel.Code = id;
                    id = null;
                }
                if (question != null)
                {

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
                    optionModel.OptionContent = right;
                    optionModel.IsCorrect = true;
                    options.Add(optionModel);
                }
                if (wrong != null)
                {
                    optionModel = new OptionTemp();
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
                    inLine = 0;
                    isMultipleChoice = false;
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
