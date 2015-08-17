using System;
using FluentAssertions;
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
using Microsoft.QualityTools.Testing.Fakes;
using ECA.Business.Service;
using ECA.Core.Exceptions;
using ECA.Business.Validation;
using Moq;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class LocationServiceTest
    {
        private TestEcaContext context;
        private LocationService service;
        private Mock<IBusinessValidator<EcaAddressValidationEntity, EcaAddressValidationEntity>> addressValidator;
        private Mock<IBusinessValidator<LocationValidationEntity, LocationValidationEntity>> locationValidator;

        [TestInitialize]
        public void TestInit()
        {
            addressValidator = new Mock<IBusinessValidator<EcaAddressValidationEntity, EcaAddressValidationEntity>>();
            locationValidator = new Mock<IBusinessValidator<LocationValidationEntity, LocationValidationEntity>>();
            context = new TestEcaContext();
            service = new LocationService(context, locationValidator.Object, addressValidator.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region GetLocationById

        [TestMethod]
        public async Task TestGetLocationById_CheckProperties()
        {
            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var city = new Location
            {
                LocationId = 100,
                LocationName = "city",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var country = new Location
            {
                LocationId = 101,
                LocationName = "country",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var region = new Location
            {
                LocationId = 102,
                LocationName = "region",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var division = new Location
            {
                LocationId = 103,
                LocationName = "division",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };

            var location = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                Region = region,
                RegionId = region.LocationId,
                Longitude = 1.0f,
                Latitude = 2.0f,
                LocationIso = "iso",
                LocationIso2 = "iso2"
            };
            context.Locations.Add(region);
            context.Locations.Add(city);
            context.Locations.Add(country);
            context.Locations.Add(division);
            context.Locations.Add(location);
            context.LocationTypes.Add(locationType);
            Action<LocationDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);
                Assert.AreEqual(location.LocationId, result.Id);
                Assert.AreEqual(location.LocationName, result.Name);
                Assert.AreEqual(location.LocationTypeId, result.LocationTypeId);
                Assert.AreEqual(locationType.LocationTypeName, result.LocationTypeName);
                Assert.AreEqual(country.LocationId, result.CountryId);
                Assert.AreEqual(country.LocationName, result.Country);
                Assert.AreEqual(region.LocationId, result.RegionId);
                Assert.AreEqual(region.LocationName, result.Region);
                Assert.AreEqual(city.LocationId, result.CityId);
                Assert.AreEqual(city.LocationName, result.City);
                Assert.AreEqual(division.LocationId, result.DivisionId);
                Assert.AreEqual(division.LocationName, result.Division);
                Assert.AreEqual(location.Longitude, result.Longitude);
                Assert.AreEqual(location.Latitude, result.Latitude);
                Assert.AreEqual(location.LocationIso, result.LocationIso);
                Assert.AreEqual(location.LocationIso2, result.LocationIso2);
            };
            var serviceResults = service.GetLocationById(location.LocationId);
            var serviceResultsAsync = await service.GetLocationByIdAsync(location.LocationId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetLocationById_DoesNotHaveCity()
        {
            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var country = new Location
            {
                LocationId = 101,
                LocationName = "country",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var region = new Location
            {
                LocationId = 102,
                LocationName = "region",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var division = new Location
            {
                LocationId = 103,
                LocationName = "division",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };

            var location = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                Region = region,
                RegionId = region.LocationId,
                Longitude = 1.0f,
                Latitude = 2.0f,
                LocationIso = "iso",
                LocationIso2 = "iso2"
            };
            context.Locations.Add(region);
            context.Locations.Add(country);
            context.Locations.Add(division);
            context.Locations.Add(location);
            context.LocationTypes.Add(locationType);
            Action<LocationDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);
            };
            var serviceResults = service.GetLocationById(location.LocationId);
            var serviceResultsAsync = await service.GetLocationByIdAsync(location.LocationId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetLocationById_DoesNotHaveCountry()
        {
            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var city = new Location
            {
                LocationId = 100,
                LocationName = "city",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var region = new Location
            {
                LocationId = 102,
                LocationName = "region",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var division = new Location
            {
                LocationId = 103,
                LocationName = "division",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };

            var location = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
                City = city,
                CityId = city.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                Region = region,
                RegionId = region.LocationId,
                Longitude = 1.0f,
                Latitude = 2.0f,
                LocationIso = "iso",
                LocationIso2 = "iso2"
            };
            context.Locations.Add(region);
            context.Locations.Add(city);
            context.Locations.Add(division);
            context.Locations.Add(location);
            context.LocationTypes.Add(locationType);
            Action<LocationDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);
            };
            var serviceResults = service.GetLocationById(location.LocationId);
            var serviceResultsAsync = await service.GetLocationByIdAsync(location.LocationId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetLocationById_DoesNotHaveRegion()
        {
            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var city = new Location
            {
                LocationId = 100,
                LocationName = "city",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var country = new Location
            {
                LocationId = 101,
                LocationName = "country",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var division = new Location
            {
                LocationId = 103,
                LocationName = "division",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };

            var location = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                Longitude = 1.0f,
                Latitude = 2.0f,
                LocationIso = "iso",
                LocationIso2 = "iso2"
            };
            context.Locations.Add(city);
            context.Locations.Add(country);
            context.Locations.Add(division);
            context.Locations.Add(location);
            context.LocationTypes.Add(locationType);
            Action<LocationDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);
            };
            var serviceResults = service.GetLocationById(location.LocationId);
            var serviceResultsAsync = await service.GetLocationByIdAsync(location.LocationId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetLocationById_DoesNotHaveDivision()
        {
            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var city = new Location
            {
                LocationId = 100,
                LocationName = "city",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var country = new Location
            {
                LocationId = 101,
                LocationName = "country",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var region = new Location
            {
                LocationId = 102,
                LocationName = "region",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var location = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Region = region,
                RegionId = region.LocationId,
                Longitude = 1.0f,
                Latitude = 2.0f,
                LocationIso = "iso",
                LocationIso2 = "iso2"
            };
            context.Locations.Add(region);
            context.Locations.Add(city);
            context.Locations.Add(country);
            context.Locations.Add(location);
            context.LocationTypes.Add(locationType);
            Action<LocationDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);
            };
            var serviceResults = service.GetLocationById(location.LocationId);
            var serviceResultsAsync = await service.GetLocationByIdAsync(location.LocationId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetLocationById_LocationDoesNotExist()
        {
            Action<LocationDTO> tester = (result) =>
            {
                Assert.IsNull(result);
            };
            var serviceResults = service.GetLocationById(1);
            var serviceResultsAsync = await service.GetLocationByIdAsync(1);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Get
        [TestMethod]
        public async Task TestGetLocations_CheckProperties()
        {

            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var city = new Location
            {
                LocationId = 100,
                LocationName = "city",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var country = new Location
            {
                LocationId = 101,
                LocationName = "country",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var region = new Location
            {
                LocationId = 102,
                LocationName = "region",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var division = new Location
            {
                LocationId = 103,
                LocationName = "division",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };

            var location = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                Region = region,
                RegionId = region.LocationId,
                Longitude = 1.0f,
                Latitude = 2.0f,
                LocationIso = "iso",
                LocationIso2 = "iso2"
            };
            context.Locations.Add(region);
            context.Locations.Add(city);
            context.Locations.Add(country);
            context.Locations.Add(division);
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
                Assert.AreEqual(country.LocationId, firstResult.CountryId);
                Assert.AreEqual(country.LocationName, firstResult.Country);
                Assert.AreEqual(region.LocationId, firstResult.RegionId);
                Assert.AreEqual(region.LocationName, firstResult.Region);
                Assert.AreEqual(city.LocationId, firstResult.CityId);
                Assert.AreEqual(city.LocationName, firstResult.City);
                Assert.AreEqual(division.LocationId, firstResult.DivisionId);
                Assert.AreEqual(division.LocationName, firstResult.Division);
                Assert.AreEqual(location.Longitude, firstResult.Longitude);
                Assert.AreEqual(location.Latitude, firstResult.Latitude);
                Assert.AreEqual(location.LocationIso, firstResult.LocationIso);
                Assert.AreEqual(location.LocationIso2, firstResult.LocationIso2);
            };
            var defaultSorter = new ExpressionSorter<LocationDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<LocationDTO>(0, 10, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<LocationDTO>(x => x.Id, ComparisonType.Equal, location.LocationId));
            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetLocations_DoesNotHaveLatitudeLongitude()
        {

            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var city = new Location
            {
                LocationId = 100,
                LocationName = "city",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var country = new Location
            {
                LocationId = 101,
                LocationName = "country",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var region = new Location
            {
                LocationId = 102,
                LocationName = "region",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var division = new Location
            {
                LocationId = 103,
                LocationName = "division",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };

            var location = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                Region = region,
                RegionId = region.LocationId,
            };
            context.Locations.Add(region);
            context.Locations.Add(city);
            context.Locations.Add(country);
            context.Locations.Add(division);
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
                Assert.AreEqual(country.LocationId, firstResult.CountryId);
                Assert.AreEqual(country.LocationName, firstResult.Country);
                Assert.AreEqual(region.LocationId, firstResult.RegionId);
                Assert.AreEqual(region.LocationName, firstResult.Region);
                Assert.AreEqual(city.LocationId, firstResult.CityId);
                Assert.AreEqual(city.LocationName, firstResult.City);
                Assert.AreEqual(division.LocationId, firstResult.DivisionId);
                Assert.AreEqual(division.LocationName, firstResult.Division);
                Assert.IsFalse(firstResult.Longitude.HasValue);
                Assert.IsFalse(firstResult.Latitude.HasValue);
            };
            var defaultSorter = new ExpressionSorter<LocationDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<LocationDTO>(0, 10, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<LocationDTO>(x => x.Id, ComparisonType.Equal, location.LocationId));
            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetLocations_DoesNotHaveRegion()
        {

            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var city = new Location
            {
                LocationId = 100,
                LocationName = "city",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var country = new Location
            {
                LocationId = 101,
                LocationName = "country",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var division = new Location
            {
                LocationId = 103,
                LocationName = "division",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };

            var location = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
            };
            context.Locations.Add(city);
            context.Locations.Add(country);
            context.Locations.Add(division);
            context.Locations.Add(location);
            context.LocationTypes.Add(locationType);
            Action<PagedQueryResults<LocationDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.IsFalse(firstResult.RegionId.HasValue);
                Assert.IsNull(firstResult.Region);
            };
            var defaultSorter = new ExpressionSorter<LocationDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<LocationDTO>(0, 10, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<LocationDTO>(x => x.Id, ComparisonType.Equal, location.LocationId));
            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetLocations_DoesNotHaveCountry()
        {

            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var city = new Location
            {
                LocationId = 100,
                LocationName = "city",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var region = new Location
            {
                LocationId = 102,
                LocationName = "region",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var division = new Location
            {
                LocationId = 103,
                LocationName = "division",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };

            var location = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
                City = city,
                CityId = city.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                Region = region,
                RegionId = region.LocationId,
            };
            context.Locations.Add(region);
            context.Locations.Add(city);
            context.Locations.Add(division);
            context.Locations.Add(location);
            context.LocationTypes.Add(locationType);
            Action<PagedQueryResults<LocationDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.IsFalse(firstResult.CountryId.HasValue);
                Assert.IsNull(firstResult.Country);
            };
            var defaultSorter = new ExpressionSorter<LocationDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<LocationDTO>(0, 10, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<LocationDTO>(x => x.Id, ComparisonType.Equal, location.LocationId));
            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetLocations_DoesNotHaveDivision()
        {

            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var city = new Location
            {
                LocationId = 100,
                LocationName = "city",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var country = new Location
            {
                LocationId = 101,
                LocationName = "country",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var region = new Location
            {
                LocationId = 102,
                LocationName = "region",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };

            var location = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Region = region,
                RegionId = region.LocationId,
            };
            context.Locations.Add(region);
            context.Locations.Add(city);
            context.Locations.Add(country);
            context.Locations.Add(location);
            context.LocationTypes.Add(locationType);
            Action<PagedQueryResults<LocationDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.IsFalse(firstResult.DivisionId.HasValue);
                Assert.IsNull(firstResult.Division);
            };
            var defaultSorter = new ExpressionSorter<LocationDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<LocationDTO>(0, 10, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<LocationDTO>(x => x.Id, ComparisonType.Equal, location.LocationId));
            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetLocations_DoesNotHaveCity()
        {

            var locationType = new LocationType
            {
                LocationTypeId = 1,
                LocationTypeName = "type"
            };
            var country = new Location
            {
                LocationId = 101,
                LocationName = "country",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var region = new Location
            {
                LocationId = 102,
                LocationName = "region",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };
            var division = new Location
            {
                LocationId = 103,
                LocationName = "division",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
            };

            var location = new Location
            {
                LocationId = 2,
                LocationName = "abc",
                LocationTypeId = locationType.LocationTypeId,
                LocationType = locationType,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                Region = region,
                RegionId = region.LocationId,
            };
            context.Locations.Add(region);
            context.Locations.Add(country);
            context.Locations.Add(division);
            context.Locations.Add(location);
            context.LocationTypes.Add(locationType);
            Action<PagedQueryResults<LocationDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.IsFalse(firstResult.CityId.HasValue);
                Assert.IsNull(firstResult.City);
            };
            var defaultSorter = new ExpressionSorter<LocationDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<LocationDTO>(0, 10, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<LocationDTO>(x => x.Id, ComparisonType.Equal, location.LocationId));
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

        #region Address

        [TestMethod]
        public async Task TestDelete()
        {
            var location = new Location
            {
                LocationId = 1
            };
            var address = new Address
            {
                AddressId = 1,
                Location = location,
                LocationId = location.LocationId
            };
            context.SetupActions.Add(() =>
            {
                context.Locations.Add(location);
                context.Addresses.Add(address);
            });
            Action beforeTester = () =>
            {
                Assert.AreEqual(1, context.Addresses.Count());
                Assert.AreEqual(1, context.Locations.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(0, context.Addresses.Count());
                Assert.AreEqual(0, context.Locations.Count());
            };
            context.Revert();
            beforeTester();
            service.Delete(address.AddressId);
            afterTester();

            context.Revert();
            beforeTester();
            await service.DeleteAsync(address.AddressId);
            afterTester();

        }

        [TestMethod]
        public async Task TestDelete_LocationDoesNotExist()
        {
            var address = new Address
            {
                AddressId = 1,
            };
            context.SetupActions.Add(() =>
            {
                context.Addresses.Add(address);
            });
            Action beforeTester = () =>
            {
                Assert.AreEqual(1, context.Addresses.Count());
                Assert.AreEqual(0, context.Locations.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(0, context.Addresses.Count());
                Assert.AreEqual(0, context.Locations.Count());
            };
            context.Revert();
            beforeTester();
            service.Delete(address.AddressId);
            afterTester();

            context.Revert();
            beforeTester();
            await service.DeleteAsync(address.AddressId);
            afterTester();

        }

        [TestMethod]
        public async Task TestDelete_AddressDoesNotExist()
        {
            var location = new Location
            {
                LocationId = 1
            };
            var address = new Address
            {
                AddressId = 1,
                Location = location,
                LocationId = location.LocationId
            };
            context.Addresses.Add(address);
            context.Locations.Add(location);
            Action tester = () =>
            {
                Assert.AreEqual(1, context.Addresses.Count());
                Assert.AreEqual(1, context.Locations.Count());
            };
            service.Delete(2);
            tester();

            await service.DeleteAsync(2);
            tester();
        }

        [TestMethod]
        public async Task TestCreate_Address_CheckProperties()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Organization>((c) =>
                {
                    return context.Organizations;
                });

                var organizationId = 5;
                var organization = new Organization
                {
                    OrganizationId = organizationId
                };
                var city = new Location
                {
                    LocationId = 1,
                };
                var country = new Location
                {
                    LocationId = 2,
                };
                var division = new Location
                {
                    LocationId = 3,
                };
                var userId = 1;
                var user = new User(userId);
                var addressTypeId = AddressType.Business.Id;
                var isPrimary = true;
                var street1 = "street1";
                var street2 = "street2";
                var street3 = "street3";
                var postalCode = "12345";
                var locationName = "location name";
                var countryId = country.LocationId;
                var cityId = city.LocationId;
                var divisionId = division.LocationId;

                var instance = new AdditionalOrganizationAddress(
                    user,
                    addressTypeId,
                    isPrimary,
                    street1,
                    street2,
                    street3,
                    postalCode,
                    locationName,
                    countryId,
                    cityId,
                    divisionId,
                    organizationId
                );
                context.SetupActions.Add(() =>
                {
                    context.Organizations.Add(organization);
                    organization.Addresses.ToList().ForEach(x => x.Location = null);
                    organization.Addresses.Clear();
                    context.Addresses.RemoveRange(context.Addresses.ToList());
                    context.Locations.RemoveRange(context.Locations.ToList());
                    context.Locations.Add(city);
                    context.Locations.Add(country);
                    context.Locations.Add(division);
                });
                Action beforeTester = () =>
                {
                    Assert.AreEqual(1, context.Organizations.Count());
                    Assert.AreEqual(0, organization.Addresses.Count);
                    Assert.AreEqual(0, context.Addresses.Count());
                    Assert.AreEqual(3, context.Locations.Count());
                };

                Action afterTester = () =>
                {
                    Assert.AreEqual(1, context.Organizations.Count());
                    Assert.AreEqual(1, context.Addresses.Count());
                    Assert.AreEqual(4, context.Locations.Count());

                    Assert.AreEqual(1, organization.Addresses.Count);
                    var organizationAddress = organization.Addresses.First();
                    Assert.IsNotNull(organizationAddress.Location);
                    var location = organizationAddress.Location;

                    Assert.AreEqual(addressTypeId, organizationAddress.AddressTypeId);
                    Assert.AreEqual(isPrimary, organizationAddress.IsPrimary);
                    Assert.AreEqual(userId, organizationAddress.History.CreatedBy);
                    Assert.AreEqual(userId, organizationAddress.History.RevisedBy);
                    DateTimeOffset.Now.Should().BeCloseTo(organizationAddress.History.CreatedOn, 2000);
                    DateTimeOffset.Now.Should().BeCloseTo(organizationAddress.History.RevisedOn, 2000);

                    Assert.AreEqual(street1, location.Street1);
                    Assert.AreEqual(street2, location.Street2);
                    Assert.AreEqual(street3, location.Street3);
                    Assert.AreEqual(postalCode, location.PostalCode);
                    Assert.AreEqual(street3, location.Street3);
                    Assert.AreEqual(locationName, location.LocationName);
                    Assert.AreEqual(countryId, location.CountryId);
                    Assert.AreEqual(cityId, location.CityId);
                    Assert.AreEqual(divisionId, location.DivisionId);
                    Assert.AreEqual(userId, location.History.CreatedBy);
                    Assert.AreEqual(userId, location.History.RevisedBy);
                    DateTimeOffset.Now.Should().BeCloseTo(location.History.CreatedOn, 2000);
                    DateTimeOffset.Now.Should().BeCloseTo(location.History.RevisedOn, 2000);
                };

                context.Revert();
                beforeTester();
                service.Create<Organization>(instance);
                afterTester();
                addressValidator.Verify(x => x.ValidateCreate(It.IsAny<EcaAddressValidationEntity>()), Times.Once());

                context.Revert();
                beforeTester();
                await service.CreateAsync<Organization>(instance);
                afterTester();
                addressValidator.Verify(x => x.ValidateCreate(It.IsAny<EcaAddressValidationEntity>()), Times.Exactly(2));
            }
        }

        [TestMethod]
        public async Task TestCreate_AddressableHasPrimaryAddress()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Organization>((c) =>
                {
                    return context.Organizations;
                });

                var organizationId = 5;
                var organization = new Organization
                {
                    OrganizationId = organizationId
                };
                var city = new Location
                {
                    LocationId = 1,
                };
                var country = new Location
                {
                    LocationId = 2,
                };
                var division = new Location
                {
                    LocationId = 3,
                };
                var existingPrimaryAddress = new Address
                {
                    AddressId = 10,
                    IsPrimary = true,
                    Organization = organization,
                    OrganizationId = organization.OrganizationId
                };

                var userId = 1;
                var user = new User(userId);
                var addressTypeId = AddressType.Business.Id;
                var isPrimary = true;
                var street1 = "street1";
                var street2 = "street2";
                var street3 = "street3";
                var postalCode = "12345";
                var locationName = "location name";
                var countryId = country.LocationId;
                var cityId = city.LocationId;
                var divisionId = division.LocationId;

                var instance = new AdditionalOrganizationAddress(
                    user,
                    addressTypeId,
                    isPrimary,
                    street1,
                    street2,
                    street3,
                    postalCode,
                    locationName,
                    countryId,
                    cityId,
                    divisionId,
                    organizationId
                );

                context.SetupActions.Add(() =>
                {
                    context.Organizations.Add(organization);
                    context.Addresses.Add(existingPrimaryAddress);
                    existingPrimaryAddress.IsPrimary = true;
                    organization.Addresses.Clear();
                    organization.Addresses.Add(existingPrimaryAddress);
                    context.Locations.Add(city);
                    context.Locations.Add(country);
                    context.Locations.Add(division);

                });
                Action beforeTester = () =>
                {

                    Assert.AreEqual(1, context.Organizations.Count());
                    Assert.AreEqual(1, organization.Addresses.Count);
                    Assert.AreEqual(1, context.Addresses.Count());
                    Assert.AreEqual(3, context.Locations.Count());
                };

                Action afterTester = () =>
                {
                    Assert.IsFalse(existingPrimaryAddress.IsPrimary);
                    Assert.AreEqual(2, context.Addresses.Count());
                    var addedAddress = context.Addresses.Where(x => x.AddressId != existingPrimaryAddress.AddressId).FirstOrDefault();
                    Assert.IsTrue(addedAddress.IsPrimary);
                };

                context.Revert();
                beforeTester();
                service.Create<Organization>(instance);
                afterTester();
                addressValidator.Verify(x => x.ValidateCreate(It.IsAny<EcaAddressValidationEntity>()), Times.Once());

                context.Revert();
                beforeTester();
                await service.CreateAsync<Organization>(instance);
                afterTester();
                addressValidator.Verify(x => x.ValidateCreate(It.IsAny<EcaAddressValidationEntity>()), Times.Exactly(2));
            }
        }

        [TestMethod]
        public async Task TestCreate_AddressableDoesNotHavePrimaryAddress()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Organization>((c) =>
                {
                    return context.Organizations;
                });

                var organizationId = 5;
                var organization = new Organization
                {
                    OrganizationId = organizationId
                };
                var city = new Location
                {
                    LocationId = 1,
                };
                var country = new Location
                {
                    LocationId = 2,
                };
                var division = new Location
                {
                    LocationId = 3,
                };
                var existingPrimaryAddress = new Address
                {
                    AddressId = 10,
                    IsPrimary = false,
                    Organization = organization,
                    OrganizationId = organization.OrganizationId
                };

                var userId = 1;
                var user = new User(userId);
                var addressTypeId = AddressType.Business.Id;
                var isPrimary = false;
                var street1 = "street1";
                var street2 = "street2";
                var street3 = "street3";
                var postalCode = "12345";
                var locationName = "location name";
                var countryId = country.LocationId;
                var cityId = city.LocationId;
                var divisionId = division.LocationId;

                var instance = new AdditionalOrganizationAddress(
                    user,
                    addressTypeId,
                    isPrimary,
                    street1,
                    street2,
                    street3,
                    postalCode,
                    locationName,
                    countryId,
                    cityId,
                    divisionId,
                    organizationId
                );

                context.SetupActions.Add(() =>
                {
                    context.Organizations.Add(organization);
                    context.Addresses.Add(existingPrimaryAddress);
                    existingPrimaryAddress.IsPrimary = false;
                    organization.Addresses.Clear();
                    organization.Addresses.Add(existingPrimaryAddress);
                    context.Locations.Add(city);
                    context.Locations.Add(country);
                    context.Locations.Add(division);

                });
                Action beforeTester = () =>
                {

                    Assert.AreEqual(1, context.Organizations.Count());
                    Assert.AreEqual(1, organization.Addresses.Count);
                    Assert.AreEqual(1, context.Addresses.Count());
                    Assert.AreEqual(3, context.Locations.Count());
                };

                Action afterTester = () =>
                {
                    Assert.IsFalse(existingPrimaryAddress.IsPrimary);
                    Assert.AreEqual(2, context.Addresses.Count());
                    Assert.AreEqual(2, context.Addresses.Where(x => !x.IsPrimary).Count());
                };

                context.Revert();
                beforeTester();
                service.Create<Organization>(instance);
                afterTester();
                addressValidator.Verify(x => x.ValidateCreate(It.IsAny<EcaAddressValidationEntity>()), Times.Once());

                context.Revert();
                beforeTester();
                await service.CreateAsync<Organization>(instance);
                afterTester();
                addressValidator.Verify(x => x.ValidateCreate(It.IsAny<EcaAddressValidationEntity>()), Times.Exactly(2));
            }
        }

        [TestMethod]
        public async Task TestCreate_OrganizationDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Organization>((c) =>
                {
                    return context.Organizations;
                });

                var organizationId = 5;
                var userId = 1;
                var user = new User(userId);
                var addressTypeId = AddressType.Business.Id;
                var isPrimary = true;
                var street1 = "street1";
                var street2 = "street2";
                var street3 = "street3";
                var postalCode = "12345";
                var locationName = "location name";
                var countryId = 2;
                var cityId = 3;
                var divisionId = 4;

                var instance = new AdditionalOrganizationAddress(
                    user,
                    addressTypeId,
                    isPrimary,
                    street1,
                    street2,
                    street3,
                    postalCode,
                    locationName,
                    countryId,
                    cityId,
                    divisionId,
                    organizationId
                );
                context.Revert();
                Func<Task> f = () =>
                {
                    return service.CreateAsync<Organization>(instance);
                };
                service.Invoking(x => x.Create<Organization>(instance)).ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The model type [{0}] with Id [{1}] was not found.", typeof(Organization).Name, organizationId));
                f.ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The model type [{0}] with Id [{1}] was not found.", typeof(Organization).Name, organizationId));
            }
        }

        [TestMethod]
        public async Task TestCreate_CityDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Organization>((c) =>
                {
                    return context.Organizations;
                });

                var organizationId = 5;
                var organization = new Organization
                {
                    OrganizationId = organizationId
                };
                var country = new Location
                {
                    LocationId = 2,
                };
                var division = new Location
                {
                    LocationId = 3,
                };
                var userId = 1;
                var user = new User(userId);
                var addressTypeId = AddressType.Business.Id;
                var isPrimary = true;
                var street1 = "street1";
                var street2 = "street2";
                var street3 = "street3";
                var postalCode = "12345";
                var locationName = "location name";
                var countryId = country.LocationId;
                var cityId = 1;
                var divisionId = division.LocationId;

                var instance = new AdditionalOrganizationAddress(
                    user,
                    addressTypeId,
                    isPrimary,
                    street1,
                    street2,
                    street3,
                    postalCode,
                    locationName,
                    countryId,
                    cityId,
                    divisionId,
                    organizationId
                );
                context.Organizations.Add(organization);
                organization.Addresses.ToList().ForEach(x => x.Location = null);
                organization.Addresses.Clear();
                context.Addresses.RemoveRange(context.Addresses.ToList());
                context.Locations.RemoveRange(context.Locations.ToList());
                context.Locations.Add(country);
                context.Locations.Add(division);

                Func<Task> f = () =>
                {
                    return service.CreateAsync<Organization>(instance);
                };

                service.Invoking(x => x.Create<Organization>(instance)).ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The [{0}] with id [{1}] was not found.", "City", cityId));

                f.ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The [{0}] with id [{1}] was not found.", "City", cityId));
            }
        }

        [TestMethod]
        public async Task TestCreate_CountryDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Organization>((c) =>
                {
                    return context.Organizations;
                });

                var organizationId = 5;
                var organization = new Organization
                {
                    OrganizationId = organizationId
                };
                var city = new Location
                {
                    LocationId = 2,
                };
                var division = new Location
                {
                    LocationId = 3,
                };
                var userId = 1;
                var user = new User(userId);
                var addressTypeId = AddressType.Business.Id;
                var isPrimary = true;
                var street1 = "street1";
                var street2 = "street2";
                var street3 = "street3";
                var postalCode = "12345";
                var locationName = "location name";
                var countryId = 1;
                var cityId = city.LocationId;
                var divisionId = division.LocationId;

                var instance = new AdditionalOrganizationAddress(
                    user,
                    addressTypeId,
                    isPrimary,
                    street1,
                    street2,
                    street3,
                    postalCode,
                    locationName,
                    countryId,
                    cityId,
                    divisionId,
                    organizationId
                );
                context.Organizations.Add(organization);
                organization.Addresses.ToList().ForEach(x => x.Location = null);
                organization.Addresses.Clear();
                context.Addresses.RemoveRange(context.Addresses.ToList());
                context.Locations.RemoveRange(context.Locations.ToList());
                context.Locations.Add(city);
                context.Locations.Add(division);

                Func<Task> f = () =>
                {
                    return service.CreateAsync<Organization>(instance);
                };

                service.Invoking(x => x.Create<Organization>(instance)).ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The [{0}] with id [{1}] was not found.", "Country", countryId));

                f.ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The [{0}] with id [{1}] was not found.", "Country", countryId));
            }
        }

        [TestMethod]
        public async Task TestCreate_DivisionDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetOf1<Organization>((c) =>
                {
                    return context.Organizations;
                });

                var organizationId = 5;
                var organization = new Organization
                {
                    OrganizationId = organizationId
                };
                var city = new Location
                {
                    LocationId = 2,
                };
                var country = new Location
                {
                    LocationId = 3,
                };
                var userId = 1;
                var user = new User(userId);
                var addressTypeId = AddressType.Business.Id;
                var isPrimary = true;
                var street1 = "street1";
                var street2 = "street2";
                var street3 = "street3";
                var postalCode = "12345";
                var locationName = "location name";
                var countryId = country.LocationId;
                var cityId = city.LocationId;
                var divisionId = 1;

                var instance = new AdditionalOrganizationAddress(
                    user,
                    addressTypeId,
                    isPrimary,
                    street1,
                    street2,
                    street3,
                    postalCode,
                    locationName,
                    countryId,
                    cityId,
                    divisionId,
                    organizationId
                );
                context.Organizations.Add(organization);
                organization.Addresses.ToList().ForEach(x => x.Location = null);
                organization.Addresses.Clear();
                context.Addresses.RemoveRange(context.Addresses.ToList());
                context.Locations.RemoveRange(context.Locations.ToList());
                context.Locations.Add(city);
                context.Locations.Add(country);

                Func<Task> f = () =>
                {
                    return service.CreateAsync<Organization>(instance);
                };

                service.Invoking(x => x.Create<Organization>(instance)).ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The [{0}] with id [{1}] was not found.", "Division", divisionId));

                f.ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The [{0}] with id [{1}] was not found.", "Division", divisionId));
            }
        }

        [TestMethod]
        public async Task TestUpdate()
        {
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var businessAddressType = new AddressType
            {
                AddressName = AddressType.Business.Value,
                AddressTypeId = AddressType.Business.Id
            };
            var homeAddressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var organizationId = 2;
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            var usa = new Location
            {
                LocationId = 1,
                LocationName = "USA"
            };
            var england = new Location
            {
                LocationId = 2,
                LocationName = "England"
            };
            var nashville = new Location
            {
                LocationId = 3,
                LocationName = "nashville",
            };
            var london = new Location
            {
                LocationId = 4,
                LocationName = "london"
            };
            var addressId = 1;
            Location addressLocation = null;
            Address address = null;

            var user = new User(updatorId);
            var addressTypeId = homeAddressType.AddressTypeId;
            var isPrimary = true;
            var street1 = "street1";
            var street2 = "street2";
            var street3 = "street3";
            var postalCode = "12345";
            var locationName = "location name";
            var countryId = 2;
            var cityId = 3;
            var divisionId = 4;
            var instance = new UpdatedEcaAddress(
                user,
                addressId,
                homeAddressType.AddressTypeId,
                isPrimary,
                street1,
                street2,
                street3,
                postalCode,
                locationName,
                countryId,
                cityId,
                divisionId
                );

            Action tester = () =>
            {
                Assert.AreEqual(1, context.Addresses.Count());
                Assert.AreEqual(addressTypeId, address.AddressTypeId);
                Assert.AreEqual(isPrimary, address.IsPrimary);
                Assert.AreEqual(creatorId, address.History.CreatedBy);
                Assert.AreEqual(yesterday, address.History.CreatedOn);
                Assert.AreEqual(updatorId, address.History.RevisedBy);
                DateTimeOffset.Now.Should().BeCloseTo(address.History.RevisedOn, 2000);

                Assert.AreEqual(street1, addressLocation.Street1);
                Assert.AreEqual(street2, addressLocation.Street2);
                Assert.AreEqual(street3, addressLocation.Street3);
                Assert.AreEqual(postalCode, addressLocation.PostalCode);
                Assert.AreEqual(cityId, addressLocation.CityId);
                Assert.AreEqual(countryId, addressLocation.CountryId);
                Assert.AreEqual(divisionId, addressLocation.DivisionId);
                Assert.AreEqual(creatorId, addressLocation.History.CreatedBy);
                Assert.AreEqual(yesterday, addressLocation.History.CreatedOn);
                Assert.AreEqual(updatorId, addressLocation.History.RevisedBy);
                DateTimeOffset.Now.Should().BeCloseTo(addressLocation.History.RevisedOn, 2000);
            };
            context.SetupActions.Add(() =>
            {
                addressLocation = new Location
                {
                    LocationName = "old location name",
                    LocationType = addressLocationType,
                    LocationId = 5,
                    LocationTypeId = addressLocationType.LocationTypeId,
                };
                addressLocation.History.CreatedBy = creatorId;
                addressLocation.History.CreatedOn = yesterday;
                addressLocation.History.RevisedBy = creatorId;

                address = new Address
                {
                    AddressId = addressId,
                    AddressType = businessAddressType,
                    AddressTypeId = businessAddressType.AddressTypeId,
                    IsPrimary = false,
                    Location = addressLocation,
                    LocationId = addressLocation.LocationId
                };
                address.History.CreatedBy = creatorId;
                address.History.CreatedOn = yesterday;
                address.History.RevisedBy = creatorId;
                address.History.RevisedOn = yesterday;
                organization.Addresses.Add(address);

                addressLocation.History.RevisedOn = yesterday;
                organization.Addresses.Clear();
                organization.Addresses.Add(address);

                context.LocationTypes.Add(addressLocationType);
                context.AddressTypes.Add(businessAddressType);
                context.AddressTypes.Add(homeAddressType);
                context.Organizations.Add(organization);
                context.Locations.Add(usa);
                context.Locations.Add(england);
                context.Locations.Add(nashville);
                context.Locations.Add(london);
                context.Locations.Add(addressLocation);
                context.Addresses.Add(address);
            });
            context.Revert();
            service.Update(instance);
            tester();
            addressValidator.Verify(x => x.ValidateUpdate(It.IsAny<EcaAddressValidationEntity>()), Times.Once());

            context.Revert();
            await service.UpdateAsync(instance);
            tester();
            addressValidator.Verify(x => x.ValidateUpdate(It.IsAny<EcaAddressValidationEntity>()), Times.Exactly(2));
        }


        [TestMethod]
        public async Task TestUpdate_HasExistingPrimaryAddress_UpdatedAddressIsPrimary()
        {
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var businessAddressType = new AddressType
            {
                AddressName = AddressType.Business.Value,
                AddressTypeId = AddressType.Business.Id
            };
            var homeAddressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var organizationId = 2;
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            var usa = new Location
            {
                LocationId = 1,
                LocationName = "USA"
            };
            var england = new Location
            {
                LocationId = 2,
                LocationName = "England"
            };
            var nashville = new Location
            {
                LocationId = 3,
                LocationName = "nashville",
            };
            var london = new Location
            {
                LocationId = 4,
                LocationName = "london"
            };
            var addressId = 1;
            var primaryAddressId = 2;
            Location addressLocation = null;
            Address address = null;
            Address primaryAddress = null;

            var user = new User(updatorId);
            var addressTypeId = homeAddressType.AddressTypeId;
            var isPrimary = true;
            var street1 = "street1";
            var street2 = "street2";
            var street3 = "street3";
            var postalCode = "12345";
            var locationName = "location name";
            var countryId = 2;
            var cityId = 3;
            var divisionId = 4;
            var instance = new UpdatedEcaAddress(
                user,
                addressId,
                homeAddressType.AddressTypeId,
                isPrimary,
                street1,
                street2,
                street3,
                postalCode,
                locationName,
                countryId,
                cityId,
                divisionId
                );

            Action tester = () =>
            {
                Assert.IsFalse(context.Addresses.Where(x => x.AddressId == primaryAddressId).FirstOrDefault().IsPrimary);
                Assert.IsTrue(context.Addresses.Where(x => x.AddressId == addressId).FirstOrDefault().IsPrimary);
            };
            context.SetupActions.Add(() =>
            {
                addressLocation = new Location
                {
                    LocationName = "old location name",
                    LocationType = addressLocationType,
                    LocationId = 5,
                    LocationTypeId = addressLocationType.LocationTypeId,
                };
                addressLocation.History.CreatedBy = creatorId;
                addressLocation.History.CreatedOn = yesterday;
                addressLocation.History.RevisedBy = creatorId;

                address = new Address
                {
                    AddressId = addressId,
                    AddressType = businessAddressType,
                    AddressTypeId = businessAddressType.AddressTypeId,
                    IsPrimary = false,
                    Location = addressLocation,
                    LocationId = addressLocation.LocationId,

                };
                primaryAddress = new Address
                {
                    AddressId = primaryAddressId,
                    AddressType = businessAddressType,
                    AddressTypeId = businessAddressType.AddressTypeId,
                    IsPrimary = true,
                    Location = addressLocation,
                    LocationId = addressLocation.LocationId
                };
                address.History.CreatedBy = creatorId;
                address.History.CreatedOn = yesterday;
                address.History.RevisedBy = creatorId;
                address.History.RevisedOn = yesterday;
                organization.Addresses.Add(address);

                addressLocation.History.RevisedOn = yesterday;
                organization.Addresses.Clear();
                organization.Addresses.Add(address);
                organization.Addresses.Add(primaryAddress);

                context.LocationTypes.Add(addressLocationType);
                context.AddressTypes.Add(businessAddressType);
                context.AddressTypes.Add(homeAddressType);
                context.Organizations.Add(organization);
                context.Locations.Add(usa);
                context.Locations.Add(england);
                context.Locations.Add(nashville);
                context.Locations.Add(london);
                context.Locations.Add(addressLocation);
                context.Addresses.Add(address);
                context.Addresses.Add(primaryAddress);
            });
            context.Revert();
            service.Update(instance);
            tester();
            addressValidator.Verify(x => x.ValidateUpdate(It.IsAny<EcaAddressValidationEntity>()), Times.Once());

            context.Revert();
            await service.UpdateAsync(instance);
            tester();
            addressValidator.Verify(x => x.ValidateUpdate(It.IsAny<EcaAddressValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestUpdate_HasExistingPrimaryAddress_UpdatedAddressIsNotPrimary()
        {
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var businessAddressType = new AddressType
            {
                AddressName = AddressType.Business.Value,
                AddressTypeId = AddressType.Business.Id
            };
            var homeAddressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var organizationId = 2;
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            var usa = new Location
            {
                LocationId = 1,
                LocationName = "USA"
            };
            var england = new Location
            {
                LocationId = 2,
                LocationName = "England"
            };
            var nashville = new Location
            {
                LocationId = 3,
                LocationName = "nashville",
            };
            var london = new Location
            {
                LocationId = 4,
                LocationName = "london"
            };
            var addressId = 1;
            var primaryAddressId = 2;
            Location addressLocation = null;
            Address address = null;
            Address primaryAddress = null;

            var user = new User(updatorId);
            var addressTypeId = homeAddressType.AddressTypeId;
            var isPrimary = false;
            var street1 = "street1";
            var street2 = "street2";
            var street3 = "street3";
            var postalCode = "12345";
            var locationName = "location name";
            var countryId = 2;
            var cityId = 3;
            var divisionId = 4;
            var instance = new UpdatedEcaAddress(
                user,
                addressId,
                homeAddressType.AddressTypeId,
                isPrimary,
                street1,
                street2,
                street3,
                postalCode,
                locationName,
                countryId,
                cityId,
                divisionId
                );

            Action tester = () =>
            {
                Assert.IsTrue(context.Addresses.Where(x => x.AddressId == primaryAddressId).FirstOrDefault().IsPrimary);
                Assert.IsFalse(context.Addresses.Where(x => x.AddressId == addressId).FirstOrDefault().IsPrimary);
            };
            context.SetupActions.Add(() =>
            {
                addressLocation = new Location
                {
                    LocationName = "old location name",
                    LocationType = addressLocationType,
                    LocationId = 5,
                    LocationTypeId = addressLocationType.LocationTypeId,
                };
                addressLocation.History.CreatedBy = creatorId;
                addressLocation.History.CreatedOn = yesterday;
                addressLocation.History.RevisedBy = creatorId;

                address = new Address
                {
                    AddressId = addressId,
                    AddressType = businessAddressType,
                    AddressTypeId = businessAddressType.AddressTypeId,
                    IsPrimary = false,
                    Location = addressLocation,
                    LocationId = addressLocation.LocationId,

                };
                primaryAddress = new Address
                {
                    AddressId = primaryAddressId,
                    AddressType = businessAddressType,
                    AddressTypeId = businessAddressType.AddressTypeId,
                    IsPrimary = true,
                    Location = addressLocation,
                    LocationId = addressLocation.LocationId
                };
                address.History.CreatedBy = creatorId;
                address.History.CreatedOn = yesterday;
                address.History.RevisedBy = creatorId;
                address.History.RevisedOn = yesterday;
                organization.Addresses.Add(address);

                addressLocation.History.RevisedOn = yesterday;
                organization.Addresses.Clear();
                organization.Addresses.Add(address);
                organization.Addresses.Add(primaryAddress);

                context.LocationTypes.Add(addressLocationType);
                context.AddressTypes.Add(businessAddressType);
                context.AddressTypes.Add(homeAddressType);
                context.Organizations.Add(organization);
                context.Locations.Add(usa);
                context.Locations.Add(england);
                context.Locations.Add(nashville);
                context.Locations.Add(london);
                context.Locations.Add(addressLocation);
                context.Addresses.Add(address);
                context.Addresses.Add(primaryAddress);
            });
            context.Revert();
            service.Update(instance);
            tester();
            addressValidator.Verify(x => x.ValidateUpdate(It.IsAny<EcaAddressValidationEntity>()), Times.Once());

            context.Revert();
            await service.UpdateAsync(instance);
            tester();
            addressValidator.Verify(x => x.ValidateUpdate(It.IsAny<EcaAddressValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestUpdate_AllAddressesNotPrimary()
        {
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var businessAddressType = new AddressType
            {
                AddressName = AddressType.Business.Value,
                AddressTypeId = AddressType.Business.Id
            };
            var homeAddressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var organizationId = 2;
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            var usa = new Location
            {
                LocationId = 1,
                LocationName = "USA"
            };
            var england = new Location
            {
                LocationId = 2,
                LocationName = "England"
            };
            var nashville = new Location
            {
                LocationId = 3,
                LocationName = "nashville",
            };
            var london = new Location
            {
                LocationId = 4,
                LocationName = "london"
            };
            var addressId = 1;
            var otherAddressId = 2;
            Location addressLocation = null;
            Address address = null;
            Address otherAddress = null;

            var user = new User(updatorId);
            var addressTypeId = homeAddressType.AddressTypeId;
            var isPrimary = false;
            var street1 = "street1";
            var street2 = "street2";
            var street3 = "street3";
            var postalCode = "12345";
            var locationName = "location name";
            var countryId = 2;
            var cityId = 3;
            var divisionId = 4;
            var instance = new UpdatedEcaAddress(
                user,
                addressId,
                homeAddressType.AddressTypeId,
                isPrimary,
                street1,
                street2,
                street3,
                postalCode,
                locationName,
                countryId,
                cityId,
                divisionId
                );

            Action tester = () =>
            {
                Assert.IsFalse(context.Addresses.Where(x => x.AddressId == otherAddressId).FirstOrDefault().IsPrimary);
                Assert.IsFalse(context.Addresses.Where(x => x.AddressId == addressId).FirstOrDefault().IsPrimary);
            };
            context.SetupActions.Add(() =>
            {
                addressLocation = new Location
                {
                    LocationName = "old location name",
                    LocationType = addressLocationType,
                    LocationId = 5,
                    LocationTypeId = addressLocationType.LocationTypeId,
                };
                addressLocation.History.CreatedBy = creatorId;
                addressLocation.History.CreatedOn = yesterday;
                addressLocation.History.RevisedBy = creatorId;

                address = new Address
                {
                    AddressId = addressId,
                    AddressType = businessAddressType,
                    AddressTypeId = businessAddressType.AddressTypeId,
                    IsPrimary = false,
                    Location = addressLocation,
                    LocationId = addressLocation.LocationId,

                };
                otherAddress = new Address
                {
                    AddressId = otherAddressId,
                    AddressType = businessAddressType,
                    AddressTypeId = businessAddressType.AddressTypeId,
                    IsPrimary = false,
                    Location = addressLocation,
                    LocationId = addressLocation.LocationId
                };
                address.History.CreatedBy = creatorId;
                address.History.CreatedOn = yesterday;
                address.History.RevisedBy = creatorId;
                address.History.RevisedOn = yesterday;
                organization.Addresses.Add(address);

                addressLocation.History.RevisedOn = yesterday;
                organization.Addresses.Clear();
                organization.Addresses.Add(address);
                organization.Addresses.Add(otherAddress);

                context.LocationTypes.Add(addressLocationType);
                context.AddressTypes.Add(businessAddressType);
                context.AddressTypes.Add(homeAddressType);
                context.Organizations.Add(organization);
                context.Locations.Add(usa);
                context.Locations.Add(england);
                context.Locations.Add(nashville);
                context.Locations.Add(london);
                context.Locations.Add(addressLocation);
                context.Addresses.Add(address);
                context.Addresses.Add(otherAddress);
            });
            context.Revert();
            service.Update(instance);
            tester();
            addressValidator.Verify(x => x.ValidateUpdate(It.IsAny<EcaAddressValidationEntity>()), Times.Once());

            context.Revert();
            await service.UpdateAsync(instance);
            tester();
            addressValidator.Verify(x => x.ValidateUpdate(It.IsAny<EcaAddressValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestUpdate_CityDoesNotExist()
        {
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var businessAddressType = new AddressType
            {
                AddressName = AddressType.Business.Value,
                AddressTypeId = AddressType.Business.Id
            };
            var homeAddressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var organizationId = 2;
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            var country = new Location
            {
                LocationId = 1,
                LocationName = "country"
            };
            var division = new Location
            {
                LocationId = 2,
                LocationName = "division"
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "city",
            };
            var addressLocation = new Location
            {
                LocationName = "old location name",
                LocationType = addressLocationType,
                LocationId = 5,
                LocationTypeId = addressLocationType.LocationTypeId,
            };
            addressLocation.History.CreatedBy = creatorId;
            addressLocation.History.CreatedOn = yesterday;
            addressLocation.History.RevisedBy = creatorId;

            var address = new Address
            {
                AddressId = 1,
                AddressType = businessAddressType,
                AddressTypeId = businessAddressType.AddressTypeId,
                IsPrimary = false,
                Location = addressLocation,
                LocationId = addressLocation.LocationId
            };
            address.History.CreatedBy = creatorId;
            address.History.CreatedOn = yesterday;
            address.History.RevisedBy = creatorId;
            address.History.RevisedOn = yesterday;
            organization.Addresses.Add(address);

            addressLocation.History.RevisedOn = yesterday;
            organization.Addresses.Clear();
            organization.Addresses.Add(address);

            context.LocationTypes.Add(addressLocationType);
            context.AddressTypes.Add(businessAddressType);
            context.AddressTypes.Add(homeAddressType);
            context.Organizations.Add(organization);
            context.Locations.Add(country);
            context.Locations.Add(division);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(address);

            var addressId = address.AddressId;
            var user = new User(updatorId);
            var addressTypeId = homeAddressType.AddressTypeId;
            var isPrimary = true;
            var street1 = "street1";
            var street2 = "street2";
            var street3 = "street3";
            var postalCode = "12345";
            var locationName = "location name";
            var countryId = country.LocationId;
            var cityId = city.LocationId;
            var divisionId = division.LocationId;
            var instance = new UpdatedEcaAddress(
                user,
                addressId,
                homeAddressType.AddressTypeId,
                isPrimary,
                street1,
                street2,
                street3,
                postalCode,
                locationName,
                countryId,
                cityId,
                divisionId
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(instance);
            };

            service.Invoking(x => x.Update(instance)).ShouldThrow<ModelNotFoundException>()
                                .WithMessage(String.Format("The [{0}] with id [{1}] was not found.", "City", cityId));

            f.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The [{0}] with id [{1}] was not found.", "City", cityId));

        }

        [TestMethod]
        public async Task TestUpdate_CountryDoesNotExist()
        {
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var businessAddressType = new AddressType
            {
                AddressName = AddressType.Business.Value,
                AddressTypeId = AddressType.Business.Id
            };
            var homeAddressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var organizationId = 2;
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            var country = new Location
            {
                LocationId = 1,
                LocationName = "country"
            };
            var division = new Location
            {
                LocationId = 2,
                LocationName = "division"
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "city",
            };
            var addressLocation = new Location
            {
                LocationName = "old location name",
                LocationType = addressLocationType,
                LocationId = 5,
                LocationTypeId = addressLocationType.LocationTypeId,
            };
            addressLocation.History.CreatedBy = creatorId;
            addressLocation.History.CreatedOn = yesterday;
            addressLocation.History.RevisedBy = creatorId;

            var address = new Address
            {
                AddressId = 1,
                AddressType = businessAddressType,
                AddressTypeId = businessAddressType.AddressTypeId,
                Location = addressLocation,
                LocationId = addressLocation.LocationId
            };
            address.History.CreatedBy = creatorId;
            address.History.CreatedOn = yesterday;
            address.History.RevisedBy = creatorId;
            address.History.RevisedOn = yesterday;
            organization.Addresses.Add(address);

            addressLocation.History.RevisedOn = yesterday;
            organization.Addresses.Clear();
            organization.Addresses.Add(address);

            context.LocationTypes.Add(addressLocationType);
            context.AddressTypes.Add(businessAddressType);
            context.AddressTypes.Add(homeAddressType);
            context.Organizations.Add(organization);
            context.Locations.Add(addressLocation);
            context.Locations.Add(city);
            context.Locations.Add(division);
            context.Addresses.Add(address);

            var addressId = address.AddressId;
            var user = new User(updatorId);
            var addressTypeId = homeAddressType.AddressTypeId;
            var isPrimary = true;
            var street1 = "street1";
            var street2 = "street2";
            var street3 = "street3";
            var postalCode = "12345";
            var locationName = "location name";
            var countryId = country.LocationId;
            var cityId = city.LocationId;
            var divisionId = division.LocationId;
            var instance = new UpdatedEcaAddress(
                user,
                addressId,
                homeAddressType.AddressTypeId,
                isPrimary,
                street1,
                street2,
                street3,
                postalCode,
                locationName,
                countryId,
                cityId,
                divisionId
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(instance);
            };

            service.Invoking(x => x.Update(instance)).ShouldThrow<ModelNotFoundException>()
                                .WithMessage(String.Format("The [{0}] with id [{1}] was not found.", "Country", countryId));

            f.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The [{0}] with id [{1}] was not found.", "Country", countryId));

        }

        [TestMethod]
        public async Task TestUpdate_DivisionDoesNotExist()
        {
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.Now.AddDays(-1.0);
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var businessAddressType = new AddressType
            {
                AddressName = AddressType.Business.Value,
                AddressTypeId = AddressType.Business.Id
            };
            var homeAddressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var organizationId = 2;
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            var country = new Location
            {
                LocationId = 1,
                LocationName = "country"
            };
            var division = new Location
            {
                LocationId = 2,
                LocationName = "division"
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "city",
            };
            var addressLocation = new Location
            {
                LocationName = "old location name",
                LocationType = addressLocationType,
                LocationId = 5,
                LocationTypeId = addressLocationType.LocationTypeId,
            };
            addressLocation.History.CreatedBy = creatorId;
            addressLocation.History.CreatedOn = yesterday;
            addressLocation.History.RevisedBy = creatorId;

            var address = new Address
            {
                AddressId = 1,
                AddressType = businessAddressType,
                AddressTypeId = businessAddressType.AddressTypeId,
                Location = addressLocation,
                LocationId = addressLocation.LocationId
            };
            address.History.CreatedBy = creatorId;
            address.History.CreatedOn = yesterday;
            address.History.RevisedBy = creatorId;
            address.History.RevisedOn = yesterday;
            organization.Addresses.Add(address);

            addressLocation.History.RevisedOn = yesterday;
            organization.Addresses.Clear();
            organization.Addresses.Add(address);

            context.LocationTypes.Add(addressLocationType);
            context.AddressTypes.Add(businessAddressType);
            context.AddressTypes.Add(homeAddressType);
            context.Organizations.Add(organization);
            context.Locations.Add(addressLocation);
            context.Locations.Add(city);
            context.Locations.Add(country);
            context.Addresses.Add(address);

            var addressId = address.AddressId;
            var user = new User(updatorId);
            var addressTypeId = homeAddressType.AddressTypeId;
            var isPrimary = true;
            var street1 = "street1";
            var street2 = "street2";
            var street3 = "street3";
            var postalCode = "12345";
            var locationName = "location name";
            var countryId = country.LocationId;
            var cityId = city.LocationId;
            var divisionId = division.LocationId;
            var instance = new UpdatedEcaAddress(
                user,
                addressId,
                homeAddressType.AddressTypeId,
                isPrimary,
                street1,
                street2,
                street3,
                postalCode,
                locationName,
                countryId,
                cityId,
                divisionId
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(instance);
            };

            service.Invoking(x => x.Update(instance)).ShouldThrow<ModelNotFoundException>()
                                .WithMessage(String.Format("The [{0}] with id [{1}] was not found.", "Division", divisionId));

            f.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The [{0}] with id [{1}] was not found.", "Division", divisionId));

        }

        [TestMethod]
        public async Task TestGetAddressById_CheckProperties()
        {
            var person = new Person
            {
                PersonId = 1,
            };
            var organization = new Organization
            {
                OrganizationId = 1
            };
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var division = new Location
            {
                LocationId = 1,
                LocationName = "TN"
            };
            var country = new Location
            {
                LocationId = 2,
                LocationName = "US",
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "Nashville"
            };
            var addressLocation = new Location
            {
                LocationId = 4,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                LocationName = "address",
                LocationType = addressLocationType,
                LocationTypeId = addressLocationType.LocationTypeId,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var addressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var address = new Address
            {
                AddressId = 1,
                AddressType = addressType,
                AddressTypeId = addressType.AddressTypeId,
                IsPrimary = true,
                Location = addressLocation,
                LocationId = addressLocation.LocationId,
                Organization = organization,
                OrganizationId = organization.OrganizationId,
                Person = person,
                PersonId = person.PersonId
            };
            context.AddressTypes.Add(addressType);
            context.Locations.Add(division);
            context.Locations.Add(country);
            context.Locations.Add(city);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(address);
            context.People.Add(person);
            context.Organizations.Add(organization);
            context.LocationTypes.Add(addressLocationType);

            Action<AddressDTO> tester = (dto) =>
            {
                Assert.AreEqual(person.PersonId, dto.PersonId);
                Assert.AreEqual(organization.OrganizationId, dto.OrganizationId);
                Assert.AreEqual(addressType.AddressTypeId, dto.AddressTypeId);
                Assert.AreEqual(addressType.AddressName, dto.AddressType);
                Assert.AreEqual(city.LocationName, dto.City);
                Assert.AreEqual(city.LocationId, dto.CityId);
                Assert.AreEqual(country.LocationName, dto.Country);
                Assert.AreEqual(country.LocationId, dto.CountryId);
                Assert.AreEqual(division.LocationName, dto.Division);
                Assert.AreEqual(division.LocationId, dto.DivisionId);
                Assert.AreEqual(addressLocation.LocationId, dto.LocationId);
                Assert.AreEqual(addressLocation.LocationName, dto.LocationName);
                Assert.AreEqual(addressLocation.Street1, dto.Street1);
                Assert.AreEqual(addressLocation.Street2, dto.Street2);
                Assert.AreEqual(addressLocation.Street3, dto.Street3);
                Assert.AreEqual(address.AddressId, dto.AddressId);
                Assert.AreEqual(address.IsPrimary, dto.IsPrimary);
            };

            var serviceResult = service.GetAddressById(address.AddressId);
            var serviceResultAsync = await service.GetAddressByIdAsync(address.AddressId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }


        [TestMethod]
        public async Task TestGetAddressById_AddressNotExist()
        {
            Action<AddressDTO> tester = (dto) =>
            {
                Assert.AreEqual(0, context.Addresses.Count());
                Assert.IsNull(dto);
            };

            var serviceResult = service.GetAddressById(1);
            var serviceResultAsync = await service.GetAddressByIdAsync(1);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetAddressById_DoesNotHaveCity()
        {
            var person = new Person
            {
                PersonId = 1,
            };
            var organization = new Organization
            {
                OrganizationId = 1
            };
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var division = new Location
            {
                LocationId = 1,
                LocationName = "TN"
            };
            var country = new Location
            {
                LocationId = 2,
                LocationName = "US",
            };
            var addressLocation = new Location
            {
                LocationId = 4,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                LocationName = "address",
                LocationType = addressLocationType,
                LocationTypeId = addressLocationType.LocationTypeId,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var addressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var address = new Address
            {
                AddressId = 1,
                AddressType = addressType,
                AddressTypeId = addressType.AddressTypeId,
                IsPrimary = true,
                Location = addressLocation,
                LocationId = addressLocation.LocationId,
                Organization = organization,
                OrganizationId = organization.OrganizationId,
                Person = person,
                PersonId = person.PersonId
            };
            context.AddressTypes.Add(addressType);
            context.Locations.Add(division);
            context.Locations.Add(country);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(address);
            context.People.Add(person);
            context.Organizations.Add(organization);
            context.LocationTypes.Add(addressLocationType);

            Action<AddressDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
            };

            var serviceResult = service.GetAddressById(address.AddressId);
            var serviceResultAsync = await service.GetAddressByIdAsync(address.AddressId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetAddressById_DoesNotHaveCountry()
        {
            var person = new Person
            {
                PersonId = 1,
            };
            var organization = new Organization
            {
                OrganizationId = 1
            };
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var division = new Location
            {
                LocationId = 1,
                LocationName = "TN"
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "Nashville"
            };
            var addressLocation = new Location
            {
                LocationId = 4,
                City = city,
                CityId = city.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                LocationName = "address",
                LocationType = addressLocationType,
                LocationTypeId = addressLocationType.LocationTypeId,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var addressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var address = new Address
            {
                AddressId = 1,
                AddressType = addressType,
                AddressTypeId = addressType.AddressTypeId,
                IsPrimary = true,
                Location = addressLocation,
                LocationId = addressLocation.LocationId,
                Organization = organization,
                OrganizationId = organization.OrganizationId,
                Person = person,
                PersonId = person.PersonId
            };
            context.AddressTypes.Add(addressType);
            context.Locations.Add(division);
            context.Locations.Add(city);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(address);
            context.People.Add(person);
            context.Organizations.Add(organization);
            context.LocationTypes.Add(addressLocationType);

            Action<AddressDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
            };

            var serviceResult = service.GetAddressById(address.AddressId);
            var serviceResultAsync = await service.GetAddressByIdAsync(address.AddressId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetAddressById_DoesNotHaveDivision()
        {
            var person = new Person
            {
                PersonId = 1,
            };
            var organization = new Organization
            {
                OrganizationId = 1
            };
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var country = new Location
            {
                LocationId = 2,
                LocationName = "US",
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "Nashville"
            };
            var addressLocation = new Location
            {
                LocationId = 4,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                LocationName = "address",
                LocationType = addressLocationType,
                LocationTypeId = addressLocationType.LocationTypeId,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var addressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var address = new Address
            {
                AddressId = 1,
                AddressType = addressType,
                AddressTypeId = addressType.AddressTypeId,
                IsPrimary = true,
                Location = addressLocation,
                LocationId = addressLocation.LocationId,
                Organization = organization,
                OrganizationId = organization.OrganizationId,
                Person = person,
                PersonId = person.PersonId
            };
            context.AddressTypes.Add(addressType);
            context.Locations.Add(country);
            context.Locations.Add(city);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(address);
            context.People.Add(person);
            context.Organizations.Add(organization);
            context.LocationTypes.Add(addressLocationType);

            Action<AddressDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
            };

            var serviceResult = service.GetAddressById(address.AddressId);
            var serviceResultAsync = await service.GetAddressByIdAsync(address.AddressId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        #endregion


        #region Create
        [TestMethod]
        public async Task TestCreate_Location_CheckProperties()
        {
            var userId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var city = new Location
            {
                LocationName = "city",
                LocationId = 1,
            };
            var country = new Location
            {
                LocationName = "country",
                LocationId = 2
            };
            var division = new Location
            {
                LocationName = "division",
                LocationId = 3
            };
            var region = new Location
            {
                LocationName = "region",
                LocationId = 4
            };
            var place = new LocationType
            {
                LocationTypeId = LocationType.Place.Id,
                LocationTypeName = "place"
            };

            context.SetupActions.Add(() =>
            {
                context.Locations.Add(city);
                context.Locations.Add(country);
                context.Locations.Add(division);
                context.Locations.Add(region);
                context.LocationTypes.Add(place);
            });
            Action beforeTester = () =>
            {
                Assert.AreEqual(4, context.Locations.Count());
            };

            var creator = new User(userId);
            var additionalLocation = new AdditionalLocation(
                creator: creator,
                locationName: "location name",
                cityId: city.LocationId,
                countryId: country.LocationId,
                divisionId: division.LocationId,
                regionId: region.LocationId,
                latitude: 2.35f,
                longitude: 5.0f,
                locationTypeId: place.LocationTypeId
                );

            Action<Location> afterTester = (serviceLocation) =>
            {
                Assert.AreEqual(5, context.Locations.Count());
                var addedLocation = context.Locations.Where(x => x.LocationName == additionalLocation.LocationName).First();

                Assert.AreEqual(additionalLocation.LocationName, addedLocation.LocationName);
                Assert.IsTrue(Object.ReferenceEquals(city, addedLocation.City));
                Assert.IsTrue(Object.ReferenceEquals(country, addedLocation.Country));
                Assert.IsTrue(Object.ReferenceEquals(division, addedLocation.Division));
                Assert.IsTrue(Object.ReferenceEquals(region, addedLocation.Region));
                Assert.AreEqual(additionalLocation.LocationName, addedLocation.LocationName);
                float delta = .01f;
                Assert.AreEqual(additionalLocation.Latitude.Value, addedLocation.Latitude.Value, delta, "The Latitude value is invalid.");
                Assert.AreEqual(additionalLocation.Longitude.Value, addedLocation.Longitude.Value, delta, "The Longitude value is invalid.");
                Assert.AreEqual(place.LocationTypeId, additionalLocation.LocationTypeId);

                Assert.AreEqual(userId, addedLocation.History.CreatedBy);
                Assert.AreEqual(userId, addedLocation.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedLocation.History.CreatedOn, 2000);
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedLocation.History.RevisedOn, 2000);
            };
            context.Revert();
            beforeTester();
            var serviceResult = service.Create(additionalLocation);
            locationValidator.Verify(x => x.ValidateCreate(It.IsAny<LocationValidationEntity>()), Times.Once());
            afterTester(serviceResult);

            context.Revert();
            beforeTester();
            serviceResult = await service.CreateAsync(additionalLocation);
            locationValidator.Verify(x => x.ValidateCreate(It.IsAny<LocationValidationEntity>()), Times.Exactly(2));
            afterTester(serviceResult);
        }

        [TestMethod]
        public async Task TestCreate_Location_LocationTypeDoesNotExist()
        {
            var userId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var city = new Location
            {
                LocationName = "city",
                LocationId = 1,
            };
            var country = new Location
            {
                LocationName = "country",
                LocationId = 2
            };
            var division = new Location
            {
                LocationName = "division",
                LocationId = 3
            };
            var region = new Location
            {
                LocationName = "region",
                LocationId = 4
            };
            context.SetupActions.Add(() =>
            {
                context.Locations.Add(city);
                context.Locations.Add(country);
                context.Locations.Add(division);
                context.Locations.Add(region);
            });
            var creator = new User(userId);
            var additionalLocation = new AdditionalLocation(
                creator: creator,
                locationName: "location name",
                cityId: city.LocationId,
                countryId: country.LocationId,
                divisionId: division.LocationId,
                regionId: region.LocationId,
                latitude: 2.35f,
                longitude: 5.0f,
                locationTypeId: LocationType.Place.Id
                );
            context.Revert();

            var message = String.Format("The location type with id [{0}] does not exist.", additionalLocation.LocationTypeId);
            Func<Task> f = () =>
            {
                return service.CreateAsync(additionalLocation);
            };
            service.Invoking(x => x.Create(additionalLocation)).ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);

        }

        [TestMethod]
        public async Task TestCreate_Location_CountryDoesNotExist()
        {
            var userId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var city = new Location
            {
                LocationName = "city",
                LocationId = 1,
            };
            var division = new Location
            {
                LocationName = "division",
                LocationId = 3
            };
            var region = new Location
            {
                LocationName = "region",
                LocationId = 4,
            };
            var place = new LocationType
            {
                LocationTypeId = LocationType.Place.Id,
                LocationTypeName = "place"
            };
            context.SetupActions.Add(() =>
            {
                context.Locations.Add(city);
                context.Locations.Add(division);
                context.Locations.Add(region);
                context.LocationTypes.Add(place);
            });

            var creator = new User(userId);
            var additionalLocation = new AdditionalLocation(
                creator: creator,
                locationName: "location name",
                cityId: city.LocationId,
                countryId: -1,
                divisionId: division.LocationId,
                regionId: region.LocationId,
                latitude: 2.35f,
                longitude: 5.0f,
                locationTypeId: place.LocationTypeId
                );

            context.Revert();
            var locationTypeName = "Country";
            var message = String.Format("The [{0}] with id [{1}] was not found.", locationTypeName, additionalLocation.CountryId.Value);
            Func<Task> f = () =>
            {
                return service.CreateAsync(additionalLocation);
            };
            service.Invoking(x => x.Create(additionalLocation)).ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestCreate_Location_DivisionDoesNotExist()
        {
            var userId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var city = new Location
            {
                LocationName = "city",
                LocationId = 1,
            };
            var country = new Location
            {
                LocationName = "country",
                LocationId = 2
            };
            var region = new Location
            {
                LocationName = "region",
                LocationId = 4,
            };
            var place = new LocationType
            {
                LocationTypeId = LocationType.Place.Id,
                LocationTypeName = "place"
            };

            context.SetupActions.Add(() =>
            {
                context.Locations.Add(city);
                context.Locations.Add(country);
                context.Locations.Add(region);
                context.LocationTypes.Add(place);
            });

            var creator = new User(userId);
            var additionalLocation = new AdditionalLocation(
                creator: creator,
                locationName: "location name",
                cityId: city.LocationId,
                countryId: country.LocationId,
                regionId: region.LocationId,
                divisionId: -1,
                latitude: 2.35f,
                longitude: 5.0f,
                locationTypeId: place.LocationTypeId
                );


            context.Revert();
            var locationTypeName = "Division";
            var message = String.Format("The [{0}] with id [{1}] was not found.", locationTypeName, additionalLocation.DivisionId.Value);
            Func<Task> f = () =>
            {
                return service.CreateAsync(additionalLocation);
            };
            service.Invoking(x => x.Create(additionalLocation)).ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestCreate_Location_CityDoesNotExist()
        {
            var userId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var country = new Location
            {
                LocationName = "country",
                LocationId = 2
            };
            var division = new Location
            {
                LocationName = "division",
                LocationId = 3
            };
            var region = new Location
            {
                LocationName = "region",
                LocationId = 4,
            };
            var place = new LocationType
            {
                LocationTypeId = LocationType.Place.Id,
                LocationTypeName = "place"
            };

            context.SetupActions.Add(() =>
            {
                context.Locations.Add(country);
                context.Locations.Add(region);
                context.Locations.Add(division);
                context.LocationTypes.Add(place);
            });

            var creator = new User(userId);
            var additionalLocation = new AdditionalLocation(
                creator: creator,
                locationName: "location name",
                cityId: -1,
                countryId: country.LocationId,
                divisionId: division.LocationId,
                regionId: region.LocationId,
                latitude: 2.35f,
                longitude: 5.0f,
                locationTypeId: place.LocationTypeId
                );


            context.Revert();
            var locationTypeName = "City";
            var message = String.Format("The [{0}] with id [{1}] was not found.", locationTypeName, additionalLocation.CityId.Value);
            Func<Task> f = () =>
            {
                return service.CreateAsync(additionalLocation);
            };
            service.Invoking(x => x.Create(additionalLocation)).ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestCreate_Location_RegionDoesNotExist()
        {
            var userId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var country = new Location
            {
                LocationName = "country",
                LocationId = 2
            };
            var division = new Location
            {
                LocationName = "division",
                LocationId = 3
            };
            var city = new Location
            {
                LocationName = "region",
                LocationId = 4,
            };
            var place = new LocationType
            {
                LocationTypeId = LocationType.Place.Id,
                LocationTypeName = "place"
            };

            context.SetupActions.Add(() =>
            {
                context.Locations.Add(country);
                context.Locations.Add(city);
                context.Locations.Add(division);
                context.LocationTypes.Add(place);
            });

            var creator = new User(userId);
            var additionalLocation = new AdditionalLocation(
                creator: creator,
                locationName: "location name",
                cityId: city.LocationId,
                countryId: country.LocationId,
                divisionId: division.LocationId,
                regionId: -1,
                latitude: 2.35f,
                longitude: 5.0f,
                locationTypeId: place.LocationTypeId
                );


            context.Revert();
            var locationTypeName = "Region";
            var message = String.Format("The [{0}] with id [{1}] was not found.", locationTypeName, additionalLocation.RegionId.Value);
            Func<Task> f = () =>
            {
                return service.CreateAsync(additionalLocation);
            };
            service.Invoking(x => x.Create(additionalLocation)).ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        #endregion

        #region Update
        [TestMethod]
        public async Task TestUpdate_Location_CheckProperties()
        {
            var creatorId = 1;
            var revisorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var city = new Location
            {
                LocationName = "city",
                LocationId = 1,
            };
            var country = new Location
            {
                LocationName = "country",
                LocationId = 2
            };
            var division = new Location
            {
                LocationName = "division",
                LocationId = 3
            };
            var region = new Location
            {
                LocationName = "region",
                LocationId = 4
            };
            var place = new LocationType
            {
                LocationTypeId = LocationType.Place.Id,
                LocationTypeName = "place"
            };
            var locationToUpdateId = 5;
            Location locationToUpdate = null;


            context.SetupActions.Add(() =>
            {
                locationToUpdate = new Location
                {
                    LocationId = locationToUpdateId
                };
                locationToUpdate.History.CreatedBy = creatorId;
                locationToUpdate.History.RevisedBy = creatorId;
                locationToUpdate.History.CreatedOn = yesterday;
                locationToUpdate.History.RevisedOn = yesterday;
                context.Locations.Add(city);
                context.Locations.Add(country);
                context.Locations.Add(division);
                context.Locations.Add(region);
                context.Locations.Add(locationToUpdate);
                context.LocationTypes.Add(place);
            });
            Action beforeTester = () =>
            {
                Assert.AreEqual(5, context.Locations.Count());
            };

            var updator = new User(revisorId);
            var updatedLocation = new UpdatedLocation(
                updator: updator,
                locationId: locationToUpdateId,
                locationName: "location name",
                cityId: city.LocationId,
                countryId: country.LocationId,
                regionId: region.LocationId,
                divisionId: division.LocationId,
                latitude: 2.35f,
                longitude: 5.0f,
                locationTypeId: place.LocationTypeId
                );

            Action afterTester = () =>
            {
                Assert.AreEqual(5, context.Locations.Count());

                Assert.AreEqual(updatedLocation.LocationName, locationToUpdate.LocationName);
                Assert.IsTrue(Object.ReferenceEquals(city, locationToUpdate.City));
                Assert.IsTrue(Object.ReferenceEquals(country, locationToUpdate.Country));
                Assert.IsTrue(Object.ReferenceEquals(division, locationToUpdate.Division));
                Assert.IsTrue(Object.ReferenceEquals(place, locationToUpdate.LocationType));
                Assert.IsTrue(Object.ReferenceEquals(region, locationToUpdate.Region));
                Assert.AreEqual(updatedLocation.LocationName, locationToUpdate.LocationName);
                float delta = .01f;
                Assert.AreEqual(updatedLocation.Latitude.Value, locationToUpdate.Latitude.Value, delta, "The Latitude value is invalid.");
                Assert.AreEqual(updatedLocation.Longitude.Value, locationToUpdate.Longitude.Value, delta, "The Longitude value is invalid.");

                Assert.AreEqual(creatorId, locationToUpdate.History.CreatedBy);
                Assert.AreEqual(yesterday, locationToUpdate.History.CreatedOn);
                Assert.AreEqual(revisorId, locationToUpdate.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(locationToUpdate.History.RevisedOn, 2000);
            };
            context.Revert();
            beforeTester();
            service.Update(updatedLocation);
            locationValidator.Verify(x => x.ValidateUpdate(It.IsAny<LocationValidationEntity>()), Times.Once());
            afterTester();

            context.Revert();
            beforeTester();
            await service.UpdateAsync(updatedLocation);
            locationValidator.Verify(x => x.ValidateUpdate(It.IsAny<LocationValidationEntity>()), Times.Exactly(2));
            afterTester();
        }

        [TestMethod]
        public async Task TestUpdate_Location_CountryDoesNotExist()
        {
            var creatorId = 1;
            var revisorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var city = new Location
            {
                LocationName = "city",
                LocationId = 1,
            };
            var division = new Location
            {
                LocationName = "division",
                LocationId = 3
            };
            var region = new Location
            {
                LocationName = "region",
                LocationId = 4
            };
            var place = new LocationType
            {
                LocationTypeId = LocationType.Place.Id,
                LocationTypeName = "place"
            };
            var locationToUpdateId = 5;
            Location locationToUpdate = null;


            context.SetupActions.Add(() =>
            {
                locationToUpdate = new Location
                {
                    LocationId = locationToUpdateId
                };
                locationToUpdate.History.CreatedBy = creatorId;
                locationToUpdate.History.RevisedBy = creatorId;
                locationToUpdate.History.CreatedOn = yesterday;
                locationToUpdate.History.RevisedOn = yesterday;
                context.Locations.Add(city);
                context.Locations.Add(division);
                context.Locations.Add(locationToUpdate);
                context.Locations.Add(region);
                context.LocationTypes.Add(place);
            });

            var updator = new User(revisorId);
            var updatedLocation = new UpdatedLocation(
                updator: updator,
                locationId: locationToUpdateId,
                locationName: "location name",
                cityId: city.LocationId,
                countryId: -1,
                divisionId: division.LocationId,
                regionId: region.LocationId,
                latitude: 2.35f,
                longitude: 5.0f,
                locationTypeId: place.LocationTypeId
                );
            context.Revert();

            var locationTypeName = "Country";
            var message = String.Format("The [{0}] with id [{1}] was not found.", locationTypeName, updatedLocation.CountryId);
            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedLocation);
            };
            service.Invoking(x => x.Update(updatedLocation)).ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_Location_DivisionDoesNotExist()
        {
            var creatorId = 1;
            var revisorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var city = new Location
            {
                LocationName = "city",
                LocationId = 1,
            };
            var country = new Location
            {
                LocationName = "country",
                LocationId = 2
            };
            var region = new Location
            {
                LocationName = "region",
                LocationId = 4
            };
            var place = new LocationType
            {
                LocationTypeId = LocationType.Place.Id,
                LocationTypeName = "place"
            };
            var locationToUpdateId = 5;
            Location locationToUpdate = null;


            context.SetupActions.Add(() =>
            {
                locationToUpdate = new Location
                {
                    LocationId = locationToUpdateId
                };
                locationToUpdate.History.CreatedBy = creatorId;
                locationToUpdate.History.RevisedBy = creatorId;
                locationToUpdate.History.CreatedOn = yesterday;
                locationToUpdate.History.RevisedOn = yesterday;
                context.Locations.Add(city);
                context.Locations.Add(country);
                context.Locations.Add(locationToUpdate);
                context.Locations.Add(region);
                context.LocationTypes.Add(place);
            });
            var updator = new User(revisorId);
            var updatedLocation = new UpdatedLocation(
                updator: updator,
                locationId: locationToUpdateId,
                locationName: "location name",
                cityId: city.LocationId,
                countryId: country.LocationId,
                divisionId: -1,
                regionId: region.LocationId,
                latitude: 2.35f,
                longitude: 5.0f,
                locationTypeId: place.LocationTypeId
                );
            context.Revert();

            var locationTypeName = "Division";
            var message = String.Format("The [{0}] with id [{1}] was not found.", locationTypeName, updatedLocation.DivisionId);
            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedLocation);
            };
            service.Invoking(x => x.Update(updatedLocation)).ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_Location_CityDoesNotExist()
        {
            var creatorId = 1;
            var revisorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var country = new Location
            {
                LocationName = "country",
                LocationId = 2
            };
            var division = new Location
            {
                LocationName = "division",
                LocationId = 3
            };
            var region = new Location
            {
                LocationName = "region",
                LocationId = 4
            };
            var place = new LocationType
            {
                LocationTypeId = LocationType.Place.Id,
                LocationTypeName = "place"
            };
            var locationToUpdateId = 5;
            Location locationToUpdate = null;


            context.SetupActions.Add(() =>
            {
                locationToUpdate = new Location
                {
                    LocationId = locationToUpdateId
                };
                locationToUpdate.History.CreatedBy = creatorId;
                locationToUpdate.History.RevisedBy = creatorId;
                locationToUpdate.History.CreatedOn = yesterday;
                locationToUpdate.History.RevisedOn = yesterday;
                context.Locations.Add(country);
                context.Locations.Add(division);
                context.Locations.Add(locationToUpdate);
                context.Locations.Add(region);
                context.LocationTypes.Add(place);
            });


            var updator = new User(revisorId);
            var updatedLocation = new UpdatedLocation(
                updator: updator,
                locationId: locationToUpdateId,
                locationName: "location name",
                cityId: -1,
                countryId: country.LocationId,
                divisionId: division.LocationId,
                regionId: region.LocationId,
                latitude: 2.35f,
                longitude: 5.0f,
                locationTypeId: place.LocationTypeId
                );
            context.Revert();

            var locationTypeName = "City";
            var message = String.Format("The [{0}] with id [{1}] was not found.", locationTypeName, updatedLocation.CityId);
            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedLocation);
            };
            service.Invoking(x => x.Update(updatedLocation)).ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_Location_RegionDoesNotExist()
        {
            var creatorId = 1;
            var revisorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var country = new Location
            {
                LocationName = "country",
                LocationId = 2
            };
            var division = new Location
            {
                LocationName = "division",
                LocationId = 3
            };
            var city = new Location
            {
                LocationName = "region",
                LocationId = 4
            };
            var place = new LocationType
            {
                LocationTypeId = LocationType.Place.Id,
                LocationTypeName = "place"
            };
            var locationToUpdateId = 5;
            Location locationToUpdate = null;

            context.SetupActions.Add(() =>
            {
                locationToUpdate = new Location
                {
                    LocationId = locationToUpdateId
                };
                locationToUpdate.History.CreatedBy = creatorId;
                locationToUpdate.History.RevisedBy = creatorId;
                locationToUpdate.History.CreatedOn = yesterday;
                locationToUpdate.History.RevisedOn = yesterday;
                context.Locations.Add(country);
                context.Locations.Add(division);
                context.Locations.Add(locationToUpdate);
                context.Locations.Add(city);
                context.LocationTypes.Add(place);
            });


            var updator = new User(revisorId);
            var updatedLocation = new UpdatedLocation(
                updator: updator,
                locationId: locationToUpdateId,
                locationName: "location name",
                cityId: city.LocationId,
                countryId: country.LocationId,
                divisionId: division.LocationId,
                regionId: -1,
                latitude: 2.35f,
                longitude: 5.0f,
                locationTypeId: place.LocationTypeId
                );
            context.Revert();

            var locationTypeName = "Region";
            var message = String.Format("The [{0}] with id [{1}] was not found.", locationTypeName, updatedLocation.RegionId);
            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedLocation);
            };
            service.Invoking(x => x.Update(updatedLocation)).ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_Location_LocationTypeDoesNotExist()
        {
            var creatorId = 1;
            var revisorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var city = new Location
            {
                LocationName = "city",
                LocationId = 1,
            };
            var country = new Location
            {
                LocationName = "country",
                LocationId = 2
            };
            var division = new Location
            {
                LocationName = "division",
                LocationId = 3
            };
            var region = new Location
            {
                LocationName = "region",
                LocationId = 4
            };
            var locationToUpdateId = 5;
            Location locationToUpdate = null;


            context.SetupActions.Add(() =>
            {
                locationToUpdate = new Location
                {
                    LocationId = locationToUpdateId
                };
                locationToUpdate.History.CreatedBy = creatorId;
                locationToUpdate.History.RevisedBy = creatorId;
                locationToUpdate.History.CreatedOn = yesterday;
                locationToUpdate.History.RevisedOn = yesterday;
                context.Locations.Add(city);
                context.Locations.Add(country);
                context.Locations.Add(division);
                context.Locations.Add(region);
                context.Locations.Add(locationToUpdate);
            });
            var updator = new User(revisorId);
            var updatedLocation = new UpdatedLocation(
                updator: updator,
                locationId: locationToUpdateId,
                locationName: "location name",
                cityId: city.LocationId,
                countryId: country.LocationId,
                divisionId: division.LocationId,
                regionId: region.LocationId,
                latitude: 2.35f,
                longitude: 5.0f,
                locationTypeId: LocationType.Place.Id
                );
            context.Revert();

            var message = String.Format("The location type with id [{0}] does not exist.", updatedLocation.LocationTypeId);
            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedLocation);
            };
            service.Invoking(x => x.Update(updatedLocation)).ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }
        #endregion

    }
}
