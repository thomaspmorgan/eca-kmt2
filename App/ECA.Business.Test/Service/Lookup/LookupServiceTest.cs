using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Lookup;
using ECA.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using ECA.Core.Query;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Business.Test.Service.Lookup
{
    public class LookupServiceTestDTO
    {
        public int Id { get; set; }

        public string Value { get; set; }
    }

    public class LookupServiceTestServiceClass : LookupService<LookupServiceTestDTO>
    {
        public LookupServiceTestServiceClass(TestEcaContext context)
            : base(context)
        {

        }

        protected override IQueryable<LookupServiceTestDTO> GetSelectDTOQuery()
        {
            var testContext = (TestEcaContext)Context;
            return testContext.LookupServiceTestDTOs.Select(x => new LookupServiceTestDTO
            {
                Id = x.Id,
                Value = x.Value
            });
        }
    }

    [TestClass]
    public class LookupServiceTest
    {
        private TestEcaContext context;
        private LookupServiceTestServiceClass service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new LookupServiceTestServiceClass(context);
        }

        #region Get
        [TestMethod]
        public async Task TestGetProjectStati_DefaultSort()
        {
            var instance1 = new LookupServiceTestDTO
            {
                Id = 1,
                Value = "A"
            };
            var instance2 = new LookupServiceTestDTO
            {
                Id = 2,
                Value = "B"
            };

            var list = new List<LookupServiceTestDTO> { instance1, instance2 };
            list = list.OrderBy(x => x.Value).ToList();
            list.ForEach(x => context.LookupServiceTestDTOs.Add(x));
            Action<PagedQueryResults<LookupServiceTestDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(2, results.Results.Count);
                Assert.AreEqual(instance2.Id, results.Results.First().Id);
                Assert.AreEqual(instance1.Id, results.Results.Last().Id);
            };

            var defaultSorter = new ExpressionSorter<LookupServiceTestDTO>(x => x.Value, SortDirection.Descending);
            var queryOperator = new QueryableOperator<LookupServiceTestDTO>(0, 10, defaultSorter);
            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectStati_Sort()
        {
            var instance1 = new LookupServiceTestDTO
            {
                Id = 1,
                Value = "A"
            };
            var instance2 = new LookupServiceTestDTO
            {
                Id = 2,
                Value = "B"
            };

            var list = new List<LookupServiceTestDTO> { instance1, instance2 };
            list = list.OrderByDescending(x => x.Value).ToList();
            list.ForEach(x => context.LookupServiceTestDTOs.Add(x));
            Action<PagedQueryResults<LookupServiceTestDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(2, results.Results.Count);
                Assert.AreEqual(instance1.Id, results.Results.First().Id);
                Assert.AreEqual(instance2.Id, results.Results.Last().Id);
            };

            var defaultSorter = new ExpressionSorter<LookupServiceTestDTO>(x => x.Value, SortDirection.Descending);
            var sorter = new ExpressionSorter<LookupServiceTestDTO>(x => x.Value, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<LookupServiceTestDTO>(0, 10, defaultSorter, null, new List<ISorter> { sorter });
            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }


        [TestMethod]
        public async Task TestGetProjectStati_Filtered()
        {
            var instance1 = new LookupServiceTestDTO
            {
                Id = 1,
                Value = "A"
            };
            var instance2 = new LookupServiceTestDTO
            {
                Id = 2,
                Value = "B"
            };

            var list = new List<LookupServiceTestDTO> { instance1, instance2 };
            list = list.OrderByDescending(x => x.Value).ToList();
            list.ForEach(x => context.LookupServiceTestDTOs.Add(x));
            Action<PagedQueryResults<LookupServiceTestDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(instance1.Id, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<LookupServiceTestDTO>(x => x.Value, SortDirection.Descending);
            var filter = new ExpressionFilter<LookupServiceTestDTO>(x => x.Value, ComparisonType.Equal, instance1.Value);
            var queryOperator = new QueryableOperator<LookupServiceTestDTO>(0, 10, defaultSorter, new List<IFilter> { filter }, null);
            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectStati_Paged()
        {
            var instance1 = new LookupServiceTestDTO
            {
                Id = 1,
                Value = "A"
            };
            var instance2 = new LookupServiceTestDTO
            {
                Id = 2,
                Value = "B"
            };

            var list = new List<LookupServiceTestDTO> { instance1, instance2 };
            list = list.OrderByDescending(x => x.Value).ToList();
            list.ForEach(x => context.LookupServiceTestDTOs.Add(x));
            Action<PagedQueryResults<LookupServiceTestDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(instance2.Id, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<LookupServiceTestDTO>(x => x.Value, SortDirection.Descending);
            var queryOperator = new QueryableOperator<LookupServiceTestDTO>(0, 1, defaultSorter, null, null);
            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }


        #endregion
    }
}
