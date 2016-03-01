using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model.Shared;
using ECA.Business.Validation.SEVIS;

namespace ECA.Business.Test.Validation.Model.Shared
{
    [TestClass]
    public class OtherFundsValidatorTest
    {
        public OtherFunds GetValidOtherFunds()
        {
            var instance = new OtherFunds();
            instance.Other = null;
            instance.International = null;
            instance.USGovt = null;
            return instance;
        }

        [TestMethod]
        public void TestOther_ShouldRunValidator()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            Assert.IsNull(instance.Other);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Other = new Other();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void TestUSGovt_ShouldRunValidator()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            Assert.IsNull(instance.Other);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.USGovt = new USGovt();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public void TestInternational_ShouldRunValidator()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            Assert.IsNull(instance.Other);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.International = new International();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void TestEVGovt_Null()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.EVGovt = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestEVGovt_ExceedsMaxLength()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.EVGovt = new string('1', OtherFundsValidator.AMOUNT_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherFundsValidator.EV_GOVT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PersonMoneyFlowErrorPath));
        }


        [TestMethod]
        public void TestEVGovt_DoesNotContainDigits()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.EVGovt = "a";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherFundsValidator.EV_GOVT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PersonMoneyFlowErrorPath));
        }

        [TestMethod]
        public void TestBinationalCommission_Null()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BinationalCommission = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestBinationalCommission_ExceedsMaxLength()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BinationalCommission = new string('1', OtherFundsValidator.AMOUNT_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherFundsValidator.BINATIONAL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PersonMoneyFlowErrorPath));
        }


        [TestMethod]
        public void TestBinationalCommission_DoesNotContainDigits()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.BinationalCommission = "a";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherFundsValidator.BINATIONAL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PersonMoneyFlowErrorPath));
        }

        [TestMethod]
        public void TestPersonal_Null()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Personal = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestPersonal_ExceedsMaxLength()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Personal = new string('1', OtherFundsValidator.AMOUNT_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherFundsValidator.PERSONAL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PersonMoneyFlowErrorPath));
        }


        [TestMethod]
        public void TestPersonal_DoesNotContainDigits()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Personal = "a";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherFundsValidator.PERSONAL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(PersonMoneyFlowErrorPath));
        }
    }
}
