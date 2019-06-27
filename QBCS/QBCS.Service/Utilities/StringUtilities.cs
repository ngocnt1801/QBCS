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
        public static string HtmlEncode(string textContent)
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
            result = StringProcess.RemoveTag(result, @"<cbr>", @"<br/>");
            result = WebUtility.HtmlEncode(result);
            return result;
        }
    }
}
