﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.Shared;

namespace ECA.Business.Test.Validation.Model.CreateEV
{
    [TestClass]
    public class FinancialInfoValidatorTest
    {

        public FinancialInfo GetValidFinancialInfo()
        {
            return new FinancialInfo
            {
                ProgramSponsorFunds = "123",
                OtherFunds = new OtherFunds(),
            };
        }

        [TestMethod]
        public void TestProgramSponsor_ExceedsMaxLength()
        {
            var validator = new FinancialInfoValidator();
            var instance = GetValidFinancialInfo();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.ProgramSponsorFunds = new string('c', FinancialInfoValidator.SPONSOR_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(FinancialInfoValidator.PROGRAM_SPONSOR_FUNDS_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestProgramSponsor_Null()
        {
            var validator = new FinancialInfoValidator();
            var instance = GetValidFinancialInfo();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.ProgramSponsorFunds = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestOtherFunds_Null()
        {
            var validator = new FinancialInfoValidator();
            var instance = GetValidFinancialInfo();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.OtherFunds = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(FinancialInfoValidator.OTHER_FUNDS_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }
        
    }
}
