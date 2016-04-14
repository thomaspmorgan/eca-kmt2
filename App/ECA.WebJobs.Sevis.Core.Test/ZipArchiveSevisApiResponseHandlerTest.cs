using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.QualityTools.Testing.Fakes;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ECA.Business.Sevis.Model.TransLog;
using Moq;
using ECA.Business.Service.Sevis;
using System.Xml.Serialization;
using ECA.Business.Service;
using ECA.Business.Queries.Models.Sevis;
using System.Reflection;

namespace ECA.WebJobs.Sevis.Core.Test
{
    [TestClass]
    public class ZipArchiveSevisApiResponseHandlerTest
    {
        private Mock<ISevisBatchProcessingService> service;

        [TestInitialize]
        public void TestInit()
        {
            service = new Mock<ISevisBatchProcessingService>();
        }

        [TestMethod]
        public void TestConstructor_UseDefaultZipArchiveDelegate()
        {
            var testService = new ZipArchiveSevisApiResponseHandler(service.Object);
            var delegateField = typeof(ZipArchiveSevisApiResponseHandler).GetField("zipArchiveDelegate", BindingFlags.NonPublic | BindingFlags.Instance);
            var delegateValue = delegateField.GetValue(testService);
            Assert.IsNotNull(delegateField);
            Assert.IsNotNull(delegateValue);
        }

        [TestMethod]
        public void TestConstructor_UseGivenZipArchiveDelegate()
        {
            Func<Stream, ZipArchive> myDelegate = (s) => null;

            var testService = new ZipArchiveSevisApiResponseHandler(service.Object, myDelegate);
            var delegateField = typeof(ZipArchiveSevisApiResponseHandler).GetField("zipArchiveDelegate", BindingFlags.NonPublic | BindingFlags.Instance);
            var delegateValue = delegateField.GetValue(testService);
            Assert.IsNotNull(delegateField);
            Assert.IsNotNull(delegateValue);
            Assert.IsTrue(Object.ReferenceEquals(myDelegate, delegateValue));
        }

        #region Upload
        [TestMethod]
        public async Task TestHandleUploadResponseStreamTask()
        {
            var transactionLog = new TransactionLogType();
            var transactionLogXml = GetXml(transactionLog);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.Unicode);
            writer.Write(transactionLogXml);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            var user = new User(1);
            var dto = new SevisBatchProcessingDTO();
            dto.BatchId = "batchId";

            Action<User, string, string, IDS2019FileProvider> callback = (u, batchId, xml, fileProvider) =>
            {
                Assert.IsTrue(Object.ReferenceEquals(u, user));
                Assert.IsNotNull(fileProvider);
                Assert.IsInstanceOfType(fileProvider, typeof(NullDS2019FileProvider));
                Assert.AreEqual(transactionLogXml, xml);
                Assert.AreEqual(dto.BatchId, batchId);
            };
            service.Setup(x => x.ProcessTransactionLogAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDS2019FileProvider>()))
                .Returns(Task.FromResult<object>(null))
                .Callback(callback);
            var handler = new ZipArchiveSevisApiResponseHandler(service.Object);
            await handler.HandleUploadResponseStreamAsync(user, dto, stream);
        }

        [TestMethod]
        public void TestHandleUploadResponseStream()
        {
            var transactionLog = new TransactionLogType();
            var transactionLogXml = GetXml(transactionLog);

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.Unicode);
            writer.Write(transactionLogXml);
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            var user = new User(1);
            var dto = new SevisBatchProcessingDTO();
            dto.BatchId = "batchId";

            Action<User, string, string, IDS2019FileProvider> callback = (u, batchId, xml, fileProvider) =>
            {
                Assert.IsTrue(Object.ReferenceEquals(u, user));
                Assert.IsNotNull(fileProvider);
                Assert.IsInstanceOfType(fileProvider, typeof(NullDS2019FileProvider));
                Assert.AreEqual(transactionLogXml, xml);
                Assert.AreEqual(dto.BatchId, batchId);
            };
            service.Setup(x => x.ProcessTransactionLog(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDS2019FileProvider>()))
                .Callback(callback);
            var handler = new ZipArchiveSevisApiResponseHandler(service.Object);
            handler.HandleUploadResponseStream(user, dto, stream);
        }
        #endregion

        #region Download
        [TestMethod]
        public async Task TestHandleDownloadResponseStreamAsync()
        {
            using (ShimsContext.Create())
            {
                var transactionLog = new TransactionLogType();
                var transactionLogXml = GetXml(transactionLog);

                var zipFileName = String.Format("{0}", SevisBatchZipArchiveHandler.TRANSACTION_LOG_FILE_NAME);
                var entry = new System.IO.Compression.Fakes.ShimZipArchiveEntry
                {
                    NameGet = () => zipFileName,
                    Open = () =>
                    {
                        var stream = new MemoryStream();
                        var writer = new StreamWriter(stream, Encoding.Unicode);
                        writer.Write(transactionLogXml);
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
                    EntriesGet = () => readonlyEntries,
                };

                var memoryStream = new MemoryStream();
                Func<Stream, ZipArchive> zipArchiveDelegate = (s) =>
                {
                    Assert.IsTrue(Object.ReferenceEquals(memoryStream, s));
                    return zipArchive;
                };

                
                var user = new User(1);
                var dto = new SevisBatchProcessingDTO();
                dto.BatchId = "batchId";
                var handler = new ZipArchiveSevisApiResponseHandler(service.Object, zipArchiveDelegate);
                Action<User, string, string, IDS2019FileProvider> callback = (u, batchId, xml, fileProvider) =>
                {
                    Assert.IsTrue(Object.ReferenceEquals(u, user));
                    Assert.IsNotNull(fileProvider);
                    Assert.IsInstanceOfType(fileProvider, typeof(SevisBatchZipArchiveHandler));
                    Assert.AreEqual(transactionLogXml, xml);
                    Assert.AreEqual(dto.BatchId, batchId);

                };
                service.Setup(x => x.ProcessTransactionLogAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDS2019FileProvider>()))
                    .Returns(Task.FromResult<object>(null))
                    .Callback(callback);

                await handler.HandleDownloadResponseStreamAsync(user, dto, memoryStream);
            }
        }

        [TestMethod]
        public void TestHandleDownloadResponseStream()
        {
            using (ShimsContext.Create())
            {
                var transactionLog = new TransactionLogType();
                var transactionLogXml = GetXml(transactionLog);

                var zipFileName = String.Format("{0}", SevisBatchZipArchiveHandler.TRANSACTION_LOG_FILE_NAME);
                var entry = new System.IO.Compression.Fakes.ShimZipArchiveEntry
                {
                    NameGet = () => zipFileName,
                    Open = () =>
                    {
                        var stream = new MemoryStream();
                        var writer = new StreamWriter(stream, Encoding.Unicode);
                        writer.Write(transactionLogXml);
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
                    EntriesGet = () => readonlyEntries,
                };

                var memoryStream = new MemoryStream();
                Func<Stream, ZipArchive> zipArchiveDelegate = (s) =>
                {
                    Assert.IsTrue(Object.ReferenceEquals(memoryStream, s));
                    return zipArchive;
                };


                var user = new User(1);
                var dto = new SevisBatchProcessingDTO();
                dto.BatchId = "batchId";
                var handler = new ZipArchiveSevisApiResponseHandler(service.Object, zipArchiveDelegate);
                Action<User, string, string, IDS2019FileProvider> callback = (u, batchId, xml, fileProvider) =>
                {
                    Assert.IsTrue(Object.ReferenceEquals(u, user));
                    Assert.IsNotNull(fileProvider);
                    Assert.IsInstanceOfType(fileProvider, typeof(SevisBatchZipArchiveHandler));
                    Assert.AreEqual(transactionLogXml, xml);
                    Assert.AreEqual(dto.BatchId, batchId);

                };
                service.Setup(x => x.ProcessTransactionLog(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDS2019FileProvider>()))
                    .Callback(callback);

                handler.HandleDownloadResponseStream(user, dto, memoryStream);
            }
        }
        #endregion

        #region Dispose

        [TestMethod]
        public void TestDispose_ServiceIsDisposable()
        {
            var disposableService = new Mock<ISevisBatchProcessingService>();
            var disposable = disposableService.As<IDisposable>();
            var serviceToDispose = new ZipArchiveSevisApiResponseHandler(disposableService.Object);
            
            var serviceField = typeof(ZipArchiveSevisApiResponseHandler).GetField("service", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(serviceToDispose);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            serviceToDispose.Dispose();
            serviceValue = serviceField.GetValue(serviceToDispose);
            Assert.IsNull(serviceValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_ServiceIsNotDisposable()
        {
            var disposableService = new Mock<ISevisBatchProcessingService>();
            var serviceToDispose = new ZipArchiveSevisApiResponseHandler(disposableService.Object);

            var serviceField = typeof(ZipArchiveSevisApiResponseHandler).GetField("service", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(serviceToDispose);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            serviceToDispose.Dispose();
            serviceValue = serviceField.GetValue(serviceToDispose);
            Assert.IsNotNull(serviceValue);
        }
        #endregion

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
