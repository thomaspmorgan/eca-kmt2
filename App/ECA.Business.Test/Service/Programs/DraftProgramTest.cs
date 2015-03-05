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
            var focusId = 100;
            var website = "http://www.google.com";
            var goalIds = new List<int> { 10 };
            var themeIds = new List<int> { 20 };
            var pointOfContactIds = new List<int> { 30 };
            var regionIds = new List<int> { 40 };

            var user = new User(userId);
            var program = new DraftProgram(
                createdBy: user,
                name: name,
                description: description,
                startDate: startDate,
                endDate: endDate,
                ownerOrganizationId: ownerOrganizationId,
                parentProgramId: parentProgramId,
                focusId: focusId,
                website: website,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds,
                regionIds: regionIds
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
            Assert.AreEqual(focusId, program.FocusId);
            Assert.AreEqual(website, program.Website);

            CollectionAssert.AreEqual(goalIds, program.GoalIds);
            CollectionAssert.AreEqual(themeIds, program.ThemeIds);
            CollectionAssert.AreEqual(pointOfContactIds, program.ContactIds);
            CollectionAssert.AreEqual(regionIds, program.RegionIds);
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
            var website = "http://www.google.com";

            var user = new User(userId);
            var program = new DraftProgram(
                createdBy: user,
                name: name,
                description: description,
                startDate: startDate,
                endDate: endDate,
                ownerOrganizationId: ownerOrganizationId,
                parentProgramId: parentProgramId,
                focusId: focusId,
                website: website,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );
            Assert.IsNotNull(program.GoalIds);
            Assert.IsNotNull(program.ThemeIds);
            Assert.IsNotNull(program.ContactIds);
            Assert.IsNotNull(program.RegionIds);
        }
    }
}
