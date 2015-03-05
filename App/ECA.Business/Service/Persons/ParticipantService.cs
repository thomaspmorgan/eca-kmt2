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
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Persons
{

    /// <summary>
    /// A ParticipantService is capable of performing crud operations on participants in the ECA system.
    /// </summary>
    public class ParticipantService : DbContextService<EcaContext>, ECA.Business.Service.Persons.IParticipantService
    {       
        /// <summary>
        /// Creates a new ParticipantService with the given context to operate against.
        /// </summary>
        /// <param name="context">The context to operate against.</param>
        public ParticipantService(EcaContext context, ILogger logger) : base(context, logger)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(logger != null, "The logger must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns the participants in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants.</returns>
        public PagedQueryResults<SimpleParticipantDTO> GetParticipants(QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            return ParticipantQueries.CreateGetSimpleParticipantsDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the participants in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participants.</returns>
        public Task<PagedQueryResults<SimpleParticipantDTO>> GetParticipantsAsync(QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            return ParticipantQueries.CreateGetSimpleParticipantsDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the participants for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participants.</returns>
        public PagedQueryResults<SimpleParticipantDTO> GetParticipantsByProjectId(int projectId, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            return ParticipantQueries.CreateGetSimpleParticipantsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
        }

        /// <summary>
        /// Returns the participants for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participants.</returns>
        public Task<PagedQueryResults<SimpleParticipantDTO>> GetParticipantsByProjectIdAsync(int projectId, QueryableOperator<SimpleParticipantDTO> queryOperator)
        {
            return ParticipantQueries.CreateGetSimpleParticipantsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
        }
        #endregion

    }
}
