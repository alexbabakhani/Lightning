using CitiLightning_YahooFinance;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Strategies.BrokerMessaging;
using CitiLightningDatabase;
namespace CitiLighting_Algorithms
{
    public enum POSITION { BUY, SELL}
    public enum CONDITION {NONE, SHORTBELOWLONG, LONGBELOWSHORT}

    public struct Order
    {
        public int NumberOfShares;
        public decimal Price;
        public POSITION Position;
    }
   
    
    public class MovingAverage : IAlgorithm, IEngine
    {

        public string Ticker { get; set; }
        public static List<double> MovingAverageList;
        private Queue<decimal> ShortMA;
        private Queue<decimal> LongMA;
        private Timer timer;
        private int ShortMAInterval;
        private int LongMAInterval;
        private int ShortMACount=0;
        private int LongMACount=0;
        private double PercentGain;
        private int NumberofShares;
        private int CurrentShares = 0;
        private double InitialInvestment;
        public Average Average { get; set; }
        private FinanceCommunicator yahoo;
        public CONDITION condition { get; set; }
        public event EventHandler MessageBrokerComms;
        public Hashtable orders = new Hashtable();
        private const int TIMER_TICKS = 500;
        public bool FirstTrade=false;
        public void Run()
        {

        }


        
        public void TradeExecuted(object sender, EventArgs e)
        {
            Console.WriteLine("The threshold was reached.");
        }
        public Message GetNextOpenOrder()
        {
            int shares = 0;
            foreach (DictionaryEntry entry in orders)
            {
                var o = (Message)entry.Value;
                if (o.Open)
                {
                    return o;
                }
            }
            return null;
        }
        public decimal GetPrice()
        {
            string a = yahoo.GetNextPrice();
            //double d = Convert.ToDouble(JsonConvert.DeserializeObject<ResponseRootObject>(a).query.row.row[0].Price);
            
            return Convert.ToDecimal(a);
        }
        public void ConditionChanged(object sender, EventArgs e)
        {
            ConditionChangeEventArgs args = e as ConditionChangeEventArgs;
            var newMessage = GetNextOpenOrder();
            if (newMessage == null) {
                newMessage = new Message();
                var id = GetRandomId();
                newMessage.id = id;
                newMessage.symbol = Ticker;
                newMessage.size = NumberofShares;
                newMessage.price = Convert.ToDouble(GetPrice());
                newMessage.Open = true;
                newMessage.dateTime = DateTime.Now;
                InitialInvestment = newMessage.size * newMessage.price;
                 if (args.NewCondition == CONDITION.LONGBELOWSHORT)
                {
                    newMessage.b = true;
                    newMessage.Position="LONG";
                }
                else
                {
                     newMessage.b = false;
                     newMessage.Position="SHORT";
                }
               
                orders[id] = newMessage;
                OnThresholdReached(new ExecuteTradeEventArgs(newMessage));
            }
                /*
            else
            {
                if (newMessage.b)
                {
                    newMessage.b = false;
                }
                else
                {
                    newMessage.b = true;
                }
                    newMessage.closeprice = Convert.ToDouble(yahoo.GetNextPrice());
            
            
                    CurrentShares = NumberofShares;
            
                    double percent = ((newMessage.price-newMessage.closeprice)*(newMessage.size))/100;
                    if (percent>PercentGain)
                    {
                        OnThresholdReached(new ExecuteTradeEventArgs(newMessage));
                    }
                     
                if ((InitialInvestment == null) || (InitialInvestment == 0))
                {
                    InitialInvestment = newMessage.size * newMessage.price;
                }

                newMessage.closedate = DateTime.Now;
                
                
                OnThresholdReached(new ExecuteTradeEventArgs(newMessage));
            
            else
            {
                var t = CurrentGain();
                Console.WriteLine("DONE");
            }*/
            
        }
        public int GetRandomId()
        {
            Random random = new Random();
            return random.Next(Int32.MaxValue - 1);
        }
        
        public bool ShouldIShort()
        {
            if ((CurrentShares == 0) & (condition == CONDITION.LONGBELOWSHORT))
            {
                return true;
            }
            return false;
        }
        public bool IsPercentageReached()
        {
                
                Message m = GetNextOpenOrder();
                if (m != null)
                {
                    var doubs = GetPrice();
                    double percent = (((m.price - Convert.ToDouble(doubs)) * (m.size))/InitialInvestment) * 100;
                    if (Math.Abs(percent) > PercentGain)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            

        }
        public int GetCurrentShares()
        {
            int shares = 0;
            foreach (DictionaryEntry entry in orders)
            {
                var o = (Message)entry.Value;
                if (o.Open)
                {
                    shares += o.size;
                }
            }
            return shares;
        }
        public double CurrentGain()
        {
            double cost = 0.0;
            foreach (DictionaryEntry entry in orders)
            {
                var o = (Message)entry.Value;

                cost += ((o.price - o.closeprice) * o.size);
                
            }
            if (cost != 0.0)
            {
                return (cost);
            }
            return 0;
        }
        protected virtual void OnThresholdReached(EventArgs e)
        {
            EventHandler handler = MessageBrokerComms;
            if (handler != null)
            {
                handler(this, e);
            }

        }
       
        public void InitTimer()
        {
            timer= new Timer();
            timer.Elapsed += new ElapsedEventHandler(Timer_Tick);
            timer.Interval = TIMER_TICKS; // in miliseconds
            timer.Start();
        }
        /*
        public async Task<decimal> GetCurrentPrice()
        {
            string s = await yahoo.GetNextPrice();

            return Convert.ToDecimal(JsonConvert.DeserializeObject<ResponseRootObject>(s).query.row.row[0].Price);
        }*/
        private async void Timer_Tick(object sender, EventArgs e)
        {
            /*Task<string> s = yahoo.GetNextPrice();
            string a = await s;*/
            decimal d = Convert.ToDecimal(GetPrice());
            //Do this while the LongQueue is not filled.
            if (LongMACount < LongMAInterval)
            {
                LongMA.Enqueue(d);
                LongMACount += 1;
                if (ShortMACount < ShortMAInterval)
                {
                    ShortMA.Enqueue(d);
                    ShortMACount += 1;
                }
                else
                {
                    ShortMA.Dequeue();
                    ShortMA.Enqueue(d);
                }
            }
            else
            {
                if (Average == null) //First Average Coming through
                {
                    Average = GetAverage();
                }
                else
                {
                    ShortMA.Dequeue();
                    LongMA.Dequeue();
                    ShortMA.Enqueue(d);
                    LongMA.Enqueue(d);
                    var newavg = GetAverage();
                    Average.LongAverage = newavg.LongAverage;
                    Average.ShortAverage = newavg.ShortAverage;
                    if(IsPercentageReached()){
                        ClosePosition();
                    }
                }
            }
        }
        public void ClosePosition(){
            Message m=GetNextOpenOrder();
            if(m.Position=="SHORT"){
                m.b=true;
            }
            else{
                m.b=false;
            }
            m.Open = false;
            CLDB db = new CLDB();
            Transaction t=new Transaction();
            t.Buy=m.b;
            t.Price=Convert.ToDecimal(m.price);
            t.Size=m.size;
            t.Strategy=("Two Moving Averages");
            t.Symbol=m.symbol;
            t.Timestamp=m.dateTime;
            t.UserId=1;
            db.AddTransaction(t);
            OnThresholdReached(new ExecuteTradeEventArgs(m));
        }
        public Average GetAverage() 
        {
            decimal sum = 0.00M;
            foreach (decimal d in LongMA)
            {
                sum += d;
            }
            var laverage = sum / LongMAInterval;
            sum = 0.00M;
            foreach (decimal d in ShortMA)
            {
                sum += d;
            }
            var saverage = sum / ShortMAInterval;
            var average=new Average(laverage, saverage);
            average.PropertyChanged += ConditionChanged;
         
            return average;
        }
        public MovingAverage(string symbol, int SMAInterval, int LMAInterval, double percentGain, int numberShares )
        {
            Ticker = symbol;
            ShortMAInterval = SMAInterval;
            LongMAInterval = LMAInterval;
            PercentGain = percentGain;
            NumberofShares = numberShares;
            ShortMA = new Queue<decimal>();
            LongMA = new Queue<decimal>();
            yahoo = new FinanceCommunicator(Ticker);
            InitTimer();
           // Initialize();
        }
        public void Initialize()
        {
            //FinancialDataFeed = new double[NumberForMovingAverage];
           
            InitTimer();
            while(LongMACount==LongMAInterval){

            }
            foreach (decimal d in ShortMA)
            {
                Console.WriteLine(d);
            }
         
            Console.ReadKey();
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
          //UpdateMovingAverage(f.GetPrice());
       }
      /*
       public void UpdateMovingAverage(double f)
       {
           double[] newarray = new double[NumberForMovingAverage];
           Array.Copy(FinancialDataFeed, 1, newarray, 0, newarray.Length - 1);
           newarray[NumberForMovingAverage - 1] = f;
           var average = GetAverage();
           MovingAverageList.Add(average);

       }
        */
      
       

    }
}
