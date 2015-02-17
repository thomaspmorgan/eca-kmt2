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
    public class ProjectsController : ApiController
    {
        private EcaContext db = new EcaContext();

        // GET: api/Projects
        public IEnumerable<ProjectDTO> GetProjects()
        {
            var projects = db.Projects;
            var projectDTOs = Mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDTO>>(projects);
            return projectDTOs;
        }
        // GET: api/Programs/3/Projects
        [Route("api/Programs/{id:int}/Projects")]
        public IEnumerable<ProjectDTO> GetProjectsByProgram(int id)
        {
            var projects = db.Projects.Include("ParentProgram").Include("Regions").Include("Status").Where(s => s.ParentProgram.ProgramId == id);
            var projectDTOs = Mapper.Map<IEnumerable<Project>, IEnumerable<ProjectDTO>>(projects);
            return projectDTOs;
        }
        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> GetProject(int id)
        {
            Project project = await db.Projects.Include(p => p.ParentProgram).Include(p => p.Regions).Include(p => p.Goals).Include(p => p.Themes).Include(p => p.ParentProgram.Owner).SingleOrDefaultAsync(p => p.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            var projectDTO = Mapper.Map<Project, ProjectDTO>(project);
            return Ok(projectDTO);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProject(int id, Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != project.ProjectId)
            {
                return BadRequest();
            }

            db.Entry(project).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
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

        // POST: api/Projects
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> PostProject(ProjectDTO projectDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var now = DateTimeOffset.Now;

            var historyDTO = new HistoryDTO();
            historyDTO.CreatedBy = 0;
            historyDTO.CreatedOn = now;
            historyDTO.RevisedBy = 0;
            historyDTO.RevisedOn = now;
            projectDTO.History = historyDTO;

            var project = Mapper.Map<ProjectDTO, Project>(projectDTO);
            project.ProjectStatusId = (await db.ProjectStatuses.FirstOrDefaultAsync(p => p.Status == "Draft")).ProjectStatusId;
            db.Projects.Add(project);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = project.ProjectId }, project);
        }

        // DELETE: api/Projects/5
        [ResponseType(typeof(Project))]
        public async Task<IHttpActionResult> DeleteProject(int id)
        {
            Project project = await db.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            db.Projects.Remove(project);
            await db.SaveChangesAsync();

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(int id)
        {
            return db.Projects.Count(e => e.ProjectId == id) > 0;
        }
    }
}