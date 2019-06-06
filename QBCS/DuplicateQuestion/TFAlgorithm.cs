using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuplicateQuestion
{
    public class TFAlgorithm
    {
        public static double CaculateSimilar(string source, string target)
        {

            string[] sourceArr = StringUtils.SplitWithSpace(source);
            string[] targetArr = StringUtils.SplitWithSpace(target);
            string commonWord = "the,a,an," +
                                "is,are,am,was,were," +
                                "do,does,";

            Dictionary<string, int[]> map = new Dictionary<string, int[]>();

            //assign vector source string
            for (int i = 0; i < sourceArr.Length; i++)
            {
                if (map.ContainsKey(sourceArr[i]))
                {
                    int count = map[sourceArr[i]][0];
                    map[sourceArr[i]][0] = count + 1;
                }
                else
                {
                    map.Add(sourceArr[i], new int[2] { 1, 0 });
                }
            }

            //assign vector target string
            for (int i = 0; i < targetArr.Length; i++)
            {
                if (map.ContainsKey(targetArr[i]))
                {
                    int count = map[targetArr[i]][1];
                    map[targetArr[i]][1] = count + 1;
                }
                else
                {
                    map.Add(targetArr[i], new int[2] { 0, 1 });
                }
            }

            //caculate similar by cosine
            int vectorMultiple = 0;
            int scalarsSource = 0;
            int scalarsTarget = 0;

            foreach (KeyValuePair<string, int[]> item in map)
            {
                if (!commonWord.Contains(item.Key))
                {
                    vectorMultiple += item.Value[0] * item.Value[1];
                    scalarsSource += item.Value[0] * item.Value[0];
                    scalarsTarget += item.Value[1] * item.Value[1];
                }
            }

            double stringSimilar = vectorMultiple / (Math.Sqrt(scalarsSource) * Math.Sqrt(scalarsTarget));

            return stringSimilar * 100;
        }

    }
}
