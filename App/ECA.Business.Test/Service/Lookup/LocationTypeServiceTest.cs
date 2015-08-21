using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Lookup;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;

namespace ECA.Business.Test.Service.Lookup
{
    [TestClass]
    public class LocationTypeServiceTest
    {
        private TestEcaContext context;
        private LocationTypeService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new LocationTypeService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }
        #region Get
        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var locationType = new LocationType
            {
                LocationTypeName = LocationType.City.Value,
                LocationTypeId = LocationType.City.Id
            };

            context.LocationTypes.Add(locationType);
            Action<PagedQueryResults<SimpleLookupDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(locationType.LocationTypeId, firstResult.Id);
                Assert.AreEqual(locationType.LocationTypeName, firstResult.Value);
            };
            var defaultSorter = new ExpressionSorter<SimpleLookupDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleLookupDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion
    }
}
