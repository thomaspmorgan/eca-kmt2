using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Admin;
using System.Collections.Generic;
using ECA.Data;
using ECA.Business.Service;
using System.ComponentModel.DataAnnotations;
using ECA.Data.Test;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class UpdatedOrganizationBindingModelTest
    {
        private InMemoryEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new InMemoryEcaContext();
        }

        [TestMethod]
        public void TestToEcaOrganization()
        {
            var model = new UpdatedOrganizationBindingModel();
            model.Description = "desc";
            model.Name = "name";
            model.OrganizationId = 1;
            model.OrganizationTypeId = OrganizationType.USEducationalInstitution.Id;
            model.ParentOrganizationId = 3;
            model.PointsOfContactIds = new List<int>();
            model.Website = "website";
            var user = new User(1);

            var instance = model.ToEcaOrganization(user);
            Assert.AreEqual(model.Description, instance.Description);
            Assert.AreEqual(model.Name, instance.Name);
            Assert.AreEqual(model.OrganizationId, instance.OrganizationId);
            Assert.AreEqual(model.OrganizationTypeId, instance.OrganizationTypeId);
            Assert.AreEqual(model.ParentOrganizationId, instance.ParentOrganizationId);
            Assert.AreEqual(model.Website, instance.Website);
            CollectionAssert.AreEqual(model.PointsOfContactIds.ToList(), instance.ContactIds.ToList());
        }

        [TestMethod]
        public void TestNameMaxLength()
        {
            var org = new UpdatedOrganizationBindingModel
            {
                Name = new String('a', Organization.NAME_MAX_LENGTH),
                Description = "desc"
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(org, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(org, new ValidationContext(org), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            org.Name = new String('a', Organization.NAME_MAX_LENGTH + 1);

            actual = Validator.TryValidateObject(org, new ValidationContext(org), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());
        }

        [TestMethod]
        public void TestNameRequired()
        {
            var org = new UpdatedOrganizationBindingModel
            {
                Name = new String('a', Organization.NAME_MAX_LENGTH),
                Description = "desc"
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(org, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(org, new ValidationContext(org), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            org.Name = null;

            actual = Validator.TryValidateObject(org, new ValidationContext(org), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());
        }

        [TestMethod]
        public void TestDescriptionMaxLength()
        {
            var org = new UpdatedOrganizationBindingModel
            {
                Name = "a",
                Description = "desc"
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(org, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(org, new ValidationContext(org), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            org.Description = new String('a', Organization.DESCRIPTION_MAX_LENGTH + 1);

            actual = Validator.TryValidateObject(org, new ValidationContext(org), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Description", results.First().MemberNames.First());
        }

        [TestMethod]
        public void TestDescriptionRequired()
        {
            var org = new UpdatedOrganizationBindingModel
            {
                Name = "a",
                Description = "desc"
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(org, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(org, new ValidationContext(org), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            org.Description = null;

            actual = Validator.TryValidateObject(org, new ValidationContext(org), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Description", results.First().MemberNames.First());
        }

        [TestMethod]
        public void TestWebsiteMaxLength()
        {
            var org = new UpdatedOrganizationBindingModel
            {
                Name = "a",
                Description = "desc",
                Website = new String('a', Organization.WEBSITE_MAX_LENGTH)
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(org, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(org, new ValidationContext(org), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            org.Website = new String('a', Organization.WEBSITE_MAX_LENGTH + 1);

            actual = Validator.TryValidateObject(org, new ValidationContext(org), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Website", results.First().MemberNames.First());
        }

    }
}
