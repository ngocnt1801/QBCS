using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QBCS.Service.Utilities
{
    public class StringUtilities
    {
        public static string FormatStringExportGIFT(string textContent)
        {
            if (textContent == null)
                return null;

            string result = textContent;
            result = StringProcess.RemoveTag(result, @"=", @"\=");
            result = StringProcess.RemoveTag(result, @"{", @"\{");
            result = StringProcess.RemoveTag(result, @"}", @"\}");
            result = StringProcess.RemoveTag(result, @"#", @"\#");
            result = StringProcess.RemoveTag(result, @"~", @"\~");
            result = StringProcess.RemoveTag(result, @":", @"\:");                        
            result = EncodeHTML(result);
            result = StringProcess.RemoveTag(result, @"&lt;cbr&gt;", @"<br/>");
            return result;
        }
        public static string FormatStringExportXML(string textContent)
        {
            if(textContent == null)
            {
                return null;
            }
            string result = textContent;
            result = StringProcess.RemoveTag(result, @"[html]", "");
            result = EncodeHTML(result);
            result = StringProcess.RemoveTag(result, @"&lt;cbr&gt;", @"<br/>");
            return result;
        }
        public static string EncodeHTML(string source)
        {
            if (source == null)
                return null;
            string result = source;
            result = StringProcess.RemoveTag(result, @"&", @"&amp;");
            result = StringProcess.RemoveTag(result, "\"", @"&quot;");
            result = StringProcess.RemoveTag(result, @"<", @"&lt;");
            result = StringProcess.RemoveTag(result, @">", @"&gt;");
            return result;
        }
    }
}
