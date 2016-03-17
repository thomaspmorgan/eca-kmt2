using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Business.Validation.Sevis.Finance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class OtherFundsValidatorTest
    {

        [TestMethod]
        public void TestOther_ShouldRunValidator()
        {
            string binationalCommission = "1";
            string personal = "2";
            string evGovt = "3";
            Other other = new Other("name", "1");
            USGovt usGovt = new USGovt("us 1", null, "1", null, null, null);
            International international = new International("int 1", null, "2", null, null, null);
            Func<OtherFunds> createEntity = () =>
             {
                 return new OtherFunds(evGovt: evGovt, binationalCommission: binationalCommission, personal: personal, usGovt: usGovt, international: international, other: other);
             };

            other = null;
            var validator = new OtherFundsValidator();
            var instance = createEntity();
            Assert.IsNull(instance.Other);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            other = new Other("name", "amount");
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void TestUSGovt_ShouldRunValidator()
        {
            string binationalCommission = "1";
            string personal = "2";
            string evGovt = "3";
            Other other = new Other("name", "1");
            USGovt usGovt = new USGovt("us 1", null, "1", null, null, null);
            International international = new International("int 1", null, "2", null, null, null);
            Func<OtherFunds> createEntity = () =>
            {
                return new OtherFunds(evGovt: evGovt, binationalCommission: binationalCommission, personal: personal, usGovt: usGovt, international: international, other: other);
            };

            usGovt = null;
            var validator = new OtherFundsValidator();
            var instance = createEntity();
            Assert.IsNull(instance.USGovt);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            usGovt = new USGovt(null, null, null, null, null, null);
            instance = createEntity();

            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public void TestInternational_ShouldRunValidator()
        {
            string binationalCommission = "1";
            string personal = "2";
            string evGovt = "3";
            Other other = new Other("name", "1");
            USGovt usGovt = new USGovt("us 1", null, "1", null, null, null);
            International international = null;
            Func<OtherFunds> createEntity = () =>
            {
                return new OtherFunds(evGovt: evGovt, binationalCommission: binationalCommission, personal: personal, usGovt: usGovt, international: international, other: other);
            };
            var validator = new OtherFundsValidator();
            var instance = createEntity();
            Assert.IsNull(instance.International);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            international = new International(null, null, null, null, null, null);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void TestEVGovt_Null()
        {
            string binationalCommission = "1";
            string personal = "2";
            string evGovt = "3";
            Other other = new Other("name", "1");
            USGovt usGovt = new USGovt("us 1", null, "1", null, null, null);
            International international = new International("int 1", null, "2", null, null, null);
            Func<OtherFunds> createEntity = () =>
            {
                return new OtherFunds(evGovt: evGovt, binationalCommission: binationalCommission, personal: personal, usGovt: usGovt, international: international, other: other);
            };
            var validator = new OtherFundsValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            evGovt = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestEVGovt_ExceedsMaxLength()
        {
            string binationalCommission = "1";
            string personal = "2";
            string evGovt = "3";
            Other other = new Other("name", "1");
            USGovt usGovt = new USGovt("us 1", null, "1", null, null, null);
            International international = new International("int 1", null, "2", null, null, null);
            Func<OtherFunds> createEntity = () =>
            {
                return new OtherFunds(evGovt: evGovt, binationalCommission: binationalCommission, personal: personal, usGovt: usGovt, international: international, other: other);
            };
            var validator = new OtherFundsValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            evGovt = new string('1', OtherFundsValidator.AMOUNT_MAX_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherFundsValidator.EXCHANGE_VISITOR_GOVERNMENT_FUNDING_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }


        [TestMethod]
        public void TestEVGovt_DoesNotContainDigits()
        {
            string binationalCommission = "1";
            string personal = "2";
            string evGovt = "3";
            Other other = new Other("name", "1");
            USGovt usGovt = new USGovt("us 1", null, "1", null, null, null);
            International international = new International("int 1", null, "2", null, null, null);
            Func<OtherFunds> createEntity = () =>
            {
                return new OtherFunds(evGovt: evGovt, binationalCommission: binationalCommission, personal: personal, usGovt: usGovt, international: international, other: other);
            };
            var validator = new OtherFundsValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            evGovt = "a";
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherFundsValidator.EXCHANGE_VISITOR_GOVERNMENT_FUNDING_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestBinationalCommission_Null()
        {
            string binationalCommission = "1";
            string personal = "2";
            string evGovt = "3";
            Other other = new Other("name", "1");
            USGovt usGovt = new USGovt("us 1", null, "1", null, null, null);
            International international = new International("int 1", null, "2", null, null, null);
            Func<OtherFunds> createEntity = () =>
            {
                return new OtherFunds(evGovt: evGovt, binationalCommission: binationalCommission, personal: personal, usGovt: usGovt, international: international, other: other);
            };
            var validator = new OtherFundsValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            binationalCommission = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestBinationalCommission_ExceedsMaxLength()
        {
            string binationalCommission = "1";
            string personal = "2";
            string evGovt = "3";
            Other other = new Other("name", "1");
            USGovt usGovt = new USGovt("us 1", null, "1", null, null, null);
            International international = new International("int 1", null, "2", null, null, null);
            Func<OtherFunds> createEntity = () =>
            {
                return new OtherFunds(evGovt: evGovt, binationalCommission: binationalCommission, personal: personal, usGovt: usGovt, international: international, other: other);
            };
            var validator = new OtherFundsValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            binationalCommission = new string('1', OtherFundsValidator.AMOUNT_MAX_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherFundsValidator.BINATIONAL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }


        [TestMethod]
        public void TestBinationalCommission_DoesNotContainDigits()
        {
            string binationalCommission = "1";
            string personal = "2";
            string evGovt = "3";
            Other other = new Other("name", "1");
            USGovt usGovt = new USGovt("us 1", null, "1", null, null, null);
            International international = new International("int 1", null, "2", null, null, null);
            Func<OtherFunds> createEntity = () =>
            {
                return new OtherFunds(evGovt: evGovt, binationalCommission: binationalCommission, personal: personal, usGovt: usGovt, international: international, other: other);
            };
            var validator = new OtherFundsValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            binationalCommission = "a";
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherFundsValidator.BINATIONAL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestPersonal_Null()
        {
            string binationalCommission = "1";
            string personal = "2";
            string evGovt = "3";
            Other other = new Other("name", "1");
            USGovt usGovt = new USGovt("us 1", null, "1", null, null, null);
            International international = new International("int 1", null, "2", null, null, null);
            Func<OtherFunds> createEntity = () =>
            {
                return new OtherFunds(evGovt: evGovt, binationalCommission: binationalCommission, personal: personal, usGovt: usGovt, international: international, other: other);
            };
            var validator = new OtherFundsValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            personal = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestPersonal_ExceedsMaxLength()
        {
            string binationalCommission = "1";
            string personal = "2";
            string evGovt = "3";
            Other other = new Other("name", "1");
            USGovt usGovt = new USGovt("us 1", null, "1", null, null, null);
            International international = new International("int 1", null, "2", null, null, null);
            Func<OtherFunds> createEntity = () =>
            {
                return new OtherFunds(evGovt: evGovt, binationalCommission: binationalCommission, personal: personal, usGovt: usGovt, international: international, other: other);
            };
            var validator = new OtherFundsValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            personal = new string('1', OtherFundsValidator.AMOUNT_MAX_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherFundsValidator.PERSONAL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }


        [TestMethod]
        public void TestPersonal_DoesNotContainDigits()
        {
            string binationalCommission = "1";
            string personal = "2";
            string evGovt = "3";
            Other other = new Other("name", "1");
            USGovt usGovt = new USGovt("us 1", null, "1", null, null, null);
            International international = new International("int 1", null, "2", null, null, null);
            Func<OtherFunds> createEntity = () =>
            {
                return new OtherFunds(evGovt: evGovt, binationalCommission: binationalCommission, personal: personal, usGovt: usGovt, international: international, other: other);
            };
            var validator = new OtherFundsValidator();
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            personal = "a";
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(OtherFundsValidator.PERSONAL_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }
    }
}
