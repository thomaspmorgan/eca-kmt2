using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using ECA.Data;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class OfficeDocumentSaveActionConfigurationTest
    {
        [TestMethod]
        public void TestCreatedExclusionRules()
        {
            Assert.AreEqual(3, Organization.OFFICE_ORGANIZATION_TYPE_IDS.Count());
            var instance = new OfficeDocumentSaveActionConfiguration();
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
            Assert.IsFalse(instance.CreatedExclusionRules.First()(office));
            Assert.IsFalse(instance.CreatedExclusionRules.First()(branch));
            Assert.IsFalse(instance.CreatedExclusionRules.First()(division));

            Assert.IsTrue(instance.CreatedExclusionRules.First()(fei));
        }

        [TestMethod]
        public void TestModifiedExclusionRules()
        {
            Assert.AreEqual(3, Organization.OFFICE_ORGANIZATION_TYPE_IDS.Count());
            var instance = new OfficeDocumentSaveActionConfiguration();
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
            Assert.IsFalse(instance.ModifiedExclusionRules.First()(office));
            Assert.IsFalse(instance.ModifiedExclusionRules.First()(branch));
            Assert.IsFalse(instance.ModifiedExclusionRules.First()(division));

            Assert.IsTrue(instance.ModifiedExclusionRules.First()(fei));
        }

        [TestMethod]
        public void TestDeletedExclusionRules()
        {
            Assert.AreEqual(3, Organization.OFFICE_ORGANIZATION_TYPE_IDS.Count());
            var instance = new OfficeDocumentSaveActionConfiguration();
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
            Assert.IsFalse(instance.DeletedExclusionRules.First()(office));
            Assert.IsFalse(instance.DeletedExclusionRules.First()(branch));
            Assert.IsFalse(instance.DeletedExclusionRules.First()(division));

            Assert.IsTrue(instance.DeletedExclusionRules.First()(fei));
        }
    }
}
