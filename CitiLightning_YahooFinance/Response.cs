using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiLightning_YahooFinance{
//{{"query":{"count":1,"created":"2015-08-06T14:49:31Z","lang":"en-US","results":{"row":{"Price":"115.495"}}}}
    public class Query
    {
         [JsonProperty("count")]
        public int count { get; set; }

         [JsonProperty("created")]
        public string created { get; set; }

         [JsonProperty("lang")]
        public string lang { get; set; }

         [JsonProperty("results")]
        public rows row { get; set; }
    }
    public class rows
    {
        [JsonProperty("row")]
        public List<Data> row { get; set; }
    }
    public class Data{
        public string Price{get;set;}
        public string Symbol;
        public string Description;
        public string StockExhange{get;set;}
        public string Date{get;set;}
        public string Time;
        public string Change;
        public string Open{get;set;}
        public string High;
        public string Close;
        public string Volume{get;set;}
        public string EarningsPerShare;
        public string PeRatio;
        public string SharesOwned{get;set;}
        public string LastTradeTime;
        public string Bid;
        public string Ask{get;set;}
        public string PctChange;
        public string HoldingsValue;
        public string HoldingsGain{get;set;}
        public string HoldingsGainPct;
        public string MarketCap;
        public string AfterHoursChange{get;set;}
        public string OrderBook;
        public string DayRange;
        public string WeekRange52{get;set;}
        public string Change52WeekLow;
        public string Change52WeekHigh;
        public string PctChange52WkLow{get;set;}
        public string PctChange52WkHigh;
        public string DayMovingAvg50;
        public string Change50DayMoving{get;set;}
        public string PctChange50DayMoving;
        public string DayMovingAvg200;
        public string Change200DayMoving;
        public string PctChange200DayMoving;
          
    }
    public class ResponseRootObject
    {
        public Query query { get; set; }
    }
}