using ECA.Business.Exceptions;
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
            var focusId = 200;
            var website = "http://www.google.com";
            var goalIds = new List<int> { 10 };
            var themeIds = new List<int> { 20 };
            var pointOfContactIds = new List<int> { 30 };
            var regionIds = new List<int> { 40 };

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
                focusId: focusId,
                website: website,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds,
                regionIds: regionIds
                );
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
            var programId = 10;
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = ProgramStatus.Active.Id;
            var focusId = 1;
            var website = "http://www.google.com";

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
            var focusId = 100;
            var website = "http://www.google.com";

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
                focusId: focusId,
                website: website,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestNameRequired_EmptyName()
        {
            var userId = 1;
            var programId = 10;
            var name = String.Empty;
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = 1;
            var focusId = 100;
            var website = "http://www.google.com";

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
                focusId: focusId,
                website: website,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestNameRequired_NullName()
        {
            var userId = 1;
            var programId = 10;
            string name = null;
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = 1;
            var focusId = 100;
            var website = "http://www.google.com";

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
                focusId: focusId,
                website: website,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestNameRequired_WhitespaceName()
        {
            var userId = 1;
            var programId = 10;
            string name = " ";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = 1;
            var focusId = 100;
            var website = "http://www.google.com";

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
                focusId: focusId,
                website: website,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );

        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestConstructor_DescriptionNull()
        {
            var userId = 1;
            var programId = 10;
            var name = "name";
            string description = null;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = ProgramStatus.Active.Id;
            var focusId = 100;
            var website = "http://www.google.com";
            var goalIds = new List<int> { 10 };
            var themeIds = new List<int> { 20 };
            var pointOfContactIds = new List<int> { 30 };
            var regionIds = new List<int> { 40 };

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
                focusId: focusId,
                website: website,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds,
                regionIds: regionIds
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestConstructor_EmptyDescription()
        {
            var userId = 1;
            var programId = 10;
            var name = "name";
            string description = String.Empty;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = ProgramStatus.Active.Id;
            var focusId = 100;
            var website = "http://www.google.com";
            var goalIds = new List<int> { 10 };
            var themeIds = new List<int> { 20 };
            var pointOfContactIds = new List<int> { 30 };
            var regionIds = new List<int> { 40 };

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
                focusId: focusId,
                website: website,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds,
                regionIds: regionIds
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestConstructor_WhitespaceDescription()
        {
            var userId = 1;
            var programId = 10;
            var name = "name";
            string description = " ";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = ProgramStatus.Active.Id;
            var focusId = 100;
            var website = "http://www.google.com";
            var goalIds = new List<int> { 10 };
            var themeIds = new List<int> { 20 };
            var pointOfContactIds = new List<int> { 30 };
            var regionIds = new List<int> { 40 };

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
                focusId: focusId,
                website: website,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds,
                regionIds: regionIds
                );
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestConstructor_NullUpdatedByUser()
        {
            var programId = 10;
            var name = "name";
            string description = "desc";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var programStatusId = ProgramStatus.Active.Id;
            var focusId = 100;
            var website = "http://www.google.com";
            var goalIds = new List<int> { 10 };
            var themeIds = new List<int> { 20 };
            var pointOfContactIds = new List<int> { 30 };
            var regionIds = new List<int> { 40 };

            var program = new EcaProgram(
                updatedBy: null,
                id: programId,
                name: name,
                description: description,
                startDate: startDate,
                endDate: endDate,
                ownerOrganizationId: ownerOrganizationId,
                parentProgramId: parentProgramId,
                programStatusId: programStatusId,
                focusId: focusId,
                website: website,
                goalIds: goalIds,
                pointOfContactIds: pointOfContactIds,
                themeIds: themeIds,
                regionIds: regionIds
                );
        }

    }
}
