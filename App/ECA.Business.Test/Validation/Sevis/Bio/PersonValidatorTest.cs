using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ECA.Business.Test.Validation.Sevis.Bio
{

    [TestClass]
    public class PersonValidatorTest
    {
        public Business.Validation.Sevis.Bio.Person GetValidPerson()
        {
            return new Business.Validation.Sevis.Bio.Person
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
                PhoneNumber = "123-456-7890"
            };
        }

        #region PhoneNumber

        [TestMethod]
        public void TestFullName_ShouldRunFullNameValidator()
        {
            var validator = new PersonValidator();
            var instance = GetValidPerson();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PhoneNumber = null;
            result = validator.Validate(instance);
            Assert.Fail("come back to this validation test.");
            //Assert.IsFalse(result.IsValid);
            //Assert.AreEqual(1, result.Errors.Count);
            //Assert.AreEqual(BiographyTestValidator.FULL_NAME_NULL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        #endregion
    }
}
