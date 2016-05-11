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
using ECA.Business.Sevis.Model;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using ECA.Business.Queries.Models.Sevis;

namespace ECA.Business.Test.Service.Sevis
{
    [TestClass]
    public class StagedSevisBatchTest
    {
        public ExchangeVisitor GetExchangeVisitor(string sevisId, int personId, int participantId)
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.DivisionIso = state;
            mailAddress.Street1 = "123 Us address";
            mailAddress.Street2 = null;
            mailAddress.City = "city";
            mailAddress.PostalCode = "55555";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.DivisionIso = state;
            usAddress.Street1 = "123 Us address";
            usAddress.Street2 = null;
            usAddress.City = "city";
            usAddress.PostalCode = "55555";

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = " birth city";
            var birthCountryCode = "US";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "8505551212";
            short positionCode = 120;
            var printForm = true;
            var remarks = "remarks";
            var programCataegoryCode = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            var person = new Business.Validation.Sevis.Bio.Person(
                fullName,
                birthCity,
                birthCountryCode,
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
                Street1 = "street 1",
                PostalCode = "12345",
                DivisionIso = "DC",
                LocationName = "US Dept of State"
            };
            var exchangeVisitor = new ExchangeVisitor(
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

        public ExchangeVisitor GetPreviouslySubmittedExchangeVisitor(string sevisId, int personId, int participantId)
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.DivisionIso = state;
            mailAddress.Street1 = "123 Us address";
            mailAddress.Street2 = null;
            mailAddress.City = "city";
            mailAddress.PostalCode = "55555";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.DivisionIso = state;
            usAddress.Street1 = "123 Us address";
            usAddress.Street2 = null;
            usAddress.City = "city";
            usAddress.PostalCode = "55555";

            var firstName = "updated first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = " different birth city";
            var birthCountryCode = "US";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "8505551212";
            short positionCode = 120;
            var printForm = true;
            var remarks = "remarks";
            var programCataegoryCode = "1D";

            var subjectFieldCode = "01.0103";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");

            var person = new Business.Validation.Sevis.Bio.Person(
                fullName,
                birthCity,
                birthCountryCode,
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
                Street1 = "street 1",
                PostalCode = "12345",
                DivisionIso = "DC",
                LocationName = "US Dept of State"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                person: person,
                financialInfo: new Business.Validation.Sevis.Finance.FinancialInfo(true, true, null, null),
                occupationCategoryCode: "99",
                programEndDate: DateTime.Now.AddDays(-10.0),
                programStartDate: DateTime.Now.AddDays(-10.0),
                dependents: new List<Business.Validation.Sevis.Bio.Dependent>(),
                siteOfActivity: siteOfActivity);
            return exchangeVisitor;
        }


        [TestMethod]
        public void TestConstructor()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var maxCreated = 10;
            var maxUpdated = 20;
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId, maxCreated, maxUpdated);
            Assert.IsNotNull(instance.BatchId);
            Assert.AreEqual(batchId, instance.BatchId);
            Assert.AreEqual(sevisOrgId, instance.SevisOrgId);
            Assert.AreEqual(sevisUsername, instance.SevisUsername);

            Assert.IsNotNull(instance.SevisBatchProcessing);
            Assert.AreEqual(batchId.ToString(), instance.SevisBatchProcessing.BatchId);
            Assert.AreEqual(sevisOrgId, instance.SevisBatchProcessing.SevisOrgId);
            Assert.AreEqual(sevisUsername, instance.SevisBatchProcessing.SevisUsername);
            Assert.AreEqual(0, instance.SevisBatchProcessing.UploadTries);
            Assert.AreEqual(0, instance.SevisBatchProcessing.DownloadTries);

            Assert.IsNotNull(instance.SEVISBatchCreateUpdateEV);
            Assert.AreEqual(sevisUsername, instance.SEVISBatchCreateUpdateEV.userID);
            Assert.IsNotNull(instance.SEVISBatchCreateUpdateEV.BatchHeader);
            Assert.AreEqual(batchId.ToString(), instance.SEVISBatchCreateUpdateEV.BatchHeader.BatchID);
            Assert.AreEqual(sevisOrgId, instance.SEVISBatchCreateUpdateEV.BatchHeader.OrgID);

            Assert.IsNull(instance.SEVISBatchCreateUpdateEV.CreateEV);
            Assert.IsNull(instance.SEVISBatchCreateUpdateEV.UpdateEV);

            Assert.IsNotNull(instance.GetExchangeVisitors());
            Assert.AreEqual(0, instance.GetExchangeVisitors().Count());

            Assert.AreEqual(maxCreated, instance.MaxCreateExchangeVisitorRecordsPerBatch);
            Assert.AreEqual(maxUpdated, instance.MaxUpdateExchangeVisitorRecordPerBatch);
        }

        [TestMethod]
        public void TestConstructor_UseDefaults()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            Assert.AreEqual(StagedSevisBatch.MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH_DEFAULT, instance.MaxCreateExchangeVisitorRecordsPerBatch);
            Assert.AreEqual(StagedSevisBatch.MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT, instance.MaxUpdateExchangeVisitorRecordPerBatch);
        }

        [TestMethod]
        public void TestCanAccomodate_SevisOrgIdIsNotEqual()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(null, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            Assert.IsFalse(instance.CanAccomodate(participant, exchangeVisitor, sevisUsername, "other org"));
        }

        [TestMethod]
        public void TestCanAccomodate_SevisUsernameIsNotEqual()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(null, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            Assert.IsFalse(instance.CanAccomodate(participant, exchangeVisitor, "other user", sevisOrgId));
        }

        [TestMethod]
        public void TestCanAccomodate_SevisIdIsNull()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(null, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            Assert.IsTrue(instance.CanAccomodate(participant, exchangeVisitor, sevisUsername, sevisOrgId));
        }

        [TestMethod]
        public void TestCanAccomodate_CreateEVArrayIsNotNull()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            instance.SEVISBatchCreateUpdateEV.CreateEV = new List<SEVISEVBatchTypeExchangeVisitor>().ToArray();
            var exchangeVisitor = GetExchangeVisitor(null, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            Assert.IsTrue(instance.CanAccomodate(participant, exchangeVisitor, sevisUsername, sevisOrgId));
        }

        [TestMethod]
        public void TestCanAccomodate_SevisIdIsWhitespace()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(" ", 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            Assert.IsTrue(instance.CanAccomodate(participant, exchangeVisitor, sevisUsername, sevisOrgId));
        }

        [TestMethod]
        public void TestCanAccomodate_SevisIdIsEmpty()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(string.Empty, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            Assert.IsTrue(instance.CanAccomodate(participant, exchangeVisitor, sevisUsername, sevisOrgId));
        }

        [TestMethod]
        public void TestCanAccomodate_SevisIdHasValue()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var sevisId = "sevisId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(sevisId, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };

            var previouslySubmittedExchangeVisitor = GetPreviouslySubmittedExchangeVisitor(sevisId, 1, 2);
            Assert.IsTrue(exchangeVisitor.GetChangeDetail(previouslySubmittedExchangeVisitor).HasChanges());
            Assert.IsTrue(instance.CanAccomodate(participant, exchangeVisitor, sevisUsername, sevisOrgId, previouslySubmittedExchangeVisitor));
        }

        [TestMethod]
        public void TestCanAccomodate_UpdateEVArrayIsNotNull()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var sevisId = "sevisId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            instance.SEVISBatchCreateUpdateEV.UpdateEV = new List<SEVISEVBatchTypeExchangeVisitor1>().ToArray();
            var exchangeVisitor = GetExchangeVisitor(sevisId, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            var previouslySubmittedExchangeVisitor = GetPreviouslySubmittedExchangeVisitor(sevisId, 1, 2);
            Assert.IsTrue(exchangeVisitor.GetChangeDetail(previouslySubmittedExchangeVisitor).HasChanges());
            Assert.IsTrue(instance.CanAccomodate(participant, exchangeVisitor, sevisUsername, sevisOrgId, previouslySubmittedExchangeVisitor));
        }

        [TestMethod]
        public void TestCanAccomodate_ExchangeVisitorIsQueuedToValidate()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            instance.SEVISBatchCreateUpdateEV.UpdateEV = new List<SEVISEVBatchTypeExchangeVisitor1>().ToArray();
            var exchangeVisitor = GetExchangeVisitor("sevisId", 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToValidate.Id,
            };
            Assert.IsTrue(instance.CanAccomodate(participant, exchangeVisitor, sevisUsername, sevisOrgId));
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdIsNull()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(null, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            Assert.IsNull(instance.SEVISBatchCreateUpdateEV.CreateEV);

            instance.AddExchangeVisitor(participant, exchangeVisitor);
            Assert.AreEqual(1, instance.SEVISBatchCreateUpdateEV.CreateEV.Count());
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdIsWhitespace()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(" ", 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            Assert.IsNull(instance.SEVISBatchCreateUpdateEV.CreateEV);

            instance.AddExchangeVisitor(participant, exchangeVisitor);
            Assert.AreEqual(1, instance.SEVISBatchCreateUpdateEV.CreateEV.Count());
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdIsEmpty()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(String.Empty, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            Assert.IsNull(instance.SEVISBatchCreateUpdateEV.CreateEV);

            instance.AddExchangeVisitor(participant, exchangeVisitor);
            Assert.AreEqual(1, instance.SEVISBatchCreateUpdateEV.CreateEV.Count());
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdHasValue()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var sevisId = "sevisId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(sevisId, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            Assert.IsNull(instance.SEVISBatchCreateUpdateEV.CreateEV);

            var previouslySubmittedExchangeVisitor = GetPreviouslySubmittedExchangeVisitor(sevisId, 1, 2);
            Assert.IsTrue(exchangeVisitor.GetChangeDetail(previouslySubmittedExchangeVisitor).HasChanges());

            instance.AddExchangeVisitor(participant, exchangeVisitor, previouslySubmittedExchangeVisitor);
            Assert.AreEqual(exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUsername, previouslySubmittedExchangeVisitor).Count(),
                instance.SEVISBatchCreateUpdateEV.UpdateEV.Count());
        }

        [TestMethod]
        public void TestAddExchangeVisitor_IsQueuedToValidate()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor("sevisId", 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToValidate.Id,
            };
            Assert.IsNull(instance.SEVISBatchCreateUpdateEV.CreateEV);

            instance.AddExchangeVisitor(participant, exchangeVisitor);
            Assert.AreEqual(1, instance.SEVISBatchCreateUpdateEV.UpdateEV.Count());
        }

        [TestMethod]
        public void TestAddExchangeVisitor_IsQueuedToValidate_ExceededDefaultCount()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor("sevisId", 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToValidate.Id,
            };
            instance.SEVISBatchCreateUpdateEV.UpdateEV = new Business.Sevis.Model.SEVISEVBatchTypeExchangeVisitor1[StagedSevisBatch.MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT];

            var message = "This StagedSevisBatch can not accomodate the given exchange visitor.";
            Action a = () => instance.AddExchangeVisitor(participant, exchangeVisitor);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdDoesNotHaveValue_ExceededDefaultCount()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(null, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            instance.SEVISBatchCreateUpdateEV.CreateEV = new Business.Sevis.Model.SEVISEVBatchTypeExchangeVisitor[StagedSevisBatch.MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH_DEFAULT];

            var message = "This StagedSevisBatch can not accomodate the given exchange visitor.";
            Action a = () => instance.AddExchangeVisitor(participant, exchangeVisitor);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdHasValue_ExceededDefaultCount()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var sevisId = "sevisId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(sevisId, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            instance.SEVISBatchCreateUpdateEV.UpdateEV = new Business.Sevis.Model.SEVISEVBatchTypeExchangeVisitor1[StagedSevisBatch.MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT];

            var previouslySubmittedExchangeVisitor = GetPreviouslySubmittedExchangeVisitor(sevisId, 1, 2);
            Assert.IsTrue(exchangeVisitor.GetChangeDetail(previouslySubmittedExchangeVisitor).HasChanges());

            var message = "This StagedSevisBatch can not accomodate the given exchange visitor.";
            Action a = () => instance.AddExchangeVisitor(participant, exchangeVisitor, previouslySubmittedExchangeVisitor);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdDoesNotHaveValue_ExceededGivenCount()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId, 1, 1);
            var exchangeVisitor = GetExchangeVisitor(null, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            instance.SEVISBatchCreateUpdateEV.CreateEV = new Business.Sevis.Model.SEVISEVBatchTypeExchangeVisitor[2];

            var message = "This StagedSevisBatch can not accomodate the given exchange visitor.";
            Action a = () => instance.AddExchangeVisitor(participant, exchangeVisitor);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdHasValue_ExceededGivenCount()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisId = "sevisId";
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId, 1, 1);
            var exchangeVisitor = GetExchangeVisitor(sevisId, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };
            instance.SEVISBatchCreateUpdateEV.UpdateEV = new Business.Sevis.Model.SEVISEVBatchTypeExchangeVisitor1[2];

            var previouslySubmittedExchangeVisitor = GetPreviouslySubmittedExchangeVisitor(sevisId, 1, 2);
            Assert.IsTrue(exchangeVisitor.GetChangeDetail(previouslySubmittedExchangeVisitor).HasChanges());

            var message = "This StagedSevisBatch can not accomodate the given exchange visitor.";
            Action a = () => instance.AddExchangeVisitor(participant, exchangeVisitor, previouslySubmittedExchangeVisitor);
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
        }

        [TestMethod]
        public void TestAddExchangeVisitor_ExchangeVisitorAlreadyAdded()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var sevisId = "sevisId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(sevisId, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };

            var previouslySubmittedExchangeVisitor = GetPreviouslySubmittedExchangeVisitor(sevisId, 1, 2);
            Assert.IsTrue(exchangeVisitor.GetChangeDetail(previouslySubmittedExchangeVisitor).HasChanges());

            instance.AddExchangeVisitor(participant, exchangeVisitor, previouslySubmittedExchangeVisitor);
            Assert.AreEqual(1, instance.GetExchangeVisitors().Count());

            instance.AddExchangeVisitor(participant, exchangeVisitor, previouslySubmittedExchangeVisitor);
            Assert.AreEqual(1, instance.GetExchangeVisitors().Count());
        }

        [TestMethod]
        public void TestSerializeSEVISBatchCreateUpdateEV_DoesNotHaveSevisId()
        {
            var sevisOrgId = "P-1-19833";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "esayya9302";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor(null, 1, 2);
            var participant = new SevisGroupedParticipantDTO
            {
                ParticipantId = exchangeVisitor.Person.ParticipantId,
                SevisCommStatusId = SevisCommStatus.QueuedToSubmit.Id,
            };

            Assert.IsNull(instance.SevisBatchProcessing.SendXml);
            instance.AddExchangeVisitor(participant, exchangeVisitor);

            Assert.IsNull(instance.SevisBatchProcessing.SendXml);
            instance.SerializeSEVISBatchCreateUpdateEV();
            Assert.IsNotNull(instance.SevisBatchProcessing.SendXml);

            Assert.IsTrue(instance.SevisBatchProcessing.SendString.Contains(StagedSevisBatch.COMMON_NAMESPACE_PREFIX));
            Assert.IsTrue(instance.SevisBatchProcessing.SendString.Contains(StagedSevisBatch.TABLE_NAMESPACE_PREFIX));
            Assert.IsTrue(instance.SevisBatchProcessing.SendString.Contains(StagedSevisBatch.EXCHANGE_VISITOR_NAMESPACE_PREFIX));
            Assert.IsTrue(instance.SevisBatchProcessing.SendString.Contains(StagedSevisBatch.XSD_NAMESPACE_PREFIX));
            Assert.IsTrue(instance.SevisBatchProcessing.SendString.Contains(StagedSevisBatch.XSI_NAMESPACE_PREFIX));

            Assert.IsTrue(instance.SevisBatchProcessing.SendString.Contains(StagedSevisBatch.COMMON_NAMESPACE_URL));
            Assert.IsTrue(instance.SevisBatchProcessing.SendString.Contains(StagedSevisBatch.TABLE_NAMESPACE_URL));
            Assert.IsTrue(instance.SevisBatchProcessing.SendString.Contains(StagedSevisBatch.EXCHANGE_VISITOR_NAMESPACE_URL));
            Assert.IsTrue(instance.SevisBatchProcessing.SendString.Contains(StagedSevisBatch.XSD_NAMESPACE_URL));
            Assert.IsTrue(instance.SevisBatchProcessing.SendString.Contains(StagedSevisBatch.XSI_NAMESPACE_URL));

            //XmlReaderSettings settings = new XmlReaderSettings();

            //settings.Schemas.Add("http://www.ice.gov/xmlschema/sevisbatch/alpha/Common", StagedSevisBatch.COMMON_NAMESPACE_URL);
            //settings.Schemas.Add("http://www.ice.gov/xmlschema/sevisbatch/alpha/Table", StagedSevisBatch.TABLE_NAMESPACE_URL);
            //settings.Schemas.Add("", StagedSevisBatch.EXCHANGE_VISITOR_NAMESPACE_URL);

            //settings.ValidationType = ValidationType.Schema;

            //using (XmlReader reader = XmlReader.Create(new StringReader(instance.SevisBatchProcessing.SendString), settings))
            //{
            //    XmlDocument document = new XmlDocument();
            //    Action a = () => document.Load(reader);
            //    a.ShouldNotThrow();                
            //}
        }



        [TestMethod]
        public void TestGetExchangeVisitorNamespaces()
        {
            var sevisOrgId = "org id";
            var batchId = BatchId.NewBatchId();
            var sevisUsername = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUsername, sevisOrgId);
            var exchangeVisitor = GetExchangeVisitor("sevisId", 1, 2);

            var namespaces = instance.GetExchangeVisitorNamespaces();
            var namespacesArray = namespaces.ToArray();
            Assert.AreEqual(5, namespacesArray.Count());

            var urls = namespacesArray.Select(x => x.Namespace).ToList();
            Assert.IsTrue(urls.Contains(StagedSevisBatch.COMMON_NAMESPACE_URL));
            Assert.IsTrue(urls.Contains(StagedSevisBatch.TABLE_NAMESPACE_URL));
            Assert.IsTrue(urls.Contains(StagedSevisBatch.EXCHANGE_VISITOR_NAMESPACE_URL));
            Assert.IsTrue(urls.Contains(StagedSevisBatch.XSD_NAMESPACE_URL));
            Assert.IsTrue(urls.Contains(StagedSevisBatch.XSI_NAMESPACE_URL));

            var prefixes = namespacesArray.Select(x => x.Name).ToList();
            Assert.IsTrue(prefixes.Contains(StagedSevisBatch.COMMON_NAMESPACE_PREFIX));
            Assert.IsTrue(prefixes.Contains(StagedSevisBatch.TABLE_NAMESPACE_PREFIX));
            Assert.IsTrue(prefixes.Contains(StagedSevisBatch.EXCHANGE_VISITOR_NAMESPACE_PREFIX));
            Assert.IsTrue(prefixes.Contains(StagedSevisBatch.XSI_NAMESPACE_PREFIX));
            Assert.IsTrue(prefixes.Contains(StagedSevisBatch.XSD_NAMESPACE_PREFIX));
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

        //static void ValidationEventHandler(object sender, ValidationEventArgs e)
        //{
        //    switch (e.Severity)
        //    {
        //        case XmlSeverityType.Error:
        //            Console.WriteLine("Error: {0}", e.Message);
        //            break;
        //        case XmlSeverityType.Warning:
        //            Console.WriteLine("Warning {0}", e.Message);
        //            break;
        //    }

        //}
    }
}
