using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Utilities
{
    public class StringProcess
    {
        public static string RemoveTag (string source, string oldString, string newString)
        {
            string result = null;
            if (source != null)
            {
                result = source.Replace(oldString, newString);
            }
            return result;
        }
        public string UpperCaseKeyWord(string source)
        {
            string result = "";
           
            //string tempResult = "";
            string INCORRECT = "incorrect";
            string FALSE = "false";
            //string NOT = "not";
            string NOT_TRUE = "not true";
            string NOT_CORRECT = "not correct";
            string TRUE = "true";
            string CORRECT = "correct";
            //bool isContinute = false;
            if (source != null)
            {
                
                result = RemoveTag(source, INCORRECT, INCORRECT.ToUpper());
                result = RemoveTag(result, FALSE, FALSE.ToUpper());
                //string[] temp = result.Split(' ');
                //for (int i = 0; i < temp.Length; i++)
                //{
                   
                //    if (temp[i].Equals(NOT))
                //    {
                //        //tempResult = RemoveTag(result, temp[i].ToString(), NOT.ToUpper());
                //        //tempResult = result.Replace(temp[i].ToString(), NOT.ToUpper());
                //        temp[i] = temp[i].ToUpper();
                //        isContinute = true;
                //        continue;
                //    }
                //    if (isContinute)
                //    {
                //        if (temp[i].Contains(TRUE))
                //        {
                //            //tempResult = RemoveTag(tempResult, temp[i].ToString(), TRUE.ToUpper());
                //            temp[i] = temp[i].ToUpper();
                //            isContinute = false;
                //            result = String.Join(" ", temp).ToString();
                //        }
                //        if (temp[i].Contains(CORRECT))
                //        {
                //            //tempResult = RemoveTag(tempResult, temp[i].ToString(), CORRECT.ToUpper());
                //            temp[i] = temp[i].ToUpper();
                //            isContinute = false;
                //            result = String.Join(" ", temp).ToString();
                //        }
                        
                //        isContinute = false;
                //    }
                    

                //}
                
                //result = RemoveTag(result, NOT, NOT.ToUpper());

                result = RemoveTag(result, NOT_TRUE, NOT_TRUE.ToUpper());
                result = RemoveTag(result, NOT_CORRECT, NOT_CORRECT.ToUpper());
                result = RemoveTag(result, TRUE, TRUE.ToUpper());
                result = RemoveTag(result, CORRECT, CORRECT.ToUpper());
               
            }
            return result;
        }
        public string RemoveHtmlBrTag(string source)
        {
            string result = null;
            
            if (source != null)
            {
                result = RemoveTag(source, @"<br>", @"\n");
                result = RemoveTag(result, @"</p>", @"</p>\n");
                
            }

            return result;
        }
        public string RemoveHtmlTagXML(string source)
        {
            string result = null;
          
            if (source != null)
            {
                //result = RemoveTag(source, "[html]", "");
                //result = RemoveTag(source, "[html]", "");
                result = RemoveTag(source, @"\=", @"=");
                result = RemoveTag(result, @"\{", @"{");
                result = RemoveTag(result, @"\}", @"}");
                result = RemoveTag(result, @"\#", @"#");
                result = RemoveTag(result, @"\~", @"~");
                result = RemoveTag(result, @"\:", @":");
                result = RemoveTag(result, @"\n", @"<cbr>");
                //result = RemoveTag(result, @"<br>", @"<cbr>");
                result = RemoveTag(result, @"\:", @":");
                result = RemoveTag(result, @"#", "");
                
                result = RemoveTag(result, @"<span lang=" + '"' + "EN" + '"' + ">", "");

            }

            return result;
        }
        public string RemoveHtmlTagGIFT(string source)
        {
            string result = null;
           
            if (source != null)
            {
                //result = RemoveTag(source, "[html]", "");
                //result = RemoveTag(source, "[html]", "");
                result = RemoveTag(source, @"\=", @"=");
                result = RemoveTag(result, @"\{", @"{");
                result = RemoveTag(result, @"\}", @"}");
                result = RemoveTag(result, @"\#", @"#");
                result = RemoveTag(result, @"\~", @"~");
                result = RemoveTag(result, @"\:", @":");
                result = RemoveTag(result, @"\n", @"<cbr>");
                //result = RemoveTag(result, @"<br>", @"<cbr>");
                result = RemoveTag(result, @"\:", @":");          
                //result = RemoveTag(result, @"#", "");
                result = RemoveTag(result, @"<span lang=" + '"' + "EN" + '"' + ">", "");
                
            }
            
            return result;
        }
    }
   
}
