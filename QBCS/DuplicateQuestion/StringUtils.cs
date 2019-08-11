using System.Text.RegularExpressions;

namespace DuplicateQuestion
{
    public class StringUtils
    {
        public static string NormalizeString(string s)
        {
            s = s.ToLower();
            RemoveHtmlSignal(ref s);
            RemoveNoContent(ref s);
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
            s = Regex.Replace(s, "[.,:;]", "");
        }

        public static string[] SplitWithSpace(string s)
        {
            s = s.ToLower();
            RemoveHtmlSignal(ref s);
            RemoveNoContent(ref s);
            RemoveSpecialCharacter(ref s);
            return s.Split(' ');
        }

        public static void RemoveHtmlSignal(ref string s)
        {
            s = s.Replace("[html]", "");
        }

        public static void RemoveNoContent(ref string s)
        {
            RemoveAString(ref s, "<cbr>");
        }

        public static void RemoveAString(ref string s, string d)
        {
            s = Regex.Replace(s, d, "");
        }

        public static void PreProcessString(ref string s)
        {
            s = s.ToLower();
            RemoveHtmlSignal(ref s);
            RemoveNoContent(ref s);
            RemoveSpecialCharacter(ref s);
        }

    }
}
