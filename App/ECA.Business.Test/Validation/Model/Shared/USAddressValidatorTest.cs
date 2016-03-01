using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.Shared;

namespace ECA.Business.Test.Validation.Model.Shared
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
            var validator = new USAddressValidator();
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Address1 = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(USAddressValidator.ADDRESS_1_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestAddress1_ExceedsMaxLength()
        {
            var validator = new USAddressValidator();
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Address1 = new string('c', USAddressValidator.ADDRESS_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(USAddressValidator.ADDRESS_1_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestAddress2_Null()
        {
            var validator = new USAddressValidator();
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
            var validator = new USAddressValidator();
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Address2 = new String('c', USAddressValidator.ADDRESS_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(USAddressValidator.ADDRESS_2_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestPostalCode_Null()
        {
            var validator = new USAddressValidator();
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(USAddressValidator.POSTAL_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestPostalCode_AllDigitsExceedsLength()
        {
            var validator = new USAddressValidator();
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = new String('1', USAddressValidator.POSTAL_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(USAddressValidator.POSTAL_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestPostalCode_DoesNotContainDigits()
        {
            var validator = new USAddressValidator();
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = new String('a', USAddressValidator.POSTAL_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(USAddressValidator.POSTAL_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestPostalCode_DoesNotHaveRequiredNumberOfDigits()
        {
            var validator = new USAddressValidator();
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.PostalCode = new String('1', USAddressValidator.POSTAL_CODE_LENGTH - 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(USAddressValidator.POSTAL_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestExplanationCode_Null()
        {
            var validator = new USAddressValidator();
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
            var validator = new USAddressValidator();
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.ExplanationCode = new string('c', USAddressValidator.EXPLANATION_CODE_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(USAddressValidator.EXPLANATION_CODE_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestExplanation_Null()
        {
            var validator = new USAddressValidator();
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
            var validator = new USAddressValidator();
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Explanation = new string('c', USAddressValidator.EXPLANATION_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(USAddressValidator.EXPLAINATION_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestExplanation_DoesNotMeetMinLength()
        {
            var validator = new USAddressValidator();
            var instance = GetValidUSAddress();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Explanation = new string('c', USAddressValidator.EXPLANATION_MIN_LENGTH - 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(USAddressValidator.EXPLAINATION_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }
    }
}
