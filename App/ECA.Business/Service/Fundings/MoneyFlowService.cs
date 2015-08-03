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
        }

        /// <summary>
        /// Gets moneyflows by the project id 
        /// </summary>
        /// <param name="projectId">The project id to find associated moneyflows</param>
        /// <param name="queryOperator"></param>
        /// <returns>List of moneyflows that are paged, sorted, and filtered</returns>
        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByProjectId(int projectId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var moneyFlows = MoneyFlowQueries.CreateGetMoneyFlowDTOsByProjectId(this.Context, projectId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by id {0} with query operator {1}.", projectId, queryOperator);
            return moneyFlows;
        }

        public PagedQueryResults<MoneyFlowDTO> GetMoneyFlowsByProgramId(int programId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var moneyFlows = MoneyFlowQueries.CreateGetMoneyFlowDTOsByProgramId(this.Context, programId, queryOperator).ToPagedQueryResults(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by program id {0} with query operator {1}.", programId, queryOperator);
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
            var moneyFlows = await MoneyFlowQueries.CreateGetMoneyFlowDTOsByProjectId(this.Context, projectId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by id {0} with query operator {1}.", projectId, queryOperator);
            return moneyFlows;
        }

        public async Task<PagedQueryResults<MoneyFlowDTO>> GetMoneyFlowsByProgramIdAsync(int programId, QueryableOperator<MoneyFlowDTO> queryOperator)
        {
            var moneyFlows = await MoneyFlowQueries.CreateGetMoneyFlowDTOsByProgramId(this.Context, programId, queryOperator).ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
            this.logger.Trace("Retrieved money flows by program id {0} with query operator {1}.", programId, queryOperator);
            return moneyFlows;
        }

        /// <summary>
        /// Returns the eca.data entity type mapping for the given MoneyFlowSourceRecipientType id.
        /// </summary>
        /// <param name="moneyFlowSourceRecipientTypeId">The id of the MoneyFlowSourceRecipientType.</param>
        /// <returns>The entity type or null if it is not mapped.</returns>
        public Type GetMoneyFlowType(int moneyFlowSourceRecipientTypeId)
        {
            var expectedMapping = new Dictionary<int, Type>();
            expectedMapping.Add(MoneyFlowSourceRecipientType.Itinerarystop.Id, typeof(ItineraryStop));
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
                description: moneyFlow.Description, 
                transactionDate: moneyFlow.TransactionDate, 
                value: moneyFlow.Value,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: moneyFlow.SourceEntityId,
                recipientEntityId: moneyFlow.RecipientEntityId);
        }

        /// <summary>
        /// Updates the system's money flow entry with the given updated money flow.
        /// </summary>
        /// <param name="updatedMoneyFlow">The updated money flow.</param>
        public void Update(UpdatedMoneyFlow updatedMoneyFlow)
        {
            var moneyFlowToUpdate = CreateGetMoneyFlowToUpdateQuery(updatedMoneyFlow.Id, updatedMoneyFlow.SourceEntityId).FirstOrDefault();
            DoUpdate(updatedMoneyFlow, moneyFlowToUpdate);
        }

        /// <summary>
        /// Updates the system's money flow entry with the given updated money flow.
        /// </summary>
        /// <param name="updatedMoneyFlow">The updated money flow.</param>
        /// <returns>The task.</returns>
        public async Task UpdateAsync(UpdatedMoneyFlow updatedMoneyFlow)
        {
            var moneyFlowToUpdate = await CreateGetMoneyFlowToUpdateQuery(updatedMoneyFlow.Id, updatedMoneyFlow.SourceEntityId).FirstOrDefaultAsync();
            DoUpdate(updatedMoneyFlow, moneyFlowToUpdate);
        }

        /// <summary>
        /// We need a query to make sure that the money flow you are updating is the one with the Id
        /// and the source entity id, for security.
        /// </summary>
        /// <param name="moneyFlowId">The money flow id.</param>
        /// <param name="sourceEntityId">The source entity id.</param>
        /// <returns>The money flow with the given money flow id and source entity id.</returns>
        private IQueryable<MoneyFlow> CreateGetMoneyFlowToUpdateQuery(int moneyFlowId, int sourceEntityId)
        {
            //In order to make sure the money flow that the client wants to update is one they have permission
            //to we need to make sure the money flow they wish to update is the one with the given id
            //and the source entity they have access to.
            return Context.MoneyFlows
                .Where(x => x.MoneyFlowId == moneyFlowId
                && (
                x.SourceItineraryStopId == sourceEntityId
                || x.SourceOrganizationId == sourceEntityId
                || x.SourceParticipantId == sourceEntityId
                || x.SourceProgramId == sourceEntityId
                || x.SourceProjectId == sourceEntityId
                ));
        }

        private void DoUpdate(UpdatedMoneyFlow updatedMoneyFlow, MoneyFlow moneyFlowToUpdate)
        {
            if (moneyFlowToUpdate == null)
            {
                throw new ModelNotFoundException(String.Format(
                    "The money flow with id [{0}] and source entity id [{1}] was not found.", 
                    updatedMoneyFlow.Id, 
                    updatedMoneyFlow.SourceEntityId));
            }
            moneyFlowToUpdate.Description = updatedMoneyFlow.Description;
            moneyFlowToUpdate.FiscalYear = updatedMoneyFlow.FiscalYear;
            moneyFlowToUpdate.MoneyFlowStatusId = updatedMoneyFlow.MoneyFlowStatusId;
            moneyFlowToUpdate.MoneyFlowTypeId = updatedMoneyFlow.MoneyFlowTypeId;
            moneyFlowToUpdate.TransactionDate = updatedMoneyFlow.TransactionDate;
            moneyFlowToUpdate.Value = updatedMoneyFlow.Value;
            updatedMoneyFlow.Audit.SetHistory(moneyFlowToUpdate);
        }
    }
}
