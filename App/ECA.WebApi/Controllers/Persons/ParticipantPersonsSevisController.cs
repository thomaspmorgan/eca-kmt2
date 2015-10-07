﻿using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// The ParticipantPersonsSevisController handles crud operations on ECA participants that are persons and their SEVIS info
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class ParticipantPersonsSevisController : ApiController
    {
        /// <summary>
        /// The default sorter for a list of participants.
        /// </summary>
        private static readonly ExpressionSorter<ParticipantPersonSevisDTO> DEFAULT_SORTER = new ExpressionSorter<ParticipantPersonSevisDTO>(x => x.ParticipantId, SortDirection.Ascending);

        private IParticipantPersonSevisService service;

        /// <summary>
        /// Creates a new ParticipantPersonsSevisController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        public ParticipantPersonsSevisController(IParticipantPersonSevisService service)
        {
            Contract.Requires(service != null, "The participantPersonSevis service must not be null.");
            this.service = service;
        }

        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participantPersonSevises.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <returns>The list of participantPersonSevises.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantPersonSevisDTO>))]
        [Route("ParticipantPersonsSevis")]
        public async Task<IHttpActionResult> GetParticipantPersonsAsync([FromUri]PagingQueryBindingModel<ParticipantPersonSevisDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantPersonSevisesAsync(queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participants and there SEVIS info by project id.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <param name="projectId">The id of the project to get participants for.</param>
        /// <returns>The list of participantPersonSevises.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantPersonSevisDTO>))]
        [Route("Projects/{projectId:int}/ParticipantPersonSevises")]
        public async Task<IHttpActionResult> GetParticipantPersonSevisesByProjectIdAsync(int projectId, [FromUri]PagingQueryBindingModel<ParticipantPersonSevisDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantPersonSevisesByProjectIdAsync(projectId, queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Retrieves the participantPersonSevis with the given id.
        /// </summary>
        /// <param name="participantId">The id of the participant.</param>
        /// <returns>The participantPersonSevis with the given id.</returns>
        [ResponseType(typeof(ParticipantPersonSevisDTO))]
        [Route("ParticipantPersonsSevis/{participantId:int}")]
        public async Task<IHttpActionResult> GetParticipantPersonByIdAsync(int participantId) 
        {
            var participantPerson = await service.GetParticipantPersonSevisByIdAsync(participantId);
            if (participantPerson != null)
            {
                return Ok(participantPerson);
            }
            else
            {
                return NotFound();
            }
        }



        /// <summary>
        /// Retrieves a listing of the paged, sorted, and filtered list of participant's SEVIS comm statuses.
        /// </summary>
        /// <param name="queryModel">The paging, filtering, and sorting model.</param>
        /// <param name="participantId">The id of the project to get participants for.</param>
        /// <returns>The list of participantPerson Sevis Comm Statuses.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantPersonSevisCommStatusDTO>))]
        [Route("ParticipantPersonSevis/{participantId:int}/SevisCommStatuses")]
        public async Task<IHttpActionResult> GetParticipantPersonSevisCommStatusesByProjectIdAsync(int participantId, [FromUri]PagingQueryBindingModel<ParticipantPersonSevisCommStatusDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetParticipantPersonSevisCommStatusesByIdAsync(participantId, queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
