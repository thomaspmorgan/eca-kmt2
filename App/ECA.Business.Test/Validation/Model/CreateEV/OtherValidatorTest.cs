﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model.Shared;

namespace ECA.Business.Test.Validation.Model.CreateEV
{
    [TestClass]
    public class OtherValidatorTest
    {

        [TestMethod]
        public void TestName_Null()
        {
            var validator = new OtherValidator();
            var instance = new Other();
            instance.Name = "name";
            instance.Amount = "1";
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Name = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherValidator.OTHER_ORGNAIZATION_FUNDING_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestName_ExceedsMaxLength()
        {
            var validator = new OtherValidator();
            var instance = new Other();
            instance.Name = "name";
            instance.Amount = "1";
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Name = new string('c', OtherValidator.NAME_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherValidator.OTHER_ORGNAIZATION_FUNDING_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestAmount_ExceedsMaxLength()
        {
            var validator = new OtherValidator();
            var instance = new Other();
            instance.Name = "name";
            instance.Amount = "1";
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Amount = new string('1', OtherValidator.AMOUNT_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestAmount_DoesNotContainDigits()
        {
            var validator = new OtherValidator();
            var instance = new Other();
            instance.Name = "name";
            instance.Amount = "1";
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Amount = "a";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }
    }
}
