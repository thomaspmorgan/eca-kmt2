﻿using ECA.Business.Queries.Models.Admin;
using System.Linq;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ECA.Business.Validation.SEVIS;

namespace ECA.Business.Test.Validation.Sevis.Bio
{

    [TestClass]
    public class PersonValidatorTest
    {
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
            var fullName = new FullName
            {
                FirstName = "first name",
                LastName = "last name",
                PassportName = "passport name",
                PreferredName = "preferred name",
                Suffix = FullNameValidator.SECOND_SUFFIX
            };
            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "1234567890";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
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
                birthCountryReason,
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

            var validator = new PersonValidator();
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
            var fullName = new FullName
            {
                FirstName = "first name",
                LastName = "last name",
                PassportName = "passport name",
                PreferredName = "preferred name",
                Suffix = FullNameValidator.SECOND_SUFFIX
            };
            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "1234567890";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
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
                birthCountryReason,
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

            var validator = new PersonValidator();
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
            var fullName = new FullName
            {
                FirstName = "first name",
                LastName = "last name",
                PassportName = "passport name",
                PreferredName = "preferred name",
                Suffix = FullNameValidator.SECOND_SUFFIX
            };
            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "1234567890";
            short positionCode = 120;
            string positionCodeAsString = positionCode.ToString();
            var printForm = true;
            var birthCountryReason = "ab";
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
                birthCountryReason,
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

            var validator = new PersonValidator();
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
            var fullName = new FullName
            {
                FirstName = "first name",
                LastName = "last name",
                PassportName = "passport name",
                PreferredName = "preferred name",
                Suffix = FullNameValidator.SECOND_SUFFIX
            };
            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "1234567890";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var birthCountryReason = "ab";
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
                birthCountryReason,
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

            var validator = new PersonValidator();
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
            var fullName = new FullName
            {
                FirstName = "first name",
                LastName = "last name",
                PassportName = "passport name",
                PreferredName = "preferred name",
                Suffix = FullNameValidator.SECOND_SUFFIX
            };
            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "1234567890";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var birthCountryReason = "ab";
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
                birthCountryReason,
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

            var validator = new PersonValidator();
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
            var fullName = new FullName
            {
                FirstName = "first name",
                LastName = "last name",
                PassportName = "passport name",
                PreferredName = "preferred name",
                Suffix = FullNameValidator.SECOND_SUFFIX
            };
            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "1234567890";
            string positionCodeAsString = new string('1', PersonValidator.POSITION_CODE_LENGTH);
            var printForm = true;
            var birthCountryReason = "ab";
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
                birthCountryReason,
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

            var validator = new PersonValidator();
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
    }
}
