using ECA.Business.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// An BusinessValidator implementation for the ItineraryService.
    /// </summary>
    public class ItineraryServiceValidator : BusinessValidatorBase<AddedEcaItineraryValidationEntity, UpdatedEcaItineraryValidationEntity>
    {
        /// <summary>
        /// The error message returned when the start date is after the end date.
        /// </summary>
        public const string START_DATE_AFTER_END_DATE_ERROR_MESSAGE = "The start date must be before the end date.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(AddedEcaItineraryValidationEntity validationEntity)
        {
            if(validationEntity.AddedItinerary != null && validationEntity.AddedItinerary.StartDate > validationEntity.AddedItinerary.EndDate)
            {
                yield return new BusinessValidationResult<AddedEcaItinerary>(x => x.StartDate, START_DATE_AFTER_END_DATE_ERROR_MESSAGE);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(UpdatedEcaItineraryValidationEntity validationEntity)
        {
            if (validationEntity.UpdatedItinerary != null && validationEntity.UpdatedItinerary.StartDate > validationEntity.UpdatedItinerary.EndDate)
            {
                yield return new BusinessValidationResult<UpdatedEcaItinerary>(x => x.StartDate, START_DATE_AFTER_END_DATE_ERROR_MESSAGE);
            }
        }
    }
}
