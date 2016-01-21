using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Persons;

namespace ECA.Business.Test.Queries.Persons
{
    [TestClass]
    public class ParticipantQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        #region CreateGetPersonParticipantsQuery
        [TestMethod]
        public void TestCreateGetPersonParticipantsQuery_CheckProperties_AddressIsNull()
        {
            var participantType = new ParticipantType
            {
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 1,
                Gender = gender,
                GenderId = gender.GenderId,
                FirstName = "first",
                LastName = "last",
                FullName = "full name"
            };

            var participant = new Participant
            {
                Person = person,
                PersonId = person.PersonId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            context.ParticipantTypes.Add(participantType);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Participants.Add(participant);

            var results = ParticipantQueries.CreateGetPersonParticipantsQuery(context);
            Assert.AreEqual(1, results.Count());
            var participantResult = results.First();

            //Assert all org properties are null
            Assert.IsFalse(participantResult.OrganizationId.HasValue);

            Assert.AreEqual(participantType.ParticipantTypeId, participantResult.ParticipantTypeId);
            Assert.AreEqual(participantType.Name, participantResult.ParticipantType);
            Assert.AreEqual(person.PersonId, participantResult.PersonId);
            Assert.AreEqual(person.FullName, participantResult.Name);
            Assert.IsNull(participantResult.Country);
            Assert.IsNull(participantResult.City);
        }

        [TestMethod]
        public void TestCreateGetPersonParticipantsQuery_CheckProperties_HasPrimaryAddress()
        {
            var participantType = new ParticipantType
            {
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 1,
                Gender = gender,
                GenderId = gender.GenderId,
                FirstName = "first",
                LastName = "last",
            };

            var participant = new Participant
            {
                Person = person,
                PersonId = person.PersonId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            var address = new Address
            {
                IsPrimary = true,
                Location = new Location
                {
                    City = new Location
                    {
                        LocationName = "city"
                    },
                    Country = new Location
                    {
                        LocationName = "country"
                    }
                }
            };
            person.Addresses.Add(address);
            address.Person = person;
            context.ParticipantTypes.Add(participantType);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Participants.Add(participant);

            var results = ParticipantQueries.CreateGetPersonParticipantsQuery(context);
            Assert.AreEqual(1, results.Count());
            var participantResult = results.First();

            //Assert all org properties are null
            Assert.IsFalse(participantResult.OrganizationId.HasValue);
            Assert.AreEqual(address.Location.City.LocationName, participantResult.City);
            Assert.AreEqual(address.Location.Country.LocationName, participantResult.Country);

        }

        [TestMethod]
        public void TestCreateGetPersonParticipantsQuery_CheckUsesPrimaryAddress()
        {
            var participantType = new ParticipantType
            {
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id,
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 1,
                Gender = gender,
                GenderId = gender.GenderId,
                FirstName = "first",
                LastName = "last",
            };

            var participant = new Participant
            {
                Person = person,
                PersonId = person.PersonId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            var address = new Address
            {
                IsPrimary = true,
                Location = new Location
                {
                    City = new Location
                    {
                        LocationName = "city"
                    },
                    Country = new Location
                    {
                        LocationName = "country"
                    }
                }
            };
            var otherAddress = new Address
            {
                IsPrimary = false,
                Location = new Location
                {
                    City = new Location
                    {
                        LocationName = "other city"
                    },
                    Country = new Location
                    {
                        LocationName = "other country"
                    }
                }
            };
            person.Addresses.Add(address);
            person.Addresses.Add(otherAddress);
            address.Person = person;
            context.ParticipantTypes.Add(participantType);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Participants.Add(participant);

            var results = ParticipantQueries.CreateGetPersonParticipantsQuery(context);
            Assert.AreEqual(1, results.Count());
            var participantResult = results.First();

            //Assert all org properties are null
            Assert.IsFalse(participantResult.OrganizationId.HasValue);
            Assert.AreEqual(address.Location.City.LocationName, participantResult.City);
            Assert.AreEqual(address.Location.Country.LocationName, participantResult.Country);

        }
        #endregion

        #region TestCreateGetParticipants

        [TestMethod]
        public void TestCreateGetOrganizationParticipantsQuery_AddressIsNull()
        {
            var participantType = new ParticipantType
            {
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
            };
            var organization = new Organization
            {
                OrganizationId = 1,
                Name = "org name"
            };

            var participant = new Participant
            {
                Organization = organization,
                OrganizationId = organization.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization);
            context.Participants.Add(participant);


            var results = ParticipantQueries.CreateGetOrganizationParticipantsQuery(context);
            Assert.AreEqual(1, results.Count());
            var participantResult = results.First();
            //Assert all org properties are null
            Assert.IsFalse(participantResult.PersonId.HasValue);
            Assert.AreEqual(participantType.ParticipantTypeId, participantResult.ParticipantTypeId);
            Assert.AreEqual(participantType.Name, participantResult.ParticipantType);
            Assert.AreEqual(organization.Name, participantResult.Name);
            Assert.IsNull(participantResult.Country);
            Assert.IsNull(participantResult.City);
        }

        [TestMethod]
        public void TestCreateGetOrganizationParticipantsQuery_HasPrimaryAddress()
        {
            var participantType = new ParticipantType
            {
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
            };
            var organization = new Organization
            {
                OrganizationId = 1,
                Name = "org name"
            };

            var participant = new Participant
            {
                Organization = organization,
                OrganizationId = organization.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            var address = new Address
            {
                IsPrimary = true,
                Location = new Location
                {
                    City = new Location
                    {
                        LocationName = "city"
                    },
                    Country = new Location
                    {
                        LocationName = "country"
                    }
                }
            };
            organization.Addresses.Add(address);
            context.Addresses.Add(address);
            context.Locations.Add(address.Location);
            context.Locations.Add(address.Location.Country);
            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization);
            context.Participants.Add(participant);


            var results = ParticipantQueries.CreateGetOrganizationParticipantsQuery(context);
            Assert.AreEqual(1, results.Count());
            var participantResult = results.First();
            //Assert all org properties are null
            Assert.IsFalse(participantResult.PersonId.HasValue);
            Assert.AreEqual(participantType.ParticipantTypeId, participantResult.ParticipantTypeId);
            Assert.AreEqual(participantType.Name, participantResult.ParticipantType);
            Assert.AreEqual(organization.Name, participantResult.Name);
            Assert.AreEqual(address.Location.City.LocationName, participantResult.City);
            Assert.AreEqual(address.Location.Country.LocationName, participantResult.Country);
        }

        [TestMethod]
        public void TestCreateGetOrganizationParticipantsQuery_CheckUsesPrimaryAddress()
        {
            var participantType = new ParticipantType
            {
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id,
            };
            var organization = new Organization
            {
                OrganizationId = 1,
                Name = "org name"
            };

            var participant = new Participant
            {
                Organization = organization,
                OrganizationId = organization.OrganizationId,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId
            };
            var address = new Address
            {
                IsPrimary = true,
                Location = new Location
                {
                    City = new Location
                    {
                        LocationName = "city"
                    },
                    Country = new Location
                    {
                        LocationName = "country"
                    }
                }
            };
            var otherAddress = new Address
            {
                IsPrimary = false,
                Location = new Location
                {
                    City = new Location
                    {
                        LocationName = "other city"
                    },
                    Country = new Location
                    {
                        LocationName = "other country"
                    }
                }
            };
            organization.Addresses.Add(address);
            organization.Addresses.Add(otherAddress);
            context.Addresses.Add(address);
            context.Locations.Add(address.Location);
            context.Locations.Add(address.Location.Country);
            context.ParticipantTypes.Add(participantType);
            context.Organizations.Add(organization);
            context.Participants.Add(participant);

            var results = ParticipantQueries.CreateGetOrganizationParticipantsQuery(context);
            Assert.AreEqual(1, results.Count());
            var participantResult = results.First();
            //Assert all org properties are null
            Assert.IsFalse(participantResult.PersonId.HasValue);
            Assert.AreEqual(participantType.ParticipantTypeId, participantResult.ParticipantTypeId);
            Assert.AreEqual(participantType.Name, participantResult.ParticipantType);
            Assert.AreEqual(organization.Name, participantResult.Name);
            Assert.AreEqual(address.Location.City.LocationName, participantResult.City);
            Assert.AreEqual(address.Location.Country.LocationName, participantResult.Country);
        }

        #endregion

        [TestMethod]
        public void TestCreateGetParticipantDTOByIdQuery()
        {
            var status = new ParticipantStatus
            {
                ParticipantStatusId = 1,
                Status = "status",
            };
            var person = new Person
            {
                PersonId = 1,
                FullName = "firstName lastName"
            };

            var history = new History
            {
                RevisedOn = DateTimeOffset.Now
            };

            var participantType = new ParticipantType
            {
                ParticipantTypeId = ParticipantType.Individual.Id,
                Name = "name",
                IsPerson = true
            };

            var participant = new Participant
            {
                ParticipantId = 1,
                PersonId = person.PersonId,
                Person = person,
                ParticipantType = participantType,
                ParticipantTypeId = participantType.ParticipantTypeId,
                History = history,
                Status = status,
                ParticipantStatusId = status.ParticipantStatusId
            };
            
            participant.Status = status;
            status.Participants.Add(participant);

            context.ParticipantStatuses.Add(status);
            context.People.Add(person);
            context.ParticipantTypes.Add(participantType);
            context.Participants.Add(participant);
            var results = ParticipantQueries.CreateGetParticipantDTOByIdQuery(context, participant.ParticipantId);
            Assert.AreEqual(1, results.Count());
            var result = results.First();
            Assert.AreEqual(participant.ParticipantId, result.ParticipantId);
            Assert.AreEqual(participant.PersonId, result.PersonId);
            Assert.IsNull(participant.OrganizationId);
            Assert.AreEqual(participant.ParticipantTypeId, result.ParticipantTypeId);
            Assert.AreEqual(participant.ParticipantType.Name, result.ParticipantType);
            Assert.AreEqual(person.FullName, result.Name);
            Assert.AreEqual(status.Status, result.ParticipantStatus);
            Assert.AreEqual(status.ParticipantStatusId, result.StatusId);
            Assert.AreEqual(history.RevisedOn, result.RevisedOn);
            Assert.AreEqual(participantType.IsPerson, result.IsPersonParticipantType);
        }

        [TestMethod]
        public void TestCreateGetParticipantDTOByIdQuery_DoesNotExist()
        {

            var results = ParticipantQueries.CreateGetParticipantDTOByIdQuery(context, 1);
            Assert.AreEqual(0, results.Count());
        }
    }
}
