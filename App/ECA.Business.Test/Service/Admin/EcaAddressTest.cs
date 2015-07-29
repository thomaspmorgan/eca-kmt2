using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.Data;
using ECA.Business.Service.Admin;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class EcaAddressTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var userId = 1;
            var user = new User(userId);
            var addressTypeId = AddressType.Business.Id;
            var isPrimary = true;
            var street1 = "street1";
            var street2 = "street2";
            var street3 = "street3";
            var postalCode = "12345";
            var locationName = "location name";
            var countryId = 2;
            var cityId = 3;
            var divisionId = 4;

            var instance = new EcaAddress(
                addressTypeId,
                isPrimary,
                street1,
                street2,
                street3,
                postalCode,
                locationName,
                countryId,
                cityId,
                divisionId
                );
            Assert.AreEqual(addressTypeId, instance.AddressTypeId);
            Assert.AreEqual(isPrimary, instance.IsPrimary);
            Assert.AreEqual(street1, instance.Street1);
            Assert.AreEqual(street2, instance.Street2);
            Assert.AreEqual(street3, instance.Street3);
            Assert.AreEqual(postalCode, instance.PostalCode);
            Assert.AreEqual(street3, instance.Street3);
            Assert.AreEqual(locationName, instance.LocationName);
            Assert.AreEqual(countryId, instance.CountryId);
            Assert.AreEqual(cityId, instance.CityId);
            Assert.AreEqual(divisionId, instance.DivisionId);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownStaticLookupException))]
        public void TestConstructor_UnknownAddressType()
        {
            var userId = 1;
            var user = new User(userId);
            var addressTypeId = -1;
            var isPrimary = true;
            var street1 = "street1";
            var street2 = "street2";
            var street3 = "street3";
            var postalCode = "12345";
            var locationName = "location name";
            var countryId = 2;
            var cityId = 3;
            var divisionId = 4;

            var instance = new EcaAddress(
                addressTypeId,
                isPrimary,
                street1,
                street2,
                street3,
                postalCode,
                locationName,
                countryId,
                cityId,
                divisionId
                );
        }
    }
}
