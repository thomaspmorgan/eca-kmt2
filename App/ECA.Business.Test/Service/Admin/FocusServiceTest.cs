using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Programs;
using System.Reflection;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;
using ECA.Business.Service.Admin;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class FocusServiceTest
    {
        private TestEcaContext context;
        private FocusService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new FocusService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGetFoci_CheckProperties()
        {
            var focus = new Focus
            {
                FocusId = 1,
                FocusName = "f"
            };
            context.Foci.Add(focus);
            Action<PagedQueryResults<FocusDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(focus.FocusId, firstResult.Id);
                Assert.AreEqual(focus.FocusName, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<FocusDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<FocusDTO>(0, 10, defaultSorter);

            var serviceResults = service.GetFoci(queryOperator);
            var serviceResultsAsync = await service.GetFociAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFoci_DefaultSorter()
        {
            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "g1"
            };
            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "g2"
            };
            context.Foci.Add(focus1);
            context.Foci.Add(focus2);
            Action<PagedQueryResults<FocusDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(focus2.FocusId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<FocusDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetFoci(queryOperator);
            var serviceResultsAsync = await service.GetFociAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFoci_Filter()
        {
            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "g1"
            };
            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "g2"
            };
            context.Foci.Add(focus1);
            context.Foci.Add(focus2);
            Action<PagedQueryResults<FocusDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(focus1.FocusId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<FocusDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusDTO>(0, 1, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<FocusDTO>(x => x.Id, ComparisonType.Equal, focus1.FocusId));

            var serviceResults = service.GetFoci(queryOperator);
            var serviceResultsAsync = await service.GetFociAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFoci_Sort()
        {
            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "g1"
            };
            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "g2"
            };
            context.Foci.Add(focus2);
            context.Foci.Add(focus1);

            Action<PagedQueryResults<FocusDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(focus2.FocusId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<FocusDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusDTO>(0, 1, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<FocusDTO>(x => x.Id, SortDirection.Descending));

            var serviceResults = service.GetFoci(queryOperator);
            var serviceResultsAsync = await service.GetFociAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFoci_Paging()
        {
            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "g1"
            };
            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "g2"
            };
            context.Foci.Add(focus2);
            context.Foci.Add(focus1);

            Action<PagedQueryResults<FocusDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
            };
            var defaultSorter = new ExpressionSorter<FocusDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetFoci(queryOperator);
            var serviceResultsAsync = await service.GetFociAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

    }
}
