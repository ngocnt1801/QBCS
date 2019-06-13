using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Utilities
{
    public static class StringProcess
    {
        public static string RemoveTag (string source, string oldString, string newString)
        {
            string result = "";
            if (source != "")
            {
                result = source.Replace(oldString, newString);
            }
            return result;
        }
    }
}
