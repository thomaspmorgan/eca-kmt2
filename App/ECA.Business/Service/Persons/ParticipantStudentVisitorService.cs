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
    /// A ParticipantStudentVisitorService is capable of performing crud operations on participant student visitors in the ECA system.
    /// </summary>
    public class ParticipantStudentVisitorService : DbContextService<EcaContext>, IParticipantStudentVisitorService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Creates a new ParticipantStudentVisitorService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ParticipantStudentVisitorService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
        }

        #region Get

        /// <summary>
        /// Returns the participantStudentVisitors in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantStudentVisitors.</returns>
        public PagedQueryResults<ParticipantStudentVisitorDTO> GetParticipantStudentVisitors(QueryableOperator<ParticipantStudentVisitorDTO> queryOperator)
        {
            var participantStudentVisitors = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorsDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantStudentVisitors with query operator [{0}].", queryOperator);
            return participantStudentVisitors;
        }

        /// <summary>
        /// Returns the participantStudentVisitors in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantStudentVisitors.</returns>
        public Task<PagedQueryResults<ParticipantStudentVisitorDTO>> GetParticipantStudentVisitorsAsync(QueryableOperator<ParticipantStudentVisitorDTO> queryOperator)
        {
            var participantStudentVisitors = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorsDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantStudentVisitors with query operator [{0}].", queryOperator);
            return participantStudentVisitors;
        }

        /// <summary>
        /// Returns the participantStudentVisitors for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantStudentVisitors.</returns>
        public PagedQueryResults<ParticipantStudentVisitorDTO> GetParticipantStudentVisitorsByProjectId(int projectId, QueryableOperator<ParticipantStudentVisitorDTO> queryOperator)
        {
            var participantStudentVisitors = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantStudentVisitors by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantStudentVisitors;
        }

        /// <summary>
        /// Returns the participantStudentVisitors for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantStudentVisitors.</returns>
        public Task<PagedQueryResults<ParticipantStudentVisitorDTO>> GetParticipantStudentVisitorsByProjectIdAsync(int projectId, QueryableOperator<ParticipantStudentVisitorDTO> queryOperator)
        {
            var participantStudentVisitors = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantStudentVisitors by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantStudentVisitors;
        }

        /// <summary>
        /// Returns a participantStudentVisitor
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantStudentVisitor</returns>
        public ParticipantStudentVisitorDTO GetParticipantStudentVisitorById(int participantId)
        {
            var participantStudentVisitor = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participantStudentVisitor by id [{0}].", participantId);
            return participantStudentVisitor;
        }

        /// <summary>
        /// Returns a participantStudentVisitor asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantStudentVisitor</returns>
        public Task<ParticipantStudentVisitorDTO> GetParticipantStudentVisitorByIdAsync(int participantId)
        {
            var participantStudentVisitor = ParticipantStudentVisitorQueries.CreateGetParticipantStudentVisitorDTOByIdQuery(this.Context, participantId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved participantStudentVisitor by id [{0}].", participantId);
            return participantStudentVisitor;
        }
        #endregion

    }
}
