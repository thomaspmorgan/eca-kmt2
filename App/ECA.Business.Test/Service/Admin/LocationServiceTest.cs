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
    public class LocationServiceTest
    {
        private TestEcaContext context;
        private LocationService service;

        [TestInitialize]
        public void TestInit()
        {
            context = DbContextHelper.GetInMemoryContext();
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

            var serviceResults = service.GetLocations(queryOperator);
            var serviceResultsAsync = await service.GetLocationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_DefaultSorter()
        {
            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var location1 = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType
            };
            var location2 = new Location
            {
                LocationId = 3,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType
            };
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.LocationTypes.Add(locationType);
            Action<PagedQueryResults<LocationDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(location2.LocationId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<LocationDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<LocationDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetLocations(queryOperator);
            var serviceResultsAsync = await service.GetLocationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_Filter()
        {
            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var location1 = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType
            };
            var location2 = new Location
            {
                LocationId = 3,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType
            };
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.LocationTypes.Add(locationType);
            Action<PagedQueryResults<LocationDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(location2.LocationId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<LocationDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<LocationDTO>(0, 1, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<LocationDTO>(x => x.Name, ComparisonType.Equal, location1.LocationName));

            var serviceResults = service.GetLocations(queryOperator);
            var serviceResultsAsync = await service.GetLocationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_Sort()
        {
            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var location1 = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType
            };
            var location2 = new Location
            {
                LocationId = 3,
                LocationName = "xyz",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType
            };
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.LocationTypes.Add(locationType);
            Action<PagedQueryResults<LocationDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(location2.LocationId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<LocationDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<LocationDTO>(0, 1, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<LocationDTO>(x => x.Name, SortDirection.Descending));

            var serviceResults = service.GetLocations(queryOperator);
            var serviceResultsAsync = await service.GetLocationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_Paging()
        {
            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var location1 = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType
            };
            var location2 = new Location
            {
                LocationId = 3,
                LocationName = "xyz",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType
            };
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.LocationTypes.Add(locationType);
            Action<PagedQueryResults<LocationDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(2, results.Results.Count);
                
            };
            var defaultSorter = new ExpressionSorter<LocationDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<LocationDTO>(0, 2, defaultSorter);

            var serviceResults = service.GetLocations(queryOperator);
            var serviceResultsAsync = await service.GetLocationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

    }
}
