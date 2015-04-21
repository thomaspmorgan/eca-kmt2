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
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class LocationServiceTest
    {
        private TestEcaContext context;
        private LocationService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new LocationService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGetLocations_CheckProperties()
        {
            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var location = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType
            };
            context.Locations.Add(location);
            context.LocationTypes.Add(locationType);
            Action<PagedQueryResults<LocationDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(location.LocationId, firstResult.Id);
                Assert.AreEqual(location.LocationName, firstResult.Name);
                Assert.AreEqual(location.LocationTypeId, firstResult.LocationTypeId);
                Assert.AreEqual(locationType.LocationTypeName, firstResult.LocationTypeName);
            };
            var defaultSorter = new ExpressionSorter<LocationDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<LocationDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Validation
        [TestMethod]
        public async Task TestGetLocationTypeIds()
        {
            var region1 = new Location
            {
                LocationTypeId = LocationType.Region.Id,
                LocationId = 10
            };
            var region2 = new Location
            {
                LocationTypeId = LocationType.Region.Id,
                LocationId = 20
            };
            var city = new Location
            {
                LocationTypeId = LocationType.City.Id,
                LocationId = 30
            };
            context.Locations.Add(region1);
            context.Locations.Add(region2);
            context.Locations.Add(city);

            Action<List<int>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Count);
                Assert.IsTrue(results.Contains(region1.LocationTypeId));
                Assert.IsTrue(results.Contains(city.LocationTypeId));
            };

            var serviceResults = service.GetLocationTypeIds(context.Locations.Select(x => x.LocationId).ToList());
            var serviceResultsAsync = await service.GetLocationTypeIdsAsync(context.Locations.Select(x => x.LocationId).ToList());
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetLocationTypeIds_NoIdes()
        {
            Action<List<int>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Count);
            };

            var serviceResults = service.GetLocationTypeIds(new List<int>());
            var serviceResultsAsync = await service.GetLocationTypeIdsAsync(new List<int>());
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

    }
}
