using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class LocationServiceAddressValidatorTest
    {
        #region DoValidateCreate
    
        
        [TestMethod]
        public void TestValidateCreate_InvalidAddressTypeId()
        {
            var addressTypeId = AddressType.Home.Id;
            var country = new Location
            {
                IsActive = true,
                LocationName = "country"
            };
            var division = new Location
            {
                IsActive = true,
                LocationName = "division"
            };
            var city = new Location
            {
                IsActive = true,
                LocationName = "city"
            };
            var postalCode = "12345";
            var entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);

            var validator = new LocationServiceAddressValidator();
            Assert.AreEqual(0, validator.ValidateCreate(entity).Count());

            addressTypeId = 0;
            entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);
            var results = validator.DoValidateCreate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("AddressTypeId", results.First().Property);
            Assert.AreEqual(LocationServiceAddressValidator.INVALID_ADDRESS_TYPE_MESSAGE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestValidateCreate_InactiveCountry()
        {
            var addressTypeId = AddressType.Home.Id;
            var country = new Location
            {
                IsActive = true,
                LocationName = "country"
            };
            var division = new Location
            {
                IsActive = true,
                LocationName = "division"
            };
            var city = new Location
            {
                IsActive = true,
                LocationName = "city"
            };
            var postalCode = "12345";
            var entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);

            var validator = new LocationServiceAddressValidator();
            Assert.AreEqual(0, validator.ValidateCreate(entity).Count());

            country.IsActive = false;
            entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);
            var results = validator.DoValidateCreate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("CountryId", results.First().Property);
            Assert.AreEqual(String.Format(LocationServiceAddressValidator.INACTIVE_COUNTRY_FORMAT_ERROR_MESSAGE, country.LocationName), results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestValidateCreate_InactiveDivision()
        {
            var addressTypeId = AddressType.Home.Id;
            var country = new Location
            {
                IsActive = true,
                LocationName = "country"
            };
            var division = new Location
            {
                IsActive = true,
                LocationName = "division"
            };
            var city = new Location
            {
                IsActive = true,
                LocationName = "city"
            };
            var postalCode = "12345";
            var entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);

            var validator = new LocationServiceAddressValidator();
            Assert.AreEqual(0, validator.ValidateCreate(entity).Count());

            division.IsActive = false;
            entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);
            var results = validator.DoValidateCreate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("DivisionId", results.First().Property);
            Assert.AreEqual(String.Format(LocationServiceAddressValidator.INACTIVE_DIVISION_FORMAT_ERROR_MESSAGE, division.LocationName), results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestValidateCreate_InactiveCity()
        {
            var addressTypeId = AddressType.Home.Id;
            var country = new Location
            {
                IsActive = true,
                LocationName = "country"
            };
            var division = new Location
            {
                IsActive = true,
                LocationName = "division"
            };
            var city = new Location
            {
                IsActive = true,
                LocationName = "city"
            };
            var postalCode = "12345";
            var entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);

            var validator = new LocationServiceAddressValidator();
            Assert.AreEqual(0, validator.ValidateCreate(entity).Count());

            city.IsActive = false;
            entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);
            var results = validator.DoValidateCreate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("CityId", results.First().Property);
            Assert.AreEqual(String.Format(LocationServiceAddressValidator.INACTIVE_CITY_FORMAT_ERROR_MESSAGE, city.LocationName), results.First().ErrorMessage);
        }

        #endregion

        #region DoValidateUpdate


        [TestMethod]
        public void TestValidateUpdate_InvalidAddressTypeId()
        {
            var addressTypeId = AddressType.Home.Id;
            var country = new Location
            {
                IsActive = true,
                LocationName = "country"
            };
            var division = new Location
            {
                IsActive = true,
                LocationName = "division"
            };
            var city = new Location
            {
                IsActive = true,
                LocationName = "city"
            };
            var postalCode = "12345";
            var entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);

            var validator = new LocationServiceAddressValidator();
            Assert.AreEqual(0, validator.ValidateCreate(entity).Count());

            addressTypeId = 0;
            entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);
            var results = validator.DoValidateUpdate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("AddressTypeId", results.First().Property);
            Assert.AreEqual(LocationServiceAddressValidator.INVALID_ADDRESS_TYPE_MESSAGE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestValidateUpdate_InactiveCountry()
        {
            var addressTypeId = AddressType.Home.Id;
            var country = new Location
            {
                IsActive = true,
                LocationName = "country"
            };
            var division = new Location
            {
                IsActive = true,
                LocationName = "division"
            };
            var city = new Location
            {
                IsActive = true,
                LocationName = "city"
            };
            var postalCode = "12345";
            var entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);

            var validator = new LocationServiceAddressValidator();
            Assert.AreEqual(0, validator.ValidateCreate(entity).Count());

            country.IsActive = false;
            entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);
            var results = validator.DoValidateUpdate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("CountryId", results.First().Property);
            Assert.AreEqual(String.Format(LocationServiceAddressValidator.INACTIVE_COUNTRY_FORMAT_ERROR_MESSAGE, country.LocationName), results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestValidateUpdate_InactiveDivision()
        {
            var addressTypeId = AddressType.Home.Id;
            var country = new Location
            {
                IsActive = true,
                LocationName = "country"
            };
            var division = new Location
            {
                IsActive = true,
                LocationName = "division"
            };
            var city = new Location
            {
                IsActive = true,
                LocationName = "city"
            };
            var postalCode = "12345";
            var entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);

            var validator = new LocationServiceAddressValidator();
            Assert.AreEqual(0, validator.ValidateCreate(entity).Count());

            division.IsActive = false;
            entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);
            var results = validator.DoValidateUpdate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("DivisionId", results.First().Property);
            Assert.AreEqual(String.Format(LocationServiceAddressValidator.INACTIVE_DIVISION_FORMAT_ERROR_MESSAGE, division.LocationName), results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestValidateUpdate_InactiveCity()
        {
            var addressTypeId = AddressType.Home.Id;
            var country = new Location
            {
                IsActive = true,
                LocationName = "country"
            };
            var division = new Location
            {
                IsActive = true,
                LocationName = "division"
            };
            var city = new Location
            {
                IsActive = true,
                LocationName = "city"
            };
            var postalCode = "12345";
            var entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);

            var validator = new LocationServiceAddressValidator();
            Assert.AreEqual(0, validator.ValidateCreate(entity).Count());

            city.IsActive = false;
            entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city, postalCode: postalCode);
            var results = validator.DoValidateUpdate(entity);
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("CityId", results.First().Property);
            Assert.AreEqual(String.Format(LocationServiceAddressValidator.INACTIVE_CITY_FORMAT_ERROR_MESSAGE, city.LocationName), results.First().ErrorMessage);
        }
        #endregion
    }
}
