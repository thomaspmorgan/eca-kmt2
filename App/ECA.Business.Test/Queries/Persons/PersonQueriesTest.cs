using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Persons;

namespace ECA.Business.Test.Queries.Persons
{
    [TestClass]
    public class PersonQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_HasCurrentParticipation()
        {
            var person = new Person
            {
                Alias = "alias",
                FamilyName = "family",
                FirstName = "firstName",
                GivenName = "givenName",
                LastName = "lastName",
                MiddleName = "middleName",
                NamePrefix = "Mr.",
                NameSuffix = "III",
                Patronym = "patronym",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status"
            };
            var participant = new Participant
            {
                Status = status,
                Person = person,
            };
            context.ParticipantStatuses.Add(status);
            person.Participations.Add(participant);
            context.People.Add(person);
            context.Genders.Add(person.Gender);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(status.Status, result.CurrentStatus);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckNames()
        {
            var person = new Person
            {
                Alias = "alias",
                FamilyName = "family",
                FirstName = "firstName",
                GivenName = "givenName",
                LastName = "lastName",
                MiddleName = "middleName",
                NamePrefix = "Mr.",
                NameSuffix = "III",
                Patronym = "patronym",
                FullName = "fullname",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status"
            };
            var participant = new Participant
            {
                Status = status,
                Person = person,
            };
            context.ParticipantStatuses.Add(status);
            person.Participations.Add(participant);
            context.People.Add(person);
            context.Genders.Add(person.Gender);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(person.Alias, result.Alias);
            Assert.AreEqual(person.FamilyName, result.FamilyName);
            Assert.AreEqual(person.FirstName, result.FirstName);
            Assert.AreEqual(person.GivenName, result.GivenName);
            Assert.AreEqual(person.LastName, result.LastName);
            Assert.AreEqual(person.MiddleName, result.MiddleName);
            Assert.AreEqual(person.NamePrefix, result.NamePrefix);
            Assert.AreEqual(person.Patronym, result.Patronym);
            Assert.AreEqual(person.FullName, result.FullName);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckDateOfBirth()
        {

            var person = new Person
            {
                DateOfBirth = DateTime.UtcNow,
                IsDateOfBirthEstimated = true,
                FullName = "fullname",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                },
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status"
            };
            var participant = new Participant
            {
                Status = status,
                Person = person,
            };
            context.ParticipantStatuses.Add(status);
            person.Participations.Add(participant);
            context.People.Add(person);
            context.Genders.Add(person.Gender);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(person.DateOfBirth, result.DateOfBirth);
            Assert.AreEqual(person.IsDateOfBirthEstimated, result.IsDateOfBirthEstimated);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckDateOfBirthIsUnknown()
        {

            var person = new Person
            {
                IsDateOfBirthUnknown = true,
                FullName = "fullname",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                },
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status"
            };
            var participant = new Participant
            {
                Status = status,
                Person = person,
            };
            context.ParticipantStatuses.Add(status);
            person.Participations.Add(participant);
            context.People.Add(person);
            context.Genders.Add(person.Gender);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(person.IsDateOfBirthUnknown, result.IsDateOfBirthUnknown);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckPlaceOfBirth()
        {
            var country = new Location
            {
                LocationName = "country",
                LocationId = 1
            };
            var division = new Location
            {
                LocationName = "division",
                LocationId = 2
            };
            var city = new Location
            {
                LocationName = "city",
                LocationId = 3,
                Division = division,
                DivisionId = division.LocationId,
                Country = country,
                CountryId = country.LocationId
            };

            var person = new Person
            {

                FullName = "fullname",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                },
                PlaceOfBirth = city,
                PlaceOfBirthId = city.LocationId
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status"
            };
            var participant = new Participant
            {
                Status = status,
                Person = person,
            };
            context.Locations.Add(city);
            context.Locations.Add(division);
            context.Locations.Add(country);
            context.ParticipantStatuses.Add(status);
            person.Participations.Add(participant);
            context.People.Add(person);
            context.Genders.Add(person.Gender);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(city.LocationId, result.CityOfBirthId);
            Assert.AreEqual(city.LocationName, result.CityOfBirth);
            Assert.AreEqual(division.LocationName, result.DivisionOfBirth);
            Assert.AreEqual(country.LocationName, result.CountryOfBirth);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_PlaceOfBirthIsUnknown()
        {  
            var person = new Person
            {

                FullName = "fullname",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                },
                IsPlaceOfBirthUnknown = true
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status"
            };
            var participant = new Participant
            {
                Status = status,
                Person = person,
            };
            context.ParticipantStatuses.Add(status);
            person.Participations.Add(participant);
            context.People.Add(person);
            context.Genders.Add(person.Gender);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(result.IsPlaceOfBirthUnknown, result.IsPlaceOfBirthUnknown);
        }


        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckPlaceOfBirth_OnlyHasCity()
        {
            var city = new Location
            {
                LocationName = "city",
                LocationId = 3,
            };

            var person = new Person
            {

                FullName = "fullname",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                },
                PlaceOfBirth = city,
                PlaceOfBirthId = city.LocationId
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status"
            };
            var participant = new Participant
            {
                Status = status,
                Person = person,
            };
            context.Locations.Add(city);
            context.ParticipantStatuses.Add(status);
            person.Participations.Add(participant);
            context.People.Add(person);
            context.Genders.Add(person.Gender);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(city.LocationId, result.CityOfBirthId);
            Assert.AreEqual(city.LocationName, result.CityOfBirth);
            Assert.IsNull(result.DivisionOfBirth);
            Assert.IsNull(result.CountryOfBirth);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_DoesNotHavePlaceOfBirth()
        {

            var person = new Person
            {

                FullName = "fullname",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                },
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status"
            };
            var participant = new Participant
            {
                Status = status,
                Person = person,
            };
            context.ParticipantStatuses.Add(status);
            person.Participations.Add(participant);
            context.People.Add(person);
            context.Genders.Add(person.Gender);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.IsNull(result.CityOfBirthId);
            Assert.IsNull(result.CityOfBirth);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CurrentParticipantion_StatusNameIsNull()
        {
            var person = new Person
            {
                Alias = "alias",
                FamilyName = "family",
                FirstName = "firstName",
                GivenName = "givenName",
                LastName = "lastName",
                MiddleName = "middleName",
                NamePrefix = "Mr.",
                NameSuffix = "III",
                Patronym = "patronym",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
            };
            var participant = new Participant
            {
                Status = status,
                Person = person,
            };
            context.ParticipantStatuses.Add(status);
            person.Participations.Add(participant);
            context.People.Add(person);
            context.Genders.Add(person.Gender);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(PersonQueries.UNKNOWN_PARTICIPANT_STATUS, result.CurrentStatus);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CurrentParticipantion_StatusIsNull()
        {
            var person = new Person
            {
                Alias = "alias",
                FamilyName = "family",
                FirstName = "firstName",
                GivenName = "givenName",
                LastName = "lastName",
                MiddleName = "middleName",
                NamePrefix = "Mr.",
                NameSuffix = "III",
                Patronym = "patronym",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };
            var participant = new Participant
            {
                Person = person,
            };
            person.Participations.Add(participant);
            context.People.Add(person);
            context.Genders.Add(person.Gender);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(PersonQueries.UNKNOWN_PARTICIPANT_STATUS, result.CurrentStatus);
        }
    }
}
