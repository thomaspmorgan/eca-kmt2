using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.Shared;
using ECA.Business.Validation.SEVIS;
using ECA.Business.Validation.SEVIS.ErrorPaths;

namespace ECA.Business.Test.Validation.Shared
{
    [TestClass]
    public class FullNameValidatorTest
    {
        public FullName GetValidFullName()
        {
            return new FullName
            {
                FirstName = "first name",
                LastName = "last name",
                PassportName = "passport name",
                PreferredName = "preferred name",
                Suffix = FullNameValidator.SENIOR_SUFFIX
            };
        }

        [TestMethod]
        public void TestFirstName_Null()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.FirstName = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestFirstName_ExceedsMaxLength()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.FirstName = new string('c', FullNameValidator.FIRST_NAME_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(FullNameValidator.FIRST_NAME_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FullNameErrorPath));
        }

        [TestMethod]
        public void TestLastName_Null()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.LastName = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(FullNameValidator.LAST_NAME_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FullNameErrorPath));
        }

        [TestMethod]
        public void TestLastName_ExceedsMaxLength()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.LastName = new string('c', FullNameValidator.LAST_NAME_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(FullNameValidator.LAST_NAME_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FullNameErrorPath));
        }

        [TestMethod]
        public void TestSuffix_Null()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Suffix = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestSuffix_CheckJuniorSuffixIsValid()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Suffix = FullNameValidator.JUNIOR_SUFFIX;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestSuffix_CheckSeniorSuffixIsValid()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Suffix = FullNameValidator.SENIOR_SUFFIX;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestSuffix_CheckFirstSuffixIsValid()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Suffix = FullNameValidator.FIRST_SUFFIX;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestSuffix_CheckSecondSuffixIsValid()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Suffix = FullNameValidator.SECOND_SUFFIX;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestSuffix_CheckThirdSuffixIsValid()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Suffix = FullNameValidator.THIRD_SUFFIX;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestSuffix_CheckFourthSuffixIsValid()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Suffix = FullNameValidator.FOURTH_SUFFIX;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            Assert.AreEqual(0, result.Errors.Count);
        }

        [TestMethod]
        public void TestSuffix_UnknownSuffix()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Suffix = "x";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(FullNameValidator.SUFFIX_VALUE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FullNameErrorPath));
        }

        [TestMethod]
        public void TestPassportName_Null()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PassportName = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestPassportName_ExceedsMaxLength()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PassportName = new string('c', FullNameValidator.PASSPORT_NAME_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(FullNameValidator.PASSPORT_NAME_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FullNameErrorPath));
        }

        [TestMethod]
        public void TestPreferredName_Null()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PreferredName = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestPreferredName_ExceedsMaxLength()
        {
            var validator = new FullNameValidator();
            var instance = GetValidFullName();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PreferredName = new string('c', FullNameValidator.PREFERRED_NAME_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(FullNameValidator.PREFFERED_NAME_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FullNameErrorPath));
        }
    }
}
