using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace WindowsFormsApp3
{
    class Dice
    {
        int value;
        int count;
        bool isExplousion;
        bool isNeedTwoMax;
        bool isEnemy;
        
        public Dice(int value, bool isExplousion = false, int count = 1, bool isEnemy = false, bool isNeedTwoMax = false)
        {
            this.value = value;
            this.isExplousion = isExplousion;
            this.count = count;
            this.isNeedTwoMax = isNeedTwoMax;
            this.isEnemy = isEnemy;
        }

        public long Roll(Random rand)
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
            }
            else
            {
                for (int i = 0; i < rezult.Count; i++)
                {
                    sum += rezult[i];
                }
            }

            return sum;
        }
    }

    class FateDice : Dice
    {
        int value;
        int count;
        bool isExplousion;
        bool isNeedTwoMax;
        bool isEnemy;

        public FateDice(bool isExplousion = false, int count = 1, bool isEnemy = false, bool isNeedTwoMax = false) : base(0, isExplousion, count, isEnemy, isNeedTwoMax)
        {
            
        }

        public new long Roll(Random rand)
        {
            List<long> rezult = new List<long>();

            for (int i = 1; i <= count; i++)
            {

                long tmp = rand.Next(0, 1 + 1);
                long oneDiceSum = tmp;

                if (isExplousion && tmp == value)
                {
                    while (tmp == value)
                    {
                        tmp = rand.Next(0, 1 + 1);
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
            }
            else
            {
                for (int i = 0; i < rezult.Count; i++)
                {
                    sum += rezult[i];
                }
            }

            return sum;
        }
    }
    class DiceRandom
    {
        Random rand;
        long iterCount;
        List<Dice> diceHand;
        public DiceRandom(int r, long i)
        {
            this.rand = new Random(r);
            this.iterCount = i;
        }

        public DiceRandom(int r, long i, List<Dice> diceHand)
        {
            this.diceHand = diceHand;
            this.rand = new Random(r);
            this.iterCount = i;
        }

        private long rollDices()
        {
            if (diceHand.Count != 0)
            {
                long sum = 0;
                for (int i = 0; i < diceHand.Count; i++)
                {
                    sum += diceHand[i].Roll(rand);
                }
                return sum;
            } else
            {
                return rollProgramm();
            }
            
        }

        private long rollProgramm()
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

            sum += new Dice(4, true, 8, true, true).Roll(rand);
            sum += new Dice(12, true, 2, false, true).Roll(rand);

            return sum;
        }

        public PointPairList getRandomScheme()
        {
            PointPairList points = new PointPairList();

            Dictionary<long, double> scheme = new Dictionary<long, double>();
            for (long i = 1; i <= iterCount; i++)
            {
                long tmp = rollDices();
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
                long tmp = rollDices();
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
