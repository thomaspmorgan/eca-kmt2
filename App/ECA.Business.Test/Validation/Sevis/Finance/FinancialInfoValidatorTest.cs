using ECA.Business.Validation.Sevis.Finance;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class FinancialInfoValidatorTest
    {

        public FinancialInfo GetValidFinancialInfo()
        {
            return new FinancialInfo
            {
                ProgramSponsorFunds = "123",
                OtherFunds = null
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
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
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
        public void TestOtherFunds_ShouldRunValidator()
        {
            var validator = new FinancialInfoValidator();
            var instance = GetValidFinancialInfo();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.OtherFunds = new OtherFunds();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
        }
    }
}
