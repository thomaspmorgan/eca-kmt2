using ECA.Business.Models.Programs;
using FluentAssertions;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class ProgramServiceTest
    {
        private TestEcaContext context;
        private ProgramService service;

        [TestInitialize]
        public void TestInit()
        {
            context = DbContextHelper.GetInMemoryContext();
            service = new ProgramService(context);
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

            program.Contacts = new HashSet<Contact>();
            program.Goals = new HashSet<Goal>();
            program.Themes = new HashSet<Theme>();
            program.Regions = new HashSet<Location>();
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

            program.Contacts = new HashSet<Contact>();
            program.Goals = new HashSet<Goal>();
            program.Themes = new HashSet<Theme>();
            program.Regions = new HashSet<Location>();

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

        [TestMethod]
        public void TestCreate_CheckProperties()
        {
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
               themeIds: themeIds
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
               themeIds: themeIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.IsNull(program.ParentProgram);
            
        }

        [TestMethod]
        public void TestCreate_CheckContacts()
        {
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
               themeIds: themeIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(1, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(contact.ContactId, program.Contacts.First().ContactId);
        }

        [TestMethod]
        public void TestCreate_CheckGoals()
        {
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
               themeIds: themeIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(0, program.Themes.Count);
            Assert.AreEqual(1, program.Goals.Count);
            Assert.AreEqual(goal.GoalId, program.Goals.First().GoalId);
        }

        [TestMethod]
        public void TestCreate_CheckThemes()
        {
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
               themeIds: themeIds
               );

            var program = service.Create(draftProgram);
            Assert.IsNotNull(program);
            Assert.AreEqual(0, program.Contacts.Count);
            Assert.AreEqual(1, program.Themes.Count);
            Assert.AreEqual(0, program.Goals.Count);
            Assert.AreEqual(theme.ThemeId, program.Themes.First().ThemeId);
        }
        #endregion
    }
}
