using System.Linq;
using System.Data.Entity;
using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using ECA.Business.Validation;
using System.Collections.Generic;
using System.Diagnostics;
using ECA.Core.Exceptions;
using NLog;
using ECA.Business.Models.Admin;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// A service to perform crud operations on moneyflows
    /// </summary>
    public class MoneyFlowService : EcaService, IMoneyFlowService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IBusinessValidator<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity> validator;

        public MoneyFlowService(EcaContext context, IOfficeService officeService, IBusinessValidator<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity> validator)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(validator != null, "The validator must not be null.");
            this.validator = validator;
        }
        /// <summary>
        /// Gets moneyflows by the project id 
        /// </summary>
        /// <param name="projectId">The project id to find associated moneyflows</param>
        /// <param name="queryOperator"></param>
        /// <returns>List of moneyflows that are paged, sorted, and filtered</returns>
        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByProjectId(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var moneyFlows = MoneyFlowQueries.CreateGetMoneyFlowsByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by id {0} with query operator {1}.", projectId, queryOperator);
            return moneyFlows;
        }

        /// <summary>
        /// Gets moneyflows by the project id asynchronously
        /// </summary>
        /// <param name="projectId">The project id to find associated moneyflows</param>
        /// <param name="queryOperator"></param>
        /// <returns>List of moneyflows that are paged, sorted, and filtered</returns>
        public async Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByProjectIdAsync(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var moneyFlows = await MoneyFlowQueries.CreateGetMoneyFlowsByProjectIdQuery(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by id {0} with query operator {1}.", projectId, queryOperator);
            return moneyFlows;
        }
        public Task<MoneyFlow> GetMoneyFlowByIdAsync(int moneyFlowId)
        {
            return this.Context.MoneyFlows.FindAsync(moneyFlowId);
        }

        public MoneyFlow GetMoneyFlowById(int moneyFlowId) 
        {
            return this.Context.MoneyFlows.Find(moneyFlowId);
        }

        public MoneyFlow Create(EcaMoneyFlow moneyFlow, User user)
        {
            validator.ValidateCreate(GetCreateValidationEntity(moneyFlow));
            var newMoneyFlow = DoCreate(moneyFlow, user);
            this.logger.Trace("Created money flow {0}.", moneyFlow);
            return newMoneyFlow;
        }

        public async Task<MoneyFlow> CreateAsync(EcaMoneyFlow moneyFlow, User user)
        {
            validator.ValidateCreate(GetCreateValidationEntity(moneyFlow));
            var newMoneyFlow = await DoCreateAsync(moneyFlow, user);
            this.logger.Trace("Created money flow {0}.", newMoneyFlow);
            return newMoneyFlow;
        }

        private MoneyFlow DoCreate(EcaMoneyFlow moneyFlow, User createdBy)
        {
            var newMoneyFlow = new MoneyFlow
            {
                MoneyFlowTypeId = moneyFlow.MoneyFlowTypeId,
                MoneyFlowStatusId = moneyFlow.MoneyFlowStatusId,
                TransactionDate = moneyFlow.TransactionDate,
                Value = moneyFlow.Value,
                Description = moneyFlow.Description,
                FiscalYear = moneyFlow.FiscalYear,
                SourceTypeId = moneyFlow.SourceTypeId,
                RecipientTypeId = moneyFlow.RecipientTypeId,
        };


            newMoneyFlow.SourceOrganization = GetOrganization(moneyFlow.SourceOrganizationId);
            newMoneyFlow.RecipientOrganization = GetOrganization(moneyFlow.RecipientOrganizationId);

            newMoneyFlow.SourceProgram = GetProgram(moneyFlow.SourceProgramId);
            newMoneyFlow.RecipientProgram = GetProgram(moneyFlow.RecipientProgramId);

            newMoneyFlow.SourceProject = GetProject(moneyFlow.SourceProjectId);
            newMoneyFlow.RecipientProject = GetProject(moneyFlow.RecipientProjectId);

            newMoneyFlow.SourceParticipant = GetParticipant(moneyFlow.SourceParticipantId);
            newMoneyFlow.RecipientParticipant = GetParticipant(moneyFlow.RecipientParticipantId);

            newMoneyFlow.SourceItineraryStop = GetItineraryStop(moneyFlow.SourceItineraryStopId);
            newMoneyFlow.RecipientItineraryStop = GetItineraryStop(moneyFlow.RecipientItineraryStopId);

            newMoneyFlow.RecipientAccommodation = GetAccomodation(moneyFlow.RecipientAccommodationId);
            newMoneyFlow.RecipientTransportation = GetTransportation(moneyFlow.RecipientTransportationId);

            moneyFlow.Audit = new Create(createdBy);
            moneyFlow.Audit.SetHistory(newMoneyFlow);
            this.Context.MoneyFlows.Add(newMoneyFlow);
            return newMoneyFlow;
        }

        private async Task<MoneyFlow> DoCreateAsync(EcaMoneyFlow moneyFlow, User createdBy)
        {
            var newMoneyFlow = new MoneyFlow
            {
                MoneyFlowTypeId = moneyFlow.MoneyFlowTypeId,
                MoneyFlowStatusId = moneyFlow.MoneyFlowStatusId,
                TransactionDate = moneyFlow.TransactionDate,
                Value = moneyFlow.Value,
                Description = moneyFlow.Description,
                FiscalYear = moneyFlow.FiscalYear,
                SourceTypeId = moneyFlow.SourceTypeId,
                RecipientTypeId = moneyFlow.RecipientTypeId,
        };

            newMoneyFlow.SourceOrganization = GetOrganization(moneyFlow.SourceOrganizationId);
            newMoneyFlow.RecipientOrganization = GetOrganization(moneyFlow.RecipientOrganizationId);

            newMoneyFlow.SourceProgram = GetProgram(moneyFlow.SourceProgramId);
            newMoneyFlow.RecipientProgram = GetProgram(moneyFlow.RecipientProgramId);

            newMoneyFlow.SourceProject = GetProject(moneyFlow.SourceProjectId);
            newMoneyFlow.RecipientProject = GetProject(moneyFlow.RecipientProjectId);

            newMoneyFlow.SourceParticipant = GetParticipant(moneyFlow.SourceParticipantId);
            newMoneyFlow.RecipientParticipant = GetParticipant(moneyFlow.RecipientParticipantId);

            newMoneyFlow.SourceItineraryStop = GetItineraryStop(moneyFlow.SourceItineraryStopId);
            newMoneyFlow.RecipientItineraryStop = GetItineraryStop(moneyFlow.RecipientItineraryStopId);

            newMoneyFlow.RecipientAccommodation = GetAccomodation(moneyFlow.RecipientAccommodationId);
            newMoneyFlow.RecipientTransportation = GetTransportation(moneyFlow.RecipientTransportationId);

            moneyFlow.Audit = new Create(createdBy);
            moneyFlow.Audit.SetHistory(newMoneyFlow);
            this.Context.MoneyFlows.Add(newMoneyFlow);
            return newMoneyFlow;
        }


        #region Update

        public void Update(EcaMoneyFlow updatedMoneyFlow)
        {
            // stub 
        }

        public async Task UpdateAsync(EcaMoneyFlow updatedMoneyFlow) 
        { 
            // stub
        }

        public MoneyFlow Copy(int moneyFlowId, User user)
        {
            // STUB
            this.logger.Trace("Copied money flow {0}.", moneyFlowId);
            return null;
        }

        public async Task<MoneyFlow> CopyAsync(int moneyFlowId, User user)
        {
            //STUB
            this.logger.Trace("Created money flow {0}.", moneyFlowId);
            return null;
        }

        #endregion

        private MoneyFlow GetParent(int? parentId)
        {
            return this.Context.MoneyFlows.Find(parentId);
        }
        private Organization GetOrganization(int? organizationId)
        {
            return this.Context.Organizations.Find(organizationId);
        }

        private Program GetProgram(int? programId)
        {
            return this.Context.Programs.Find(programId);
        }
        private Participant GetParticipant(int? participantId)
        {
            return this.Context.Participants.Find(participantId);
        }
        private Project GetProject(int? projectId)
        {
            return this.Context.Projects.Find(projectId);
        }
        private ItineraryStop GetItineraryStop(int? itineraryStopId)
        {
            return this.Context.ItineraryStops.Find(itineraryStopId);
        }
        private Accommodation GetAccomodation(int? accomodationId)
        {
            return this.Context.Accommodations.Find(accomodationId);
        }
        private Transportation GetTransportation(int? transportationId)
        {
            return this.Context.Transportations.Find(transportationId);
        }
        
        private MoneyFlowServiceCreateValidationEntity GetCreateValidationEntity(EcaMoneyFlow draftMoneyFlow)
        {
            return new MoneyFlowServiceCreateValidationEntity(draftMoneyFlow.Description, draftMoneyFlow.Value,
                draftMoneyFlow.TransactionDate);
        }

    }
}
