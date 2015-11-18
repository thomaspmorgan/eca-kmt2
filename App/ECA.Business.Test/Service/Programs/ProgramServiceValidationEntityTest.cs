using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Programs;
using System.Collections.Generic;
using ECA.Data;
using ECA.Business.Service.Admin;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class ProgramServiceValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var regionLocationTypeIds = new List<int> { 1 };
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var inactiveRegionids = new List<int> { 1, 1, 1 };
            var parentPrograms = new List<OrganizationProgramDTO>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 2;
            var parentProgram = new Program();
            var name = "hello";
            var description = "desc";
            var programId = 1;
            var officeSettings = new OfficeSettings();
            var entity = new ProgramServiceValidationEntity(
                programId,
                name,
                description, 
                regionLocationTypeIds,
                inactiveRegionids,
                contactIds,
                themeIds,
                goalIds,
                regionIds,
                categoryIds,
                objectiveIds,
                owner,
                officeSettings,
                parentProgramId,
                parentProgram,
                parentPrograms
                );

            Assert.IsTrue(Object.ReferenceEquals(regionLocationTypeIds, entity.RegionLocationTypeIds));
            Assert.IsTrue(Object.ReferenceEquals(contactIds, entity.ContactIds));
            Assert.IsTrue(Object.ReferenceEquals(themeIds, entity.ThemeIds));
            Assert.IsTrue(Object.ReferenceEquals(goalIds, entity.GoalIds));
            Assert.IsTrue(Object.ReferenceEquals(categoryIds, entity.CategoryIds));
            Assert.IsTrue(Object.ReferenceEquals(objectiveIds, entity.ObjectiveIds));
            Assert.IsTrue(Object.ReferenceEquals(owner, entity.OwnerOrganization));
            Assert.IsTrue(Object.ReferenceEquals(parentProgram, entity.ParentProgram));
            Assert.IsTrue(Object.ReferenceEquals(name, entity.Name));
            Assert.IsTrue(Object.ReferenceEquals(description, entity.Description));
            Assert.IsTrue(Object.ReferenceEquals(officeSettings, entity.OwnerOfficeSettings));
            Assert.IsTrue(Object.ReferenceEquals(parentPrograms, entity.ParentProgramParentPrograms));
            CollectionAssert.AreEqual(inactiveRegionids.Distinct().ToList(), entity.InactiveRegionIds.ToList());

            Assert.AreEqual(parentProgramId, entity.ParentProgramId);
            Assert.AreEqual(programId, entity.ProgramId);
        }

        [TestMethod]
        public void TestConstructor_NullParentProgramParentPrograms()
        {
            var regionLocationTypeIds = new List<int> { 1 };
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var inactiveRegionids = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 2;
            var parentProgram = new Program();
            var name = "hello";
            var description = "desc";
            var programId = 1;
            var officeSettings = new OfficeSettings();
            var entity = new ProgramServiceValidationEntity(
                programId,
                name,
                description,
                regionLocationTypeIds,
                inactiveRegionids,
                contactIds,
                themeIds,
                goalIds,
                regionIds,
                categoryIds,
                objectiveIds,
                owner,
                officeSettings,
                parentProgramId,
                parentProgram,
                null
                );

            Assert.IsNotNull(entity.ParentProgramParentPrograms);
            Assert.IsNotNull(entity.InactiveRegionIds);
        }
    }
}
