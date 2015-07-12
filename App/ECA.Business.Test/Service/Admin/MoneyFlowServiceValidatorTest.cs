using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Projects;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class MoneyFlowServiceValidatorTest
    {
        private MoneyFlowServiceValidator validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new MoneyFlowServiceValidator();
        }
        #region Create

        [TestMethod]
        public void TestDoCreate_NullValue()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = new DateTimeOffset();
            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            value = 0;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.INVALID_AMOUNT_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Value", validationErrors.First().Property);

        }

        [TestMethod]
        public void TestDoCreate_NullDescription()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = new DateTimeOffset();
            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            description = null;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);

        }


        [TestMethod]
        public void TestDoCreate_EmptyDescription()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = new DateTimeOffset();
            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            description = String.Empty;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);

        }

        [TestMethod]
        public void TestDoCreate_WhitespaceDescription()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = new DateTimeOffset();
            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            description = " ";
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);

        }
        #endregion

    }
}
