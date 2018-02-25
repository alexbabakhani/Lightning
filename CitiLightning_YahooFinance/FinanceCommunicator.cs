using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using System.Threading;
using System.Text.RegularExpressions;
namespace CitiLightning_YahooFinance
{
    public class FinanceCommunicator
    {
        private Dictionary<string, string> dictionary;
        public static ManualResetEvent resetEvent = new ManualResetEvent(false);
        private const string BASE_WEBURL_URL = "http://query.yahooapis.com/v1/public/yql?q=";
        //"http://download.finance.yahoo.com/d/quotes.csv?s=";
        private const string BASE_SUFFIX_URL = "&e=.csv";
        //private const string BASE_PREFIX_URL = "'http://download.finance.yahoo.com/d/quotes.csv?s=";
        private const string BASE_PREFIX_URL = "http://10.38.66.200:8080/Yahoo/d/quotes.csv?s=/Yahoo/d/quotes.csv?s=";
        private string URL;
        private string Ticker;

        
        public async Task<string> GetData(params string[] args)
        {
            StringBuilder webaddress = new StringBuilder();
            webaddress.Append(BASE_WEBURL_URL);
            StringBuilder columns = new StringBuilder();
            StringBuilder temp = new StringBuilder(BASE_PREFIX_URL + Ticker + "&f=");

            for (var i = 0; i < args.Length; i++)
            {
                temp.Append((dictionary[args[i].ToString()]));
                columns.Append(args[i] + ",");
            }
            temp.Append("&e=.csv'");
            webaddress.Append(HttpUtility.UrlEncode("select * from csv where url=" + temp.ToString() + " and columns=" + "'" + columns + "'"));
            webaddress.Append("&format=json");
            webaddress.Append("&diagnostics=false");
            Console.WriteLine(webaddress);
            Task<string> results;

            using (WebClient wc = new WebClient())
            {
                results = wc.DownloadStringTaskAsync(webaddress.ToString());  //asynchronous
            }
            return await results;
        }
        
        public async Task<string> GetNextPrice2()
        {
            StringBuilder webaddress = new StringBuilder();
            webaddress.Append(BASE_WEBURL_URL);
            StringBuilder columns = new StringBuilder();
            StringBuilder temp=new StringBuilder(BASE_PREFIX_URL +Ticker +  "&f=");
            temp.Append((dictionary["Price"]));
            columns.Append("Price");
            temp.Append("&e=.csv'");
            webaddress.Append(HttpUtility.UrlEncode("select * from csv where url=" + temp.ToString() + " and columns="+"'"+columns+"'"));
            webaddress.Append("&format=json");
            webaddress.Append("&diagnostics=false");
            //Console.WriteLine(webaddress);
            string returnstring;
             
            using (HttpClient wc = new HttpClient())
            {
                returnstring= await wc.GetStringAsync(webaddress.ToString());  //asynchronous
                
            }
           //var json = JsonConvert.DeserializeObject<ResponseRootObject>(returnstring);
            return returnstring;
        }
        public string GetNextPrice()
        {
            StringBuilder webaddress = new StringBuilder();
            webaddress.Append(BASE_WEBURL_URL);
            StringBuilder columns = new StringBuilder();
            StringBuilder temp = new StringBuilder(BASE_PREFIX_URL + Ticker + "&f=l1&e=.csv");
            //temp.Append((dictionary["Price"]));
          //  columns.Append("Price");
            //temp.Append("&e=.csv'");
            //webaddress.Append(HttpUtility.UrlEncode("select * from csv where url=" + temp.ToString() + " and columns=" + "'" + columns + "'"));
           // webaddress.Append("&format=json");
            //webaddress.Append("&diagnostics=false");
            //Console.WriteLine(webaddress);
            string returnstring;

            using (WebClient wc = new WebClient() )
            {
                returnstring = wc.DownloadString(temp.ToString());  //asynchronous

            }
            string replacement = Regex.Replace(returnstring, @"\t|\n|\r", "");
            // var json = JsonConvert.DeserializeObject<ResponseRootObject>(returnstring);
            return replacement;
        }
        public FinanceCommunicator(string symbol)
        {
            Ticker = symbol;
            dictionary = new Dictionary<string, string>();
            dictionary["Symbol"] = "s";
            dictionary["Description"] = "n";
            dictionary["StockExchange"] = "x";
            dictionary["Price"] = "l1";
            dictionary["Date"] = "d1";
            dictionary["Time"] = "t1";
            dictionary["Change"] = "c1";
            dictionary["Open"] = "o";
            dictionary["High"] = "h";
            dictionary["Low"] = "g";
            dictionary["Close"] = "p";
            dictionary["Volume"] = "v";
            dictionary["Earnings/Share"] = "e";
            dictionary["PeRatio"] = "r";
            dictionary["SharesOwned"] = "s1";
            dictionary["LastTradeTime"] = "k1";
            dictionary["Bid"] = "b";
            dictionary["Ask"] = "a";
            dictionary["PctChange"] = "k2";
            dictionary["HoldingsValue"] = "v1";
            dictionary["HoldingsGain"] = "g4";
            dictionary["HoldingsGainPct"] = "g1";
            dictionary["MarketCap"] = "j3";
            dictionary["AfterHoursChange"] = "c8";
            dictionary["OrderBook"] = "i5";
            dictionary["DayRange"] = "m";
            dictionary["52WeekRange"] = "w";
            dictionary["Change52WeekLow"] = "j5";
            dictionary["Change52WeekHigh"] = "k4";
            dictionary["PctChange52WkLow"] = "j6";
            dictionary["PctChange52WkHigh"] = "k5";
            dictionary["50DayMovingAvg"] = "m3";
            dictionary["Change50DayMoving"] = "m7";
            dictionary["PctChange50DayMoving"] = "m8";
            dictionary["200DayMovingAvg"] = "m4";
            dictionary["Change200DayMoving"] = "m5";
            dictionary["PctChange200DayMoving"] = "m6";

        }
       
    
    }
}
