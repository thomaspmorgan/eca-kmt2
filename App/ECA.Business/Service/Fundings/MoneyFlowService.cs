using ECA.Business.Queries.Fundings;
using System.Linq;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Data;
using NLog;
using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections.Generic;
using ECA.Business.Models.Fundings;
using ECA.Core.Exceptions;
using ECA.Business.Exceptions;
using ECA.Business.Queries.Admin;

namespace ECA.Business.Service.Fundings
{
    /// <summary>
    /// A service to perform crud operations on moneyflows
    /// </summary>
    public class MoneyFlowService : EcaService, IMoneyFlowService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IBusinessValidator<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity> validator;
        private Action<object, int, Type> throwIfEntityNotFound;
        private Action<int, MoneyFlow, MoneyFlow> throwSecurityViolationIfNull;
        private Action<int, OfficeDTO> throwSecurityViolationIfOrgIsOffice;

        /// <summary>
        /// Creates a new instance with the context to operate against and the validator.
        /// </summary>
        /// <param name="context">The context to perform crud operations on.</param>
        /// <param name="validator">The business validator.</param>
        public MoneyFlowService(EcaContext context, IBusinessValidator<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity> validator)
            : base(context)
        {
            Contract.Requires(context != null, "The context must not be null.");
            Contract.Requires(validator != null, "The validator must not be null.");
            this.validator = validator;
            throwIfEntityNotFound = (instance, id, type) =>
            {
                if (instance == null)
                {
                    throw new ModelNotFoundException(String.Format("The entity of type [{0}] with id [{1}] was not found.", type.Name, id));
                }
            };
            throwSecurityViolationIfNull = (userId, instance, actualMoneyFlow) =>
            {
                if (instance == null && actualMoneyFlow != null)
                {
                    throw new BusinessSecurityException(
                        String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.", 
                        userId, 
                        actualMoneyFlow.MoneyFlowId));
                }
            };
            throwSecurityViolationIfOrgIsOffice = (organizationId, officeDto) =>
            {
                if(officeDto != null)
                {
                    throw new BusinessSecurityException(
                        String.Format("The organization with the given id [{0}] is an office named [{1}].  This office must be accessed using office related methods only.",
                        organizationId,
                        officeDto.Name));
                }
            };
        }

        #region Get
        /// <summary>
        /// Returns the money flows for the project with the given id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The project's money flows.</returns>
        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByProjectId(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var moneyFlows = MoneyFlowQueries.CreateGetMoneyFlowDTOsByProjectId(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by id {0} with query operator {1}.", projectId, queryOperator);
            return moneyFlows;
        }

        /// <summary>
        /// Returns the money flows for the project with the given id.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The project's money flows.</returns>
        public async Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByProjectIdAsync(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var moneyFlows = await MoneyFlowQueries.CreateGetMoneyFlowDTOsByProjectId(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by id {0} with query operator {1}.", projectId, queryOperator);
            return moneyFlows;
        }

        /// <summary>
        /// Returns the money flows for the program with the given id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The programs's money flows.</returns>
        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByProgramId(int programId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var moneyFlows = MoneyFlowQueries.CreateGetMoneyFlowDTOsByProgramId(this.Context, programId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by program id {0} with query operator {1}.", programId, queryOperator);
            return moneyFlows;
        }

        /// <summary>
        /// Returns the money flows for the program with the given id.
        /// </summary>
        /// <param name="programId">The program id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The programs's money flows.</returns>
        public async Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByProgramIdAsync(int programId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var moneyFlows = await MoneyFlowQueries.CreateGetMoneyFlowDTOsByProgramId(this.Context, programId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by program id {0} with query operator {1}.", programId, queryOperator);
            return moneyFlows;
        }

        /// <summary>
        /// Returns the money flows for the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The organization's money flows.</returns>
        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByOrganizationId(int organizationId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var office = CreateGetOfficeByOrganizationIdQuery(organizationId).FirstOrDefault();
            throwSecurityViolationIfOrgIsOffice(organizationId, office);
            var moneyFlows = MoneyFlowQueries.CreateGetMoneyFlowDTOsByOrganizationId(this.Context, organizationId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by organization id {0} with query operator {1}.", organizationId, queryOperator);
            return moneyFlows;
        }

        /// <summary>
        /// Returns the money flows for the organization with the given id.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The organization's money flows.</returns>
        public async Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByOrganizationIdAsync(int organizationId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var office = await CreateGetOfficeByOrganizationIdQuery(organizationId).FirstOrDefaultAsync();
            throwSecurityViolationIfOrgIsOffice(organizationId, office);
            var moneyFlows = await MoneyFlowQueries.CreateGetMoneyFlowDTOsByOrganizationId(this.Context, organizationId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by organization id {0} with query operator {1}.", organizationId, queryOperator);
            return moneyFlows;
        }

        /// <summary>
        /// Returns the money flows for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The office's money flows.</returns>
        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByOfficeId(int officeId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var office = CreateGetOfficeByOrganizationIdQuery(officeId).FirstOrDefault();
            var moneyFlows = MoneyFlowQueries.CreateGetMoneyFlowDTOsByOfficeId(this.Context, officeId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by organization id {0} with query operator {1}.", officeId, queryOperator);
            return moneyFlows;
        }

        /// <summary>
        /// Returns the money flows for the office with the given id.
        /// </summary>
        /// <param name="officeId">The office id.</param>
        /// <param name="queryOperator">The query operator.</param>
        /// <returns>The office's money flows.</returns>
        public async Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByOfficeIdAsync(int officeId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var moneyFlows = await MoneyFlowQueries.CreateGetMoneyFlowDTOsByOfficeId(this.Context, officeId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by organization id {0} with query operator {1}.", officeId, queryOperator);
            return moneyFlows;
        }

        private IQueryable<OfficeDTO> CreateGetOfficeByOrganizationIdQuery(int organizationId)
        {
            return OfficeQueries.CreateGetOfficeByIdQuery(this.Context, organizationId);
        }
        #endregion

        /// <summary>
        /// Returns the eca.data entity type mapping for the given MoneyFlowSourceRecipientType id.
        /// </summary>
        /// <param name="moneyFlowSourceRecipientTypeId">The id of the MoneyFlowSourceRecipientType.</param>
        /// <returns>The entity type or null if it is not mapped.</returns>
        public Type GetMoneyFlowType(int moneyFlowSourceRecipientTypeId)
        {
            var expectedMapping = new Dictionary<int, Type>();
            expectedMapping.Add(MoneyFlowSourceRecipientType.ItineraryStop.Id, typeof(ItineraryStop));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Office.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Organization.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Participant.Id, typeof(Participant));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Program.Id, typeof(Program));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Project.Id, typeof(Project));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Post.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Accomodation.Id, typeof(Accommodation));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Transportation.Id, typeof(Transportation));
            if (expectedMapping.ContainsKey(moneyFlowSourceRecipientTypeId))
            {
                return expectedMapping[moneyFlowSourceRecipientTypeId];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns true, if the given id corresponds to a ECA.Data entity type.
        /// </summary>
        /// <param name="moneyFlowSourceRecipientTypeId">The id of the MoneyFlowSourceRecipientType.</param>
        /// <returns>Returns true, if the given id corresponds to a ECA.Data entity type, otherwise false.</returns>
        public bool IsMoneyFlowType(int moneyFlowSourceRecipientTypeId)
        {
            return GetMoneyFlowType(moneyFlowSourceRecipientTypeId) != null;
        }

        #region Create
        /// <summary>
        /// Adds the given money flow object to the ECA System.
        /// </summary>
        /// <param name="moneyFlow">The new money flow.</param>
        /// <returns>The ECA.Data money flow entity.</returns>
        public MoneyFlow Create(AdditionalMoneyFlow moneyFlow)
        {   
            var hasSourceEntityType = IsMoneyFlowType(moneyFlow.SourceEntityTypeId);
            var hasRecipientEntityType = IsMoneyFlowType(moneyFlow.RecipientEntityTypeId);
            object sourceEntity = null;
            object recipientEntity = null;
            validator.ValidateCreate(GetCreateValidationEntity(moneyFlow, hasSourceEntityType, hasRecipientEntityType));
            if (hasSourceEntityType)
            {
                Contract.Assert(moneyFlow.SourceEntityId.HasValue, "The source entity id should have a value here.  This should be checked by validator.");
                var sourceType = GetMoneyFlowType(moneyFlow.SourceEntityTypeId);
                sourceEntity = Context.Set(sourceType).Find(moneyFlow.SourceEntityId.Value);
                throwIfEntityNotFound(sourceEntity, moneyFlow.SourceEntityId.Value, sourceType);
            }
            if (hasRecipientEntityType)
            {
                Contract.Assert(moneyFlow.RecipientEntityId.HasValue, "The recipient entity id should have a value here.  This should be checked by validator.");
                var recipientType = GetMoneyFlowType(moneyFlow.RecipientEntityTypeId);
                recipientEntity = Context.Set(recipientType).Find(moneyFlow.RecipientEntityId.Value);
                throwIfEntityNotFound(recipientEntity, moneyFlow.RecipientEntityId.Value, recipientType);
            }
            return DoCreate(moneyFlow);

        }

        /// <summary>
        /// Adds the given money flow object to the ECA System.
        /// </summary>
        /// <param name="moneyFlow">The new money flow.</param>
        /// <returns>The ECA.Data money flow entity.</returns>
        public async Task<MoneyFlow> CreateAsync(AdditionalMoneyFlow moneyFlow)
        {   
            var hasSourceEntityType = IsMoneyFlowType(moneyFlow.SourceEntityTypeId);
            var hasRecipientEntityType = IsMoneyFlowType(moneyFlow.RecipientEntityTypeId);
            object sourceEntity = null;
            object recipientEntity = null;
            validator.ValidateCreate(GetCreateValidationEntity(moneyFlow, hasSourceEntityType, hasRecipientEntityType));
            if (hasSourceEntityType)
            {
                Contract.Assert(moneyFlow.SourceEntityId.HasValue, "The source entity id should have a value here.  This should be checked by validator.");
                var sourceType = GetMoneyFlowType(moneyFlow.SourceEntityTypeId);
                sourceEntity = await Context.Set(sourceType).FindAsync(moneyFlow.SourceEntityId.Value);
                throwIfEntityNotFound(sourceEntity, moneyFlow.SourceEntityId.Value, sourceType);
            }
            if (hasRecipientEntityType)
            {
                Contract.Assert(moneyFlow.RecipientEntityId.HasValue, "The recipient entity id should have a value here.  This should be checked by validator.");
                var recipientType = GetMoneyFlowType(moneyFlow.RecipientEntityTypeId);
                recipientEntity = await Context.Set(recipientType).FindAsync(moneyFlow.RecipientEntityId.Value);
                throwIfEntityNotFound(recipientEntity, moneyFlow.RecipientEntityId.Value, recipientType);
            }
            return DoCreate(moneyFlow);
        }

        private MoneyFlow DoCreate(AdditionalMoneyFlow moneyFlow)
        {
            var newMoneyFlow = moneyFlow.GetMoneyFlow();
            Context.MoneyFlows.Add(newMoneyFlow);
            return newMoneyFlow;
        }

        private MoneyFlowServiceCreateValidationEntity GetCreateValidationEntity(AdditionalMoneyFlow moneyFlow, bool hasSourceEntityType, bool hasRecipientEntityType)
        {
            return new MoneyFlowServiceCreateValidationEntity(
                sourceEntityTypeId: moneyFlow.SourceEntityTypeId,
                recipientEntityTypeId: moneyFlow.RecipientEntityTypeId,
                description: moneyFlow.Description, 
                transactionDate: moneyFlow.TransactionDate, 
                value: moneyFlow.Value,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: moneyFlow.SourceEntityId,
                recipientEntityId: moneyFlow.RecipientEntityId,
                fiscalYear: moneyFlow.FiscalYear);
        }
        #endregion

        #region Update

        /// <summary>
        /// Updates the system's money flow entry with the given updated money flow.
        /// </summary>
        /// <param name="updatedMoneyFlow">The updated money flow.</param>
        public void Update(UpdatedMoneyFlow updatedMoneyFlow)
        {
            var moneyFlowToUpdate = Context.MoneyFlows.Find(updatedMoneyFlow.Id);
            var permissableMoneyFlow = CreateGetMoneyFlowByIdAndEntityIdQuery(updatedMoneyFlow.Id, updatedMoneyFlow.SourceOrRecipientEntityId).FirstOrDefault();
            throwSecurityViolationIfNull(updatedMoneyFlow.Audit.User.Id, permissableMoneyFlow, moneyFlowToUpdate);
            DoUpdate(updatedMoneyFlow, moneyFlowToUpdate);
        }

        /// <summary>
        /// Updates the system's money flow entry with the given updated money flow.
        /// </summary>
        /// <param name="updatedMoneyFlow">The updated money flow.</param>
        /// <returns>The task.</returns>
        public async Task UpdateAsync(UpdatedMoneyFlow updatedMoneyFlow)
        {
            var moneyFlowToUpdate = await Context.MoneyFlows.FindAsync(updatedMoneyFlow.Id);
            var permissableMoneyFlow = await CreateGetMoneyFlowByIdAndEntityIdQuery(updatedMoneyFlow.Id, updatedMoneyFlow.SourceOrRecipientEntityId).FirstOrDefaultAsync();
            throwSecurityViolationIfNull(updatedMoneyFlow.Audit.User.Id, permissableMoneyFlow, moneyFlowToUpdate);
            DoUpdate(updatedMoneyFlow, moneyFlowToUpdate);
        }

        private void DoUpdate(UpdatedMoneyFlow updatedMoneyFlow, MoneyFlow moneyFlowToUpdate)
        {
            if (moneyFlowToUpdate == null)
            {
                throw new ModelNotFoundException(String.Format(
                    "The money flow with id [{0}] and source entity id [{1}] was not found.", 
                    updatedMoneyFlow.Id, 
                    updatedMoneyFlow.SourceOrRecipientEntityId));
            }
            validator.ValidateUpdate(GetUpdateValidationEntity(updatedMoneyFlow));
            moneyFlowToUpdate.Description = updatedMoneyFlow.Description;
            moneyFlowToUpdate.FiscalYear = updatedMoneyFlow.FiscalYear;
            moneyFlowToUpdate.MoneyFlowStatusId = updatedMoneyFlow.MoneyFlowStatusId;
            moneyFlowToUpdate.MoneyFlowTypeId = updatedMoneyFlow.MoneyFlowTypeId;
            moneyFlowToUpdate.TransactionDate = updatedMoneyFlow.TransactionDate;
            moneyFlowToUpdate.Value = updatedMoneyFlow.Value;
            updatedMoneyFlow.Audit.SetHistory(moneyFlowToUpdate);
        }

        private MoneyFlowServiceUpdateValidationEntity GetUpdateValidationEntity(UpdatedMoneyFlow moneyFlow)
        {
            return new MoneyFlowServiceUpdateValidationEntity(
                description: moneyFlow.Description, 
                value: moneyFlow.Value, 
                fiscalYear: moneyFlow.FiscalYear);
        }
        #endregion

        #region Delete

        /// <summary>
        /// Deletes the money from the system.
        /// </summary>
        /// <param name="deletedMoneyFlow">The money flow to delete.</param>
        public void Delete(DeletedMoneyFlow deletedMoneyFlow)
        {
            var moneyFlowToDelete = Context.MoneyFlows.Find(deletedMoneyFlow.Id);
            var permissableMoneyFlow = CreateGetMoneyFlowByIdAndEntityIdQuery(deletedMoneyFlow.Id, deletedMoneyFlow.SourceOrRecipientEntityId).FirstOrDefault();
            throwSecurityViolationIfNull(deletedMoneyFlow.Audit.User.Id, permissableMoneyFlow, moneyFlowToDelete);
            DoDelete(moneyFlowToDelete);
        }

        /// <summary>
        /// Deletes the money from the system.
        /// </summary>
        /// <param name="deletedMoneyFlow">The money flow to delete.</param>
        public async Task DeleteAsync(DeletedMoneyFlow deletedMoneyFlow)
        {
            var moneyFlowToDelete = await Context.MoneyFlows.FindAsync(deletedMoneyFlow.Id);
            var permissableMoneyFlow = await CreateGetMoneyFlowByIdAndEntityIdQuery(deletedMoneyFlow.Id, deletedMoneyFlow.SourceOrRecipientEntityId).FirstOrDefaultAsync();
            throwSecurityViolationIfNull(deletedMoneyFlow.Audit.User.Id, permissableMoneyFlow, moneyFlowToDelete);
            DoDelete(moneyFlowToDelete);
        }

        private void DoDelete(MoneyFlow moneyFlowToDelete)
        {
            Contract.Requires(moneyFlowToDelete != null, "The money flow to delete must not be null.");
            Context.MoneyFlows.Remove(moneyFlowToDelete);
        }
        #endregion

        /// <summary>
        /// We need a query to make sure that the money flow you are updating is the one with the Id
        /// and the source or recipient entity id.  This is an additional security measure to ensure that user who
        /// was granted access to edit a certain entities money flow does not then try to go an edit a seperate money flow.
        /// </summary>
        /// <param name="moneyFlowId">The money flow id.</param>
        /// <param name="entityId">The permissable entity id, i.e. the entity by which a user has been granted access to modify.</param>
        /// <returns>The money flow with the given money flow id and source entity id.</returns>
        private IQueryable<MoneyFlow> CreateGetMoneyFlowByIdAndEntityIdQuery(int moneyFlowId, int entityId)
        {
            //In order to make sure the money flow that the client wants to update is one they have permission
            //to we need to make sure the money flow they wish to update is the one with the given id
            //and the source entity they have access to.
            return Context.MoneyFlows
                .Where(x => x.MoneyFlowId == moneyFlowId
                && (
                x.SourceItineraryStopId == entityId
                || x.SourceOrganizationId == entityId
                || x.SourceParticipantId == entityId
                || x.SourceProgramId == entityId
                || x.SourceProjectId == entityId
                || x.RecipientAccommodationId == entityId
                || x.RecipientItineraryStopId == entityId
                || x.RecipientOrganizationId == entityId
                || x.RecipientParticipantId == entityId
                || x.RecipientProgramId == entityId
                || x.RecipientProjectId == entityId
                || x.RecipientTransportationId == entityId
                ));
        }
    }
}
