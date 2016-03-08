using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ECA.Business.Validation;
using FluentValidation.Results;
using ECA.Business.Validation.SEVIS.ErrorPaths;

namespace ECA.Business.Test.Validation
{
    [TestClass]
    public class SimpleValidationResultTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var isValid = true;
            var errors = new List<SimpleValidationFailure>();
            var instance = new SimpleValidationResult(isValid, errors);
            Assert.AreEqual(isValid, instance.IsValid);
            Assert.IsTrue(Object.ReferenceEquals(errors, instance.Errors));
        }

        [TestMethod]
        public void TestConstructor_NullErrors()
        {
            var isValid = true;
            var instance = new SimpleValidationResult(isValid, null);
            Assert.AreEqual(isValid, instance.IsValid);
            Assert.IsNotNull(instance.Errors);
            Assert.AreEqual(0, instance.Errors.Count());
        }

        [TestMethod]
        public void TestConstructor_ValidationResult_HasErrorsWithErrorPath()
        {   
            var validationFailure = new ValidationFailure("property", "error");
            validationFailure.CustomState = new FundingErrorPath();

            var validationFailures = new List<ValidationFailure>();
            validationFailures.Add(validationFailure);
            var validationResult = new ValidationResult(validationFailures);
            Assert.IsFalse(validationResult.IsValid);

            var instance = new SimpleValidationResult(validationResult);
            Assert.IsFalse(instance.IsValid);
            Assert.AreEqual(1, instance.Errors.Count());
            var firstError = instance.Errors.First();
            Assert.AreEqual(validationFailure.PropertyName, firstError.PropertyName);
            Assert.AreEqual(validationFailure.ErrorMessage, firstError.ErrorMessage);
            Assert.IsTrue(Object.ReferenceEquals(validationFailure.CustomState, firstError.CustomState));
        }

        [TestMethod]
        public void TestConstructor_ValidationResult_HasErrorsWithoutErrorPath()
        {
            var validationFailure = new ValidationFailure("property", "error");
            validationFailure.CustomState = null;

            var validationFailures = new List<ValidationFailure>();
            validationFailures.Add(validationFailure);
            var validationResult = new ValidationResult(validationFailures);
            Assert.IsFalse(validationResult.IsValid);

            var instance = new SimpleValidationResult(validationResult);
            Assert.IsFalse(instance.IsValid);
            Assert.AreEqual(1, instance.Errors.Count());
            var firstError = instance.Errors.First();
            Assert.IsNull(firstError.CustomState);
        }

        [TestMethod]
        public void TestConstructor_ValidationResult_IsValid()
        {
            var validationFailures = new List<ValidationFailure>();
            var validationResult = new ValidationResult(validationFailures);
            Assert.IsTrue(validationResult.IsValid);

            var instance = new SimpleValidationResult(validationResult);
            Assert.IsTrue(instance.IsValid);
            Assert.AreEqual(0, instance.Errors.Count());
        }
    }
}
