using ECA.Business.Validation;
using ECA.Core.Generation;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// A LocationServiceValidator is used to validate new and updated locations within the eca system.
    /// </summary>
    public class LocationServiceValidator : BusinessValidatorBase<LocationValidationEntity, LocationValidationEntity>
    {
        /// <summary>
        /// The error message to return when a location is marked as an address.
        /// </summary>
        public const string LOCATION_MUST_NOT_BE_AN_ADDRESS_ERROR_MESSAGE = "The location must not be an address.";

        /// <summary>
        /// The error message to return when a city does not have a country specified.
        /// </summary>
        public const string CITY_MUST_HAVE_COUNTRY = "A city location must have an associated country.";

        /// <summary>
        /// The error message to return when a building does not have a country specified.
        /// </summary>
        public const string BUILDING_MUST_HAVE_COUNTRY = "A building must have an associated country.";

        /// <summary>
        /// The error message to return when a building does not have a country specified.
        /// </summary>
        public const string PLACE_MUST_HAVE_COUNTRY = "A place must have an associated country.";

        /// <summary>
        /// The error message to return when a city does not belong to the given division.
        /// </summary>
        public const string CITY_MUST_BELONG_TO_DIVISION = "The given city must belong to the given division.";

        /// <summary>
        /// The error message to return when a division does not belong to the given country.
        /// </summary>
        public const string DIVISION_MUST_BELONG_COUNTRY = "The given division must belong to the given country.";

        /// <summary>
        /// The error message to return when a division does not belong to the given country.
        /// </summary>
        public const string LATITUDE_LONGITUDE_SPECIFIED_BUT_INVALID_LOCATION_TYPE = "If the location is specified by longitude and/or latitude it must be a place, or building.";

        /// <summary>
        /// The location name is invalid.
        /// </summary>
        public const string INVALID_LOCATION_NAME = "The location name is invalid.";

        /// <summary>
        /// Performs validation on a location create.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>The validation results.</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateCreate(LocationValidationEntity validationEntity)
        {
            return GetLocationTypesValidations(validationEntity)
                .Union(GetLocationValidations(validationEntity))
                .Union(GetPropertyValidations(validationEntity));
        }

        /// <summary>
        /// Performs validation on a location update.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>The validation results.</returns>
        public override IEnumerable<BusinessValidationResult> DoValidateUpdate(LocationValidationEntity validationEntity)
        {
            return DoValidateCreate(validationEntity);
        }

        /// <summary>
        /// Returns the property validations.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>The validation results.</returns>
        public IEnumerable<BusinessValidationResult> GetPropertyValidations(LocationValidationEntity validationEntity)
        {
            if (String.IsNullOrWhiteSpace(validationEntity.LocationName))
            {
                yield return new BusinessValidationResult<EcaLocation>(x => x.LocationName, INVALID_LOCATION_NAME);
            }
        }

        /// <summary>
        /// Returns the validations related to location types.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>The validation results.</returns>
        public IEnumerable<BusinessValidationResult> GetLocationTypesValidations(LocationValidationEntity validationEntity)
        {
            var locationType = GetLocationType(validationEntity);
            if (locationType == LocationType.Address)
            {
                yield return new BusinessValidationResult<EcaLocation>(x => x.LocationTypeId, LOCATION_MUST_NOT_BE_AN_ADDRESS_ERROR_MESSAGE);
            }
            if (locationType == LocationType.City
                && validationEntity.Country == null)
            {
                yield return new BusinessValidationResult<EcaLocation>(x => x.CityId, CITY_MUST_HAVE_COUNTRY);
            }
            if (locationType == LocationType.Building
                && validationEntity.Country == null)
            {
                yield return new BusinessValidationResult<EcaLocation>(x => x.CountryId, BUILDING_MUST_HAVE_COUNTRY);
            }
            if (locationType == LocationType.Place
                && validationEntity.Country == null)
            {
                yield return new BusinessValidationResult<EcaLocation>(x => x.CountryId, PLACE_MUST_HAVE_COUNTRY);
            }
            if((validationEntity.Longitude.HasValue || validationEntity.Latitude.HasValue)
                && locationType != LocationType.Building 
                && locationType != LocationType.Place)
            {
                yield return new BusinessValidationResult<EcaLocation>(x => x.LocationTypeId, LATITUDE_LONGITUDE_SPECIFIED_BUT_INVALID_LOCATION_TYPE);
            }
        }

        /// <summary>
        /// Returns validation results related to location relationships.
        /// </summary>
        /// <param name="validationEntity">The validation entity.</param>
        /// <returns>The validation results.</returns>
        public IEnumerable<BusinessValidationResult> GetLocationValidations(LocationValidationEntity validationEntity)
        {
            if (validationEntity.City != null
                && validationEntity.Division != null
                && validationEntity.City.DivisionId != validationEntity.Division.LocationId)
            {
                yield return new BusinessValidationResult<EcaLocation>(x => x.CityId, CITY_MUST_BELONG_TO_DIVISION);
            }
            if (validationEntity.Division != null
                && validationEntity.Country != null
                && validationEntity.Division.CountryId != validationEntity.Country.LocationId)
            {
                yield return new BusinessValidationResult<EcaLocation>(x => x.DivisionId, DIVISION_MUST_BELONG_COUNTRY);
            }
        }

        private StaticLookup GetLocationType(LocationValidationEntity entity)
        {
            return LocationType.GetStaticLookup(entity.LocationTypeId);
        }
    }
}
