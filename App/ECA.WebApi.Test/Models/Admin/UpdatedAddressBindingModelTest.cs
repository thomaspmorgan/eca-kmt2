using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Admin;
using ECA.Business.Service;
using ECA.Data;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class UpdatedAddressBindingModelTest
    {
        [TestMethod]
        public void TestToUpdatedEcaAddress()
        {
            var model = new UpdatedAddressBindingModel
            {
                AddressDisplayName = "display",
                AddressTypeId = AddressType.Business.Id,
                CityId = 1,
                CountryId = 2,
                DivisionId = 3,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
                AddressId = 10
            };
            var user = new User(1);
            var instance = model.ToUpdatedEcaAddress(user);
            Assert.AreEqual(model.AddressDisplayName, instance.AddressDisplayName);
            Assert.AreEqual(model.AddressTypeId, instance.AddressTypeId);
            Assert.AreEqual(model.CityId, instance.CityId);
            Assert.AreEqual(model.CountryId, instance.CountryId);
            Assert.AreEqual(model.DivisionId, instance.DivisionId);
            Assert.AreEqual(model.AddressDisplayName, instance.LocationName);
            Assert.AreEqual(model.PostalCode, instance.PostalCode);
            Assert.AreEqual(model.Street1, instance.Street1);
            Assert.AreEqual(model.Street2, instance.Street2);
            Assert.AreEqual(model.Street3, instance.Street3);
            Assert.AreEqual(model.AddressId, instance.AddressId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Update.User));
        }
    }
}
