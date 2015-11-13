using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class EcaAddressValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var addressTypeId = AddressType.Home.Id;
            var country = new Location
            {
                IsActive = true
            };
            var division = new Location
            {
                IsActive = true
            };
            var city = new Location
            {
                IsActive = true
            };
            var entity = new EcaAddressValidationEntity(addressTypeId: addressTypeId, country: country, division: division, city: city);
            Assert.IsTrue(Object.ReferenceEquals(country, entity.Country));
            Assert.IsTrue(Object.ReferenceEquals(division, entity.Division));
            Assert.IsTrue(Object.ReferenceEquals(city, entity.City));
            Assert.AreEqual(addressTypeId, entity.AddressTypeId);
        }
    }
}
