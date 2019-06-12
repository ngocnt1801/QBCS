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
        static string category = null;
        static string topic = null;
        static string level = null;
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
                string wrong = null;
                bool isStart = false;
                bool isEnd = false;
                int countCode = 0;
                int countRight = 0;
                int countWrong = 0;
                int countCate = 0;
                int countStartCate = 0;
                bool isStartCate = false;
                if (!line.Contains("//"))
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        char let = line[i];

                        #region start count to track the position
                        if (let == '$')
                        {
                            category = null;
                            level = null;
                            topic = null;
                            countStartCate++;
                            continue;
                        }
                        if (let == '/')
                        {

                            isStartCate = true;
                            countCate++;
                            continue;
                        }
                        if (let == ':')
                        {
                            countCode++;
                            isStart = true;
                            continue;
                        }
                        if (let == '=' && isStartQuestion)
                        {
                            countRight++;
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
                if (question != null)
                {                 
                    quesModel.Code = id;
                    quesModel.QuestionContent = question;
                   

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
