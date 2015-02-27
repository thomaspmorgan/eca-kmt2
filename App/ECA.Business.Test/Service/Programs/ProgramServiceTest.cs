using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
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

        #region Dispose
        [TestMethod]
        public void TestDispose_Context()
        {
            var testContext = DbContextHelper.GetInMemoryContext();
            var testService = new ProgramService(testContext);

            var contextField = typeof(ProgramService).GetField("context", BindingFlags.Instance | BindingFlags.NonPublic);
            var contextValue = contextField.GetValue(testService);
            Assert.IsNotNull(contextField);
            Assert.IsNotNull(contextValue);

            testService.Dispose();
            contextValue = contextField.GetValue(testService);
            Assert.IsNull(contextValue);
            Assert.IsTrue(testContext.IsDisposed);

        }
        #endregion

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
                CollectionAssert.AreEqual(program.Contacts.Select(x => x.ContactId).ToList(), publishedProgram.ContactIds.ToList());
                CollectionAssert.AreEqual(
                    context.Locations.Where(x => x.LocationTypeId == LocationType.Country.Id).Select(x => x.LocationId).ToList(), 
                    publishedProgram.CountryIds.ToList());

                CollectionAssert.AreEqual(
                    context.Locations.Where(x => x.LocationTypeId == LocationType.Country.Id).Select(x => x.LocationIso).ToList(), 
                    publishedProgram.CountryIsos.ToList());

                Assert.AreEqual(program.Description, publishedProgram.Description);
                CollectionAssert.AreEqual(context.Goals.Select(x => x.GoalId).ToList(), publishedProgram.GoalIds.ToList());
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
    }
}
