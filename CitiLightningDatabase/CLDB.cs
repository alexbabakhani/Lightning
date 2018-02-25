using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CitiLightningDatabase
{
    public class CLDB
    {
        public List<Ticker> GetStockTickers()
        {
            List<Ticker> tickerList;
            using (var dbcontext = new NorthwindEntities())
            {
                tickerList = (from c in dbcontext.Tickers
                              select c).ToList<Ticker>();

            }
            return tickerList;
        }
        public List<Ticker> GetStockTickers(string contains)
        {
            List<Ticker> tickerList;
            using (var dbcontext = new NorthwindEntities())
            {
                tickerList = (from c in dbcontext.Tickers
                              select c).Where(u => u.Symbol.Contains(contains) | u.Name.Contains(contains)).ToList<Ticker>();
            }
            return tickerList;
        }
        public List<Strategy> GetStrategies(int user)
        {
            List<Strategy> tickerList;
            using (var dbcontext = new NorthwindEntities())
            {
                tickerList = (from c in dbcontext.Strategies
                              select c).Where(u => u.UserId == user).ToList<Strategy>();
            }

            return tickerList;
           
        }
        /*
        public void AddToPortfolio(int userid, string symbol, int numberofshares, decimal price, bool isActive)
        {
            Portfolio p = new Portfolio();
            if (isActive)
            {
                p.IsActive = 1;
            }
            else
            {
                p.IsActive = 0;
            }
            p.NumberOfShares = numberofshares;
            p.PriceBought = price;
            p.Symbol = symbol;
            p.UserID = userid;
        }*/
        public void AddTransaction(Transaction transaction) {

            using (var dbcontext = new NorthwindEntities())
            {
                dbcontext.Transactions.Add(transaction);
                dbcontext.SaveChanges();
            }
        
        }
        public void AddTransaction(decimal price, string strategy, string symbol, DateTime timestamp, bool boolean, int size, string status = "N/A")
        {
            Transaction t = new Transaction();
            t.Price = price;
            t.Status = status;
            t.Strategy=strategy;
            t.Symbol=symbol;
            t.Timestamp = timestamp;
            t.Size = size;
            t.Buy = boolean;
            using (var dbcontext = new NorthwindEntities())
            {
                dbcontext.Transactions.Add(t);
                dbcontext.SaveChanges();
            }
        }
        public List<Transaction> GetTransactions(int userID)
        {
            List<Transaction> transactions;
            using (var dbcontext = new NorthwindEntities())
            {
                transactions = (from c in dbcontext.Transactions
                                 select c).Where(x => x.UserId==userID).ToList<Transaction>();
                
            }
            return transactions;

        }
        public void AddStockTicker(List<Ticker> tickers)
        {
            using (var dbcontext = new NorthwindEntities())
            {
                foreach (Ticker t in tickers)
                {
                    dbcontext.Tickers.Add(t);


                } 
                var q = dbcontext.SaveChanges();
            }
        }
    }
}
