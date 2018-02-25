using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiLightningDatabase
{
    public class CL_Database
    {
        public List<Ticker> GetStockTickers()
        {
            List<Ticker> tickerList;
            using (var dbcontext = new CL_DBEntities())
            {
                tickerList = (from c in dbcontext.Tickers
                        select c).ToList<Ticker>();
                
            }
            return tickerList;
        }
        public List<Ticker> GetStockTickers(string contains)
        {
            List<Ticker> tickerList;
            using(var dbcontext=new CL_DBEntities()){
                tickerList = (from c in dbcontext.Tickers
                              select c).Where(u => u.Symbol.Contains(contains) | u.Description.Contains(contains)).ToList<Ticker>();
            }
            return tickerList;
        }
        public void AddStockTicker(Ticker tickers)
        {
            using (var dbcontext = new CL_DBEntities())
            {
                dbcontext.Tickers.Add(tickers);
                var q = dbcontext.SaveChanges();
            }
        }
        public void AddStockTicker(List<Ticker> tickers)
        {
            using (var dbcontext = new CL_DBEntities())
            {
                foreach (Ticker t in tickers)
                {
                    dbcontext.Tickers.Add(t);
                   

                }
                var q=dbcontext.SaveChanges();
            }
        }
    }
}
