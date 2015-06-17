using ECA.Business.Validation;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// The MoneyFlowServiceValidator is used to validate a MoneyFlow on create and update.
    /// </summary>
    public class MoneyFlowServiceValidator : BusinessValidatorBase<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity>
    {
        /// <summary>
        /// The error message when a MoneyFlow is given an invalid description.
        /// </summary>
        public const string INVALID_DESCRIPTION_ERROR_MESSAGE = "The description of the MoneyFlow is invalid.";

        /// <summary>
        /// The error message when the start and ends dates of a MoneyFlow are invalid.
        /// </summary>
        public const string INVALID_TRANSACTION_DATE_MESSAGE = "The transaction date is invalid.";
        /// <summary>
        /// The error message when the start and ends dates of a MoneyFlow are invalid.
        /// </summary>
        public const string INVALID_AMOUNT_MESSAGE = "The amount is invalid.";

        /// <summary>
        /// Returns enumerated validation results for a MoneyFlow create.
        /// </summary>
        /// <param name="validationEntity">The create entity.</param>
        /// <returns>The enumerated errors.</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(MoneyFlowServiceCreateValidationEntity validationEntity)
        {

            if (String.IsNullOrWhiteSpace(validationEntity.Description))
            {
                yield return new BusinessValidationResult<DraftMoneyFlow>(x => x.Description, INVALID_DESCRIPTION_ERROR_MESSAGE);
            }
            if (validationEntity.Value == 0)
            {
                yield return new BusinessValidationResult<DraftMoneyFlow>(x => x.Value, INVALID_AMOUNT_MESSAGE);
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
                yield return new BusinessValidationResult<DraftMoneyFlow>(x => x.Description, INVALID_DESCRIPTION_ERROR_MESSAGE);
            }

        }
    }
}
