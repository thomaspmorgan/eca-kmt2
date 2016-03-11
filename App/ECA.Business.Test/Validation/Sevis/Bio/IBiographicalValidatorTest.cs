using ECA.Business.Queries.Models.Admin;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ECA.Business.Test.Validation.Sevis.Bio
{

    public class BiographicalTestClass : IBiographical
    {
        public string BirthCity
        {
            get; set;
        }

        public string BirthCountryCode
        {
            get; set;
        }

        public string BirthCountryReason
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
    }

    [TestClass]
    public class IBiographicalValidatorTest
    {
        public BiographicalTestClass GetValidBiographical()
        {
            return new BiographicalTestClass
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

        [TestMethod]
        public void TestAddMoreChecksHere()
        {
            Assert.Fail("This should be a Person validator instead of a Biography validator and I need to test other properties that are now part of the person.");
        }

        #region Full Name

        [TestMethod]
        public void TestFullName_ShouldRunFullNameValidator()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.FullName = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.FULL_NAME_NULL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Birthdate
        [TestMethod]
        public void TestBirthDate_IsNull()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthDate = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.BIRTH_DATE_NULL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(BirthDateErrorPath));
        }

        #endregion

        #region Gender
        [TestMethod]
        public void TestGender_IsNull()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Gender = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.GENDER_REQUIRED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion

        #region Gender

        [TestMethod]
        public void TestGender_NotMaleOrFemaleGenderCode()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Gender = "U";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.GENDER_MUST_BE_A_VALUE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(GenderErrorPath));
        }

        [TestMethod]
        public void TestGender_MaleGenderCode()
        {
            var validator = new BiographyValidator();
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
            var validator = new BiographyValidator();
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
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCity = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CityOfBirthErrorPath));
        }

        [TestMethod]
        public void TestBirthCity_EmptyString()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCity = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CityOfBirthErrorPath));
        }

        [TestMethod]
        public void TestBirthCity_ExceedsMaxLength()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCity = new String('c', BiographyValidator.CITY_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.CITY_OF_BIRTH_REQUIRED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CityOfBirthErrorPath));
        }

        #endregion

        #region Birth Country Code

        [TestMethod]
        public void TestBirthCountryCode_Null()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.BIRTH_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CountryOfBirthErrorPath));
        }

        [TestMethod]
        public void TestBirthCountryCode_EmptyString()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryCode = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.BIRTH_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CountryOfBirthErrorPath));
        }

        [TestMethod]
        public void TestBirthCountry_ExceedsMaxLength()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryCode = new String('c', BiographyValidator.COUNTRY_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.BIRTH_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CountryOfBirthErrorPath));
        }

        #endregion

        #region Citizenship Country Code

        [TestMethod]
        public void TestCitizenshipCountryCode_Null()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.CitizenshipCountryCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CitizenshipErrorPath));
        }

        [TestMethod]
        public void TestCitizenshipCountryCode_EmptyString()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.CitizenshipCountryCode = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CitizenshipErrorPath));
        }

        [TestMethod]
        public void TestCitizenshipCountry_ExceedsMaxLength()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.CitizenshipCountryCode = new String('c', BiographyValidator.COUNTRY_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.CITIZENSHIP_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CitizenshipErrorPath));
        }

        #endregion

        #region Permanent Residence Country Code

        [TestMethod]
        public void TestPermanentResidenceCountryCode_Null()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PermanentResidenceCountryCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PermanentResidenceCountryErrorPath));
        }

        [TestMethod]
        public void TestPermanentResidenceCountryCode_EmptyString()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PermanentResidenceCountryCode = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PermanentResidenceCountryErrorPath));
        }

        [TestMethod]
        public void TestPermanentResidenceCountryCode_ExceedsMaxLength()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PermanentResidenceCountryCode = new String('c', BiographyValidator.COUNTRY_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.PERMANENT_RESIDENCE_COUNTRY_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PermanentResidenceCountryErrorPath));
        }

        #endregion

        #region Birth Country Reason

        [TestMethod]
        public void TestBirthCountryReason_Null()
        {
            var validator = new BiographyValidator();
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
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryReason = new string('c', BiographyValidator.BIRTH_COUNTRY_REASON_LENGTH);
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestBirthCountryReason_ExceedsMaxLength()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BirthCountryReason = new string('c', BiographyValidator.BIRTH_COUNTRY_REASON_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.BIRTH_COUNTRY_REASON_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(CountryOfBirthErrorPath));
        }

        #endregion

        #region Email

        [TestMethod]
        public void TestEmailAddress_Null()
        {
            var validator = new BiographyValidator();
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
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.EmailAddress = "someone";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.INVALID_EMAIL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(EmailErrorPath));
        }

        [TestMethod]
        public void TestEmailAddress_ExceedsMaxLength()
        {
            var validator = new BiographyValidator();
            var instance = GetValidBiographical();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.EmailAddress = "someone@isp.co" + new string('m', BiographyValidator.EMAIL_MAX_LENGTH);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(BiographyValidator.EMAIL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(EmailErrorPath));
        }

        #endregion
    }
}
