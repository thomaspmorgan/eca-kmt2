﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Persons;
using ECA.Business.Validation.Model;
using System.Collections.Generic;
using ECA.Business.Validation.Model.Shared;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Service.Admin;

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

        #region Dependent
        [TestMethod]
        public void TestCreateGetParticipantDependentsBiographicalQuery()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                BirthCountry = sevisCountryOfCitizenship,
                BirthCountryId = sevisCountryOfCitizenship.BirthCountryId
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
            };
            var cityOfBirth = new Location
            {
                LocationId = 55,
                LocationName = "city of birth",
                Country = countryOfBirth,
                CountryId = countryOfBirth.LocationId,
                LocationTypeId = LocationType.City.Id
            };
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "gender",
                SevisGenderCode = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE
            };
            var dependent = new Person
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
            dependent.CountriesOfCitizenship.Add(countryOfCitizenship);
            

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = dependent,
                PersonId = dependent.PersonId
            };
            dependent.Addresses.Add(residenceAddress);
            
            var emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = 2,
                EmailAddressTypeName = "email address Type"
            };
            var email = new EmailAddress
            {
                EmailAddressId = 250,
                Address = "someone@isp.com",
                Person = dependent,
                PersonId = dependent.PersonId,
                EmailAddressTypeId = emailAddressType.EmailAddressTypeId,
                EmailAddressType = emailAddressType
            };
            dependent.EmailAddresses.Add(email);
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = 97,
                PhoneNumberTypeName = "phone number type"
            };
            var phoneNumber = new PhoneNumber
            {
                PhoneNumberId = 8562,
                Person = dependent,
                PersonId = dependent.PersonId,
                Number = "123-456-7890",
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId
            };

            var participatingPerson = new Person
            {
                PersonId = dependent.PersonId + 1,
            };
            participatingPerson.Family.Add(dependent);
            dependent.Family.Add(participatingPerson);
            var participant = new Participant
            {
                ParticipantId = 10,
                PersonId = participatingPerson.PersonId,
                Person = participatingPerson,
            };
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = participant.ParticipantId,
                Participant = participant,
            };
            participant.ParticipantPerson = participantPerson;


            dependent.PhoneNumbers.Add(phoneNumber);
            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.PhoneNumbers.Add(phoneNumber);
            context.PhoneNumberTypes.Add(phoneNumberType);
            context.EmailAddressTypes.Add(emailAddressType);
            context.EmailAddresses.Add(email);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            context.People.Add(dependent);
            context.Locations.Add(countryOfCitizenship);
            context.Locations.Add(cityOfBirth);
            context.Locations.Add(countryOfBirth);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);
            
            var result = ExchangeVisitorQueries.CreateGetParticipantDependentsBiographicalQuery(context, participant.ParticipantId).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.AreEqual(dependent.CountriesOfCitizenship.Count(), biography.NumberOfCitizenships);
            Assert.AreEqual(dependent.Alias, biography.FullName.PreferredName);
            Assert.AreEqual(dependent.FirstName, biography.FullName.FirstName);
            Assert.AreEqual(dependent.LastName, biography.FullName.LastName);
            Assert.AreEqual(dependent.NameSuffix, biography.FullName.Suffix);
            Assert.IsNull(biography.FullName.PassportName);
            Assert.AreEqual(dependent.PersonId, biography.PersonId);
            Assert.AreEqual(residenceAddress.AddressId, biography.AddressId);
            Assert.AreEqual(email.EmailAddressId, biography.EmailAddressId);
            Assert.AreEqual(gender.GenderId, biography.GenderId);
            Assert.AreEqual(gender.SevisGenderCode, biography.Gender);
            Assert.AreEqual(phoneNumber.PhoneNumberId, biography.PhoneNumberId);
            Assert.AreEqual(phoneNumber.Number, biography.PhoneNumber);

            Assert.AreEqual(email.Address, biography.EmailAddress);

            Assert.AreEqual(sevisCountryOfCitizenship.CountryCode, biography.CitizenshipCountryCode);

            Assert.AreEqual(cityOfBirth.LocationName, biography.BirthCity);
            Assert.AreEqual(sevisBirthCountry.CountryCode, biography.BirthCountryCode);
            Assert.AreEqual(sevisResidenceCountry.CountryCode, biography.PermanentResidenceCountryCode);
            Assert.IsNull(biography.BirthCountryReason);
            Assert.IsNull(biography.Relationship);
        }

        [TestMethod]
        public void TestCreateGetParticipantDependentsBiographicalQuery_ParticipantDoesNotHaveDependents()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                BirthCountry = sevisCountryOfCitizenship,
                BirthCountryId = sevisCountryOfCitizenship.BirthCountryId
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
            };
            var cityOfBirth = new Location
            {
                LocationId = 55,
                LocationName = "city of birth",
                Country = countryOfBirth,
                CountryId = countryOfBirth.LocationId,
                LocationTypeId = LocationType.City.Id
            };
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "gender",
                SevisGenderCode = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE
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

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
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
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = 97,
                PhoneNumberTypeName = "phone number type"
            };
            var phoneNumber = new PhoneNumber
            {
                PhoneNumberId = 8562,
                Person = person,
                PersonId = person.PersonId,
                Number = "123-456-7890",
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId

            };
            person.PhoneNumbers.Add(phoneNumber);
            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.PhoneNumbers.Add(phoneNumber);
            context.PhoneNumberTypes.Add(phoneNumberType);
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
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);

            var result = ExchangeVisitorQueries.CreateGetParticipantDependentsBiographicalQuery(context, participant.ParticipantId).ToList();
            Assert.AreEqual(0, result.Count);
            
            var sanityCheckResult = ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(context, participant.ParticipantId).ToList();
            Assert.AreEqual(1, sanityCheckResult.Count);
        }

        #endregion

        #region TestCreateGetBiographicalDataByParticipantIdQuery

        [TestMethod]
        public void TestCreateGetBiographicalDataByParticipantIdQuery_ParticipantDoesNotExist()
        {
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                LocationIso2 = "iso2"
            };
            var sevisCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountry = sevisCountry,
                BirthCountryId = sevisCountry.BirthCountryId
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
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Locations.Add(countryOfCitizenship);
            context.Locations.Add(cityOfBirth);
            context.Locations.Add(countryOfBirth);
            context.BirthCountries.Add(sevisCountry);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(context, participant.ParticipantId).ToList();
            Assert.AreEqual(1, result.Count);

            result = ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(context, participant.ParticipantId + 1).ToList();
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataByParticipantIdQuery_CheckExistence()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                BirthCountry = sevisCountryOfCitizenship,
                BirthCountryId = sevisCountryOfCitizenship.BirthCountryId
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
            };
            var cityOfBirth = new Location
            {
                LocationId = 55,
                LocationName = "city of birth",
                Country = countryOfBirth,
                CountryId = countryOfBirth.LocationId,
                LocationTypeId = LocationType.City.Id
            };
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "gender",
                SevisGenderCode = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE
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

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
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
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = 97,
                PhoneNumberTypeName = "phone number type"
            };
            var phoneNumber = new PhoneNumber
            {
                PhoneNumberId = 8562,
                Person = person,
                PersonId = person.PersonId,
                Number = "123-456-7890",
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId

            };
            person.PhoneNumbers.Add(phoneNumber);
            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.PhoneNumbers.Add(phoneNumber);
            context.PhoneNumberTypes.Add(phoneNumberType);
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
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(context, participant.ParticipantId).ToList();
            Assert.AreEqual(1, result.Count);
        }
        #endregion

        #region CreateGetBiographicalDataQuery

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_CheckProperties()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                BirthCountry = sevisCountryOfCitizenship,
                BirthCountryId = sevisCountryOfCitizenship.BirthCountryId
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
            };
            var cityOfBirth = new Location
            {
                LocationId = 55,
                LocationName = "city of birth",
                Country = countryOfBirth,
                CountryId = countryOfBirth.LocationId,
                LocationTypeId = LocationType.City.Id
            };
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "gender",
                SevisGenderCode = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE
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

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
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
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = 97,
                PhoneNumberTypeName = "phone number type"
            };
            var phoneNumber = new PhoneNumber
            {
                PhoneNumberId = 8562,
                Person = person,
                PersonId = person.PersonId,
                Number = "123-456-7890",
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId

            };
            person.PhoneNumbers.Add(phoneNumber);
            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.PhoneNumbers.Add(phoneNumber);
            context.PhoneNumberTypes.Add(phoneNumberType);
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
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.AreEqual(person.CountriesOfCitizenship.Count(), biography.NumberOfCitizenships);
            Assert.AreEqual(person.Alias, biography.FullName.PreferredName);
            Assert.AreEqual(person.FirstName, biography.FullName.FirstName);
            Assert.AreEqual(person.LastName, biography.FullName.LastName);
            Assert.AreEqual(person.NameSuffix, biography.FullName.Suffix);
            Assert.IsNull(biography.FullName.PassportName);
            Assert.AreEqual(person.PersonId, biography.PersonId);
            Assert.AreEqual(residenceAddress.AddressId, biography.AddressId);
            Assert.AreEqual(email.EmailAddressId, biography.EmailAddressId);
            Assert.AreEqual(gender.GenderId, biography.GenderId);
            Assert.AreEqual(gender.SevisGenderCode, biography.Gender);
            Assert.AreEqual(phoneNumber.PhoneNumberId, biography.PhoneNumberId);
            Assert.AreEqual(phoneNumber.Number, biography.PhoneNumber);

            Assert.AreEqual(email.Address, biography.EmailAddress);

            Assert.AreEqual(sevisCountryOfCitizenship.CountryCode, biography.CitizenshipCountryCode);

            Assert.AreEqual(cityOfBirth.LocationName, biography.BirthCity);
            Assert.AreEqual(sevisBirthCountry.CountryCode, biography.BirthCountryCode);
            Assert.AreEqual(sevisResidenceCountry.CountryCode, biography.PermanentResidenceCountryCode);
            Assert.IsNull(biography.BirthCountryReason);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_CheckCityOfBirthNameIsTruncated()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                BirthCountry = sevisCountryOfCitizenship,
                BirthCountryId = sevisCountryOfCitizenship.BirthCountryId
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
            };
            var cityOfBirth = new Location
            {
                LocationId = 55,
                LocationName = new string('a', BiographicalValidator.CITY_MAX_LENGTH + 1),
                Country = countryOfBirth,
                CountryId = countryOfBirth.LocationId,
                LocationTypeId = LocationType.City.Id
            };
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "gender",
                SevisGenderCode = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE
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

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
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
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = 97,
                PhoneNumberTypeName = "phone number type"
            };
            var phoneNumber = new PhoneNumber
            {
                PhoneNumberId = 8562,
                Person = person,
                PersonId = person.PersonId,
                Number = "123-456-7890",
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId

            };
            person.PhoneNumbers.Add(phoneNumber);
            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.PhoneNumbers.Add(phoneNumber);
            context.PhoneNumberTypes.Add(phoneNumberType);
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
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();

            Assert.AreEqual(cityOfBirth.LocationName.Substring(0, BiographicalValidator.CITY_MAX_LENGTH), biography.BirthCity);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_CityOfBirthDoesNotHaveAName()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                BirthCountry = sevisCountryOfCitizenship,
                BirthCountryId = sevisCountryOfCitizenship.BirthCountryId
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
            };
            var cityOfBirth = new Location
            {
                LocationId = 55,
                LocationName = null,
                Country = countryOfBirth,
                CountryId = countryOfBirth.LocationId,
                LocationTypeId = LocationType.City.Id
            };
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "gender",
                SevisGenderCode = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE
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

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
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
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = 97,
                PhoneNumberTypeName = "phone number type"
            };
            var phoneNumber = new PhoneNumber
            {
                PhoneNumberId = 8562,
                Person = person,
                PersonId = person.PersonId,
                Number = "123-456-7890",
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId

            };
            person.PhoneNumbers.Add(phoneNumber);
            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.PhoneNumbers.Add(phoneNumber);
            context.PhoneNumberTypes.Add(phoneNumberType);
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
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();

            Assert.IsNull(biography.BirthCity);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_SevisMaleGenderCode()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                BirthCountry = sevisCountryOfCitizenship,
                BirthCountryId = sevisCountryOfCitizenship.BirthCountryId
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
            };
            var cityOfBirth = new Location
            {
                LocationId = 55,
                LocationName = "city of birth",
                Country = countryOfBirth,
                CountryId = countryOfBirth.LocationId,
                LocationTypeId = LocationType.City.Id
            };
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "gender",
                SevisGenderCode = Gender.SEVIS_MALE_GENDER_CODE_VALUE
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

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
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
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = 97,
                PhoneNumberTypeName = "phone number type"
            };
            var phoneNumber = new PhoneNumber
            {
                PhoneNumberId = 8562,
                Person = person,
                PersonId = person.PersonId,
                Number = "123-456-7890",
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId

            };
            person.PhoneNumbers.Add(phoneNumber);
            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.PhoneNumbers.Add(phoneNumber);
            context.PhoneNumberTypes.Add(phoneNumberType);
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
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.AreEqual(gender.GenderId, biography.GenderId);
            Assert.AreEqual(Gender.SEVIS_MALE_GENDER_CODE_VALUE, biography.Gender);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_GenderDoesNotHaveValidSevisGenderCode()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                BirthCountry = sevisCountryOfCitizenship,
                BirthCountryId = sevisCountryOfCitizenship.BirthCountryId
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
            };
            var cityOfBirth = new Location
            {
                LocationId = 55,
                LocationName = "city of birth",
                Country = countryOfBirth,
                CountryId = countryOfBirth.LocationId,
                LocationTypeId = LocationType.City.Id
            };
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "gender",
                SevisGenderCode = "U"
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

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
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
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = 97,
                PhoneNumberTypeName = "phone number type"
            };
            var phoneNumber = new PhoneNumber
            {
                PhoneNumberId = 8562,
                Person = person,
                PersonId = person.PersonId,
                Number = "123-456-7890",
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId

            };
            person.PhoneNumbers.Add(phoneNumber);
            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.PhoneNumbers.Add(phoneNumber);
            context.PhoneNumberTypes.Add(phoneNumberType);
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
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.AreEqual(gender.GenderId, biography.GenderId);
            Assert.IsNull(biography.Gender);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_PlaceOfBirthIsNotACity()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                BirthCountry = sevisCountryOfCitizenship,
                BirthCountryId = sevisCountryOfCitizenship.BirthCountryId
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
            };
            var cityOfBirth = new Location
            {
                LocationId = 55,
                LocationName = "city of birth",
                Country = countryOfBirth,
                CountryId = countryOfBirth.LocationId,
                LocationTypeId = LocationType.Address.Id
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

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
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
            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = 97,
                PhoneNumberTypeName = "phone number type"
            };
            var phoneNumber = new PhoneNumber
            {
                PhoneNumberId = 8562,
                Person = person,
                PersonId = person.PersonId,
                Number = "123-456-7890",
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId

            };
            person.PhoneNumbers.Add(phoneNumber);
            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.PhoneNumbers.Add(phoneNumber);
            context.PhoneNumberTypes.Add(phoneNumberType);
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
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.IsNull(biography.BirthCity);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_DateOfBirthIsEstimated()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                BirthCountry = sevisCountryOfCitizenship,
                BirthCountryId = sevisCountryOfCitizenship.BirthCountryId
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
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
                PlaceOfBirthId = cityOfBirth.LocationId,
                IsDateOfBirthEstimated = true
            };
            person.CountriesOfCitizenship.Add(countryOfCitizenship);

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
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

            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Locations.Add(countryOfCitizenship);
            context.Locations.Add(cityOfBirth);
            context.Locations.Add(countryOfBirth);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.IsNull(biography.BirthDate);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_DateOfBirthIsNotEstimated()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                BirthCountry = sevisCountryOfCitizenship,
                BirthCountryId = sevisCountryOfCitizenship.BirthCountryId
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
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
                PlaceOfBirthId = cityOfBirth.LocationId,
                IsDateOfBirthEstimated = false
            };
            person.CountriesOfCitizenship.Add(countryOfCitizenship);

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
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

            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Locations.Add(countryOfCitizenship);
            context.Locations.Add(cityOfBirth);
            context.Locations.Add(countryOfBirth);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.AreEqual(person.DateOfBirth, biography.BirthDate);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_NoCountriesOfCitizenship()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
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

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
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
            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Locations.Add(cityOfBirth);
            context.Locations.Add(countryOfBirth);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.AreEqual(person.CountriesOfCitizenship.Count(), biography.NumberOfCitizenships);
            Assert.IsNull(biography.CitizenshipCountryCode);
        }


        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_MoreThanOneCountriesOfCitizenship()
        {
            var sevisCountryOfCitizenship = new BirthCountry
            {
                BirthCountryId = 1000978,
                CountryCode = "country of citizenship code"
            };
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                BirthCountry = sevisCountryOfCitizenship,
                BirthCountryId = sevisCountryOfCitizenship.BirthCountryId
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
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
            person.CountriesOfCitizenship.Add(new Location());

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
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
            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Locations.Add(countryOfCitizenship);
            context.Locations.Add(cityOfBirth);
            context.Locations.Add(countryOfBirth);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(residenceAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);
            context.BirthCountries.Add(sevisCountryOfCitizenship);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.AreEqual(person.CountriesOfCitizenship.Count(), biography.NumberOfCitizenships);
            Assert.IsNull(biography.CitizenshipCountryCode);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_CheckUsesPrimaryAddress()
        {
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                LocationIso2 = "iso2"
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
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

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = "address country",
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                IsPrimary = true,
                Person = person,
                PersonId = person.PersonId
            };
            var otherAddress = new Address
            {
                AddressId = 13,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                IsPrimary = false,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
            person.Addresses.Add(otherAddress);
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

            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Locations.Add(countryOfCitizenship);
            context.Locations.Add(cityOfBirth);
            context.Locations.Add(countryOfBirth);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(residenceAddress);
            context.Addresses.Add(otherAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.AreEqual(residenceAddress.AddressId, biography.AddressId);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_CheckIsNotUSCountryAddress()
        {
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                LocationIso2 = "iso2"
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
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

            var sevisResidenceCountry = new BirthCountry
            {
                BirthCountryId = 90,
                CountryCode = "sevis country code",
                CountryName = "sevis country name"
            };
            var addressCountry = new Location
            {
                LocationId = 29,
                LocationName = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME,
                BirthCountry = sevisResidenceCountry,
                BirthCountryId = sevisResidenceCountry.BirthCountryId
            };
            var addressLocation = new Location
            {
                LocationId = 16,
                Country = addressCountry,
                CountryId = addressCountry.LocationId,
            };
            var residenceAddressType = new AddressType
            {
                AddressTypeId = 1,
                AddressName = "address type"
            };
            var residenceAddress = new Address
            {
                AddressId = 12,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                IsPrimary = true,
                Person = person,
                PersonId = person.PersonId
            };
            var otherAddress = new Address
            {
                AddressId = 13,
                LocationId = addressLocation.LocationId,
                Location = addressLocation,
                AddressTypeId = residenceAddressType.AddressTypeId,
                AddressType = residenceAddressType,
                IsPrimary = false,
                Person = person,
                PersonId = person.PersonId
            };
            person.Addresses.Add(residenceAddress);
            person.Addresses.Add(otherAddress);
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

            context.AddressTypes.Add(residenceAddressType);
            context.BirthCountries.Add(sevisResidenceCountry);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Locations.Add(countryOfCitizenship);
            context.Locations.Add(cityOfBirth);
            context.Locations.Add(countryOfBirth);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(residenceAddress);
            context.Addresses.Add(otherAddress);
            context.Locations.Add(addressCountry);
            context.BirthCountries.Add(sevisBirthCountry);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.IsNull(biography.PermanentResidenceCountryCode);
            Assert.IsNull(biography.AddressId);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_NoAddressesSet()
        {
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                LocationIso2 = "iso2"
            };
            var sevisBirthCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisBirthCountry.BirthCountryId,
                BirthCountry = sevisBirthCountry
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
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Locations.Add(countryOfCitizenship);
            context.Locations.Add(cityOfBirth);
            context.Locations.Add(countryOfBirth);
            context.BirthCountries.Add(sevisBirthCountry);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.IsFalse(biography.AddressId.HasValue);
            Assert.IsNull(biography.PermanentResidenceCountryCode);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_CheckUsesPrimaryEmailAndPhone()
        {
            var countryOfCitizenship = new Location
            {
                LocationId = 87,
                LocationName = "citizenship",
                LocationIso2 = "iso2"
            };
            var sevisCountry = new BirthCountry
            {
                BirthCountryId = 698,
                CountryCode = "birth country code"
            };
            var countryOfBirth = new Location
            {
                LocationId = 42,
                LocationName = "country of birth",
                BirthCountryId = sevisCountry.BirthCountryId,
                BirthCountry = sevisCountry
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
            var emailAddressType = new EmailAddressType
            {
                EmailAddressTypeId = 2,
                EmailAddressTypeName = "email address Type"
            };
            var primaryEmail = new EmailAddress
            {
                EmailAddressId = 250,
                Address = "someone@isp.com",
                Person = person,
                PersonId = person.PersonId,
                EmailAddressTypeId = emailAddressType.EmailAddressTypeId,
                EmailAddressType = emailAddressType,
                IsPrimary = true
            };
            var otherEmail = new EmailAddress
            {
                EmailAddressId = 86872,
                Address = "nonprimary@someone@isp.com",
                Person = person,
                PersonId = person.PersonId,
                EmailAddressTypeId = emailAddressType.EmailAddressTypeId,
                EmailAddressType = emailAddressType,
                IsPrimary = false
            };
            person.EmailAddresses.Add(otherEmail);
            person.EmailAddresses.Add(primaryEmail);

            var phoneNumberType = new PhoneNumberType
            {
                PhoneNumberTypeId = 97,
                PhoneNumberTypeName = "phone number type"
            };
            var primaryPhone = new PhoneNumber
            {
                PhoneNumberId = 8562,
                Person = person,
                PersonId = person.PersonId,
                Number = "123-456-7890",
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId,
                IsPrimary = true

            };
            var otherPhone = new PhoneNumber
            {
                PhoneNumberId = 8532,
                Person = person,
                PersonId = person.PersonId,
                Number = "555-555-5555",
                PhoneNumberType = phoneNumberType,
                PhoneNumberTypeId = phoneNumberType.PhoneNumberTypeId,
                IsPrimary = false

            };
            person.PhoneNumbers.Add(otherPhone);
            person.PhoneNumbers.Add(primaryPhone);

            context.PhoneNumbers.Add(otherPhone);
            context.PhoneNumbers.Add(primaryPhone);
            context.PhoneNumberTypes.Add(phoneNumberType);
            context.EmailAddressTypes.Add(emailAddressType);
            context.EmailAddresses.Add(primaryEmail);
            context.EmailAddresses.Add(otherEmail);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Genders.Add(gender);
            context.People.Add(person);
            context.Locations.Add(countryOfCitizenship);
            context.Locations.Add(cityOfBirth);
            context.Locations.Add(countryOfBirth);
            context.BirthCountries.Add(sevisCountry);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.AreEqual(primaryPhone.Number, biography.PhoneNumber);
            Assert.AreEqual(primaryEmail.Address, biography.EmailAddress);
            Assert.AreEqual(primaryPhone.PhoneNumberId, biography.PhoneNumberId);
            Assert.AreEqual(primaryEmail.EmailAddressId, biography.EmailAddressId);
        }

        [TestMethod]
        public void TestCreateGetBiographicalDataQuery_AllRelationshipsNull()
        {
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
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
                GenderId = gender.GenderId
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

            context.Genders.Add(gender);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.People.Add(person);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataQuery(context).ToList();
            Assert.AreEqual(1, result.Count);
            var biography = result.First();
            Assert.AreEqual(0, biography.NumberOfCitizenships);
            Assert.IsNull(biography.FullName.PassportName);
            Assert.IsNull(biography.BirthDate);
            Assert.IsNull(biography.BirthCity);
            Assert.IsNull(biography.BirthCountryCode);
            Assert.IsNull(biography.CitizenshipCountryCode);
            Assert.IsNull(biography.BirthCountryReason);
            Assert.IsNull(biography.EmailAddress);
            Assert.IsNull(biography.PermanentResidenceCountryCode);
            Assert.IsFalse(biography.PhoneNumberId.HasValue);
            Assert.IsFalse(biography.EmailAddressId.HasValue);
            Assert.IsFalse(biography.AddressId.HasValue);
        }

        #endregion

        #region CreateGetSubjectFieldByParticipantIdQuery
        [TestMethod]
        public void TestCreateGetSubjectFieldByParticipantIdQuery_CheckProperties()
        {
            var fieldOfStudy = new FieldOfStudy
            {
                Description = "desc",
                FieldOfStudyCode = "code",
                FieldOfStudyId = 2,
            };
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var exchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                FieldOfStudy = fieldOfStudy,
                FieldOfStudyId = fieldOfStudy.FieldOfStudyId
            };
            participant.ParticipantExchangeVisitor = exchangeVisitor;

            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(exchangeVisitor);
            context.FieldOfStudies.Add(fieldOfStudy);

            var result = ExchangeVisitorQueries.CreateGetSubjectFieldByParticipantIdQuery(context, participant.ParticipantId).FirstOrDefault();
            Assert.IsNotNull(result);
            Assert.AreEqual(fieldOfStudy.FieldOfStudyCode, result.SubjectFieldCode);
            Assert.AreEqual(fieldOfStudy.Description, result.Remarks);
            Assert.IsNull(result.ForeignDegreeLevel);
            Assert.IsNull(result.ForeignFieldOfStudy);
        }

        [TestMethod]
        public void TestCreateGetSubjectFieldByParticipantIdQuery_ParticipantDoesNotExist()
        {
            var fieldOfStudy = new FieldOfStudy
            {
                Description = "desc",
                FieldOfStudyCode = "code",
                FieldOfStudyId = 2,
            };
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var exchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                FieldOfStudy = fieldOfStudy,
                FieldOfStudyId = fieldOfStudy.FieldOfStudyId
            };
            participant.ParticipantExchangeVisitor = exchangeVisitor;

            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(exchangeVisitor);
            context.FieldOfStudies.Add(fieldOfStudy);

            var result = ExchangeVisitorQueries.CreateGetSubjectFieldByParticipantIdQuery(context, participant.ParticipantId).FirstOrDefault();
            Assert.IsNotNull(result);

            result = ExchangeVisitorQueries.CreateGetSubjectFieldByParticipantIdQuery(context, participant.ParticipantId + 1).FirstOrDefault();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestCreateGetSubjectFieldByParticipantIdQuery_FieldOfStudyNotSet()
        {
            var fieldOfStudy = new FieldOfStudy
            {
                Description = "desc",
                FieldOfStudyCode = "code",
                FieldOfStudyId = 2,
            };
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var exchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                FieldOfStudy = fieldOfStudy,
                FieldOfStudyId = fieldOfStudy.FieldOfStudyId
            };
            participant.ParticipantExchangeVisitor = exchangeVisitor;

            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(exchangeVisitor);
            context.FieldOfStudies.Add(fieldOfStudy);

            var result = ExchangeVisitorQueries.CreateGetSubjectFieldByParticipantIdQuery(context, participant.ParticipantId).FirstOrDefault();
            Assert.IsNotNull(result);

            exchangeVisitor.FieldOfStudy = null;
            exchangeVisitor.FieldOfStudyId = null;
            result = ExchangeVisitorQueries.CreateGetSubjectFieldByParticipantIdQuery(context, participant.ParticipantId).FirstOrDefault();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestCreateGetSubjectFieldByParticipantIdQuery_ExchangeVisitorNotSet()
        {
            var fieldOfStudy = new FieldOfStudy
            {
                Description = "desc",
                FieldOfStudyCode = "code",
                FieldOfStudyId = 2,
            };
            var participant = new Participant
            {
                ParticipantId = 1
            };
            var exchangeVisitor = new ParticipantExchangeVisitor
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                FieldOfStudy = fieldOfStudy,
                FieldOfStudyId = fieldOfStudy.FieldOfStudyId
            };
            participant.ParticipantExchangeVisitor = exchangeVisitor;

            context.Participants.Add(participant);
            context.ParticipantExchangeVisitors.Add(exchangeVisitor);
            context.FieldOfStudies.Add(fieldOfStudy);

            var result = ExchangeVisitorQueries.CreateGetSubjectFieldByParticipantIdQuery(context, participant.ParticipantId).FirstOrDefault();
            Assert.IsNotNull(result);

            exchangeVisitor.ParticipantId = participant.ParticipantId + 1;
            result = ExchangeVisitorQueries.CreateGetSubjectFieldByParticipantIdQuery(context, participant.ParticipantId).FirstOrDefault();
            Assert.IsNull(result);
        }
        #endregion

        #region CreateGetUsAddressByAddressId
        [TestMethod]
        public void TestCreateGetUsAddressByAddressIdQuery_CheckProperties()
        {
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var division = new Location
            {
                LocationId = 1,
                LocationName = "TN"
            };
            var country = new Location
            {
                LocationId = 2,
                LocationName = "US",
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "Nashville"
            };
            var addressLocation = new Location
            {
                LocationId = 4,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                LocationName = "address",
                LocationType = addressLocationType,
                LocationTypeId = addressLocationType.LocationTypeId,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var addressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var address = new Address
            {
                AddressId = 1,
                AddressType = addressType,
                AddressTypeId = addressType.AddressTypeId,
                IsPrimary = true,
                Location = addressLocation,
                LocationId = addressLocation.LocationId,
            };
            context.AddressTypes.Add(addressType);
            context.Locations.Add(division);
            context.Locations.Add(country);
            context.Locations.Add(city);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(address);
            context.LocationTypes.Add(addressLocationType);

            Action<USAddress> tester = (usAddress) =>
            {
                Assert.AreEqual(addressLocation.Street1, usAddress.Address1);
                Assert.AreEqual(addressLocation.Street2, usAddress.Address2);
                Assert.AreEqual(city.LocationName, usAddress.City);
                Assert.AreEqual(division.LocationName, usAddress.State);
                Assert.AreEqual(addressLocation.PostalCode, usAddress.PostalCode);
            };
            var serviceResult = ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(context, address.AddressId).FirstOrDefault();
            tester(serviceResult);
        }

        [TestMethod]
        public void TestCreateGetUsAddressByAddressIdQuery_AddressDoesNotExist()
        {
            var addressLocationType = new LocationType
            {
                LocationTypeId = LocationType.Address.Id,
                LocationTypeName = LocationType.Address.Value
            };
            var division = new Location
            {
                LocationId = 1,
                LocationName = "TN"
            };
            var country = new Location
            {
                LocationId = 2,
                LocationName = "US",
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "Nashville"
            };
            var addressLocation = new Location
            {
                LocationId = 4,
                City = city,
                CityId = city.LocationId,
                Country = country,
                CountryId = country.LocationId,
                Division = division,
                DivisionId = division.LocationId,
                LocationName = "address",
                LocationType = addressLocationType,
                LocationTypeId = addressLocationType.LocationTypeId,
                PostalCode = "12345",
                Street1 = "street1",
                Street2 = "street2",
                Street3 = "street3",
            };
            var addressType = new AddressType
            {
                AddressName = AddressType.Home.Value,
                AddressTypeId = AddressType.Home.Id
            };
            var address = new Address
            {
                AddressId = 1,
                AddressType = addressType,
                AddressTypeId = addressType.AddressTypeId,
                IsPrimary = true,
                Location = addressLocation,
                LocationId = addressLocation.LocationId,
            };
            context.AddressTypes.Add(addressType);
            context.Locations.Add(division);
            context.Locations.Add(country);
            context.Locations.Add(city);
            context.Locations.Add(addressLocation);
            context.Addresses.Add(address);
            context.LocationTypes.Add(addressLocationType);

            var serviceResult = ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(context, address.AddressId).FirstOrDefault();
            Assert.IsNotNull(serviceResult);

            serviceResult = ExchangeVisitorQueries.CreateGetUsAddressByAddressIdQuery(context, address.AddressId + 1).FirstOrDefault();
            Assert.IsNull(serviceResult);
        }
        #endregion

        #region International Funding
        [TestMethod]
        public void TestCreateGetInternationalFundingQuery_HasInternationalOrgsWithCodes()
        {

            var internationalFundingOrg1 = new InternationalOrganization
            {
                OrganizationId = 2,
                OrganizationCode = "org1code",
                Description = "org 1 desc"
            };
            var internationalFundingOrg2 = new InternationalOrganization
            {
                OrganizationId = 3,
                OrganizationCode = "org2code",
                Description = "org 2 desc"
            };
            var org1Amount = 2.2m;
            var org2Amount = 5.7m;
            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = 1,
                FundingIntlOrg1 = org1Amount,
                IntlOrg1 = internationalFundingOrg1,
                IntlOrg1Id = internationalFundingOrg1.OrganizationId,
                FundingIntlOrg2 = org2Amount,
                IntlOrg2 = internationalFundingOrg2,
                IntlOrg2Id = internationalFundingOrg2.OrganizationId,
            };
            context.InternationalOrganizations.Add(internationalFundingOrg1);
            context.InternationalOrganizations.Add(internationalFundingOrg2);
            context.ParticipantExchangeVisitors.Add(visitor);

            var result = ExchangeVisitorQueries.CreateGetInternationalFundingQuery(context, visitor.ParticipantId).FirstOrDefault();
            Assert.IsNotNull(result);
            Func<decimal?, string> getExpectedFundingStringValue = (value) =>
            {
                if (value.HasValue)
                {
                    return ((int)value.Value).ToString();
                }
                else
                {
                    return null;
                }
            };

            Assert.AreEqual(getExpectedFundingStringValue(org1Amount), result.Amount1);
            Assert.AreEqual(internationalFundingOrg1.OrganizationCode, result.Org1);
            Assert.IsNull(result.OtherName1);

            Assert.AreEqual(getExpectedFundingStringValue(org2Amount), result.Amount2);
            Assert.AreEqual(internationalFundingOrg2.OrganizationCode, result.Org2);
            Assert.IsNull(result.OtherName2);
        }

        [TestMethod]
        public void TestCreateGetInternationalFundingQuery_HasOtherInternationalOrgs()
        {

            var internationalFundingOrg1 = new InternationalOrganization
            {
                OrganizationId = 2,
                OrganizationCode = InternationalValidator.OTHER_ORG_CODE,
            };
            var internationalFundingOrg2 = new InternationalOrganization
            {
                OrganizationId = 3,
                OrganizationCode = InternationalValidator.OTHER_ORG_CODE,
            };
            var org1Amount = 2.2m;
            var org2Amount = 5.7m;
            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = 1,
                FundingIntlOrg1 = org1Amount,
                IntlOrg1 = internationalFundingOrg1,
                IntlOrg1Id = internationalFundingOrg1.OrganizationId,
                IntlOrg1OtherName = "other 1 name",
                FundingIntlOrg2 = org2Amount,
                IntlOrg2 = internationalFundingOrg2,
                IntlOrg2Id = internationalFundingOrg2.OrganizationId,
                IntlOrg2OtherName = "other 2 name"
            };
            context.InternationalOrganizations.Add(internationalFundingOrg1);
            context.InternationalOrganizations.Add(internationalFundingOrg2);
            context.ParticipantExchangeVisitors.Add(visitor);

            var result = ExchangeVisitorQueries.CreateGetInternationalFundingQuery(context, visitor.ParticipantId).FirstOrDefault();
            Assert.IsNotNull(result);
            Func<decimal?, string> getExpectedFundingStringValue = (value) =>
            {
                if (value.HasValue)
                {
                    return ((int)value.Value).ToString();
                }
                else
                {
                    return null;
                }
            };

            Assert.AreEqual(getExpectedFundingStringValue(org1Amount), result.Amount1);
            Assert.AreEqual(InternationalValidator.OTHER_ORG_CODE, result.Org1);
            Assert.AreEqual(visitor.IntlOrg1OtherName, result.OtherName1);

            Assert.AreEqual(getExpectedFundingStringValue(org2Amount), result.Amount2);
            Assert.AreEqual(InternationalValidator.OTHER_ORG_CODE, result.Org2);
            Assert.AreEqual(visitor.IntlOrg2OtherName, result.OtherName2);
        }

        [TestMethod]
        public void TestCreateGetInternationalFundingQuery_NoInternationalFunding()
        {
            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = 1,
            };
            context.ParticipantExchangeVisitors.Add(visitor);

            var result = ExchangeVisitorQueries.CreateGetInternationalFundingQuery(context, visitor.ParticipantId).FirstOrDefault();
            Assert.IsNotNull(result);

            Assert.IsNull(result.Amount1);
            Assert.IsNull(result.Org1);
            Assert.IsNull(result.OtherName1);

            Assert.IsNull(result.Amount2);
            Assert.IsNull(result.Org2);
            Assert.IsNull(result.OtherName2);
        }

        [TestMethod]
        public void TestCreateGetInternationalFundingQuery_ParticipantExchangeVisitorDoesNotExist()
        {
            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = 1,
            };
            context.ParticipantExchangeVisitors.Add(visitor);

            var result = ExchangeVisitorQueries.CreateGetInternationalFundingQuery(context, visitor.ParticipantId).FirstOrDefault();
            Assert.IsNotNull(result);

            result = ExchangeVisitorQueries.CreateGetInternationalFundingQuery(context, visitor.ParticipantId + 1).FirstOrDefault();
            Assert.IsNull(result);
        }
        #endregion

        #region US Gov Funding
        [TestMethod]
        public void TestCreateGetUSFundingQuery_HasUSGovOrgsWithCodes()
        {

            var govAgency1 = new USGovernmentAgency
            {
                AgencyId = 2,
                AgencyCode = "agency1code",
                Description = "org 1 desc"
            };
            var govAgency2 = new USGovernmentAgency
            {
                AgencyId = 3,
                AgencyCode = "agency2code",
                Description = "org 2 desc"
            };
            var org1Amount = 2.2m;
            var org2Amount = 5.7m;
            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = 1,

                FundingGovtAgency1 = org1Amount,
                GovtAgency1 = govAgency1,
                GovtAgency1Id = govAgency1.AgencyId,

                FundingGovtAgency2 = org2Amount,
                GovtAgency2 = govAgency2,
                GovtAgency2Id = govAgency2.AgencyId,
            };
            context.USGovernmentAgencies.Add(govAgency1);
            context.USGovernmentAgencies.Add(govAgency2);
            context.ParticipantExchangeVisitors.Add(visitor);

            var result = ExchangeVisitorQueries.CreateGetUSFundingQuery(context, visitor.ParticipantId).FirstOrDefault();
            Assert.IsNotNull(result);
            Func<decimal?, string> getExpectedFundingStringValue = (value) =>
            {
                if (value.HasValue)
                {
                    return ((int)value.Value).ToString();
                }
                else
                {
                    return null;
                }
            };

            Assert.AreEqual(getExpectedFundingStringValue(org1Amount), result.Amount1);
            Assert.AreEqual(govAgency1.AgencyCode, result.Org1);
            Assert.IsNull(result.OtherName1);

            Assert.AreEqual(getExpectedFundingStringValue(org2Amount), result.Amount2);
            Assert.AreEqual(govAgency2.AgencyCode, result.Org2);
            Assert.IsNull(result.OtherName2);
        }

        [TestMethod]
        public void TestCreateGetUSFundingQuery_HasOtherUSGovAgencies()
        {

            var govAgency1 = new USGovernmentAgency
            {
                AgencyId = 2,
                AgencyCode = USGovtValidator.OTHER_ORG_CODE,
            };
            var govAgency2 = new USGovernmentAgency
            {
                AgencyId = 3,
                AgencyCode = USGovtValidator.OTHER_ORG_CODE,
            };
            var org1Amount = 2.2m;
            var org2Amount = 5.7m;
            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = 1,

                FundingGovtAgency1 = org1Amount,
                GovtAgency1 = govAgency1,
                GovtAgency1Id = govAgency1.AgencyId,
                GovtAgency1OtherName = "gov agency 1 other",

                FundingGovtAgency2 = org2Amount,
                GovtAgency2 = govAgency2,
                GovtAgency2Id = govAgency2.AgencyId,
                GovtAgency2OtherName = "gov agency 2 other"
            };
            context.USGovernmentAgencies.Add(govAgency1);
            context.USGovernmentAgencies.Add(govAgency2);
            context.ParticipantExchangeVisitors.Add(visitor);

            var result = ExchangeVisitorQueries.CreateGetUSFundingQuery(context, visitor.ParticipantId).FirstOrDefault();
            Assert.IsNotNull(result);
            Func<decimal?, string> getExpectedFundingStringValue = (value) =>
            {
                if (value.HasValue)
                {
                    return ((int)value.Value).ToString();
                }
                else
                {
                    return null;
                }
            };

            Assert.AreEqual(getExpectedFundingStringValue(org1Amount), result.Amount1);
            Assert.AreEqual(USGovtValidator.OTHER_ORG_CODE, result.Org1);
            Assert.AreEqual(visitor.GovtAgency1OtherName, result.OtherName1);

            Assert.AreEqual(getExpectedFundingStringValue(org2Amount), result.Amount2);
            Assert.AreEqual(USGovtValidator.OTHER_ORG_CODE, result.Org2);
            Assert.AreEqual(visitor.GovtAgency2OtherName, result.OtherName2);

        }

        [TestMethod]
        public void TestCreateGetUSFundingQuery_NoUsGovFunding()
        {

            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = 1,
            };
            context.ParticipantExchangeVisitors.Add(visitor);

            var result = ExchangeVisitorQueries.CreateGetUSFundingQuery(context, visitor.ParticipantId).FirstOrDefault();
            Assert.IsNotNull(result);

            Assert.IsNull(result.Amount1);
            Assert.IsNull(result.Org1);
            Assert.IsNull(result.OtherName1);

            Assert.IsNull(result.Amount2);
            Assert.IsNull(result.Org2);
            Assert.IsNull(result.OtherName2);
        }

        [TestMethod]
        public void TestCreateGetUSFundingQuery_ParticipantExchangeVisitorDoesNotExist()
        {

            var visitor = new ParticipantExchangeVisitor
            {
                ParticipantId = 1,
            };
            context.ParticipantExchangeVisitors.Add(visitor);

            var result = ExchangeVisitorQueries.CreateGetUSFundingQuery(context, visitor.ParticipantId).FirstOrDefault();
            Assert.IsNotNull(result);

            result = ExchangeVisitorQueries.CreateGetUSFundingQuery(context, visitor.ParticipantId + 1).FirstOrDefault();
            Assert.IsNull(result);
        }
        #endregion
    }
}