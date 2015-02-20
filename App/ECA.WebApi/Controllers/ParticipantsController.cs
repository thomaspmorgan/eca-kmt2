﻿using System;
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
    public class ParticipantsController : ApiController
    {
        private EcaContext db = new EcaContext();

        // GET: api/Participants
        public IQueryable<Participant> GetParticipants()
        {
            return db.Participants;
        }

        // GET: api/Participants/5
        [ResponseType(typeof(Participant))]
        public async Task<IHttpActionResult> GetParticipant(int id)
        {
            Participant participant = await db.Participants.FindAsync(id);
            if (participant == null)
            {
                return NotFound();
            }

            return Ok(participant);
        }

        // GET: api/Projects/5/Participants
        [Route("api/Projects/{id:int}/Participants")]
        public IEnumerable<ParticipantDTO> GetParticipantsByProject(int id) 
        {
            var participants = db.Participants
                .Include("Organization")
                .Include("Person")
                .Where(participant => participant.Projects.Any(project => project.ProjectId == id));
            var participantDTOs = Mapper.Map<IEnumerable<Participant>, IEnumerable<ParticipantDTO>>(participants); 
            return participantDTOs;
        }

        // PUT: api/Participants/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutParticipant(int id, Participant participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != participant.ParticipantId)
            {
                return BadRequest();
            }

            db.Entry(participant).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParticipantExists(id))
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

        // POST: api/Participants
        [ResponseType(typeof(Participant))]
        public async Task<IHttpActionResult> PostParticipant(Participant participant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Participants.Add(participant);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = participant.ParticipantId }, participant);
        }

        // DELETE: api/Participants/5
        [ResponseType(typeof(Participant))]
        public async Task<IHttpActionResult> DeleteParticipant(int id)
        {
            Participant participant = await db.Participants.FindAsync(id);
            if (participant == null)
            {
                return NotFound();
            }

            db.Participants.Remove(participant);
            await db.SaveChangesAsync();

            return Ok(participant);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ParticipantExists(int id)
        {
            return db.Participants.Count(e => e.ParticipantId == id) > 0;
        }
    }
}