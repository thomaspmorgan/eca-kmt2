using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Service.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Data;
using ECA.Business.Sevis.Model;
using Microsoft.QualityTools.Testing.Fakes;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class DependentTest
    {
        [TestMethod]
        public void TestIsSpousalDependent_IsSpouse()
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
            var isDeleted = true;

            var birthDate = DateTime.UtcNow;
            var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                );
            Assert.IsTrue(instance.IsSpousalDependent());
            Assert.IsFalse(instance.IsChildDependent());
        }

        [TestMethod]
        public void TestIsSpousalDependent_IsChild()
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
            var relationship = DependentCodeType.Item02.ToString();
            var isTravelingWithParticipant = true;
            var isDeleted = true;

            var birthDate = DateTime.UtcNow;
            var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                );
            Assert.IsFalse(instance.IsSpousalDependent());
            Assert.IsTrue(instance.IsChildDependent());
        }

        [TestMethod]
        public void TestIsSpousalDependent_RelationshipIsNull()
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
            string relationship = null;
            var isTravelingWithParticipant = true;
            var isDeleted = true;

            var birthDate = DateTime.UtcNow;
            var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                );
            Assert.IsFalse(instance.IsSpousalDependent());
        }

        [TestMethod]
        public void TestIsSpousalDependent_RelationshipIsEmpty()
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
            string relationship = String.Empty;
            var isTravelingWithParticipant = true;
            var isDeleted = true;

            var birthDate = DateTime.UtcNow;
            var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                );
            Assert.IsFalse(instance.IsSpousalDependent());
        }

        [TestMethod]
        public void TestIsSpousalDependent_RelationshipIsWhitespace()
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
            string relationship = " ";
            var isTravelingWithParticipant = true;
            var isDeleted = true;

            var birthDate = DateTime.UtcNow;
            var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                );
            Assert.IsFalse(instance.IsSpousalDependent());
        }

        [TestMethod]
        public void TestIsChildDependent_IsChild()
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
            var relationship = DependentCodeType.Item02.ToString();
            var isTravelingWithParticipant = true;
            var isDeleted = true;

            var birthDate = DateTime.UtcNow;
            var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                );
            Assert.IsFalse(instance.IsSpousalDependent());
            Assert.IsTrue(instance.IsChildDependent());
        }

        [TestMethod]
        public void TestIsChildDependent_IsSpouse()
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
            var isDeleted = true;

            var birthDate = DateTime.UtcNow;
            var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                );
            Assert.IsTrue(instance.IsSpousalDependent());
            Assert.IsFalse(instance.IsChildDependent());
        }

        [TestMethod]
        public void TestIsChildDependent_RelationshipIsNull()
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
            string relationship = null;
            var isTravelingWithParticipant = true;
            var isDeleted = true;

            var birthDate = DateTime.UtcNow;
            var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                );
            Assert.IsFalse(instance.IsChildDependent());
        }

        [TestMethod]
        public void TestIsChildDependent_RelationshipIsEmpty()
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
            string relationship = String.Empty;
            var isTravelingWithParticipant = true;
            var isDeleted = true;

            var birthDate = DateTime.UtcNow;
            var instance = new AddedDependent(
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
                isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                );
            Assert.IsFalse(instance.IsChildDependent());
        }

        [TestMethod]
        public void TestIsChildDependent_RelationshipIsWhitespace()
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
            string relationship = " ";
            var isTravelingWithParticipant = true;
            var isDeleted = true;

            var birthDate = DateTime.UtcNow;
            var instance = new AddedDependent(
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
                isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                );
            Assert.IsFalse(instance.IsChildDependent());
        }

        [TestMethod]
        public void TestGetAge_BornToday()
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
            var isDeleted = true;

            var birthDate = DateTime.UtcNow;
            var instance = new AddedDependent(
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
                isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                );
            Assert.AreEqual(0, instance.GetAge());
        }

        [TestMethod]
        public void TestGetAge_BornExactlyOneYearBefore()
        {
            using (ShimsContext.Create())
            {
                var now = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var birthDate = new DateTime(2009, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                System.Fakes.ShimDateTime.UtcNowGet = () => now;

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
                var isDeleted = true;

                var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                    );
                Assert.AreEqual(1, instance.GetAge());
            }

        }

        [TestMethod]
        public void TestGetAge_BornLastYearTomorrow()
        {
            using (ShimsContext.Create())
            {
                var now = new DateTime(2010, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var birthDate = new DateTime(2009, 1, 2, 0, 0, 0, DateTimeKind.Utc);

                System.Fakes.ShimDateTime.UtcNowGet = () => now;

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
                var isDeleted = true;

                var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                    );
                Assert.AreEqual(0, instance.GetAge());
            }

        }

        [TestMethod]
        public void TestGetAge_BornLastYearYesterday()
        {
            using (ShimsContext.Create())
            {
                var now = new DateTime(2010, 1, 2, 0, 0, 0, DateTimeKind.Utc);
                var birthDate = new DateTime(2009, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                System.Fakes.ShimDateTime.UtcNowGet = () => now;

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
                var isDeleted = true;

                var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                    );
                Assert.AreEqual(1, instance.GetAge());
            }
        }

        [TestMethod]
        public void TestGetAge_BornOnLeapDayOneYearBefore_IsNowFeb28()
        {
            using (ShimsContext.Create())
            {
                var now = new DateTime(2013, 2, 28, 0, 0, 0, DateTimeKind.Utc);
                var birthDate = new DateTime(2012, 2, 29, 0, 0, 0, DateTimeKind.Utc);

                System.Fakes.ShimDateTime.UtcNowGet = () => now;

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
                var isDeleted = true;

                var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                    );
                Assert.AreEqual(0, instance.GetAge());
            }
        }

        [TestMethod]
        public void TestGetAge_BornOnLeapDayOneYearBefore_IsNowFeb27()
        {
            using (ShimsContext.Create())
            {
                var now = new DateTime(2013, 2, 27, 0, 0, 0, DateTimeKind.Utc);
                var birthDate = new DateTime(2012, 2, 29, 0, 0, 0, DateTimeKind.Utc);

                System.Fakes.ShimDateTime.UtcNowGet = () => now;

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
                var isDeleted = true;

                var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                    );
                Assert.AreEqual(0, instance.GetAge());
            }
        }

        [TestMethod]
        public void TestGetAge_BornOnLeapDayOneYearBefore_IsNowMarch1()
        {
            using (ShimsContext.Create())
            {
                var now = new DateTime(2013, 3, 1, 0, 0, 0, DateTimeKind.Utc);
                var birthDate = new DateTime(2012, 2, 29, 0, 0, 0, DateTimeKind.Utc);

                System.Fakes.ShimDateTime.UtcNowGet = () => now;

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
                var isDeleted = true;

                var instance = new AddedDependent(
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                    );
                Assert.AreEqual(1, instance.GetAge());
            }
        }

        [TestMethod]
        public void TestGetAge_BirthDateIsNull()
        {
            using (ShimsContext.Create())
            {
                var now = new DateTime(2013, 3, 1, 0, 0, 0, DateTimeKind.Utc);
                System.Fakes.ShimDateTime.UtcNowGet = () => now;

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
                var isDeleted = true;

                var instance = new AddedDependent(
                    fullName: fullName,
                    birthCity: birthCity,
                    birthCountryCode: birthCountryCode,
                    birthCountryReasonCode: birthCountryReasonCode,
                    birthDate: null,
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                    );
                Assert.AreEqual(-1, instance.GetAge());
            }
        }

        [TestMethod]
        public void TestShouldValidate()
        {
            using (ShimsContext.Create())
            {
                var now = new DateTime(2013, 3, 1, 0, 0, 0, DateTimeKind.Utc);
                System.Fakes.ShimDateTime.UtcNowGet = () => now;

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
                var isDeleted = true;

                var instance = new AddedDependent(
                    fullName: fullName,
                    birthCity: birthCity,
                    birthCountryCode: birthCountryCode,
                    birthCountryReasonCode: birthCountryReasonCode,
                    birthDate: null,
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
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    isDeleted: isDeleted
                    );
                Assert.AreEqual(!instance.IgnoreDependentValidation(), instance.ShouldValidate());
            }
        }

        #region GetChangeDetail
        [TestMethod]
        public void TestGetChangeDetail_SameInstance()
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
            var isDeleted = true;

            var instance = new AddedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReasonCode: birthCountryReasonCode,
                birthDate: null,
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
                isTravelingWithParticipant: isTravelingWithParticipant,
                isDeleted: isDeleted
                );

            var changeDetail = instance.GetChangeDetail(instance);
            Assert.IsFalse(changeDetail.HasChanges());
        }

        [TestMethod]
        public void TestGetChangeDetail_HasChange()
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
            var isDeleted = true;

            var instance = new AddedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReasonCode: birthCountryReasonCode,
                birthDate: null,
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
                isTravelingWithParticipant: isTravelingWithParticipant,
                isDeleted: isDeleted
                );

            var otherInstace = new AddedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReasonCode: birthCountryReasonCode,
                birthDate: null,
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
                isTravelingWithParticipant: !isTravelingWithParticipant,
                isDeleted: isDeleted
                );

            var changeDetail = instance.GetChangeDetail(otherInstace);
            Assert.IsTrue(changeDetail.HasChanges());
        }

        [TestMethod]
        public void TestGetChangeDetail_HasChildChange()
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
            var isDeleted = true;

            var instance = new AddedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReasonCode: birthCountryReasonCode,
                birthDate: null,
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
                isTravelingWithParticipant: isTravelingWithParticipant,
                isDeleted: isDeleted
                );


            var otherUSAddress = new AddressDTO
            {
                AddressId = 2,
                Country = "country"
            };
            var otherInstace = new AddedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReasonCode: birthCountryReasonCode,
                birthDate: null,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                relationship: relationship,
                mailAddress: mailAddress,
                usAddress: otherUSAddress,
                printForm: printForm,
                participantId: participantId,
                personId: personId,
                isTravelingWithParticipant: isTravelingWithParticipant,
                isDeleted: isDeleted
                );

            var changeDetail = instance.GetChangeDetail(otherInstace);
            Assert.IsTrue(changeDetail.HasChanges());
        }
        #endregion
    }
}
