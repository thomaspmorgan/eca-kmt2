using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Validation.Model;
using ECA.Data;
using ECA.Business.Validation.Model.Shared;
using ECA.Business.Validation.SEVIS;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using ECA.Business.Validation.Sevis.Bio;
using FluentValidation.Attributes;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [Validator(typeof(BiographyTestValidator))]
    public class BiographyTestClass : Biography
    {

    }

    public class BiographyTestValidator : BiographyValidator
    {

    }

    [TestClass]
    public class BiographyValidatorTest
    {
        public BiographyTestClass GetValidBiographical()
        {
            return new BiographyTestClass
            {
                BirthCity = "birth city",
                BirthCountryCode = "US",
                BirthCountryReason = "re",
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "UK",
                EmailAddress = "email@isp.com",
                FullName = new FullName
                {
                    FirstName = "first name",
                    LastName = "last name",
                    PassportName = "passport name",
                    PreferredName = "preferred name",
                    Suffix = FullNameValidator.SECOND_SUFFIX
                },
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                PermanentResidenceCountryCode = "FR",
            };
        }

        #region Full Name

        [TestMethod]
        public void TestFullName_ShouldRunFullNameValidator()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.FullName = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.FULL_NAME_NULL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Birthdate
        [TestMethod]
        public void TestBirthDate_IsNull()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthDate = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.BIRTH_DATE_NULL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(BirthDateErrorPath));
        }

        #endregion

        #region Gender
        [TestMethod]
        public void TestGender_IsNull()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Gender = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.GENDER_REQUIRED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Gender

        [TestMethod]
        public void TestGender_NotMaleOrFemaleGenderCode()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Gender = "U";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.GENDER_MUST_BE_A_VALUE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(GenderErrorPath));
        }

        [TestMethod]
        public void TestGender_MaleGenderCode()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestGender_FemaleGenderCode()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
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
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCity = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CityOfBirthErrorPath));
        }

        [TestMethod]
        public void TestBirthCity_EmptyString()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCity = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CityOfBirthErrorPath));
        }

        [TestMethod]
        public void TestBirthCity_ExceedsMaxLength()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCity = new String('c', BiographyTestValidator.CITY_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CityOfBirthErrorPath));
        }

        #endregion

        #region Birth Country Code

        [TestMethod]
        public void TestBirthCountryCode_Null()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.BIRTH_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CountryOfBirthErrorPath));
        }

        [TestMethod]
        public void TestBirthCountryCode_EmptyString()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryCode = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.BIRTH_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CountryOfBirthErrorPath));
        }

        [TestMethod]
        public void TestBirthCountry_ExceedsMaxLength()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryCode = new String('c', BiographyTestValidator.COUNTRY_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.BIRTH_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CountryOfBirthErrorPath));
        }

        #endregion

        #region Citizenship Country Code

        [TestMethod]
        public void TestCitizenshipCountryCode_Null()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.CitizenshipCountryCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CitizenshipErrorPath));
        }

        [TestMethod]
        public void TestCitizenshipCountryCode_EmptyString()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.CitizenshipCountryCode = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CitizenshipErrorPath));
        }

        [TestMethod]
        public void TestCitizenshipCountry_ExceedsMaxLength()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.CitizenshipCountryCode = new String('c', BiographyTestValidator.COUNTRY_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CitizenshipErrorPath));
        }

        #endregion

        #region Permanent Residence Country Code

        [TestMethod]
        public void TestPermanentResidenceCountryCode_Null()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PermanentResidenceCountryCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PermanentResidenceCountryErrorPath));
        }

        [TestMethod]
        public void TestPermanentResidenceCountryCode_EmptyString()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PermanentResidenceCountryCode = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PermanentResidenceCountryErrorPath));
        }

        [TestMethod]
        public void TestPermanentResidenceCountryCode_ExceedsMaxLength()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PermanentResidenceCountryCode = new String('c', BiographyTestValidator.COUNTRY_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PermanentResidenceCountryErrorPath));
        }

        #endregion

        #region Birth Country Reason

        [TestMethod]
        public void TestBirthCountryReason_Null()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryReason = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestBirthCountryReason_MaxLength()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryReason = new string('c', BiographyTestValidator.BIRTH_COUNTRY_REASON_LENGTH);
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestBirthCountryReason_ExceedsMaxLength()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryReason = new string('c', BiographyTestValidator.BIRTH_COUNTRY_REASON_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.BIRTH_COUNTRY_REASON_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CountryOfBirthErrorPath));
        }

        #endregion

        #region Email

        [TestMethod]
        public void TestEmailAddress_Null()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
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
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.EmailAddress = "someone";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.INVALID_EMAIL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(EmailErrorPath));
        }

        [TestMethod]
        public void TestEmailAddress_ExceedsMaxLength()
        {
            var validator = new BiographyTestValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.EmailAddress = "someone@isp.co" + new string('m', BiographyTestValidator.EMAIL_MAX_LENGTH);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyTestValidator.EMAIL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(EmailErrorPath));
        }

        #endregion
    }
}
