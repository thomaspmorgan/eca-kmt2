using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class LocationServiceValidatorTest
    {

        #region DoValidateCreate
        [TestMethod]
        public void TestDoValidateCreate_NullLocationName()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            locationName = null;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("LocationName", error.Property);
            Assert.AreEqual(LocationServiceValidator.INVALID_LOCATION_NAME, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_EmptyLocationName()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            locationName = String.Empty;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("LocationName", error.Property);
            Assert.AreEqual(LocationServiceValidator.INVALID_LOCATION_NAME, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_WhitespaceLocationName()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            locationName = " ";
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("LocationName", error.Property);
            Assert.AreEqual(LocationServiceValidator.INVALID_LOCATION_NAME, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_IsAddressType()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            locationTypeId = LocationType.Address.Id;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("LocationTypeId", error.Property);
            Assert.AreEqual(LocationServiceValidator.LOCATION_MUST_NOT_BE_AN_ADDRESS_ERROR_MESSAGE, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_CityDoesNotHaveCountryDefined()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            country = null;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("CityId", error.Property);
            Assert.AreEqual(LocationServiceValidator.CITY_MUST_HAVE_COUNTRY, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_BuildingDoesNotHaveCountry()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            locationTypeId = LocationType.Building.Id;
            country = null;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("CountryId", error.Property);
            Assert.AreEqual(LocationServiceValidator.BUILDING_MUST_HAVE_COUNTRY, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_PlaceDoesNotHaveCountry()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            locationTypeId = LocationType.Place.Id;
            country = null;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("CountryId", error.Property);
            Assert.AreEqual(LocationServiceValidator.PLACE_MUST_HAVE_COUNTRY, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_LongitudeDefinedButNotABuilding()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            longitude = 1.0f;
            locationTypeId = LocationType.City.Id;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("LocationTypeId", error.Property);
            Assert.AreEqual(LocationServiceValidator.LATITUDE_LONGITUDE_SPECIFIED_BUT_INVALID_LOCATION_TYPE, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_LatitudeDefinedButNotABuilding()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            latitude = 1.0f;
            locationTypeId = LocationType.City.Id;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("LocationTypeId", error.Property);
            Assert.AreEqual(LocationServiceValidator.LATITUDE_LONGITUDE_SPECIFIED_BUT_INVALID_LOCATION_TYPE, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_LatitudeDefinedIsAPlace()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            latitude = 1.0f;
            locationTypeId = LocationType.Place.Id;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());
        }

        [TestMethod]
        public void TestDoValidateCreate_LongitudeDefinedIsAPlace()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            longitude = 1.0f;
            locationTypeId = LocationType.Place.Id;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());
        }

        [TestMethod]
        public void TestDoValidateCreate_LatitudeDefinedIsABuilding()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            latitude = 1.0f;
            locationTypeId = LocationType.Building.Id;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());
        }

        [TestMethod]
        public void TestDoValidateCreate_LongitudeDefinedIsABuilding()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            longitude = 1.0f;
            locationTypeId = LocationType.Building.Id;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());
        }

        [TestMethod]
        public void TestDoValidateCreate_CityAndDivisionNotEqual()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            city.DivisionId = -1;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("CityId", error.Property);
            Assert.AreEqual(LocationServiceValidator.CITY_MUST_BELONG_TO_DIVISION, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_DivisionAndCountryNotEqual()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(0, errors.Count());

            division.CountryId = -1;
            errors = locationValidator.DoValidateCreate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("DivisionId", error.Property);
            Assert.AreEqual(LocationServiceValidator.DIVISION_MUST_BELONG_COUNTRY, error.ErrorMessage);
        }

        #endregion

        #region DoValidateUpdate
        [TestMethod]
        public void TestDoValidateUpdate_NullLocationName()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            locationName = null;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("LocationName", error.Property);
            Assert.AreEqual(LocationServiceValidator.INVALID_LOCATION_NAME, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_EmptyLocationName()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            locationName = String.Empty;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("LocationName", error.Property);
            Assert.AreEqual(LocationServiceValidator.INVALID_LOCATION_NAME, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_WhitespaceLocationName()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            locationName = " ";
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("LocationName", error.Property);
            Assert.AreEqual(LocationServiceValidator.INVALID_LOCATION_NAME, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_IsAddressType()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            locationTypeId = LocationType.Address.Id;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("LocationTypeId", error.Property);
            Assert.AreEqual(LocationServiceValidator.LOCATION_MUST_NOT_BE_AN_ADDRESS_ERROR_MESSAGE, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_CityDoesNotHaveCountryDefined()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            country = null;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("CityId", error.Property);
            Assert.AreEqual(LocationServiceValidator.CITY_MUST_HAVE_COUNTRY, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_BuildingDoesNotHaveCountry()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            locationTypeId = LocationType.Building.Id;
            country = null;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("CountryId", error.Property);
            Assert.AreEqual(LocationServiceValidator.BUILDING_MUST_HAVE_COUNTRY, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_PlaceDoesNotHaveCountry()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            locationTypeId = LocationType.Place.Id;
            country = null;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("CountryId", error.Property);
            Assert.AreEqual(LocationServiceValidator.PLACE_MUST_HAVE_COUNTRY, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_LongitudeDefinedButNotABuilding()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            longitude = 1.0f;
            locationTypeId = LocationType.City.Id;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("LocationTypeId", error.Property);
            Assert.AreEqual(LocationServiceValidator.LATITUDE_LONGITUDE_SPECIFIED_BUT_INVALID_LOCATION_TYPE, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_LatitudeDefinedButNotABuilding()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            latitude = 1.0f;
            locationTypeId = LocationType.City.Id;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("LocationTypeId", error.Property);
            Assert.AreEqual(LocationServiceValidator.LATITUDE_LONGITUDE_SPECIFIED_BUT_INVALID_LOCATION_TYPE, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_LatitudeDefinedIsAPlace()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            latitude = 1.0f;
            locationTypeId = LocationType.Place.Id;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());
        }

        [TestMethod]
        public void TestDoValidateUpdate_LongitudeDefinedIsAPlace()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            longitude = 1.0f;
            locationTypeId = LocationType.Place.Id;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());
        }

        [TestMethod]
        public void TestDoValidateUpdate_LatitudeDefinedIsABuilding()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            latitude = 1.0f;
            locationTypeId = LocationType.Building.Id;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());
        }

        [TestMethod]
        public void TestDoValidateUpdate_LongitudeDefinedIsABuilding()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            longitude = 1.0f;
            locationTypeId = LocationType.Building.Id;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());
        }

        [TestMethod]
        public void TestDoValidateUpdate_CityAndDivisionNotEqual()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            city.DivisionId = -1;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("CityId", error.Property);
            Assert.AreEqual(LocationServiceValidator.CITY_MUST_BELONG_TO_DIVISION, error.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_DivisionAndCountryNotEqual()
        {
            var locationName = "location name";
            var locationTypeId = LocationType.City.Id;
            var cityId = 1;
            var countryId = 2;
            var divisionId = 3;
            float? latitude = null;
            float? longitude = null;
            var country = new Location
            {
                LocationId = countryId,
                LocationTypeId = LocationType.Country.Id,
            };
            var division = new Location
            {
                LocationId = divisionId,
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };
            var city = new Location
            {
                Division = division,
                DivisionId = division.LocationId,
                LocationId = cityId,
                LocationTypeId = LocationType.City.Id,
            };
            Func<LocationValidationEntity> createEntity = () =>
            {
                var ecaLocation = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
                return new LocationValidationEntity(ecaLocation, country, division, city);
            };

            var locationValidator = new LocationServiceValidator();

            var errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(0, errors.Count());

            division.CountryId = -1;
            errors = locationValidator.DoValidateUpdate(createEntity());
            Assert.AreEqual(1, errors.Count());
            var error = errors.First();
            Assert.AreEqual("DivisionId", error.Property);
            Assert.AreEqual(LocationServiceValidator.DIVISION_MUST_BELONG_COUNTRY, error.ErrorMessage);
        }

        #endregion
    }
}
