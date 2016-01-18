using ECA.Business.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Itineraries
{
    /// <summary>
    /// Validates ItineraryStop crud operations.
    /// </summary>
    public class ItineraryStopServiceValidator : BusinessValidatorBase<EcaItineraryStopValidationEntity, EcaItineraryStopValidationEntity>
    {
        /// <summary>
        /// The itinerary stop departure date error message when the departure date does not fall within the itinerary stop dates.
        /// </summary>
        public const string ITINERARY_STOP_DEPARTURE_DATE_IS_NOT_WITHIN_ITINERARY_DATES = "The city stop departure date does not fall within the travel period dates.";

        /// <summary>
        /// The itinerary stop arrival date error message when the arrival date does not fall within the itinerary stop dates.
        /// </summary>
        public const string ITINERARY_STOP_ARRIVAL_DATE_IS_NOT_WITHIN_ITINERARY_DATES = "The city stop arrival date does not fall within the travel period dates.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(EcaItineraryStopValidationEntity validationEntity)
        {
            return DoValidation(validationEntity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(EcaItineraryStopValidationEntity validationEntity)
        {
            return DoValidation(validationEntity);
        }

        /// <summary>
        /// Validates itinerary stop dates fall within the itinerary dates.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>The validation results.</returns>
        public IEnumerable<BusinessValidationResult> DoValidation(EcaItineraryStopValidationEntity validationEntity)
        {
            if(!(validationEntity.ItineraryStopArrivalDate >= validationEntity.ItineraryStartDate && validationEntity.ItineraryStopArrivalDate <= validationEntity.ItineraryEndDate))
            {
                yield return new BusinessValidationResult<EcaItineraryStop>(x => x.ArrivalDate, ITINERARY_STOP_ARRIVAL_DATE_IS_NOT_WITHIN_ITINERARY_DATES);
            }
            if (!(validationEntity.ItineraryStopDepartureDate >= validationEntity.ItineraryStartDate && validationEntity.ItineraryStopDepartureDate <= validationEntity.ItineraryEndDate))
            {
                yield return new BusinessValidationResult<EcaItineraryStop>(x => x.DepartureDate, ITINERARY_STOP_DEPARTURE_DATE_IS_NOT_WITHIN_ITINERARY_DATES);
            }
        }
    }
}
