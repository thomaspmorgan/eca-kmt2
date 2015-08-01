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
            if ((int)validationEntity.Value <= 0)
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

        }
        /// <summary>
        /// Returns enumerated validation results for a MoneyFlow update.
        /// </summary>
        /// <param name="validationEntity">The update entity.</param>
        /// <returns>The enumerated errors.</returns>

        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(MoneyFlowServiceUpdateValidationEntity validationEntity)
        {

            //if (String.IsNullOrWhiteSpace(validationEntity.Description))
            //{
            //    yield return new BusinessValidationResult<DraftMoneyFlow>(x => x.Description, INVALID_DESCRIPTION_ERROR_MESSAGE);
            //}
            return Enumerable.Empty<BusinessValidationResult>();

        }
    }
}
