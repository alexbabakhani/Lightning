using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CitiLightning_YahooFinance;
using Newtonsoft.Json;
using System.Xml;
using System.IO;
using CitiLightningDatabase;
using System.Globalization;
using CitiLighting_Algorithms;
namespace Testing
{
    class Program
    {
        static ManualResetEvent resetEvent = new ManualResetEvent(false);



        static void Main(string[] args)
        {
            //DateTime.ParseExact("2014-07-31T22:33:22.801-04:00","yyyy-MM-ddThh:mm:ss.fff-ff:ff",CultureInfo.InvariantCulture);
          //  var dt= Convert.ToDateTime("2014-07-31T22:33:22.801-04:00");
          //  CLDB db = new CLDB();
          //  db.AddTransaction(2.5M, "blank", "aapl", DateTime.Now, true, 2000, "hi");
            
            /*
            //string s=String.Format("{0}YYYY-mm-dd.ffffff", DateTime.Now);
            Console.WriteLine(DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.fff-ff:ff"));
            //Console.WriteLine(s);//
            //<whenAsDate>2014-07-31T22:33:22.801-04:00</whenAsDate>
            
            Program prog = new Program();
            var fc = new CitiLightning_YahooFinance.FinanceCommunicator("AAPL,GOOG");
            //prog.ActivelyRetrieveTickerPrice();
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("query");
            XmlNode price = xmlDoc.CreateElement("price");
            rootNode.AppendChild(price);
            xmlDoc.AppendChild(rootNode);
            Console.WriteLine(xmlDoc);
            Console.ReadKey();*/
            //Program prog = new Program();
            //prog.AddTickersFromFile();
           // MovingAverage ma = new MovingAverage("tsla,goog", 6, 12, 5.0, 500);
            //Program prog = new Program();
            Console.WriteLine(String.Format("{0:yyyy-MM-ddThh:mm:ss.fff-ff:ff}", DateTime.Now));
            Console.ReadKey();
            FinanceCommunicator comms = new FinanceCommunicator("AAPL");
            //string d=comms.GetNextPrice2();
            //prog.temp();
            //FinanceCommunicator comm = new FinanceCommunicator("aapl,goog");
            //string a=comm.GetNextPrice().Result;
        }
        
       
        public void AddTickersFromFile()
        {
            CLDB db = new CLDB();
            var lines = File.ReadLines(@"NASDAQ.txt");
            List<Ticker> tickers = new List<Ticker>();
            
            foreach (string line in lines)
            {
                Ticker tick = new Ticker();
                
                var splitcomma = line.Split(',');
                tick.Name = splitcomma[1];
                tick.Symbol = splitcomma[0];
                tickers.Add(tick);
            }
            lines = File.ReadLines(@"NYSE.txt");
            tickers = new List<Ticker>();

            foreach (string line in lines)
            {
                Ticker tick = new Ticker();

                var splitcomma = line.Split(',');
                tick.Name = splitcomma[1];
                tick.Symbol = splitcomma[0];
                tickers.Add(tick);
            }
            db.AddStockTicker(tickers);
        }
        /*
        public async void ActivelyRetrieveTickerPrice()
        {
            var fc = new CitiLightning_YahooFinance.FinanceCommunicator("AAPL,GOOG");
            while (true)
            {
                string s = await fc.GetNextPrice();
                var response=JsonConvert.DeserializeObject<ResponseRootObject>(s);
                Console.WriteLine(response.query.row.row[0].Price);
            }
        }*/
    }
}
