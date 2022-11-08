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
        bool isEnemy;
        
        public Dice(int value, bool isExplousion = false, int count = 1, bool isEnemy = false, bool isNeedTwoMax = false)
        {
            this.value = value;
            this.isExplousion = isExplousion;
            this.count = count;
            this.isEnemy = isEnemy;
        }

        public List<long> Roll(Random rand)
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

            return rezult;
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

    public class DiceHand
    {
        List<Dice> diceHand;
        Random rand;
        public long bonus;
        public int needCount;
        public double mult;

        public DiceHand(long bonus = 0, double mult = 1, int needCount = 0)
        {
            rand = new Random(DateTime.UtcNow.Millisecond);
            this.bonus = bonus;
            this.diceHand = new List<Dice>();
            this.needCount = needCount;
            this.mult = mult;
        }

        private long takeMaxDices(int lastDice, List<long> rezult)
        {
            if (rezult.Count == 0 || lastDice == 0)
            {
                return 0;
            }
            
            long nowMax = rezult.Max();
            rezult.Remove(nowMax);
            long tmp = takeMaxDices(lastDice - 1, rezult);
            rezult.Add(nowMax);

            return tmp + nowMax;
        }

        public long rollDices()
        {
            if (diceHand.Count != 0)
            {
                List<long> rezult = new List<long>();

                for (int i = 0; i < diceHand.Count; i++)
                {
                    rezult.Concat<long>(diceHand[i].Roll(rand));
                }

                //бонус
                long sum = bonus;
                
                //ограниченное число кубов
                if (needCount != 0)
                {
                    sum += takeMaxDices(needCount, rezult);
                } else
                {
                    sum += rezult.Sum();
                }

                //умножение
                sum = Convert.ToInt64(sum * mult);

                return sum;
            }
            else
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

            sum += new Dice(4, true, 8, true, true).Roll(rand)[0];
            sum += new Dice(12, true, 2, false, true).Roll(rand)[0];

            return sum;
        }
    }

    class DiceHandBuilder
    {
        Random rand;
        long iterCount;
        public DiceHand diceHand;
        public DiceHandBuilder(long i)
        {
            this.diceHand = new DiceHand();
            this.iterCount = i;
        }

        public DiceHandBuilder(long i, DiceHand diceHand)
        {
            this.diceHand = diceHand;
            this.iterCount = i;
        }

        public PointPairList getRandomScheme()
        {
            PointPairList points = new PointPairList();

            Dictionary<long, double> scheme = new Dictionary<long, double>();
            for (long i = 1; i <= iterCount; i++)
            {
                long tmp = diceHand.rollDices();
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
                long tmp = diceHand.rollDices();
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
