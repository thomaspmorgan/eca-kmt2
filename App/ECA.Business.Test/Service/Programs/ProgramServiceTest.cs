using ECA.Business.Exceptions;
using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service;
using ECA.Business.Service.Programs;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        private Mock<IBusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity>> mockValidator;

        [TestInitialize]
        public void TestInit()
        {
            mockValidator = new Mock<IBusinessValidator<ProgramServiceValidationEntity, ProgramServiceValidationEntity>>();
            mockValidator.Setup(x => x.ValidateCreate(It.IsAny<ProgramServiceValidationEntity>())).Returns(new List<BusinessValidationResult>());
            mockValidator.Setup(x => x.ValidateUpdate(It.IsAny<ProgramServiceValidationEntity>())).Returns(new List<BusinessValidationResult>());

            context = new TestEcaContext();
            service = new ProgramService(context, mockValidator.Object);
        }

        private void SetupMockValidatorToThrowException()
        {
            mockValidator.Setup(x => x.ValidateCreate(It.IsAny<ProgramServiceValidationEntity>())).Callback(() => { throw new ValidationException(); });
            mockValidator.Setup(x => x.ValidateUpdate(It.IsAny<ProgramServiceValidationEntity>())).Callback(() => { throw new ValidationException(); });
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
            var rowVersion = new byte[1] { (byte)1 };

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
                Name = "owner",
                OfficeSymbol = "symbol"
            };
            var focus = new Focus
            {
                FocusId = 501,
                FocusName = "focus"
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "name",
                Description = "description",
                ParentProgram = parentProgram,
                RowVersion = rowVersion,
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
            context.Foci.Add(focus);

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

                CollectionAssert.AreEqual(rowVersion, publishedProgram.RowVersion);

                Assert.AreEqual(context.Foci.Select(x => x.FocusName).First(), publishedProgram.Focus.Value);
                Assert.AreEqual(context.Foci.Select(x => x.FocusId).First(), publishedProgram.Focus.Id);

                Assert.AreEqual(program.Description, publishedProgram.Description);

                Assert.AreEqual(program.ProgramId, publishedProgram.Id);
                Assert.AreEqual(program.Name, publishedProgram.Name);
                Assert.AreEqual(parentProgram.ProgramId, publishedProgram.ParentProgramId);

                Assert.AreEqual(now, publishedProgram.RevisedOn);
                Assert.AreEqual(program.StartDate, publishedProgram.StartDate);
                Assert.AreEqual(owner.Name, publishedProgram.OwnerName);
                Assert.AreEqual(owner.Description, publishedProgram.OwnerDescription);
                Assert.AreEqual(owner.OrganizationId, publishedProgram.OwnerOrganizationId);
                Assert.AreEqual(owner.OfficeSymbol, publishedProgram.OwnerOfficeSymbol);

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
            var parentProgramId = 3;
            var parentProgram = new Program { ProgramId = parentProgramId };
            context.Programs.Add(parentProgram);
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });

            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
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
            Assert.AreEqual(ownerId, program.OwnerId);
            Assert.AreEqual(parentProgramId, program.ParentProgram.ProgramId);
            Assert.AreEqual(ProgramStatus.Draft.Id, program.ProgramStatusId);
            Assert.AreEqual(website, program.Website);
        }

        [TestMethod]
        public void TestCreate_DoesNotHaveParentProgram()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });

            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: null,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.IsNull(program.ParentProgram);

        }

        [TestMethod]
        public async Task TestCreateAsync_CheckProperties()
        {
            var parentProgramId = 3;
            var parentProgram = new Program { ProgramId = parentProgramId };
            context.Programs.Add(parentProgram);
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
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
            Assert.AreEqual(ownerId, program.OwnerId);
            Assert.AreEqual(parentProgramId, program.ParentProgram.ProgramId);
            Assert.AreEqual(ProgramStatus.Draft.Id, program.ProgramStatusId);
            Assert.AreEqual(website, program.Website);
        }

        [TestMethod]
        public async Task TestCreateAsync_DoesNotHaveParentProgram()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var focusId = 100;
            var focus = new Focus { FocusId = focusId };
            context.Foci.Add(focus);

            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);

            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: null,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
            Assert.IsNull(program.ParentProgram);

        }


        [TestMethod]
        public async Task TestCreateAsync_CheckFocus()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var focusId = 100;
            var focus = new Focus { FocusId = focusId };
            context.Foci.Add(focus);
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
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int> { contact.ContactId };
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );

            var program = await service.CreateAsync(draftProgram);
            Assert.IsNotNull(program);
        }

        [TestMethod]
        public void TestCreate_CheckContacts()
        {
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var focusId = 100;
            var focus = new Focus { FocusId = focusId };
            context.Foci.Add(focus);
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
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int> { contact.ContactId };
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
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
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
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
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int> { contact.ContactId };
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
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
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });

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
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var goalIds = new List<int> { goal.GoalId };
            var themeIds = new List<int>();
            var contactIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
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
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });

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
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var goalIds = new List<int> { goal.GoalId };
            var themeIds = new List<int>();
            var contactIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
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
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });

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
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var goalIds = new List<int>();
            var themeIds = new List<int> { theme.ThemeId };
            var contactIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
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
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var focusId = 100;
            var focus = new Focus { FocusId = focusId };
            context.Foci.Add(focus);
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
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var goalIds = new List<int>();
            var themeIds = new List<int> { theme.ThemeId };
            var contactIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
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
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var focusId = 100;
            var focus = new Focus { FocusId = focusId };
            context.Foci.Add(focus);
            var region = new Location { LocationId = 1, LocationTypeId = LocationType.Region.Id };
            context.Locations.Add(region);
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var goalIds = new List<int>();
            var themeIds = new List<int>();
            var contactIds = new List<int>();
            var regionIds = new List<int> { region.LocationId };
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
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
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var focusId = 100;
            var focus = new Focus { FocusId = focusId };
            context.Foci.Add(focus);
            var region = new Location { LocationId = 1, LocationTypeId = LocationType.Region.Id };
            context.Locations.Add(region);
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var goalIds = new List<int>();
            var themeIds = new List<int>();
            var contactIds = new List<int>();
            var regionIds = new List<int> { region.LocationId };
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: contactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
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
        public void TestCreate_EnsureExecutesValidator()
        {
            var ownerId = 12;
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );
            SetupMockValidatorToThrowException();
            service.Create(draftProgram);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task TestCreateAsync_EnsureExecutesValidator()
        {
            var ownerId = 12;
            var userId = 1;
            var user = new User(userId);
            var name = "name";
            var description = "description";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var parentProgramId = 3;
            var website = "http://www.google.com";
            var pointOfContactIds = new List<int>();
            var themeIds = new List<int>();
            var goalIds = new List<int>();
            var regionIds = new List<int>();
            var categoryIds = new List<int>();
            var objectiveIds = new List<int>();

            var draftProgram = new DraftProgram(
               createdBy: user,
               name: name,
               description: description,
               startDate: startDate,
               endDate: endDate,
               ownerOrganizationId: ownerId,
               parentProgramId: parentProgramId,
               website: website,
               goalIds: goalIds,
               pointOfContactIds: pointOfContactIds,
               themeIds: themeIds,
               regionIds: regionIds,
               categoryIds: categoryIds,
               objectiveIds: objectiveIds
               );
            SetupMockValidatorToThrowException();
            await service.CreateAsync(draftProgram);

        }

        #endregion

        #region Update
        [TestMethod]
        public void TestUpdate_CheckProperties()
        {
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var originalRowVersion = new byte[1] { (byte)0 };
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
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;
            var updatedRowVersion = new byte[1] { (byte)1 };

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: updatedRowVersion,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                categoryIds: null,
                objectiveIds: null
                );
            service.Update(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(newDescription, updatedProgram.Description);
            Assert.AreEqual(newEndDate, updatedProgram.EndDate);
            Assert.AreEqual(newName, updatedProgram.Name);
            Assert.AreEqual(ownerId, updatedProgram.OwnerId);
            Assert.AreEqual(ownerId, updatedProgram.Owner.OrganizationId);
            Assert.AreEqual(program.ProgramId, updatedProgram.ProgramId);
            Assert.AreEqual(newProgramStatusId, updatedProgram.ProgramStatusId);
            Assert.AreEqual(newStartDate, updatedProgram.StartDate);
            Assert.AreEqual(newWebsite, updatedProgram.Website);

            Assert.AreEqual(yesterday, updatedProgram.History.CreatedOn);
            Assert.AreEqual(creatorId, updatedProgram.History.CreatedBy);
            Assert.AreEqual(revisorId, updatedProgram.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(updatedProgram.History.RevisedOn, DbContextHelper.DATE_PRECISION);
            CollectionAssert.AreEqual(updatedRowVersion, updatedProgram.RowVersion);

        }

        [TestMethod]
        public async Task TestUpdateAsync_CheckProperties()
        {
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var originalRowVersion = new byte[1] { (byte)1 };
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
                ProgramStatusId = ProgramStatus.Draft.Id,
                ParentProgram = null,
                RowVersion = originalRowVersion,
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
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;
            var updatedRowVersion = new byte[1] { (byte)1 };

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: updatedRowVersion,
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                categoryIds: null,
                objectiveIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);

            var updatedProgram = context.Programs.First();
            Assert.AreEqual(newDescription, updatedProgram.Description);
            Assert.AreEqual(newEndDate, updatedProgram.EndDate);
            Assert.AreEqual(newName, updatedProgram.Name);
            Assert.AreEqual(ownerId, updatedProgram.OwnerId);
            Assert.AreEqual(ownerId, updatedProgram.Owner.OrganizationId);
            Assert.AreEqual(program.ProgramId, updatedProgram.ProgramId);
            Assert.AreEqual(newProgramStatusId, updatedProgram.ProgramStatusId);
            Assert.AreEqual(newStartDate, updatedProgram.StartDate);
            Assert.AreEqual(newWebsite, updatedProgram.Website);

            Assert.AreEqual(yesterday, updatedProgram.History.CreatedOn);
            Assert.AreEqual(creatorId, updatedProgram.History.CreatedBy);

            CollectionAssert.AreEqual(updatedRowVersion, updatedProgram.RowVersion);

            Assert.AreEqual(revisorId, updatedProgram.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(updatedProgram.History.RevisedOn, DbContextHelper.DATE_PRECISION);
        }

        [TestMethod]
        public void TestUpdate_CheckGoals()
        {
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var now = DateTime.UtcNow;
            var creatorId = 1;
            var revisorId = 2;
            var parentProgram = new Program
            {
                ProgramId = 2
            };
            var focus = new Focus
            {
                FocusId = 100
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "old name",
                Description = "old description",
                StartDate = DateTimeOffset.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1.0),
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
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: new List<int> { 1 },
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
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
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
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
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: new List<int> { 1 },
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
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
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
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
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: new List<int> { 1 },
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
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
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
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
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: new List<int> { 1 },
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
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
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
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
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: new List<int> { 1 },
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
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
            Assert.AreEqual(0, context.Programs.Count());
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
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
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: new List<int> { 1 },
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
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
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var region = new Location { LocationId = 1, LocationTypeId = LocationType.Region.Id };
            context.Locations.Add(region);
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
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: new List<int> { region.LocationId },
                                categoryIds: null,
                objectiveIds: null
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
            var ownerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = ownerId });
            var region = new Location { LocationId = 1, LocationTypeId = LocationType.Region.Id };
            context.Locations.Add(region);
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
            var newWebsite = "new website";
            var newParentProgramId = parentProgram.ProgramId;

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(revisorId),
                id: program.ProgramId,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: ownerId,
                parentProgramId: newParentProgramId,
                programStatusId: newProgramStatusId,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: new List<int> { region.LocationId },
                                categoryIds: null,
                objectiveIds: null
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
            var newOwnerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = newOwnerId });
            context.Foci.Add(new Focus { FocusId = 1 });
            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";
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
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
                );
            service.Update(updatedEcaProgram);
        }

        [TestMethod]
        [ExpectedException(typeof(ModelNotFoundException))]
        public async Task TestUpdateAsync_ModelNotFoundException()
        {
            var newOwnerId = 12;
            context.Organizations.Add(new Organization { OrganizationId = newOwnerId });
            context.Foci.Add(new Focus { FocusId = 1 });
            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";

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
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
                );
            await service.UpdateAsync(updatedEcaProgram);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void TestUpdate_EnsureExecutesValidator()
        {
            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(1),
                id: 1,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: 1,
                parentProgramId: 1,
                programStatusId: 1,
                programRowVersion: new byte[0],
                website: newWebsite,
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
                );
            context.Programs.Add(new Program { ProgramId = updatedEcaProgram.Id });
            SetupMockValidatorToThrowException();
            service.Update(updatedEcaProgram);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public async Task TestUpdateAsync_EnsureExecutesValidator()
        {
            var newName = "new name";
            var newDescription = "new description";
            var newStartDate = DateTimeOffset.UtcNow.AddDays(10.0);
            var newEndDate = DateTimeOffset.UtcNow.AddDays(12.0);
            var newProgramStatusId = ProgramStatus.Completed.Id;
            var newWebsite = "new website";

            var updatedEcaProgram = new EcaProgram(
                updatedBy: new User(1),
                id: 1,
                name: newName,
                description: newDescription,
                startDate: newStartDate,
                endDate: newEndDate,
                ownerOrganizationId: 1,
                parentProgramId: 1,
                programStatusId: 1,
                website: newWebsite,
                programRowVersion: new byte[0],
                goalIds: null,
                pointOfContactIds: null,
                themeIds: null,
                regionIds: null,
                                categoryIds: null,
                objectiveIds: null
                );
            SetupMockValidatorToThrowException();
            context.Programs.Add(new Program { ProgramId = updatedEcaProgram.Id });
            await service.UpdateAsync(updatedEcaProgram);
        }

        #endregion

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
        public async Task TestGetLocationTypeIds()
        {
            var expectedTypeIds = new List<int> { LocationType.Region.Id };
            var region = new Location { LocationId = 1, LocationTypeId = LocationType.Region.Id };

            context.Locations.Add(region);
            CollectionAssert.AreEqual(expectedTypeIds, service.GetLocationTypeIds(new List<int> { region.LocationId }));
            CollectionAssert.AreEqual(expectedTypeIds, await service.GetLocationTypeIdsAsync(new List<int> { region.LocationId }));
        }
    }
}

