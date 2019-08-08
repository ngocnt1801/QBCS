using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QBCS.Service.Utilities
{
    public class StringProcess
    {
        public static string RemoveTag(string source, string oldString, string newString)
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
        public string RemoveHtmlBrTagForUpdateQuestion(string source)
        {
            string result = null;

            if (source != null)
            {
                result = RemoveTag(source, "\r\n", "<cbr>");
                result = RemoveTag(result, "\n", "<cbr>");
                result = RemoveTag(result, "\t\n", "");
                result = RemoveTag(result, "\t", "");
                result = RemoveTag(result, "\r", "");

            }

            return result;
        }
        public string RemoveHtmlBrTag(string source)
        {
            string result = null;

            if (source != null)
            {
                result = RemoveTag(source, @"<br>", @"\n");
                result = RemoveStringSharp(result);
                result = RemoveTag(result, @"<br/>", @"\n");
                result = RemoveTag(result, @"<br />", @"\n");
                result = RemoveTag(result, @"<br style", @"\n<br style");
                result = RemoveTag(result, @"<br>", @"\n");
                result = RemoveTag(result, @"</p>", @"</p>\n");

            }

            return result;
        }
        public string CleanInvalidXmlChars(string strInput)
        {
            //Returns same value if the value is empty.
            if (string.IsNullOrWhiteSpace(strInput))
            {
                return strInput;
            }
            // From xml spec valid chars:
            // #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]    
            // any Unicode character, excluding the surrogate blocks, FFFE, and FFFF.
            string RegularExp = @"[^\x09\x0A\x0D\x20-\xD7FF\xE000-\xFFFD\x10000-x10FFFF]";
            return Regex.Replace(strInput, RegularExp, String.Empty);
        }

        public string RemoveHtmlTagXML(string source)
        {
            string result = null;

            if (source != null)
            {
                string partern = "(<cbr>){2,}";
                string parternRemoveFirstHtml = "^(<cbr>)"; //Remove <br> start of the line

                //result = CleanInvalidXmlChars(source);             
                result = RemoveTag(source, @"\=", @"=");
                result = RemoveTag(result, @"\{", @"{");
                result = RemoveTag(result, "[moodle]", "");
                result = RemoveTag(result, "[markdown]", "");
                result = RemoveTag(result, "[plain]", "");
                result = RemoveTag(result, @"\}", @"}");
                result = RemoveTag(result, @"\#", @"#");
                result = RemoveTag(result, @"\~", @"~");
                result = RemoveTag(result, @"\:", @":");

                result = RemoveTag(result, @"\n", @"<cbr>");
                result = RemoveTag(result, @"<br />", @"<cbr>");
                result = RemoveTag(result, @"<br/>", @"<cbr>");
                result = RemoveTag(result, @"\:", @":");
                // result = RemoveTag(result, @"#", "");

                result = RemoveTag(result, @"<span lang=" + '"' + "EN" + '"' + ">", "");
                result = Regex.Replace(result, partern, @"<cbr>");
                result = Regex.Replace(result, parternRemoveFirstHtml, "");



            }

            return result;
        }
        public string RemoveWordStyle(string source)
        {
            string result = null;
            if (source != null)
            {
                string partern = "<style([\\s\\S]+?)</style>";
                result = Regex.Replace(source, partern, "");
            }
            return result;
        }
        public string RemoveHtmlTagGIFT(string source)
        {
            string result = null;

            if (source != null)
            {
                string partern = "(<cbr>){2,}";
                string parternRemoveFirstHtml = "^(<cbr>)"; //Remove <br> start of the line
                //result = RemoveTag(source, "[html]", "");
                //result = RemoveTag(source, "[html]", "");
                result = RemoveTag(source, @"\=", @"=");
                result = RemoveTag(result, @"\{", @"{");
                result = RemoveTag(result, @"\}", @"}");
                result = RemoveTag(result, "[moodle]", "");
                result = RemoveTag(result, "[markdown]", "");
                result = RemoveTag(result, "[plain]", "");
                //result = RemoveTag(result, @"\#", @"#");
                result = RemoveTag(result, @"\~", @"~");
                result = RemoveTag(result, @"\:", @":");

                result = RemoveTag(result, @"\n", @"<cbr>");
                result = RemoveTag(result, @"<br />", @"<cbr>");
                result = RemoveTag(result, @"<br/>", @"<cbr>");
                result = RemoveTag(result, @"\:", @":");
                // result = RemoveTag(result, @"\#", "#");
                result = RemoveUnExpectedTagGIFT(result);
                result = RemoveTag(result, @"<span lang=" + '"' + "EN" + '"' + ">", "");
                result = Regex.Replace(result, partern, @"<cbr>");
                result = Regex.Replace(result, parternRemoveFirstHtml, "");

            }

            return result;
        }
        public string RemoveStringSharp(string source)
        {
            string result = "";
            string UNEXPECTED_SHARP = "#<span lang";
            string EXPECTED_SHARP = "<span lang";
            string SPLASH_SHARP = @"\#<span lang";
            if (source != null)
            {
                if (source.Contains(UNEXPECTED_SHARP) && !source.Contains(SPLASH_SHARP))
                {
                    result = RemoveTag(source, UNEXPECTED_SHARP, EXPECTED_SHARP);
                }
                else
                {
                    result = source;
                }

            }
            return result;
        }
        public string GetImageNameXML(string source)
        {
            string result = "";
            string FLAG_IMAGE = "@@PLUGINFILE@@";
            if (source != null && source.Contains(FLAG_IMAGE))
            {
                StringBuilder imageName = new StringBuilder();
                string matchString = Regex.Match(source, "<img.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase).Groups[1].Value;
                StringBuilder sb = new StringBuilder();
                sb.Append(matchString);
                bool isImage = false;
                for (int i = 0; i < sb.Length; i++)
                {
                    if (sb[i].Equals('/')) //start read Image name
                    {
                        isImage = true;
                        continue;
                    }
                    if (sb[i].Equals('"'))
                    { //end read Image name
                        isImage = false;
                        continue;
                    }
                    if (isImage == true)
                    {
                        imageName.Append(sb[i]);
                    }
                }

                if (imageName != null)
                {
                    result = imageName.ToString();
                }
            }
            return result;
        }
        public string RemoveUnExpectedTagGIFT(string source)
        {
            string result = null;

            if (source != null)
            {
                if (source.Contains("#") && !source.Contains(@"\#"))
                {
                    result = RemoveTag(source, "#", "");
                    return result;
                }
                if (source.Contains(@"\#") && !source.Contains("#"))
                {
                    result = RemoveTag(source, @"\#", "#");
                    return result;
                }
                if (source.Contains(@"\#") && source.Contains("#"))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(source);
                    List<int> listArrayKeeping = new List<int>();
                    List<int> listArrayRemove = new List<int>();
                    bool isFlag = false;
                    for (int i = 0; i < sb.Length; i++)
                    {

                        if (sb[i].Equals(@"\"))
                        {
                            isFlag = true;
                            continue;
                        }
                        //if (isFlag && sb[i].Equals("#"))
                        //{
                        //    listArrayKeeping.Add(i);
                        //}
                        if (isFlag == false && sb[i].Equals("#"))
                        {
                            listArrayRemove.Add(i);
                        }
                        isFlag = false;
                    }
                    foreach (var item in listArrayRemove)
                    {
                        sb.Remove(item, 1);
                    }
                    result = sb.ToString();
                    if (result.Contains(@"\#"))
                    {
                        result = RemoveTag(result, @"\#", "#");
                    }
                }
                if (!source.Contains(@"\#") && !source.Contains("#"))
                {
                    result = source;
                }

            }

            return result;
        }
    }

}
