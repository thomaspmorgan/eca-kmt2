using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.Logging;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class PersonServiceTest
    {
        private TestEcaContext context;
        private PersonService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new PersonService(context, new TraceLogger());
        }

        #region Get Pii By Id
        [TestMethod]
        public async Task TestGetPiiById_CheckProperties()
        {
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "genderName"
            };

            var person = new Person
            {
                PersonId = 1,
                Gender = gender,
                DateOfBirth = DateTimeOffset.Now,
                FirstName = "firstName",
                LastName = "lastName",
                NamePrefix = "namePrefix",
                NameSuffix = "nameSuffix",
                GivenName = "givenName",
                FamilyName = "familyName",
                MiddleName = "middleName",
                Patronym = "patronym",
                Alias = "alias",
                Ethnicity = "ethnicity",
                MedicalConditions = "medical conditions"
            };

            context.Genders.Add(gender);
            context.People.Add(person);

            Action<PiiDTO> tester = (serviceResult) => 
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(gender.GenderName, serviceResult.Gender);
                DateTimeOffset.UtcNow.Should().BeCloseTo(serviceResult.DateOfBirth, DbContextHelper.DATE_PRECISION);
                Assert.AreEqual(person.FirstName, serviceResult.FirstName);
                Assert.AreEqual(person.LastName, serviceResult.LastName);
                Assert.AreEqual(person.NamePrefix, serviceResult.NamePrefix);
                Assert.AreEqual(person.NameSuffix, serviceResult.NameSuffix);
                Assert.AreEqual(person.GivenName, serviceResult.GivenName);
                Assert.AreEqual(person.FamilyName, serviceResult.FamilyName);
                Assert.AreEqual(person.MiddleName, serviceResult.MiddleName);
                Assert.AreEqual(person.Patronym, serviceResult.Patronym);
                Assert.AreEqual(person.Alias, serviceResult.Alias);
                Assert.AreEqual(person.Ethnicity, serviceResult.Ethnicity);
                Assert.AreEqual(person.MedicalConditions, serviceResult.MedicalConditions);
            };

            var result = this.service.GetPiiById(person.PersonId);
            var resultAsync = await this.service.GetPiiByIdAsync(person.PersonId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetPiiById_CheckCountriesOfCitizenship()
        {
            var location = new Location
            {
                LocationId = 1,
                LocationName = "locationName"
            };

            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "locationName2"
            };

            var location3 = new Location
            {
                LocationId = 3,
                LocationName = "locationName3"
            };

            var person = new Person
            {
                PersonId = 1,
                Gender = new Gender(),
            };

            person.CountriesOfCitizenship.Add(location);
            person.CountriesOfCitizenship.Add(location2);
            person.CountriesOfCitizenship.Add(location3);

            context.Locations.Add(location);
            context.Locations.Add(location2);
            context.Locations.Add(location3);
            context.People.Add(person);

            Action<PiiDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                CollectionAssert.AreEqual(context.Locations.Select(x => x.LocationId).ToList(), serviceResult.CountriesOfCitizenship.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(context.Locations.Select(x => x.LocationName).ToList(), serviceResult.CountriesOfCitizenship.Select(x => x.Value).ToList());
            };
            var result = this.service.GetPiiById(person.PersonId);
            var resultAsync = await this.service.GetPiiByIdAsync(person.PersonId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetPiiById_CheckHomeAddresses()
        {
            var country = new Location
            {
                LocationId = 2,
                LocationName = "country"
            };

            var location = new Location
            {
                LocationId = 1,
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
                City = "city",
                PostalCode = "postalCode",
                CountryId = 2,
                Country = country
            };

            var address = new Address
            {
                AddressId = 1,
                AddressTypeId = AddressType.Home.Id,
                Location = location
            };


            var person = new Person
            {
                PersonId = 1,
                Gender = new Gender()
            };

            
            person.Addresses.Add(address);

            context.Locations.Add(country);
            context.Locations.Add(location);
            context.Addresses.Add(address);
            context.People.Add(person);

            Action<PiiDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                var homeAddresses = serviceResult.HomeAddresses;
                Assert.AreEqual(1, homeAddresses.Count());
                var homeAddress = homeAddresses.FirstOrDefault();
                Assert.AreEqual(location.Street1, homeAddress.Street1);
                Assert.AreEqual(location.Street2, homeAddress.Street2);
                Assert.AreEqual(location.Street3, homeAddress.Street3);
                Assert.AreEqual(location.City, homeAddress.City);
                Assert.AreEqual(location.PostalCode, homeAddress.PostalCode);
                Assert.AreEqual(country.LocationName, homeAddress.Country);
            };
            var result = this.service.GetPiiById(person.PersonId);
            var resultAsync = await this.service.GetPiiByIdAsync(person.PersonId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetPiiById_CheckMaritalStatus()
        {
            var maritalStatus = new MaritalStatus 
            {
                MaritalStatusId = 1,
                Description = "description"
            };

            var person = new Person
            {
                PersonId = 1,
                Gender = new Gender(),
                MaritalStatusId = maritalStatus.MaritalStatusId,
                MaritalStatus = maritalStatus
            };

            context.MaritalStatuses.Add(maritalStatus);
            context.People.Add(person);

            Action<PiiDTO> tester = (serviceResult) =>
            {
                Assert.AreEqual(maritalStatus.Description, serviceResult.MaritalStatus);
            };
            var result = this.service.GetPiiById(person.PersonId);
            var resultAsync = await this.service.GetPiiByIdAsync(person.PersonId);

            tester(result);
            tester(resultAsync);
        }

                [TestMethod]
        public async Task TestGetPiiById_PlaceOfBirth()
        {
            var country = new Location
            {
                LocationId = 1,
                LocationTypeId = LocationType.Country.Id,
                LocationName = "country"
            };

            var city = new Location
            {
               LocationId = 2, 
               LocationTypeId = LocationType.City.Id,
               Country = country,
               CountryId = country.CountryId
            };

            var person = new Person
            {
                PersonId = 1,
                Gender = new Gender(),
                PlaceOfBirth = city,
                PlaceOfBirthId = city.LocationId
            };

            context.Locations.Add(country);
            context.Locations.Add(city);
            context.People.Add(person);

            Action<PiiDTO> tester = (serviceResult) =>
            {
                Assert.AreEqual(city.LocationName, serviceResult.CityOfBirth);
                Assert.AreEqual(country.LocationName, serviceResult.CountryOfBirth);
            };

            var result = service.GetPiiById(person.PersonId);
            var resultAsync = service.GetPiiById(person.PersonId);

            tester(result);
            tester(resultAsync);
        }
        #endregion

        #region Get Contact Info By Id
        [TestMethod]
        public async Task TestGetContactInfoById_Emails()
        {
            var email = new EmailAddress
            {
                EmailAddressId = 1,
                Address = "test@test.com"
            };

            var email2 = new EmailAddress
            {
                EmailAddressId = 2,
                Address = "test1@test.com"

            };
            var email3 = new EmailAddress
            {
                EmailAddressId = 3,
                Address = "test2@test.com"
            };

            var person = new Person
            {
                PersonId = 1
            };

            person.Emails.Add(email);

            context.EmailAddresses.Add(email);
            context.People.Add(person);

            Action<ContactInfoDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(person.Emails.Count(), serviceResult.Emails.Count());
                CollectionAssert.AreEqual(person.Emails.Select(x => x.EmailAddressId).ToList(), serviceResult.Emails.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(person.Emails.Select(x => x.Address).ToList(), serviceResult.Emails.Select(x => x.Value).ToList());
            };
            
            var result = service.GetContactInfoById(person.PersonId);
            var resultAsync = await service.GetContactInfoByIdAsync(person.PersonId);
        }

        [TestMethod]
        public async Task TestGetContactInfoById_SocialMedias()
        {
            var socialMediaType = new SocialMediaType
            {
                SocialMediaTypeId = 1,
                SocialMediaTypeName = "socialMediaTypeName"
            };

            var socialMedia = new SocialMedia
            {
                SocialMediaId = 1,
                SocialMediaType = socialMediaType,
                SocialMediaTypeId = SocialMediaType.Facebook.Id,
                SocialMediaValue = "socialMediaValue"
            };

            var person = new Person
            {
                PersonId = 1
            };

            person.SocialMedias.Add(socialMedia);

            context.SocialMediaTypes.Add(socialMediaType);
            context.SocialMedias.Add(socialMedia);
            context.People.Add(person);

            Action<ContactInfoDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(person.SocialMedias.Count(), serviceResult.SocialMedias.Count());
                CollectionAssert.AreEqual(person.SocialMedias.Select(x => x.SocialMediaId).ToList(), serviceResult.SocialMedias.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(person.SocialMedias.Select(x => x.SocialMediaType.SocialMediaTypeName).ToList(), serviceResult.SocialMedias.Select(x => x.Type).ToList());
                CollectionAssert.AreEqual(person.SocialMedias.Select(x => x.SocialMediaValue).ToList(), serviceResult.SocialMedias.Select(x => x.Value).ToList());
            };

            var result = service.GetContactInfoById(person.PersonId);
            var resultAsync = await service.GetContactInfoByIdAsync(person.PersonId);
        }

        [TestMethod]
        public async Task TestGetContactInfoById_PhoneNumbers()
        {
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = 1,
                PhoneNumberTypeName = PhoneNumberType.Home.Value
            };

            var phoneNumber = new PhoneNumber
            {
                PhoneNumberId = 1,
                PhoneNumberTypeId  = phoneNumberType.PhoneNumberTypeId,
                Number = "1234567890"
            };

            var person = new Person
            {
                PersonId = 1
            };

            person.PhoneNumbers.Add(phoneNumber);

            context.PhoneNumberTypes.Add(phoneNumberType);
            context.PhoneNumbers.Add(phoneNumber);
            context.People.Add(person);

            Action<ContactInfoDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(person.PhoneNumbers.Count(), serviceResult.PhoneNumbers.Count());
                CollectionAssert.AreEqual(person.PhoneNumbers.Select(x => x.PhoneNumberId).ToList(), serviceResult.PhoneNumbers.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(person.PhoneNumbers.Select(x => x.PhoneNumberType.PhoneNumberTypeName).ToList(), serviceResult.PhoneNumbers.Select(x => x.Type).ToList());
                CollectionAssert.AreEqual(person.PhoneNumbers.Select(x => x.Number).ToList(), serviceResult.PhoneNumbers.Select(x => x.Value).ToList());
            };

            var result = service.GetContactInfoById(person.PersonId);
            var resultAsync = await service.GetContactInfoByIdAsync(person.PersonId);
        }

        #endregion
    }
}
