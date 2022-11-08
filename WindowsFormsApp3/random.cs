using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace WindowsFormsApp3
{
    class DiceRandom
    {
        Random rand;
        long iterCount;
        public DiceRandom(int r, long i = 100000)
        {
            this.rand = new Random(r);
            this.iterCount = i;
        }

        long rollDice(int value, bool isExplousion = false, int count = 1, bool isNeedTwoMax = false)
        {
            List<long> rezult = new List<long>();

            for (int i = 1; i <= count; i++)
            {
                
                long tmp = rand.Next(1, value + 1);
                long oneDiceSum = tmp;

                if (isExplousion && tmp == value)
                {
                    while (tmp == value)
                    {
                        tmp = rand.Next(1, value + 1);
                        oneDiceSum += tmp;
                    }
                }
                rezult.Add(oneDiceSum);
            }

            long sum = 0;
            if (isNeedTwoMax)
            {
                long tmp = rezult.Max();
                sum += rezult.Max();
                rezult.Remove(tmp);
                sum += rezult.Max();
                rezult.Add(tmp);
            } else
            {
                for (int i = 0; i < rezult.Count; i++)
                {
                    sum += rezult[i];
                }
            }

            return sum;
        }

        long rollFateDice()
        {
            return rand.Next(0, 1 + 1);
        }

        long Roll()
        {
            // вписать значения
            long sum = 0;
            int tmp;
            //tmp = rand.Next(1, 6+1);
            //if (tmp % 2 == 1)
            //{
            //    tmp = -tmp;
            //}
            //sum += tmp;
            //tmp = rand.Next(1, 6 + 1);
            //if (tmp % 2 == 1)
            //{
            //    tmp = -tmp;
            //}
            //sum += tmp;

            sum -= rollDice(4, true, 8, true);
            sum += rollDice(12, true, 2, true);

            return sum;
        }

        public PointPairList getRandomScheme()
        {
            PointPairList points = new PointPairList();

            Dictionary<long, double> scheme = new Dictionary<long, double>();
            for (long i = 1; i <= iterCount; i++)
            {
                long tmp = Roll();
                if (scheme.ContainsKey(tmp))
                {
                    scheme[tmp] += 1.0;
                } 
                else
                {
                    scheme.Add(tmp, 1);
                }
                
            }

            foreach (var i in scheme) {
                points.Add(i.Key, i.Value / iterCount*100);
            }
            points.Sort();

            return points;
        }

        public PointPairList getMoreThenRandomScheme()
        {
            PointPairList points = new PointPairList();
            PointPairList pointsMoreThen = new PointPairList();

            Dictionary<long, double> scheme = new Dictionary<long, double>();
            for (long i = 1; i <= iterCount; i++)
            {
                long tmp = Roll();
                if (scheme.ContainsKey(tmp))
                {
                    scheme[tmp] += 1.0;
                }
                else
                {
                    scheme.Add(tmp, 1);
                }

            }

            foreach (var i in scheme)
            {
                points.Add(i.Key, i.Value / iterCount * 100);
            }
            points.Sort();

            foreach (var i in points)
            {
                double sum = 0;
                foreach (var j in points)
                {
                    if (j.X >= i.X)
                    {
                        sum += j.Y;
                    }
                }
                pointsMoreThen.Add(i.X, sum);
            }

            return pointsMoreThen;
        }
    }
}
