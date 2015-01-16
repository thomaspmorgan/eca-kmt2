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

namespace ECA.WebApi.Controllers
{
    public class SpecialStatusController : ApiController
    {
        private EcaContext db = new EcaContext();

        // GET: api/SpecialStatus
        public IQueryable<SpecialStatus> GetSpecialStatus()
        {
            return db.SpecialStatus;
        }

        // GET: api/SpecialStatus/5
        [ResponseType(typeof(SpecialStatus))]
        public async Task<IHttpActionResult> GetSpecialStatus(int id)
        {
            SpecialStatus specialStatus = await db.SpecialStatus.FindAsync(id);
            if (specialStatus == null)
            {
                return NotFound();
            }

            return Ok(specialStatus);
        }

        // PUT: api/SpecialStatus/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSpecialStatus(int id, SpecialStatus specialStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != specialStatus.SpecialStatusId)
            {
                return BadRequest();
            }

            db.Entry(specialStatus).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SpecialStatusExists(id))
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

        // POST: api/SpecialStatus
        [ResponseType(typeof(SpecialStatus))]
        public async Task<IHttpActionResult> PostSpecialStatus(SpecialStatus specialStatus)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SpecialStatus.Add(specialStatus);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = specialStatus.SpecialStatusId }, specialStatus);
        }

        // DELETE: api/SpecialStatus/5
        [ResponseType(typeof(SpecialStatus))]
        public async Task<IHttpActionResult> DeleteSpecialStatus(int id)
        {
            SpecialStatus specialStatus = await db.SpecialStatus.FindAsync(id);
            if (specialStatus == null)
            {
                return NotFound();
            }

            db.SpecialStatus.Remove(specialStatus);
            await db.SaveChangesAsync();

            return Ok(specialStatus);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SpecialStatusExists(int id)
        {
            return db.SpecialStatus.Count(e => e.SpecialStatusId == id) > 0;
        }
    }
}