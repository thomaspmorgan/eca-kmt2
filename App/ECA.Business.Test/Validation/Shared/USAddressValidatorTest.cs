using ECA.Business.Validation.Model.Sevis;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ECA.Business.Test.Validation.Shared
{
    [TestClass]
    public class USAddressValidatorTest
    {
        public USAddress GetValidUSAddress()
        {
            return new USAddress
            {
                Address1 = "address 1",
                Address2 = "address 2",
                City = "city",
                State = "state",
                PostalCode = "12345",
                Explanation = "explanation",
                ExplanationCode = "cd"
            };
        }

        [TestMethod]
        public void TestAddress1_Null()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Address1 = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(string.Format(USAddressValidator.ADDRESS_1_ERROR_MESSAGE, name, USAddressValidator.ADDRESS_MAX_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestAddress1_ExceedsMaxLength()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Address1 = new string('c', USAddressValidator.ADDRESS_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(string.Format(USAddressValidator.ADDRESS_1_ERROR_MESSAGE, name, USAddressValidator.ADDRESS_MAX_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestAddress2_Null()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Address2 = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestAddress2_ExceedsMaxLength()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Address2 = new String('c', USAddressValidator.ADDRESS_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(string.Format(USAddressValidator.ADDRESS_2_ERROR_MESSAGE, name, USAddressValidator.ADDRESS_MAX_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestPostalCode_Null()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(String.Format(USAddressValidator.POSTAL_CODE_ERROR_MESSAGE, name, USAddressValidator.POSTAL_CODE_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestPostalCode_AllDigitsExceedsLength()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = new String('1', USAddressValidator.POSTAL_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(String.Format(USAddressValidator.POSTAL_CODE_ERROR_MESSAGE, name, USAddressValidator.POSTAL_CODE_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestPostalCode_DoesNotContainDigits()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = new String('a', USAddressValidator.POSTAL_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(String.Format(USAddressValidator.POSTAL_CODE_ERROR_MESSAGE, name, USAddressValidator.POSTAL_CODE_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestPostalCode_DoesNotHaveRequiredNumberOfDigits()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = new String('1', USAddressValidator.POSTAL_CODE_LENGTH - 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(String.Format(USAddressValidator.POSTAL_CODE_ERROR_MESSAGE, name, USAddressValidator.POSTAL_CODE_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestExplanationCode_Null()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.ExplanationCode = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestExplanationCode_ExceedsMaxLength()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.ExplanationCode = new string('c', USAddressValidator.EXPLANATION_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(string.Format(USAddressValidator.EXPLANATION_CODE_ERROR_MESSAGE, name, USAddressValidator.EXPLANATION_CODE_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestExplanation_Null()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Explanation = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestExplanation_ExceedsMaxLength()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Explanation = new string('c', USAddressValidator.EXPLANATION_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(String.Format(USAddressValidator.EXPLANATION_ERROR_MESSAGE, name, USAddressValidator.EXPLANATION_MIN_LENGTH, USAddressValidator.EXPLANATION_MAX_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestExplanation_DoesNotMeetMinLength()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Explanation = new string('c', USAddressValidator.EXPLANATION_MIN_LENGTH - 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(String.Format(USAddressValidator.EXPLANATION_ERROR_MESSAGE, name, USAddressValidator.EXPLANATION_MIN_LENGTH, USAddressValidator.EXPLANATION_MAX_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestCity_ExceedsMaxLength()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.City = new string('c', USAddressValidator.CITY_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(string.Format(USAddressValidator.CITY_ERROR_MESSAGE, name, USAddressValidator.CITY_MAX_LENGTH), result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestCity_IsEmpty()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.City = string.Empty;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestCity_IsNull()
        {
            var name = "address name";
            var validator = new USAddressValidator(name);
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.City = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }
    }
}
