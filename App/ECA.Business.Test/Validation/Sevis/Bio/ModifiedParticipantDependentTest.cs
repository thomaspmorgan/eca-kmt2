using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Sevis.Model;
using ECA.Data;
using ECA.Business.Service.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Validation.Sevis;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class ModifiedParticipantDependentTest
    {
        [TestMethod]
        public void TestConstructor()
        {
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

            var dependent = new AddedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReasonCode: birthCountryReasonCode,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                relationship: relationship,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                participantId: participantId,
                personId: personId,
                isTravelingWithParticipant: isTravelingWithParticipant
                );

            var modifiedParticipantDependent = new ModifiedParticipantDependent(dependent);

            Assert.IsTrue(Object.ReferenceEquals(dependent, modifiedParticipantDependent.Dependent));
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorDependent()
        {
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

            var dependent = new AddedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReasonCode: birthCountryReasonCode,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                relationship: relationship,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                participantId: participantId,
                personId: personId,
                isTravelingWithParticipant: isTravelingWithParticipant
                );

            var modifiedParticipantDependent = new ModifiedParticipantDependent(dependent: dependent);

            var instance = modifiedParticipantDependent.GetSEVISEVBatchTypeExchangeVisitorDependent();
            Assert.IsNotNull(instance.Item);
            Assert.IsInstanceOfType(instance.Item, typeof(SEVISEVBatchTypeExchangeVisitorDependentAdd));
            Assert.IsNotNull(instance.UserDefinedA);
            Assert.IsNotNull(instance.UserDefinedB);

            var key = new ParticipantSevisKey(instance.UserDefinedA, instance.UserDefinedB);
            Assert.AreEqual(participantId, key.ParticipantId);
            Assert.AreEqual(personId, key.PersonId);
        }
    }
}
