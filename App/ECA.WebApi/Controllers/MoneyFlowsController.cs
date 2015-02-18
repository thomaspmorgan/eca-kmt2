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
using ECA.Data;
using ECA.WebApi.Models;
using AutoMapper;

namespace ECA.WebApi.Controllers
{
    public class MoneyFlowsController : ApiController
    {
        private EcaContext db = new EcaContext();

        // GET: api/MoneyFlows
        public IQueryable<MoneyFlow> GetMoneyFlows()
        {
            return db.MoneyFlows;
        }

        // GET: api/MoneyFlows/5
        [ResponseType(typeof(MoneyFlow))]
        public async Task<IHttpActionResult> GetMoneyFlow(int id)
        {
            MoneyFlow moneyFlow = await db.MoneyFlows.FindAsync(id);
            if (moneyFlow == null)
            {
                return NotFound();
            }

            return Ok(moneyFlow);
        }

        // GET: api/Projects/5/MoneyFlows
        [Route("api/Projects/{id:int}/MoneyFlows")]
        public IEnumerable<MoneyFlowDTO> GetMoneyFlowsByProject(int id)
        {
            var moneyOutFlows = db.MoneyFlows
                .Include(m => m.RecipientParticipant)
                .Include(m => m.RecipientAccommodation)
                .Include(m => m.RecipientTransportation)
                .Where(m => m.SourceProjectId == id);

            var moneyInFlows = db.MoneyFlows
                .Include(m => m.SourceProgram)
                .Where(m => m.RecipientProjectId == id);

            var moneyFlows = moneyInFlows.Union(moneyOutFlows);

            var moneyFlowDTOs = Mapper.Map<IEnumerable<MoneyFlow>, IEnumerable<MoneyFlowDTO>>(moneyFlows);

            return moneyFlowDTOs;
        }

        // PUT: api/MoneyFlows/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutMoneyFlow(int id, MoneyFlow moneyFlow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != moneyFlow.MoneyFlowId)
            {
                return BadRequest();
            }

            db.Entry(moneyFlow).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoneyFlowExists(id))
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

        // POST: api/MoneyFlows
        [ResponseType(typeof(MoneyFlow))]
        public async Task<IHttpActionResult> PostMoneyFlow(MoneyFlow moneyFlow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MoneyFlows.Add(moneyFlow);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = moneyFlow.MoneyFlowId }, moneyFlow);
        }

        // DELETE: api/MoneyFlows/5
        [ResponseType(typeof(MoneyFlow))]
        public async Task<IHttpActionResult> DeleteMoneyFlow(int id)
        {
            MoneyFlow moneyFlow = await db.MoneyFlows.FindAsync(id);
            if (moneyFlow == null)
            {
                return NotFound();
            }

            db.MoneyFlows.Remove(moneyFlow);
            await db.SaveChangesAsync();

            return Ok(moneyFlow);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MoneyFlowExists(int id)
        {
            return db.MoneyFlows.Count(e => e.MoneyFlowId == id) > 0;
        }
    }
}