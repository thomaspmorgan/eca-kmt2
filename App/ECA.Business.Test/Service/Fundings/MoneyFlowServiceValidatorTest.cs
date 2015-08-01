using ECA.Business.Service.Fundings;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace ECA.Business.Test.Service.Fundings
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
        public void TestDoCreate_ValueIsLessThanZero()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            value = -1;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.INVALID_AMOUNT_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Value", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoCreate_ValueIsZero()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId
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
        public void TestDoCreate_EmptyDescription()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId
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
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId
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

        [TestMethod]
        public void TestDoCreate_NullDescription()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId
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
        public void TestDoCreate_AccomodationSourceType()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            sourceEntityTypeId = MoneyFlowSourceRecipientType.Accomodation.Id;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(String.Format(MoneyFlowServiceValidator.INVALID_SOURCE_TYPE_MESSAGE_FORMAT, MoneyFlowSourceRecipientType.Accomodation.Value), validationErrors.First().ErrorMessage);
            Assert.AreEqual("SourceEntityTypeId", validationErrors.First().Property);
        }
        [TestMethod]
        public void TestDoCreate_ExpenseSourceType()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            sourceEntityTypeId = MoneyFlowSourceRecipientType.Expense.Id;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(String.Format(MoneyFlowServiceValidator.INVALID_SOURCE_TYPE_MESSAGE_FORMAT, MoneyFlowSourceRecipientType.Expense.Value), validationErrors.First().ErrorMessage);
            Assert.AreEqual("SourceEntityTypeId", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoCreate_HasSourceEntityType_NullRecipientId()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            sourceEntityId = null;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.NULL_SOURCE_ENTITY_ID_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("SourceEntityId", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoCreate_HasRecipientEntityType_NullRecipientId()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            recipientEntityId = null;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.NULL_RECIPIENT_ENTITY_ID_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("RecipientEntityId", validationErrors.First().Property);
        }
        #endregion

    }
}
