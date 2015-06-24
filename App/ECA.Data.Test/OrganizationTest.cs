using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Data;
using System.Collections.Generic;
using ECA.Core.Generation;
using System.Reflection;

namespace ECA.Data.Test
{
    [TestClass]
    public class OrganizationTest
    {
        [TestMethod]
        public void TestOrganization_OfficeTypeIds()
        {
            var expectedOfficeTypeIds = new int[] { OrganizationType.Office.Id, OrganizationType.Division.Id, OrganizationType.Branch.Id };
            var officeTypeIds = Organization.OFFICE_ORGANIZATION_TYPE_IDS;
            CollectionAssert.AreEqual(expectedOfficeTypeIds, officeTypeIds);
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
