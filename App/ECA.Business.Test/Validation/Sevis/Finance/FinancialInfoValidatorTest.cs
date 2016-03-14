using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Business.Validation.Sevis.Finance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class FinancialInfoValidatorTest
    {


        [TestMethod]
        public void TestProgramSponsor_ExceedsMaxLength()
        {
            var otherFunds = new OtherFunds(null, null, null, null, null, null);
            var programSponsorFunds = "100";
            var receivedUsGovtFunds = true;
            var printForm = true;
            Func<FinancialInfo> createEntity = () =>
            {   
                var financialInfo = new FinancialInfo(
                    printForm: printForm,
                    receivedUSGovtFunds: receivedUsGovtFunds,
                    programSponsorFunds: programSponsorFunds,
                    otherFunds: otherFunds);
                return financialInfo;
            };

            var validator = new FinancialInfoValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            programSponsorFunds = new string('c', FinancialInfoValidator.SPONSOR_MAX_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(FinancialInfoValidator.PROGRAM_SPONSOR_FUNDS_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestProgramSponsor_Null()
        {
            var otherFunds = new OtherFunds(null, null, null, null, null, null);
            var programSponsorFunds = "100";
            var receivedUsGovtFunds = true;
            var printForm = true;
            Func<FinancialInfo> createEntity = () =>
            {

                var financialInfo = new FinancialInfo(
                    printForm: printForm,
                    receivedUSGovtFunds: receivedUsGovtFunds,
                    programSponsorFunds: programSponsorFunds,
                    otherFunds: otherFunds);
                return financialInfo;
            };
            var validator = new FinancialInfoValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            programSponsorFunds = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestOtherFunds_ShouldRunValidator()
        {
            OtherFunds otherFunds = null;
            var programSponsorFunds = "100";
            var receivedUsGovtFunds = true;
            var printForm = true;
            Func<FinancialInfo> createEntity = () =>
            {

                var financialInfo = new FinancialInfo(
                    printForm: printForm,
                    receivedUSGovtFunds: receivedUsGovtFunds,
                    programSponsorFunds: programSponsorFunds,
                    otherFunds: otherFunds);
                return financialInfo;
            };
            var validator = new FinancialInfoValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            otherFunds = new OtherFunds("a", null, null, null, null, null);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
        }
    }
}
