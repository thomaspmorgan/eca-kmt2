using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NLog;

namespace ECA.Business.Service.Persons
{

    /// <summary>
    /// A ParticipantPersonService is capable of performing crud operations on participantPersons in the ECA system.
    /// </summary>
    public class ParticipantPersonSevisService : DbContextService<EcaContext>, IParticipantPersonSevisService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new ParticipantPersonService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ParticipantPersonSevisService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        public PagedQueryResults<ParticipantPersonSevisDTO> GetParticipantPersonSevises(QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var participantPersonSevises = ParticipantPersonSevisQueries.CreateGetParticipantPersonSevisesDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons with query operator [{0}].", queryOperator);
            return participantPersonSevises;
        }

        /// <summary>
        /// Returns the participantPersonSevises in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisDTO>> GetParticipantPersonSevisesAsync(QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var participantPersonSevises = ParticipantPersonSevisQueries.CreateGetParticipantPersonSevisesDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons with query operator [{0}].", queryOperator);
            return participantPersonSevises;
        }

        /// <summary>
        /// Returns the participantPersonSevises for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersonSevises.</returns>
        public PagedQueryResults<ParticipantPersonSevisDTO> GetParticipantPersonSevisesByProjectId(int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var participantPersonSevises = ParticipantPersonSevisQueries.CreateGetParticipantPersonSevisesDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantPersonSevises;
        }

        /// <summary>
        /// Returns the participantPersonSevises for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersonSevises.</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisDTO>> GetParticipantPersonSevisesByProjectIdAsync(int projectId, QueryableOperator<ParticipantPersonSevisDTO> queryOperator)
        {
            var participantPersonSevises = ParticipantPersonSevisQueries.CreateGetParticipantPersonSevisesDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantPersonSevises;
        }

        /// <summary>
        /// Returns a participantPersonSevis
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public ParticipantPersonSevisDTO GetParticipantPersonSevisById(int participantId)
        {
            var participantPersonSevis = ParticipantPersonSevisQueries.CreateGetParticipantPersonSevisDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevis;
        }

        /// <summary>
        /// Returns a participantPersonSevis asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public Task<ParticipantPersonSevisDTO> GetParticipantPersonSevisByIdAsync(int participantId)
        {
            var participantPersonSevis = ParticipantPersonSevisQueries.CreateGetParticipantPersonSevisDTOByIdQuery(this.Context, participantId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevis;
        }
        #endregion

        #region ParticipantPersonSevisStatus


        /// Sevis Comm Status

        /// <summary>
        /// Returns the participantPersonSevisCommStatus in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        public PagedQueryResults<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonSevisCommStatuses(QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            var participantPersonSevisCommStatuses = ParticipantPersonSevisCommStatusQueries.CreateGetParticipantPersonSevisCommStatusDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersonSevisCommStatuses with query operator [{0}].", queryOperator);
            return participantPersonSevisCommStatuses;
        }

        /// <summary>
        /// Returns the participantPersonSevisCommStatus in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersonSevises.</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> GetParticipantPersonSevisCommStatusesAsync(QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            var participantPersonSevisCommStatuses = ParticipantPersonSevisCommStatusQueries.CreateGetParticipantPersonSevisCommStatusDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersonSevisCommStatuses with query operator [{0}].", queryOperator);
            return participantPersonSevisCommStatuses;
        }

        /// <summary>
        /// Returns a participantPersonSevis
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public PagedQueryResults<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonSevisCommStatusesById(int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            var participantPersonSevisCommStatuses = ParticipantPersonSevisCommStatusQueries.CreateGetParticipantPersonSevisCommStatusDTOByIdQuery(this.Context, participantId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevisCommStatuses;
        }

        /// <summary>
        /// Returns a participantPersonSevisCommStatus asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPersonSevis</returns>
        public Task<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>> GetParticipantPersonSevisCommStatusesByIdAsync(int participantId, QueryableOperator<ParticipantPersonSevisCommStatusDTO> queryOperator)
        {
            var participantPersonSevisCommStatuses = ParticipantPersonSevisCommStatusQueries.CreateGetParticipantPersonSevisCommStatusDTOByIdQuery(this.Context, participantId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersonSevis by id [{0}].", participantId);
            return participantPersonSevisCommStatuses;
        }


        public IQueryable<ParticipantPersonSevisCommStatusDTO> GetParticipantPersonSevisCommStatusesByParticipantIds(int[] participantIds)
        {
            var results = ParticipantPersonSevisCommStatusQueries.CreateParticipantPersonSevisCommStatusesDTOsByParticipantIdsQuery(Context, participantIds);
            logger.Trace("Retrieved participantPersonSevises by array of participant Ids");
            return results;
        }

        /// <summary>
        /// Sets sevis communication status for participant ids
        /// </summary>
        /// <param name="participantIds">The participant ids to update communcation status</param>
        /// <returns>List of participant ids that were updated</returns>
        public async Task<int[]> SendToSevis(int[] participantIds)
        {
            foreach(var participantId in participantIds)
            {
                var status = new ParticipantPersonSevisCommStatus
                {
                    ParticipantId = participantId,
                    SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
                    AddedOn = DateTimeOffset.Now

                };

                Context.ParticipantPersonSevisCommStatuses.Add(status);
            }
            return participantIds;
        }
            
        #endregion

    }
}
