using Newtonsoft.Json;
using QBCS.Entity;
using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QBCS.Service.Utilities
{
    public class GIFTUtilities
    {
        static QuestionTmpModel quesModel = new QuestionTmpModel();
        static OptionTemp optionModel = new OptionTemp();
        //public List<QuestionTmpModel> test (StreamReader reader, List<QuestionTmpModel> listQuestion)
        //{
        //    string line;
        //    quesModel = StripTagsCharArray(reader, listQuestion);
        //    //while ((line = reader.ReadLine()) != null)
        //    //{
        //    //    quesModel = StripTagsCharArray(line, listQuestion);
        //    //    if (quesModel.QuestionContent != null && quesModel.OptionsContent != null)
        //    //    {
        //    //        listQuestion.Add(quesModel);
        //    //    }

        //    //}

        //    return listQuestion;
        //}

        public List<QuestionTmpModel> StripTagsCharArray(StreamReader reader)
        {
            string line = null;
            List<QuestionTmpModel> list = new List<QuestionTmpModel>();
            List<OptionTemp> options = new List<OptionTemp>();
            bool isStartQuestion = false;
            while ((line = reader.ReadLine()) != null)
            {

                string id = null;
                string question = null;
                string right = null;
                string wrong1 = null;
                string wrong2 = null;
                string wrong3 = null;
                bool isStart = false;
                bool isEnd = false;
                int countCode = 0;
                int countRight = 0;
                int countWrong = 0;
                if (!line.Contains("//") && !line.Contains("$CATEGORY"))
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        char let = line[i];

                        if (let == ':')
                        {
                            countCode++;
                            isStart = true;
                            continue;
                        }
                        if (let == '=' && isStartQuestion)
                        {
                            countRight++;
                            //countWrong = 0;
                            continue;
                        }
                        if (let == '~' && isStartQuestion)
                        {
                            countWrong++;
                            countRight = 0;
                            continue;
                        }
                        if (let == '}')
                        {
                            isEnd = true;
                            continue;
                        }
                        if (let == '{')
                        {
                            countCode = 0;
                            isStartQuestion = true;
                            continue;
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

                            wrong1 += let;
                            continue;
                        }
                        //if (countWrong == 2 && !isEnd)
                        //{

                        //    wrong2 += let;
                        //    continue;
                        //}
                        //if (countWrong == 3 && !isEnd)
                        //{

                        //    wrong3 += let;
                        //    continue;
                        //}
                    }
                }
                // Console.WriteLine(name);
                //if (id != null)
                //{
                //    quesModel.Code = id;
                //}
                if (question != null)
                {
                    //string jsonQuestion = null;
                    //List<string> tempJson = new List<string>();
                    //tempJson.Add(question);
                    //jsonQuestion = JsonConvert.SerializeObject(tempJson);                  
                    quesModel.Code = id;
                    quesModel.QuestionContent = question;
                   

                }
                if (right != null)
                {
                    optionModel = new OptionTemp();
                    // string jsonQuestion = null;
                    //List<string> tempJson = new List<string>();
                    //tempJson.Add(right);         
                    //jsonQuestion = JsonConvert.SerializeObject(tempJson);
                    optionModel.OptionContent = right;
                    optionModel.IsCorrect = true;
                    options.Add(optionModel);
                    //quesModel.OptionsContent = option;


                }
                if (wrong1 != null)
                {
                    optionModel = new OptionTemp();
                    // string jsonQuestion = null;
                    //List<string> tempJson = new List<string>();
                    //tempJson.Add(right);         
                    //jsonQuestion = JsonConvert.SerializeObject(tempJson);
                    optionModel.OptionContent = wrong1;
                    optionModel.IsCorrect = false;
                    options.Add(optionModel);
                    //quesModel.OptionsContent = option;
                }
                //if (wrong2 != null)
                //{
                //    optionModel = new OptionTemp();
                //    // string jsonQuestion = null;
                //    //List<string> tempJson = new List<string>();
                //    //tempJson.Add(right);         
                //    //jsonQuestion = JsonConvert.SerializeObject(tempJson);
                //    optionModel.OptionContent = wrong2;
                //    optionModel.IsCorrect = false;

                //    options.Add(optionModel);
                //    //quesModel.OptionsContent = option;
                //}
                //if (wrong3 != null)
                //{
                //    optionModel = new OptionTemp();
                //    // string jsonQuestion = null;
                //    //List<string> tempJson = new List<string>();
                //    //tempJson.Add(right);         
                //    //jsonQuestion = JsonConvert.SerializeObject(tempJson);
                //    optionModel.OptionContent = wrong3;
                //    optionModel.IsCorrect = false;

                //    options.Add(optionModel);
                //    //quesModel.OptionsContent = option;
                //}
                if (quesModel.QuestionContent != null && options.Count() == 4 && quesModel.Code != null)
                {
                    quesModel.Options = options;
                    list.Add(quesModel);
                    quesModel = new QuestionTmpModel();
                    options = new List<OptionTemp>();
                }
            }

            return list;
        }

        //foreach (var item in list)
        //{
        //    if (item.QuestionContent != null && item.OptionsContent != null)
        //    {
        //        Console.WriteLine("Name {0}", item.QuestionContent);
        //        Console.WriteLine("Right {0}", item.OptionsContent);

        //    }

        //}
        //Console.WriteLine("Ques {0}" , id);
        //Console.WriteLine("Name {0}", name);
        //Console.WriteLine("Right {0}", right);
        //Console.WriteLine("Wrong {0}", wrong);
        //string code = new string(array1, 0, arrayIndexCode);
        //string ques = new string(array2, 0, arrayIndexQuestion);
        //string code3 = new string(array3, 0, arrayIndexRight);
        //string code4 = new string(array4, 0, arrayIndexWrong);

        //if (code != "")
        //{
        //    //Console.WriteLine("Code: {0}", code.Trim());
        //}
        //if (ques != "")
        //{
        //    //Console.WriteLine("Question: {0}", ques.Trim());
        //}
        //if (code3 != "")
        //{

        //    string[] arrListStr2 = code3.Split(new char[] { ',' });

        //    for (int i = 0; i < arrListStr2.Length; i++)
        //    {

        //        if (!arrListStr2[i].Contains('\0'))
        //        {
        //           // Console.WriteLine("Right: {0}", arrListStr2[i]);
        //        }

        //    }
        //}
        //if (code4 != "")
        //{
        //    //code4 = code4.Substring(0, code4.IndexOf(','));
        //    string[] arrListStr = code4.Split(new char[] { ',' });
        //    RemoveNull(arrListStr);
        //    for (int i = 0; i < arrListStr.Length; i++)
        //    {
        //        if (!arrListStr[i].Contains('\0'))
        //        {
        //            //Console.WriteLine("Wrong: {0}", arrListStr[i].Trim());
        //        }

        //    }
        //    //Console.WriteLine("Right: {0}", code4.Trim());

        //}


        //return new string(array1, 0, arrayIndex);


        //State Machine for GIFT file
        //public List<QuestionTmpModel> StripTagsCharArray(StreamReader reader)
        //{
        //    List<QuestionTmpModel> questions = new List<QuestionTmpModel>();
        //    string line;
        //    int arrayIndexCode = 0;
        //    int arrayIndexQuestion = 0;
        //    int arrayIndexRight = 0;
        //    int arrayIndexWrong = 0;
        //    //char[] arrayCode;
        //    //char[] arrayQuesContent;
        //    //char[] arrayRight;
        //    //char[] arrayWrong;
        //    string code = null;
        //    string question = null;
        //    string questionRight = null;
        //    string questionWrong = null;
        //    while ((line = reader.ReadLine()) != null)
        //    {
        //        bool isStart = false;
        //        bool isEnd = false;
        //        int countCode = 0;
        //        int countRight = 0;
        //        int countWrong = 0;
        //        char temp = ';';
        //        char[] arrayCode = new char[10000];
        //        char[] arrayQuesContent = new char[10000];
        //        char[] arrayRight = new char[10000];
        //        char[] arrayWrong = new char[10000];
        //        for (int i = 0; i < line.Length; i++)
        //        {
        //            char let = line[i];
        //            //start code and question
        //            if (let == ':')
        //            {
        //                countCode++;
        //                isStart = true;
        //                continue;
        //            }
        //            //Right answer
        //            if (let == '=')
        //            {
        //                countRight++;
        //                countWrong = 0;
        //                temp = ',';
        //                continue;
        //            }
        //            //Wrong answer
        //            if (let == '~')
        //            {
        //                countWrong++;
        //                countRight = 0;
        //                temp = ',';
        //                continue;
        //            }
        //            //end gift
        //            if (let == '}')
        //            {
        //                isEnd = true;
        //                continue;
        //            }
        //            //get code question
        //            if (countCode < 3 && isStart)
        //            {
        //                arrayCode[arrayIndexCode] = let;
        //                arrayIndexCode++;
        //                continue;
        //            }
        //            //get question content
        //            if (countCode >= 4 && !let.Equals('\0'))
        //            {
        //                if ((int)let != 0)
        //                {
        //                    if (isStart)
        //                    {
        //                        arrayQuesContent[arrayIndexQuestion] = temp;
        //                    }
        //                    arrayQuesContent[arrayIndexQuestion + 1] = let;
        //                    arrayIndexQuestion++;
        //                    isStart = false;
        //                    continue;
        //                }
        //            }
        //            //get right answer by char
        //            if (countRight >= 1 && !isEnd)
        //            {
        //                if ((int)let != 0)
        //                {

        //                    if (temp != ';')
        //                    {
        //                        arrayQuesContent[arrayIndexQuestion + 1] = temp;
        //                        arrayRight[arrayIndexRight + 1] = temp;
        //                        arrayWrong[arrayIndexWrong] = temp;// insert ',' into array

        //                    }
        //                    arrayRight[arrayIndexRight] = let;
        //                    arrayIndexRight++;
        //                    temp = ';';
        //                    continue;
        //                }
        //            }
        //            //get wrong answer by char
        //            if (countWrong >= 1 && !isEnd)
        //            {
        //                if ((int)let != 0)
        //                {
        //                    if (temp != ';')
        //                    {
        //                        arrayQuesContent[arrayIndexQuestion + 1] = temp;
        //                        arrayRight[arrayIndexRight] = temp;
        //                        arrayWrong[arrayIndexWrong] = temp; // insert ',' into array 
        //                    }
        //                    arrayWrong[arrayIndexWrong + 1] = let; //get a character
        //                    arrayIndexWrong++;
        //                    temp = ';';
        //                    continue;
        //                }

        //            }
        //        }

        //        code = new string(arrayCode, 0, arrayIndexCode);
        //        question = new string(arrayQuesContent, 0, arrayIndexQuestion);
        //        questionRight = new string(arrayRight, 0, arrayIndexRight);
        //        questionWrong = new string(arrayWrong, 0, arrayIndexWrong);

        //        List<string> listTemp = new List<string>();



        //        if (question != null)
        //        {

        //            string[] arrListStrTmp = question.Split(new char[] { ',' });
        //            RemoveNull(arrListStrTmp);

        //            listTemp = new List<string>();
        //            for (int i = 0; i < arrListStrTmp.Length; i++)
        //            {

        //                if (arrListStrTmp[i] != "\0")
        //                {
        //                    listTemp.Add(arrListStrTmp[i]);
        //                    //Console.WriteLine("Right: {0}", arrListStrTmp[i]);
        //                    string json = JsonConvert.SerializeObject(listTemp);
        //                    quesModel.QuestionContent = json;
        //                }

        //            }



        //            //Console.WriteLine("Question: {0}", question.Trim());
        //        }
        //        if (questionRight != "")
        //        {

        //            string[] arrListStrTmp = questionRight.Split(new char[] { ',' });
        //            RemoveNull(arrListStrTmp);
        //            listTemp = new List<string>();
        //            for (int i = 0; i < arrListStrTmp.Length; i++)
        //            {

        //                if (arrListStrTmp[i] != "\0")
        //                {
        //                    listTemp.Add(arrListStrTmp[i]);
        //                    //Console.WriteLine("Right: {0}", arrListStrTmp[i]);
        //                }
        //                string json = JsonConvert.SerializeObject(listTemp);
        //                quesModel.OptionsContent = json;
        //            }

        //        }
        //        if (questionWrong != "")
        //        {

        //            string[] arrListStr = questionWrong.Split(new char[] { ',' });
        //            RemoveNull(arrListStr);
        //            listTemp = new List<string>();
        //            for (int i = 0; i < arrListStr.Length; i++)
        //            {
        //                if (arrListStr[i] != "\0")
        //                {
        //                    //DE sau khi co them cot cau hoi sai
        //                    //quesModel.OptionsContent += arrListStr[i] + " ";
        //                    //Console.WriteLine("Wrong: {0}", arrListStr[i].Trim());
        //                }

        //            }


        //        }
        //        if (quesModel.QuestionContent != "[]" && quesModel.OptionsContent != "[]")
        //        {
        //            questions.Add(quesModel);
        //            quesModel = new QuestionTmpModel();
        //        }

        //    }

        //    //List<QuestionTmpModel> listQuestion = new List<QuestionTmpModel>();

        //    //QuestionTmpModel quesModel = new QuestionTmpModel();


        //    //listQuestion.Add(quesModel);

        //    return questions;
        //}
        //Remove null 
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
