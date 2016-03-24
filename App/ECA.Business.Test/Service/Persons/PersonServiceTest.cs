using ECA.Business.Exceptions;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service;
using ECA.Business.Service.Lookup;
using ECA.Business.Service.Persons;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class PersonServiceTest
    {
        private TestEcaContext context;
        private PersonService service;
        private Mock<IBusinessValidator<PersonServiceValidationEntity, PersonServiceValidationEntity>> validator;
        
        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            validator = new Mock<IBusinessValidator<PersonServiceValidationEntity, PersonServiceValidationEntity>>();
            service = new PersonService(context, validator.Object);
        }

        #region Get General By Id

        [TestMethod]
        public async Task TestGetGeneralById_CheckProperties()
        {
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "Male"
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "New"
            };
            var ptype = new ParticipantType
            {
                ParticipantTypeId = 1,
                Name = "Person"
            };
            var participantOrigination = new Organization
            {
                OrganizationId = 1,
                Name = "partOrg",
            };
            var prominentCat1 = new ProminentCategory
            {
                ProminentCategoryId = 1,
                Name = "Cat1"
            };
            var activity1 = new Activity
            {
                ActivityId = 1,
                Title = "Event1"
            };
            var membership1 = new Membership
            {
                MembershipId = 1,
                Name = "member1"
            };
            var language1 = new Language
            {
                LanguageId = 1,
                LanguageName = "lang1"
            };
            var languageProficiency1 = new PersonLanguageProficiency
            {
                LanguageId = 1,
                IsNativeLanguage = false,
                SpeakingProficiency = 5,
                ReadingProficiency = 5,
                ComprehensionProficiency = 5,
            };
            var impact1 = new Impact
            {
                ImpactId = 1,
                Description = "desc1"
            };
            var person = new Person
            {
                PersonId = 1,
                Gender = gender,
                DateOfBirth = DateTime.Now,
                IsDateOfBirthUnknown = false,
                FirstName = "firstName",
                LastName = "lastName",
                NamePrefix = "namePrefix",
                NameSuffix = "nameSuffix",
            };
            var participant = new Participant
            {
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId,
                ParticipantType = ptype,
                Person = person,
                ProjectId = 1
            };
            var commStatus = new SevisCommStatus
            {
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                SevisCommStatusName = SevisCommStatus.ReadyToSubmit.Value
            };
            var sevisCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                SevisCommStatusId = commStatus.SevisCommStatusId,
                SevisCommStatus = commStatus
            };
            List<ParticipantPersonSevisCommStatus> sevisCommStatuses = new List<ParticipantPersonSevisCommStatus>();
            sevisCommStatuses.Add(sevisCommStatus);
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisId = "N0000000001",
                ParticipantPersonSevisCommStatuses = sevisCommStatuses
            };
            sevisCommStatus.ParticipantPerson = participantPerson;
            participant.ParticipantPerson = participantPerson;
            context.SevisCommStatuses.Add(commStatus);
            context.ParticipantPersonSevisCommStatuses.Add(sevisCommStatus);
            context.ParticipantStatuses.Add(status);
            person.Participations.Add(participant);
            context.ParticipantTypes.Add(ptype);
            context.ParticipantStatuses.Add(status);
            person.Participations.Add(participant);
            context.Genders.Add(gender);
            context.Languages.Add(language1);
            languageProficiency1.Language = language1;
            context.PersonLanguageProficiencies.Add(languageProficiency1);
            context.ProminentCategories.Add(prominentCat1);
            context.Activities.Add(activity1);
            context.Memberships.Add(membership1);
            context.Impacts.Add(impact1);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            person.ProminentCategories.Add(prominentCat1);
            person.Activities.Add(activity1);
            person.Memberships.Add(membership1);
            person.LanguageProficiencies.Add(languageProficiency1);
            person.Impacts.Add(impact1);
            context.People.Add(person);
            
            Action<GeneralDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(person.ProminentCategories.FirstOrDefault().Name, serviceResult.ProminentCategories.FirstOrDefault().Value);
                Assert.AreEqual(person.Activities.FirstOrDefault().Title, serviceResult.Activities.FirstOrDefault().Value);
                Assert.AreEqual(person.Memberships.FirstOrDefault().Name, serviceResult.Memberships.FirstOrDefault().Name);
                Assert.AreEqual(person.LanguageProficiencies.FirstOrDefault().Language.LanguageName, serviceResult.LanguageProficiencies.FirstOrDefault().LanguageName);
                Assert.AreEqual(person.Impacts.FirstOrDefault().Description, serviceResult.ImpactStories.FirstOrDefault().Value);
                Assert.AreEqual(person.Participations.FirstOrDefault().ProjectId, serviceResult.ProjectId);
                Assert.AreEqual(participant.ParticipantPerson.SevisId, serviceResult.SevisId);
            };

            var result = this.service.GetGeneralById(person.PersonId);
            var resultAsync = await this.service.GetGeneralByIdAsync(person.PersonId);

            tester(result);
            tester(resultAsync);
        }
        
        #endregion

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
                DateOfBirth = DateTime.Now,
                IsDateOfBirthUnknown = true,
                IsDateOfBirthEstimated = true,
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
                MedicalConditions = "medical conditions",
                MaritalStatus = new MaritalStatus(),
                PlaceOfBirth = new Location()
            };
            var dependant1 = new Person
            {
                PersonId = 2,
                Gender = gender,
                FirstName = "firstName",
                LastName = "lastName",
                DateOfBirth = DateTime.Now,
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status"
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId,
                Person = person,
                ProjectId = 1,
            };
            var commStatus = new SevisCommStatus
            {
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
                SevisCommStatusName = SevisCommStatus.ReadyToSubmit.Value
            };
            var sevisCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                SevisCommStatusId = commStatus.SevisCommStatusId,
                SevisCommStatus = commStatus                                
            };
            List<ParticipantPersonSevisCommStatus> sevisCommStatuses = new List<ParticipantPersonSevisCommStatus>();
            sevisCommStatuses.Add(sevisCommStatus);
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                SevisId = "N0000000001",
                ParticipantPersonSevisCommStatuses = sevisCommStatuses
            };
            sevisCommStatus.ParticipantPerson = participantPerson;
            participant.ParticipantPerson = participantPerson;
            context.SevisCommStatuses.Add(commStatus);
            context.ParticipantPersonSevisCommStatuses.Add(sevisCommStatus);
            context.ParticipantStatuses.Add(status);
            person.Participations.Add(participant);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.People.Add(dependant1);
            person.Family.Add(dependant1);

            Action<PiiDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(gender.GenderName, serviceResult.Gender);
                DateTimeOffset.UtcNow.Should().BeCloseTo(serviceResult.DateOfBirth.Value, DbContextHelper.DATE_PRECISION);
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
                Assert.AreEqual(person.IsDateOfBirthEstimated, serviceResult.IsDateOfBirthEstimated);
                Assert.AreEqual(person.IsDateOfBirthUnknown, serviceResult.IsDateOfBirthUnknown);
                Assert.AreEqual(person.Family.FirstOrDefault().LastName + ", " + person.Family.FirstOrDefault().FirstName, serviceResult.Dependants.FirstOrDefault().Value);
                Assert.AreEqual(person.Participations.FirstOrDefault().ProjectId, serviceResult.ProjectId);
                Assert.AreEqual(participant.ParticipantPerson.SevisId, serviceResult.SevisId);
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
                MaritalStatus = new MaritalStatus(),
                PlaceOfBirth = new Location()
            };

            person.PlaceOfBirth.Country = new Location();

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

            var city = new Location
            {
                LocationId = 3,
                LocationName = "city",
                LocationTypeId = LocationType.City.Id,
                CountryId = country.LocationId
            };

            var division = new Location
            {
                LocationId = 10,
                LocationName = "division",
                LocationTypeId = LocationType.Division.Id,
                Country = country,
                CountryId = country.LocationId
            };

            var location = new Location
            {
                LocationId = 1,
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
                City = city,
                PostalCode = "postalCode",
                CountryId = 2,
                Country = country,
                LocationName = "location name",
                Division = division,
                DivisionId = division.LocationId
            };

            var addressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };

            var address = new Address
            {
                AddressId = 1,
                AddressTypeId = AddressType.Home.Id,
                Location = location,
                AddressType = addressType,
                IsPrimary = true
            };

            var person = new Person
            {
                PersonId = 1,
                Gender = new Gender(),
                MaritalStatus = new MaritalStatus(),
                PlaceOfBirth = new Location()
            };

            person.PlaceOfBirth.Country = new Location();

            person.Addresses.Add(address);
            context.AddressTypes.Add(addressType);
            context.Locations.Add(city);
            context.Locations.Add(country);
            context.Locations.Add(location);
            context.Addresses.Add(address);
            context.People.Add(person);
            Action<PiiDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                var testAddresses = serviceResult.Addresses;
                Assert.AreEqual(1, testAddresses.Count());
                var testAddress = testAddresses.FirstOrDefault();
                Assert.AreEqual(location.Street1, testAddress.Street1);
                Assert.AreEqual(location.Street2, testAddress.Street2);
                Assert.AreEqual(location.Street3, testAddress.Street3);
                Assert.AreEqual(location.LocationName, testAddress.LocationName);
                Assert.AreEqual(city.LocationName, testAddress.City);
                Assert.AreEqual(address.IsPrimary, testAddress.IsPrimary);
                Assert.AreEqual(addressType.AddressName, testAddress.AddressType);
                Assert.AreEqual(addressType.AddressTypeId, testAddress.AddressTypeId);
                Assert.AreEqual(location.PostalCode, testAddress.PostalCode);
                Assert.AreEqual(country.LocationName, testAddress.Country);
                Assert.AreEqual(division.LocationName, testAddress.Division);
                Assert.AreEqual(division.LocationId, testAddress.DivisionId);
                Assert.AreEqual(location.LocationId, testAddress.LocationId);
                Assert.AreEqual(address.AddressId, testAddress.AddressId);
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
                MaritalStatus = maritalStatus,
                PlaceOfBirth = new Location()
            };

            person.PlaceOfBirth.Country = new Location();

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
            var countryType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };
            var divisionType = new LocationType
            {
                LocationTypeId = LocationType.Division.Id,
                LocationTypeName = LocationType.Division.Value
            };
            var cityType = new LocationType
            {
                LocationTypeId = LocationType.City.Id,
                LocationTypeName= LocationType.City.Value
            };

            var country = new Location
            {
                LocationId = 1,
                LocationType = countryType,
                LocationTypeId = countryType.LocationTypeId,
                LocationName = "country"
            };
            var division = new Location
            {
                LocationId = 3,
                LocationType = divisionType,
                LocationTypeId = divisionType.LocationTypeId,
                LocationName = "division",
                Country = country,
                CountryId = country.LocationId,
            };
            var city = new Location
            {
                LocationId = 2,
                LocationType = cityType,
                LocationTypeId = cityType.LocationTypeId,
                Country = country,
                CountryId = country.CountryId,
                LocationName = "city",
                Division = division,
                DivisionId = division.LocationId
            };

            var person = new Person
            {
                PersonId = 1,
                Gender = new Gender(),
                PlaceOfBirth = city,
                PlaceOfBirthId = city.LocationId,
                MaritalStatus = new MaritalStatus()
            };

            context.Locations.Add(country);
            context.Locations.Add(city);
            context.Locations.Add(division);
            context.LocationTypes.Add(countryType);
            context.LocationTypes.Add(divisionType);
            context.LocationTypes.Add(cityType);
            context.People.Add(person);

            Action<PiiDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult.PlaceOfBirth);
                Assert.AreEqual(city.LocationId, serviceResult.PlaceOfBirth.Id);
            };

            var result = service.GetPiiById(person.PersonId);
            var resultAsync = await service.GetPiiByIdAsync(person.PersonId);

            tester(result);
            tester(resultAsync);
        }
        
        //[TestMethod]
        //public async Task TestDeletePersonDependentById_CheckDependentDeleted()
        //{
        //    var gender = new Gender
        //    {
        //        GenderId = 1,
        //        GenderName = "genderName"
        //    };
        //    var person = new Person
        //    {
        //        PersonId = 1,
        //        Gender = gender,
        //        DateOfBirth = DateTime.Now,
        //        IsDateOfBirthUnknown = true,
        //        IsDateOfBirthEstimated = true,
        //        FirstName = "firstName",
        //        LastName = "lastName",
        //        NamePrefix = "namePrefix",
        //        NameSuffix = "nameSuffix",
        //        GivenName = "givenName",
        //        FamilyName = "familyName",
        //        MiddleName = "middleName",
        //        Patronym = "patronym",
        //        Alias = "alias",
        //        Ethnicity = "ethnicity",
        //        MedicalConditions = "medical conditions",
        //        MaritalStatus = new MaritalStatus(),
        //        PlaceOfBirth = new Location()
        //    };
        //    var dependant1 = new Person
        //    {
        //        PersonId = 2,
        //        Gender = gender,
        //        FirstName = "firstName",
        //        LastName = "lastName",
        //        DateOfBirth = DateTime.Now,
        //    };
        //    var status = new ParticipantStatus
        //    {
        //        ParticipantStatusId = 1,
        //        Status = "status"
        //    };
        //    var participant = new Participant
        //    {
        //        ParticipantId = 1,
        //        Status = status,
        //        ParticipantStatusId = status.ParticipantStatusId,
        //        Person = person,
        //        ProjectId = 1,
        //    };
        //    var commStatus = new SevisCommStatus
        //    {
        //        SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id,
        //        SevisCommStatusName = SevisCommStatus.ReadyToSubmit.Value
        //    };
        //    var sevisCommStatus = new ParticipantPersonSevisCommStatus
        //    {
        //        ParticipantId = participant.ParticipantId,
        //        SevisCommStatusId = commStatus.SevisCommStatusId,
        //        SevisCommStatus = commStatus
        //    };
        //    List<ParticipantPersonSevisCommStatus> sevisCommStatuses = new List<ParticipantPersonSevisCommStatus>();
        //    sevisCommStatuses.Add(sevisCommStatus);
        //    var participantPerson = new ParticipantPerson
        //    {
        //        ParticipantId = participant.ParticipantId,
        //        Participant = participant,
        //        SevisId = "N0000000001",
        //        ParticipantPersonSevisCommStatuses = sevisCommStatuses
        //    };
        //    sevisCommStatus.ParticipantPerson = participantPerson;
        //    participant.ParticipantPerson = participantPerson;
        //    context.SevisCommStatuses.Add(commStatus);
        //    context.ParticipantPersonSevisCommStatuses.Add(sevisCommStatus);
        //    context.ParticipantStatuses.Add(status);
        //    person.Participations.Add(participant);
        //    context.Participants.Add(participant);
        //    context.ParticipantPersons.Add(participantPerson);
        //    context.Genders.Add(gender);
        //    context.People.Add(person);
        //    context.People.Add(dependant1);
        //    person.Family.Add(dependant1);
                
        //    await service.DeletePersonDependentByIdAsync(person.PersonId, dependant1.PersonId);
        //    await service.SaveChangesAsync();

        //    Action<PiiDTO> tester = (serviceResult) =>
        //    {
        //        Assert.IsTrue(serviceResult.Dependants.Count() == 0);
        //    };
            
        //    var result = this.service.GetPiiById(person.PersonId);
        //    var resultAsync = await this.service.GetPiiByIdAsync(person.PersonId);

        //    tester(result);
        //    tester(resultAsync);
        //}


        #endregion

        #region Get Contact Info By Id
        [TestMethod]
        public async Task TestGetContactInfoById_Emails()
        {
            var person = new Person
            {
                PersonId = 1
            };

            var emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = 1,
                EmailAddressTypeName = "Home"
            };
            var email = new EmailAddress
            {
                EmailAddressId = 1,
                Address = "test@test.com",
                EmailAddressTypeId = 1,
                Person = person,
                PersonId = person.PersonId,
                EmailAddressType = emailAddressType,
                IsPrimary = true
            
            };

            var email2 = new EmailAddress
            {
                EmailAddressId = 2,
                Address = "test1@test.com",
                EmailAddressTypeId = 1,
                EmailAddressType = emailAddressType,
                IsPrimary = false
            };

            person.EmailAddresses.Add(email);

            context.EmailAddressTypes.Add(emailAddressType);
            context.EmailAddresses.Add(email);
            context.EmailAddresses.Add(email2);
            context.People.Add(person);

            Action<ContactInfoDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(person.EmailAddresses.Count(), serviceResult.EmailAddresses.Count());
                CollectionAssert.AreEqual(person.EmailAddresses.Select(x => x.EmailAddressId).ToList(), serviceResult.EmailAddresses.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(person.EmailAddresses.Select(x => x.Address).ToList(), serviceResult.EmailAddresses.Select(x => x.Address).ToList());
                CollectionAssert.AreEqual(person.EmailAddresses.Select(x => x.EmailAddressTypeId).ToList(), serviceResult.EmailAddresses.Select(x => x.EmailAddressTypeId).ToList());

                var emailAddressPersonIds = serviceResult.EmailAddresses.Select(x => x.PersonId).Distinct().ToList();
                Assert.AreEqual(1, emailAddressPersonIds.Count);
                Assert.AreEqual(person.PersonId, emailAddressPersonIds.First());

                var contactIds = serviceResult.EmailAddresses.Select(x => x.ContactId).Distinct().ToList();
                Assert.AreEqual(1, contactIds.Count());
                Assert.IsNull(contactIds.First());
            };

            var result = service.GetContactInfoById(person.PersonId);
            var resultAsync = await service.GetContactInfoByIdAsync(person.PersonId);

            tester(result);
            tester(resultAsync);
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
                CollectionAssert.AreEqual(person.SocialMedias.Select(x => x.SocialMediaType.SocialMediaTypeName).ToList(), serviceResult.SocialMedias.Select(x => x.SocialMediaType).ToList());
                CollectionAssert.AreEqual(person.SocialMedias.Select(x => x.SocialMediaType.SocialMediaTypeId).ToList(), serviceResult.SocialMedias.Select(x => x.SocialMediaTypeId).ToList());
                CollectionAssert.AreEqual(person.SocialMedias.Select(x => x.SocialMediaValue).ToList(), serviceResult.SocialMedias.Select(x => x.Value).ToList());
            };

            var result = service.GetContactInfoById(person.PersonId);
            var resultAsync = await service.GetContactInfoByIdAsync(person.PersonId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetContactInfoById_PhoneNumbers()
        {
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = 1,
                PhoneNumberTypeName = PhoneNumberType.Home.Value
            };

            var person = new Person
            {
                PersonId = 1
            };

            var phoneNumber = new PhoneNumber
            {
                PhoneNumberId = 1,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId,
                PhoneNumberType = phoneNumberType,
                Number = "1234567890",
                Person = person,
                PersonId = person.PersonId
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
                CollectionAssert.AreEqual(person.PhoneNumbers.Select(x => x.PhoneNumberType.PhoneNumberTypeName).ToList(), serviceResult.PhoneNumbers.Select(x => x.PhoneNumberType).ToList());
                CollectionAssert.AreEqual(person.PhoneNumbers.Select(x => x.Number).ToList(), serviceResult.PhoneNumbers.Select(x => x.Number).ToList());

                var phoneNumberPersonIds = serviceResult.PhoneNumbers.Select(x => x.PersonId).Distinct().ToList();
                Assert.AreEqual(1, phoneNumberPersonIds.Count);
                Assert.AreEqual(person.PersonId, phoneNumberPersonIds.First());
                Assert.AreEqual(0, serviceResult.EmailAddresses.Select(x => x.ContactId).Distinct().Count());
            };

            var result = service.GetContactInfoById(person.PersonId);
            var resultAsync = await service.GetContactInfoByIdAsync(person.PersonId);

            tester(result);
            tester(resultAsync);
        }

        #endregion

        #region Get Evaluation Notes By Id
        // ***** moved to EvaluationServiceTest.cs

        //[TestMethod]
        //public async Task TestGetEvaluationNotesById()
        //{
        //    int principalId = 1;
        //    int personId = 2;
        //    DateTime date1 = new DateTime(2015, 6, 11);
        //    DateTime date2 = new DateTime(2015, 6, 2);
        //    var user = new UserAccount
        //    {
        //        PrincipalId = principalId,
        //        FirstName = "Jack",
        //        LastName = "Diddly",
        //        DisplayName = "Jack Diddly",
        //        EmailAddress = "jack@diddly.us"
        //    };

        //    var history1 = new History
        //    {
        //        CreatedBy = principalId,
        //        CreatedOn = new DateTimeOffset(date1),
        //        RevisedBy = principalId,
        //        RevisedOn = new DateTimeOffset(date1)
        //    };

        //    var history2 = new History
        //    {
        //        CreatedBy = principalId,
        //        CreatedOn = new DateTimeOffset(date2),
        //        RevisedBy = principalId,
        //        RevisedOn = new DateTimeOffset(date2)
        //    };

        //    var person = new Person
        //    {
        //        PersonId = personId
        //    };

        //    var eval1 = new PersonEvaluationNote
        //    {
        //        EvaluationNoteId = 1,
        //        PersonId = personId,
        //        EvaluationNote = "Jack is awesome.",
        //        History = history1
        //    };

        //    var eval2 = new PersonEvaluationNote
        //    {
        //        EvaluationNoteId = 2,
        //        PersonId = personId,
        //        EvaluationNote = "Jack is really cool.",
        //        History = history2
        //    };

        //    person.EvaluationNotes.Add(eval1);
        //    person.EvaluationNotes.Add(eval2);

        //    context.UserAccounts.Add(user);
        //    context.PersonEvaluationNotes.Add(eval1);
        //    context.PersonEvaluationNotes.Add(eval2);
        //    context.People.Add(person);

        //    Action<IList<EvaluationNoteDTO>> tester = (serviceResult) =>
        //    {
        //        Assert.IsNotNull(serviceResult);
        //        Assert.AreEqual(person.EvaluationNotes.Count(), serviceResult.Count());
        //        CollectionAssert.AreEqual(person.EvaluationNotes.Select(x => x.EvaluationNote).ToList(), serviceResult.Select(x => x.EvaluationNote).ToList());
        //        CollectionAssert.AreEqual(person.EvaluationNotes.Select(x => x.History.CreatedBy).ToList(), serviceResult.Select(x => x.UserId).ToList());
        //    };

        //    var result = service.GetEvaluationNotesByPersonId(personId);
        //    var resultAsync = await service.GetEvaluationNotesByPersonIdAsync(personId);

        //    tester(result);
        //    tester(resultAsync);
        //}
        #endregion

        #region Get Educations By Person Id
        [TestMethod]
        public async Task TestGetEducationsByPersonId_CheckProperties()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var today = DateTimeOffset.UtcNow;
            var person = new Person
            {
                PersonId = 1,
            };
            //var city = new Location
            //{
            //    LocationId = 1,
            //    LocationName = "city",
            //};
            //var country = new Location
            //{
            //    LocationId = 2,
            //    LocationName = "country"
            //};
            //var addressLocation = new Location
            //{
            //    LocationId = 3,
            //    Country = country,
            //    CountryId = country.LocationId,
            //    City = city,
            //    CityId = city.LocationId,
            //};
            //var address = new Address
            //{
            //    AddressId = 1,
            //    IsPrimary = true,
            //    Location = addressLocation,
            //    LocationId = addressLocation.LocationId
            //};
            //var organizationType = new OrganizationType
            //{
            //    OrganizationTypeId = 1,
            //    OrganizationTypeName = "type"
            //};
            //var organization = new Organization
            //{
            //    OrganizationId = 1,
            //    Name = "name",
            //    Status = "status",
            //    OrganizationTypeId = organizationType.OrganizationTypeId,
            //    OrganizationType = organizationType
            //};
            //organization.Addresses.Add(address);
            //address.Organization = organization;
            //address.OrganizationId = organization.OrganizationId;
            var education = new ProfessionEducation
            {
                PersonOfEducation_PersonId = person.PersonId,
                ProfessionEducationId = 1,
                Title = "title",
                Role = "role",
                DateFrom = yesterday,
                DateTo = today
                //OrganizationId = organization.OrganizationId,
                //Organization = organization
            };

            context.SetupActions.Add(() =>
            {
                context.People.Add(person);
                //context.Locations.Add(city);
                //context.Locations.Add(country);
                //context.Locations.Add(addressLocation);
                //context.Addresses.Add(address);
                //context.Organizations.Add(organization);
                //context.OrganizationTypes.Add(organizationType);
                context.ProfessionEducations.Add(education);
            });
            context.Revert();
            Action<List<EducationEmploymentDTO>> tester = (list) =>
            {
                Assert.AreEqual(1, list.Count);
                var dto = list.First();
                Assert.AreEqual(education.ProfessionEducationId, dto.ProfessionEducationId);
                Assert.AreEqual(education.Title, dto.Title);
                Assert.AreEqual(education.Role, dto.Role);
                Assert.AreEqual(education.DateFrom, dto.StartDate);
                Assert.AreEqual(education.DateTo, dto.EndDate);
                //Assert.AreEqual(education.Organization, dto.Organization);
                //Assert.AreEqual(organization.OrganizationId, dto.Organization.OrganizationId);
                //Assert.AreEqual(organizationType.OrganizationTypeName, dto.Organization.OrganizationType);
                //Assert.AreEqual(organization.Status, dto.Organization.Status);
                //Assert.AreEqual(organization.Name, dto.Organization.Name);
            };
            var results = service.GetEducationsByPersonId(person.PersonId);
            var resultsAsync = await service.GetEducationsByPersonIdAsync(person.PersonId);
            tester(results.ToList());
            tester(resultsAsync.ToList());
        }

        //[TestMethod]
        //public async Task TestGetEducationsByPersonId_CheckUsesPrimaryAddress()
        //{
        //    var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
        //    var today = DateTimeOffset.UtcNow;
        //    var person = new Person
        //    {
        //        PersonId = 1,
        //    };
        //    var city = new Location
        //    {
        //        LocationId = 1,
        //        LocationName = "city",
        //    };
        //    var country = new Location
        //    {
        //        LocationId = 2,
        //        LocationName = "country"
        //    };
        //    var addressLocation = new Location
        //    {
        //        LocationId = 3,
        //        Country = country,
        //        CountryId = country.LocationId,
        //        City = city,
        //        CityId = city.LocationId,
        //    };
        //    var primaryAddress = new Address
        //    {
        //        AddressId = 1,
        //        IsPrimary = true,
        //        Location = addressLocation,
        //        LocationId = addressLocation.LocationId
        //    };
        //    var otherAddress = new Address
        //    {
        //        AddressId = 2,
        //        IsPrimary = false,
        //    };
        //    var organizationType = new OrganizationType
        //    {
        //        OrganizationTypeId = 1,
        //        OrganizationTypeName = "type"
        //    };
        //    var organization = new Organization
        //    {
        //        OrganizationId = 1,
        //        Name = "name",
        //        Status = "status",
        //        OrganizationTypeId = organizationType.OrganizationTypeId,
        //        OrganizationType = organizationType
        //    };
        //    organization.Addresses.Add(primaryAddress);
        //    primaryAddress.Organization = organization;
        //    primaryAddress.OrganizationId = organization.OrganizationId;

        //    organization.Addresses.Add(otherAddress);
        //    otherAddress.Organization = organization;
        //    otherAddress.OrganizationId = organization.OrganizationId;
        //    var education = new ProfessionEducation
        //    {
        //        PersonOfEducation_PersonId = person.PersonId,
        //        ProfessionEducationId = 1,
        //        Title = "title",
        //        Role = "role",
        //        DateFrom = yesterday,
        //        DateTo = today,
        //        OrganizationId = organization.OrganizationId,
        //        Organization = organization
        //    };

        //    context.SetupActions.Add(() =>
        //    {
        //        context.People.Add(person);
        //        context.Locations.Add(city);
        //        context.Locations.Add(country);
        //        context.Locations.Add(addressLocation);
        //        context.Addresses.Add(primaryAddress);
        //        context.Addresses.Add(otherAddress);
        //        context.Organizations.Add(organization);
        //        context.OrganizationTypes.Add(organizationType);
        //        context.ProfessionEducations.Add(education);
        //    });
        //    context.Revert();
        //    Action<List<EducationEmploymentDTO>> tester = (list) =>
        //    {
        //        Assert.AreEqual(1, list.Count);
        //        var dto = list.First();
        //        Assert.AreEqual(education.Organization, dto.Organization);
        //    };
        //    var results = service.GetEducationsByPersonId(person.PersonId);
        //    var resultsAsync = await service.GetEducationsByPersonIdAsync(person.PersonId);
        //    tester(results.ToList());
        //    tester(resultsAsync.ToList());
        //}

        //[TestMethod]
        //public async Task TestGetEducationsByPersonId_AddressDoesNotHaveLocation()
        //{
        //    var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
        //    var today = DateTimeOffset.UtcNow;
        //    var person = new Person
        //    {
        //        PersonId = 1,
        //    };
        //    var address = new Address
        //    {
        //        AddressId = 1,
        //        IsPrimary = true,
        //    };
        //    var organizationType = new OrganizationType
        //    {
        //        OrganizationTypeId = 1,
        //        OrganizationTypeName = "type"
        //    };
        //    var organization = new Organization
        //    {
        //        OrganizationId = 1,
        //        Name = "name",
        //        Status = "status",
        //        OrganizationTypeId = organizationType.OrganizationTypeId,
        //        OrganizationType = organizationType
        //    };
        //    organization.Addresses.Add(address);
        //    address.Organization = organization;
        //    address.OrganizationId = organization.OrganizationId;
        //    var education = new ProfessionEducation
        //    {
        //        PersonOfEducation_PersonId = person.PersonId,
        //        ProfessionEducationId = 1,
        //        Title = "title",
        //        Role = "role",
        //        DateFrom = yesterday,
        //        DateTo = today,
        //        OrganizationId = organization.OrganizationId,
        //        Organization = organization
        //    };

        //    context.SetupActions.Add(() =>
        //    {
        //        context.People.Add(person);
        //        context.Addresses.Add(address);
        //        context.Organizations.Add(organization);
        //        context.OrganizationTypes.Add(organizationType);
        //        context.ProfessionEducations.Add(education);
        //    });
        //    context.Revert();
        //    Action<List<EducationEmploymentDTO>> tester = (list) =>
        //    {
        //        Assert.AreEqual(1, list.Count);
        //        var dto = list.First();
        //        Assert.IsNull(dto.Organization.Location);
        //    };
        //    var results = service.GetEducationsByPersonId(person.PersonId);
        //    var resultsAsync = await service.GetEducationsByPersonIdAsync(person.PersonId);
        //    tester(results.ToList());
        //    tester(resultsAsync.ToList());
        //}

        //[TestMethod]
        //public async Task TestGetEducationsByPersonId_AddressDoesNotHaveCity()
        //{
        //    var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
        //    var today = DateTimeOffset.UtcNow;
        //    var person = new Person
        //    {
        //        PersonId = 1,
        //    };
        //    var country = new Location
        //    {
        //        LocationId = 2,
        //        LocationName = "country"
        //    };
        //    var addressLocation = new Location
        //    {
        //        LocationId = 3,
        //        Country = country,
        //        CountryId = country.LocationId
        //    };
        //    var address = new Address
        //    {
        //        AddressId = 1,
        //        IsPrimary = true,
        //        Location = addressLocation,
        //        LocationId = addressLocation.LocationId
        //    };
        //    var organizationType = new OrganizationType
        //    {
        //        OrganizationTypeId = 1,
        //        OrganizationTypeName = "type"
        //    };
        //    var organization = new Organization
        //    {
        //        OrganizationId = 1,
        //        Name = "name",
        //        Status = "status",
        //        OrganizationTypeId = organizationType.OrganizationTypeId,
        //        OrganizationType = organizationType
        //    };
        //    organization.Addresses.Add(address);
        //    address.Organization = organization;
        //    address.OrganizationId = organization.OrganizationId;
        //    var education = new ProfessionEducation
        //    {
        //        PersonOfEducation_PersonId = person.PersonId,
        //        ProfessionEducationId = 1,
        //        Title = "title",
        //        Role = "role",
        //        DateFrom = yesterday,
        //        DateTo = today,
        //        OrganizationId = organization.OrganizationId,
        //        Organization = organization
        //    };

        //    context.SetupActions.Add(() =>
        //    {
        //        context.People.Add(person);
        //        context.Locations.Add(country);
        //        context.Locations.Add(addressLocation);
        //        context.Addresses.Add(address);
        //        context.Organizations.Add(organization);
        //        context.OrganizationTypes.Add(organizationType);
        //        context.ProfessionEducations.Add(education);
        //    });
        //    context.Revert();
        //    Action<List<EducationEmploymentDTO>> tester = (list) =>
        //    {
        //        Assert.AreEqual(1, list.Count);
        //        var dto = list.First();
        //        Assert.IsNotNull(dto.Organization.Location);
        //    };
        //    var results = service.GetEducationsByPersonId(person.PersonId);
        //    var resultsAsync = await service.GetEducationsByPersonIdAsync(person.PersonId);
        //    tester(results.ToList());
        //    tester(resultsAsync.ToList());
        //}

        //[TestMethod]
        //public async Task TestGetEducationsByPersonId_AddressDoesNotHaveCountry()
        //{
        //    var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
        //    var today = DateTimeOffset.UtcNow;
        //    var person = new Person
        //    {
        //        PersonId = 1,
        //    };
        //    var city = new Location
        //    {
        //        LocationId = 1,
        //        LocationName = "city",
        //    };
        //    var addressLocation = new Location
        //    {
        //        LocationId = 3,
        //        City = city,
        //        CityId = city.LocationId,
        //    };
        //    var address = new Address
        //    {
        //        AddressId = 1,
        //        IsPrimary = true,
        //        Location = addressLocation,
        //        LocationId = addressLocation.LocationId
        //    };
        //    var organizationType = new OrganizationType
        //    {
        //        OrganizationTypeId = 1,
        //        OrganizationTypeName = "type"
        //    };
        //    var organization = new Organization
        //    {
        //        OrganizationId = 1,
        //        Name = "name",
        //        Status = "status",
        //        OrganizationTypeId = organizationType.OrganizationTypeId,
        //        OrganizationType = organizationType
        //    };
        //    organization.Addresses.Add(address);
        //    address.Organization = organization;
        //    address.OrganizationId = organization.OrganizationId;
        //    var education = new ProfessionEducation
        //    {
        //        PersonOfEducation_PersonId = person.PersonId,
        //        ProfessionEducationId = 1,
        //        Title = "title",
        //        Role = "role",
        //        DateFrom = yesterday,
        //        DateTo = today,
        //        OrganizationId = organization.OrganizationId,
        //        Organization = organization
        //    };

        //    context.SetupActions.Add(() =>
        //    {
        //        context.People.Add(person);
        //        context.Locations.Add(city);
        //        context.Locations.Add(addressLocation);
        //        context.Addresses.Add(address);
        //        context.Organizations.Add(organization);
        //        context.OrganizationTypes.Add(organizationType);
        //        context.ProfessionEducations.Add(education);
        //    });
        //    context.Revert();
        //    Action<List<EducationEmploymentDTO>> tester = (list) =>
        //    {
        //        Assert.AreEqual(1, list.Count);
        //        var dto = list.First();
        //        Assert.IsNotNull(dto.Organization.Location);
        //    };
        //    var results = service.GetEducationsByPersonId(person.PersonId);
        //    var resultsAsync = await service.GetEducationsByPersonIdAsync(person.PersonId);
        //    tester(results.ToList());
        //    tester(resultsAsync.ToList());
        //}

        //[TestMethod]
        //public async Task TestGetEducationsByPersonId_DoesNotHaveOrganization()
        //{
        //    var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
        //    var today = DateTimeOffset.UtcNow;
        //    var person = new Person
        //    {
        //        PersonId = 1,
        //    };
        //    var education = new ProfessionEducation
        //    {
        //        PersonOfEducation_PersonId = person.PersonId,
        //        ProfessionEducationId = 1,
        //        Title = "title",
        //        Role = "role",
        //        DateFrom = yesterday,
        //        DateTo = today,
        //    };

        //    context.SetupActions.Add(() =>
        //    {
        //        context.People.Add(person);
        //        context.ProfessionEducations.Add(education);
        //    });
        //    context.Revert();
        //    Action<List<EducationEmploymentDTO>> tester = (list) =>
        //    {
        //        Assert.AreEqual(1, list.Count);
        //    };
        //    var results = service.GetEducationsByPersonId(person.PersonId);
        //    var resultsAsync = await service.GetEducationsByPersonIdAsync(person.PersonId);
        //    tester(results.ToList());
        //    tester(resultsAsync.ToList());
        //}

        [TestMethod]
        public async Task TestGetEducationsByPersonId_PersonDoesNotExist()
        {
            Action<List<EducationEmploymentDTO>> tester = (list) =>
            {
                Assert.AreEqual(0, list.Count);
            };
            var results = service.GetEducationsByPersonId(1);
            var resultsAsync = await service.GetEducationsByPersonIdAsync(1);
            tester(results.ToList());
            tester(resultsAsync.ToList());
        }
        #endregion

        #region Get Employments By Person Id
        [TestMethod]
        public async Task TestGetEmploymentsByPersonId_CheckProperties()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var today = DateTimeOffset.UtcNow;
            var person = new Person
            {
                PersonId = 1,
            };
            //var city = new Location
            //{
            //    LocationId = 1,
            //    LocationName = "city",
            //};
            //var country = new Location
            //{
            //    LocationId = 2,
            //    LocationName = "country"
            //};
            //var addressLocation = new Location
            //{
            //    LocationId = 3,
            //    Country = country,
            //    CountryId = country.LocationId,
            //    City = city,
            //    CityId = city.LocationId,
            //};
            //var address = new Address
            //{
            //    AddressId = 1,
            //    IsPrimary = true,
            //    Location = addressLocation,
            //    LocationId = addressLocation.LocationId
            //};
            //var organizationType = new OrganizationType
            //{
            //    OrganizationTypeId = 1,
            //    OrganizationTypeName = "type"
            //};
            //var organization = new Organization
            //{
            //    OrganizationId = 1,
            //    Name = "name",
            //    Status = "status",
            //    OrganizationTypeId = organizationType.OrganizationTypeId,
            //    OrganizationType = organizationType
            //};
            //organization.Addresses.Add(address);
            //address.Organization = organization;
            //address.OrganizationId = organization.OrganizationId;
            var employment = new ProfessionEducation
            {
                PersonOfProfession_PersonId = person.PersonId,
                ProfessionEducationId = 1,
                Title = "title",
                Role = "role",
                DateFrom = yesterday,
                DateTo = today
                //OrganizationId = organization.OrganizationId,
                //Organization = organization
            };

            context.SetupActions.Add(() =>
            {
                context.People.Add(person);
                //context.Locations.Add(city);
                //context.Locations.Add(country);
                //context.Locations.Add(addressLocation);
                //context.Addresses.Add(address);
                //context.Organizations.Add(organization);
                //context.OrganizationTypes.Add(organizationType);
                context.ProfessionEducations.Add(employment);
            });
            context.Revert();
            Action<List<EducationEmploymentDTO>> tester = (list) =>
            {
                Assert.AreEqual(1, list.Count);
                var dto = list.First();
                Assert.AreEqual(employment.ProfessionEducationId, dto.ProfessionEducationId);
                Assert.AreEqual(employment.Title, dto.Title);
                Assert.AreEqual(employment.Role, dto.Role);
                Assert.AreEqual(employment.DateFrom, dto.StartDate);
                Assert.AreEqual(employment.DateTo, dto.EndDate);
            };
            var results = service.GetEmploymentsByPersonId(person.PersonId);
            var resultsAsync = await service.GetEmploymentsByPersonIdAsync(person.PersonId);
            tester(results.ToList());
            tester(resultsAsync.ToList());
        }

        [TestMethod]
        public async Task TestGetEmploymentsByPersonId_PersonDoesNotExist()
        {
            Action<List<EducationEmploymentDTO>> tester = (list) =>
            {
                Assert.AreEqual(0, list.Count);
            };
            var results = service.GetEmploymentsByPersonId(1);
            var resultsAsync = await service.GetEmploymentsByPersonIdAsync(1);
            tester(results.ToList());
            tester(resultsAsync.ToList());
        }
        #endregion

        #region Create
        [TestMethod]
        public async Task TestCreateAsync_CheckProperties()
        {
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = 5;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int>();


            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.NotApplicable.Id
            };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: project.ProjectId,
                participantTypeId: participantTypeId,
                firstName: firstName,
                lastName: lastName,
                gender: gender,
                dateOfBirth: dateOfBirth,
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);

            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
            });
            Action<Person> tester = (testPerson) =>
            {
                Assert.AreEqual(newPerson.FirstName, testPerson.FirstName);
                Assert.AreEqual(newPerson.LastName, testPerson.LastName);
                Assert.AreEqual(newPerson.Gender, testPerson.GenderId);
                Assert.AreEqual(newPerson.DateOfBirth, testPerson.DateOfBirth);
                Assert.AreEqual(newPerson.IsDateOfBirthEstimated, testPerson.IsDateOfBirthEstimated);
                Assert.AreEqual(newPerson.IsDateOfBirthUnknown, testPerson.IsDateOfBirthUnknown);
                Assert.AreEqual(newPerson.IsPlaceOfBirthUnknown, testPerson.IsPlaceOfBirthUnknown);
                Assert.AreEqual(personTypeId, testPerson.PersonTypeId);
                Assert.AreEqual(user.Id, testPerson.History.CreatedBy);
                Assert.AreEqual(user.Id, testPerson.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(testPerson.History.CreatedOn, 20000);
                DateTimeOffset.UtcNow.Should().BeCloseTo(testPerson.History.RevisedOn, 20000);

                Assert.AreEqual(1, context.Participants.Count());
                Assert.AreEqual(1, context.ParticipantPersons.Count());
                Assert.AreEqual(0, context.ParticipantExchangeVisitors.Count());
                Assert.AreEqual(1, testPerson.Participations.Count);

                var participant = testPerson.Participations.First();
                Assert.IsTrue(Object.ReferenceEquals(participant, context.Participants.First()));
                Assert.IsNotNull(participant.ParticipantPerson);
                Assert.IsTrue(Object.ReferenceEquals(participant.ParticipantPerson, context.ParticipantPersons.First()));

                Assert.IsNull(participant.ParticipantExchangeVisitor);
            };
            context.Revert();
            var person = await service.CreateAsync(newPerson);
            tester(person);
            validator.Verify(x => x.ValidateCreate(It.IsAny<PersonServiceValidationEntity>()), Times.Once());
        }

        [TestMethod]
        public async Task TestCreateAsync_ExchangeVisitor()
        {
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = 5;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int>();


            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: project.ProjectId,
                participantTypeId: participantTypeId,
                firstName: firstName,
                lastName: lastName,
                gender: gender,
                dateOfBirth: dateOfBirth,
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);

            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
            });
            Action<Person> tester = (testPerson) =>
            {
                Assert.AreEqual(newPerson.FirstName, testPerson.FirstName);
                Assert.AreEqual(newPerson.LastName, testPerson.LastName);
                Assert.AreEqual(newPerson.Gender, testPerson.GenderId);
                Assert.AreEqual(newPerson.DateOfBirth, testPerson.DateOfBirth);
                Assert.AreEqual(newPerson.IsDateOfBirthEstimated, testPerson.IsDateOfBirthEstimated);
                Assert.AreEqual(newPerson.IsDateOfBirthUnknown, testPerson.IsDateOfBirthUnknown);
                Assert.AreEqual(newPerson.IsPlaceOfBirthUnknown, testPerson.IsPlaceOfBirthUnknown);
                Assert.AreEqual(personTypeId, testPerson.PersonTypeId);
                Assert.AreEqual(user.Id, testPerson.History.CreatedBy);
                Assert.AreEqual(user.Id, testPerson.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(testPerson.History.CreatedOn, 20000);
                DateTimeOffset.UtcNow.Should().BeCloseTo(testPerson.History.RevisedOn, 20000);

                Assert.AreEqual(1, context.Participants.Count());
                Assert.AreEqual(1, context.ParticipantPersons.Count());
                Assert.AreEqual(1, context.ParticipantExchangeVisitors.Count());
                Assert.AreEqual(1, testPerson.Participations.Count);

                var participant = testPerson.Participations.First();
                Assert.IsTrue(Object.ReferenceEquals(participant, context.Participants.First()));
                Assert.IsNotNull(participant.ParticipantPerson);
                Assert.IsTrue(Object.ReferenceEquals(participant.ParticipantPerson, context.ParticipantPersons.First()));

                Assert.IsNotNull(participant.ParticipantExchangeVisitor);
                Assert.IsTrue(Object.ReferenceEquals(participant.ParticipantExchangeVisitor, context.ParticipantExchangeVisitors.First()));
            };
            context.Revert();
            var person = await service.CreateAsync(newPerson);
            tester(person);
            validator.Verify(x => x.ValidateCreate(It.IsAny<PersonServiceValidationEntity>()), Times.Once());
        }

        [TestMethod]
        public async Task TestCreateAsync_ExchangeVisitorWithDefaults()
        {
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = 5;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int>();


            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };

            var defaultExchangeVisitorFunding = new DefaultExchangeVisitorFunding
            {
                ProjectId = project.ProjectId,
                FundingSponsor = 1,
                FundingPersonal = 2,
                FundingVisGovt = 3,
                FundingVisBNC = 4,
                FundingGovtAgency1 = 5,
                GovtAgency1Id = 1,
                GovtAgency1OtherName = "govt agency 1 other name",
                FundingGovtAgency2 = 6,
                GovtAgency2Id = 2,
                GovtAgency2OtherName = "govt agency 2 other name",
                FundingIntlOrg1 = 7,
                IntlOrg1Id = 3,
                IntlOrg1OtherName = "intl org 1 other name",
                FundingIntlOrg2 = 8,
                IntlOrg2Id = 4,
                IntlOrg2OtherName = "intl org 2 other name",
                FundingOther = 9,
                OtherName = "other name",
                FundingTotal = 55
        };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: project.ProjectId,
                participantTypeId: participantTypeId,
                firstName: firstName,
                lastName: lastName,
                gender: gender,
                dateOfBirth: dateOfBirth,
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);

            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
                context.DefaultExchangeVisitorFunding.Add(defaultExchangeVisitorFunding);
            });
            Action<Person> tester = (testPerson) =>
            {
                Assert.AreEqual(newPerson.FirstName, testPerson.FirstName);
                Assert.AreEqual(newPerson.LastName, testPerson.LastName);
                Assert.AreEqual(newPerson.Gender, testPerson.GenderId);
                Assert.AreEqual(newPerson.DateOfBirth, testPerson.DateOfBirth);
                Assert.AreEqual(newPerson.IsDateOfBirthEstimated, testPerson.IsDateOfBirthEstimated);
                Assert.AreEqual(newPerson.IsDateOfBirthUnknown, testPerson.IsDateOfBirthUnknown);
                Assert.AreEqual(newPerson.IsPlaceOfBirthUnknown, testPerson.IsPlaceOfBirthUnknown);
                Assert.AreEqual(personTypeId, testPerson.PersonTypeId);
                Assert.AreEqual(user.Id, testPerson.History.CreatedBy);
                Assert.AreEqual(user.Id, testPerson.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(testPerson.History.CreatedOn, 20000);
                DateTimeOffset.UtcNow.Should().BeCloseTo(testPerson.History.RevisedOn, 20000);

                Assert.AreEqual(1, context.Participants.Count());
                Assert.AreEqual(1, context.ParticipantPersons.Count());
                Assert.AreEqual(1, context.ParticipantExchangeVisitors.Count());
                Assert.AreEqual(1, testPerson.Participations.Count);

                var participant = testPerson.Participations.First();
                Assert.IsTrue(Object.ReferenceEquals(participant, context.Participants.First()));
                Assert.IsNotNull(participant.ParticipantPerson);
                Assert.IsTrue(Object.ReferenceEquals(participant.ParticipantPerson, context.ParticipantPersons.First()));

                Assert.IsNotNull(participant.ParticipantExchangeVisitor);
                Assert.IsTrue(Object.ReferenceEquals(participant.ParticipantExchangeVisitor, context.ParticipantExchangeVisitors.First()));

                Assert.AreEqual(defaultExchangeVisitorFunding.FundingSponsor, participant.ParticipantExchangeVisitor.FundingSponsor);
                Assert.AreEqual(defaultExchangeVisitorFunding.FundingPersonal, participant.ParticipantExchangeVisitor.FundingPersonal);
                Assert.AreEqual(defaultExchangeVisitorFunding.FundingVisGovt, participant.ParticipantExchangeVisitor.FundingVisGovt);
                Assert.AreEqual(defaultExchangeVisitorFunding.FundingVisBNC, participant.ParticipantExchangeVisitor.FundingVisBNC);
                Assert.AreEqual(defaultExchangeVisitorFunding.FundingGovtAgency1, participant.ParticipantExchangeVisitor.FundingGovtAgency1);
                Assert.AreEqual(defaultExchangeVisitorFunding.FundingGovtAgency2, participant.ParticipantExchangeVisitor.FundingGovtAgency2);
                Assert.AreEqual(defaultExchangeVisitorFunding.FundingIntlOrg1, participant.ParticipantExchangeVisitor.FundingIntlOrg1);
                Assert.AreEqual(defaultExchangeVisitorFunding.FundingIntlOrg2, participant.ParticipantExchangeVisitor.FundingIntlOrg2);
                Assert.AreEqual(defaultExchangeVisitorFunding.FundingOther, participant.ParticipantExchangeVisitor.FundingOther);
                Assert.AreEqual(defaultExchangeVisitorFunding.FundingTotal, participant.ParticipantExchangeVisitor.FundingTotal);

                Assert.AreEqual(defaultExchangeVisitorFunding.GovtAgency1Id, participant.ParticipantExchangeVisitor.GovtAgency1Id);
                Assert.AreEqual(defaultExchangeVisitorFunding.GovtAgency1OtherName, participant.ParticipantExchangeVisitor.GovtAgency1OtherName);
                Assert.AreEqual(defaultExchangeVisitorFunding.GovtAgency2Id, participant.ParticipantExchangeVisitor.GovtAgency2Id);
                Assert.AreEqual(defaultExchangeVisitorFunding.GovtAgency2OtherName, participant.ParticipantExchangeVisitor.GovtAgency2OtherName);
                Assert.AreEqual(defaultExchangeVisitorFunding.IntlOrg1Id, participant.ParticipantExchangeVisitor.IntlOrg1Id);
                Assert.AreEqual(defaultExchangeVisitorFunding.IntlOrg1OtherName, participant.ParticipantExchangeVisitor.IntlOrg1OtherName);
                Assert.AreEqual(defaultExchangeVisitorFunding.IntlOrg2Id, participant.ParticipantExchangeVisitor.IntlOrg2Id);
                Assert.AreEqual(defaultExchangeVisitorFunding.IntlOrg2OtherName, participant.ParticipantExchangeVisitor.IntlOrg2OtherName);
            };
            context.Revert();
            var person = await service.CreateAsync(newPerson);
            tester(person);
            validator.Verify(x => x.ValidateCreate(It.IsAny<PersonServiceValidationEntity>()), Times.Once());
        }

        [TestMethod]
        public async Task TestCreateAsync_DateOfBirthEstimated()
        {
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = false;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = 5;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int>();

            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.NotApplicable.Id
            };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: project.ProjectId,
                participantTypeId: participantTypeId,
                firstName: firstName,
                lastName: lastName,
                gender: gender,
                dateOfBirth: dateOfBirth,
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);

            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
            });
            Action<Person> tester = (testPerson) =>
            {
                Assert.AreEqual(newPerson.IsDateOfBirthEstimated, testPerson.IsDateOfBirthEstimated);
                Assert.AreEqual(newPerson.IsDateOfBirthUnknown, testPerson.IsDateOfBirthUnknown);
                Assert.AreEqual(newPerson.IsPlaceOfBirthUnknown, testPerson.IsPlaceOfBirthUnknown);
            };
            context.Revert();
            var person = await service.CreateAsync(newPerson);
            tester(person);
        }

        [TestMethod]
        public async Task TestCreateAsync_DateOfBirthUnknown()
        {
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = false;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = 5;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int>();

            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.NotApplicable.Id
            };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: project.ProjectId,
                participantTypeId: participantTypeId,
                firstName: firstName,
                lastName: lastName,
                gender: gender,
                dateOfBirth: dateOfBirth,
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);

            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
            });
            Action<Person> tester = (testPerson) =>
            {
                Assert.AreEqual(newPerson.IsDateOfBirthEstimated, testPerson.IsDateOfBirthEstimated);
                Assert.AreEqual(newPerson.IsDateOfBirthUnknown, testPerson.IsDateOfBirthUnknown);
                Assert.AreEqual(newPerson.IsPlaceOfBirthUnknown, testPerson.IsPlaceOfBirthUnknown);
            };
            context.Revert();
            var person = await service.CreateAsync(newPerson);
            tester(person);
        }

        [TestMethod]
        public async Task TestCreateAsync_PlaceOfBirthUnknown()
        {
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = 5;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int>();

            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.NotApplicable.Id
            };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: project.ProjectId,
                participantTypeId: participantTypeId,
                firstName: firstName,
                lastName: lastName,
                gender: gender,
                dateOfBirth: dateOfBirth,
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);

            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
            });
            Action<Person> tester = (testPerson) =>
            {
                Assert.AreEqual(newPerson.IsDateOfBirthEstimated, testPerson.IsDateOfBirthEstimated);
                Assert.AreEqual(newPerson.IsPlaceOfBirthUnknown, testPerson.IsPlaceOfBirthUnknown);
                Assert.AreEqual(newPerson.IsDateOfBirthUnknown, testPerson.IsDateOfBirthUnknown);
            };
            context.Revert();
            var person = await service.CreateAsync(newPerson);
            tester(person);
        }

        //[TestMethod]
        //public async Task TestCreateDependentAsync_CheckProperties()
        //{
        //    var user = new User(1);
        //    int personId = 1;
        //    int personTypeId = PersonType.Spouse.Id;
        //    var firstName = "first";
        //    var lastName = "last";
        //    var suffix = "jr";
        //    var passport = "first last";
        //    var preferred = "first last";
        //    var gender = Gender.Female.Id;
        //    var dateOfBirth = DateTime.Now;
        //    int placeOfBirth = 193;
        //    var birthCountryReason = "military";
        //    var countriesOfCitizenship = new List<int>();
        //    var countryResidence = 193;
        //    bool isTravellingWithParticipant = true;

        //    var newPerson = new NewPersonDependent(createdBy: user, personId: personId, personTypeId: personTypeId,
        //        firstName: firstName, lastName: lastName, nameSuffix: suffix, passportName: passport, preferredName: preferred, genderId: gender,
        //        dateOfBirth: dateOfBirth, locationOfBirthId: placeOfBirth, residenceLocationId: countryResidence, birthCountryReason: birthCountryReason,
        //        countriesOfCitizenship: countriesOfCitizenship, isTravelWithParticipant: isTravellingWithParticipant);
            
        //    Action<PersonDependent> tester = (testPerson) =>
        //    {
        //        Assert.AreEqual(newPerson.PersonId, testPerson.PersonId);
        //        Assert.AreEqual(newPerson.PersonTypeId, testPerson.PersonTypeId);
        //        Assert.AreEqual(newPerson.FirstName, testPerson.FirstName);
        //        Assert.AreEqual(newPerson.LastName, testPerson.LastName);
        //        Assert.AreEqual(newPerson.NameSuffix, testPerson.NameSuffix);
        //        Assert.AreEqual(newPerson.PassportName, testPerson.PassportName);
        //        Assert.AreEqual(newPerson.PreferredName, testPerson.PreferredName);
        //        Assert.AreEqual(newPerson.GenderId, testPerson.GenderId);
        //        Assert.AreEqual(newPerson.DateOfBirth, testPerson.DateOfBirth);
        //        Assert.AreEqual(newPerson.PlaceOfBirth_LocationId, testPerson.PlaceOfBirth_LocationId);
        //        Assert.AreEqual(newPerson.Residence_LocationId, testPerson.Residence_LocationId);
        //        Assert.AreEqual(newPerson.BirthCountryReason, testPerson.BirthCountryReason);
        //        CollectionAssert.AreEqual(newPerson.CountriesOfCitizenship, testPerson.CountriesOfCitizenship.Select(x => x.LocationId).ToList());
        //        Assert.AreEqual(newPerson.IsTravellingWithParticipant, testPerson.IsTravellingWithParticipant);

        //        Assert.AreEqual(user.Id, testPerson.History.CreatedBy);
        //        Assert.AreEqual(user.Id, testPerson.History.RevisedBy);
        //        DateTimeOffset.UtcNow.Should().BeCloseTo(testPerson.History.CreatedOn, 20000);
        //        DateTimeOffset.UtcNow.Should().BeCloseTo(testPerson.History.RevisedOn, 20000);
        //    };
        //    context.Revert();
        //    var person = await service.CreateDependentAsync(newPerson);
        //    tester(person);
        //}

        [TestMethod]
        public async Task TestCreateAsync_CityOfBirth()
        {
            var city = new Location
            {
                LocationId = 1
            };

            context.Locations.Add(city);

            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = city.LocationId;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int>();

            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.NotApplicable.Id
            };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: project.ProjectId,
                participantTypeId: participantTypeId,
                firstName: firstName,
                lastName: lastName,
                gender: gender,
                dateOfBirth: dateOfBirth,
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);
            context.SetupActions.Add(() =>
            {
                context.Locations.Add(city);
                context.Projects.Add(project);
            });
            Action<Person> tester = (testPerson) =>
            {
                Assert.AreEqual(city.LocationId, testPerson.PlaceOfBirthId);
            };
            context.Revert();
            var person = await service.CreateAsync(newPerson);
            tester(person);
        }

        [TestMethod]
        public async Task TestCreateAsync_CountriesOfCitizenship()
        {
            var country = new Location
            {
                LocationId = 2
            };

            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var participantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = 5;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int> { country.LocationId };

            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.NotApplicable.Id
            };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: project.ProjectId,
                participantTypeId: participantTypeId,
                firstName: firstName,
                lastName: lastName,
                gender: gender,
                dateOfBirth: dateOfBirth,
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);
            context.SetupActions.Add(() =>
            {
                context.Locations.Add(country);
                context.Projects.Add(project);
            });
            Action<Person> tester = (testPerson) =>
            {
                CollectionAssert.AreEqual(newPerson.CountriesOfCitizenship, testPerson.CountriesOfCitizenship.Select(x => x.LocationId).ToList());
            };
            context.Revert();
            var person = await service.CreateAsync(newPerson);
            tester(person);

        }

        [TestMethod]
        public async Task TestCreateAsync_Associations()
        {
            var project = new Project
            {
                ProjectId = 1
            };

            context.Projects.Add(project);
            
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var projectId = project.ProjectId;
            var participantTypeId = ParticipantType.Individual.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = 5;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int> { 1 };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: projectId,
                participantTypeId: participantTypeId,
                firstName: firstName,
                lastName: lastName,
                gender: gender,
                dateOfBirth: dateOfBirth,
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);

            Action<Person> tester = (testPerson) =>
            {
                var participant = context.Participants.Where(x => x.PersonId == testPerson.PersonId).FirstOrDefault();
                Assert.AreEqual(testPerson.PersonId, participant.PersonId);
                Assert.AreEqual(participantTypeId, participant.ParticipantTypeId);
                // Check that participant is associated to project
                var participantProject = participant.Project;
                Assert.IsTrue(Object.ReferenceEquals(project, participant.Project));
            };

            var person = await service.CreateAsync(newPerson);
            tester(person);
        }

        [TestMethod]
        public async Task TestGetExistingPersonAsync_DoesNotExist()
        {
            var user = new User(1);
            var firstName = "first";
            var lastName = "last";
            var projectId = 1;
            var participantTypeId = ParticipantType.Individual.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = 5;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int> { 1 };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: projectId,
                participantTypeId: participantTypeId,
                firstName: firstName,
                lastName: lastName,
                gender: gender,
                dateOfBirth: dateOfBirth,
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);

            var person = await service.GetExistingPersonAsync(newPerson);
            Assert.IsNull(person);
        }

        [TestMethod]
        public async Task TestGetExistingPersonAsync_NamesDifferentCases()
        {
            var existingPerson = new Person
            {
                FirstName = "firstName",
                LastName = "lastName",
                GenderId = Gender.Female.Id,
                DateOfBirth = DateTime.Now,
                PlaceOfBirthId = 1
            };

            context.People.Add(existingPerson);

            var user = new User(1);
            var projectId = 1;
            var participantTypeId = ParticipantType.Individual.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = existingPerson.PlaceOfBirthId;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int> { 1 };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: projectId,
                participantTypeId: participantTypeId,
                firstName: existingPerson.FirstName.ToUpper(),
                lastName: existingPerson.LastName.ToLower(),
                gender: gender,
                dateOfBirth: dateOfBirth,
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);

            var person = await service.GetExistingPersonAsync(newPerson);
            Assert.IsNotNull(person);
        }

        [TestMethod]
        public async Task TestGetExistingPersonAsync_NamesHaveSpacesBeforeAndAfter()
        {
            var existingPerson = new Person
            {
                FirstName = " firstName ",
                LastName = " lastName ",
                GenderId = Gender.Female.Id,
                DateOfBirth = DateTime.Now,
                PlaceOfBirthId = 1
            };

            context.People.Add(existingPerson);

            var user = new User(1);
            var projectId = 1;
            var participantTypeId = ParticipantType.Individual.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = existingPerson.PlaceOfBirthId;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int> { 1 };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: projectId,
                participantTypeId: participantTypeId,
                firstName: existingPerson.FirstName.Trim(),
                lastName: existingPerson.LastName.Trim(),
                gender: gender,
                dateOfBirth: dateOfBirth,
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);
            var person = await service.GetExistingPersonAsync(newPerson);
            Assert.IsNotNull(person);
        }

        [TestMethod]
        public async Task TestGetExistingPersonAsync_DateOfBirthWithDifferentTime()
        {
            var dateAndTime = new DateTime(2008, 5, 1, 8, 6, 32, DateTimeKind.Utc);
            var existingPerson = new Person
            {
                FirstName = "firstName",
                LastName = "lastName",
                GenderId = Gender.Female.Id,
                DateOfBirth = dateAndTime,
                PlaceOfBirthId = 1
            };

            context.People.Add(existingPerson);

            var user = new User(1);
            var projectId = 1;
            var participantTypeId = ParticipantType.Individual.Id;
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var isDateOfBirthUnknown = true;
            var isDateOfBirthEstimated = true;
            var isPlaceOfBirthUnknown = true;
            var cityOfBirth = existingPerson.PlaceOfBirthId;
            var personTypeId = PersonType.Participant.Id;
            var countriesOfCitizenship = new List<int> { 1 };

            var newPerson = new NewPerson(
                createdBy: user,
                projectId: projectId,
                participantTypeId: participantTypeId,
                firstName: existingPerson.FirstName.ToUpper(),
                lastName: existingPerson.LastName.ToUpper(),
                gender: gender,
                dateOfBirth: existingPerson.DateOfBirth.Value.AddHours(2),
                isDateOfBirthUnknown: isDateOfBirthUnknown,
                isDateOfBirthEstimated: isDateOfBirthEstimated,
                isPlaceOfBirthUnknown: isPlaceOfBirthUnknown,
                cityOfBirth: cityOfBirth,
                personTypeId: personTypeId,
                countriesOfCitizenship: countriesOfCitizenship);
            var person = await service.GetExistingPersonAsync(newPerson);
            Assert.IsNotNull(person);
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task TestUpdatePiiAsync_CheckProperties()
        {
            var creatorId = 1;
            var updatorId = 2;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var person = new Person
            {
                PersonId = 1,
                FirstName = "",
                LastName = "",
                GenderId = Gender.Male.Id,
                PlaceOfBirthId = default(int)
            };
            person.History.CreatedBy = creatorId;
            person.History.CreatedOn = yesterday;
            person.History.RevisedBy = creatorId;
            person.History.RevisedOn = yesterday;

            context.People.Add(person);

            var pii = new UpdatePii(new User(updatorId),
                                    person.PersonId,
                                    "firstName",
                                    "lastName",
                                    "namePrefix",
                                    "nameSuffix",
                                    "givenName",
                                    "familyName",
                                    "middleName",
                                    "patronym",
                                    "alias",
                                    default(int),
                                    "ethnicity",
                                    default(int?),
                                    DateTime.Now,
                                    false,
                                    false,
                                    new List<int>(),
                                    false,
                                    "medicalConditions",
                                    default(int));
            var updatedPerson = await service.UpdatePiiAsync(pii);
            Assert.AreEqual(pii.FirstName, updatedPerson.FirstName);
            Assert.AreEqual(pii.LastName, updatedPerson.LastName);
            Assert.AreEqual(pii.NamePrefix, updatedPerson.NamePrefix);
            Assert.AreEqual(pii.NameSuffix, updatedPerson.NameSuffix);
            Assert.AreEqual(pii.GivenName, updatedPerson.GivenName);
            Assert.AreEqual(pii.FamilyName, updatedPerson.FamilyName);
            Assert.AreEqual(pii.MiddleName, updatedPerson.MiddleName);
            Assert.AreEqual(pii.Patronym, updatedPerson.Patronym);
            Assert.AreEqual(pii.Alias, updatedPerson.Alias);
            Assert.AreEqual(pii.Ethnicity, updatedPerson.Ethnicity);
            Assert.AreEqual(pii.DateOfBirth, updatedPerson.DateOfBirth);
            Assert.AreEqual(pii.MedicalConditions, updatedPerson.MedicalConditions);
            Assert.AreEqual(creatorId, updatedPerson.History.CreatedBy);
            Assert.AreEqual(yesterday, updatedPerson.History.CreatedOn);
            Assert.AreEqual(updatorId, updatedPerson.History.RevisedBy);
            DateTimeOffset.UtcNow.Should().BeCloseTo(updatedPerson.History.RevisedOn, 20000);

            validator.Verify(x => x.ValidateUpdate(It.IsAny<PersonServiceValidationEntity>()), Times.Once());
        }

        [TestMethod]
        public async Task TestUpdatePiiAsync_CheckDateOfBirthIsEstimated()
        {
            var person = new Person
            {
                PersonId = 1,
                FirstName = "",
                LastName = "",
                GenderId = Gender.Male.Id,
                PlaceOfBirthId = default(int)
            };

            context.People.Add(person);

            var pii = new UpdatePii(new User(0),
                                    person.PersonId,
                                    "firstName",
                                    "lastName",
                                    "namePrefix",
                                    "nameSuffix",
                                    "givenName",
                                    "familyName",
                                    "middleName",
                                    "patronym",
                                    "alias",
                                    default(int),
                                    "ethnicity",
                                    default(int?),
                                    DateTime.Now,
                                    false,
                                    true,
                                    new List<int>(),
                                    false,
                                    "medicalConditions",
                                    default(int));
            var updatedPerson = await service.UpdatePiiAsync(pii);
            Assert.IsTrue(updatedPerson.IsDateOfBirthEstimated.HasValue);
            Assert.IsTrue(updatedPerson.IsDateOfBirthEstimated.Value);
        }

        [TestMethod]
        public async Task TestUpdatePiiAsync_CheckDateOfBirthIsUnknown()
        {
            var person = new Person
            {
                PersonId = 1,
                FirstName = "",
                LastName = "",
                GenderId = Gender.Male.Id,
                PlaceOfBirthId = default(int)
            };

            context.People.Add(person);

            var pii = new UpdatePii(new User(0),
                                    person.PersonId,
                                    "firstName",
                                    "lastName",
                                    "namePrefix",
                                    "nameSuffix",
                                    "givenName",
                                    "familyName",
                                    "middleName",
                                    "patronym",
                                    "alias",
                                    default(int),
                                    "ethnicity",
                                    default(int?),
                                    null,
                                    true,
                                    true,
                                    new List<int>(),
                                    false,
                                    "medicalConditions",
                                    default(int));
            var updatedPerson = await service.UpdatePiiAsync(pii);
            Assert.IsTrue(updatedPerson.IsDateOfBirthUnknown.HasValue);
            Assert.IsTrue(updatedPerson.IsDateOfBirthUnknown.Value);
        }

        [TestMethod]
        public async Task TestUpdatePiiAsync_CheckPlaceOfBirthIsUnknown()
        {
            var person = new Person
            {
                PersonId = 1,
                FirstName = "",
                LastName = "",
                GenderId = Gender.Male.Id,
            };

            context.People.Add(person);

            var pii = new UpdatePii(new User(0),
                                    person.PersonId,
                                    "firstName",
                                    "lastName",
                                    "namePrefix",
                                    "nameSuffix",
                                    "givenName",
                                    "familyName",
                                    "middleName",
                                    "patronym",
                                    "alias",
                                    default(int),
                                    "ethnicity",
                                    default(int?),
                                    DateTime.Now,
                                    false,
                                    true,
                                    new List<int>(),
                                    true,
                                    "medicalConditions",
                                    default(int));
            var updatedPerson = await service.UpdatePiiAsync(pii);
            Assert.IsTrue(updatedPerson.IsPlaceOfBirthUnknown.HasValue);
            Assert.IsTrue(updatedPerson.IsPlaceOfBirthUnknown.Value);
        }

        [TestMethod]
        public async Task TestUpdatePiiAsync_CheckGender()
        {
            var gender = new Gender
            {
                GenderId = Gender.Female.Id,
                GenderName = Gender.Female.Value
            };

            var person = new Person
            {
                PersonId = 1,
                FirstName = "",
                LastName = "",
                GenderId = Gender.Male.Id,
            };

            context.Genders.Add(gender);
            context.People.Add(person);

            var pii = new UpdatePii(new User(0),
                                    person.PersonId,
                                    "firstName",
                                    "lastName",
                                    "namePrefix",
                                    "nameSuffix",
                                    "givenName",
                                    "familyName",
                                    "middleName",
                                    "patronym",
                                    "alias",
                                    gender.GenderId,
                                    "ethnicity",
                                    default(int?),
                                    DateTime.Now,
                                    false,
                                    false,
                                    new List<int>(),
                                    false,
                                    "medicalConditions",
                                    default(int));
            var updatedPerson = await service.UpdatePiiAsync(pii);
            Assert.AreEqual(gender.GenderId, updatedPerson.GenderId);
        }

        [TestMethod]
        public async Task TestUpdatePiiAsync_CheckPlaceOfBirth()
        {
            var placeOfBirth = new Location
            {
                LocationId = 1
            };

            var person = new Person
            {
                PersonId = 1,
                FirstName = "",
                LastName = "",
                GenderId = Gender.Male.Id                
            };
            
            context.Locations.Add(placeOfBirth);
            context.People.Add(person);

            var pii = new UpdatePii(new User(0),
                                    person.PersonId,
                                    "firstName",
                                    "lastName",
                                    "namePrefix",
                                    "nameSuffix",
                                    "givenName",
                                    "familyName",
                                    "middleName",
                                    "patronym",
                                    "alias",
                                    default(int),
                                    "ethnicity",
                                    placeOfBirth.LocationId,
                                    DateTime.Now,
                                    false,
                                    false,
                                    new List<int>(),
                                    false,
                                    "medicalConditions",
                                    default(int));
            var updatedPerson = await service.UpdatePiiAsync(pii);
            Assert.AreEqual(placeOfBirth.LocationId, updatedPerson.PlaceOfBirthId);
        }

        [TestMethod]
        public async Task TestUpdatePiiAsync_CheckCountriesOfCitizenship()
        {
            var country = new Location
            {
                LocationId = 1
            };

            var person = new Person
            {
                PersonId = 1,
                FirstName = "",
                LastName = "",
                GenderId = Gender.Male.Id,
            };
            
            context.Locations.Add(country);
            context.People.Add(person);

            var countriesOfCitizenship = new List<int>();
            countriesOfCitizenship.Add(country.LocationId);

            var pii = new UpdatePii(new User(0),
                                    person.PersonId,
                                    "firstName",
                                    "lastName",
                                    "namePrefix",
                                    "nameSuffix",
                                    "givenName",
                                    "familyName",
                                    "middleName",
                                    "patronym",
                                    "alias",
                                    default(int),
                                    "ethnicity",
                                    default(int?),
                                    DateTime.Now,
                                    false,
                                    false,
                                    countriesOfCitizenship,
                                    false,
                                    "medicalConditions",
                                    default(int));
            var updatedPerson = await service.UpdatePiiAsync(pii);

            CollectionAssert.AreEqual(countriesOfCitizenship, updatedPerson.CountriesOfCitizenship.Select(x => x.LocationId).ToList());
        }

        [TestMethod]
        public async Task TestUpdatePiiAsync_PlaceOfBirthDoesNotExist()
        {
            var person = new Person
            {
                PersonId = 1,
                FirstName = "",
                LastName = "",
                GenderId = Gender.Male.Id,
            };

            var placeOfBirthId = 0;
            context.People.Add(person);

            var pii = new UpdatePii(new User(0),
                                    person.PersonId,
                                    "firstName",
                                    "lastName",
                                    "namePrefix",
                                    "nameSuffix",
                                    "givenName",
                                    "familyName",
                                    "middleName",
                                    "patronym",
                                    "alias",
                                    default(int),
                                    "ethnicity",
                                    placeOfBirthId,
                                    DateTime.Now,
                                    false,
                                    false,
                                    new List<int>(),
                                    false,
                                    "medicalConditions",
                                    default(int));
            Func<Task> f = () =>
            {
                return service.UpdatePiiAsync(pii);
            };
            var message = String.Format("The location entity with id [{0}] was not found.", placeOfBirthId);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdatePiiAsync_CheckMaritalStatus()
        {
            var maritalStatus = new MaritalStatus
            {
                MaritalStatusId = MaritalStatus.Single.Id,
                Status = MaritalStatus.Single.Value
            };

            var person = new Person
            {
                PersonId = 1,
                FirstName = "",
                LastName = "",
                GenderId = Gender.Male.Id,
            };
            
            context.MaritalStatuses.Add(maritalStatus);
            context.People.Add(person);

            var pii = new UpdatePii(new User(0),
                                    person.PersonId,
                                    "firstName",
                                    "lastName",
                                    "namePrefix",
                                    "nameSuffix",
                                    "givenName",
                                    "familyName",
                                    "middleName",
                                    "patronym",
                                    "alias",
                                    default(int),
                                    "ethnicity",
                                    default(int?),
                                    DateTime.Now,
                                    false,
                                    false,
                                    new List<int>(),
                                    false,
                                    "medicalConditions",
                                    maritalStatus.MaritalStatusId);
            var updatedPerson = await service.UpdatePiiAsync(pii);
            Assert.AreEqual(maritalStatus.MaritalStatusId, updatedPerson.MaritalStatusId);
        }

        [TestMethod]
        public async Task TestUpdatePiiAsync_CheckSevisId()
        {
            var person = new Person
            {
                PersonId = 1,
                FirstName = "",
                LastName = "",
                GenderId = Gender.Male.Id,
            };

            var participant = new Participant
            {
                ParticipantId = 1,
                PersonId = person.PersonId
            };

            context.People.Add(person);

            var pii = new UpdatePii(new User(0),
                                    person.PersonId,
                                     "firstName",
                                    "lastName",
                                    "namePrefix",
                                    "nameSuffix",
                                    "givenName",
                                    "familyName",
                                    "middleName",
                                    "patronym",
                                    "alias",
                                    default(int),
                                    "ethnicity",
                                    default(int?),
                                    DateTime.Now,
                                    false,
                                    false,
                                    new List<int>(),
                                    false,
                                    "medicalConditions",
                                    default(int));
            var updatedPerson = await service.UpdatePiiAsync(pii);
        }

        [TestMethod]
        public async Task TestUpdatePiiAsync_CheckDuplicate()
        {
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };

            var placeOfBirth = new Location
            {
                LocationId = 1
            };

            var dateOfBirth = DateTime.Now;

            var person = new Person
            {
                PersonId = 1,
                FirstName = "firstName",
                LastName = "lastName",
                GenderId = gender.GenderId,
                PlaceOfBirthId = placeOfBirth.LocationId,
                DateOfBirth = dateOfBirth
            };

            var participant = new Participant
            {
                ParticipantId = 1,
                PersonId = person.PersonId
            };

            context.Genders.Add(gender);
            context.Locations.Add(placeOfBirth);
            context.People.Add(person);
            context.Participants.Add(participant);

            var pii = new UpdatePii(new User(0),
                                    2,
                                    "firstName",
                                    "lastName",
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null,
                                    gender.GenderId,
                                    null,
                                    placeOfBirth.LocationId,
                                    dateOfBirth,
                                    false,
                                    false,
                                    new List<int>(),
                                    false,
                                    null,
                                    default(int));
            Func<Task> f = () =>
            {
                return service.UpdatePiiAsync(pii);
            };
            f.ShouldThrow<EcaBusinessException>().WithMessage("The person already exists.");
        }
        
        


        #endregion

        #region Get People
        [TestMethod]
        public async Task TestGetPeople_CheckProperties()
        {
            var dob = DateTime.UtcNow;
            var gender = new Gender
            {
                GenderId = Gender.Female.Id,
                GenderName = Gender.Female.Value
            };
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
            var person = new Person
            {
                Alias = "alias",
                DateOfBirth = dob,
                FamilyName = "family",
                FirstName = "firstName",
                Gender = gender,
                GenderId = gender.GenderId,
                GivenName = "given",
                LastName = "last name",
                MiddleName = "middle",
                NamePrefix = "name prefix",
                NameSuffix = "name suffix",
                Patronym = "patronym",
                PersonId = 1,
                PersonType = ptype
            };
            context.Genders.Add(gender);
            context.People.Add(person);

            Action<PagedQueryResults<SimplePersonDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var firstResult = results.Results.First();
                Assert.AreEqual(gender.GenderName, firstResult.Gender);
                Assert.AreEqual(person.Alias, firstResult.Alias);
                Assert.AreEqual(person.DateOfBirth, firstResult.DateOfBirth);
                Assert.AreEqual(person.FamilyName, firstResult.FamilyName);
                Assert.AreEqual(person.FirstName, firstResult.FirstName);
                Assert.AreEqual(person.LastName, firstResult.LastName);
                Assert.AreEqual(person.MiddleName, firstResult.MiddleName);
                Assert.AreEqual(person.NamePrefix, firstResult.NamePrefix);
                Assert.AreEqual(person.NameSuffix, firstResult.NameSuffix);
                Assert.AreEqual(person.Patronym, firstResult.Patronym);
                Assert.AreEqual(person.PersonId, firstResult.PersonId);
            };
            var defaultSort = new ExpressionSorter<SimplePersonDTO>(x => x.PersonId, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimplePersonDTO>(0, 10, defaultSort);
            var serviceResults = service.GetPeople(queryOperator);
            var serviceResultsAsync = await service.GetPeopleAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetPeople_DefaultSort()
        {
            var dob = DateTime.UtcNow;
            var gender = new Gender
            {
                GenderId = Gender.Female.Id,
                GenderName = Gender.Female.Value
            };
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
            var person1 = new Person
            {
                PersonId = 1,
                GenderId = gender.GenderId,
                Gender = gender,
                PersonType = ptype
            };
            var person2 = new Person
            {
                PersonId = 2,
                GenderId = gender.GenderId,
                Gender = gender,
                PersonType = ptype
            };
            context.Genders.Add(gender);
            context.People.Add(person1);
            context.People.Add(person2);

            Action<PagedQueryResults<SimplePersonDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(2, results.Results.Count);
                Assert.AreEqual(person2.PersonId, results.Results.First().PersonId);
                Assert.AreEqual(person1.PersonId, results.Results.Last().PersonId);
            };
            var defaultSort = new ExpressionSorter<SimplePersonDTO>(x => x.PersonId, SortDirection.Descending);
            var queryOperator = new QueryableOperator<SimplePersonDTO>(0, 10, defaultSort);
            var serviceResults = service.GetPeople(queryOperator);
            var serviceResultsAsync = await service.GetPeopleAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetPeople_Filter()
        {
            var dob = DateTime.UtcNow;
            var gender = new Gender
            {
                GenderId = Gender.Female.Id,
                GenderName = Gender.Female.Value
            };
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
            var person1 = new Person
            {
                PersonId = 1,
                GenderId = gender.GenderId,
                Gender = gender,
                PersonType = ptype
            };
            var person2 = new Person
            {
                PersonId = 2,
                GenderId = gender.GenderId,
                Gender = gender,
                PersonType = ptype
            };
            context.Genders.Add(gender);
            context.People.Add(person1);
            context.People.Add(person2);

            Action<PagedQueryResults<SimplePersonDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(person1.PersonId, results.Results.First().PersonId);
            };
            var defaultSort = new ExpressionSorter<SimplePersonDTO>(x => x.PersonId, SortDirection.Descending);
            var queryOperator = new QueryableOperator<SimplePersonDTO>(0, 10, defaultSort);
            queryOperator.Filters.Add(new ExpressionFilter<SimplePersonDTO>(x => x.PersonId, ComparisonType.Equal, person1.PersonId));
            var serviceResults = service.GetPeople(queryOperator);
            var serviceResultsAsync = await service.GetPeopleAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        [TestMethod]
        public async Task TestGetPeople_Paging()
        {
            var dob = DateTime.UtcNow;
            var gender = new Gender
            {
                GenderId = Gender.Female.Id,
                GenderName = Gender.Female.Value
            };
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
            var person1 = new Person
            {
                PersonId = 1,
                GenderId = gender.GenderId,
                Gender = gender,
                PersonType = ptype
            };
            var person2 = new Person
            {
                PersonId = 2,
                GenderId = gender.GenderId,
                Gender = gender,
                PersonType = ptype
            };
            context.Genders.Add(gender);
            context.People.Add(person1);
            context.People.Add(person2);

            Action<PagedQueryResults<SimplePersonDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(person1.PersonId, results.Results.First().PersonId);
            };
            var defaultSort = new ExpressionSorter<SimplePersonDTO>(x => x.PersonId, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimplePersonDTO>(0, 1, defaultSort);
            var serviceResults = service.GetPeople(queryOperator);
            var serviceResultsAsync = await service.GetPeopleAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        #endregion
    }
}