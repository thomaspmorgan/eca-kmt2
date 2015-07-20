using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Business.Service;
using ECA.Data;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class AdditionalOrganizationAddressTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var userId = 1;
            var user = new User(userId);
            var addressTypeId = AddressType.Business.Id;
            var displayName = "display";
            var street1 = "street1";
            var street2 = "street2";
            var street3 = "street3";
            var postalCode = "12345";
            var locationName = "location name";
            var countryId = 2;
            var cityId = 3;
            var divisionId = 4;
            var organizationId = 5;

            var instance = new AdditionalOrganizationAddress(
                user,
                addressTypeId,
                displayName,
                street1,
                street2,
                street3,
                postalCode,
                locationName,
                countryId,
                cityId,
                divisionId,
                organizationId
                );
            Assert.AreEqual(organizationId, instance.GetAddressableEntityId());
            //ensure base constructor called
            Assert.AreEqual(street1, instance.Street1);
        }
    }
}
