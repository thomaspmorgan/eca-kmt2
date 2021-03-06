﻿using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Persons;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ECA.Business.Test.Validation.Sevis.Bio
{

    [TestClass]
    public class PersonValidatorTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var sevisId = "sevisId";
            var startDate = DateTime.UtcNow;
            var sevisOrgId = "P-1-11833";
            var isValidated = false;
            var validator = new PersonValidator(sevisId, sevisOrgId, isValidated, startDate);
            Assert.AreEqual(sevisId, validator.SevisId);
            Assert.AreEqual(startDate, validator.ParticipantStartDate);
            Assert.AreEqual(isValidated, validator.IsValidated);
            Assert.AreEqual(sevisOrgId, validator.SevisOrgId);
        }

        [TestMethod]
        public void TestGetPersonType()
        {
            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            Assert.AreEqual(PersonValidator.PERSON_TYPE, validator.GetPersonType(null));
        }

        [TestMethod]
        public void TestGetBirthCityErrorPath()
        {
            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            Assert.IsInstanceOfType(validator.GetBirthCityErrorPath(null), typeof(CityOfBirthErrorPath));
        }

        [TestMethod]
        public void TestGetBirthCountryCodeErrorPath()
        {
            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            Assert.IsInstanceOfType(validator.GetBirthCountryCodeErrorPath(null), typeof(CountryOfBirthErrorPath));
        }

        [TestMethod]
        public void TestGetBirthDateErrorPath()
        {
            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            Assert.IsInstanceOfType(validator.GetBirthDateErrorPath(null), typeof(BirthDateErrorPath));
        }

        [TestMethod]
        public void TestGetCitizenshipCountryCodeErrorPath()
        {
            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            Assert.IsInstanceOfType(validator.GetCitizenshipCountryCodeErrorPath(null), typeof(CitizenshipErrorPath));
        }

        [TestMethod]
        public void TestGetEmailAddressErrorPath()
        {
            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            Assert.IsInstanceOfType(validator.GetEmailAddressErrorPath(null), typeof(EmailErrorPath));
        }

        [TestMethod]
        public void TestGetGenderErrorPath()
        {
            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            Assert.IsInstanceOfType(validator.GetGenderErrorPath(null), typeof(GenderErrorPath));
        }

        [TestMethod]
        public void TestGetPermanentResidenceCountryCodeErrorPath()
        {
            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            Assert.IsInstanceOfType(validator.GetPermanentResidenceCountryCodeErrorPath(null), typeof(PermanentResidenceCountryErrorPath));
        }

        [TestMethod]
        public void TestGetPhoneNumberErrorPath()
        {
            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            Assert.IsInstanceOfType(validator.GetPhoneNumberErrorPath(null), typeof(PhoneNumberErrorPath));
        }

        [TestMethod]
        public void TestGetNameDelegate()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var d = validator.GetNameDelegate();
            Assert.AreEqual(string.Format("{0} {1}", fullName.FirstName, fullName.LastName), d(createEntity()));
        }

        [TestMethod]
        public void TestPersonValidator_ProgramCategoryCodeNull()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            programCategory = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(PersonValidator.CATEGORY_CODE_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(ProgramCategoryCodeErrorPath));
        }

        [TestMethod]
        public void TestPersonValidator_ProgramCategoryCodeExceedsLength()
        {

            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            programCategory = new string('a', PersonValidator.CATEGORY_CODE_LENGTH + 1);
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(PersonValidator.PROGRAM_CATEGORY_CODE_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(ProgramCategoryCodeErrorPath));
        }

        [TestMethod]
        public void TestPersonValidator_PositionCodeIsNull()
        {

            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            short positionCode = 120;
            string positionCodeAsString = positionCode.ToString();
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            positionCodeAsString = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(PersonValidator.POSITION_CODE_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(PositionCodeErrorPath));
        }

        [TestMethod]
        public void TestPersonValidator_PositionCodeExceedsMaxLength()
        {

            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH + 1);
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(PersonValidator.POSITION_CODE_MUST_BE_DIGITS_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(PositionCodeErrorPath));
        }

        [TestMethod]
        public void TestPersonValidator_PositionCodeisNotANumber()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            positionCodeAsString = "a";
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(PersonValidator.POSITION_CODE_MUST_BE_DIGITS_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(PositionCodeErrorPath));
        }

        [TestMethod]
        public void TestPersonValidator_SubjectFieldIsNull()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            subjectField = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(PersonValidator.SUBJECT_FIELD_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(FieldOfStudyErrorPath));
        }

        [TestMethod]
        public void TestPersonValidator_EmailAddressIsNull()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            email = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(1.0));
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(String.Format(PersonValidator.EMAIL_ADDRESS_REQUIRED_FORMAT_MESSAGE, EmailAddressType.Personal.Value), results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(EmailErrorPath));
        }

        [TestMethod]
        public void TestPersonValidator_EmailAddressIsNull_SevisOrgIdIsNull()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO - 1.0));
            email = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);

            validator = new PersonValidator("sevisId", null, false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO - 1.0));
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(String.Format(PersonValidator.EMAIL_ADDRESS_REQUIRED_FORMAT_MESSAGE, EmailAddressType.Personal.Value), results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(EmailErrorPath));
        }

        [TestMethod]
        public void TestPersonValidator_EmailAddressIsNull_SevisOrgIdStartsWithG()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            email = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            validator = new PersonValidator("sevisId", PersonValidator.G_PROGRAM_PREFIX + "sevisOrgId", false, DateTime.UtcNow.AddDays(1.0));
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);            
        }

        [TestMethod]
        public void TestPersonValidator_EmailAddressIsNull_PersonHasNullSevisId()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            email = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            validator = new PersonValidator(null, "sevisOrgId", false, DateTime.UtcNow.AddDays(1.0));
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestPersonValidator_EmailAddressIsNull_PersonHasEmptySevisId()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            email = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            validator = new PersonValidator(string.Empty, "sevisOrgId", false, DateTime.UtcNow.AddDays(1.0));
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestPersonValidator_EmailAddressIsNull_PersonHasWhitespaceSevisId()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            email = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            validator = new PersonValidator(" ", "sevisOrgId", false, DateTime.UtcNow.AddDays(1.0));
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestPersonValidator_EmailAddressIsNull_StartDateNotPassed()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            email = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestPersonValidator_ShouldRunMailAddressValidator()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(-1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            mailAddress.Street1 = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void TestPersonValidator_MailAddressIsNull()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            mailAddress = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(1.0));
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(String.Format(PersonValidator.MAILING_ADDRESS_REQUIRED_FORMAT_MESSAGE, AddressType.Host.Value, LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME), results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestPersonValidator_MailAddressIsNull_PersonHasNullSevisId()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            mailAddress = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            validator = new PersonValidator(null, "sevisOrgId", false, DateTime.UtcNow.AddDays(1.0));
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestPersonValidator_MailAddressIsNull_PersonHasEmptySevisId()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            mailAddress = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            validator = new PersonValidator(string.Empty, "sevisOrgId", false, DateTime.UtcNow.AddDays(1.0));
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestPersonValidator_MailAddressIsNull_PersonHasWhitespaceSevisId()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            mailAddress = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            validator = new PersonValidator(" ", "sevisOrgId", false, DateTime.UtcNow.AddDays(1.0));
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestPersonValidator_MailAddressIsNull_StartDateHasNotYetPassed()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            mailAddress = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        #region Phone Number
        [TestMethod]
        public void TestPhoneNumber_IsNull()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            phone = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
            validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(1.0));
            instance = createEntity();
            results = validator.Validate(instance);

            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(
                String.Format(PersonValidator.VISITING_PHONE_REQUIRED_ERROR_MESSAGE,
                Data.PhoneNumberType.Visiting.Value,
                validator.GetPersonType(instance),
                validator.GetNameDelegate()(instance)),
                results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(PhoneNumberErrorPath));
        }

        [TestMethod]
        public void TestPhoneNumber_SevisOrgIdIsNull_PhoneNumberIsNull()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            phone = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
            validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO - 1.0));
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);


            validator = new PersonValidator("sevisId", null, false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO - 1.0));
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(
                String.Format(PersonValidator.VISITING_PHONE_REQUIRED_ERROR_MESSAGE,
                Data.PhoneNumberType.Visiting.Value,
                validator.GetPersonType(instance),
                validator.GetNameDelegate()(instance)),
                results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(PhoneNumberErrorPath));


        }

        [TestMethod]
        public void TestPhoneNumber_IsNull_SevisOrgIdStartsWithG()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            phone = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
            validator = new PersonValidator("sevisId", PersonValidator.G_PROGRAM_PREFIX + "sevisOrgId", false, DateTime.UtcNow.AddDays(1.0));
            instance = createEntity();
            results = validator.Validate(instance);

            Assert.IsTrue(results.IsValid);
        }
        #endregion

        #region Permanent Residence Country Code

        [TestMethod]
        public void TestPermanentResidenceCountryCode_Null()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            permanentResidenceCountryCode = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(
                String.Format(PersonValidator.PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance), AddressType.Home.Value, LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME),
                results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(PermanentResidenceCountryErrorPath));
        }

        [TestMethod]
        public void TestPermanentResidenceCountryCode_NotSupported()
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

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
            var phone = "18505551212";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            Func<Business.Validation.Sevis.Bio.Person> createEntity = () =>
            {
                return new Business.Validation.Sevis.Bio.Person(
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
                positionCodeAsString,
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            };

            var validator = new PersonValidator("sevisId", "sevisOrgId", false, DateTime.UtcNow.AddDays(ParticipantPersonsSevisService.NUMBER_OF_DAYS_BEFORE_START_DATE_A_PARTICIPANT_NEEDS_VALIDATION_INFO + 1.0));
            var instance = createEntity();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            permanentResidenceCountryCode = "US";
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(
                String.Format(PersonValidator.PERMANENT_RESIDENCE_COUNTRY_NOT_SUPPORTED, permanentResidenceCountryCode, validator.GetPersonType(instance), validator.GetNameDelegate()(instance), AddressType.Home.Value),
                results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(PermanentResidenceCountryErrorPath));
        }

        #endregion
    }
}
