using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiLighting_Algorithms
{
    public enum TYPE {BUY,SELL};
    public enum ORDERSTATUS {PARTIALLY_FILLED, FILLED, REJECTED}
    public class ThresholdReachedEventArgs : EventArgs
    {
        public TYPE OrderType;

        public ORDERSTATUS OrderStatus;
        public int Threshold { get; set; }
        public DateTime TimeReached { get; set; }
    }
    public class Class1
    {
        public static event EventHandler ThresholdReached;

        protected virtual void OnThresholdReached(EventArgs e)
        {
            EventHandler handler = ThresholdReached;
            if (handler != null)
            {
                handler(this, e);
            }

        }
        /*
        public static void Main(string[] args)
        {
            MovingAverage ma = new MovingAverage("tsla,goog", 6, 12, 5.0, 500);
            MovingAverage ma1 = new MovingAverage("tsla,goog", 6, 12, 5.0, 500);
            Console.ReadKey();
        }
      */
        static void c_ThresholdReached(object sender, EventArgs e)
        {
            Console.WriteLine("The threshold was reached.");
        }
    }
}
