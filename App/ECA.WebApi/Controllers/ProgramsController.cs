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

namespace ECA.WebApi
{
    public class ProgramsController : ApiController
    {
        private EcaContext db = new EcaContext();

        // GET: api/Programs
        public IQueryable<Program> GetPrograms()
        {
            return db.Programs;
        }

        // GET: api/ProgramsList
        [Route("api/programslist")]
        public IEnumerable<Models.ProgramList> GetProgramsList()
        {
            //create and return a ProgramList object which contains a custom list of elements belonging to programs
            var list = db.Programs.Select(s => new Models.ProgramList() {ProgramId = s.ProgramId, Name = s.Name, Description = s.Description }).OrderBy(s => s.Name);
            return list;
        }

        // GET: api/Programs/5
        [ResponseType(typeof(ProgramDTO))]
        public async Task<IHttpActionResult> GetProgram(int id)
        {
            Program program = await db.Programs.Include(p => p.Themes).Include(p => p.Owner).Include(p =>p.Regions).SingleOrDefaultAsync(i => i.ProgramId == id);
            if (program == null)
            {
                return NotFound();
            }

            return Ok(GetProgramDTO(program));
        }
        private ProgramDTO GetProgramDTO(Program program)
        {
            var programDTO = Mapper.Map<Program, ProgramDTO>(program);
            return programDTO;
        }
        // PUT: api/Programs/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProgram(int id, Program program)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != program.ProgramId)
            {
                return BadRequest();
            }

            db.Entry(program).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgramExists(id))
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

        // POST: api/Programs
        [ResponseType(typeof(Program))]
        public async Task<IHttpActionResult> PostProgram(Program program)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Programs.Add(program);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = program.ProgramId }, program);
        }

        // DELETE: api/Programs/5
        [ResponseType(typeof(Program))]
        public async Task<IHttpActionResult> DeleteProgram(int id)
        {
            Program program = await db.Programs.FindAsync(id);
            if (program == null)
            {
                return NotFound();
            }

            db.Programs.Remove(program);
            await db.SaveChangesAsync();

            return Ok(program);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProgramExists(int id)
        {
            return db.Programs.Count(e => e.ProgramId == id) > 0;
        }
    }
}