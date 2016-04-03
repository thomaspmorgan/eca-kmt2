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

namespace ECA.Business.Test.Service.Sevis
{
    [TestClass]
    public class StagedSevisBatchTest
    {
        public ExchangeVisitor GetExchangeVisitor(string sevisUserId, string sevisId, int personId, int participantId)
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "123 Us address";
            mailAddress.Street2 = null;
            mailAddress.City = "city";
            mailAddress.PostalCode = "55555";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
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
                Division = "DC",
                LocationName = "US Dept of State"
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisUserId: sevisUserId,
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
            var sevisUserId = "sevisUserId";
            var maxCreated = 10;
            var maxUpdated = 20;
            var instance = new StagedSevisBatch(batchId, sevisUserId, orgId, maxCreated, maxUpdated);
            Assert.IsNotNull(instance.BatchId);
            Assert.AreEqual(instance.GetBatchId(batchId), instance.BatchId);

            Assert.IsNotNull(instance.SevisBatchProcessing);
            Assert.AreEqual(instance.GetBatchId(batchId), instance.SevisBatchProcessing.BatchId);

            Assert.IsNotNull(instance.SEVISBatchCreateUpdateEV);
            Assert.AreEqual(sevisUserId, instance.SEVISBatchCreateUpdateEV.userID);
            Assert.IsNotNull(instance.SEVISBatchCreateUpdateEV.BatchHeader);
            Assert.AreEqual(instance.GetBatchId(batchId), instance.SEVISBatchCreateUpdateEV.BatchHeader.BatchID);
            Assert.AreEqual(orgId, instance.SEVISBatchCreateUpdateEV.BatchHeader.OrgID);

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
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var sevisUserId = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUserId, orgId);
            Assert.AreEqual(StagedSevisBatch.MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH_DEFAULT, instance.MaxCreateExchangeVisitorRecordsPerBatch);
            Assert.AreEqual(StagedSevisBatch.MAX_UPDATE_EXCHANGE_VISITOR_RECORD_PER_BATCH_DEFAULT, instance.MaxUpdateExchangeVisitorRecordPerBatch);
        }

        [TestMethod]
        public void TestCanAccomodate_SevisIdIsNull()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, user, orgId);
            var exchangeVisitor = GetExchangeVisitor(user, null, 1, 2);
            Assert.IsTrue(instance.CanAccomodate(exchangeVisitor));
        }

        [TestMethod]
        public void TestCanAccomodate_CreateEVArrayIsNotNull()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, user, orgId);
            instance.SEVISBatchCreateUpdateEV.CreateEV = new List<SEVISEVBatchTypeExchangeVisitor>().ToArray();
            var exchangeVisitor = GetExchangeVisitor(user, null, 1, 2);
            Assert.IsTrue(instance.CanAccomodate(exchangeVisitor));
        }

        [TestMethod]
        public void TestCanAccomodate_SevisIdIsWhitespace()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var user = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, user, orgId);
            var exchangeVisitor = GetExchangeVisitor(user, " ", 1, 2);
            Assert.IsTrue(instance.CanAccomodate(exchangeVisitor));
        }

        [TestMethod]
        public void TestCanAccomodate_SevisIdIsEmpty()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var sevisUserId = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUserId, orgId);
            var exchangeVisitor = GetExchangeVisitor(sevisUserId, string.Empty, 1, 2);
            Assert.IsTrue(instance.CanAccomodate(exchangeVisitor));
        }

        [TestMethod]
        public void TestCanAccomodate_SevisIdHasValue()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var sevisUserId = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUserId, orgId);
            var exchangeVisitor = GetExchangeVisitor(sevisUserId, "sevisId", 1, 2);
            Assert.IsTrue(instance.CanAccomodate(exchangeVisitor));
        }

        [TestMethod]
        public void TestCanAccomodate_UpdateEVArrayIsNotNull()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var sevisUserId = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUserId, orgId);
            instance.SEVISBatchCreateUpdateEV.UpdateEV = new List<SEVISEVBatchTypeExchangeVisitor1>().ToArray();
            var exchangeVisitor = GetExchangeVisitor(sevisUserId, "sevisId", 1, 2);
            Assert.IsTrue(instance.CanAccomodate(exchangeVisitor));
        }

        [TestMethod]
        public void TestAddExchangeVisitor_SevisIdDoesNotHaveValue_ExceededDefaultCount()
        {
            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var sevisUserId = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUserId, orgId);
            var exchangeVisitor = GetExchangeVisitor(sevisUserId, null, 1, 2);

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
            var sevisUserId = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUserId, orgId);
            var exchangeVisitor = GetExchangeVisitor(sevisUserId, "sevisId", 1, 2);

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
            var sevisUserId = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUserId, orgId, 1, 1);
            var exchangeVisitor = GetExchangeVisitor(sevisUserId, null, 1, 2);

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
            var sevisUserId = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUserId, orgId, 1, 1);
            var exchangeVisitor = GetExchangeVisitor(sevisUserId, "sevisId", 1, 2);

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
            var sevisUserId = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUserId, orgId);
            var exchangeVisitor = GetExchangeVisitor(sevisUserId, "sevisId", 1, 2);

            instance.AddExchangeVisitor(exchangeVisitor);
            Assert.AreEqual(1, instance.GetExchangeVisitors().Count());

            instance.AddExchangeVisitor(exchangeVisitor);
            Assert.AreEqual(1, instance.GetExchangeVisitors().Count());
        }

        [TestMethod]
        public void TestSerializeSEVISBatchCreateUpdateEV_DoesNotHaveSevisId()
        {
            var orgId = "P-1-19833";
            var batchId = Guid.NewGuid();
            var sevisUserId = "esayya9302";
            var instance = new StagedSevisBatch(batchId, sevisUserId, orgId);
            var exchangeVisitor = GetExchangeVisitor(sevisUserId, null, 1, 2);

            Assert.IsNull(instance.SevisBatchProcessing.SendXml);
            instance.AddExchangeVisitor(exchangeVisitor);

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

            var orgId = "org id";
            var batchId = Guid.NewGuid();
            var sevisUserId = "sevisUserId";
            var instance = new StagedSevisBatch(batchId, sevisUserId, orgId);
            var exchangeVisitor = GetExchangeVisitor(sevisUserId, "sevisId", 1, 2);
            
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
