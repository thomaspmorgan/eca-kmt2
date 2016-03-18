using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Business.Validation.Sevis.Finance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class OtherValidatorTest
    {

        [TestMethod]
        public void TestName_Null()
        {
            string name = null;
            string amount = null;
            Func<Other> createEntity = () =>
            {
                return new Other(name: name, amount: amount);
            };
            var validator = new OtherValidator();
            
            name = "name";
            amount = "1";
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            name = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherValidator.OTHER_ORGNAIZATION_FUNDING_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestName_ExceedsMaxLength()
        {
            string name = null;
            string amount = null;
            Func<Other> createEntity = () =>
            {
                return new Other(name: name, amount: amount);
            };
            var validator = new OtherValidator();
            
            name = "name";
            amount = "1";
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            name = new string('c', OtherValidator.NAME_MAX_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherValidator.OTHER_ORGNAIZATION_FUNDING_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestAmount_ExceedsMaxLength()
        {
            string name = null;
            string amount = null;
            Func<Other> createEntity = () =>
            {
                return new Other(name: name, amount: amount);
            };
            var validator = new OtherValidator();
            
            name = "name";
            amount = "1";
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            amount = new string('1', OtherValidator.AMOUNT_MAX_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestAmount_DoesNotContainDigits()
        {
            string name = null;
            string amount = null;
            Func<Other> createEntity = () =>
            {
                return new Other(name: name, amount: amount);
            };
            var validator = new OtherValidator();
            
            name = "name";
            amount = "1";
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            amount = "a";
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestAmount_Null()
        {
            string name = null;
            string amount = null;
            Func<Other> createEntity = () =>
            {
                return new Other(name: name, amount: amount);
            };
            var validator = new OtherValidator();

            name = "name";
            amount = "1";
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            amount = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }
    }
}
