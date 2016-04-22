using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using RichardSzalay.MockHttp;
using System.Collections.Specialized;
using ECA.Core.Settings;
using System.Configuration;
using System.Xml.Linq;
using System.IO.Compression;
using System.IO;

namespace ECA.Net.Tests
{
    [TestClass()]
    public class SevisCommTests
    {
        private IEcaHttpMessageHandlerService mockedEcaHttpMesssageHandlerService;
        private NameValueCollection settings;
        private AppSettings appSettings;
        public string sevisUser = "testus1234";
        public string orgId = "P-1-12345";
        public string batchId = "2739d72c8a293b";
        private string uploadXmlString = "<SEVISBatchCreateUpdateEV xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:noNamespaceSchemaLocation=\"http://www.ice.gov/xmlschema/sevisbatch/alpha/Create-UpdateExchangeVisitor.xsd\" xmlns:table=\"http://www.ice.gov/xmlschema/sevisbatch/alpha/SEVISTable.xsd\" xmlns:common=\"http://www.ice.gov/xmlschema/sevisbatch/alpha/Common.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" userID=\"esayya9302\"><BatchHeader><BatchID>2739d72c8a293a</BatchID><OrgID>P-1-19833</OrgID></BatchHeader><CreateEV><ExchangeVisitor requestID=\"1P59071\" userID=\"esayya9302\" printForm=\"true\"><UserDefinedA>59071</UserDefinedA><UserDefinedB>B63272</UserDefinedB><Biographical><FullName><LastName>HelloWorld</LastName><FirstName>Brian</FirstName></FullName><BirthDate>1980-01-01</BirthDate><Gender>M</Gender><BirthCity>Franica, Paris</BirthCity><BirthCountryCode>FR</BirthCountryCode><CitizenshipCountryCode>FR</CitizenshipCountryCode><PermanentResidenceCountryCode>FR</PermanentResidenceCountryCode><EmailAddress>someone@isp.com</EmailAddress><PhoneNumber>8505551212</PhoneNumber></Biographical><PositionCode>341</PositionCode><PrgStartDate>2016-04-01</PrgStartDate><PrgEndDate>2016-04-30</PrgEndDate><CategoryCode>06</CategoryCode><SubjectField><SubjectFieldCode>52.0301</SubjectFieldCode><Remarks>Accounting</Remarks></SubjectField><USAddress><Address1>2200 C STREET NW</Address1><Address2 xsi:nil=\"true\" /><City>WASHINGTON</City><State>DC</State><PostalCode>20522</PostalCode></USAddress><FinancialInfo><ReceivedUSGovtFunds>false</ReceivedUSGovtFunds><OtherFunds /></FinancialInfo><AddSiteOfActivity><SiteOfActivity xsi:type=\"SOA\"><Address1>2200 C STREET NW</Address1><Address2 xsi:nil=\"true\" /><City>WASHINGTON</City><State>DC</State><PostalCode>20522</PostalCode><SiteName>US Department of State</SiteName><PrimarySite>true</PrimarySite></SiteOfActivity></AddSiteOfActivity></ExchangeVisitor></CreateEV></SEVISBatchCreateUpdateEV>";
        private string uploadUrl = "https://egov.ice.gov/alphasevisbatch/action/batchUpload";
        private string downloadUrl = "https://egov.ice.gov/alphasevisbatch/action/batchDownload";
        public string uploadResultXml = "<Result />";
        string xmlTransactionLogFileName = "xml_transaction_log.xml";
        string xmlTransactionLogString = "<result></result>";

        [TestInitialize]
        public void TestInit()
        {
            mockedEcaHttpMesssageHandlerService = new mockEcaWebRequestHandlerService();
            settings = new NameValueCollection();
            settings.Add("sevis.UploadUri", uploadUrl);
            settings.Add("sevis.DownloadUri", downloadUrl);
            settings.Add("sevis.Thumbprint", "f6d68b2d9b7018ed94f78f18cb6c020e3aed28c7");
            appSettings = new AppSettings(settings,null);
        }


        [TestMethod()]
        public void SevisCommTest()
        {
            var comm = new SevisComm(appSettings, mockedEcaHttpMesssageHandlerService);

            Assert.IsNotNull(comm);
        }

        [TestMethod()]
        public async Task UploadAsyncTest_Base()
        {
            var xml = XElement.Parse(uploadXmlString);
            var comm = new SevisComm(appSettings, mockedEcaHttpMesssageHandlerService);

            Action<HttpResponseMessage> tester = async (s) =>
            {
                Assert.IsTrue(s.IsSuccessStatusCode);
                var xmlResult = await s.Content.ReadAsStringAsync();
                Assert.AreEqual(xmlResult, uploadResultXml);
            };
            var response = await comm.UploadAsync(xml, batchId, orgId, sevisUser);
            tester(response);
        }

        [TestMethod()]
        public async Task DownloadAsyncTest_Base()
        {
            var comm = new SevisComm(appSettings, mockedEcaHttpMesssageHandlerService);

            Action<HttpResponseMessage> tester = async (s) =>
            {
                Assert.IsTrue(s.IsSuccessStatusCode);
                Stream zipStream = await s.Content.ReadAsStreamAsync();
                var zipResult =  new ZipArchive(zipStream);
                var xmlTransactionLogEntry = zipResult.Entries.First(e => e.Name == xmlTransactionLogFileName);
                Assert.IsNotNull(xmlTransactionLogEntry);
                StreamReader reader = new StreamReader(xmlTransactionLogEntry.Open());
                string xmloutput = reader.ReadToEnd();
                Assert.AreEqual(xmloutput, xmlTransactionLogString);
            };
            var response = await comm.DownloadAsync(batchId, orgId, sevisUser);
            tester(response);
        }


    }

    public class mockEcaWebRequestHandlerService: IEcaHttpMessageHandlerService
    {
        public string sevisUser = "testus1234";
        public string orgId = "P-1-12345";
        public string batchId = "2739d72c8a293b";
        public string uploadXmlString = "<SEVISBatchCreateUpdateEV xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:noNamespaceSchemaLocation=\"http://www.ice.gov/xmlschema/sevisbatch/alpha/Create-UpdateExchangeVisitor.xsd\" xmlns:table=\"http://www.ice.gov/xmlschema/sevisbatch/alpha/SEVISTable.xsd\" xmlns:common=\"http://www.ice.gov/xmlschema/sevisbatch/alpha/Common.xsd\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" userID=\"esayya9302\"><BatchHeader><BatchID>2739d72c8a293a</BatchID><OrgID>P-1-19833</OrgID></BatchHeader><CreateEV><ExchangeVisitor requestID=\"1P59071\" userID=\"esayya9302\" printForm=\"true\"><UserDefinedA>59071</UserDefinedA><UserDefinedB>B63272</UserDefinedB><Biographical><FullName><LastName>HelloWorld</LastName><FirstName>Brian</FirstName></FullName><BirthDate>1980-01-01</BirthDate><Gender>M</Gender><BirthCity>Franica, Paris</BirthCity><BirthCountryCode>FR</BirthCountryCode><CitizenshipCountryCode>FR</CitizenshipCountryCode><PermanentResidenceCountryCode>FR</PermanentResidenceCountryCode><EmailAddress>someone@isp.com</EmailAddress><PhoneNumber>8505551212</PhoneNumber></Biographical><PositionCode>341</PositionCode><PrgStartDate>2016-04-01</PrgStartDate><PrgEndDate>2016-04-30</PrgEndDate><CategoryCode>06</CategoryCode><SubjectField><SubjectFieldCode>52.0301</SubjectFieldCode><Remarks>Accounting</Remarks></SubjectField><USAddress><Address1>2200 C STREET NW</Address1><Address2 xsi:nil=\"true\" /><City>WASHINGTON</City><State>DC</State><PostalCode>20522</PostalCode></USAddress><FinancialInfo><ReceivedUSGovtFunds>false</ReceivedUSGovtFunds><OtherFunds /></FinancialInfo><AddSiteOfActivity><SiteOfActivity xsi:type=\"SOA\"><Address1>2200 C STREET NW</Address1><Address2 xsi:nil=\"true\" /><City>WASHINGTON</City><State>DC</State><PostalCode>20522</PostalCode><SiteName>US Department of State</SiteName><PrimarySite>true</PrimarySite></SiteOfActivity></AddSiteOfActivity></ExchangeVisitor></CreateEV></SEVISBatchCreateUpdateEV>";
        public string uploadUrl = "https://egov.ice.gov/alphasevisbatch/action/batchUpload";
        public string downloadUrl = "https://egov.ice.gov/alphasevisbatch/action/batchDownload";
        public string uploadResultXml = "<Result />";
        public string xmlTransactionLogString = "<result></result>";

        public ZipArchive zipResult;
       
        public HttpMessageHandler GetHttpMessageHandler(string Thumbprint)
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.Expect(uploadUrl)
                .Respond("application/xml", uploadResultXml);


            mockHttp.Expect(downloadUrl)
                .Respond("application/zip", GetZipArchiveResult());

            return mockHttp;
        }
        

        private Stream GetZipArchiveResult()
        {
            byte[] xmlTransactionLogFile = System.Text.Encoding.UTF8.GetBytes(xmlTransactionLogString);
            string xmlTransactionLogFileName = "xml_transaction_log.xml";
            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {
                    var fileInArchive = archive.CreateEntry(xmlTransactionLogFileName);
                    using (var entryStream = fileInArchive.Open())
                    using (var fileToCompressStream = new MemoryStream(xmlTransactionLogFile))
                    {
                        fileToCompressStream.CopyTo(entryStream);
                    }
                }
                return outStream;
            }
        }
    }
}