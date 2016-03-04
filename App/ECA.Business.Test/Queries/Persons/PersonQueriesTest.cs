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

        #region CreateGetRelatedPersonByDependentFamilyMemberQuery
        [TestMethod]
        public void TestCreateGetRelatedPersonByDependentFamilyMemberQuery_CheckProperties()
        {
            var participantPersonType = new PersonType
            {
                PersonTypeId = PersonType.Participant.Id,
                IsDependentPersonType = false
            };
            var spousePersonType = new PersonType
            {
                PersonTypeId = PersonType.Spouse.Id,
                IsDependentPersonType = true
            };
            var dependent = new Person
            {
                PersonId = 10,
                PersonTypeId = spousePersonType.PersonTypeId,
                PersonType = spousePersonType
            };
            
            var person = new Person
            {
                PersonTypeId = participantPersonType.PersonTypeId,
                PersonType = participantPersonType,
                PersonId = 1,
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
            dependent.Family.Add(person);
            person.Family.Add(dependent);

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
            context.People.Add(dependent);
            context.Genders.Add(person.Gender);
            context.PersonTypes.Add(spousePersonType);
            context.PersonTypes.Add(participantPersonType);

            var result = PersonQueries.CreateGetRelatedPersonByDependentFamilyMemberQuery(context, dependent.PersonId).FirstOrDefault();
            Assert.IsNotNull(result);
            Assert.AreEqual(person.PersonId, result.PersonId);
        }

        [TestMethod]
        public void TestCreateGetRelatedPersonByDependentFamilyMemberQuery_DependentDoesNotHaveParticipantFamilyMember()
        {
            var spousePersonType = new PersonType
            {
                PersonTypeId = PersonType.Spouse.Id,
                IsDependentPersonType = true
            };
            var dependent = new Person
            {
                PersonId = 10,
                PersonTypeId = spousePersonType.PersonTypeId,
                PersonType = spousePersonType
            };

            var person = new Person
            {
                PersonTypeId = spousePersonType.PersonTypeId,
                PersonType = spousePersonType,
                PersonId = 1,
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
            dependent.Family.Add(person);
            person.Family.Add(dependent);

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
            context.People.Add(dependent);
            context.Genders.Add(person.Gender);
            context.PersonTypes.Add(spousePersonType);

            var result = PersonQueries.CreateGetRelatedPersonByDependentFamilyMemberQuery(context, dependent.PersonId).FirstOrDefault();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestCreateGetRelatedPersonByDependentFamilyMemberQuery_DepdendentDoesNotHaveFmaily()
        {
            var spousePersonType = new PersonType
            {
                PersonTypeId = PersonType.Spouse.Id,
                IsDependentPersonType = true
            };
            var dependent = new Person
            {
                PersonId = 10,
                PersonTypeId = spousePersonType.PersonTypeId,
                PersonType = spousePersonType
            };
            context.People.Add(dependent);
            context.PersonTypes.Add(spousePersonType);

            var result = PersonQueries.CreateGetRelatedPersonByDependentFamilyMemberQuery(context, dependent.PersonId).FirstOrDefault();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestCreateGetRelatedPersonByDependentFamilyMemberQuery_DepdendentDoesNotExist()
        {
            var result = PersonQueries.CreateGetRelatedPersonByDependentFamilyMemberQuery(context, 0).FirstOrDefault();
            Assert.IsNull(result);
        }
        #endregion

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_HasCurrentParticipation()
        {
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
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
                },
                PersonType = ptype
            };
            var activeStatus = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id,
                Status = ParticipantStatus.Active.Value
            };
            var activeParticipant = new Participant
            {
                ParticipantId = 25,
                Status = activeStatus,
                Person = person,
                ProjectId = 10
            };
            var alumnusStatus = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Alumnus.Id,
                Status = ParticipantStatus.Alumnus.Value
            };
            var alumnusParticipant = new Participant
            {
                ParticipantId = 250,
                Status = activeStatus,
                Person = person,
                ProjectId = 100
            };
            context.ParticipantStatuses.Add(activeStatus);
            context.ParticipantStatuses.Add(alumnusStatus);

            person.Participations.Add(activeParticipant);
            person.Participations.Add(alumnusParticipant);

            person.Participations = person.Participations.OrderBy(x => x.ParticipantStatusId).ToList();

            context.People.Add(person);
            context.Participants.Add(activeParticipant);
            context.Participants.Add(alumnusParticipant);
            context.Genders.Add(person.Gender);

            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(activeStatus.Status, result.CurrentStatus);
            Assert.AreEqual(activeParticipant.ParticipantId, result.ParticipantId);
            Assert.AreEqual(activeParticipant.ProjectId, result.ProjectId);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_DoesNotHaveCurrentParticipation()
        {
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
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
                },
                PersonType = ptype
            };
            context.People.Add(person);
            context.Genders.Add(person.Gender);
            var result = PersonQueries.CreateGetSimplePersonDTOsQuery(context).First();
            Assert.AreEqual(PersonQueries.UNKNOWN_PARTICIPANT_STATUS, result.CurrentStatus);
            Assert.IsNull(result.ParticipantId);
            Assert.IsNull(result.ProjectId);
        }

        [TestMethod]
        public void TestCreateGetSimplePersonDTOsQuery_CheckNames()
        {
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
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
                },
                PersonType = ptype
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
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
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
                PersonType = ptype
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
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
            var person = new Person
            {
                IsDateOfBirthUnknown = true,
                FullName = "fullname",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                },
                PersonType = ptype
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
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
            var person = new Person
            {

                FullName = "fullname",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                },
                PersonType = ptype,
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
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
            var person = new Person
            {
                FullName = "fullname",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                },
                IsPlaceOfBirthUnknown = true,
                PersonType = ptype
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
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
            var person = new Person
            {

                FullName = "fullname",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                },
                PersonType = ptype,
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
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
            var person = new Person
            {
                FullName = "fullname",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                },
                PersonType = ptype
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
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
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
                },
                PersonType = ptype
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
            var ptype = new PersonType
            {
                IsDependentPersonType = false,
                PersonTypeId = PersonType.Participant.Id
            };
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
                },
                PersonType = ptype
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
