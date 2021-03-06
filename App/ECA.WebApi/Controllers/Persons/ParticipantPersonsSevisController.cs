﻿using CAM.Data;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Models.Sevis;
using ECA.Business.Service.Persons;
using ECA.Business.Validation.Sevis;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.Core.Settings;
using ECA.WebApi.Custom.Storage;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using System;
using System.Diagnostics.Contracts;
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

        /// <summary>
        /// The default sorter for a participant's sevis comm statuses.
        /// </summary>
        private static readonly ExpressionSorter<ParticipantPersonSevisCommStatusDTO> DEFAULT_SEVIS_COMM_STATUS_SORTER = new ExpressionSorter<ParticipantPersonSevisCommStatusDTO>(x => x.AddedOn, SortDirection.Descending);

        private IParticipantPersonsSevisService participantService;
        private IUserProvider userProvider;
        private IFileStorageHandler storageHandler;
        private AppSettings appSettings;

        /// <summary>
        /// Creates a new ParticipantPersonsSevisController with the given service.
        /// </summary>
        /// <param name="participantService">The participant person sevis service.</param>
        /// <param name="userProvider">The user provider</param>
        /// <param name="appSettings">The application settings.</param>
        /// <param name="storageHandler">The file storage handler.</param>
        public ParticipantPersonsSevisController(IParticipantPersonsSevisService participantService, IUserProvider userProvider, IFileStorageHandler storageHandler, AppSettings appSettings)
        {
            Contract.Requires(participantService != null, "The participantPersonSevis service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            Contract.Requires(storageHandler != null, "The storage handler must not be null.");
            this.participantService = participantService;
            this.userProvider = userProvider;
            this.storageHandler = storageHandler;
            this.appSettings = appSettings;
        }

        /// <summary>
        /// Gets list of sevis participants
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="queryModel">The query model</param>
        /// <returns>List of sevis participants</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantPersonSevisDTO>))]
        [Route("Project/{projectId:int}/SevisParticipants")]
        public async Task<IHttpActionResult> GetSevisParticipantsByProjectIdAsync(int projectId, [FromUri]PagingQueryBindingModel<ParticipantPersonSevisDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await participantService.GetSevisParticipantsByProjectIdAsync(projectId, queryModel.ToQueryableOperator(DEFAULT_SORTER, x => x.FullName, x => x.SevisStatus, x => x.SevisId));
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
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersonSevis with the given id.</returns>
        [ResponseType(typeof(ParticipantPersonSevisDTO))]
        [Route("Project/{projectId:int}/ParticipantPersonsSevis/{participantId:int}")]
        public async Task<IHttpActionResult> GetParticipantPersonsSevisByIdAsync(int projectId, int participantId)
        {
            var participantPerson = await participantService.GetParticipantPersonsSevisByIdAsync(projectId, participantId);
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
        /// Retrieves the participantPersonSevis comm statuses with the given id.
        /// </summary>
        /// <param name="participantId">The id of the participant.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="model">The query operator binding model.</param>
        /// <returns>The participantPersonSevis with the given id.</returns>
        [ResponseType(typeof(PagedQueryResults<ParticipantPersonSevisCommStatusDTO>))]
        [Route("Project/{projectId:int}/ParticipantPersonsSevis/{participantId:int}/CommStatuses")]
        public async Task<IHttpActionResult> GetParticipantPersonsSevisCommStatusesByIdAsync(int projectId, int participantId, [FromUri]PagingQueryBindingModel<ParticipantPersonSevisCommStatusDTO> model)
        {
            if (ModelState.IsValid)
            {
                var statuses = await participantService.GetSevisCommStatusesByParticipantIdAsync(projectId, participantId, model.ToQueryableOperator(DEFAULT_SEVIS_COMM_STATUS_SORTER));
                return Ok(statuses);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Retrieves the participantPersonSevis comm statuses with the given id.
        /// </summary>
        /// <param name="participantId">The id of the participant.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <param name="batchId">The BatchId to process.</param>
        /// <returns>The participantPersonSevis with the given id.</returns>
        [ResponseType(typeof(SevisBatchInfoDTO))]
        [Route("Project/{projectId:int}/ParticipantPersonsSevis/{participantId:int}/Batch/{batchId}")]
        public async Task<IHttpActionResult> GetSevisBatchProcessingInfoAsync(int projectId, int participantId, string batchId)
        {
            var currentUser = this.userProvider.GetCurrentUser();
            var businessUser = this.userProvider.GetBusinessUser(currentUser);
            var info = await participantService.GetBatchInfoByBatchIdAsync(businessUser.Id, projectId, participantId, batchId);
            return Ok(info);
        }

        /// <summary>
        /// Updates the new participantPersonSevis with the given participantId.
        /// </summary>
        /// <param name="model">The new email address.</param>
        /// <param name="projectId">The project id of the participant.</param>
        /// <returns>The saved email address.</returns>
        [HttpPut]
        [Route("Project/{projectId:int}/ParticipantPersonsSevis")]
        [ResourceAuthorize(Permission.EDIT_SEVIS_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        [ResponseType(typeof(ParticipantPersonSevisDTO))]
        public async Task<IHttpActionResult> PutParticipantPersonsSevisAsync(int projectId, [FromBody]UpdatedParticipantPersonSevisBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await participantService.UpdateAsync(model.ToUpdatedParticipantPersonSevis(businessUser));
                await participantService.SaveChangesAsync();
                var dto = await participantService.GetParticipantPersonsSevisByIdAsync(projectId, model.ParticipantId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Send to sevis async
        /// </summary>
        /// <param name="model">The participants to send to sevis.</param>
        /// <param name="projectId">The project id of the participants.</param>
        /// <param name="applicationId">The application id for which the participants belong.</param>
        /// <returns>Success or error</returns>
        [Route("Application/{applicationId:int}/Project/{projectId:int}/ParticipantPersonsSevis/SendToSevis")]
        [ResourceAuthorize(Permission.SEND_TO_SEVIS_VALUE, ResourceType.APPLICATION_VALUE, "applicationId")]
        [ResourceAuthorize(Permission.EDIT_SEVIS_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        public async Task<IHttpActionResult> PostSendToSevisAsync(int applicationId, int projectId, SendToSevisBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var hasSevisCredentials = await userProvider.HasSevisUserAccountAsync(currentUser, model.SevisUsername, model.SevisOrgId);
                if (!hasSevisCredentials)
                {
                    throw new HttpResponseException(System.Net.HttpStatusCode.Forbidden);
                }
                var businessModel = model.ToParticipantsToBeSentToSevis(businessUser, projectId);
                var statuses = await participantService.SendToSevisAsync(businessModel);
                await participantService.SaveChangesAsync();
                return Ok(statuses);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Gets the DS2019 file asyncronously
        /// </summary>
        /// <param name="projectId">The project id</param>
        /// <param name="participantId">The participant id</param>
        /// <returns>DS2019 file</returns>
        [Route("Project/{projectId:int}/ParticipantPersonSevis/{participantId:int}/DS2019File")]
        [ResourceAuthorize(Permission.EDIT_SEVIS_VALUE, ResourceType.PROJECT_VALUE, "projectId")]
        public async Task<HttpResponseMessage> GetDS2019FileAsync(int projectId, int participantId)
        {
            var currentUser = userProvider.GetCurrentUser();
            var businessUser = userProvider.GetBusinessUser(currentUser);
            var fileName = await participantService.GetDS2019FileNameAsync(businessUser, projectId, participantId);
            if (fileName != null)
            {
                var container = appSettings.DS2019FileStorageContainer;
                var message = await storageHandler.GetFileAsync(fileName, container);
                if (message != null)
                {
                    return message;
                }
            } 

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "File not found.");
        }
    }
}
