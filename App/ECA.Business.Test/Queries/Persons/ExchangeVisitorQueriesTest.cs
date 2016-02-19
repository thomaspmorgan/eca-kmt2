using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Persons;
using ECA.Business.Validation.Model;

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

        #region CreateGetBiographicalDataByParticipantIdQuery

        [TestMethod]
        public void CreateGetBiographicalDataByPersonIdQuery_CheckProperties()
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
            context.BirthCountries.Add(sevisCountry);

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
            Assert.AreEqual(sevisCountry.CountryCode, biography.BirthCountryCode);
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

        [TestMethod]
        public void CreateGetBiographicalDataByPersonIdQuery_ParticipantDoesNotExist()
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
            context.BirthCountries.Add(sevisCountry);

            var result = ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(context, participant.ParticipantId).ToList();
            Assert.AreEqual(1, result.Count);

            result = ExchangeVisitorQueries.CreateGetBiographicalDataByParticipantIdQuery(context, participant.ParticipantId + 1).ToList();
            Assert.AreEqual(0, result.Count);
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
        public void TestCreateGetInternationalFundingQuery_HasInternationalOrgsWithoutCodes()
        {

            var internationalFundingOrg1 = new InternationalOrganization
            {
                OrganizationId = 2,
                OrganizationCode = null,
                Description = "org 1 desc"
            };
            var internationalFundingOrg2 = new InternationalOrganization
            {
                OrganizationId = 3,
                OrganizationCode = null,
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
            Assert.IsNull(result.Org1);
            Assert.AreEqual(internationalFundingOrg1.Description, result.OtherName1);

            Assert.AreEqual(getExpectedFundingStringValue(org2Amount), result.Amount2);
            Assert.IsNull(result.Org2);
            Assert.AreEqual(internationalFundingOrg2.Description, result.OtherName2);
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
        public void TestCreateGetUSFundingQuery_HasUSGovOrgsWithoutCodes()
        {

            var govAgency1 = new USGovernmentAgency
            {
                AgencyId = 2,
                AgencyCode = null,
                Description = "org 1 desc"
            };
            var govAgency2 = new USGovernmentAgency
            {
                AgencyId = 3,
                AgencyCode = null,
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
            Assert.IsNull(result.Org1);
            Assert.AreEqual(govAgency1.Description, result.OtherName1);

            Assert.AreEqual(getExpectedFundingStringValue(org2Amount), result.Amount2);
            Assert.IsNull(result.Org2);
            Assert.AreEqual(govAgency2.Description, result.OtherName2);
            
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
