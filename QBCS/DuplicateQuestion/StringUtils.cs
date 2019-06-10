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
            s = Regex.Replace(s, "[^a-zA-Z0-9\\s]", "");
        }

        public static string[] SplitWithSpace(string s)
        {
            s = s.ToLower();
            RemoveSpecialCharacter(ref s);
            return s.Split(' ');
        }

    }
}
