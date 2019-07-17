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
        public List<QuestionTmpModel> StripTagsCharArray(StreamReader reader, bool checkCate, bool checkHTML)
        {
            List<QuestionTmpModel> list = new List<QuestionTmpModel>();
            try
            {
                
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
                    int colon = 0;
                    string question = null;
                    string right = null;
                    string wrong = null;
                    string result = null;
                    string mark = null;
                    //string flagCategory = "";
                    int countRight = 0;
                    int countWrong = 0;
                    //int countCate = 0;
                    //int countStartCate = 0;
                    int countElement = 1;
                    bool isBlock = false;
                    //bool isStartCate = false;
                    bool isInLine = false;
                    bool isStart = false;
                    bool isEnd = false;
                    bool keeping = false;

                    //bool isComma = false;
                    bool isMultipleChoice = false;
                    if (line.Contains("$CATEGORY"))
                    {
                        string[] temp = line.Split('/');
                        for (int i = 0; i < temp.Count(); i++)
                        {
                            if (i == 1)
                            {
                                category = temp[i];
                            }
                            if (i == 2)
                            {
                                learningOutcome = temp[i];
                            }
                            if (i == 3)
                            {
                                level = temp[i];
                            }
                        }

                    }
                    if (!line.StartsWith("//"))
                    {

                        line = stringProcess.RemoveHtmlBrTag(line);
                        string resultTmp = "";
                        if (checkHTML == false)
                        {
                            HtmlDocument htmlDoc = new HtmlDocument();
                            htmlDoc.LoadHtml(line);
                            resultTmp = htmlDoc.DocumentNode.InnerText;
                        }
                        if (!resultTmp.Equals(""))
                        {
                            result = WebUtility.HtmlDecode(resultTmp);
                        }
                        else
                        {
                            result = line;
                        }

                        result = stringProcess.RemoveHtmlTagGIFT(result);
                        for (int i = 0; i < result.Length; i++)
                        {
                            char let = result[i];
                            #region start count to track the position
                            //if (let == '$' && !isStartQuestion)
                            //{
                            //    category = "";
                            //    level = "";
                            //    learningOutcome = "";
                            //    countStartCate++;
                            //    continue;
                            //}
                            //if (let == '$')
                            //{
                            //    category = "";
                            //    level = "";
                            //    learningOutcome = "";
                            //    flagCategory += let;


                            //}
                            //if (flagCategory.Equals("$CATEGORY"))
                            //{
                            //    countStartCate++;
                            //    continue;
                            //}

                            //if (let == '/' && !isStartQuestion)
                            //{

                            //    isStartCate = true;
                            //    countCate++;
                            //    continue;
                            //}
                            if (let == ':' && !isStartQuestion)
                            {
                                countCode++;
                                colon = colon + 1;
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
                            //if (countStartCate == 3 && isStartCate)
                            //{

                            //    if (countCate == 1)
                            //    {
                            //        category += let;
                            //        continue;
                            //    }
                            //    if (countCate == 2)
                            //    {
                            //        learningOutcome += let;
                            //        continue;
                            //    }
                            //    if (countCate == 3)
                            //    {
                            //        level += let;
                            //        continue;
                            //    }


                            //}
                            // if (countCode <= 3 && countStartCate == 0 && isStart)
                            if (countCode <= 3 && isStart)
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
                                    if (colon >= 5)
                                    {
                                        question += ':';
                                        colon = 4;
                                    }
                                    question += let;
                                    inLine = 0;
                                    isStart = false;
                                    continue;
                                }
                                if (inLine == 0)
                                {
                                    if (colon >= 5)
                                    {
                                        question += ':';
                                        colon = 4;
                                    }
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
                                if (!mark.Equals(""))
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
                    if (category != null /*&& learningOutcome != null && level != null*/ && checkCate == true)
                    {
                        if (checkCate == false)
                        {
                            category = "";
                            learningOutcome = "";
                            level = "";
                            //countStartCate = 0;
                            //isStartCate = false;
                        }
                        else
                        {
                            quesModel.Category = category;
                            quesModel.LearningOutcome = learningOutcome;
                            quesModel.Level = level;
                            //countStartCate = 0;
                            //isStartCate = false;
                        }


                    }
                    if (id != null)
                    {
                        quesModel.Code = id;
                        id = null;
                    }
                    if (question != null)
                    {
                        if (question.Equals(""))
                        {
                            quesModel.QuestionContentError = "Question content is empty";
                        }

                        if (quesModel.QuestionContent != null)
                        {
                            quesModel.QuestionContent += "<br>" + question;
                        }
                        else
                        {
                            question = stringProcess.UpperCaseKeyWord(question);
                            if (question.Contains("[html]"))
                            {
                                quesModel.QuestionContent = question;
                            }
                            else
                            {
                                quesModel.QuestionContent = "[html]" + question;
                            }
                           

                        }
                        question = null;

                    }

                    if (right != null)
                    {
                        optionModel = new OptionTemp();
                        optionModel.OptionContent = "[html]" +right;
                        optionModel.IsCorrect = true;
                        options.Add(optionModel);
                    }
                    if (wrong != null)
                    {
                        optionModel = new OptionTemp();
                        optionModel.OptionContent = "[html]" + wrong;
                        optionModel.IsCorrect = false;
                        options.Add(optionModel);
                    }

                    if (quesModel.QuestionContent != null && isEnd && quesModel.Code != null)
                    {
                        if (options.Count < 4)
                        {
                            quesModel.Error = "Number of option " + options.Count.ToString();
                        }
                        quesModel.Options = options;
                        list.Add(quesModel);
                        quesModel = new QuestionTmpModel();
                        options = new List<OptionTemp>();
                        countCode = 0;
                        inLine = 0;
                        isMultipleChoice = false;
                    }
                }
            }
            catch (Exception ex)
            {
                
                quesModel.OtherError = "Other Error " + ex.Message.ToString();
                
            }
            
            return list;
        }


       
    }
}
