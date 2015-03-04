﻿using Moq;
using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class ProgramServiceTest
    {
        private TestEcaContext context;
        private ProgramService service;
        private Mock<ILocationService> locationServiceMock;
        private List<int> locationTypeIds;

        [TestInitialize]
        public void TestInit()
        {
            locationTypeIds = new List<int>();
            context = DbContextHelper.GetInMemoryContext();
            locationServiceMock = new Mock<ILocationService>();
            locationServiceMock.Setup(x => x.GetLocationTypeIds(It.IsAny<List<int>>())).Returns(locationTypeIds);
            locationServiceMock.Setup(x => x.GetLocationTypeIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(locationTypeIds);
            service = new ProgramService(context, locationServiceMock.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGetProgramById_CheckProperties_HasParentProgram()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;

            var contact = new Contact
            {
                ContactId = 100
            };
            var theme = new Theme
            {
                ThemeId = 2,
                ThemeName = "theme"
            };
            var goal = new Goal
            {
                GoalId = 4,
            };
            
            var country = new Location
            {
                LocationId = 500,
                LocationName = "country",
                LocationIso = "countryIso",
                LocationTypeId = LocationType.Country.Id,
            };
            var region = new Location
            {
                LocationName = "region",
                LocationId = 3,
                LocationIso = "locationIso",
                LocationTypeId = LocationType.Region.Id
            };
            country.Region = region;

            var parentProgram = new Program
            {
                ProgramId = 10,
            };
            var owner = new Organization
            {
                OrganizationId = 30,
                Description = "owner desc",
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "name",
                Description = "description",
                ParentProgram = parentProgram,
                StartDate = DateTimeOffset.UtcNow,
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
                Owner = owner
            };
            program.Contacts.Add(contact);
            program.Goals.Add(goal);
            program.Regions.Add(region);

            context.Organizations.Add(owner);
            context.Programs.Add(program);
            context.Contacts.Add(contact);
            context.Themes.Add(theme);
            context.Goals.Add(goal);
            context.Locations.Add(country);
            context.Programs.Add(parentProgram);
            context.Locations.Add(region);

            Action<ProgramDTO> tester = (publishedProgram) =>
            {
                CollectionAssert.AreEqual(program.Contacts.Select(x => x.ContactId).ToList(), publishedProgram.Contacts.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(program.Contacts.Select(x => x.FullName).ToList(), publishedProgram.Contacts.Select(x => x.Value).ToList());


                CollectionAssert.AreEqual(
                    context.Locations.Where(x => x.LocationTypeId == LocationType.Country.Id).Select(x => x.LocationId).ToList(),
                    publishedProgram.CountryIsos.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(
                    context.Locations.Where(x => x.LocationTypeId == LocationType.Country.Id).Select(x => x.LocationIso).ToList(), 
                    publishedProgram.CountryIsos.Select(x => x.Value).ToList());


                CollectionAssert.AreEqual(
                    context.Locations.Where(x => x.LocationTypeId == LocationType.Region.Id).Select(x => x.LocationId).ToList(),
                    publishedProgram.RegionIsos.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(
                    context.Locations.Where(x => x.LocationTypeId == LocationType.Region.Id).Select(x => x.LocationIso).ToList(),
                    publishedProgram.RegionIsos.Select(x => x.Value).ToList());


                CollectionAssert.AreEqual(context.Goals.Select(x => x.GoalName).ToList(), publishedProgram.Goals.Select(x => x.Value).ToList());
                CollectionAssert.AreEqual(context.Goals.Select(x => x.GoalId).ToList(), publishedProgram.Goals.Select(x => x.Id).ToList());

                Assert.AreEqual(program.Description, publishedProgram.Description);
                
                Assert.AreEqual(program.ProgramId, publishedProgram.Id);
                Assert.AreEqual(program.Name, publishedProgram.Name);
                Assert.AreEqual(parentProgram.ProgramId, publishedProgram.ParentProgramId);

                Assert.AreEqual(now, publishedProgram.RevisedOn);
                Assert.AreEqual(program.StartDate, publishedProgram.StartDate);
                Assert.AreEqual(owner.Name, publishedProgram.OwnerName);
                Assert.AreEqual(owner.Description, publishedProgram.OwnerDescription);
                Assert.AreEqual(owner.OrganizationId, publishedProgram.OwnerOrganizationId);
                
            };
            var result = service.GetProgramById(program.ProgramId);
            var resultAsync = await service.GetProgramByIdAsync(program.ProgramId);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProgramById_DoesNotHaveParentProgram()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var program = new Program
            {
                ProgramId = 1,
                Name = "name",
                Description = "description",
                ParentProgram = null,
                StartDate = DateTimeOffset.UtcNow,
                Owner = new Organization
                {
                    OrganizationId = 1,
                },
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                }
            };

            context.Programs.Add(program);
            Action<ProgramDTO> tester = (publishedProgram) =>
            {
                Assert.IsFalse(publishedProgram.ParentProgramId.HasValue);
            };
            var result = service.GetProgramById(program.ProgramId);
            var resultAsync = await service.GetProgramByIdAsync(program.ProgramId);
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProgramById_ProgramDoesNotExist()
        {
            Assert.IsNull(service.GetProgramById(-1));
            Assert.IsNull(await service.GetProgramByIdAsync(-1));
        }

        [TestMethod]
        public async Task TestGetPrograms_CheckProperties()
        {
            var org = new Organization
            {
                OrganizationId = 100
            };

            var program = new Program
            {
                ProgramId = 1,
                Name = "A",
                Description = "Description A",
                Owner = org
            };
            context.Organizations.Add(org);
            context.Programs.Add(program);

            Action<PagedQueryResults<SimpleProgramDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(org.OrganizationId, firstResult.OwnerId);
                Assert.AreEqual(program.ProgramId, firstResult.ProgramId);
                Assert.AreEqual(program.Name, firstResult.Name);
                Assert.AreEqual(program.Description, firstResult.Description);
            };

            var queryOperator = new QueryableOperator<SimpleProgramDTO>(0, 10, new ExpressionSorter<SimpleProgramDTO>(x => x.Name, SortDirection.Ascending));
            var serviceResults = service.GetPrograms(queryOperator);
            var serviceResultsAsync = await service.GetProgramsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetPrograms_Filter()
        {
            var org = new Organization
            {
                OrganizationId = 100
            };

            var program1 = new Program
            {
                ProgramId = 1,
                Name = "A",
                Description = "Description A",
                Owner = org
            };
            var program2 = new Program
            {
                ProgramId = 2,
                Name = "B",
                Description = "Description B",
                Owner = org
            };
            context.Organizations.Add(org);
            context.Programs.Add(program1);
            context.Programs.Add(program2);

            Action<PagedQueryResults<SimpleProgramDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(org.OrganizationId, firstResult.OwnerId);
                Assert.AreEqual(program2.ProgramId, firstResult.ProgramId);
            };

            var queryOperator = new QueryableOperator<SimpleProgramDTO>(0, 10, new ExpressionSorter<SimpleProgramDTO>(x => x.Name, SortDirection.Ascending));
            queryOperator.Filters.Add(new ExpressionFilter<SimpleProgramDTO>(x => x.Name, ComparisonType.Equal, program2.Name));
            var serviceResults = service.GetPrograms(queryOperator);
            var serviceResultsAsync = await service.GetProgramsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetPrograms_DefaultSort()
        {
            var org = new Organization
            {
                OrganizationId = 100
            };

            var program1 = new Program
            {
                ProgramId = 1,
                Name = "A",
                Description = "Description A",
                Owner = org
            };
            var program2 = new Program
            {
                ProgramId = 2,
                Name = "B",
                Description = "Description B",
                Owner = org
            };
            context.Organizations.Add(org);
            context.Programs.Add(program1);
            context.Programs.Add(program2);

            Action<PagedQueryResults<SimpleProgramDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(2, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(org.OrganizationId, firstResult.OwnerId);
                Assert.AreEqual(program2.ProgramId, firstResult.ProgramId);
            };

            var queryOperator = new QueryableOperator<SimpleProgramDTO>(0, 1, new ExpressionSorter<SimpleProgramDTO>(x => x.Name, SortDirection.Descending));
            var serviceResults = service.GetPrograms(queryOperator);
            var serviceResultsAsync = await service.GetProgramsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetPrograms_Sort()
        {
            var org = new Organization
            {
                OrganizationId = 100
            };

            var program1 = new Program
            {
                ProgramId = 1,
                Name = "A",
                Description = "Description A",
                Owner = org
            };
            var program2 = new Program
            {
                ProgramId = 2,
                Name = "B",
                Description = "Description B",
                Owner = org
            };
            context.Organizations.Add(org);
            context.Programs.Add(program1);
            context.Programs.Add(program2);

            Action<PagedQueryResults<SimpleProgramDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(2, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(org.OrganizationId, firstResult.OwnerId);
                Assert.AreEqual(program2.ProgramId, firstResult.ProgramId);
            };

            var queryOperator = new QueryableOperator<SimpleProgramDTO>(0, 1, new ExpressionSorter<SimpleProgramDTO>(x => x.Name, SortDirection.Ascending));
            queryOperator.Sorters.Add(new ExpressionSorter<SimpleProgramDTO>(x => x.ProgramId, SortDirection.Descending));
            var serviceResults = service.GetPrograms(queryOperator);
            var serviceResultsAsync = await service.GetProgramsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetPrograms_Paging()
        {
            var org = new Organization
            {
                OrganizationId = 100
            };

            var program1 = new Program
            {
                ProgramId = 1,
                Name = "A",
                Description = "Description A",
                Owner = org
            };
            var program2 = new Program
            {
                ProgramId = 2,
                Name = "B",
                Description = "Description B",
                Owner = org
            };
            context.Organizations.Add(org);
            context.Programs.Add(program1);
            context.Programs.Add(program2);

            Action<PagedQueryResults<SimpleProgramDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(2, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
            };

            var queryOperator = new QueryableOperator<SimpleProgramDTO>(0, 1, new ExpressionSorter<SimpleProgramDTO>(x => x.Name, SortDirection.Descending));
            var serviceResults = service.GetPrograms(queryOperator);
            var serviceResultsAsync = await service.GetProgramsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Create
        [TestMethod]
        public void TestCreate_CheckProperties()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var focus = "focus";
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: parentProgramId,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.IsNotNull(program.ParentProgram);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);

            Assert.AreEqual(user.Id, program.History.CreatedBy);
            Assert.AreEqual(user.Id, program.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(program.History.RevisedOn, DbContextHelper.DATE_PRECISION);
            DateTimeOffset.UtcNow.Should().BeCloseTo(program.History.CreatedOn, DbContextHelper.DATE_PRECISION);

            Assert.AreEqual(name, program.Name);
            Assert.AreEqual(description, program.Description);
            Assert.AreEqual(startDate, program.StartDate);
            Assert.AreEqual(endDate, program.EndDate);
            Assert.AreEqual(ownerOrganizationId, program.OwnerId);
            Assert.AreEqual(parentProgramId, program.ParentProgram.ProgramId);
            Assert.AreEqual(ProgramStatus.Draft.Id, program.ProgramStatusId);
            Assert.AreEqual(focus, program.Focus);
            Assert.AreEqual(website, program.Website);
        }

        [TestMethod]
        public void TestCreate_DoesNotHaveParentProgram()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var focus = "focus";
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: null,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.IsNull(program.ParentProgram);
            
        }

        [TestMethod]
        public async Task TestCreateAsync_CheckProperties()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var focus = "focus";
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: parentProgramId,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
            Assert.IsNotNull(program.ParentProgram);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);

            Assert.AreEqual(user.Id, program.History.CreatedBy);
            Assert.AreEqual(user.Id, program.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(program.History.RevisedOn, DbContextHelper.DATE_PRECISION);
            DateTimeOffset.UtcNow.Should().BeCloseTo(program.History.CreatedOn, DbContextHelper.DATE_PRECISION);

            Assert.AreEqual(name, program.Name);
            Assert.AreEqual(description, program.Description);
            Assert.AreEqual(startDate, program.StartDate);
            Assert.AreEqual(endDate, program.EndDate);
            Assert.AreEqual(ownerOrganizationId, program.OwnerId);
            Assert.AreEqual(parentProgramId, program.ParentProgram.ProgramId);
            Assert.AreEqual(ProgramStatus.Draft.Id, program.ProgramStatusId);
            Assert.AreEqual(focus, program.Focus);
            Assert.AreEqual(website, program.Website);
        }

        [TestMethod]
        public async Task TestCreateAsync_DoesNotHaveParentProgram()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var focus = "focus";
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: null,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
            Assert.IsNull(program.ParentProgram);

        }

        [TestMethod]
        public void TestCreate_CheckContacts()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            var contact = new Contact
            {
                ContactId = 1,
                FullName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var focus = "focus";
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int> { contact.ContactId };
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: parentProgramId,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(contact.ContactId, program.Contacts.First().ContactId);
        }

        [TestMethod]
        public async Task TestCreateAsync_CheckContacts()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            var contact = new Contact
            {
                ContactId = 1,
                FullName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var focus = "focus";
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int> { contact.ContactId };
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: parentProgramId,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(contact.ContactId, program.Contacts.First().ContactId);
        }

        [TestMethod]
        public void TestCreate_CheckGoals()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            var goal = new Goal
            {
                GoalId = 1,
                GoalName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var focus = "focus";
            var website = "http://www.google.com";
            var goalIds = new List<int> { goal.GoalId };
            var themeIds = new List<int>();
            var contactIds = new List<int>();
            var regionIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: parentProgramId,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(1, program.Goals.Count);
            Assert.AreEqual(goal.GoalId, program.Goals.First().GoalId);
        }

        [TestMethod]
        public async Task TestCreateAsync_CheckGoals()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            var goal = new Goal
            {
                GoalId = 1,
                GoalName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var focus = "focus";
            var website = "http://www.google.com";
            var goalIds = new List<int> { goal.GoalId };
            var themeIds = new List<int>();
            var contactIds = new List<int>();
            var regionIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: parentProgramId,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(1, program.Goals.Count);
            Assert.AreEqual(goal.GoalId, program.Goals.First().GoalId);
        }

        [TestMethod]
        public void TestCreate_CheckThemes()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            var theme = new Theme
            {
                ThemeId = 1,
                ThemeName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var focus = "focus";
            var website = "http://www.google.com";
            var goalIds = new List<int>();
            var themeIds = new List<int> { theme.ThemeId };
            var contactIds = new List<int>();
            var regionIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: parentProgramId,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(1, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(theme.ThemeId, program.Themes.First().ThemeId);
        }

        [TestMethod]
        public async Task TestCreateAsync_CheckThemes()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            var theme = new Theme
            {
                ThemeId = 1,
                ThemeName = "contact name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var focus = "focus";
            var website = "http://www.google.com";
            var goalIds = new List<int>();
            var themeIds = new List<int> { theme.ThemeId };
            var contactIds = new List<int>();
            var regionIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: parentProgramId,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(1, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(theme.ThemeId, program.Themes.First().ThemeId);
        }

        [TestMethod]
        public void TestCreate_CheckRegions()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            var region = new Location
            {
                LocationId = 1,
                LocationName = "name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var focus = "focus";
            var website = "http://www.google.com";
            var goalIds = new List<int>();
            var themeIds = new List<int> ();
            var contactIds = new List<int>();
            var regionIds = new List<int>{region.LocationId};

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: parentProgramId,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(1, program.Regions.Count);
            Assert.AreEqual(region.LocationId, program.Regions.First().LocationId);
        }

        [TestMethod]
        public async Task TestCreateAsync_CheckRegions()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            var region = new Location
            {
                LocationId = 1,
                LocationName = "name"
            };
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var focus = "focus";
            var website = "http://www.google.com";
            var goalIds = new List<int>();
            var themeIds = new List<int>();
            var contactIds = new List<int>();
            var regionIds = new List<int> { region.LocationId };

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: parentProgramId,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(1, program.Regions.Count);
            Assert.AreEqual(region.LocationId, program.Regions.First().LocationId);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestCreate_RegionsAreNotRegions()
        {
            locationTypeIds.Add(LocationType.State.Id);
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var focus = "focus";
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: parentProgramId,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = service.Create(draftProgram);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task TestCreateAsync_RegionsAreNotRegions()
        {
            locationTypeIds.Add(LocationType.State.Id);
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var ownerOrganizationId = 2;
            var parentProgramId = 3;
            var focus = "focus";
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerOrganizationId,
               parentProgramId: parentProgramId,
               focus: focus,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds
               );

            var program = await service.CreateAsync(draftProgram);
        }
        #endregion

        #region Update
        [TestMethod]
        public void TestUpdate_CheckProperties()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                Focus = "focus",
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = creatorId,
                    RevisedOn = yesterday
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );
            service.Update(updatedEcaProgram);
            
            var updatedProgram = context.Programs.First();
            Assert.AreEqual(newDescription, updatedProgram.Description);
            Assert.AreEqual(newEndDate, updatedProgram.EndDate);
            Assert.AreEqual(newFocus, updatedProgram.Focus);
            Assert.AreEqual(newName, updatedProgram.Name);
            Assert.AreEqual(newOwnerId, updatedProgram.OwnerId);
            Assert.AreEqual(newOwnerId, updatedProgram.Owner.OrganizationId);
            Assert.AreEqual(program.ProgramId, updatedProgram.ProgramId);
            Assert.AreEqual(newProgramStatusId, updatedProgram.ProgramStatusId);
            Assert.AreEqual(newStartDate, updatedProgram.StartDate);
            Assert.AreEqual(newWebsite, updatedProgram.Website);

            Assert.AreEqual(yesterday, updatedProgram.History.CreatedOn);
            Assert.AreEqual(creatorId, updatedProgram.History.CreatedBy);

            Assert.AreEqual(revisorId, updatedProgram.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(updatedProgram.History.RevisedOn);
            
        }

        [TestMethod]
        public async Task TestUpdateAsync_CheckProperties()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                Focus = "focus",
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = creatorId,
                    RevisedOn = yesterday
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(newDescription, updatedProgram.Description);
            Assert.AreEqual(newEndDate, updatedProgram.EndDate);
            Assert.AreEqual(newFocus, updatedProgram.Focus);
            Assert.AreEqual(newName, updatedProgram.Name);
            Assert.AreEqual(newOwnerId, updatedProgram.OwnerId);
            Assert.AreEqual(newOwnerId, updatedProgram.Owner.OrganizationId);
            Assert.AreEqual(program.ProgramId, updatedProgram.ProgramId);
            Assert.AreEqual(newProgramStatusId, updatedProgram.ProgramStatusId);
            Assert.AreEqual(newStartDate, updatedProgram.StartDate);
            Assert.AreEqual(newWebsite, updatedProgram.Website);

            Assert.AreEqual(yesterday, updatedProgram.History.CreatedOn);
            Assert.AreEqual(creatorId, updatedProgram.History.CreatedBy);

            Assert.AreEqual(revisorId, updatedProgram.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(updatedProgram.History.RevisedOn);
        }

        [TestMethod]
        public void TestUpdate_CheckGoals()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                Focus = "focus",
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: new List<int> {1},
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );
            service.Update(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(1, updatedProgram.Goals.Count);
            Assert.AreEqual(0, updatedProgram.Regions.Count);
            Assert.AreEqual(0, updatedProgram.Themes.Count);
            Assert.AreEqual(0, updatedProgram.Contacts.Count);

            Assert.AreEqual(updatedEcaProgram.GoalIds.First(), updatedProgram.Goals.First().GoalId);
        }

        [TestMethod]
        public async Task TestUpdateAsync_CheckGoals()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                Focus = "focus",
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: new List<int> { 1 },
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(1, updatedProgram.Goals.Count);
            Assert.AreEqual(0, updatedProgram.Regions.Count);
            Assert.AreEqual(0, updatedProgram.Themes.Count);
            Assert.AreEqual(0, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.GoalIds.First(), updatedProgram.Goals.First().GoalId);
        }

        [TestMethod]
        public void TestUpdate_CheckThemes()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                Focus = "focus",
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: new List<int> { 1 },
                regionIds: null
                );
            service.Update(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(0, updatedProgram.Goals.Count);
            Assert.AreEqual(0, updatedProgram.Regions.Count);
            Assert.AreEqual(1, updatedProgram.Themes.Count);
            Assert.AreEqual(0, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.ThemeIds.First(), updatedProgram.Themes.First().ThemeId);
        }

        [TestMethod]
        public async Task TestUpdateAsync_CheckThemes()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                Focus = "focus",
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: new List<int> { 1 },
                regionIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(0, updatedProgram.Goals.Count);
            Assert.AreEqual(0, updatedProgram.Regions.Count);
            Assert.AreEqual(1, updatedProgram.Themes.Count);
            Assert.AreEqual(0, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.ThemeIds.First(), updatedProgram.Themes.First().ThemeId);
        }

        [TestMethod]
        public void TestUpdate_CheckPointOfContacts()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                Focus = "focus",
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: new List<int> { 1 },
                themeIds: null,
                regionIds: null
                );
            service.Update(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(0, updatedProgram.Goals.Count);
            Assert.AreEqual(0, updatedProgram.Regions.Count);
            Assert.AreEqual(0, updatedProgram.Themes.Count);
            Assert.AreEqual(1, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.ContactIds.First(), updatedProgram.Contacts.First().ContactId);
        }

        [TestMethod]
        public async Task TestUpdateAsync_CheckPointOfContacts()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                Focus = "focus",
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: new List<int> { 1 },
                themeIds: null,
                regionIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(0, updatedProgram.Goals.Count);
            Assert.AreEqual(0, updatedProgram.Regions.Count);
            Assert.AreEqual(0, updatedProgram.Themes.Count);
            Assert.AreEqual(1, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.ContactIds.First(), updatedProgram.Contacts.First().ContactId);
        }

        [TestMethod]
        public void TestUpdate_CheckRegions()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                Focus = "focus",
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: new List<int> { 1 }
                );
            service.Update(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(0, updatedProgram.Goals.Count);
            Assert.AreEqual(1, updatedProgram.Regions.Count);
            Assert.AreEqual(0, updatedProgram.Themes.Count);
            Assert.AreEqual(0, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.RegionIds.First(), updatedProgram.Regions.First().LocationId);
        }

        [TestMethod]
        public async Task TestUpdateAsync_CheckRegions()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                Focus = "focus",
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = revisorId,
                    RevisedOn = now
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: new List<int> { 1 }
                );
            await service.UpdateAsync(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(0, updatedProgram.Goals.Count);
            Assert.AreEqual(1, updatedProgram.Regions.Count);
            Assert.AreEqual(0, updatedProgram.Themes.Count);
            Assert.AreEqual(0, updatedProgram.Contacts.Count);
            Assert.AreEqual(updatedEcaProgram.RegionIds.First(), updatedProgram.Regions.First().LocationId);
        }

        [TestMethod]
        [ExpectedException(typeof(ModelNotFoundException))]
        public void TestUpdate_ModelNotFoundException()
        {
            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = 12;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(1),
                id: 1,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );
            service.Update(updatedEcaProgram);
        }

        [TestMethod]
        [ExpectedException(typeof(ModelNotFoundException))]
        public async Task TestUpdateAsync_ModelNotFoundException()
        {
            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = 12;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(1),
                id: 1,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestUpdate_RegionsAreNotRegions()
        {
            locationTypeIds.Add(LocationType.State.Id);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                Focus = "focus",
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = creatorId,
                    RevisedOn = yesterday
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );
            service.Update(updatedEcaProgram);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task TestUpdateAsync_RegionsAreNotRegions()
        {
            locationTypeIds.Add(LocationType.State.Id);
            Assert.AreEqual(0, context.Programs.Count());
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
                Focus = "focus",
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                Website = "old website",
                History = new History
                {
                    CreatedBy = creatorId,
                    CreatedOn = yesterday,
                    RevisedBy = creatorId,
                    RevisedOn = yesterday
                },
            };
            context.Programs.Add(program);
            context.Programs.Add(parentProgram);
            Assert.AreEqual(2, context.Programs.Count());

            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newFocus = "new focus";
            var newWebsite = "new website";
            var newOwnerId = 12;
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: newOwnerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                focus: newFocus,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);
        }
        #endregion

        [TestMethod]
        public void TestValidateAllLocationsAreRegions_NotADistinctList()
        {
            var typeIds = new List<int>();
            typeIds.Add(LocationType.Region.Id);
            typeIds.Add(LocationType.Region.Id);
            service.ValidateAllLocationsAreRegions(typeIds);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidateAllLocationsAreRegions_ListContainsNoRegionTypeIds()
        {
            var typeIds = new List<int>();
            typeIds.Add(LocationType.State.Id);
            service.ValidateAllLocationsAreRegions(typeIds);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidateAllLocationsAreRegions_EmptyList()
        {
            var typeIds = new List<int>();
            service.ValidateAllLocationsAreRegions(typeIds);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestValidateAllLocationsAreRegions_NullList()
        {
            service.ValidateAllLocationsAreRegions(null);
        }

        [TestMethod]
        public async Task TestGetLocationTypeIds()
        {
            locationTypeIds.Add(LocationType.Region.Id);
            CollectionAssert.AreEqual(locationTypeIds, service.GetLocationTypeIds(new List<int>()));
            CollectionAssert.AreEqual(locationTypeIds, await service.GetLocationTypeIdsAsync(new List<int>()));
        }

        [TestMethod]
        public void TestSetGoals()
        {
            var original = new Goal { GoalId = 1 };

            var program = new Program();
            program.Goals.Add(original);

            var newGoal = new Goal { GoalId = 2 };
            var newGoalIds = new List<int> { newGoal.GoalId };
            service.SetGoals(newGoalIds, program);
            Assert.AreEqual(1, program.Goals.Count);
            Assert.AreEqual(newGoal.GoalId, program.Goals.First().GoalId);

        }

        [TestMethod]
        public void TestSetThemes()
        {
            var original = new Theme { ThemeId = 1 };

            var program = new Program();
            program.Themes.Add(original);

            var newTheme = new Theme { ThemeId = 2 };
            var newThemeIds = new List<int> { newTheme.ThemeId };
            service.SetThemes(newThemeIds, program);
            Assert.AreEqual(1, program.Themes.Count);
            Assert.AreEqual(newTheme.ThemeId, program.Themes.First().ThemeId);

        }

        [TestMethod]
        public void TestSetRegions()
        {
            var original = new Location { LocationId = 1 };

            var program = new Program();
            program.Regions.Add(original);

            var newLocation = new Location { LocationId = 2 };
            var newLocationIds = new List<int> { newLocation.LocationId };
            service.SetRegions(newLocationIds, program);
            Assert.AreEqual(1, program.Regions.Count);
            Assert.AreEqual(newLocation.LocationId, program.Regions.First().LocationId);
        }

        [TestMethod]
        public void TestSetPointsOfContact()
        {
            var original = new Contact { ContactId = 1 };

            var program = new Program();
            program.Contacts.Add(original);

            var newContact = new Contact { ContactId = 2 };
            var newContactIds = new List<int> { newContact.ContactId };
            service.SetPointOfContacts(newContactIds, program);
            Assert.AreEqual(1, program.Contacts.Count);
            Assert.AreEqual(newContact.ContactId, program.Contacts.First().ContactId);
        }
    }
}

