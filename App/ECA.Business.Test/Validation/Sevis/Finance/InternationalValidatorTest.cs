﻿using ECA.Business.Validation.Sevis.Finance;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class InternationalValidatorTest
    {

        [TestMethod]
        public void TestOrg1Code_NotOtherOrgCode_NotRequiredCodeLength()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
             {
                 return new International(
                     org1: org1,
                     otherName1: otherName1,
                     amount1: amount1,
                     org2: org2,
                     otherName2: otherName2,
                     amount2: amount2
                 );
             };
            var validator = new InternationalValidator();
            
            org1 = new string('a', InternationalValidator.ORG_CODE_MAX_LENGTH);
            amount1 = "1";
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            org1 = String.Empty;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.ORG_1_CODE_NOT_SPECIFIED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));

            org1 = new string('a', InternationalValidator.ORG_CODE_MAX_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.ORG_1_CODE_NOT_SPECIFIED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestOrg1Code_IsOtherOrgCode_OtherNameExceedsLength()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();

            org1 = InternationalValidator.OTHER_ORG_CODE;
            amount1 = "1";
            otherName1 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH);
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            otherName1 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.OTHER_ORG_1_NAME_REQUIRED, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestOrg1Code_IsOtherOrgCode_OtherNameNull()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();

            org1 = InternationalValidator.OTHER_ORG_CODE;
            amount1 = "1";
            otherName1 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH);
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            otherName1 = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.OTHER_ORG_1_NAME_REQUIRED, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestAmount1_Null()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();
            org1 = "abc";
            amount1 = "1234";
            var instance = createEntity();
            
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            amount1 = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestAmount1_ExceedsMaxLength()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();
            
            org1 = "abc";
            amount1 = "1234";
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            amount1 = new string('3', InternationalValidator.AMOUNT_MAX_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }


        [TestMethod]
        public void TestAmount1_DoesNotContainNumbers()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();
            org1 = "abc";
            amount1 = "1234";
            var instance = createEntity();
            
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            amount1 = "a";
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestAmount2_IsNull()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();
            
            org1 = "abc";
            amount1 = "1";
            amount2 = "1234";
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            amount2 = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void TestAmount2_DoesNotContainDigits()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();
            
            org1 = "abc";
            amount1 = "1";
            amount2 = "1234";
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            amount2 = "abc";
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestAmount2_ExceedsMaxLength()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();
            
            org1 = "abc";
            amount1 = "1";
            amount2 = "1234";
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            amount2 = new string('1', InternationalValidator.AMOUNT_MAX_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestAmount2_Org2IsNotNull()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();
            
            org1 = "abc";
            amount1 = "1";
            org2 = null;
            amount2 = null;

            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            org2 = "xxx";
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestAmount2_OtherName2IsNotNull()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();
            org1 = "abc";
            amount1 = "1";
            org2 = null;
            amount2 = null;
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            org2 = InternationalValidator.OTHER_ORG_CODE;
            otherName2 = "a";
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.AMOUNT_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }


        [TestMethod]
        public void TestOrg2Code_NotOtherOrgCode_NotRequiredCodeLength()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();
            
            org1 = new string('a', InternationalValidator.ORG_CODE_MAX_LENGTH);
            amount1 = "1";
            org2 = new string('a', InternationalValidator.ORG_CODE_MAX_LENGTH);
            amount2 = "1";
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            org2 = String.Empty;
            instance = createEntity();
            result = validator.Validate(instance);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.ORG_2_CODE_NOT_SPECIFIED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));

            org2 = new string('a', InternationalValidator.ORG_CODE_MAX_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.ORG_2_CODE_NOT_SPECIFIED_ERROR_MESSAGE, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestOrg2Code_IsOtherOrgCode_OtherNameExceedsLength()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();
            

            org1 = InternationalValidator.OTHER_ORG_CODE;
            amount1 = "1";
            otherName1 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH);

            org2 = InternationalValidator.OTHER_ORG_CODE;
            amount2 = "1";
            otherName2 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH);
            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            otherName2 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH + 1);
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.OTHER_ORG_2_NAME_REQUIRED, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestOrg2Code_IsOtherOrgCode_OtherNameNull()
        {
            string amount1 = null;
            string org1 = null;
            string otherName1 = null;
            string amount2 = null;
            string org2 = null;
            string otherName2 = null;
            Func<International> createEntity = () =>
            {
                return new International(
                    org1: org1,
                    otherName1: otherName1,
                    amount1: amount1,
                    org2: org2,
                    otherName2: otherName2,
                    amount2: amount2
                );
            };
            var validator = new InternationalValidator();
            

            org1 = InternationalValidator.OTHER_ORG_CODE;
            amount1 = "1";
            otherName1 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH);


            org2 = InternationalValidator.OTHER_ORG_CODE;
            amount2 = "1";
            otherName2 = new string('a', InternationalValidator.OTHER_ORG_NAME_MAX_LENGTH);

            var instance = createEntity();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);
            
            otherName2 = null;
            instance = createEntity();
            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count);
            Assert.AreEqual(InternationalValidator.OTHER_ORG_2_NAME_REQUIRED, result.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(result.Errors.First().CustomState, typeof(FundingErrorPath));
        }
    }
}
