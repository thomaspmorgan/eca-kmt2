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
using System.Threading.Tasks;
using NLog;
using ECA.Core.Exceptions;

namespace ECA.Business.Service.Persons
{

    /// <summary>
    /// A ParticipantExchangeVisitorService is capable of performing crud operations on participant exchange visitors in the ECA system.
    /// </summary>
    public class ParticipantExchangeVisitorService : DbContextService<EcaContext>, IParticipantExchangeVisitorService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Action<int, object, Type> throwIfModelDoesNotExist;

        /// <summary>
        /// Creates a new ParticipantExchangeVisitorService with the given context to operate against.
        /// </summary>
        /// <param name="saveActions">The save actions.</param>
        /// <param name="context">The context to operate against.</param>
        public ParticipantExchangeVisitorService(EcaContext context, List<ISaveAction> saveActions = null) : base(context, saveActions)
        {
            Contract.Requires(context != null, "The context must not be null.");
            throwIfModelDoesNotExist = (id, instance, type) =>
            {
                if (instance == null)
                {
                    throw new ModelNotFoundException(String.Format("The model of type [{0}] with id [{1}] was not found.", type.Name, id));
                }
            };
        }

        #region Get

        /// <summary>
        /// Returns the participantExchangeVisitors in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantExchangeVisitors.</returns>
        public PagedQueryResults<ParticipantExchangeVisitorDTO> GetParticipantExchangeVisitors(QueryableOperator<ParticipantExchangeVisitorDTO> queryOperator)
        {
            var participantExchangeVisitors = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorsDTOQuery(this.Context, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantExchangeVisitors with query operator [{0}].", queryOperator);
            return participantExchangeVisitors;
        }

        /// <summary>
        /// Returns the participantExchangeVisitors in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The participantExchangeVisitors.</returns>
        public Task<PagedQueryResults<ParticipantExchangeVisitorDTO>> GetParticipantExchangeVisitorsAsync(QueryableOperator<ParticipantExchangeVisitorDTO> queryOperator)
        {
            var participantExchangeVisitors = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorsDTOQuery(this.Context, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantExchangeVisitors with query operator [{0}].", queryOperator);
            return participantExchangeVisitors;
        }

        /// <summary>
        /// Returns the participantExchangeVisitors for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantExchangeVisitors.</returns>
        public PagedQueryResults<ParticipantExchangeVisitorDTO> GetParticipantExchangeVisitorsByProjectId(int projectId, QueryableOperator<ParticipantExchangeVisitorDTO> queryOperator)
        {
            var participantExchangeVisitors = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantExchangeVisitors by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantExchangeVisitors;
        }

        /// <summary>
        /// Returns the participantExchangeVisitors for the project with the given id in the system.
        /// </summary>
        /// <param name="queryOperator">The query operator.</param>
        /// <param name="projectId">The id of the project.</param>
        /// <returns>The participantExchangeVisitors.</returns>
        public Task<PagedQueryResults<ParticipantExchangeVisitorDTO>> GetParticipantExchangeVisitorsByProjectIdAsync(int projectId, QueryableOperator<ParticipantExchangeVisitorDTO> queryOperator)
        {
            var participantExchangeVisitors = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorsDTOByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved participantExchangeVisitors by project id [{0}] and query operator [{1}].", projectId, queryOperator);
            return participantExchangeVisitors;
        }

        /// <summary>
        /// Returns a participantExchangeVisitors
        /// </summary>
        /// <param name="participantId">The participantId to lookup</param>
        /// <returns>The participantExchangeVisitors</returns>
        public ParticipantExchangeVisitorDTO GetParticipantExchangeVisitorById(int participantId)
        {
            var participantExchangeVisitor = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorDTOByIdQuery(this.Context, participantId).FirstOrDefault();
            this.logger.Trace("Retrieved participantExchangeVisitors by id [{0}].", participantId);
            return participantExchangeVisitor;
        }

        /// <summary>
        /// Returns a participantExchangeVisitors asyncronously
        /// </summary>
        /// <param name="participantId">The participant id to lookup</param>
        /// <returns>The participantExchangeVisitors</returns>
        public Task<ParticipantExchangeVisitorDTO> GetParticipantExchangeVisitorByIdAsync(int participantId)
        {
            var participantExchangeVisitor = ParticipantExchangeVisitorQueries.CreateGetParticipantExchangeVisitorDTOByIdQuery(this.Context, participantId).FirstOrDefaultAsync();
            this.logger.Trace("Retrieved participantExchangeVisitor by id [{0}].", participantId);
            return participantExchangeVisitor;
        }
        #endregion

        #region update

        /// <summary>
        /// Updates a participant person exchange visitor  info with given updated exchange visitor  information.
        /// </summary>
        /// <param name="updatedParticipantStudentVistor">The updated participant person exchange visitor  info.</param>
        public ParticipantExchangeVisitorDTO Update(UpdatedParticipantExchangeVisitor updatedParticipantExchangeVisitor)
        {
            var participantExchangeVisitor = CreateGetParticipantExchangeVisitorByIdQuery(updatedParticipantExchangeVisitor.ParticipantId).FirstOrDefault();
            throwIfModelDoesNotExist(updatedParticipantExchangeVisitor.ParticipantId, participantExchangeVisitor, typeof(ParticipantStudentVisitor));

            DoUpdate(participantExchangeVisitor, updatedParticipantExchangeVisitor);
            return this.GetParticipantExchangeVisitorById(updatedParticipantExchangeVisitor.ParticipantId);
        }

        /// <summary>
        /// Updates a participant person student visitor info with given updated exchange visitor  information.
        /// </summary>
        /// <param name="updatedParticipantExchangeVisitor">The updated participant person exchange visitor  info.</param>
        /// <returns>The task.</returns>
        public async Task<ParticipantExchangeVisitorDTO> UpdateAsync(UpdatedParticipantExchangeVisitor updatedParticipantExchangeVisitor)
        {
            var participantExchangeVisitor = await CreateGetParticipantExchangeVisitorByIdQuery(updatedParticipantExchangeVisitor.ParticipantId).FirstOrDefaultAsync();
            throwIfModelDoesNotExist(updatedParticipantExchangeVisitor.ParticipantId, participantExchangeVisitor, typeof(ParticipantExchangeVisitor));

            DoUpdate(participantExchangeVisitor, updatedParticipantExchangeVisitor);

            return await this.GetParticipantExchangeVisitorByIdAsync(updatedParticipantExchangeVisitor.ParticipantId);
        }

        public async Task CreateParticipantExchangeVisitor(int participantId, User creator)
        {
            var participant = await Context.Participants.FindAsync(participantId);
            throwIfModelDoesNotExist(participantId, participant, typeof(Participant));

            var participantExchangeVisitor = await Context.ParticipantExchangeVisitors.FindAsync(participantId);
            if (participantExchangeVisitor == null)
            {
                DoCreateParticipantExchangeVistor(participantId, creator);
            }
        }

        private void DoUpdate(ParticipantExchangeVisitor participantExchangeVisitor, UpdatedParticipantExchangeVisitor updatedParticipantExchangeVisitor)
        {
            //participantPersonValidator.ValidateUpdate(GetUpdatedPersonParticipantValidationEntity(participantType));
            updatedParticipantExchangeVisitor.Audit.SetHistory(participantExchangeVisitor);

            participantExchangeVisitor.FieldOfStudyId = updatedParticipantExchangeVisitor.FieldOfStudyId;
            participantExchangeVisitor.PositionId = updatedParticipantExchangeVisitor.PositionId;
            participantExchangeVisitor.ProgramCategoryId = updatedParticipantExchangeVisitor.ProgramCategoryId;
            participantExchangeVisitor.FundingSponsor = updatedParticipantExchangeVisitor.FundingSponsor;
            participantExchangeVisitor.FundingPersonal = updatedParticipantExchangeVisitor.FundingPersonal;
            participantExchangeVisitor.FundingVisGovt = updatedParticipantExchangeVisitor.FundingVisGovt;
            participantExchangeVisitor.FundingVisBNC = updatedParticipantExchangeVisitor.FundingVisBNC;
            participantExchangeVisitor.FundingGovtAgency1 = updatedParticipantExchangeVisitor.FundingGovtAgency1;
            participantExchangeVisitor.GovtAgency1Id = updatedParticipantExchangeVisitor.GovtAgency1Id;
            participantExchangeVisitor.GovtAgency1OtherName = updatedParticipantExchangeVisitor.GovtAgency1OtherName;
            participantExchangeVisitor.FundingGovtAgency2 = updatedParticipantExchangeVisitor.FundingGovtAgency2;
            participantExchangeVisitor.GovtAgency2Id = updatedParticipantExchangeVisitor.GovtAgency2Id;
            participantExchangeVisitor.GovtAgency2OtherName = updatedParticipantExchangeVisitor.GovtAgency2OtherName;
            participantExchangeVisitor.FundingIntlOrg1 = updatedParticipantExchangeVisitor.FundingIntlOrg1;
            participantExchangeVisitor.IntlOrg1Id = updatedParticipantExchangeVisitor.IntlOrg1Id;
            participantExchangeVisitor.IntlOrg1OtherName = updatedParticipantExchangeVisitor.IntlOrg1OtherName;
            participantExchangeVisitor.FundingIntlOrg2 = updatedParticipantExchangeVisitor.FundingIntlOrg2;
            participantExchangeVisitor.IntlOrg2Id = updatedParticipantExchangeVisitor.IntlOrg2Id;
            participantExchangeVisitor.IntlOrg2OtherName = updatedParticipantExchangeVisitor.IntlOrg2OtherName;
            participantExchangeVisitor.FundingOther = updatedParticipantExchangeVisitor.FundingOther;
            participantExchangeVisitor.OtherName = updatedParticipantExchangeVisitor.OtherName;
            participantExchangeVisitor.FundingTotal = updatedParticipantExchangeVisitor.FundingTotal;
        }

        private void DoCreateParticipantExchangeVistor(int participantId, User creator)
        {
            var newParticipantExchangeVisitor = new NewParticipantExchangeVisitor(creator,participantId);
            var participantExchangeVisitor = new ParticipantExchangeVisitor();
            participantExchangeVisitor.ParticipantId = participantId;
            newParticipantExchangeVisitor.Audit.SetHistory(participantExchangeVisitor);
            Context.ParticipantExchangeVisitors.Add(participantExchangeVisitor);
        }

        private IQueryable<ParticipantExchangeVisitor> CreateGetParticipantExchangeVisitorByIdQuery(int participantId)
        {
            return Context.ParticipantExchangeVisitors.Where(x => x.ParticipantId == participantId);
        }

        #endregion
    }
}
