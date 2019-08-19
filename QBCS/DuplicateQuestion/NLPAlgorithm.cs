using WordsMatching;

namespace DuplicateQuestion
{
    public class NLPAlgorithm
    {
        public static double CaculateSimilar(string s1, string s2)
        {
            Wnlib.WNCommon.path = @"F:\WordNet\dict\";

            s1 = StringUtils.NormalizeString(s1);
            s2 = StringUtils.NormalizeString(s2);

            SentenceSimilarity semsim = new SentenceSimilarity();
            float score = 0;
            score = semsim.GetScore(
            s1,
            s2);

            return score * 100;
        }
    }
}
