using Strategies.BrokerMessaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiLighting_Algorithms
{
    public class MovingAverageEngine : IEngine
    {
        public static BrokerMessaging broker = new BrokerMessaging();
        public static event EventHandler EngineComms;
        public MovingAverage ma;
        public static void Main(string[] args)
        {
            MovingAverageEngine engine = new MovingAverageEngine();
            Console.ReadKey();
        }
        public MovingAverageEngine()
        {
            ma = new MovingAverage("tsla", 6, 12, 1.0, 5000);
            ma.MessageBrokerComms += ExecuteTrade;
            
            broker.ThresholdReached += TradeExecuted;
            //broker.SendOrderToBroker();
            broker.ListenForBrokerResponse();
            
            //var dt = DateTime.ParseExact("2014-07-31T22:33:22.801-04:00", "yyyy-MM-ddThh:mm:ss.fff-ff:ff", new CultureInfo("en-US"));
            //Console.WriteLine(dt);
            //CLDB c = new CLDB();
            //Console.WriteLine("Adding transaction...");
            //c.AddTransaction(2.5M, "strategy", "ll", DateTime.Now, true, 52, "hello");
            
        }
        public void Run()
        {
               
        }
        public void TradeExecuted(object sender, EventArgs e)
        {
            
            var args = e as ExecuteTradeEventArgs;/*
            if (ma.orders.ContainsKey(args.Message.id))
            {
                Message o = (Message)(ma.orders[args.Message.id]);
                o.Open = false;
            }*/
            
        }
        public void ExecuteTrade(object sender, EventArgs e)
        {
            var args=e as ExecuteTradeEventArgs;
            Console.WriteLine("The threshold was reached.");
            broker.SendOrderToBroker(args.Message);
        }
        

   
        
        
    }
}
