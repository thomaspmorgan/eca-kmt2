using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Persons;

namespace ECA.Business.Test.Queries.Persons
{
    [TestClass]
    public class ExchangeVisitorQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInitialize()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void CreateGetBiographicalDataByPersonIdQuery_CheckProperties()
        {
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                LocationIso2 = "iso2"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth"
            };
            var cityOfBirth = new Location
            {
                LocationId = 55,
                LocationName = "city of birth",
                Country = countryOfBirth,
                CountryId = countryOfBirth.LocationId
            };
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "gender",
                SevisGenderCode = "sevis code"
            };
            var person = new Person
            {
                PersonId = 100,
                FullName = "full name",
                Alias = "alias",
                FirstName = "first name",
                LastName = "last name",
                NameSuffix = "suffix",
                Gender = gender,
                GenderId = gender.GenderId,
                PlaceOfBirth = cityOfBirth,
                PlaceOfBirthId = cityOfBirth.LocationId
            };
            person.CountriesOfCitizenship.Add(countryOfCitizenship);

            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                LocationIso2 = "address country iso"
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var hostInstitutionAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
            };

            var participant = new Participant
            {
                ParticipantId = 10,
                PersonId = person.PersonId,
                Person = person,
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
                HostInstitutionAddress = hostInstitutionAddress,
                HostInstitutionAddressId = hostInstitutionAddress.AddressId
            };
            participant.ParticipantPerson = participantPerson;
            var emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = 2,
                EmailAddressTypeName = "email address Type"
            };
            var email = new EmailAddress
            {
                EmailAddressId = 250,
                Address = "someone@isp.com",
                Person = person,
                PersonId = person.PersonId,
                EmailAddressTypeId = emailAddressType.EmailAddressTypeId,
                EmailAddressType = emailAddressType
            };
            person.EmailAddresses.Add(email);

            context.EmailAddressTypes.Add(emailAddressType);
            context.EmailAddresses.Add(email);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Locations.Add(countryOfCitizenship);
            context.Locations.Add(cityOfBirth);
            context.Locations.Add(countryOfBirth);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(hostInstitutionAddress);
            context.Locations.Add(addressCountry);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(context, participant.ParticipantId).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.AreEqual(person.Alias, biography.FullName.PreferredName);
            Assert.AreEqual(person.FirstName, biography.FullName.FirstName);
            Assert.AreEqual(person.LastName, biography.FullName.LastName);
            Assert.AreEqual(person.NameSuffix, biography.FullName.Suffix);
            Assert.IsNull(biography.FullName.PassportName);

            Assert.AreEqual(email.Address, biography.EmailAddress);

            Assert.AreEqual(countryOfCitizenship.LocationIso2, biography.CitizenshipCountryCode);

            Assert.AreEqual(cityOfBirth.LocationName, biography.BirthCity);
            Assert.AreEqual(countryOfBirth.LocationIso2, biography.BirthCountryCode);
            Assert.IsNull(biography.BirthCountryReason);

            Assert.IsNull(biography.ResidentialAddress);
        }

        [TestMethod]
        public void CreateGetBiographicalDataByPersonIdQuery_AllRelationshipsNull()
        {
            var person = new Person
            {
                PersonId = 100,
                FullName = "full name",
                Alias = "alias",
                FirstName = "first name",
                LastName = "last name",
                NameSuffix = "suffix",
            };
            
            var participant = new Participant
            {
                ParticipantId = 10,
                PersonId = person.PersonId,
                Person = person,
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
            };
            participant.ParticipantPerson = participantPerson;

            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.People.Add(person);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(context, participant.ParticipantId).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.IsNull(biography.FullName.PassportName);
            Assert.IsNull(biography.BirthDate);
            Assert.IsNull(biography.BirthCity);
            Assert.IsNull(biography.BirthCountryCode);
            Assert.IsNull(biography.CitizenshipCountryCode);
            Assert.IsNull(biography.BirthCountryReason);
            Assert.IsNull(biography.EmailAddress);
            Assert.IsNull(biography.PermanentResidenceCountryCode);
            Assert.IsNull(biography.ResidentialAddress);
        }
    }
}
