using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QBCS.Service.Utilities
{
    public class StringUtilities
    {
        public static string HtmlEncode(string text)
        {
            if (text == null)
                return null;

            StringBuilder stringBuilder = new StringBuilder(text.Length);

            int len = text.Length;
            for (int i = 0; i < len; i++)
            {
                switch (text[i])
                {
                    case '<':
                        stringBuilder.Append("&lt;");
                        break;
                    case '>':
                        stringBuilder.Append("&gt;");
                        break;
                    case '"':
                        stringBuilder.Append("&quot;");
                        break;
                    case '&':
                        stringBuilder.Append("&amp;");
                        break;
                    default:
                        stringBuilder.Append(text[i]);
                        break;
                }
            }
            return stringBuilder.ToString();
        }
    }
}
