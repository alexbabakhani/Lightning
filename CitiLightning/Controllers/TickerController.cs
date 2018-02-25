using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using CitiLightningDatabase;
using CitiLightning_BusinessLogic;
using System.Web.Http.Cors;
using Newtonsoft.Json;
namespace CitiLightning.Controllers
{
  
    public class TickerController : ApiController
    {
        private NorthwindEntities db = new NorthwindEntities();
        private CitiLightning_BusinessLogic.CLDB BL = new CitiLightning_BusinessLogic.CLDB();

        // GET api/Ticker
        public List<Ticker> GetTickers()
        {
           // List<Ticker> tickers = BL.GetStockTickers();

           // return tickers;
            List<Ticker> tickers = BL.GetStockTickers();
            return tickers;
        }

        // GET api/Ticker/5
        [ResponseType(typeof(Ticker))]
        public List<Ticker> GetTicker(string id)
        {
            List<Ticker> tickers = BL.GetStockTickers(id);
            return tickers;
            //return tickers;
        }

        // PUT api/Ticker/5
        public async Task<IHttpActionResult> PutTicker(int id, Ticker ticker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ticker.Id)
            {
                return BadRequest();
            }

            db.Entry(ticker).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TickerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/Ticker
        [ResponseType(typeof(Ticker))]
        public async Task<IHttpActionResult> PostTicker(Ticker ticker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Tickers.Add(ticker);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = ticker.Id }, ticker);
        }

        // DELETE api/Ticker/5
        [ResponseType(typeof(Ticker))]
        public async Task<IHttpActionResult> DeleteTicker(int id)
        {
            Ticker ticker = await db.Tickers.FindAsync(id);
            if (ticker == null)
            {
                return NotFound();
            }

            db.Tickers.Remove(ticker);
            await db.SaveChangesAsync();

            return Ok(ticker);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TickerExists(int id)
        {
            return db.Tickers.Count(e => e.Id == id) > 0;
        }
    }
}