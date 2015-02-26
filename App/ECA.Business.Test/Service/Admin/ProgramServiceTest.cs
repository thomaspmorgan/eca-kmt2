using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using System.Reflection;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Business.Test.Service.Admin
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
        public async Task TestGetPrograms_Filtered()
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
        #endregion
    }
}
