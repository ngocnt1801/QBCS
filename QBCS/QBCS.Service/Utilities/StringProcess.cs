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
        public string RemoveHtmlBrTag(string source)
        {
            string result = null;

            if (source != null)
            {
                result = RemoveTag(source, @"<br>", @"\n");
            }

            return result;
        }
        public string RemoveHtmlTag(string source)
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
    }
   
}
