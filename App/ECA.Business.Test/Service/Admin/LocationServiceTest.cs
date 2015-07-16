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

        #region Address
        [TestMethod]
        public async Task TestCreate_CheckProperties()
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
                var displayName = "display";
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
                    displayName,
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
                    Assert.AreEqual(displayName, organizationAddress.DisplayName);
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

                context.Revert();
                beforeTester();
                await service.CreateAsync<Organization>(instance);
                afterTester();
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
                var displayName = "display";
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
                    displayName,
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
                var displayName = "display";
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
                    displayName,
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
                var displayName = "display";
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
                    displayName,
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
                var displayName = "display";
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
                    displayName,
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
            var displayName = "display";
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
                displayName,
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
                Assert.AreEqual(displayName, address.DisplayName);
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
                    DisplayName = "old display",
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
            //need to make sure country, city, region, division etc all exist
            context.Revert();
            service.Update(instance);
            tester();

            context.Revert();
            await service.UpdateAsync(instance);
            tester();
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
                DisplayName = "old display",
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
            var displayName = "display";
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
                displayName,
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
                DisplayName = "old display",
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
            var displayName = "display";
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
                displayName,
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
                DisplayName = "old display",
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
            var displayName = "display";
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
                displayName,
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
                DisplayName = "display Name",
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
                Assert.AreEqual(address.DisplayName, dto.AddressDisplayName);
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
                DisplayName = "display Name",
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
                DisplayName = "display Name",
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
                DisplayName = "display Name",
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

    }
}
