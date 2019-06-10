using DuplicateQuestion;
using QBCS.Entity;
using QBCS.Repository.Implement;
using QBCS.Repository.Interface;
using QBCS.Service.Utilities;
using SimMetrics.Net.Metric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QBCS.Service.Implement
{
    public class TestService
    {
        private IUnitOfWork u;
        public TestService()
        {
            u = new UnitOfWork();
        }

        public int TestFunc()
        {
            return u.Repository<Question>().GetAll().First().Options.Count;
        }

        public double CheckSimilarity()
        {

            var list = u.Repository<Question>().GetAll().ToList();

            //double[] results = new double[list.Count];

            //for (int i = 0; i < list.Count; i++)
            //{
            //    //results[i] = 100 - StringSimilarity.LevenshteinCaculate(list[0].QuestionContent, list[i].QuestionContent) * 100 / list[0].QuestionContent.Length;
            //    //results[i] = StringSimilarity.RateSimilarity(list[0].QuestionContent, list[i].QuestionContent);
            //    //var sim = new CosineSimilarity();
            //    //var sim = new JaccardSimilarity();
            //    var sim = new DiceSimilarity();
            //    results[i] = sim.GetSimilarity(list[0].QuestionContent, list[i].QuestionContent);
            //}
            //var sim = new DiceSimilarity();
            var sim = new CosineSimilarity();
            //var sim = new OverlapCoefficient();
            //var sim = new BlockDistance();
            //var sim = new ChapmanLengthDeviation(); // neu cau ngan qua thi xac xuat cao cung chua chac la giong nhau
            //var sim = new MongeElkan();// tham chieu tu Jaro Winkler https://cs.stackexchange.com/questions/32530/a-reference-for-pseudocode-for-monge-elkan-algorithm
            //var sim = new SmithWatermanGotohWindowedAffine();
            //double result = sim.GetSimilarity("What is software engineering", "software engineering is __");
            //double result = sim.GetSimilarity("i bit a dog", "a dog bit me");
            double result = sim.GetSimilarity("By switching on and off, the __ can be used to represent the 1s and 0s that are foundation of all that goes on in the computer",
                                                "what can be used to represent the on and off that are foundation of all that goes on in the computer?");
            return result;

        }

        public int CountDistinctWord()
        {
            var list = u.Repository<Question>().GetAll().ToList();
            IDictionary<string, int> map = new Dictionary<string, int>();
            foreach (var item in list)
            {
                string[] words = item.QuestionContent.Trim().Split(' ');
                foreach (string word in words)
                {
                    if (map.ContainsKey(word.Trim().ToLower()))
                    {
                        //map[word] = map[word] + 1;

                    }else
                    {
                        map.Add(word.Trim().ToLower(), 0);
                    }
                }
            }

            return map.Count;
        }

        public string CheckDuplidate()
        {
            //test
            string s1 = "By switching on and off, the __ can be used to represent the 1s and 0s that are foundation of all that goes on in the computer";
            string s2 = "what can be used to represent the 1s and 0s that are foundation of all that goes on in the computer?";
            s1 = StringUtils.NormalizeString(s1);
            s2 = StringUtils.NormalizeString(s2);

            double result12 = LevenshteinDistance.CalculateSimilarity(s1, s2);
            double result21 = LevenshteinDistance.CalculateSimilarity(s2, s1);

            return "S1-S2: " + result12 + " - S2-S1: " + result21;
        }
    }
}
