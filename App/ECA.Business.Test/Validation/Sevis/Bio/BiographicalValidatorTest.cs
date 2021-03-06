﻿using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneNumbers;
using System;
using System.Linq;

namespace ECA.Business.Test.Validation.Sevis.Bio
{

    public class BiographicalTestClass : IBiographical, IFluentValidatable
    {
        public string BirthCity
        {
            get; set;
        }

        public string BirthCountryCode
        {
            get; set;
        }

        public int BirthCountryReasonId
        {
            get; set;
        }

        public DateTime? BirthDate
        {
            get; set;
        }

        public string CitizenshipCountryCode
        {
            get; set;
        }

        public string EmailAddress
        {
            get; set;
        }

        public FullName FullName
        {
            get; set;
        }

        public string Gender
        {
            get; set;
        }

        public AddressDTO MailAddress
        {
            get; set;
        }

        public string PermanentResidenceCountryCode
        {
            get; set;
        }

        public string PhoneNumber
        {
            get; set;
        }

        public AddressDTO USAddress
        {
            get; set;
        }

        public Func<bool> ShouldValidateDelegate { get; set; }

        public bool ShouldValidate()
        {
            return ShouldValidateDelegate();
        }
    }

    public class BiographicalTestClassValidator : BiographicalValidator<BiographicalTestClass>
    {
        public BiographicalTestClassValidator()
        {
            this.FullName = "Full Name";
        }

        public const string PERSON_TYPE = "PersonType";

        public string FullName { get; set; }

        public override Func<BiographicalTestClass, object> GetNameDelegate()
        {
            return (t) => FullName;
        }

        public override string GetPersonType(BiographicalTestClass instance)
        {
            return PERSON_TYPE;
        }

        public override ErrorPath GetBirthDateErrorPath(BiographicalTestClass instance)
        {
            return new BirthDateErrorPath();
        }

        public override ErrorPath GetGenderErrorPath(BiographicalTestClass instance)
        {
            return new GenderErrorPath();
        }

        public override ErrorPath GetBirthCityErrorPath(BiographicalTestClass instance)
        {
            return new CityOfBirthErrorPath();
        }

        public override ErrorPath GetBirthCountryCodeErrorPath(BiographicalTestClass instance)
        {
            return new CountryOfBirthErrorPath();
        }

        public override ErrorPath GetCitizenshipCountryCodeErrorPath(BiographicalTestClass instance)
        {
            return new CitizenshipErrorPath();
        }

        public override ErrorPath GetPermanentResidenceCountryCodeErrorPath(BiographicalTestClass instance)
        {
            return new PermanentResidenceCountryErrorPath();
        }

        public override ErrorPath GetEmailAddressErrorPath(BiographicalTestClass instance)
        {
            return new EmailErrorPath();
        }

        public override ErrorPath GetPhoneNumberErrorPath(BiographicalTestClass instance)
        {
            return new PhoneNumberErrorPath();
        }
    }

    [TestClass]
    public class BiographicalValidatorTest
    {
        public BiographicalTestClass GetValidBiographical(Func<bool> shouldValidateDelegate)
        {
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            return new BiographicalTestClass
            {
                BirthCity = "birth city",
                BirthCountryCode = "US",
                BirthCountryReasonId = BirthCountryReason.BornToForeignDiplomat.Id,
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "UK",
                EmailAddress = "email@isp.com",
                FullName = fullName,
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                PermanentResidenceCountryCode = "FR",
                PhoneNumber = "18502663026",
                ShouldValidateDelegate = shouldValidateDelegate
            };
        }

        #region Full Name

        [TestMethod]
        public void TestFullName_ShouldRunFullNameValidator()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.FullName = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator<BiographicalTestClass>.FULL_NAME_NULL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Birthdate
        [TestMethod]
        public void TestBirthDate_IsNull()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthDate = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.BIRTH_DATE_NULL_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance)),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(BirthDateErrorPath));
        }

        #endregion

        #region Gender
        [TestMethod]
        public void TestGender_IsNull()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Gender = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.GENDER_REQUIRED_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance)),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(GenderErrorPath));
        }

        [TestMethod]
        public void TestGender_NotMaleOrFemaleGenderCode()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Gender = "U";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.GENDER_MUST_BE_A_VALUE_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance), Gender.SEVIS_MALE_GENDER_CODE_VALUE, Gender.SEVIS_FEMALE_GENDER_CODE_VALUE),
                result.Errors.First().ErrorMessage);

            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(GenderErrorPath));
        }

        [TestMethod]
        public void TestGender_MaleGenderCode()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestGender_FemaleGenderCode()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        #endregion

        #region Birth City

        [TestMethod]
        public void TestBirthCity_Null()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCity = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance)),
                result.Errors.First().ErrorMessage);

            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CityOfBirthErrorPath));
        }

        [TestMethod]
        public void TestBirthCity_EmptyString()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCity = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance)),
                result.Errors.First().ErrorMessage);

            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CityOfBirthErrorPath));
        }

        [TestMethod]
        public void TestBirthCity_ExceedsMaxLength()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCity = new String('c', BiographicalValidator<BiographicalTestClass>.CITY_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance)),
                result.Errors.First().ErrorMessage);

            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CityOfBirthErrorPath));
        }

        #endregion

        #region Birth Country Code

        [TestMethod]
        public void TestBirthCountryCode_Null()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.BIRTH_COUNTRY_CODE_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance)),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CountryOfBirthErrorPath));
        }

        [TestMethod]
        public void TestBirthCountryCode_EmptyString()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryCode = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.BIRTH_COUNTRY_CODE_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance)),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CountryOfBirthErrorPath));
        }

        [TestMethod]
        public void TestBirthCountry_ExceedsMaxLength()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryCode = new String('c', BiographicalValidator<BiographicalTestClass>.COUNTRY_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.BIRTH_COUNTRY_CODE_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance)),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CountryOfBirthErrorPath));
        }

        #endregion

        #region Citizenship Country Code

        [TestMethod]
        public void TestCitizenshipCountryCode_Null()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.CitizenshipCountryCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance)),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CitizenshipErrorPath));
        }

        [TestMethod]
        public void TestCitizenshipCountryCode_EmptyString()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.CitizenshipCountryCode = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance)),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CitizenshipErrorPath));
        }

        [TestMethod]
        public void TestCitizenshipCountry_ExceedsMaxLength()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.CitizenshipCountryCode = new String('c', BiographicalValidator<BiographicalTestClass>.COUNTRY_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, validator.GetPersonType(instance), validator.GetNameDelegate()(instance)),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CitizenshipErrorPath));
        }

        #endregion

        #region Email

        [TestMethod]
        public void TestEmailAddress_Null()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.EmailAddress = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestEmailAddress_NotValid()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.EmailAddress = "someone";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.EMAIL_ERROR_MESSAGE, instance.EmailAddress, validator.GetPersonType(instance), validator.GetNameDelegate()(instance), BiographicalTestClassValidator.EMAIL_MAX_LENGTH),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(EmailErrorPath));
        }

        [TestMethod]
        public void TestEmailAddress_ExceedsMaxLength()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.EmailAddress = "someone@isp.co" + new string('m', BiographicalValidator<BiographicalTestClass>.EMAIL_MAX_LENGTH);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.EMAIL_ERROR_MESSAGE, instance.EmailAddress, validator.GetPersonType(instance), validator.GetNameDelegate()(instance), BiographicalTestClassValidator.EMAIL_MAX_LENGTH),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(EmailErrorPath));
        }

        #endregion

        #region Phone Number
        [TestMethod]
        public void TestPhoneNumber_HasCharacters()
        {
            var phonenumberUtil = PhoneNumberUtil.GetInstance();
            var example = phonenumberUtil.GetExampleNumber(Data.PhoneNumber.US_PHONE_NUMBER_REGION_KEY);
            var formattedExample = phonenumberUtil.Format(example, PhoneNumberFormat.INTERNATIONAL);

            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PhoneNumber = "abc";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            Assert.AreEqual(
                String.Format(BiographicalTestClassValidator.PHONE_NUMBER_ERROR_MESSAGE,
                Data.PhoneNumberType.Visiting.Value,
                instance.PhoneNumber,
                validator.GetPersonType(instance),
                validator.GetNameDelegate()(instance),
                formattedExample),
                result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PhoneNumberErrorPath));
        }
        #endregion

        #region MailAddress
        [TestMethod]
        public void TestMailAddressShouldRunValidator()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.MailAddress = new AddressDTO
            {
                Street1 = "street 1",
                Division = "FL",
                PostalCode = "12345",
                Country = "hello world"
            };
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(String.Format(AddressDTOValidator.COUNTRY_ERROR_MESSAGE, AddressDTOValidator.PERSON_HOST_ADDRESS, LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME), result.Errors.First().ErrorMessage);
        }
        #endregion

        #region USAddress
        [TestMethod]
        public void TestUSAddressShouldRunValidator()
        {
            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.USAddress = new AddressDTO
            {
                Street1 = "street 1",
                Division = "FL",
                PostalCode = "12345",
                Country = "hello world"
            };
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(String.Format(AddressDTOValidator.COUNTRY_ERROR_MESSAGE, AddressDTOValidator.C_STREET_ADDRESS, LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME), result.Errors.First().ErrorMessage);
        }
        #endregion

        #region Should Not Validate
        [TestMethod]
        public void TestShouldValidate_ShouldNotValidate()
        {
            var phonenumberUtil = PhoneNumberUtil.GetInstance();
            var example = phonenumberUtil.GetExampleNumber(Data.PhoneNumber.US_PHONE_NUMBER_REGION_KEY);
            var formattedExample = phonenumberUtil.Format(example, PhoneNumberFormat.INTERNATIONAL);

            var validator = new BiographicalTestClassValidator();
            var instance = GetValidBiographical(() => true);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PhoneNumber = "abc";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);

            instance.ShouldValidateDelegate = () => false;

            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }
        #endregion
    }
}
