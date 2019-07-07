using System.Text.RegularExpressions;

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

        public static void RemoveSpace(ref string s)
        {
            s = s.Trim().Replace(" ", "");
        }

        public static void RemoveSpecialCharacter(ref string s)
        {
            RemoveHtmlSignal(ref s);
            s = Regex.Replace(s, "[.,:;]", "");
        }

        public static string[] SplitWithSpace(string s)
        {
            s = s.ToLower();
            //RemoveSpecialCharacter(ref s);
            return s.Split(' ');
        }

        public static void RemoveHtmlSignal(ref string s)
        {
            s = s.Replace("[html]", "");
        }

    }
}
