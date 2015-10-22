using ECA.Business.Models.Fundings;
using ECA.Business.Validation;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Fundings
{
    /// <summary>
    /// The MoneyFlowServiceValidator is used to validate a MoneyFlow on create and update.
    /// </summary>
    public class MoneyFlowServiceValidator : BusinessValidatorBase<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity>
    {
        /// <summary>
        /// The error message when a MoneyFlow description is invalid.
        /// </summary>
        public const string INVALID_DESCRIPTION_ERROR_MESSAGE = "The description of the MoneyFlow is invalid.";

        /// <summary>
        /// The error message when the transaction amount is invalid.
        /// </summary>
        public const string INVALID_AMOUNT_MESSAGE = "The amount is invalid.";

        /// <summary>
        /// The error message when the money flow source or recipient type is invalid.
        /// </summary>
        public const string INVALID_SOURCE_TYPE_MESSAGE_FORMAT = "The source type [{0}] is invalid.";

        /// <summary>
        /// The error message when the source entity id null or does not have a value.
        /// </summary>
        public const string NULL_SOURCE_ENTITY_ID_MESSAGE = "The source entity id is unknown.";

        /// <summary>
        /// The error message when the source entity id null or does not have a value.
        /// </summary>
        public const string NULL_RECIPIENT_ENTITY_ID_MESSAGE = "The recipient entity id is unknown.";

        /// <summary>
        /// The fiscal year is less than zero.
        /// </summary>
        public const string FISCAL_YEAR_LESS_THAN_ZERO_MESSAGE = "The fiscal year must be greater than zero.";

        /// <summary>
        /// The string to format when the description exceeds the max length.
        /// </summary>
        public const string DESCRIPTION_EXCEEDS_MAX_LENGTH_FORMAT = "The description exceeds the max length of [{0}].";

        /// <summary>
        /// The source and recipient entities are equal error message.
        /// </summary>
        public const string SOURCE_AND_RECIPIENT_ENTITIES_EQUAL_ERROR_MESSAGE = "The source and recipient must not be the same.";

        /// <summary>
        /// The error message to return when the recipient entity type is not valid for the given source entity type.
        /// </summary>
        public const string RECIPIENT_ENTITY_TYPE_IS_NOT_VALID_FOR_SOURCE_ENTITY_TYPE = "The recipient entity type is not valid for the given source entity type.";

        /// <summary>
        /// The error message to return when the recipient is not a participant of the source project.
        /// </summary>
        public const string RECIPIENT_PARTICIPANT_IS_NOT_A_PARTICIPANT_OF_THE_PROJECT = "The recipient participant is not a participant of the source project.";

        /// <summary>
        /// The error message to return when the created or updated money flow value exceeds the allowable limit of the parent money flow.
        /// </summary>
        public const string VALUE_EXCEEDS_PARENT_MONEY_FLOW_WITHDRAWAL_LIMIT = "The money flow value exceeds the parent money flow withdrawal limit.";

        /// <summary>
        /// Returns enumerated validation results for a MoneyFlow create.
        /// </summary>
        /// <param name="validationEntity">The create entity.</param>
        /// <returns>The enumerated errors.</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(MoneyFlowServiceCreateValidationEntity validationEntity)
        {
            if (String.IsNullOrWhiteSpace(validationEntity.Description))
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.Description, INVALID_DESCRIPTION_ERROR_MESSAGE);
            }
            if (validationEntity.Description != null && validationEntity.Description.Length > MoneyFlow.DESCRIPTION_MAX_LENGTH)
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.Description, String.Format(DESCRIPTION_EXCEEDS_MAX_LENGTH_FORMAT, MoneyFlow.DESCRIPTION_MAX_LENGTH));
            }
            if ((int)validationEntity.Value < 0)
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.Value, INVALID_AMOUNT_MESSAGE);
            }
            if (validationEntity.SourceEntityTypeId == MoneyFlowSourceRecipientType.Accomodation.Id)
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.SourceEntityTypeId,
                    String.Format(INVALID_SOURCE_TYPE_MESSAGE_FORMAT, MoneyFlowSourceRecipientType.Accomodation.Value));
            }
            if (validationEntity.SourceEntityTypeId == MoneyFlowSourceRecipientType.Expense.Id)
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.SourceEntityTypeId,
                    String.Format(INVALID_SOURCE_TYPE_MESSAGE_FORMAT, MoneyFlowSourceRecipientType.Expense.Value));
            }
            if (validationEntity.HasSourceEntityType && !validationEntity.SourceEntityId.HasValue)
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.SourceEntityId, NULL_SOURCE_ENTITY_ID_MESSAGE);
            }
            if (validationEntity.HasRecipientEntityType && !validationEntity.RecipientEntityId.HasValue)
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.RecipientEntityId, NULL_RECIPIENT_ENTITY_ID_MESSAGE);
            }
            if (validationEntity.FiscalYear <= 0)
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.FiscalYear, FISCAL_YEAR_LESS_THAN_ZERO_MESSAGE);
            }
            if (validationEntity.SourceEntityTypeId == validationEntity.RecipientEntityTypeId
                && validationEntity.RecipientEntityId == validationEntity.SourceEntityId)
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.SourceEntityId, SOURCE_AND_RECIPIENT_ENTITIES_EQUAL_ERROR_MESSAGE);
            }
            if (!validationEntity.AllowedRecipientEntityTypeIds.Contains(validationEntity.RecipientEntityTypeId))
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.RecipientEntityTypeId, RECIPIENT_ENTITY_TYPE_IS_NOT_VALID_FOR_SOURCE_ENTITY_TYPE);
            }
            if (validationEntity.SourceEntityTypeId == MoneyFlowSourceRecipientType.Project.Id
                && validationEntity.RecipientEntityTypeId == MoneyFlowSourceRecipientType.Participant.Id
                && validationEntity.RecipientEntityId.HasValue
                && !validationEntity.AllowedProjectParticipantIds.Contains(validationEntity.RecipientEntityId.Value))
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.RecipientEntityId, RECIPIENT_PARTICIPANT_IS_NOT_A_PARTICIPANT_OF_THE_PROJECT);
            }
            if (validationEntity.ParentMoneyFlowWithdrawlMaximum.HasValue
                && validationEntity.ParentMoneyFlowWithdrawlMaximum.Value < validationEntity.Value)
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.Value, VALUE_EXCEEDS_PARENT_MONEY_FLOW_WITHDRAWAL_LIMIT);
            }
        }
        /// <summary>
        /// Returns enumerated validation results for a MoneyFlow update.
        /// </summary>
        /// <param name="validationEntity">The update entity.</param>
        /// <returns>The enumerated errors.</returns>

        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(MoneyFlowServiceUpdateValidationEntity validationEntity)
        {
            if (String.IsNullOrWhiteSpace(validationEntity.Description))
            {
                yield return new BusinessValidationResult<UpdatedMoneyFlow>(x => x.Description, INVALID_DESCRIPTION_ERROR_MESSAGE);
            }
            if (validationEntity.Description != null && validationEntity.Description.Length > MoneyFlow.DESCRIPTION_MAX_LENGTH)
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.Description, String.Format(DESCRIPTION_EXCEEDS_MAX_LENGTH_FORMAT, MoneyFlow.DESCRIPTION_MAX_LENGTH));
            }
            if ((int)validationEntity.Value < 0)
            {
                yield return new BusinessValidationResult<UpdatedMoneyFlow>(x => x.Value, INVALID_AMOUNT_MESSAGE);
            }
            if (validationEntity.FiscalYear <= 0)
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.FiscalYear, FISCAL_YEAR_LESS_THAN_ZERO_MESSAGE);
            }
            if (validationEntity.ParentMoneyFlowWithdrawlMaximum.HasValue
                && validationEntity.ParentMoneyFlowWithdrawlMaximum.Value < validationEntity.Value)
            {
                yield return new BusinessValidationResult<AdditionalMoneyFlow>(x => x.Value, VALUE_EXCEEDS_PARENT_MONEY_FLOW_WITHDRAWAL_LIMIT);
            }
        }
    }
}
