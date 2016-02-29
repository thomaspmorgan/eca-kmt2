using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model.Shared;

namespace ECA.Business.Test.Validation.Model.CreateEV
{
    [TestClass]
    public class InternationalValidatorTest
    {
        public International GetValidInternational()
        {
            return new International
            {
            };
        }

        [TestMethod]
        public void TestOrg1Code_NotOtherOrgCode_NotRequiredCodeLength()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();
            instance.Org1 = new string('a', InternationalValidator.ORG_CODE_MAX_LENGTH);
            instance.Amount1 = "1";
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Org1 = String.Empty;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.ORG_1_CODE_NOT_SPECIFIED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);

            instance.Org1 = new string('a', InternationalValidator.ORG_CODE_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.ORG_1_CODE_NOT_SPECIFIED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestOrg1Code_IsOtherOrgCode_OtherNameExceedsLength()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();

            instance.Org1 = InternationalValidator.OTHER_ORG_CODE;
            instance.Amount1 = "1";
            instance.OtherName1 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.OtherName1 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.OTHER_ORG_1_NAME_REQUIRED, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestOrg1Code_IsOtherOrgCode_OtherNameNull()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();

            instance.Org1 = InternationalValidator.OTHER_ORG_CODE;
            instance.Amount1 = "1";
            instance.OtherName1 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH);
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.OtherName1 = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.OTHER_ORG_1_NAME_REQUIRED, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestAmount1_Null()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();
            instance.Org1 = "abc";
            instance.Amount1 = "1234";
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Amount1 = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestAmount1_ExceedsMaxLength()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();
            instance.Org1 = "abc";
            instance.Amount1 = "1234";
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Amount1 = new string('3', InternationalValidator.AMOUNT_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }


        [TestMethod]
        public void TestAmount1_DoesNotContainNumbers()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();
            instance.Org1 = "abc";
            instance.Amount1 = "1234";
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Amount1 = "a";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestAmount2_IsNull()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();
            instance.Org1 = "abc";
            instance.Amount1 = "1";
            instance.Amount2 = "1234";
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Amount2 = null;
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestAmount2_DoesNotContainDigits()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();
            instance.Org1 = "abc";
            instance.Amount1 = "1";
            instance.Amount2 = "1234";
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Amount2 = "abc";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestAmount2_ExceedsMaxLength()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();
            instance.Org1 = "abc";
            instance.Amount1 = "1";
            instance.Amount2 = "1234";
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Amount2 = new string('1', InternationalValidator.AMOUNT_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestAmount2_Org2IsNotNull()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();
            instance.Org1 = "abc";
            instance.Amount1 = "1";
            instance.Org2 = null;
            instance.Amount2 = null;

            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Org2 = "xxx";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestAmount2_OtherName2IsNotNull()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();
            instance.Org1 = "abc";
            instance.Amount1 = "1";
            instance.Org2 = null;
            instance.Amount2 = null;

            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Org2 = InternationalValidator.OTHER_ORG_CODE;
            instance.OtherName2 = "a";
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }


        [TestMethod]
        public void TestOrg2Code_NotOtherOrgCode_NotRequiredCodeLength()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();
            instance.Org1 = new string('a', InternationalValidator.ORG_CODE_MAX_LENGTH);
            instance.Amount1 = "1";
            instance.Org2 = new string('a', InternationalValidator.ORG_CODE_MAX_LENGTH);
            instance.Amount2 = "1";

            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.Org2 = String.Empty;
            result = validator.Validate(instance);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.ORG_2_CODE_NOT_SPECIFIED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);

            instance.Org2 = new string('a', InternationalValidator.ORG_CODE_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.ORG_2_CODE_NOT_SPECIFIED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestOrg2Code_IsOtherOrgCode_OtherNameExceedsLength()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();

            instance.Org1 = InternationalValidator.OTHER_ORG_CODE;
            instance.Amount1 = "1";
            instance.OtherName1 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH);

            instance.Org2 = InternationalValidator.OTHER_ORG_CODE;
            instance.Amount2 = "1";
            instance.OtherName2 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH);

            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.OtherName2 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH + 1);
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.OTHER_ORG_2_NAME_REQUIRED, result.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestOrg2Code_IsOtherOrgCode_OtherNameNull()
        {
            var validator = new InternationalValidator();
            var instance = GetValidInternational();

            instance.Org1 = InternationalValidator.OTHER_ORG_CODE;
            instance.Amount1 = "1";
            instance.OtherName1 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH);


            instance.Org2 = InternationalValidator.OTHER_ORG_CODE;
            instance.Amount2 = "1";
            instance.OtherName2 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH);

            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            instance.OtherName2 = null;
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.OTHER_ORG_2_NAME_REQUIRED, result.Errors.First().ErrorMessage);
        }
    }
}
