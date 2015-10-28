using ECA.Business.Service.Fundings;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
        public void TestDoValidateCreate_FiscalYearIsNotEqualToParentMoneyFlow()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            parentFiscalYear++;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.FISCAL_YEARS_MUST_BE_EQUAL_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("FiscalYear", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_FiscalYearIsZero()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = null;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            fiscalYear = 0;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.FISCAL_YEAR_LESS_THAN_ZERO_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("FiscalYear", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_FiscalYearLessThanZero()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = null;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            fiscalYear = -1;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.FISCAL_YEAR_LESS_THAN_ZERO_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("FiscalYear", validationErrors.First().Property);
        }


        [TestMethod]
        public void TestDoValidateCreate_ValueIsLessThanZero()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
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
        public void TestDoValidateCreate_EmptyDescription()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
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
        public void TestDoValidateCreate_WhitespaceDescription()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
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
        public void TestDoValidateCreate_NullDescription()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
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
        public void TestDoValidateCreate_DescriptionExceedsMaxLength()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            description = new String('s', MoneyFlow.DESCRIPTION_MAX_LENGTH + 1);
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(String.Format(MoneyFlowServiceValidator.DESCRIPTION_EXCEEDS_MAX_LENGTH_FORMAT, MoneyFlow.DESCRIPTION_MAX_LENGTH), validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_AccomodationSourceType()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
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
        public void TestDoValidateCreate_ExpenseSourceType()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
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
        public void TestDoValidateCreate_HasSourceEntityType_NullRecipientId()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
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
        public void TestDoValidateCreate_HasRecipientEntityType_NullRecipientId()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
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

        [TestMethod]
        public void TestDoValidateCreate_SourceAndRecipientAreEqual()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId, sourceEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            recipientEntityId = 1;
            recipientEntityTypeId = sourceEntityTypeId;

            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.SOURCE_AND_RECIPIENT_ENTITIES_EQUAL_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("SourceEntityId", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_CheckEqualSourceAndRecipient_SourceAndRecipientTypesAreNotEqual()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 1;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId, MoneyFlowSourceRecipientType.Project.Id };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            Assert.AreEqual(1, validator.DoValidateCreate(createEntity()).Count());
            var validationErrors = validator.DoValidateCreate(createEntity()).ToList();
            Assert.AreEqual(MoneyFlowServiceValidator.SOURCE_AND_RECIPIENT_ENTITIES_EQUAL_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("SourceEntityId", validationErrors.First().Property);

            recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            validationErrors = validator.DoValidateCreate(createEntity()).ToList();
            Assert.AreEqual(0, validationErrors.Count);            
        }

        [TestMethod]
        public void TestDoValidateCreate_CheckEqualSourceAndRecipient_SourceAndRecipientsAreNotEqual()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 1;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            Assert.AreEqual(1, validator.DoValidateCreate(createEntity()).Count());
            var validationErrors = validator.DoValidateCreate(createEntity()).ToList();
            Assert.AreEqual(MoneyFlowServiceValidator.SOURCE_AND_RECIPIENT_ENTITIES_EQUAL_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("SourceEntityId", validationErrors.First().Property);

            recipientEntityId = 2;
            validationErrors = validator.DoValidateCreate(createEntity()).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }


        [TestMethod]
        public void TestDoValidateCreate_RecipientEntityTypeIdIsNotAllowed()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            var validationErrors = validator.DoValidateCreate(createEntity()).ToList();
            Assert.AreEqual(0, validationErrors.Count);

            allowedRecipientEntityTypeIds = new List<int> { MoneyFlowSourceRecipientType.Accomodation.Id };
            validationErrors = validator.DoValidateCreate(createEntity()).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.RECIPIENT_ENTITY_TYPE_IS_NOT_VALID_FOR_SOURCE_ENTITY_TYPE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("RecipientEntityTypeId", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_NoRecipientEntityTypeIdsAllowed()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            var validationErrors = validator.DoValidateCreate(createEntity()).ToList();
            Assert.AreEqual(0, validationErrors.Count);

            allowedRecipientEntityTypeIds = new List<int>();
            validationErrors = validator.DoValidateCreate(createEntity()).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.RECIPIENT_ENTITY_TYPE_IS_NOT_VALID_FOR_SOURCE_ENTITY_TYPE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("RecipientEntityTypeId", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_ProjectSource_ParticipantRecipient_ParticipantNotAllowed()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Participant.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int> { recipientEntityId.Value };
            var parentMoneyFlowWithdrawalMaximum = 100m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            allowedProjectParticipantIds.Clear();
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.RECIPIENT_PARTICIPANT_IS_NOT_A_PARTICIPANT_OF_THE_PROJECT, validationErrors.First().ErrorMessage);
            Assert.AreEqual("RecipientEntityId", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_ParentMoneyFlowWithdrawlLimit_GreaterThanLimit()
        {
            var value = 5.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 10m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            parentMoneyFlowWithdrawalMaximum = 4.00m;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.VALUE_EXCEEDS_PARENT_MONEY_FLOW_WITHDRAWAL_LIMIT, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Value", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateCreate_ParentMoneyFlowWithdrawlLimit_EqualToLimit()
        {
            var value = 5.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            var parentMoneyFlowWithdrawalMaximum = 10m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            value = parentMoneyFlowWithdrawalMaximum;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }

        [TestMethod]
        public void TestDoValidateCreate_NullParentMoneyFlowWithdrawlLimit()
        {
            var value = 5.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            var allowedRecipientEntityTypeIds = new List<int> { recipientEntityTypeId };
            var allowedProjectParticipantIds = new List<int>();
            decimal? parentMoneyFlowWithdrawalMaximum = 10m;

            Func<MoneyFlowServiceCreateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceCreateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: parentMoneyFlowWithdrawalMaximum,
                description: description,
                transactionDate: transactionDate,
                hasRecipientEntityType: hasRecipientEntityType,
                hasSourceEntityType: hasSourceEntityType,
                sourceEntityId: sourceEntityId,
                recipientEntityId: recipientEntityId,
                sourceEntityTypeId: sourceEntityTypeId,
                recipientEntityTypeId: recipientEntityTypeId,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear,
                allowedRecipientEntityTypeIds: allowedRecipientEntityTypeIds,
                allowedProjectParticipantIds: allowedProjectParticipantIds
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            parentMoneyFlowWithdrawalMaximum = null;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }

        #endregion

        #region Update
        [TestMethod]
        public void TestDoValidateUpdate_ValueExceedsParentMoneyFlowWithdrawalLimit()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            decimal? withdrawalLimit = 100m;
            Func<MoneyFlowServiceUpdateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceUpdateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: withdrawalLimit,
                description: description,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());

            withdrawalLimit = 0m;
            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.VALUE_EXCEEDS_PARENT_MONEY_FLOW_WITHDRAWAL_LIMIT, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Value", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ValueIsEqualParentMoneyFlowWithdrawalLimit()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            decimal? withdrawalLimit = 100m;
            Func<MoneyFlowServiceUpdateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceUpdateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: withdrawalLimit,
                description: description,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());

            value = withdrawalLimit.Value;
            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ParentMoneyFlowWithdrawalLimitNotProvided()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            decimal? withdrawalLimit = 100m;
            Func<MoneyFlowServiceUpdateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceUpdateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: withdrawalLimit,
                description: description,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());

            withdrawalLimit = null;
            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }

        [TestMethod]
        public void TestDoValidateUpdate_FiscalYearLessThanZero()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            int fiscalYear = 2000;
            int? parentFiscalYear = null;
            decimal? withdrawalLimit = 100m;
            Func<MoneyFlowServiceUpdateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceUpdateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: withdrawalLimit,
                description: description,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());

            fiscalYear = -1;
            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.FISCAL_YEAR_LESS_THAN_ZERO_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("FiscalYear", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_FiscalYearIsZero()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            int fiscalYear = 2000;
            int? parentFiscalYear = null;
            decimal? withdrawalLimit = 100m;
            Func<MoneyFlowServiceUpdateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceUpdateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: withdrawalLimit,
                description: description,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());

            fiscalYear = 0;
            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.FISCAL_YEAR_LESS_THAN_ZERO_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("FiscalYear", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ValueIsLessThanZero()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            decimal? withdrawalLimit = 100m;
            Func<MoneyFlowServiceUpdateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceUpdateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: withdrawalLimit,
                description: description,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());

            value = -1;
            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.INVALID_AMOUNT_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Value", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_EmptyDescription()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            decimal? withdrawalLimit = 100m;
            Func<MoneyFlowServiceUpdateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceUpdateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: withdrawalLimit,
                description: description,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());

            description = String.Empty;
            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_NullDescription()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            decimal? withdrawalLimit = 100m;
            Func<MoneyFlowServiceUpdateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceUpdateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: withdrawalLimit,
                description: description,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());

            description = null;
            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_WhitespaceDescription()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            decimal? withdrawalLimit = 100m;
            Func<MoneyFlowServiceUpdateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceUpdateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: withdrawalLimit,
                description: description,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());

            description = " ";
            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_DescriptionExceedsLength()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            decimal? withdrawalLimit = 100m;
            Func<MoneyFlowServiceUpdateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceUpdateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: withdrawalLimit,
                description: description,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());

            description = new String('s', MoneyFlow.DESCRIPTION_MAX_LENGTH + 1);
            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(String.Format(MoneyFlowServiceValidator.DESCRIPTION_EXCEEDS_MAX_LENGTH_FORMAT, MoneyFlow.DESCRIPTION_MAX_LENGTH), validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_FiscalYearDoesNotEqualParentFiscalYear()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            int fiscalYear = 2000;
            int? parentFiscalYear = fiscalYear;
            decimal? withdrawalLimit = 100m;
            Func<MoneyFlowServiceUpdateValidationEntity> createEntity = () =>
            {
                return new MoneyFlowServiceUpdateValidationEntity(
                value: value,
                parentMoneyFlowWithdrawalMaximum: withdrawalLimit,
                description: description,
                fiscalYear: fiscalYear,
                parentFiscalYear: parentFiscalYear
                );
            };
            Assert.AreEqual(0, validator.ValidateUpdate(createEntity()).Count());

            parentFiscalYear++;
            var entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(MoneyFlowServiceValidator.FISCAL_YEARS_MUST_BE_EQUAL_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("FiscalYear", validationErrors.First().Property);
        }
        #endregion
    }
}
