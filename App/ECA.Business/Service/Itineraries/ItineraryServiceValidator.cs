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
        /// The error message returned when an itinerary end date is before the max itinerary stops date.
        /// </summary>
        public const string END_DATE_BEFORE_ITINERARY_STOP_MAX_DATE = "The travel period end date is before the latest city stop date.";

        /// <summary>
        /// The error message returned when an itinerary start date is before the min itinerary stops date.
        /// </summary>
        public const string START_DATE_BEFORE_ITINERARY_STOP_MIN_DATE = "The travel period start date is after the earliest city stop date.";

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

            var allDates = new List<DateTimeOffset>();
            allDates.AddRange(validationEntity.ItineraryStops.Where(x => x.DateArrive.HasValue).Select(x => x.DateArrive.Value).ToList());
            allDates.AddRange(validationEntity.ItineraryStops.Where(x => x.DateLeave.HasValue).Select(x => x.DateLeave.Value).ToList());
            if(allDates.Count > 0)
            {
                var minDate = allDates.Min();
                var maxDate = allDates.Max();
                if(validationEntity.UpdatedItinerary.EndDate < maxDate)
                {
                    yield return new BusinessValidationResult<UpdatedEcaItinerary>(x => x.EndDate, END_DATE_BEFORE_ITINERARY_STOP_MAX_DATE);
                }
                if(validationEntity.UpdatedItinerary.StartDate > minDate)
                {
                    yield return new BusinessValidationResult<UpdatedEcaItinerary>(x => x.StartDate, START_DATE_BEFORE_ITINERARY_STOP_MIN_DATE);
                }
            }
        }
    }
}
