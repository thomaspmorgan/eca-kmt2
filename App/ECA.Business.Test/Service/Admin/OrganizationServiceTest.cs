using System;
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

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new OrganizationService(context);
        }

        /** Not working
        [TestMethod]
        public async Task TestGetOrganizations_CheckProperties()
        {
            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Division.Id,
                OrganizationTypeName = OrganizationType.Division.Value
            };

            var organization = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                Name = "name",
                Description = "test",
                Status = "status",
                ParentOrganization = new Organization()
            };

            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(organization);

            var defaultSorter = new ExpressionSorter<SimpleOrganizationDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleOrganizationDTO>(0, 1, defaultSorter);
            var serviceResultsAsync = await service.GetOrganizationsAsync(queryOperator);

            Assert.AreEqual(1, serviceResultsAsync.Total);
        }
        **/

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
                Assert.AreEqual(-1, testDto.ParentOrganizationId);
            };

            var dto = service.GetOrganizationById(org.OrganizationId);
            var dtoAsync = await service.GetOrganizationByIdAsync(org.OrganizationId);
            tester(dto);
            tester(dtoAsync);
        }

        [TestMethod]
        public async Task TestGetById_CheckLocations()
        {
            var country = new Location
            {
                LocationName = "USA",
                LocationId = 1
            };
            var addressLocation = new Location
            {
                LocationId = 2,
                City = "Nashville",
                Country = country,
                CountryId = country.LocationId,
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
            var address = new Address
            {
                AddressId = 1,
                Location = addressLocation,
                LocationId = addressLocation.LocationId,
                Organization = org,
                OrganizationId = org.OrganizationId
            };
            org.Addresses.Add(address);
            context.Addresses.Add(address);
            context.Organizations.Add(org);
            context.OrganizationTypes.Add(orgType);
            Action<OrganizationDTO> tester = (testDto) =>
            {
                Assert.AreEqual(1, testDto.Locations.Count());
                var testAddress = testDto.Locations.First();
                Assert.AreEqual(addressLocation.City, testAddress.City);
                Assert.AreEqual(-1, testAddress.CityId);
                Assert.AreEqual(addressLocation.Country.LocationName, testAddress.Country);
                Assert.AreEqual(addressLocation.Country.LocationId, testAddress.CountryId);
                Assert.AreEqual(address.AddressId, testAddress.Id);
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
            org.SocialMediaPresence.Add(facebook);

            context.SocialMedias.Add(facebook);
            context.SocialMediaTypes.Add(facebookType);
            context.Organizations.Add(org);
            context.OrganizationTypes.Add(orgType);
            Action<OrganizationDTO> tester = (testDto) =>
            {
                Assert.AreEqual(1, testDto.SocialMedias.Count());
                var firstMedia = testDto.SocialMedias.First();
                Assert.AreEqual(facebookType.SocialMediaTypeName, firstMedia.Type);
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
    }
}

