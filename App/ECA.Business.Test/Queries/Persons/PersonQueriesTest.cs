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
