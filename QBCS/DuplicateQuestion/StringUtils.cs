using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DuplicateQuestion
{
    public class StringUtils
    {
        public static string NormalizeString(string s)
        {
            s = s.ToLower();
            RemoveSpace(ref s);
            RemoveSpecialCharacter(ref s);
            return s;
        }

        private static void RemoveSpace(ref string s)
        {
            s = s.Trim().Replace(" ", "");
        }

        private static void RemoveSpecialCharacter(ref string s)
        {
            s = Regex.Replace(s, "[^a-zA-Z0-9]", "");
        }

    }
}
