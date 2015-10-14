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
    /// A ParticipantService is capable of performing crud operations on participants in the ECA system.
    /// </summary>
    public class ParticipantService : DbContextService<EcaContext>, ECA.Business.Service.Persons.IParticipantService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new ParticipantService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ParticipantService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns the participants in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants.</returns>
        public PagedQueryResults<SimpleParticipantDTO> GetParticipants(QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            var participants = ParticipantQueries.CreateGetSimpleParticipantsDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participants with query operator [{0}].", queryOperator);
            return participants;
        }

        /// <summary>
        /// Returns the participants in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants.</returns>
        public Task<PagedQueryResults<SimpleParticipantDTO>> GetParticipantsAsync(QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            var participants = ParticipantQueries.CreateGetSimpleParticipantsDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participants with query operator [{0}].", queryOperator);
            return participants;
        }

        /// <summary>
        /// Returns the participants for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participants.</returns>
        public PagedQueryResults<SimpleParticipantDTO> GetParticipantsByProjectId(int projectId, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            var participants = ParticipantQueries.CreateGetSimpleParticipantsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participants by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participants;
        }

        /// <summary>
        /// Returns the participants for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participants.</returns>
        public Task<PagedQueryResults<SimpleParticipantDTO>> GetParticipantsByProjectIdAsync(int projectId, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            var participants = ParticipantQueries.CreateGetSimpleParticipantsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participants by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participants;
        }

        /// <summary>
        /// Returns a participant 
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participant</returns>
        public ParticipantDTO GetParticipantById(int participantId)
        {
            var participant = ParticipantQueries.CreateGetParticipantDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participant by id [{0}].", participantId);
            return participant;
        }

        /// <summary>
        /// Returns a participant asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participant</returns>
        public Task<ParticipantDTO> GetParticipantByIdAsync(int participantId)
        {
            var participant = ParticipantQueries.CreateGetParticipantDTOByIdQuery(this.Context, participantId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved participant by id [{0}].", participantId);
            return participant;
        }
        #endregion

    }
}
