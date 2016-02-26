using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model.Shared;

namespace ECA.Business.Test.Validation.Model.CreateEV
{
    [TestClass]
    public class OtherFundsValidatorTest
    {
        public OtherFunds GetValidOtherFunds()
        {
            return new OtherFunds
            {   
            };
        }

        [TestMethod]
        public void TestIsValid()
        {
            var validator = new OtherFundsValidator();
            var instance = GetValidOtherFunds();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }
    }
}
