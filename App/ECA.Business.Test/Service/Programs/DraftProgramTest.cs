using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Models.Programs;
using ECA.Data;
using System.Collections.Generic;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class DraftProgramTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var userId = 1;
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var goalIds = new List<int> { 10 };
            var themeIds = new List<int> { 20 };
            var pointOfContactIds = new List<int> { 30 };
            var regionIds = new List<int> { 40 };
            var categoryIds = new List<int> { 50 };
            var objectiveIds = new List<int> { 60 };
            var websites = new List<string> { "http://www.google.com" };

            var user = new User(userId);
            var program = new DraftProgram(
                createdBy: user,
                name: name,
                description: description,
                startDate: startDate,
                endDate: endDate,
                ownerOrganizationId: ownerOrganizationId,
                parentProgramId: parentProgramId,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds,
                regionIds: regionIds,
                categoryIds: categoryIds,
                objectiveIds: objectiveIds,
                websites: websites

                );
            Assert.AreEqual(user, program.Audit.User);
            DateTimeOffset.UtcNow.Should().BeCloseTo(program.Audit.Date, DbContextHelper.DATE_PRECISION);

            Assert.AreEqual(name, program.Name);
            Assert.AreEqual(description, program.Description);
            Assert.AreEqual(startDate, program.StartDate);
            Assert.AreEqual(endDate, program.EndDate);
            Assert.AreEqual(ownerOrganizationId, program.OwnerOrganizationId);
            Assert.AreEqual(parentProgramId, program.ParentProgramId);
            Assert.AreEqual(ProgramStatus.Draft.Id, program.ProgramStatusId);
            Assert.IsNull(program.RowVersion);

            CollectionAssert.AreEqual(goalIds, program.GoalIds);
            CollectionAssert.AreEqual(themeIds, program.ThemeIds);
            CollectionAssert.AreEqual(pointOfContactIds, program.ContactIds);
            CollectionAssert.AreEqual(regionIds, program.RegionIds);
            CollectionAssert.AreEqual(websites, program.Websites);
        }

        [TestMethod]
        public void TestConstructor_NullListsOfIds()
        {
            var userId = 1;
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = ProgramStatus.Active.Id;
            var focusId = 100;

            var user = new User(userId);
            var program = new DraftProgram(
                createdBy: user,
                name: name,
                description: description,
                startDate: startDate,
                endDate: endDate,
                ownerOrganizationId: ownerOrganizationId,
                parentProgramId: parentProgramId,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                categoryIds: null,
                objectiveIds: null,
                websites: null
                );
            Assert.IsNotNull(program.GoalIds);
            Assert.IsNotNull(program.ThemeIds);
            Assert.IsNotNull(program.ContactIds);
            Assert.IsNotNull(program.RegionIds);
            Assert.IsNotNull(program.Websites);
        }
    }
}
