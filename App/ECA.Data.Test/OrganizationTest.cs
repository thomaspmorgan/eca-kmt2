using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Data;
using System.Collections.Generic;
using ECA.Core.Generation;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace ECA.Data.Test
{
    [TestClass]
    public class OrganizationTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestOrganization_OfficeTypeIds()
        {
            var expectedOfficeTypeIds = new int[] { OrganizationType.Office.Id, OrganizationType.Division.Id, OrganizationType.Branch.Id };
            var officeTypeIds = Organization.OFFICE_ORGANIZATION_TYPE_IDS;
            CollectionAssert.AreEqual(expectedOfficeTypeIds, officeTypeIds);
        }
        [TestMethod]
        public void TestGetId()
        {
            var org = new Organization
            {
                OrganizationId = 1
            };
            Assert.AreEqual(org.OrganizationId, org.GetId());
        }

        [TestMethod]
        public void TestNameMaxLength()
        {
            var org = new Organization
            {
                Name = new String('a', Organization.NAME_MAX_LENGTH),
                OrganizationType = new OrganizationType(),
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
            var org = new Organization
            {
                Name = new String('a', Organization.NAME_MAX_LENGTH),
                OrganizationType = new OrganizationType(),
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
            var org = new Organization
            {
                Name = "a",
                OrganizationType = new OrganizationType(),
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
            var org = new Organization
            {
                Name = "a",
                OrganizationType = new OrganizationType(),
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
            var org = new Organization
            {
                Name = "a",
                OrganizationType = new OrganizationType(),
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

        #region Permissable
        [TestMethod]
        public void TestIsExempt()
        {
            var org = new Organization
            {
                OrganizationId = 1
            };
            foreach (var officeTypeId in Organization.OFFICE_ORGANIZATION_TYPE_IDS)
            {
                org.OrganizationTypeId = officeTypeId;
                var permissable = org as IPermissable;
                Assert.IsFalse(permissable.IsExempt());
            }
            var orgType = typeof(OrganizationType);
            var instance = (OrganizationType)Activator.CreateInstance(orgType);
            var staticProperties = orgType.GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allStaticLookups = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            var nonOfficeOrgTypeIds = allStaticLookups.Where(x => !Organization.OFFICE_ORGANIZATION_TYPE_IDS.Contains(x.Id)).Select(x => x.Id).ToList();

            foreach (var nonOfficeOrgTypeId in nonOfficeOrgTypeIds)
            {
                org.OrganizationTypeId = nonOfficeOrgTypeId;
                var permissable = org as IPermissable;
                Assert.IsTrue(org.IsExempt());
            }
        }

        [TestMethod]
        public void TestGetId_Permissable()
        {
            var org = new Organization
            {
                OrganizationId = 1
            };
            var permissable = org as IPermissable;
            Assert.AreEqual(org.OrganizationId, permissable.GetId());
        }

        [TestMethod]
        public void TestGetPermissableType()
        {
            var org = new Organization
            {
                OrganizationId = 1
            };
            var permissable = org as IPermissable;
            Assert.AreEqual(PermissableType.Office, permissable.GetPermissableType());
        }

        [TestMethod]
        public void TestGetParentPermissableType()
        {
            var org = new Organization
            {
                OrganizationId = 1
            };
            var permissable = org as IPermissable;

            org.Invoking(x => x.GetParentPermissableType()).ShouldThrow<NotSupportedException>().WithMessage("The organization does not have a parent permissable type.");
        }

        [TestMethod]
        public void TestParentId_Permissable()
        {
            var org = new Organization
            {
                OrganizationId = 1
            };
            var permissable = org as IPermissable;
            Assert.IsFalse(permissable.GetParentId().HasValue);
        }

        #endregion
    }
}
