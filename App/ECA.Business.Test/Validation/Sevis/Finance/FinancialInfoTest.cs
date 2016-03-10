using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Finance;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class FinancialInfoTest
    {
        [TestMethod]
        public void TestGetEVPersonTypeFinancialInfo()
        {
            var financialInfo = new FinancialInfo
            {
                OtherFunds = new OtherFunds(),
                ProgramSponsorFunds = "prog sponsor funds",
                ReceivedUSGovtFunds = true
            };

            var instance = financialInfo.GetEVPersonTypeFinancialInfo();
            Assert.IsNotNull(instance.OtherFunds);
            Assert.AreEqual(financialInfo.ProgramSponsorFunds, instance.ProgramSponsorFunds);
            Assert.AreEqual(financialInfo.ReceivedUSGovtFunds, instance.ReceivedUSGovtFunds);
        }

        [TestMethod]
        public void TestGetEVPersonTypeFinancialInfo_NullOtherFunds()
        {
            var financialInfo = new FinancialInfo
            {
                OtherFunds = null
            };

            var instance = financialInfo.GetEVPersonTypeFinancialInfo();
            Assert.IsNull(instance.OtherFunds);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorFinancialInfo()
        {
            var financialInfo = new FinancialInfo
            {
                OtherFunds = new OtherFunds(),
                PrintForm = true,
                ProgramSponsorFunds = "sponsor funds",
                ReceivedUSGovtFunds = true
            };

            var instance = financialInfo.GetSEVISEVBatchTypeExchangeVisitorFinancialInfo();
            Assert.IsNotNull(instance.OtherFunds);
            Assert.AreEqual(financialInfo.PrintForm, instance.printForm);
            Assert.AreEqual(financialInfo.ProgramSponsorFunds, instance.ProgramSponsorFunds);
            Assert.AreEqual(financialInfo.ReceivedUSGovtFunds, instance.ReceivedUSGovtFunds);
            Assert.IsTrue(instance.ReceivedUSGovtFundsSpecified);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorFinancialInfo_OtherFundsIsNull()
        {
            var financialInfo = new FinancialInfo
            {
                OtherFunds = null,
                PrintForm = true,
                ProgramSponsorFunds = "sponsor funds",
                ReceivedUSGovtFunds = true
            };

            var instance = financialInfo.GetSEVISEVBatchTypeExchangeVisitorFinancialInfo();
            Assert.IsNull(instance.OtherFunds);
        }
    }
}
