using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CitiLightningDatabase;

namespace CitiLightning.Controllers
{
    public class StrategyController : ApiController
    {
        private NorthwindEntities db = new NorthwindEntities();
        private CitiLightning_BusinessLogic.CLDB BL = new CitiLightning_BusinessLogic.CLDB();
        // GET api/Strategy
        public IQueryable<Strategy> GetStrategies()
        {
            return db.Strategies;
        }

        // GET api/Strategy/5
        [ResponseType(typeof(Strategy))]
        public List<Strategy> GetStrategy(int id)
        {
            List<Strategy> strategy = BL.GetStrategies(id);

            return strategy;
        }

        // PUT api/Strategy/5
        public IHttpActionResult PutStrategy(int id, [FromBody] Strategy strategy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //if (id != strategy.Id)
            //{
            //    return BadRequest();
            //}

            db.Entry(strategy).State = System.Data.Entity.EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StrategyExists(id))
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

        // POST api/Strategy
        [ResponseType(typeof(Strategy))]
        public IHttpActionResult PostStrategy(Strategy strategy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Strategies.Add(strategy);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = strategy.Id }, strategy);
        }

        // DELETE api/Strategy/5
        [ResponseType(typeof(Strategy))]
        public IHttpActionResult DeleteStrategy(int id)
        {
            Strategy strategy = db.Strategies.Find(id);
            if (strategy == null)
            {
                return NotFound();
            }

            db.Strategies.Remove(strategy);
            db.SaveChanges();

            return Ok(strategy);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StrategyExists(int id)
        {
            return db.Strategies.Count(e => e.Id == id) > 0;
        }
    }
}