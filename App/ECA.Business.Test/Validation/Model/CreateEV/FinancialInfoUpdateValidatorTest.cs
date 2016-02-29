using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.Shared;

namespace ECA.Business.Test.Validation.Model.CreateEV
{
    [TestClass]
    public class FinancialInfoUpdateValidatorTest
    {

        public FinancialInfoUpdate GetValidFinancialInfoUpdate()
        {
            return new FinancialInfoUpdate
            {
                ProgramSponsorFunds = "123",
                OtherFunds = new OtherFunds(),
            };
        }

        [TestMethod]
        public void TestIsValid()
        {
            var validator = new FinancialInfoUpdateValidator();
            var instance = GetValidFinancialInfoUpdate();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            
        }
        
    }
}
