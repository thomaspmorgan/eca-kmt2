﻿using ECA.Business.Queries.Models.Persons;
using System.Linq;
using ECA.Business.Service.Persons;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using ECA.Business.Service;
using System.Data.Entity;
using ECA.Business.Exceptions;
using ECA.Business.Validation;
using Moq;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;

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
        public async Task TestGetGenerById_CheckProperties()
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
                Name = "partOrg"
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

            var language1 = new LanguageProficiency
            {
                LanguageProficiencyId = 1,
                LanguageName = "lang1"
            };

            var dependant1 = new Person
            {
                PersonId = 2,
                Gender = gender,
                FirstName = "firstName",
                LastName = "lastName",
                DateOfBirth = DateTime.Now,
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
                FirstName = "firstName",
                LastName = "lastName",
                NamePrefix = "namePrefix",
                NameSuffix = "nameSuffix",
            };

            context.Genders.Add(gender);
            context.LanguangeProficiencies.Add(language1);
            context.ProminentCategories.Add(prominentCat1);
            context.Activities.Add(activity1);
            context.Memberships.Add(membership1);
            context.People.Add(dependant1);
            context.Impacts.Add(impact1);

            person.ProminentCategories.Add(prominentCat1);
            person.Activities.Add(activity1);
            person.Memberships.Add(membership1);
            person.LanguageProficiencies.Add(language1);
            person.Family.Add(dependant1);
            person.Impacts.Add(impact1);

            context.People.Add(person);


            Action<GeneralDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(person.ProminentCategories.FirstOrDefault().Name, serviceResult.ProminentCategories.FirstOrDefault().Value);
                Assert.AreEqual(person.Activities.FirstOrDefault().Title, serviceResult.Activities.FirstOrDefault().Value);
                Assert.AreEqual(person.Memberships.FirstOrDefault().Name, serviceResult.Memberships.FirstOrDefault().Name);
                Assert.AreEqual(person.LanguageProficiencies.FirstOrDefault().LanguageName, serviceResult.LanguageProficiencies.FirstOrDefault().Value);
                Assert.AreEqual(person.Family.FirstOrDefault().LastName + ", " + person.Family.FirstOrDefault().FirstName, serviceResult.Dependants.FirstOrDefault().Value);
                Assert.AreEqual(person.Impacts.FirstOrDefault().Description, serviceResult.ImpactStories.FirstOrDefault().Value);
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

            person.PlaceOfBirth.Country = new Location();

            context.Genders.Add(gender);
            context.People.Add(person);

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
                LocationTypeId = LocationType.State.Id,
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
                DisplayName = "display"
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
                Assert.AreEqual(address.DisplayName, testAddress.AddressDisplayName);
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
                PlaceOfBirthId = city.LocationId,
                MaritalStatus = new MaritalStatus()
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
            var resultAsync = await service.GetPiiByIdAsync(person.PersonId);

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

            var phoneNumber = new PhoneNumber
            {
                PhoneNumberId = 1,
                PhoneNumberTypeId  = phoneNumberType.PhoneNumberTypeId,
                PhoneNumberType = phoneNumberType,
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

            tester(result);
            tester(resultAsync);
        }

        #endregion

        #region Get EvaluationNotes By Id

        [TestMethod]
        public async Task TestGetEvaluationNotesById()
        {
            int principalId = 1;
            int personId = 2;
            DateTime date1 = new DateTime(2015,6,11);
            DateTime date2 = new DateTime(2015, 6, 2);
            var user = new UserAccount
            {
                PrincipalId = principalId,
                FirstName = "Jack",
                LastName = "Diddly",
                DisplayName = "Jack Diddly",
                EmailAddress = "jack@diddly.us"
            };

            var history1 = new History
            {
                CreatedBy = principalId,
                CreatedOn = new DateTimeOffset(date1),
                RevisedBy = principalId,
                RevisedOn = new DateTimeOffset(date1)
            };

            var history2 = new History
            {
                CreatedBy = principalId,
                CreatedOn = new DateTimeOffset(date2),
                RevisedBy = principalId,
                RevisedOn = new DateTimeOffset(date2)
            };

            var person = new Person
            {
                PersonId = personId
            };

            var eval1 = new PersonEvaluationNote
            {
                EvaluationNoteId = 1,
                PersonId = personId,
                EvaluationNote = "Jack is awesome.",
                History = history1
            };

            var eval2 = new PersonEvaluationNote
            {
                EvaluationNoteId = 2,
                PersonId = personId,
                EvaluationNote = "Jack is really cool.",
                History = history2
            };

            person.EvaluationNotes.Add(eval1);
            person.EvaluationNotes.Add(eval2);

            context.UserAccounts.Add(user);
            context.PersonEvaluationNotes.Add(eval1);
            context.PersonEvaluationNotes.Add(eval2);
            context.People.Add(person);

            Action<IList<EvaluationNoteDTO>> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(person.EvaluationNotes.Count(), serviceResult.Count());
                CollectionAssert.AreEqual(person.EvaluationNotes.Select(x => x.EvaluationNote).ToList(), serviceResult.Select(x => x.EvaluationNote).ToList());
                CollectionAssert.AreEqual(person.EvaluationNotes.Select(x => x.History.CreatedBy).ToList(), serviceResult.Select(x => x.UserId).ToList());
            };

            var result = service.GetEvaluationNotesByPersonId(personId);
            var resultAsync = await service.GetEvaluationNotesByPersonIdAsync(personId);

            tester(result);
            tester(resultAsync);

        }
        #endregion

        #region Create
        [TestMethod]
        public async Task TestCreateAsync()
        {
            var newPerson = new NewPerson(new User(0), 1, "firstName", "lastName",
                                          Gender.Male.Id, DateTime.Now, 1,
                                          new List<int>());
            var person = await service.CreateAsync(newPerson);

            Assert.AreEqual(newPerson.FirstName, person.FirstName);
            Assert.AreEqual(newPerson.LastName, person.LastName);
            Assert.AreEqual(newPerson.Gender, person.GenderId);
            Assert.AreEqual(newPerson.DateOfBirth, person.DateOfBirth);
           
        }

        [TestMethod]
        public async Task TestCreateAsync_CityOfBirth()
        {
            var city = new Location 
            {
                LocationId = 1
            };

            context.Locations.Add(city);

            var newPerson = new NewPerson(new User(0), 1, "firstName", "lastName",
                                        Gender.Male.Id, DateTime.Now, city.LocationId,
                                        new List<int>());
            var person = await service.CreateAsync(newPerson);
            Assert.AreEqual(city.LocationId, person.PlaceOfBirthId);
        }

        [TestMethod]
        public async Task TestCreateAsync_CountriesOfCitizenship()
        {
            var country = new Location
            {
                LocationId = 2
            };

            context.Locations.Add(country);

            List<int> countriesOfCitizenship = new List<int>(new int[] { country.LocationId });
            var newPerson = new NewPerson(new User(0), 1, "firstName", "lastName",
                                         Gender.Male.Id, DateTime.Now, 1,
                                         countriesOfCitizenship);
            var person = await service.CreateAsync(newPerson);
            CollectionAssert.AreEqual(newPerson.CountriesOfCitizenship,
                person.CountriesOfCitizenship.Select(x => x.LocationId).ToList());
        }

        [TestMethod]
        public async Task TestCreateAsync_Associations()
        {
            var project = new Project
            {
                ProjectId = 1
            };

            context.Projects.Add(project);

            var newPerson = new NewPerson(new User(0), project.ProjectId, "firstName", "lastName",
                                         Gender.Male.Id, DateTime.Now, 1,
                                         new List<int>());
            var person = await service.CreateAsync(newPerson);
            // Check that participant is associated to person
            var participant = await context.Participants.Where(x => x.PersonId == person.PersonId).FirstOrDefaultAsync();
            Assert.AreEqual(person.PersonId, participant.PersonId);
            // Check that participant is associated to project
            var participantProject = participant.Project;
            Assert.IsTrue(Object.ReferenceEquals(project, participant.Project));
        }

        [TestMethod]
        [ExpectedException(typeof(EcaBusinessException), "The person already exists.")]
        public async Task TestCreateAsync_Duplicate()
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

            var newPerson = new NewPerson(new User(0), 1, existingPerson.FirstName, existingPerson.LastName,
                                          existingPerson.GenderId, existingPerson.DateOfBirth.Value, 1,
                                          new List<int>());
            var person = await service.CreateAsync(newPerson);
        }
        
        [TestMethod]
        public async Task TestGetExistingPerson_DoesNotExist()
        {
            var newPerson = new NewPerson(new User(0), 1, "firstName", "lastName", 1, DateTime.Now, 1, new List<int>());
            var person = await service.GetExistingPerson(newPerson);
            Assert.IsNull(person);
        }

        [TestMethod]
        public async Task TestGetExistingPerson_NamesDifferentCases()
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

            var newPerson = new NewPerson(new User(0), 1, existingPerson.FirstName.ToUpper(), existingPerson.LastName.ToLower(),
                                          existingPerson.GenderId, existingPerson.DateOfBirth.Value, 1,
                                          new List<int>());
            var person = await service.GetExistingPerson(newPerson);
            Assert.IsNotNull(person);
        }

        [TestMethod]
        public async Task TestGetExistingPerson_NamesHaveSpacesBeforeAndAfter()
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

            var newPerson = new NewPerson(new User(0), 1, existingPerson.FirstName.Trim(), existingPerson.LastName.Trim(),
                                          existingPerson.GenderId, existingPerson.DateOfBirth.Value, 1,
                                          new List<int>());
            var person = await service.GetExistingPerson(newPerson);
            Assert.IsNotNull(person);
        }

        [TestMethod]
        public async Task TestGetExistingPerson_DateOfBirthWithDifferentTime()
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

            var newPerson = new NewPerson(new User(0), 1, existingPerson.FirstName.ToUpper(), existingPerson.LastName.ToLower(),
                                          existingPerson.GenderId, existingPerson.DateOfBirth.Value.AddHours(2), 1,
                                          new List<int>());
            var person = await service.GetExistingPerson(newPerson);
            Assert.IsNotNull(person);
        }
        #endregion

        #region Update
        [TestMethod]
        public async Task TestUpdatePiiAsync_CheckProperties()
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
                                    default(int),
                                    DateTime.Now,
                                    new List<int>(),
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
                PlaceOfBirthId = default(int)
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
                                    default(int),
                                    DateTime.Now,
                                    new List<int>(),
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
                GenderId = Gender.Male.Id,
                PlaceOfBirthId = default(int)
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
                                    new List<int>(),
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
                PlaceOfBirthId = default(int)
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
                                    default(int),
                                    DateTime.Now,
                                    countriesOfCitizenship,
                                    "medicalConditions",
                                    default(int));
            var updatedPerson = await service.UpdatePiiAsync(pii);

            CollectionAssert.AreEqual(countriesOfCitizenship, updatedPerson.CountriesOfCitizenship.Select(x => x.LocationId).ToList());
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
                PlaceOfBirthId = default(int)
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
                                    default(int),
                                    DateTime.Now,
                                    new List<int>(),
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
                PlaceOfBirthId = default(int)
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
                                    default(int),
                                    DateTime.Now,
                                    new List<int>(),
                                    "medicalConditions",
                                    default(int));
            var updatedPerson = await service.UpdatePiiAsync(pii);
        }

        [TestMethod]
        [ExpectedException(typeof(EcaBusinessException), "The person already exists.")]
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
                                    new List<int>(),
                                    null,
                                    default(int));
            var updatedPerson = await service.UpdatePiiAsync(pii);
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
            var person1 = new Person
            {
                PersonId = 1,
                GenderId = gender.GenderId,
                Gender = gender
            };
            var person2 = new Person
            {
                PersonId = 2,
                GenderId = gender.GenderId,
                Gender = gender
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
            var person1 = new Person
            {
                PersonId = 1,
                GenderId = gender.GenderId,
                Gender = gender
            };
            var person2 = new Person
            {
                PersonId = 2,
                GenderId = gender.GenderId,
                Gender = gender
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
            var person1 = new Person
            {
                PersonId = 1,
                GenderId = gender.GenderId,
                Gender = gender
            };
            var person2 = new Person
            {
                PersonId = 2,
                GenderId = gender.GenderId,
                Gender = gender
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