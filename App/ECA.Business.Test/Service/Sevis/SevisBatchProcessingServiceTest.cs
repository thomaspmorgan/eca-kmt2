using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Sevis;
using System.Xml.Linq;
using ECA.Data;
using ECA.Business.Service.Persons;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Sevis
{
    [TestClass]
    public class SevisBatchProcessingServiceTest
    {
        private TestEcaContext context;
        private SevisBatchProcessingService service;
        private ParticipantPersonsSevisService sevisService;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new SevisBatchProcessingService(context, null);
            sevisService = new ParticipantPersonsSevisService(context, null);
        }

        [TestMethod]
        public void TestSevisBatchProcessing_Create()
        {
            var sevisBatchProcessing = service.Create();
            Assert.IsTrue(sevisBatchProcessing.BatchId == 0);
        }

        [TestMethod]
        public void TestSevisBatchProcessing_XML_Get()
        {
            var sevisBatchProcessing = service.Create();
            var xml = sevisBatchProcessing.SendXml;
        }
        
        [TestMethod]
        public void TestSevisBatchProcessing_XML_SetGet()
        {
            var sevisBatchProcessing = service.Create();
            string testXmlString = "<root><e1>test</e1><e2>test2</e2></root>";
            sevisBatchProcessing.SendXml = XElement.Parse(testXmlString);
            string outXmlString = sevisBatchProcessing.SendXml.ToString(SaveOptions.DisableFormatting);
            Assert.AreEqual(testXmlString, outXmlString);

            sevisBatchProcessing.TransactionLogXml = XElement.Parse(testXmlString);
            outXmlString = sevisBatchProcessing.SendXml.ToString(SaveOptions.DisableFormatting);
            Assert.AreEqual(testXmlString, outXmlString);
        }

        [TestMethod]
        public void TestSevisBatchProcessing_GetById()
        {
            var sbp1 = new ECA.Data.SevisBatchProcessing
            {
                BatchId = 1,
                SendXml = XElement.Parse("<root><e1>teste1</e1></root>")
            };

            context.SevisBatchProcessings.Add(sbp1);

            var sbpDTO = service.GetById(1);

            Assert.AreEqual(sbpDTO.BatchId, sbp1.BatchId);
            Assert.AreEqual(sbpDTO.SendXml.ToString(), sbp1.SendXml.ToString());
        }

        [TestMethod]
        public void TestSevisBatchProcessing_SaveBatchResult()
        {
            var user = new User(1);
            var sbp1 = new ECA.Data.SevisBatchProcessing
            {
                BatchId = 1,
                TransactionLogXml = XElement.Parse(@"<Root><Process><Record sevisID='N0012309439' requestID='1179' userID='50'><Result><ErrorCode>S1056</ErrorCode><ErrorMessage>Invalid student visa type for this action</ErrorMessage></Result></Record></Process></Root>")
            };

            context.SevisBatchProcessings.Add(sbp1);

            var updates = sevisService.UpdateParticipantPersonSevisBatchStatusAsync(user, 1);

            Assert.IsTrue(updates.Result == 1);
        }


    }
}
