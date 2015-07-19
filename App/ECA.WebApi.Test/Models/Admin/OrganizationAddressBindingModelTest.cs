using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Admin;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class OrganizationAddressBindingModelTest
    {
        [TestMethod]
        public void TestToAdditionalAddress()
        {
            var model = new OrganizationAddressBindingModel
            {
                AddressDisplayName = "display",
                AddressTypeId = AddressType.Business.Id,
                CityId = 1,
                CountryId = 2,
                DivisionId = 3,
                LocationName = "location name",
                Id = 4,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var user = new User(1);
            var instance = model.ToAdditionalAddress(user);
            Assert.AreEqual(model.AddressDisplayName, instance.AddressDisplayName);
            Assert.AreEqual(model.AddressTypeId, instance.AddressTypeId);
            Assert.AreEqual(model.CityId, instance.CityId);
            Assert.AreEqual(model.CountryId, instance.CountryId);
            Assert.AreEqual(model.DivisionId, instance.DivisionId);
            Assert.AreEqual(model.LocationName, instance.LocationName);
            Assert.AreEqual(model.PostalCode, instance.PostalCode);
            Assert.AreEqual(model.Street1, instance.Street1);
            Assert.AreEqual(model.Street2, instance.Street2);
            Assert.AreEqual(model.Street3, instance.Street3);
            Assert.AreEqual(model.Id, instance.GetAddressableEntityId());
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Create.User));
        }
    }
}
