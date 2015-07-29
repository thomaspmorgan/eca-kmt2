using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Admin;
using ECA.Data;
using ECA.Business.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
                IsPrimary = true,
                AddressTypeId = AddressType.Business.Id,
                CityId = 1,
                CountryId = 2,
                DivisionId = 3,
                Id = 4,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var user = new User(1);
            var instance = model.ToAdditionalAddress(user);
            Assert.AreEqual(model.IsPrimary, instance.IsPrimary);
            Assert.AreEqual(model.AddressTypeId, instance.AddressTypeId);
            Assert.AreEqual(model.CityId, instance.CityId);
            Assert.AreEqual(model.CountryId, instance.CountryId);
            Assert.AreEqual(model.DivisionId, instance.DivisionId);
            Assert.IsNull(instance.LocationName);
            Assert.AreEqual(model.PostalCode, instance.PostalCode);
            Assert.AreEqual(model.Street1, instance.Street1);
            Assert.AreEqual(model.Street2, instance.Street2);
            Assert.AreEqual(model.Street3, instance.Street3);
            Assert.AreEqual(model.Id, instance.GetAddressableEntityId());
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Create.User));
        }

        [TestMethod]
        public void TestStreet1_MaxLength()
        {
            var model = new OrganizationAddressBindingModel
            {
                IsPrimary = true,
                AddressTypeId = AddressType.Business.Id,
                CityId = 1,
                CountryId = 2,
                DivisionId = 3,
                Id = 4,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(model, new ValidationContext(model), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            model.Street1 = new String('a', Location.STREET_MAX_LENGTH + 1);

            actual = Validator.TryValidateObject(model, new ValidationContext(model), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Street1", results.First().MemberNames.First());
        }

        [TestMethod]
        public void TestStreet2_MaxLength()
        {
            var model = new OrganizationAddressBindingModel
            {
                IsPrimary = true,
                AddressTypeId = AddressType.Business.Id,
                CityId = 1,
                CountryId = 2,
                DivisionId = 3,
                Id = 4,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(model, new ValidationContext(model), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            model.Street2 = new String('a', Location.STREET_MAX_LENGTH + 1);

            actual = Validator.TryValidateObject(model, new ValidationContext(model), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Street2", results.First().MemberNames.First());
        }

        [TestMethod]
        public void TestStreet3_MaxLength()
        {
            var model = new OrganizationAddressBindingModel
            {
                IsPrimary = true,
                AddressTypeId = AddressType.Business.Id,
                CityId = 1,
                CountryId = 2,
                DivisionId = 3,
                Id = 4,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(model, new ValidationContext(model), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            model.Street3 = new String('a', Location.STREET_MAX_LENGTH + 1);

            actual = Validator.TryValidateObject(model, new ValidationContext(model), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Street3", results.First().MemberNames.First());
        }
    }
}
