using QBCS.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Utilities
{
    public class GIFTUtilities
    {
        //State Machine for GIFT file
        public void StripTagsCharArray(string source)
        {
            char[] arrayCode = new char[source.Length];
            char[] arrayQuesContent = new char[source.Length];
            char[] arrayRight = new char[source.Length];
            char[] arrayWrong = new char[source.Length];
            int arrayIndexCode = 0;
            int arrayIndexQuestion = 0;
            int arrayIndexRight = 0;
            int arrayIndexWrong = 0;
            bool isStart = false;
            bool isEnd = false;
            int countCode = 0;
            int countRight = 0;
            int countWrong = 0;
            char temp = ';';
            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                //start code and question
                if (let == ':')
                {
                    countCode++;
                    isStart = true;
                    continue;
                }
                //Right answer
                if (let == '=')
                {
                    countRight++;
                    countWrong = 0;
                    temp = ',';
                    continue;
                }
                //Wrong answer
                if (let == '~')
                {
                    countWrong++;
                    countRight = 0;
                    temp = ',';
                    continue;
                }
                //end gift
                if (let == '}')
                {
                    isEnd = true;
                    continue;
                }
                //get code question
                if (countCode < 3 && isStart)
                {
                    arrayCode[arrayIndexCode] = let;
                    arrayIndexCode++;
                    continue;
                }
                //get question content
                if (countCode >= 4)
                {
                    if (let != '\0')
                    {
                        arrayQuesContent[arrayIndexQuestion] = let;
                        arrayIndexQuestion++;
                        isStart = false;
                        continue;
                    }
                }
                //get right answer by char
                if (countRight >= 1 && !isEnd)
                {
                    if (let != '\0')
                    {

                        if (temp != ';')
                        {
                            arrayRight[arrayIndexRight + 1] = temp;
                            arrayWrong[arrayIndexWrong] = temp;// insert ',' into array
                        }
                        arrayRight[arrayIndexRight] = let;
                        arrayIndexRight++;
                        temp = ';';
                        continue;
                    }
                }
                //get wrong answer by char
                if (countWrong >= 1 && !isEnd)
                {
                    if (let != '\0')
                    {
                        if (temp != ';')
                        {
                            arrayRight[arrayIndexRight] = temp;
                            arrayWrong[arrayIndexWrong] = temp; // insert ',' into array 
                        }
                        arrayWrong[arrayIndexWrong + 1] = let; //get a character
                        arrayIndexWrong++;
                        temp = ';';
                        continue;
                    }

                }
            }
            string code = new string(arrayCode, 0, arrayIndexCode);
            string question = new string(arrayQuesContent, 0, arrayIndexQuestion);
            string questionRight = new string(arrayRight, 0, arrayIndexRight);
            string questionWrong = new string(arrayWrong, 0, arrayIndexWrong);

            if (code != "")
            {
                //Console.WriteLine("Code: {0}", code.Trim());
            }
            if (question != "")
            {
                //Console.WriteLine("Question: {0}", question.Trim());
            }
            if (questionRight != "")
            {

                string[] arrListStrTmp = questionRight.Split(new char[] { ',' });

                for (int i = 0; i < arrListStrTmp.Length; i++)
                {

                    if (!arrListStrTmp[i].Contains('\0'))
                    {
                        //Console.WriteLine("Right: {0}", arrListStrTmp[i]);
                    }

                }
            }
            if (questionWrong != "")
            {
                
                string[] arrListStr = questionWrong.Split(new char[] { ',' });
                RemoveNull(arrListStr);
                for (int i = 0; i < arrListStr.Length; i++)
                {
                    if (!arrListStr[i].Contains('\0'))
                    {
                        //Console.WriteLine("Wrong: {0}", arrListStr[i].Trim());
                    }

                }
                

            }
        }
        //Remove null 
        public void RemoveNull(string[] array)
        {
            List<string> list = new List<string>(array);
            for (int index = 0; index < list.Count; index++)
            {
                bool nullOrEmpty = list[index].Contains('\0');
                if (nullOrEmpty)
                {
                    list.RemoveAt(index);
                    --index;
                }
            }
        }
    }
}
