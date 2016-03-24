using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Sevis;
using ECA.Business.Service;
using ECA.Business.Validation.Sevis;
using System.Collections.Generic;
using ECA.Data;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Service.Admin;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Test.Service.Sevis
{
    [TestClass]
    public class StagedSevisBatchTest
    {
        public ExchangeVisitor GetExchangeVisitor(User user, string sevisId, int personId, int participantId)
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "123-456-7890";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "reason";
            var remarks = "remarks";
            var programCataegoryCode = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);

            var person = new Business.Validation.Sevis.Bio.Person(
                fullName,
                birthCity,
                birthCountryCode,
                birthCountryReason,
                birthDate,
                citizenshipCountryCode,
                email,
                gender,
                permanentResidenceCountryCode,
                phone,
                remarks,
                positionCode.ToString(),
                programCataegoryCode,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            var siteOfActivity = new AddressDTO
            {
                Division = "DC",
                LocationName = "name"
            };
            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now,
                programStartDate: DateTime.Now,
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            return exchangeVisitor;
        }


        [TestMethod]
        public void TestConstructor()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = new User(1);
            var maxCreated = 10;
            var maxUpdated = 20;
            var instance = new StagedSevisBatch(batchId, user, orgId, maxCreated, maxUpdated);
            Assert.IsNotNull(instance.BatchId);
            Assert.AreEqual(instance.GetBatchId(batchId), instance.BatchId);

            Assert.IsNotNull(instance.SevisBatchProcessing);
            Assert.AreEqual(instance.GetBatchId(batchId), instance.SevisBatchProcessing.BatchId);

            Assert.IsNotNull(instance.SEVISBatchCreateUpdateEV);
            Assert.AreEqual(user.Id.ToString(), instance.SEVISBatchCreateUpdateEV.userID);
            Assert.IsNotNull(instance.SEVISBatchCreateUpdateEV.BatchHeader);
            Assert.AreEqual(instance.GetBatchId(batchId), instance.SEVISBatchCreateUpdateEV.BatchHeader.BatchID);
            Assert.AreEqual(orgId, instance.SEVISBatchCreateUpdateEV.BatchHeader.OrgID);

            Assert.IsNotNull(instance.SEVISBatchCreateUpdateEV.CreateEV);
            Assert.IsNotNull(instance.SEVISBatchCreateUpdateEV.UpdateEV);
            Assert.AreEqual(0, instance.SEVISBatchCreateUpdateEV.CreateEV.Count());
            Assert.AreEqual(0, instance.SEVISBatchCreateUpdateEV.UpdateEV.Count());

            Assert.IsNotNull(instance.GetExchangeVisitors());
            Assert.AreEqual(0, instance.GetExchangeVisitors().Count());

            Assert.AreEqual(maxCreated, instance.MaxCreateExchangeVisitorRecordsPerBatch);
            Assert.AreEqual(maxUpdated, instance.MaxUpdateExchangeVisitorRecordPerBatch);
        }

        [TestMethod]
        public void TestConstructor_UseDefaults()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = new User(1);
            var instance = new StagedSevisBatch(batchId, user, orgId);
            Assert.AreEqual(StagedSevisBatch.MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH_DEFAULT, instance.MaxCreateExchangeVisitorRecordsPerBatch);
            Assert.AreEqual(StagedSevisBatch.MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT, instance.MaxUpdateExchangeVisitorRecordPerBatch);
        }

        [TestMethod]
        public void TestCanAccomodate_SevisIdIsNull()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = new User(1);
            var instance = new StagedSevisBatch(batchId, user, orgId);
            var exchangeVisitor = GetExchangeVisitor(user, null, 1, 2);
            Assert.IsTrue(instance.CanAccomodate(exchangeVisitor));
        }

        [TestMethod]
        public void TestCanAccomodate_SevisIdIsWhitespace()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = new User(1);
            var instance = new StagedSevisBatch(batchId, user, orgId);
            var exchangeVisitor = GetExchangeVisitor(user, " ", 1, 2);
            Assert.IsTrue(instance.CanAccomodate(exchangeVisitor));
        }

        [TestMethod]
        public void TestCanAccomodate_SevisIdIsEmpty()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = new User(1);
            var instance = new StagedSevisBatch(batchId, user, orgId);
            var exchangeVisitor = GetExchangeVisitor(user, string.Empty, 1, 2);
            Assert.IsTrue(instance.CanAccomodate(exchangeVisitor));
        }

        [TestMethod]
        public void TestCanAccomodate_SevisIdHasValue()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = new User(1);
            var instance = new StagedSevisBatch(batchId, user, orgId);
            var exchangeVisitor = GetExchangeVisitor(user, "sevisId", 1, 2);
            Assert.IsTrue(instance.CanAccomodate(exchangeVisitor));
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdDoesNotHaveValue_ExceededDefaultCount()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = new User(1);
            var instance = new StagedSevisBatch(batchId, user, orgId);
            var exchangeVisitor = GetExchangeVisitor(user, null, 1, 2);

            instance.SEVISBatchCreateUpdateEV.CreateEV = new Business.Sevis.Model.SEVISEVBatchTypeExchangeVisitor[StagedSevisBatch.MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH_DEFAULT];

            var message = "This StagedSevisBatch can not accomodate the given exchange visitor.";
            Action a = () => instance.AddExchangeVisitor(exchangeVisitor);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdHasValue_ExceededDefaultCount()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = new User(1);
            var instance = new StagedSevisBatch(batchId, user, orgId);
            var exchangeVisitor = GetExchangeVisitor(user, "sevisId", 1, 2);

            instance.SEVISBatchCreateUpdateEV.UpdateEV = new Business.Sevis.Model.SEVISEVBatchTypeExchangeVisitor1[StagedSevisBatch.MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT];

            var message = "This StagedSevisBatch can not accomodate the given exchange visitor.";
            Action a = () => instance.AddExchangeVisitor(exchangeVisitor);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdDoesNotHaveValue_ExceededGivenCount()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = new User(1);
            var instance = new StagedSevisBatch(batchId, user, orgId, 1, 1);
            var exchangeVisitor = GetExchangeVisitor(user, null, 1, 2);

            instance.SEVISBatchCreateUpdateEV.CreateEV = new Business.Sevis.Model.SEVISEVBatchTypeExchangeVisitor[2];

            var message = "This StagedSevisBatch can not accomodate the given exchange visitor.";
            Action a = () => instance.AddExchangeVisitor(exchangeVisitor);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdHasValue_ExceededGivenCount()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = new User(1);
            var instance = new StagedSevisBatch(batchId, user, orgId, 1, 1);
            var exchangeVisitor = GetExchangeVisitor(user, "sevisId", 1, 2);

            instance.SEVISBatchCreateUpdateEV.UpdateEV = new Business.Sevis.Model.SEVISEVBatchTypeExchangeVisitor1[2];

            var message = "This StagedSevisBatch can not accomodate the given exchange visitor.";
            Action a = () => instance.AddExchangeVisitor(exchangeVisitor);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestAddExchangeVisitor_ExchangeVisitorAlreadyAdded()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = new User(1);
            var instance = new StagedSevisBatch(batchId, user, orgId);
            var exchangeVisitor = GetExchangeVisitor(user, "sevisId", 1, 2);

            instance.AddExchangeVisitor(exchangeVisitor);
            Assert.AreEqual(1, instance.GetExchangeVisitors().Count());

            instance.AddExchangeVisitor(exchangeVisitor);
            Assert.AreEqual(1, instance.GetExchangeVisitors().Count());
        }

        [TestMethod]
        public void TestSerializeSEVISBatchCreateUpdateEV()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = new User(1);
            var instance = new StagedSevisBatch(batchId, user, orgId);
            var exchangeVisitor = GetExchangeVisitor(user, "sevisId", 1, 2);

            Assert.IsNull(instance.SevisBatchProcessing.SendXml);
            instance.AddExchangeVisitor(exchangeVisitor);

            Assert.IsNull(instance.SevisBatchProcessing.SendXml);
            instance.SerializeSEVISBatchCreateUpdateEV();
            Assert.IsNotNull(instance.SevisBatchProcessing.SendXml);
        }

        [TestMethod]
        public void TestGetBatchId()
        {
            var guid = Guid.NewGuid();
            var expectedBatchId = guid.ToString();
            expectedBatchId = expectedBatchId.Replace("-", String.Empty);
            var maxLength = 14;
            var index = expectedBatchId.Length - maxLength;

            expectedBatchId = expectedBatchId.Substring(index);
            Assert.IsTrue(guid.ToString().Replace("-", String.Empty).EndsWith(expectedBatchId));
            Assert.AreEqual(maxLength, expectedBatchId.Length);
            Assert.IsFalse(expectedBatchId.Contains("-"));
            
        }
    }
}
