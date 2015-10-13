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
    public class ParticipantPersonService : DbContextService<EcaContext>, ECA.Business.Service.Persons.IParticipantPersonService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new ParticipantPersonService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        /// <param name="logger">The logger.</param>
        public ParticipantPersonService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns the participantPersons in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersons.</returns>
        public PagedQueryResults<SimpleParticipantPersonDTO> GetParticipantPersons(QueryableOperator<SimpleParticipantPersonDTO> queryOperator)
        {
            var participantPersons = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons with query operator [{0}].", queryOperator);
            return participantPersons;
        }

        /// <summary>
        /// Returns the participantPersons in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantPersons.</returns>
        public Task<PagedQueryResults<SimpleParticipantPersonDTO>> GetParticipantPersonsAsync(QueryableOperator<SimpleParticipantPersonDTO> queryOperator)
        {
            var participantPersons = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons with query operator [{0}].", queryOperator);
            return participantPersons;
        }

        /// <summary>
        /// Returns the participantPersons for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersons.</returns>
        public PagedQueryResults<SimpleParticipantPersonDTO> GetParticipantPersonsByProjectId(int projectId, QueryableOperator<SimpleParticipantPersonDTO> queryOperator)
        {
            var participantPersons = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantPersons;
        }

        /// <summary>
        /// Returns the participantPersons for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantPersons.</returns>
        public Task<PagedQueryResults<SimpleParticipantPersonDTO>> GetParticipantPersonsByProjectIdAsync(int projectId, QueryableOperator<SimpleParticipantPersonDTO> queryOperator)
        {
            var participantPersons = ParticipantPersonQueries.CreateGetSimpleParticipantPersonsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantPersons by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantPersons;
        }

        /// <summary>
        /// Returns a participant Person
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantPerson</returns>
        public SimpleParticipantPersonDTO GetParticipantPersonById(int participantId)
        {
            var participantPerson = ParticipantPersonQueries.CreateGetParticipantPersonDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participantPerson by id [{0}].", participantId);
            return participantPerson;
        }

        /// <summary>
        /// Returns a participantPerson asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantPerson</returns>
        public Task<SimpleParticipantPersonDTO> GetParticipantPersonByIdAsync(int participantId)
        {
            var participant = ParticipantPersonQueries.CreateGetParticipantPersonDTOByIdQuery(this.Context, participantId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved participantPerson by id [{0}].", participantId);
            return participant;
        }
        #endregion

    }
}
