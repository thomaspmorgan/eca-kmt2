using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.Compression;
using Microsoft.QualityTools.Testing.Fakes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using ECA.Business.Sevis.Model.TransLog;
using System.Xml.Serialization;
using System.Text;
using ECA.Business.Sevis.Model;

namespace ECA.WebJobs.Sevis.Core.Test
{
    [TestClass]
    public class SevisBatchZipArchiveHandlerTest
    {
        [TestMethod]
        public void TestZipArchiveConstructor_UseDefaultTransactionLogFileName()
        {
            using (ShimsContext.Create())
            {
                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive();
                var archiveHandler = new SevisBatchZipArchiveHandler(zipArchive);
                Assert.IsNotNull(archiveHandler.ZipArchive);
                Assert.AreEqual(SevisBatchZipArchiveHandler.TRANSACTION_LOG_FILE_NAME, archiveHandler.TransactionLogFileName);
            }
        }

        [TestMethod]
        public void TestZipArchiveConstructor_UseGivenTransactionLogFileName()
        {
            using (ShimsContext.Create())
            {
                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive();
                var transactionLogFileName = "hello world";
                var archiveHandler = new SevisBatchZipArchiveHandler(zipArchive, transactionLogFileName);
                Assert.IsNotNull(archiveHandler.ZipArchive);
                Assert.AreEqual(transactionLogFileName, archiveHandler.TransactionLogFileName);
            }
        }

        [TestMethod]
        public void TestGetStream_ArchiveHasFile()
        {
            using (ShimsContext.Create())
            {
                var memoryStream = new MemoryStream();
                var sevisId = "N000012345";
                var now = DateTime.UtcNow;
                var zipFileName = String.Format("_{0}_{1}.pdf", sevisId, now);
                var entry = new System.IO.Compression.Fakes.ShimZipArchiveEntry
                {
                    NameGet = () => zipFileName,
                    Open = () => memoryStream
                };

                var entries = new List<ZipArchiveEntry>();
                entries.Add(entry);
                var readonlyEntries = new ReadOnlyCollection<ZipArchiveEntry>(entries);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive
                {
                    EntriesGet = () => readonlyEntries
                };

                var archiveHandler = new SevisBatchZipArchiveHandler(zipArchive);
                Assert.IsTrue(Object.ReferenceEquals(memoryStream, archiveHandler.GetStream(sevisId)));
                memoryStream.Dispose();
            }
        }

        [TestMethod]
        public void TestGetStream_ArchiveDoesNotHaveFile()
        {
            using (ShimsContext.Create())
            {
                var sevisId = "N000012345";

                var entries = new List<ZipArchiveEntry>();
                var readonlyEntries = new ReadOnlyCollection<ZipArchiveEntry>(entries);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive
                {
                    EntriesGet = () => readonlyEntries
                };

                var archiveHandler = new SevisBatchZipArchiveHandler(zipArchive);
                Assert.IsNull(archiveHandler.GetStream(sevisId));
            }
        }

        [TestMethod]
        public async Task TestGetDS2019FileStream_ArchiveHasFile()
        {
            using (ShimsContext.Create())
            {
                var memoryStream = new MemoryStream();
                var sevisId = "N000012345";
                var now = DateTime.UtcNow;
                var zipFileName = String.Format("_{0}_{1}.pdf", sevisId, now);
                var entry = new System.IO.Compression.Fakes.ShimZipArchiveEntry
                {
                    NameGet = () => zipFileName,
                    Open = () => memoryStream
                };

                var entries = new List<ZipArchiveEntry>();
                entries.Add(entry);
                var readonlyEntries = new ReadOnlyCollection<ZipArchiveEntry>(entries);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive
                {
                    EntriesGet = () => readonlyEntries
                };

                var archiveHandler = new SevisBatchZipArchiveHandler(zipArchive);

                Action<Stream> tester = (testStream) =>
                {
                    Assert.IsTrue(Object.ReferenceEquals(memoryStream, testStream));
                };
                var requestId = new RequestId(1, RequestIdType.Participant, RequestActionType.Create);
                var stream = archiveHandler.GetDS2019FileStream(requestId, sevisId);
                var streamAsync = await archiveHandler.GetDS2019FileStreamAsync(requestId, sevisId);
                tester(stream);
                tester(streamAsync);

                memoryStream.Dispose();
            }
        }

        [TestMethod]
        public async Task TestGetDS2019FileStream_ArchiveDoesNotHaveFile()
        {
            using (ShimsContext.Create())
            {
                var sevisId = "N000012345";
                var now = DateTime.UtcNow;
                var zipFileName = String.Format("_{0}_{1}.pdf", sevisId, now);

                var entries = new List<ZipArchiveEntry>();
                var readonlyEntries = new ReadOnlyCollection<ZipArchiveEntry>(entries);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive
                {
                    EntriesGet = () => readonlyEntries
                };

                var archiveHandler = new SevisBatchZipArchiveHandler(zipArchive);

                Action<Stream> tester = (testStream) =>
                {
                    Assert.IsNull(testStream);
                };
                var requestId = new RequestId(1, RequestIdType.Participant, RequestActionType.Create);
                var stream = archiveHandler.GetDS2019FileStream(requestId, sevisId);
                var streamAsync = await archiveHandler.GetDS2019FileStreamAsync(requestId, sevisId);
                tester(stream);
                tester(streamAsync);
            }
        }

        [TestMethod]
        public void TestGetTransactionLogXml_ArchiveHasFile()
        {
            using (ShimsContext.Create())
            {
                var transactionLog = new TransactionLogType();
                var xml = GetXml(transactionLog);                

                var xmlTransactionLogFileName = "transaction_log.xml";
                var zipFileName = String.Format("{0}", xmlTransactionLogFileName);
                var entry = new System.IO.Compression.Fakes.ShimZipArchiveEntry
                {
                    NameGet = () => zipFileName,
                    Open = () =>
                    {
                        var stream = new MemoryStream();
                        var writer = new StreamWriter(stream, Encoding.Unicode);
                        writer.Write(xml);
                        writer.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        return stream;
                    }
                };

                var entries = new List<ZipArchiveEntry>();
                entries.Add(entry);
                var readonlyEntries = new ReadOnlyCollection<ZipArchiveEntry>(entries);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive
                {
                    EntriesGet = () => readonlyEntries
                };

                var archiveHandler = new SevisBatchZipArchiveHandler(zipArchive, xmlTransactionLogFileName);
                var testXml = archiveHandler.GetTransactionLogXml();
                Assert.AreEqual(xml, testXml);
            }
        }

        [TestMethod]
        public void TestGetTransactionLogXml_ArchiveDoesNotHaveFile()
        {
            using (ShimsContext.Create())
            {
                var transactionLog = new TransactionLogType();
                var xml = GetXml(transactionLog);

                var xmlTransactionLogFileName = "transaction_log.xml";
                var zipFileName = String.Format("{0}", xmlTransactionLogFileName);
                

                var entries = new List<ZipArchiveEntry>();
                var readonlyEntries = new ReadOnlyCollection<ZipArchiveEntry>(entries);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive
                {
                    EntriesGet = () => readonlyEntries
                };

                var archiveHandler = new SevisBatchZipArchiveHandler(zipArchive, xmlTransactionLogFileName);
                var testXml = archiveHandler.GetTransactionLogXml();
                Assert.IsNull(testXml);
            }
        }

        [TestMethod]
        public async Task TestGetTransactionLogXmlAsync_ArchiveHasFile()
        {
            using (ShimsContext.Create())
            {
                var transactionLog = new TransactionLogType();
                var xml = GetXml(transactionLog);

                var xmlTransactionLogFileName = "transaction_log.xml";
                var zipFileName = String.Format("{0}", xmlTransactionLogFileName);
                var entry = new System.IO.Compression.Fakes.ShimZipArchiveEntry
                {
                    NameGet = () => zipFileName,
                    Open = () =>
                    {
                        var stream = new MemoryStream();
                        var writer = new StreamWriter(stream, Encoding.Unicode);
                        writer.Write(xml);
                        writer.Flush();
                        stream.Seek(0, SeekOrigin.Begin);
                        return stream;
                    }
                };

                var entries = new List<ZipArchiveEntry>();
                entries.Add(entry);
                var readonlyEntries = new ReadOnlyCollection<ZipArchiveEntry>(entries);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive
                {
                    EntriesGet = () => readonlyEntries
                };

                var archiveHandler = new SevisBatchZipArchiveHandler(zipArchive, xmlTransactionLogFileName);
                var testXml = await archiveHandler.GetTransactionLogXmlAsync();
                Assert.AreEqual(xml, testXml);
            }
        }

        [TestMethod]
        public async Task TestGetTransactionLogXmlAsync_ArchiveDoesNotHaveFile()
        {
            using (ShimsContext.Create())
            {
                var transactionLog = new TransactionLogType();
                var xml = GetXml(transactionLog);

                var xmlTransactionLogFileName = "transaction_log.xml";
                var zipFileName = String.Format("{0}", xmlTransactionLogFileName);


                var entries = new List<ZipArchiveEntry>();
                var readonlyEntries = new ReadOnlyCollection<ZipArchiveEntry>(entries);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive
                {
                    EntriesGet = () => readonlyEntries
                };

                var archiveHandler = new SevisBatchZipArchiveHandler(zipArchive, xmlTransactionLogFileName);
                var testXml = await archiveHandler.GetTransactionLogXmlAsync();
                Assert.IsNull(testXml);
            }
        }

        [TestMethod]
        public void TestDispose()
        {
            using (ShimsContext.Create())
            {
                var sevisId = "N000012345";
                var now = DateTime.UtcNow;
                var zipFileName = String.Format("_{0}_{1}.pdf", sevisId, now);

                var zipArchive = new System.IO.Compression.Fakes.ShimZipArchive();

                var archiveHandler = new SevisBatchZipArchiveHandler(zipArchive);
                Assert.IsNotNull(archiveHandler.ZipArchive);
                archiveHandler.Dispose();
                Assert.IsNull(archiveHandler.ZipArchive);
            }
        }

        private string GetXml(TransactionLogType transactionLog)
        {
            using (var textWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(TransactionLogType));
                serializer.Serialize(textWriter, transactionLog);
                var xml = textWriter.ToString();
                return xml;
            }
        }
    }
}
