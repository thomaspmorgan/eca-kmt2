using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Business.Sevis.Model;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class DependentValidatorTest
    {

        [TestMethod]
        public void TestGetPersonType()
        {
            var validator = new DependentValidator();
            Assert.AreEqual(DependentValidator.PERSON_TYPE, validator.GetPersonType(null));
        }

        [TestMethod]
        public void TestGetNameDelegate()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "street1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "street2";
            usAddress.PostalCode = "22222";

            string birthCity = "birth city";
            string birthCountryCode = "US";
            var birthCountryReasonCode = USBornReasonType.Item01.ToString();
            DateTime birthDate = DateTime.Now;
            string citizenshipCountryCode = "UK";
            string emailAddress = "email@isp.com";

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            string gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;
            string permanentResidenceCountryCode = "FR";
            string phoneNumber = "18505551212";
            string relationship = "relations";
            var isTravelingWithParticipant = true;
            Func<AddedDependent> createEntity = () =>
            {
                return new AddedDependent(
                    fullName: fullName,
                    birthCity: birthCity,
                    birthCountryCode: birthCountryCode,
                    birthCountryReasonCode: birthCountryReasonCode,
                    birthDate: birthDate,
                    citizenshipCountryCode: citizenshipCountryCode,
                    emailAddress: emailAddress,
                    gender: gender,
                    permanentResidenceCountryCode: permanentResidenceCountryCode,
                    phoneNumber: phoneNumber,
                    relationship: relationship,
                    mailAddress: mailAddress,
                    usAddress: usAddress,
                    printForm: true,
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    personId: 10,
                    participantId: 20);
            };

            var validator = new DependentValidator();
            var d = validator.GetNameDelegate();
            Assert.AreEqual(string.Format("{0} {1}", fullName.FirstName, fullName.LastName), d(createEntity()));
        }

        [TestMethod]
        public void TestRelationship_Null()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "street1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "street2";
            usAddress.PostalCode = "22222";

            string birthCity = "birth city";
            string birthCountryCode = "US";
            var birthCountryReasonCode = USBornReasonType.Item01.ToString();
            DateTime birthDate = DateTime.Now;
            string citizenshipCountryCode = "UK";
            string emailAddress = "email@isp.com";

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            string gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;
            string permanentResidenceCountryCode = "FR";
            string phoneNumber = "18505551212";
            string relationship = "relations";
            var isTravelingWithParticipant = true;
            Func<AddedDependent> createEntity = () =>
            {
                return new AddedDependent(
                    fullName: fullName,
                    birthCity: birthCity,
                    birthCountryCode: birthCountryCode,
                    birthCountryReasonCode: birthCountryReasonCode,
                    birthDate: birthDate,
                    citizenshipCountryCode: citizenshipCountryCode,
                    emailAddress: emailAddress,
                    gender: gender,
                    permanentResidenceCountryCode: permanentResidenceCountryCode,
                    phoneNumber: phoneNumber,
                    relationship: relationship,
                    mailAddress: mailAddress,
                    usAddress: usAddress,
                    printForm: true,
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    personId: 10,
                    participantId: 20);
            };

            var instance = createEntity();
            var validator = new DependentValidator();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            relationship = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual(
                String.Format(DependentValidator.DEPENDENT_RELATIONSHIP_REQUIRED, validator.GetPersonType(instance), validator.GetNameDelegate()(instance)), 
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(DependentErrorPath));
        }

        #region Birth Country Reason Code

        [TestMethod]
        public void TestBirthCountryReason_Null()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "street1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "street2";
            usAddress.PostalCode = "22222";

            string birthCity = "birth city";
            string birthCountryCode = "US";
            var birthCountryReasonCode = USBornReasonType.Item01.ToString();
            DateTime birthDate = DateTime.Now;
            string citizenshipCountryCode = "UK";
            string emailAddress = "email@isp.com";

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            string gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;
            string permanentResidenceCountryCode = "FR";
            string phoneNumber = "18505551212";
            string relationship = "relations";
            var isTravelingWithParticipant = true;
            Func<AddedDependent> createEntity = () =>
            {
                return new AddedDependent(
                    fullName: fullName,
                    birthCity: birthCity,
                    birthCountryCode: birthCountryCode,
                    birthCountryReasonCode: birthCountryReasonCode,
                    birthDate: birthDate,
                    citizenshipCountryCode: citizenshipCountryCode,
                    emailAddress: emailAddress,
                    gender: gender,
                    permanentResidenceCountryCode: permanentResidenceCountryCode,
                    phoneNumber: phoneNumber,
                    relationship: relationship,
                    mailAddress: mailAddress,
                    usAddress: usAddress,
                    printForm: true,
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    personId: 10,
                    participantId: 20);
            };

            var instance = createEntity();
            var validator = new DependentValidator();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            birthCountryReasonCode = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count());
        }

        [TestMethod]
        public void TestBirthCountryReason_MaxLength()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "street1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "street2";
            usAddress.PostalCode = "22222";

            string birthCity = "birth city";
            string birthCountryCode = "US";
            var birthCountryReasonCode = USBornReasonType.Item01.ToString();
            DateTime birthDate = DateTime.Now;
            string citizenshipCountryCode = "UK";
            string emailAddress = "email@isp.com";

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            string gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;
            string permanentResidenceCountryCode = "FR";
            string phoneNumber = "18505551212";
            string relationship = "relations";
            var isTravelingWithParticipant = true;
            Func<AddedDependent> createEntity = () =>
            {
                return new AddedDependent(
                    fullName: fullName,
                    birthCity: birthCity,
                    birthCountryCode: birthCountryCode,
                    birthCountryReasonCode: birthCountryReasonCode,
                    birthDate: birthDate,
                    citizenshipCountryCode: citizenshipCountryCode,
                    emailAddress: emailAddress,
                    gender: gender,
                    permanentResidenceCountryCode: permanentResidenceCountryCode,
                    phoneNumber: phoneNumber,
                    relationship: relationship,
                    mailAddress: mailAddress,
                    usAddress: usAddress,
                    printForm: true,
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    personId: 10,
                    participantId: 20);
            };

            var instance = createEntity();
            var validator = new DependentValidator();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            birthCountryReasonCode = new string('c', DependentValidator.BIRTH_COUNTRY_REASON_LENGTH);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count());
        }

        [TestMethod]
        public void TestBirthCountryReason_ExceedsMaxLength()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "street1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "street2";
            usAddress.PostalCode = "22222";

            string birthCity = "birth city";
            string birthCountryCode = "US";
            var birthCountryReasonCode = USBornReasonType.Item01.ToString();
            DateTime birthDate = DateTime.Now;
            string citizenshipCountryCode = "UK";
            string emailAddress = "email@isp.com";

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            string gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;
            string permanentResidenceCountryCode = "FR";
            string phoneNumber = "18505551212";
            string relationship = "relations";
            var isTravelingWithParticipant = true;
            Func<AddedDependent> createEntity = () =>
            {
                return new AddedDependent(
                    fullName: fullName,
                    birthCity: birthCity,
                    birthCountryCode: birthCountryCode,
                    birthCountryReasonCode: birthCountryReasonCode,
                    birthDate: birthDate,
                    citizenshipCountryCode: citizenshipCountryCode,
                    emailAddress: emailAddress,
                    gender: gender,
                    permanentResidenceCountryCode: permanentResidenceCountryCode,
                    phoneNumber: phoneNumber,
                    relationship: relationship,
                    mailAddress: mailAddress,
                    usAddress: usAddress,
                    printForm: true,
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    personId: 10,
                    participantId: 20);
            };

            var instance = createEntity();
            var validator = new DependentValidator();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            birthCountryReasonCode = new string('c', DependentValidator.BIRTH_COUNTRY_REASON_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(DependentValidator.BIRTH_COUNTRY_REASON_ERROR_MESSAGE, 
                validator.GetPersonType(instance),
                validator.GetNameDelegate()(instance),
                DependentValidator.BIRTH_COUNTRY_REASON_LENGTH),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(DependentErrorPath));
        }

        #endregion

        #region Birthdate
        [TestMethod]
        public void TestBirthDate_IsBornNow()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "street1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "street2";
            usAddress.PostalCode = "22222";

            string birthCity = "birth city";
            string birthCountryCode = "US";
            var birthCountryReasonCode = USBornReasonType.Item01.ToString();
            DateTime birthDate = DateTime.Now;
            string citizenshipCountryCode = "UK";
            string emailAddress = "email@isp.com";

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            string gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;
            string permanentResidenceCountryCode = "FR";
            string phoneNumber = "18505551212";
            string relationship = "relations";
            var isTravelingWithParticipant = true;
            Func<AddedDependent> createEntity = () =>
            {
                return new AddedDependent(
                    fullName: fullName,
                    birthCity: birthCity,
                    birthCountryCode: birthCountryCode,
                    birthCountryReasonCode: birthCountryReasonCode,
                    birthDate: birthDate,
                    citizenshipCountryCode: citizenshipCountryCode,
                    emailAddress: emailAddress,
                    gender: gender,
                    permanentResidenceCountryCode: permanentResidenceCountryCode,
                    phoneNumber: phoneNumber,
                    relationship: relationship,
                    mailAddress: mailAddress,
                    usAddress: usAddress,
                    printForm: true,
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    personId: 10,
                    participantId: 20);
            };

            var instance = createEntity();
            var validator = new DependentValidator();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);   
        }

        [TestMethod]
        public void TestBirthDate_IsExactlyMaxYearsOld()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "street1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "street2";
            usAddress.PostalCode = "22222";

            string birthCity = "birth city";
            string birthCountryCode = "US";
            var birthCountryReasonCode = USBornReasonType.Item01.ToString();
            DateTime birthDate = DateTime.Now;
            string citizenshipCountryCode = "UK";
            string emailAddress = "email@isp.com";

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            string gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;
            string permanentResidenceCountryCode = "FR";
            string phoneNumber = "18505551212";
            string relationship = "relations";
            var isTravelingWithParticipant = true;
            Func<AddedDependent> createEntity = () =>
            {
                return new AddedDependent(
                    fullName: fullName,
                    birthCity: birthCity,
                    birthCountryCode: birthCountryCode,
                    birthCountryReasonCode: birthCountryReasonCode,
                    birthDate: birthDate,
                    citizenshipCountryCode: citizenshipCountryCode,
                    emailAddress: emailAddress,
                    gender: gender,
                    permanentResidenceCountryCode: permanentResidenceCountryCode,
                    phoneNumber: phoneNumber,
                    relationship: relationship,
                    mailAddress: mailAddress,
                    usAddress: usAddress,
                    printForm: true,
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    personId: 10,
                    participantId: 20);
            };

            var instance = createEntity();
            var validator = new DependentValidator();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            birthDate = birthDate.AddYears(-1 * DependentValidator.MAX_DEPENDENT_AGE);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual(
                String.Format(DependentValidator.DEPENDENT_IS_TO_OLD_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance), DependentValidator.MAX_DEPENDENT_AGE),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(DependentErrorPath));
        }

        [TestMethod]
        public void TestBirthDate_IsMoreThanMaxYearsOld()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "street1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "street2";
            usAddress.PostalCode = "22222";

            string birthCity = "birth city";
            string birthCountryCode = "US";
            var birthCountryReasonCode = USBornReasonType.Item01.ToString();
            DateTime birthDate = DateTime.Now;
            string citizenshipCountryCode = "UK";
            string emailAddress = "email@isp.com";

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            string gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;
            string permanentResidenceCountryCode = "FR";
            string phoneNumber = "18505551212";
            string relationship = "relations";
            var isTravelingWithParticipant = true;
            Func<AddedDependent> createEntity = () =>
            {
                return new AddedDependent(
                    fullName: fullName,
                    birthCity: birthCity,
                    birthCountryCode: birthCountryCode,
                    birthCountryReasonCode: birthCountryReasonCode,
                    birthDate: birthDate,
                    citizenshipCountryCode: citizenshipCountryCode,
                    emailAddress: emailAddress,
                    gender: gender,
                    permanentResidenceCountryCode: permanentResidenceCountryCode,
                    phoneNumber: phoneNumber,
                    relationship: relationship,
                    mailAddress: mailAddress,
                    usAddress: usAddress,
                    printForm: true,
                    isTravelingWithParticipant: isTravelingWithParticipant,
                    personId: 10,
                    participantId: 20);
            };

            var instance = createEntity();
            var validator = new DependentValidator();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            birthDate = birthDate.AddYears(-2 * DependentValidator.MAX_DEPENDENT_AGE);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count());
            Assert.AreEqual(
                String.Format(DependentValidator.DEPENDENT_IS_TO_OLD_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance), DependentValidator.MAX_DEPENDENT_AGE),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(DependentErrorPath));
        }
        #endregion
    }
}
