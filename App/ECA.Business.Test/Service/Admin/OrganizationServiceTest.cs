using System;
using FluentAssertions;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using System.Linq;
using Microsoft.QualityTools.Testing.Fakes;
using Moq;
using ECA.Business.Validation;
using ECA.Business.Service;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Admin
{
    /// <summary>
    /// Summary description for OrganizationServiceTest
    /// </summary>
    [TestClass]
    public class OrganizationServiceTest
    {
        private TestEcaContext context;
        private OrganizationService service;
        private Mock<IBusinessValidator<Object, UpdateOrganizationValidationEntity>> validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new Mock<IBusinessValidator<object, UpdateOrganizationValidationEntity>>();
            context = new TestEcaContext();
            service = new OrganizationService(context, validator.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetOrganizations_CheckProperties()
        {
            var country = new Location
            {
                LocationId = 1,
                LocationName = "Country",
            };
            var city = new Location
            {
                LocationId = 2,
                LocationName = "City"
            };
            var addressLocation = new Location
            {
                LocationId = 3,
                City = city,
                Country = country
            };
            var address = new Address
            {
                AddressId = 1,
                Location = addressLocation
            };
            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };

            var organization = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                OrganizationType = organizationType,
                Name = "name",
                Description = "test",
                Status = "status",
                ParentOrganization = new Organization()
            };
            organization.Addresses.Add(address);
            address.Organization = organization;
            context.Addresses.Add(address);
            context.Locations.Add(addressLocation);
            context.Locations.Add(country);
            context.Locations.Add(city);
            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(organization);
            Action<PagedQueryResults<SimpleOrganizationDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var org = results.Results.First();
                Assert.AreEqual(organization.Name, org.Name);
                Assert.AreEqual(organizationType.OrganizationTypeName, org.OrganizationType);
                Assert.AreEqual(organization.OrganizationId, org.OrganizationId);
                Assert.AreEqual(organization.Status, org.Status);
                Assert.AreEqual(city.LocationName + ", " + country.LocationName, org.Location);
            };
            var defaultSorter = new ExpressionSorter<SimpleOrganizationDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleOrganizationDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetOrganizations(queryOperator);
            var serviceResultsAsync = await service.GetOrganizationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOrganizations_CheckPrimaryAddress()
        {
            var country = new Location
            {
                LocationId = 1,
                LocationName = "Country",
            };
            var city = new Location
            {
                LocationId = 2,
                LocationName = "City"
            };
            var addressLocation = new Location
            {
                LocationId = 3,
                City = city,
                Country = country
            };
            var primaryAddress = new Address
            {
                AddressId = 1,
                Location = addressLocation,
                IsPrimary = true
            };
            var secondaryAddress = new Address
            {
                Location = new Location
                {
                    City = new Location
                    {
                        LocationName = "other City"
                    },
                    Country = new Location
                    {
                        LocationName = "other country"
                    }
                }
            };
            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };

            var organization = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                OrganizationType = organizationType,
                Name = "name",
                Description = "test",
                Status = "status",
                ParentOrganization = new Organization()
            };
            organization.Addresses.Add(secondaryAddress);
            secondaryAddress.Organization = organization;
            organization.Addresses.Add(primaryAddress);
            primaryAddress.Organization = organization;

            context.Addresses.Add(secondaryAddress);
            context.Addresses.Add(primaryAddress);
            context.Locations.Add(addressLocation);
            context.Locations.Add(country);
            context.Locations.Add(city);
            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(organization);
            Action<PagedQueryResults<SimpleOrganizationDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var org = results.Results.First();
                Assert.AreEqual(city.LocationName + ", " + country.LocationName, org.Location);
            };
            var defaultSorter = new ExpressionSorter<SimpleOrganizationDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleOrganizationDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetOrganizations(queryOperator);
            var serviceResultsAsync = await service.GetOrganizationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOrganizations_AddressHasCountryOnly()
        {
            var country = new Location
            {
                LocationId = 1,
                LocationName = "Country",
            };
            var addressLocation = new Location
            {
                LocationId = 3,
                Country = country
            };
            var address = new Address
            {
                AddressId = 1,
                Location = addressLocation
            };
            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };

            var organization = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                OrganizationType = organizationType,
                Name = "name",
                Description = "test",
                Status = "status",
                ParentOrganization = new Organization()
            };
            organization.Addresses.Add(address);
            address.Organization = organization;
            context.Addresses.Add(address);
            context.Locations.Add(addressLocation);
            context.Locations.Add(country);
            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(organization);
            Action<PagedQueryResults<SimpleOrganizationDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var org = results.Results.First();
                Assert.AreEqual(country.LocationName, org.Location);
            };
            var defaultSorter = new ExpressionSorter<SimpleOrganizationDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleOrganizationDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetOrganizations(queryOperator);
            var serviceResultsAsync = await service.GetOrganizationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOrganizations_AddressHasCityOnly()
        {
            var city = new Location
            {
                LocationId = 2,
                LocationName = "City"
            };
            var addressLocation = new Location
            {
                LocationId = 3,
                City = city
            };
            var address = new Address
            {
                AddressId = 1,
                Location = addressLocation
            };
            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };

            var organization = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                OrganizationType = organizationType,
                Name = "name",
                Description = "test",
                Status = "status",
                ParentOrganization = new Organization()
            };
            organization.Addresses.Add(address);
            address.Organization = organization;
            context.Addresses.Add(address);
            context.Locations.Add(addressLocation);
            context.Locations.Add(city);
            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(organization);
            Action<PagedQueryResults<SimpleOrganizationDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var org = results.Results.First();
                Assert.AreEqual(city.LocationName, org.Location);
            };
            var defaultSorter = new ExpressionSorter<SimpleOrganizationDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleOrganizationDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetOrganizations(queryOperator);
            var serviceResultsAsync = await service.GetOrganizationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetOrganizations_AddressIsNull()
        {

            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };

            var organization = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                OrganizationType = organizationType,
                Name = "name",
                Description = "test",
                Status = "status",
                ParentOrganization = new Organization()
            };
            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(organization);
            Action<PagedQueryResults<SimpleOrganizationDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var org = results.Results.First();
                Assert.AreEqual(String.Empty, org.Location);
            };
            var defaultSorter = new ExpressionSorter<SimpleOrganizationDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleOrganizationDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetOrganizations(queryOperator);
            var serviceResultsAsync = await service.GetOrganizationsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        #endregion

        #region Get By Id
        [TestMethod]
        public async Task TestGetById_OrgIdDoesNotExist()
        {
            Action<OrganizationDTO> tester = (testDto) =>
            {
                Assert.IsNull(testDto);
            };
            var dto = service.GetOrganizationById(0);
            var dtoAsync = await service.GetOrganizationByIdAsync(0);
            tester(dto);
            tester(dtoAsync);
        }

        [TestMethod]
        public async Task TestGetById_CheckProperties()
        {
            var lastRevised = DateTimeOffset.UtcNow;
            var orgType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };
            var org = new Organization
            {
                Description = "description",
                Name = "name",
                OrganizationId = 1,
                OrganizationType = orgType,
                OrganizationTypeId = orgType.OrganizationTypeId,
                Website = "website"
            };
            org.History.RevisedOn = lastRevised;

            context.Organizations.Add(org);
            context.OrganizationTypes.Add(orgType);
            Action<OrganizationDTO> tester = (testDto) =>
            {
                Assert.AreEqual(orgType.OrganizationTypeId, testDto.OrganizationTypeId);
                Assert.AreEqual(orgType.OrganizationTypeName, testDto.OrganizationType);
                Assert.AreEqual(org.Description, testDto.Description);
                Assert.AreEqual(org.Name, testDto.Name);
                Assert.AreEqual(org.Website, testDto.Website);
            };

            var dto = service.GetOrganizationById(org.OrganizationId);
            var dtoAsync = await service.GetOrganizationByIdAsync(org.OrganizationId);
            tester(dto);
            tester(dtoAsync);
        }

        [TestMethod]
        public async Task TestGetById_HasParentOrg()
        {
            var parentOrg = new Organization
            {
                Name = "parent Org",
                OrganizationId = 2
            };
            var lastRevised = DateTimeOffset.UtcNow;
            var orgType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };
            var org = new Organization
            {
                Description = "description",
                Name = "name",
                OrganizationId = 1,
                OrganizationType = orgType,
                OrganizationTypeId = orgType.OrganizationTypeId,
                Website = "website",
                ParentOrganization = parentOrg,
            };
            org.History.RevisedOn = lastRevised;


            context.Organizations.Add(org);
            context.Organizations.Add(parentOrg);
            context.OrganizationTypes.Add(orgType);
            Action<OrganizationDTO> tester = (testDto) =>
            {
                Assert.AreEqual(parentOrg.Name, testDto.ParentOrganizationName);
                Assert.AreEqual(parentOrg.OrganizationId, testDto.ParentOrganizationId);
            };

            var dto = service.GetOrganizationById(org.OrganizationId);
            var dtoAsync = await service.GetOrganizationByIdAsync(org.OrganizationId);
            tester(dto);
            tester(dtoAsync);
        }

        [TestMethod]
        public async Task TestGetById_DoesNotHaveParentOrg()
        {
            var lastRevised = DateTimeOffset.UtcNow;
            var orgType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };
            var org = new Organization
            {
                Description = "description",
                Name = "name",
                OrganizationId = 1,
                OrganizationType = orgType,
                OrganizationTypeId = orgType.OrganizationTypeId,
                Website = "website",
            };
            org.History.RevisedOn = lastRevised;


            context.Organizations.Add(org);
            context.OrganizationTypes.Add(orgType);
            Action<OrganizationDTO> tester = (testDto) =>
            {
                Assert.IsNull(testDto.ParentOrganizationName);
                Assert.IsFalse(testDto.ParentOrganizationId.HasValue);
            };

            var dto = service.GetOrganizationById(org.OrganizationId);
            var dtoAsync = await service.GetOrganizationByIdAsync(org.OrganizationId);
            tester(dto);
            tester(dtoAsync);
        }

        [TestMethod]
        public async Task TestGetById_CheckAddresses()
        {
            var country = new Location
            {
                LocationName = "USA",
                LocationId = 1
            };

            var city = new Location
            {
                LocationId = 3,
                LocationName = "Nashville"
            };
            var division = new Location
            {
                LocationId = 4,
                LocationName = "TN"
            };
            var addressLocation = new Location
            {
                LocationId = 2,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };

            var lastRevised = DateTimeOffset.UtcNow;
            var orgType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };
            var org = new Organization
            {
                Description = "description",
                Name = "name",
                OrganizationId = 1,
                OrganizationType = orgType,
                OrganizationTypeId = orgType.OrganizationTypeId,
                Website = "website"
            };
            org.History.RevisedOn = lastRevised;
            var addressType = new AddressType
            {
                AddressName = AddressType.Business.Value,
                AddressTypeId = AddressType.Business.Id
            };
            var address = new Address
            {
                AddressId = 1,
                Location = addressLocation,
                LocationId = addressLocation.LocationId,
                Organization = org,
                OrganizationId = org.OrganizationId,
                AddressType = addressType,
                AddressTypeId = addressType.AddressTypeId
            };
            org.Addresses.Add(address);
            context.Addresses.Add(address);
            context.AddressTypes.Add(addressType);
            context.Organizations.Add(org);
            context.OrganizationTypes.Add(orgType);
            context.Locations.Add(city);
            context.Locations.Add(division);
            context.Locations.Add(country);
            Action<OrganizationDTO> tester = (testDto) =>
            {
                Assert.AreEqual(1, testDto.Addresses.Count());
                var testAddress = testDto.Addresses.First();
                Assert.AreEqual(addressLocation.City.LocationName, testAddress.City);
                Assert.AreEqual(addressLocation.City.LocationId, testAddress.CityId);
                Assert.AreEqual(addressLocation.Country.LocationName, testAddress.Country);
                Assert.AreEqual(addressLocation.Country.LocationId, testAddress.CountryId);
                Assert.AreEqual(addressLocation.Division.LocationName, testAddress.Division);
                Assert.AreEqual(addressLocation.Division.LocationId, testAddress.DivisionId);
                Assert.AreEqual(address.AddressId, testAddress.AddressId);
                Assert.AreEqual(addressLocation.LocationId, testAddress.LocationId);
                Assert.AreEqual(addressLocation.PostalCode, testAddress.PostalCode);
                Assert.AreEqual(addressLocation.Street1, testAddress.Street1);
                Assert.AreEqual(addressLocation.Street2, testAddress.Street2);
                Assert.AreEqual(addressLocation.Street3, testAddress.Street3);
            };

            var dto = service.GetOrganizationById(org.OrganizationId);
            var dtoAsync = await service.GetOrganizationByIdAsync(org.OrganizationId);
            tester(dto);
            tester(dtoAsync);
        }

        [TestMethod]
        public async Task TestGetById_CheckSocialMedia()
        {
            var lastRevised = DateTimeOffset.UtcNow;
            var orgType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };
            var org = new Organization
            {
                Description = "description",
                Name = "name",
                OrganizationId = 1,
                OrganizationType = orgType,
                OrganizationTypeId = orgType.OrganizationTypeId,
                Website = "website"
            };
            var facebookType = new SocialMediaType
            {
                SocialMediaTypeId = SocialMediaType.Facebook.Id,
                SocialMediaTypeName = SocialMediaType.Facebook.Value
            };
            var facebook = new SocialMedia
            {
                Organization = org,
                OrganizationId = org.OrganizationId,
                SocialMediaId = 1,
                SocialMediaType = facebookType,
                SocialMediaTypeId = facebookType.SocialMediaTypeId,
                SocialMediaValue = "someone@facebook.com"
            };
            org.History.RevisedOn = lastRevised;
            org.SocialMedias.Add(facebook);

            context.SocialMedias.Add(facebook);
            context.SocialMediaTypes.Add(facebookType);
            context.Organizations.Add(org);
            context.OrganizationTypes.Add(orgType);
            Action<OrganizationDTO> tester = (testDto) =>
            {
                Assert.AreEqual(1, testDto.SocialMedias.Count());
                var firstMedia = testDto.SocialMedias.First();
                Assert.AreEqual(facebookType.SocialMediaTypeId, firstMedia.SocialMediaTypeId);
                Assert.AreEqual(facebookType.SocialMediaTypeName, firstMedia.SocialMediaType);
                Assert.AreEqual(facebook.SocialMediaValue, firstMedia.Value);
                Assert.AreEqual(facebook.SocialMediaId, firstMedia.Id);
            };

            var dto = service.GetOrganizationById(org.OrganizationId);
            var dtoAsync = await service.GetOrganizationByIdAsync(org.OrganizationId);
            tester(dto);
            tester(dtoAsync);
        }

        [TestMethod]
        public async Task TestGetById_CheckContacts()
        {
            var lastRevised = DateTimeOffset.UtcNow;
            var orgType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.ForeignEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.ForeignEducationalInstitution.Value
            };
            var org = new Organization
            {
                Description = "description",
                Name = "name",
                OrganizationId = 1,
                OrganizationType = orgType,
                OrganizationTypeId = orgType.OrganizationTypeId,
                Website = "website"
            };
            var contact = new Contact
            {
                ContactId = 1,
                FullName = "full name"
            };

            org.History.RevisedOn = lastRevised;
            org.Contacts.Add(contact);
            context.Contacts.Add(contact);
            context.Organizations.Add(org);
            context.OrganizationTypes.Add(orgType);
            Action<OrganizationDTO> tester = (testDto) =>
            {
                Assert.AreEqual(1, testDto.Contacts.Count());
                var firstContact = testDto.Contacts.First();
                Assert.AreEqual(contact.ContactId, firstContact.Id);
                Assert.AreEqual(contact.FullName, firstContact.Value);
            };

            var dto = service.GetOrganizationById(org.OrganizationId);
            var dtoAsync = await service.GetOrganizationByIdAsync(org.OrganizationId);
            tester(dto);
            tester(dtoAsync);
        }

        #endregion

        #region Create
        [TestMethod]
        public async Task TestCreateAsync_CheckProperties()
        {
            var user = new User(1);
            var name = "name";
            var description = "description";
            var organizationType = OrganizationType.USEducationalInstitution.Id;
            var website = "http://google.com";
            var newOrganization = new NewOrganization(user, name, description, organizationType, new List<int>(), website, new List<int>());
            var organization = await service.CreateAsync(newOrganization);

            Assert.AreEqual(name, organization.Name);
            Assert.AreEqual(description, organization.Description);
            Assert.AreEqual(organizationType, organization.OrganizationTypeId);
            Assert.AreEqual(website, organization.Website);
            Assert.AreEqual("Active", organization.Status);
        }

        [TestMethod]
        public async Task TestCreateAsync_CheckRoles()
        {
            var organizationRole = new OrganizationRole
            {
                OrganizationRoleId = 1,
            };

            context.OrganizationRoles.Add(organizationRole);

            var user = new User(1);
            var name = "name";
            var description = "description";
            var organizationType = OrganizationType.USEducationalInstitution.Id;
            var website = "http://google.com";
            var newOrganization = new NewOrganization(user, name, description, organizationType, new List<int> { organizationRole.OrganizationRoleId }, website, new List<int>());
            var organization = await service.CreateAsync(newOrganization);

            var role = organization.OrganizationRoles.FirstOrDefault();
            Assert.AreEqual(organizationRole.OrganizationRoleId, role.OrganizationRoleId);
        }

        [TestMethod]
        public async Task TestCreateAsync_CheckContacts()
        {
            var organizationContact = new Contact
            {
                ContactId = 1
            };

            context.Contacts.Add(organizationContact);

            var user = new User(1);
            var name = "name";
            var description = "description";
            var organizationType = OrganizationType.USEducationalInstitution.Id;
            var website = "http://google.com";
            var newOrganization = new NewOrganization(user, name, description, organizationType, new List<int>(), website, new List<int> { organizationContact.ContactId });
            var organization = await service.CreateAsync(newOrganization);

            var contact = organization.Contacts.FirstOrDefault();
            Assert.AreEqual(organizationContact.ContactId, contact.ContactId);
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task TestUpdate_OrganizationDoesNotExist()
        {

            var updater = new User(1);
            var instance = new EcaOrganization(updater, 0, "website", OrganizationType.USEducationalInstitution.Id, null, null, null, "name", "desc");
            Func<Task> invokeAsync = () =>
            {
                return service.UpdateAsync(instance);
            };

            service.Invoking(x => x.Update(instance)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The organization with id [{0}] was not found.", instance.OrganizationId));

            invokeAsync.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The organization with id [{0}] was not found.", instance.OrganizationId));
        }

        [TestMethod]
        public async Task TestUpdate_ParentOrganizationDoesNotExist()
        {
            var organization = new Organization
            {
                OrganizationId = 1
            };
            var parentOrgId = 10;
            context.Organizations.Add(organization);
            var updater = new User(1);
            var instance = new EcaOrganization(updater, organization.OrganizationId, "website", OrganizationType.USEducationalInstitution.Id, null, null, parentOrgId, "name", "desc");
            Func<Task> invokeAsync = () =>
            {
                return service.UpdateAsync(instance);
            };

            service.Invoking(x => x.Update(instance)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The organization with id [{0}] was not found.", parentOrgId));

            invokeAsync.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The organization with id [{0}] was not found.", parentOrgId));
        }

        [TestMethod]
        public async Task TestUpdate_OrganizationTypeDoesNotExist()
        {
            var orgTypeId = OrganizationType.USEducationalInstitution.Id;
            var organization = new Organization
            {
                OrganizationId = 1
            };
            context.Organizations.Add(organization);
            var updater = new User(1);
            var instance = new EcaOrganization(updater, organization.OrganizationId, "website", orgTypeId, null, null, null, "name", "desc");
            Func<Task> invokeAsync = () =>
            {
                return service.UpdateAsync(instance);
            };

            service.Invoking(x => x.Update(instance)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The organization type with id [{0}] was not found.", orgTypeId));

            invokeAsync.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The organization type with id [{0}] was not found.", orgTypeId));
        }

        [TestMethod]
        public async Task TestUpdate_CheckProperties()
        {
            var creatorId = 10;
            var orgId = 1;
            var oldName = "oldName";
            var oldDescription = "oldDescription";
            var oldWebsite = "old website";
            var createDate = DateTimeOffset.Now.AddDays(-1.0);

            var publicInternationalOrganizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.PublicInternationalOrganizationPio.Id,
                OrganizationTypeName = OrganizationType.PublicInternationalOrganizationPio.Value
            };
            var otherOrganizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.USEducationalInstitution.Id,
                OrganizationTypeName = OrganizationType.USEducationalInstitution.Value
            };

            var organization = new Organization
            {
                Description = oldDescription,
                Name = oldName,
                OrganizationId = orgId,
                OrganizationTypeId = publicInternationalOrganizationType.OrganizationTypeId,
                OrganizationType = publicInternationalOrganizationType,
                Website = oldWebsite
            };
            organization.History.CreatedBy = creatorId;
            organization.History.CreatedOn = createDate;
            organization.History.RevisedBy = creatorId;
            organization.History.RevisedOn = createDate;

            context.SetupActions.Add(() =>
            {
                Assert.AreEqual(0, context.Organizations.Count());
                Assert.AreEqual(0, context.OrganizationTypes.Count());
                context.Organizations.Add(organization);
                context.OrganizationTypes.Add(otherOrganizationType);
                context.OrganizationTypes.Add(publicInternationalOrganizationType);
            });

            var updaterId = 20;
            var newName = "new Name";
            var newDescription = "new Description";
            var newWebsite = "new website";
            var updater = new User(updaterId);

            Action tester = () =>
            {
                Assert.AreEqual(1, context.Organizations.Count());
                Assert.AreEqual(2, context.OrganizationTypes.Count());
                var updatedOrg = context.Organizations.Where(x => x.OrganizationId == orgId).FirstOrDefault();
                Assert.AreEqual(newName, updatedOrg.Name);
                Assert.AreEqual(newDescription, updatedOrg.Description);
                Assert.AreEqual(newWebsite, updatedOrg.Website);
                Assert.AreEqual(createDate, updatedOrg.History.CreatedOn);
                Assert.AreEqual(creatorId, updatedOrg.History.CreatedBy);
                Assert.AreEqual(updaterId, updatedOrg.History.RevisedBy);
                DateTimeOffset.Now.Should().BeCloseTo(updatedOrg.History.RevisedOn, 2000);
            };

            var instance = new EcaOrganization(updater, orgId, newWebsite, otherOrganizationType.OrganizationTypeId, null, null, null, newName, newDescription);
            context.Revert();
            service.Update(instance);
            tester();

            context.Revert();
            await service.UpdateAsync(instance);
            tester();
        }

        [TestMethod]
        public async Task TestUpdate_SetContacts()
        {
            var creatorId = 10;
            var orgId = 1;
            var oldName = "oldName";
            var oldDescription = "oldDescription";
            var oldWebsite = "old website";
            var createDate = DateTimeOffset.Now.AddDays(-1.0);

            var publicInternationalOrganizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.PublicInternationalOrganizationPio.Id,
                OrganizationTypeName = OrganizationType.PublicInternationalOrganizationPio.Value
            };
            var organization = new Organization
            {
                Description = oldDescription,
                Name = oldName,
                OrganizationId = orgId,
                OrganizationTypeId = publicInternationalOrganizationType.OrganizationTypeId,
                OrganizationType = publicInternationalOrganizationType,
                Website = oldWebsite
            };
            var contact = new Contact
            {
                ContactId = 1
            };

            context.SetupActions.Add(() =>
            {
                Assert.AreEqual(0, context.Organizations.Count());
                Assert.AreEqual(0, context.OrganizationTypes.Count());
                Assert.AreEqual(0, context.Contacts.Count());
                organization.Contacts.Clear();
                context.Organizations.Add(organization);
                context.OrganizationTypes.Add(publicInternationalOrganizationType);
                context.Contacts.Add(contact);
            });

            var updaterId = 20;
            var newName = "new Name";
            var newDescription = "new Description";
            var newWebsite = "new website";
            var updater = new User(updaterId);

            Action tester = () =>
            {
                Assert.AreEqual(1, context.Organizations.Count());
                Assert.AreEqual(1, context.OrganizationTypes.Count());
                var updatedOrg = context.Organizations.Where(x => x.OrganizationId == orgId).FirstOrDefault();
                Assert.AreEqual(1, updatedOrg.Contacts.Count());
                Assert.AreEqual(contact.ContactId, updatedOrg.Contacts.First().ContactId);
            };
            var contactIds = new List<int> { contact.ContactId };
            var instance = new EcaOrganization(updater, orgId, newWebsite, organization.OrganizationTypeId, null, contactIds, null, newName, newDescription);
            context.Revert();
            service.Update(instance);
            tester();

            context.Revert();
            await service.UpdateAsync(instance);
            tester();
        }

        [TestMethod]
        public async Task TestUpdate_SetParentOrganization()
        {
            var orgId = 1;
            var oldName = "oldName";
            var oldDescription = "oldDescription";
            var oldWebsite = "old website";

            var publicInternationalOrganizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.PublicInternationalOrganizationPio.Id,
                OrganizationTypeName = OrganizationType.PublicInternationalOrganizationPio.Value
            };

            var organization = new Organization
            {
                Description = oldDescription,
                Name = oldName,
                OrganizationId = orgId,
                OrganizationTypeId = publicInternationalOrganizationType.OrganizationTypeId,
                OrganizationType = publicInternationalOrganizationType,
                Website = oldWebsite
            };
            var parentOrganization = new Organization
            {
                OrganizationId = organization.OrganizationId + 1
            };

            context.SetupActions.Add(() =>
            {
                organization.ParentOrganizationId = null;
                organization.ParentOrganization = null;

                Assert.AreEqual(0, context.Organizations.Count());
                Assert.AreEqual(0, context.OrganizationTypes.Count());
                Assert.IsNull(organization.ParentOrganization);
                Assert.IsFalse(organization.ParentOrganizationId.HasValue);
                context.Organizations.Add(organization);
                context.Organizations.Add(parentOrganization);
                context.OrganizationTypes.Add(publicInternationalOrganizationType);
            });

            var updaterId = 20;
            var updater = new User(updaterId);

            Action tester = () =>
            {
                Assert.AreEqual(2, context.Organizations.Count());
                Assert.AreEqual(1, context.OrganizationTypes.Count());
                var updatedOrg = context.Organizations.Where(x => x.OrganizationId == orgId).FirstOrDefault();
                Assert.AreEqual(parentOrganization, updatedOrg.ParentOrganization);
                Assert.AreEqual(parentOrganization.OrganizationId, updatedOrg.ParentOrganizationId.Value);
            };

            var instance = new EcaOrganization(updater, orgId, organization.Website, organization.OrganizationTypeId, null, null, parentOrganization.OrganizationId, organization.Name, organization.Description);
            context.Revert();
            service.Update(instance);
            tester();

            context.Revert();
            await service.UpdateAsync(instance);
            tester();
        }

        [TestMethod]
        public async Task TestUpdate_RemoveParentOrganization()
        {
            var orgId = 1;
            var oldName = "oldName";
            var oldDescription = "oldDescription";
            var oldWebsite = "old website";

            var publicInternationalOrganizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.PublicInternationalOrganizationPio.Id,
                OrganizationTypeName = OrganizationType.PublicInternationalOrganizationPio.Value
            };
            var parentOrganization = new Organization
            {
                OrganizationId = 2
            };

            var organization = new Organization
            {
                Description = oldDescription,
                Name = oldName,
                OrganizationId = orgId,
                OrganizationTypeId = publicInternationalOrganizationType.OrganizationTypeId,
                OrganizationType = publicInternationalOrganizationType,
                Website = oldWebsite,
                ParentOrganizationId = parentOrganization.OrganizationId,
                ParentOrganization = parentOrganization
            };

            context.SetupActions.Add(() =>
            {
                organization.ParentOrganization = parentOrganization;
                organization.ParentOrganizationId = parentOrganization.OrganizationId;
                Assert.AreEqual(0, context.Organizations.Count());
                Assert.AreEqual(0, context.OrganizationTypes.Count());
                Assert.IsNotNull(organization.ParentOrganization);
                Assert.IsTrue(organization.ParentOrganizationId.HasValue);
                context.Organizations.Add(organization);
                context.Organizations.Add(parentOrganization);
                context.OrganizationTypes.Add(publicInternationalOrganizationType);
            });

            var updaterId = 20;
            var updater = new User(updaterId);

            Action tester = () =>
            {
                Assert.AreEqual(2, context.Organizations.Count());
                Assert.AreEqual(1, context.OrganizationTypes.Count());
                var updatedOrg = context.Organizations.Where(x => x.OrganizationId == orgId).FirstOrDefault();
                Assert.IsNull(updatedOrg.ParentOrganization);
                Assert.IsFalse(updatedOrg.ParentOrganizationId.HasValue);
            };

            var instance = new EcaOrganization(updater, orgId, organization.Website, organization.OrganizationTypeId, null, null, null, organization.Name, organization.Description);
            context.Revert();
            service.Update(instance);
            tester();

            context.Revert();
            await service.UpdateAsync(instance);
            tester();
        }

        [TestMethod]
        public async Task TestUpdate_CheckValidatorIsExecuted()
        {
            var orgId = 1;
            var oldName = "oldName";
            var oldDescription = "oldDescription";
            var oldWebsite = "old website";

            var publicInternationalOrganizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.PublicInternationalOrganizationPio.Id,
                OrganizationTypeName = OrganizationType.PublicInternationalOrganizationPio.Value
            };

            var organization = new Organization
            {
                Description = oldDescription,
                Name = oldName,
                OrganizationId = orgId,
                OrganizationTypeId = publicInternationalOrganizationType.OrganizationTypeId,
                OrganizationType = publicInternationalOrganizationType,
                Website = oldWebsite,
            };

            context.SetupActions.Add(() =>
            {
                Assert.AreEqual(0, context.Organizations.Count());
                Assert.AreEqual(0, context.OrganizationTypes.Count());
                context.Organizations.Add(organization);
                context.OrganizationTypes.Add(publicInternationalOrganizationType);
            });

            var updaterId = 20;
            var updater = new User(updaterId);

            validator.Verify(x => x.ValidateUpdate(It.IsAny<UpdateOrganizationValidationEntity>()), Times.Never());
            var instance = new EcaOrganization(updater, orgId, organization.Website, organization.OrganizationTypeId, null, null, null, organization.Name, organization.Description);
            context.Revert();
            service.Update(instance);
            validator.Verify(x => x.ValidateUpdate(It.IsAny<UpdateOrganizationValidationEntity>()), Times.Once());

            context.Revert();
            await service.UpdateAsync(instance);
            validator.Verify(x => x.ValidateUpdate(It.IsAny<UpdateOrganizationValidationEntity>()), Times.Exactly(2));
        }
        #endregion
    }
}

