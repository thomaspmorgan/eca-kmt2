using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Logging;
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

namespace ECA.Business.Service.Persons
{

    /// <summary>
    /// A ParticipantService is capable of performing crud operations on participants in the ECA system.
    /// </summary>
    public class ParticipantService : DbContextService<EcaContext>, ECA.Business.Service.Persons.IParticipantService
    {
        private static readonly string COMPONENT_NAME = typeof(ParticipantService).FullName;
        private readonly ILogger logger;

        /// <summary>
        /// Creates a new ParticipantService with the given context to operate against.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        /// <param name="logger">The logger.</param>
        public ParticipantService(EcaContext context, ILogger logger) : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
            this.logger = logger;
        }

        #region Get

        /// <summary>
        /// Returns the participants in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants.</returns>
        public PagedQueryResults<SimpleParticipantDTO> GetParticipants(QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            var stopwatch = Stopwatch.StartNew();
            var participants = ParticipantQueries.CreateGetSimpleParticipantsDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "queryOperator", queryOperator } });
            return participants;
        }

        /// <summary>
        /// Returns the participants in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants.</returns>
        public Task<PagedQueryResults<SimpleParticipantDTO>> GetParticipantsAsync(QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            var stopwatch = Stopwatch.StartNew();
            var participants = ParticipantQueries.CreateGetSimpleParticipantsDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "queryOperator", queryOperator } });
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
            var stopwatch = Stopwatch.StartNew();
            var participants = ParticipantQueries.CreateGetSimpleParticipantsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "queryOperator", queryOperator } });
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
            var stopwatch = Stopwatch.StartNew();
            var participants = ParticipantQueries.CreateGetSimpleParticipantsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "queryOperator", queryOperator } });
            return participants;
        }

        /// <summary>
        /// Returns a participant 
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participant</returns>
        public SimpleParticipantDTO GetParticipantById(int participantId)
        {
            var stopwatch = Stopwatch.StartNew();
            var participant = ParticipantQueries.CreateGetSimpleParticipantDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "participantId", participantId } });
            return participant;
        }

        /// <summary>
        /// Returns a participant asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participant</returns>
        public Task<SimpleParticipantDTO> GetParticipantByIdAsync(int participantId)
        {
            var stopwatch = Stopwatch.StartNew();
            var participant = ParticipantQueries.CreateGetSimpleParticipantDTOByIdQuery(this.Context, participantId).FirstOrDefaultAsync();
            stopwatch.Stop();
            this.logger.TraceApi(COMPONENT_NAME, stopwatch.Elapsed, new Dictionary<string, object> { { "participantId", participantId } });
            return participant;
        }
        #endregion

    }
}
