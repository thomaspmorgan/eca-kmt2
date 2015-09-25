using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.Data;
using System.Collections.Generic;
using ECA.Business.Service.Admin;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class EcaOrganizationTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var userId = 1;
            var user = new User(userId);
            var website = "http://www.google.com";
            var organizationTypeId = OrganizationType.ForeignEducationalInstitution.Id;
            var contactIds = new List<int>();
            var organizationRoleIds = new List<int>();
            var parentOrgId = 2;
            var name = "name";
            var description = "description";
            var organizationId = 3;

            var instance = new EcaOrganization(user, organizationId, website, organizationTypeId, organizationRoleIds, contactIds, parentOrgId, name, description);
            Assert.IsNotNull(instance.Update);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Update.User));
            Assert.AreEqual(website, instance.Website);
            Assert.AreEqual(parentOrgId, instance.ParentOrganizationId);
            Assert.AreEqual(name, instance.Name);
            Assert.AreEqual(description, instance.Description);
            Assert.AreEqual(organizationId, instance.OrganizationId);
            Assert.IsTrue(Object.ReferenceEquals(contactIds, instance.ContactIds));
        }

        [TestMethod]
        public void TestConstructor_ContactIdsIsNull()
        {
            var userId = 1;
            var user = new User(userId);
            var website = "http://www.google.com";
            var organizationTypeId = OrganizationType.ForeignEducationalInstitution.Id;
            var organizationRoleIds = new List<int>();
            var parentOrgId = 2;
            var name = "name";
            var description = "description";
            var organizationId = 3;

            List<int> contactIds = null;

            var instance = new EcaOrganization(user, organizationId, website, organizationTypeId, organizationRoleIds, contactIds, parentOrgId, name, description);
            Assert.IsNotNull(instance.ContactIds);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownStaticLookupException))]
        public void TestConstructor_UnknownOrganizationTypeId()
        {
            var userId = 1;
            var user = new User(userId);
            var website = "http://www.google.com";
            var organizationTypeId = -1;
            var organizationRoleIds = new List<int>();
            var parentOrgId = 2;
            var name = "name";
            var description = "description";
            var organizationId = 3;
            List<int> contactIds = null;

            var instance = new EcaOrganization(user, organizationId, website, organizationTypeId, organizationRoleIds, contactIds, parentOrgId, name, description);
        }
    }
}
