using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Finance;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class FinancialInfoTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovt usGovt = new USGovt(null, null, null, null, null, null);
            International international = new International(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);
            
            var programSponsorFunds = "prog sponsor funds";
            var receivedUsGovtFunds = true;
            var printForm = true;
            var instance = new FinancialInfo(
                printForm,
                receivedUsGovtFunds,
                programSponsorFunds,
                otherFunds);

            Assert.IsTrue(Object.ReferenceEquals(otherFunds, instance.OtherFunds));
            Assert.AreEqual(programSponsorFunds, instance.ProgramSponsorFunds);
            Assert.AreEqual(receivedUsGovtFunds, instance.ReceivedUSGovtFunds);
            Assert.AreEqual(printForm, instance.PrintForm);
        }

        [TestMethod]
        public void TestConstructor_PrintFormIsFalse()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovt usGovt = new USGovt(null, null, null, null, null, null);
            International international = new International(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);

            var programSponsorFunds = "prog sponsor funds";
            var receivedUsGovtFunds = true;
            var printForm = false;
            var instance = new FinancialInfo(
                printForm,
                receivedUsGovtFunds,
                programSponsorFunds,
                otherFunds);

            Assert.AreEqual(receivedUsGovtFunds, instance.ReceivedUSGovtFunds);
            Assert.AreEqual(printForm, instance.PrintForm);
        }

        [TestMethod]
        public void TestConstructor_ReceivedUSGovtFundsIsFalse()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovt usGovt = new USGovt(null, null, null, null, null, null);
            International international = new International(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);
            var programSponsorFunds = "prog sponsor funds";
            var receivedUsGovtFunds = false;
            var printForm = false;
            var instance = new FinancialInfo(
                printForm,
                receivedUsGovtFunds,
                programSponsorFunds,
                otherFunds);

            Assert.AreEqual(receivedUsGovtFunds, instance.ReceivedUSGovtFunds);
            Assert.AreEqual(printForm, instance.PrintForm);
        }

        [TestMethod]
        public void TestGetEVPersonTypeFinancialInfo()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovt usGovt = new USGovt(null, null, null, null, null, null);
            International international = new International(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);

            var programSponsorFunds = "prog sponsor funds";
            var receivedUsGovtFunds = true;
            var printForm = true;
            var financialInfo = new FinancialInfo(
                printForm: printForm, 
                receivedUSGovtFunds: receivedUsGovtFunds, 
                programSponsorFunds: programSponsorFunds, 
                otherFunds: otherFunds);

            var instance = financialInfo.GetEVPersonTypeFinancialInfo();
            Assert.IsNotNull(instance.OtherFunds);
            Assert.AreEqual(financialInfo.ProgramSponsorFunds, instance.ProgramSponsorFunds);
            Assert.AreEqual(financialInfo.ReceivedUSGovtFunds, instance.ReceivedUSGovtFunds);
        }

        [TestMethod]
        public void TestGetEVPersonTypeFinancialInfo_NullOtherFunds()
        {
            OtherFunds otherFunds = null;
            var programSponsorFunds = "prog sponsor funds";
            var receivedUsGovtFunds = true;
            var printForm = true;
            var financialInfo = new FinancialInfo(
                printForm: printForm,
                receivedUSGovtFunds: receivedUsGovtFunds,
                programSponsorFunds: programSponsorFunds,
                otherFunds: otherFunds);
            var instance = financialInfo.GetEVPersonTypeFinancialInfo();
            Assert.IsNull(instance.OtherFunds);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorFinancialInfo()
        {
            string binationalCommission = "commission";
            string personal = "peronsal";
            string evGovt = "ev gov";
            Other other = new Other("name", "amount");
            USGovt usGovt = new USGovt(null, null, null, null, null, null);
            International international = new International(null, null, null, null, null, null);

            var otherFunds = new OtherFunds(
                evGovt: evGovt,
                binationalCommission: binationalCommission,
                personal: personal,
                usGovt: usGovt,
                international: international,
                other: other);

            var programSponsorFunds = "prog sponsor funds";
            var receivedUsGovtFunds = true;
            var printForm = true;
            var financialInfo = new FinancialInfo(
                printForm: printForm,
                receivedUSGovtFunds: receivedUsGovtFunds,
                programSponsorFunds: programSponsorFunds,
                otherFunds: otherFunds);

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
            OtherFunds otherFunds = null;
            var programSponsorFunds = "prog sponsor funds";
            var receivedUsGovtFunds = true;
            var printForm = true;
            var financialInfo = new FinancialInfo(
                printForm: printForm,
                receivedUSGovtFunds: receivedUsGovtFunds,
                programSponsorFunds: programSponsorFunds,
                otherFunds: otherFunds);

            var instance = financialInfo.GetSEVISEVBatchTypeExchangeVisitorFinancialInfo();
            Assert.IsNull(instance.OtherFunds);
        }
    }
}
