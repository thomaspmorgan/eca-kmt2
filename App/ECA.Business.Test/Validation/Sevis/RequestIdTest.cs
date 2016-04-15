using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Business.Validation.Sevis;
using ECA.Data;
using ECA.Business.Service.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Sevis.Model;
using FluentAssertions;

namespace ECA.Business.Test.Validation
{
    [TestClass]
    public class RequestIdTest
    {
        public AddressDTO GetSOAAsAddressDTO()
        {
            var stateName = "TN";
            return new AddressDTO
            {
                Street1 = "street 1",
                Street2 = "street 2",
                City = "city",
                Division = stateName,
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME,
                LocationName = "location name",
                PostalCode = "postal code",
            };
        }

        public Business.Validation.Sevis.Bio.Person GetPerson(bool setMailAddress = true, bool setUSAddress = true)
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "123-456-7890";
            short positionCode = 120;
            var printForm = true;
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);

            var person = new Business.Validation.Sevis.Bio.Person(
                fullName,
                birthCity,
                birthCountryCode,
                birthDate,
                citizenshipCountryCode,
                email,
                gender,
                permanentResidenceCountryCode,
                phone,
                remarks,
                positionCode.ToString(),
                programCataegoryCode,
                subjectField,
                setMailAddress ? mailAddress : null,
                setUSAddress ? usAddress : null,
                printForm,
                personId,
                participantId);
            return person;
        }

        public FinancialInfo GetFinancialInfo()
        {
            var other = new Other(null, null);
            var usGovt = new USGovernmentFunding(null, null, null, null, null, null);
            var international = new InternationalFunding(null, null, null, null, null, null);
            var otherFunds = new OtherFunds(null, null, null, usGovt, international, other);

            var programSponsorFunds = "100";
            var receivedUsGovtFunds = true;
            var printForm = true;
            var financialInfo = new FinancialInfo(
                    printForm: printForm,
                    receivedUSGovtFunds: receivedUsGovtFunds,
                    programSponsorFunds: programSponsorFunds,
                    otherFunds: otherFunds);
            return financialInfo;
        }

        [TestMethod]
        public void TestConstructor_Person()
        {
            var person = GetPerson();
            var requestId = new RequestId(person);
            Assert.AreEqual(person.ParticipantId, requestId.Id);
            Assert.AreEqual(RequestIdType.Participant, requestId.RequestIdType);

            var idString = requestId.ToString();
            Assert.IsTrue(idString.Contains(person.ParticipantId.ToString()));

            var testInstance = new RequestId(idString);
            Assert.AreEqual(person.ParticipantId, testInstance.Id);
            Assert.AreEqual(RequestIdType.Participant, testInstance.RequestIdType);
        }

        [TestMethod]
        public void TestConstructor_Dependent()
        {   
            var financialInfo = GetFinancialInfo();
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "123-456-7890";
            var mailAddress = new AddressDTO
            {
                AddressId = 1,
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME
            };
            var usAddress = new AddressDTO
            {
                AddressId = 2,
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME
            };
            var printForm = true;
            var birthCountryReasonCode = USBornReasonType.Item01.ToString();
            var relationship = DependentCodeType.Item01.ToString();
            var isTravelingWithParticipant = true;
            var isDeleted = false;
            var addedDependent = new AddedDependent(
                fullName,
                birthCity,
                birthCountryCode,
                birthCountryReasonCode,
                birthDate,
                citizenshipCountryCode,
                email,
                gender,
                permanentResidenceCountryCode,
                phone,
                relationship,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId,
                isTravelingWithParticipant,
                isDeleted: isDeleted
                );
            var requestId = new RequestId(addedDependent);
            Assert.AreEqual(addedDependent.PersonId, requestId.Id);
            Assert.AreEqual(RequestIdType.Dependent, requestId.RequestIdType);

            var idString = requestId.ToString();
            Assert.IsTrue(idString.Contains(addedDependent.PersonId.ToString()));

            var testInstance = new RequestId(idString);
            Assert.AreEqual(addedDependent.PersonId, testInstance.Id);
            Assert.AreEqual(RequestIdType.Dependent, testInstance.RequestIdType);
        }

        [TestMethod]
        public void TestConstructor_PersonFinancialInfo()
        {
            var financialInfo = GetFinancialInfo();
            var person = GetPerson();

            var requestId = new RequestId(person, financialInfo);
            var idString = requestId.ToString();
            Assert.IsTrue(idString.Contains(person.ParticipantId.ToString()));

            var testInstance = new RequestId(idString);
            Assert.AreEqual(person.ParticipantId, testInstance.Id);
            Assert.AreEqual(RequestIdType.FinancialInfo, testInstance.RequestIdType);
        }

        [TestMethod]
        public void TestConstructor_PersonSubjectField()
        {   
            var person = GetPerson();
            var requestId = new RequestId(person, person.SubjectField);
            var idString = requestId.ToString();
            Assert.IsTrue(idString.Contains(person.ParticipantId.ToString()));

            var testInstance = new RequestId(idString);
            Assert.AreEqual(person.ParticipantId, testInstance.Id);
            Assert.AreEqual(RequestIdType.SubjectField, testInstance.RequestIdType);
        }

        [TestMethod]
        public void TestConstructor_String_InvalidRequestIdString()
        {
            var idString = "hello world";
            Action a = () => new RequestId(idString);
            a.ShouldThrow<NotSupportedException>().WithMessage("The request id string is not a valid request id.");
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var person = GetPerson();
            var requestId = new RequestId(person);
            Assert.AreNotEqual(0, requestId.GetHashCode());
        }

        [TestMethod]
        public void TestEquals_NullObject()
        {
            var person = GetPerson();
            var requestId = new RequestId(person);
            Assert.IsFalse(requestId.Equals(null));
        }

        [TestMethod]
        public void TestEquals_DifferentObjectType()
        {
            var person = GetPerson();
            var requestId = new RequestId(person);
            Assert.IsFalse(requestId.Equals(1));
        }

        [TestMethod]
        public void TestEquals_SameRequestIdDifferentInstance()
        {
            var person = GetPerson();
            var requestId = new RequestId(person);
            var otherRequestId = new RequestId(person);
            Assert.IsTrue(requestId.Equals(otherRequestId));
        }

        [TestMethod]
        public void TestEquals_SameInstance()
        {
            var person = GetPerson();
            var requestId = new RequestId(person);
            Assert.IsTrue(requestId.Equals(requestId));
        }

        [TestMethod]
        public void TestEquals_DifferentRequestId()
        {
            var person = GetPerson();
            var requestId = new RequestId(person);
            var otherRequestId = new RequestId(person, person.SubjectField);
            Assert.IsFalse(requestId.Equals(otherRequestId));
        }
    }
}
