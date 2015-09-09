using ECA.Business.Exceptions;
using System.Linq;
using ECA.Business.Models.Programs;
using ECA.Business.Service;
using ECA.Core.Exceptions;
using ECA.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class EcaProgramTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var userId = 1;
            var programId = 10;
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = ProgramStatus.Active.Id;
            var goalIds = new List<int> { 10 };
            var themeIds = new List<int> { 20 };
            var pointOfContactIds = new List<int> { 30 };
            var regionIds = new List<int> { 40 };
            var categoryIds = new List<int> { 50};
            var objectiveIds = new List<int> { 60};
            var rowVersion = new byte[1] { (byte)1 };

            var user = new User(userId);
            var program = new EcaProgram(
                updatedBy: user,
                id: programId,
                name: name,
                description: description,
                startDate: startDate,
                endDate: endDate,
                ownerOrganizationId: ownerOrganizationId,
                parentProgramId: parentProgramId,
                programStatusId: programStatusId,
                programRowVersion: rowVersion,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds,
                regionIds: regionIds,
                categoryIds: categoryIds,
                objectiveIds: objectiveIds);
            Assert.AreEqual(user, program.Audit.User);
            DateTimeOffset.UtcNow.Should().BeCloseTo(program.Audit.Date, DbContextHelper.DATE_PRECISION);

            Assert.AreEqual(programId, program.Id);
            Assert.AreEqual(name, program.Name);
            Assert.AreEqual(description, program.Description);
            Assert.AreEqual(startDate, program.StartDate);
            Assert.AreEqual(endDate, program.EndDate);
            Assert.AreEqual(parentProgramId, program.ParentProgramId);
            Assert.AreEqual(ownerOrganizationId, program.OwnerOrganizationId);
            Assert.AreEqual(programStatusId, program.ProgramStatusId);

            CollectionAssert.AreEqual(goalIds, program.GoalIds);
            CollectionAssert.AreEqual(themeIds, program.ThemeIds);
            CollectionAssert.AreEqual(pointOfContactIds, program.ContactIds);
            CollectionAssert.AreEqual(regionIds, program.RegionIds);
            CollectionAssert.AreEqual(rowVersion, program.RowVersion);
        }

        [TestMethod]
        public void TestConstructor_DistinctListOfIds()
        {
            var userId = 1;
            var programId = 10;
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = ProgramStatus.Active.Id;
            var goalIds = new List<int> { 10, 10 };
            var themeIds = new List<int> { 20, 20 };
            var pointOfContactIds = new List<int> { 30, 30 };
            var regionIds = new List<int> { 40, 40 };
            var categoryIds = new List<int> { 50, 50 };
            var objectiveIds = new List<int> { 60, 60 };
            var rowVersion = new byte[1] { (byte)1 };

            var user = new User(userId);
            var program = new EcaProgram(
                updatedBy: user,
                id: programId,
                name: name,
                description: description,
                startDate: startDate,
                endDate: endDate,
                ownerOrganizationId: ownerOrganizationId,
                parentProgramId: parentProgramId,
                programStatusId: programStatusId,
                programRowVersion: rowVersion,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
                );


            CollectionAssert.AreEqual(goalIds.Distinct().ToList(), program.GoalIds);
            CollectionAssert.AreEqual(themeIds.Distinct().ToList(), program.ThemeIds);
            CollectionAssert.AreEqual(pointOfContactIds.Distinct().ToList(), program.ContactIds);
            CollectionAssert.AreEqual(regionIds.Distinct().ToList(), program.RegionIds);
        }

        [TestMethod]
        public void TestConstructor_NullListsOfIds()
        {
            var userId = 1;
            var programId = 10;
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = ProgramStatus.Active.Id;
            var focusId = 1;

            var user = new User(userId);
            var program = new EcaProgram(
                updatedBy: user,
                id: programId,
                name: name,
                description: description,
                startDate: startDate,
                endDate: endDate,
                ownerOrganizationId: ownerOrganizationId,
                parentProgramId: parentProgramId,
                programStatusId: programStatusId,
                programRowVersion: new byte[0],
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
               regionIds: null,
               categoryIds: null,
               objectiveIds: null
                );
            Assert.IsNotNull(program.GoalIds);
            Assert.IsNotNull(program.ThemeIds);
            Assert.IsNotNull(program.ContactIds);
            Assert.IsNotNull(program.RegionIds);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownStaticLookupException))]
        public void TestConstructor_InvalidProgramStatusId() 
        {
            var userId = 1;
            var programId = 10;
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = -1;

            var user = new User(userId);
            var program = new EcaProgram(
                updatedBy: user,
                id: programId,
                name: name,
                description: description,
                startDate: startDate,
                endDate: endDate,
                ownerOrganizationId: ownerOrganizationId,
                parentProgramId: parentProgramId,
                programStatusId: programStatusId,
                programRowVersion: new byte[0],
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                categoryIds: null,
                objectiveIds: null
                );
        }
    }
}
