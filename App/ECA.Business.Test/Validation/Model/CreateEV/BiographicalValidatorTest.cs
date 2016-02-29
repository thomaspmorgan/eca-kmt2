using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Validation.Model;
using ECA.Data;
using ECA.Business.Validation.Model.Shared;

namespace ECA.Business.Test.Validation.Model.CreateEV
{
    [TestClass]
    public class BiographicalValidatorTest
    {
        public Biographical GetValidBiographical()
        {
            return new Biographical
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
                ResidentialAddress = new ResidentialAddress()
            };
        }

        #region Full Name

        [TestMethod]
        public void TestFullName_ShouldRunFullNameValidator()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.FullName = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.FULL_NAME_NULL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Birthdate
        [TestMethod]
        public void TestBirthDate_IsNull()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthDate = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.BIRTH_DATE_NULL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Birthdate
        [TestMethod]
        public void TestGender_IsNull()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Gender = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.GENDER_REQUIRED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Gender

        [TestMethod]
        public void TestGender_NotMaleOrFemaleGenderCode()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Gender = "U";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.GENDER_MUST_BE_A_VALUE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestGender_MaleGenderCode()
        {
            var validator = new BiographicalValidator();
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
            var validator = new BiographicalValidator();
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
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCity = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestBirthCity_EmptyString()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCity = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestBirthCity_ExceedsMaxLength()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCity = new String('c', BiographicalValidator.CITY_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Birth Country Code

        [TestMethod]
        public void TestBirthCountryCode_Null()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.BIRTH_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestBirthCountryCode_EmptyString()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryCode = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.BIRTH_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestBirthCountry_ExceedsMaxLength()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryCode = new String('c', BiographicalValidator.COUNTRY_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.BIRTH_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Citizenship Country Code

        [TestMethod]
        public void TestCitizenshipCountryCode_Null()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.CitizenshipCountryCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestCitizenshipCountryCode_EmptyString()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.CitizenshipCountryCode = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestCitizenshipCountry_ExceedsMaxLength()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.CitizenshipCountryCode = new String('c', BiographicalValidator.COUNTRY_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Permanent Residence Country Code

        [TestMethod]
        public void TestPermanentResidenceCountryCode_Null()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PermanentResidenceCountryCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestPermanentResidenceCountryCode_EmptyString()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PermanentResidenceCountryCode = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestPermanentResidenceCountryCode_ExceedsMaxLength()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PermanentResidenceCountryCode = new String('c', BiographicalValidator.COUNTRY_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Birth Country Reason

        [TestMethod]
        public void TestBirthCountryReason_Null()
        {
            var validator = new BiographicalValidator();
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
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryReason = new string('c', BiographicalValidator.BIRTH_COUNTRY_REASON_LENGTH);
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestBirthCountryReason_ExceedsMaxLength()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryReason = new string('c', BiographicalValidator.BIRTH_COUNTRY_REASON_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.BIRTH_COUNTRY_REASON_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Email

        [TestMethod]
        public void TestEmailAddress_Null()
        {
            var validator = new BiographicalValidator();
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
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.EmailAddress = "someone";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.INVALID_EMAIL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestEmailAddress_ExceedsMaxLength()
        {
            var validator = new BiographicalValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.EmailAddress = "someone@isp.co" + new string('m', BiographicalValidator.EMAIL_MAX_LENGTH);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographicalValidator.EMAIL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion
    }
}
