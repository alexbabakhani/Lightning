using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CLAlgorithms
{
    public enum POSITION { BUY, SELL}
    public class MovingAverage : IAlgorithm
    {

        public Ticker Ticker { get; set; }
        private double[] FinancialDataFeed;
        public static List<double> MovingAverageList;
        private int NumberForMovingAverage;


        private int Count = 0;
      
       

        public MovingAverage(double[] feeds, Ticker t, int mv = 5)
        {
            Ticker = t;
            FinancialDataFeed = feeds;
            NumberForMovingAverage = mv;
            MovingAverageList=new List<double>();
        }
        public bool CancelTrade()
        {
            throw new NotImplementedException();
        }

        public bool ExecuteTrade()
        {
            throw new NotImplementedException();
        }

        public bool TermimateTrade()
        {
            throw new NotImplementedException();
        }

        public void UpdateData(Feed f)
        {
           UpdateMovingAverage(f.GetPrice());
        }

        public void UpdateMovingAverage(double f)
        {
            double[] newarray = new double[NumberForMovingAverage];
            Array.Copy(FinancialDataFeed, 1, newarray, 0, newarray.Length - 1);
            newarray[NumberForMovingAverage - 1] = f;
            var average = GetAverage();
            MovingAverageList.Add(average);

        }

        public double GetAverage()
        {
            var sum = 0.00;
            for (var i = 0; i < FinancialDataFeed.Length; i++)
            {
                sum += FinancialDataFeed[i];
            }
            var average = sum/NumberForMovingAverage;
            return average;
        }


    }
}
