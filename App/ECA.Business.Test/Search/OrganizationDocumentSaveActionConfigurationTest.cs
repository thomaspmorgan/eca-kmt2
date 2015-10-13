using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using ECA.Data;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class OrganizationDocumentSaveActionConfigurationTest
    {
        [TestMethod]
        public void TestCreatedExclusionRules()
        {
            Assert.AreEqual(3, Organization.OFFICE_ORGANIZATION_TYPE_IDS.Count());
            var instance = new OrganizationDocumentSaveActionConfiguration();
            Assert.IsNotNull(instance.CreatedExclusionRules);
            Assert.AreEqual(1, instance.CreatedExclusionRules.Count());

            var fei = new Organization
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id
            };
            var office = new Organization
            {
                OrganizationTypeId = OrganizationType.Office.Id
            };
            var branch = new Organization
            {
                OrganizationTypeId = OrganizationType.Branch.Id
            };
            var division = new Organization
            {
                OrganizationTypeId = OrganizationType.Division.Id
            };
            Assert.IsTrue(instance.CreatedExclusionRules.First()(office));
            Assert.IsTrue(instance.CreatedExclusionRules.First()(branch));
            Assert.IsTrue(instance.CreatedExclusionRules.First()(division));

            Assert.IsFalse(instance.CreatedExclusionRules.First()(fei));
        }

        [TestMethod]
        public void TestModifiedExclusionRules()
        {
            Assert.AreEqual(3, Organization.OFFICE_ORGANIZATION_TYPE_IDS.Count());
            var instance = new OrganizationDocumentSaveActionConfiguration();
            Assert.IsNotNull(instance.ModifiedExclusionRules);
            Assert.AreEqual(1, instance.ModifiedExclusionRules.Count());

            var fei = new Organization
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id
            };
            var office = new Organization
            {
                OrganizationTypeId = OrganizationType.Office.Id
            };
            var branch = new Organization
            {
                OrganizationTypeId = OrganizationType.Branch.Id
            };
            var division = new Organization
            {
                OrganizationTypeId = OrganizationType.Division.Id
            };
            Assert.IsTrue(instance.ModifiedExclusionRules.First()(office));
            Assert.IsTrue(instance.ModifiedExclusionRules.First()(branch));
            Assert.IsTrue(instance.ModifiedExclusionRules.First()(division));

            Assert.IsFalse(instance.ModifiedExclusionRules.First()(fei));
        }

        [TestMethod]
        public void TestDeletedExclusionRules()
        {
            Assert.AreEqual(3, Organization.OFFICE_ORGANIZATION_TYPE_IDS.Count());
            var instance = new OrganizationDocumentSaveActionConfiguration();
            Assert.IsNotNull(instance.DeletedExclusionRules);
            Assert.AreEqual(1, instance.DeletedExclusionRules.Count());

            var fei = new Organization
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id
            };
            var office = new Organization
            {
                OrganizationTypeId = OrganizationType.Office.Id
            };
            var branch = new Organization
            {
                OrganizationTypeId = OrganizationType.Branch.Id
            };
            var division = new Organization
            {
                OrganizationTypeId = OrganizationType.Division.Id
            };
            Assert.IsTrue(instance.DeletedExclusionRules.First()(office));
            Assert.IsTrue(instance.DeletedExclusionRules.First()(branch));
            Assert.IsTrue(instance.DeletedExclusionRules.First()(division));

            Assert.IsFalse(instance.DeletedExclusionRules.First()(fei));
        }

        [TestMethod]
        public void TestGetId()
        {
            var organization = new Organization
            {
                OrganizationId = 10
            };
            var instance = new OrganizationDocumentSaveActionConfiguration();
            Assert.AreEqual(organization.OrganizationId, instance.GetId(organization));
        }

        [TestMethod]
        public void TestDocumentTypeId()
        {
            var organization = new Organization
            {
                OrganizationId = 10
            };
            var instance = new OrganizationDocumentSaveActionConfiguration();
            Assert.AreEqual(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, instance.GetDocumentTypeId(organization));
        }
    }
}
