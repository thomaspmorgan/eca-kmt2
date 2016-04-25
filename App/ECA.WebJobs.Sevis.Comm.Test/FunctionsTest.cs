using ECA.Business.Queries.Models.Sevis;
using ECA.Business.Service;
using ECA.Business.Service.Sevis;
using ECA.Core.Settings;
using ECA.WebJobs.Sevis.Comm;
using ECA.WebJobs.Sevis.Core;
using FluentAssertions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using ECA.Net;

namespace ECA.WebJobs.Sevis.Staging.Test
{
    public class TestTimerSchedule : TimerSchedule
    {
        public TestTimerSchedule()
        {

        }

        public override DateTime GetNextOccurrence(DateTime now)
        {
            return DateTime.UtcNow.AddDays(1.0);
        }
    }

    [TestClass]
    public class FunctionsTest
    {
        private Mock<ISevisBatchProcessingService> service;
        private Mock<ISevisApiResponseHandler> responseHandler;
        private Mock<IEcaHttpMessageHandlerService> requestHandlerService;
        private Functions instance;
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;

        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            service = new Mock<ISevisBatchProcessingService>();
            responseHandler = new Mock<ISevisApiResponseHandler>();
            requestHandlerService = new Mock<IEcaHttpMessageHandlerService>();
            instance = new Functions(service.Object, responseHandler.Object, requestHandlerService.Object, settings);
        }

        [TestMethod]
        public void TestGetSystemUser()
        {
            var userId = 1;
            appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
            var user = instance.GetSystemUser();
            Assert.AreEqual(userId, user.Id);
        }

        [TestMethod]
        public async Task TestProcessTimer()
        {
            var userId = 1;
            appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
            appSettings.Add(AppSettings.SEVIS_MAX_CREATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");
            appSettings.Add(AppSettings.SEVIS_MAX_UPDATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");
            appSettings.Add(AppSettings.SEVIS_UPLOAD_URI_KEY, "https://egov.ice.gov/alphasevisbatch/action/batchUpload");
            appSettings.Add(AppSettings.SEVIS_DOWNLOAD_URI_KEY, "https://egov.ice.gov/alphasevisbatch/action/batchDownload");
            appSettings.Add(AppSettings.SEVIS_MAX_UPDATE_EXCHANGE_VISITOR_RECORDS_PER_BATCH, "1");
            appSettings.Add(AppSettings.SEVIS_THUMBPRINT, "f14e780d72921fda4b8079d887114dfd1922d624");
            var timerInfo = new TimerInfo(new TestTimerSchedule());
            service.Setup(x => x.GetNextBatchToDownloadAsync()).ReturnsAsync(null);
            service.Setup(x => x.GetNextBatchToUploadAsync()).ReturnsAsync(null);
            Func<Task> f = () => instance.ProcessTimer(timerInfo);
            f.ShouldNotThrow();
        }

        #region ProcessAsync
        [TestMethod]
        public async Task TestProcessAsync()
        {
            using (ShimsContext.Create())
            {
                var userId = 1;
                appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
                
                var uploadCalled = false;
                var downloadCalled = false;
                var dto = new SevisBatchProcessingDTO
                {
                    BatchId = "batchId",
                    SendString = "<root></root>",
                    SevisOrgId = "sevis org Id",
                    SevisUsername = "sevis username"
                };
                var responseContent = "hello world";
                var message = new HttpResponseMessage
                {
                    Content = new StringContent(responseContent),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
                var shimComm = new ECA.Net.Fakes.ShimSevisComm
                {
                    UploadAsyncXElementStringStringString = (xElement, batchId, sOrgId, sUsername) =>
                    {
                        uploadCalled = true;
                        service.Setup(x => x.GetNextBatchToUploadAsync()).ReturnsAsync(null);
                        return Task.FromResult<HttpResponseMessage>(message);
                    },
                    DownloadAsyncStringStringString = (batchId, sOrgId, sUsername) =>
                    {
                        downloadCalled = true;
                        service.Setup(x => x.GetNextBatchToDownloadAsync()).ReturnsAsync(null);
                        return Task.FromResult<HttpResponseMessage>(message);
                    }
                };
                service.Setup(x => x.GetNextBatchToDownloadAsync()).ReturnsAsync(dto);
                service.Setup(x => x.GetNextBatchToUploadAsync()).ReturnsAsync(dto);
                await instance.ProcessAsync(service.Object, shimComm, settings);
                Assert.IsTrue(uploadCalled);
                Assert.IsTrue(downloadCalled);
                service.Verify(x => x.DeleteProcessedBatchesAsync(), Times.Once());
            }
        }
        #endregion

        #region Upload
        [TestMethod]
        public async Task TestUploadBatchAsync_IsSuccess()
        {
            using (ShimsContext.Create())
            {
                var userId = 1;
                appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
                var dto = new SevisBatchProcessingDTO
                {
                    BatchId = "batchId",
                    SendString = "<root></root>",
                    SevisOrgId = "sevis org Id",
                    SevisUsername = "sevis username"
                };
                var responseContent = "hello world";
                var message = new HttpResponseMessage
                {
                    Content = new StringContent(responseContent),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
                var shimComm = new ECA.Net.Fakes.ShimSevisComm
                {
                    UploadAsyncXElementStringStringString = (xElement, batchId, sOrgId, sUsername) =>
                    {
                        Assert.AreEqual(dto.BatchId, batchId);
                        Assert.AreEqual(dto.SevisOrgId, sOrgId);
                        Assert.AreEqual(dto.SevisUsername, sUsername);
                        return Task.FromResult<HttpResponseMessage>(message);
                    }
                };
                Action<User, SevisBatchProcessingDTO, Stream> callback = (u, d, s) =>
                {
                    Assert.AreEqual(userId, u.Id);
                    Assert.IsTrue(Object.ReferenceEquals(dto, d));
                    Assert.IsNotNull(s);
                    using (var streamReader = new StreamReader(s))
                    {
                        var stringContent = streamReader.ReadToEnd();
                        Assert.AreEqual(responseContent, stringContent);
                    }
                };
                responseHandler.Setup(x => x.HandleUploadResponseStreamAsync(It.IsAny<User>(), It.IsAny<SevisBatchProcessingDTO>(), It.IsAny<Stream>()))
                    .Returns(Task.FromResult<Object>(null))
                    .Callback(callback);

                await instance.UploadBatchAsync(shimComm, dto);
                responseHandler.Verify(x => x.HandleUploadResponseStreamAsync(It.IsAny<User>(), It.IsAny<SevisBatchProcessingDTO>(), It.IsAny<Stream>()), Times.Once());
            }
        }

        [TestMethod]
        public async Task TestUploadBatchAsync_IsFailure()
        {
            using (ShimsContext.Create())
            {
                var userId = 1;
                appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
                var dto = new SevisBatchProcessingDTO
                {
                    Id = 1,
                    BatchId = "batchId",
                    SendString = "<root></root>",
                    SevisOrgId = "sevis org Id",
                    SevisUsername = "sevis username"
                };
                var responseContent = "hello world";
                var message = new HttpResponseMessage
                {
                    Content = new StringContent(responseContent),
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                var shimComm = new ECA.Net.Fakes.ShimSevisComm
                {
                    UploadAsyncXElementStringStringString = (xElement, batchId, sOrgId, sUsername) =>
                    {
                        Assert.AreEqual(dto.BatchId, batchId);
                        Assert.AreEqual(dto.SevisOrgId, sOrgId);
                        Assert.AreEqual(dto.SevisUsername, sUsername);
                        return Task.FromResult<HttpResponseMessage>(message);
                    }
                };
                Action<int, Exception> callback = (bId, exc) =>
                {
                    Assert.AreEqual(dto.Id, bId);
                    Assert.IsNull(exc);

                };

                service.Setup(x => x.HandleFailedUploadBatchAsync(It.IsAny<int>(), It.IsAny<Exception>()))
                    .Returns(Task.FromResult<object>(null))
                    .Callback(callback);

                await instance.UploadBatchAsync(shimComm, dto);
                service.Verify(x => x.HandleFailedUploadBatchAsync(It.IsAny<int>(), It.IsAny<Exception>()), Times.Once());
            }
        }

        [TestMethod]
        public async Task TestUploadBatchesAsync_HasBatchToUpload()
        {
            using (ShimsContext.Create())
            {
                var userId = 1;
                appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
                var dto = new SevisBatchProcessingDTO
                {
                    BatchId = "batchId",
                    SendString = "<root></root>",
                    SevisOrgId = "sevis org Id",
                    SevisUsername = "sevis username"
                };
                service.Setup(x => x.GetNextBatchToUploadAsync()).ReturnsAsync(dto);

                var responseContent = "hello world";
                var message = new HttpResponseMessage
                {
                    Content = new StringContent(responseContent),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
                var shimComm = new ECA.Net.Fakes.ShimSevisComm
                {
                    UploadAsyncXElementStringStringString = (xElement, batchId, sOrgId, sUsername) =>
                    {
                        Assert.AreEqual(dto.BatchId, batchId);
                        Assert.AreEqual(dto.SevisOrgId, sOrgId);
                        Assert.AreEqual(dto.SevisUsername, sUsername);
                        service.Setup(x => x.GetNextBatchToUploadAsync()).ReturnsAsync(null);
                        return Task.FromResult<HttpResponseMessage>(message);
                    }
                };
                Action<User, SevisBatchProcessingDTO, Stream> callback = (u, d, s) =>
                {
                    Assert.AreEqual(userId, u.Id);
                    Assert.IsTrue(Object.ReferenceEquals(dto, d));
                    Assert.IsNotNull(s);
                    using (var streamReader = new StreamReader(s))
                    {
                        var stringContent = streamReader.ReadToEnd();
                        Assert.AreEqual(responseContent, stringContent);
                    }

                };
                responseHandler.Setup(x => x.HandleUploadResponseStreamAsync(It.IsAny<User>(), It.IsAny<SevisBatchProcessingDTO>(), It.IsAny<Stream>()))
                    .Returns(Task.FromResult<Object>(null))
                    .Callback(callback);

                await instance.UploadBatchesAsync(shimComm);
                responseHandler.Verify(x => x.HandleUploadResponseStreamAsync(It.IsAny<User>(), It.IsAny<SevisBatchProcessingDTO>(), It.IsAny<Stream>()), Times.Once());
            }
        }

        [TestMethod]
        public async Task TestUploadBatchesAsync_DoesNotHaveBatchToUpload()
        {
            using (ShimsContext.Create())
            {
                var userId = 1;
                appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
                service.Setup(x => x.GetNextBatchToUploadAsync()).ReturnsAsync(null);

                var shimComm = new ECA.Net.Fakes.ShimSevisComm
                {

                };

                await instance.UploadBatchesAsync(shimComm);
                responseHandler.Verify(x => x.HandleUploadResponseStreamAsync(It.IsAny<User>(), It.IsAny<SevisBatchProcessingDTO>(), It.IsAny<Stream>()), Times.Never());
            }
        }

        #endregion

        #region Download
        [TestMethod]
        public async Task TestDownloadBatchAsync_IsSuccess()
        {
            using (ShimsContext.Create())
            {
                var userId = 1;
                appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
                var dto = new SevisBatchProcessingDTO
                {
                    BatchId = "batchId",
                    SendString = "<root></root>",
                    SevisOrgId = "sevis org Id",
                    SevisUsername = "sevis username"
                };
                var responseContent = "hello world";
                var message = new HttpResponseMessage
                {
                    Content = new StringContent(responseContent),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
                var shimComm = new ECA.Net.Fakes.ShimSevisComm
                {
                    DownloadAsyncStringStringString = (batchId, sOrgId, sUsername) =>
                    {
                        Assert.AreEqual(dto.BatchId, batchId);
                        Assert.AreEqual(dto.SevisOrgId, sOrgId);
                        Assert.AreEqual(dto.SevisUsername, sUsername);
                        return Task.FromResult<HttpResponseMessage>(message);
                    }
                };
                Action<User, SevisBatchProcessingDTO, Stream> callback = (u, d, s) =>
                {
                    Assert.AreEqual(userId, u.Id);
                    Assert.IsTrue(Object.ReferenceEquals(dto, d));
                    Assert.IsNotNull(s);
                    using (var streamReader = new StreamReader(s))
                    {
                        var stringContent = streamReader.ReadToEnd();
                        Assert.AreEqual(responseContent, stringContent);
                    }
                };
                responseHandler.Setup(x => x.HandleDownloadResponseStreamAsync(It.IsAny<User>(), It.IsAny<SevisBatchProcessingDTO>(), It.IsAny<Stream>()))
                    .Returns(Task.FromResult<Object>(null))
                    .Callback(callback);

                await instance.DownloadBatchAsync(shimComm, dto);
                responseHandler.Verify(x => x.HandleDownloadResponseStreamAsync(It.IsAny<User>(), It.IsAny<SevisBatchProcessingDTO>(), It.IsAny<Stream>()), Times.Once());
            }
        }

        [TestMethod]
        public async Task TestDownloadBatchAsync_IsFailure()
        {
            using (ShimsContext.Create())
            {
                var userId = 1;
                appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
                var dto = new SevisBatchProcessingDTO
                {
                    Id = 1,
                    BatchId = "batchId",
                    SendString = "<root></root>",
                    SevisOrgId = "sevis org Id",
                    SevisUsername = "sevis username"
                };
                var responseContent = "hello world";
                var message = new HttpResponseMessage
                {
                    Content = new StringContent(responseContent),
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                var shimComm = new ECA.Net.Fakes.ShimSevisComm
                {
                    DownloadAsyncStringStringString = (batchId, sOrgId, sUsername) =>
                    {
                        Assert.AreEqual(dto.BatchId, batchId);
                        Assert.AreEqual(dto.SevisOrgId, sOrgId);
                        Assert.AreEqual(dto.SevisUsername, sUsername);
                        return Task.FromResult<HttpResponseMessage>(message);
                    }
                };
                Action<int, Exception> callback = (id, exc) =>
                {
                    Assert.AreEqual(dto.Id, id);
                    Assert.IsNull(exc);
                };
                service.Setup(x => x.HandleFailedDownloadBatchAsync(It.IsAny<int>(), It.IsAny<Exception>()))
                    .Returns(Task.FromResult<Object>(null))
                    .Callback(callback);

                await instance.DownloadBatchAsync(shimComm, dto);
                service.Verify(x => x.HandleFailedDownloadBatchAsync(It.IsAny<int>(), It.IsAny<Exception>()), Times.Once());
            }
        }

        [TestMethod]
        public async Task TestDownloadBatceshAsync_HasBatchToDownload()
        {
            using (ShimsContext.Create())
            {
                var userId = 1;
                appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
                var dto = new SevisBatchProcessingDTO
                {
                    BatchId = "batchId",
                    SendString = "<root></root>",
                    SevisOrgId = "sevis org Id",
                    SevisUsername = "sevis username"
                };
                service.Setup(x => x.GetNextBatchToDownloadAsync()).ReturnsAsync(dto);
                var responseContent = "hello world";
                var message = new HttpResponseMessage
                {
                    Content = new StringContent(responseContent),
                    StatusCode = System.Net.HttpStatusCode.OK
                };
                var shimComm = new ECA.Net.Fakes.ShimSevisComm
                {
                    DownloadAsyncStringStringString = (batchId, sOrgId, sUsername) =>
                    {
                        Assert.AreEqual(dto.BatchId, batchId);
                        Assert.AreEqual(dto.SevisOrgId, sOrgId);
                        Assert.AreEqual(dto.SevisUsername, sUsername);
                        service.Setup(x => x.GetNextBatchToDownloadAsync()).ReturnsAsync(null);
                        return Task.FromResult<HttpResponseMessage>(message);
                    }
                };
                Action<User, SevisBatchProcessingDTO, Stream> callback = (u, d, s) =>
                {
                    Assert.AreEqual(userId, u.Id);
                    Assert.IsTrue(Object.ReferenceEquals(dto, d));
                    Assert.IsNotNull(s);
                    using (var streamReader = new StreamReader(s))
                    {
                        var stringContent = streamReader.ReadToEnd();
                        Assert.AreEqual(responseContent, stringContent);
                    }
                };
                responseHandler.Setup(x => x.HandleDownloadResponseStreamAsync(It.IsAny<User>(), It.IsAny<SevisBatchProcessingDTO>(), It.IsAny<Stream>()))
                    .Returns(Task.FromResult<Object>(null))
                    .Callback(callback);

                await instance.DownloadBatchesAsync(shimComm);
                responseHandler.Verify(x => x.HandleDownloadResponseStreamAsync(It.IsAny<User>(), It.IsAny<SevisBatchProcessingDTO>(), It.IsAny<Stream>()), Times.Once());
            }
        }

        [TestMethod]
        public async Task TestDownloadBatceshAsync_NoBatches()
        {
            using (ShimsContext.Create())
            {
                var userId = 1;
                appSettings.Add(AppSettings.SYSTEM_USER_ID_KEY, userId.ToString());
                
                service.Setup(x => x.GetNextBatchToDownloadAsync()).ReturnsAsync(null);
                var shimComm = new ECA.Net.Fakes.ShimSevisComm
                {
                   
                };

                await instance.DownloadBatchesAsync(shimComm);
                responseHandler.Verify(x => x.HandleDownloadResponseStreamAsync(It.IsAny<User>(), It.IsAny<SevisBatchProcessingDTO>(), It.IsAny<Stream>()), Times.Never());
            }
        }
        #endregion

        #region Dispose
        [TestMethod]
        public void TestDispose_Service()
        {
            var disposableService = new Mock<ISevisBatchProcessingService>();
            var disposable = disposableService.As<IDisposable>();
            instance = new Functions(disposableService.Object, responseHandler.Object, requestHandlerService.Object, settings);
            
            var serviceField = typeof(Functions).GetField("service", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            instance.Dispose();
            serviceValue = serviceField.GetValue(instance);
            Assert.IsNull(serviceValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_Service_NotDisposable()
        {
            var disposableService = new Mock<ISevisBatchProcessingService>();
            instance = new Functions(disposableService.Object, responseHandler.Object, requestHandlerService.Object, settings);

            var serviceField = typeof(Functions).GetField("service", BindingFlags.NonPublic | BindingFlags.Instance);
            var serviceValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceField);
            Assert.IsNotNull(serviceValue);

            instance.Dispose();
            serviceValue = serviceField.GetValue(instance);
            Assert.IsNotNull(serviceValue);
        }

        [TestMethod]
        public void TestDispose_ResponseHandler()
        {
            var disposableService = new Mock<ISevisApiResponseHandler>();
            var disposable = disposableService.As<IDisposable>();
            instance = new Functions(service.Object, disposableService.Object, requestHandlerService.Object, settings);

            var responseHandlerField = typeof(Functions).GetField("responseHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            var handlerValue = responseHandlerField.GetValue(instance);
            Assert.IsNotNull(responseHandlerField);
            Assert.IsNotNull(handlerValue);

            instance.Dispose();
            handlerValue = responseHandlerField.GetValue(instance);
            Assert.IsNull(handlerValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_ResponseHandler_NotDisposable()
        {
            var disposableService = new Mock<ISevisApiResponseHandler>();
            instance = new Functions(service.Object, disposableService.Object, requestHandlerService.Object, settings);

            var responseHandlerField = typeof(Functions).GetField("responseHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            var handlerValue = responseHandlerField.GetValue(instance);
            Assert.IsNotNull(responseHandlerField);
            Assert.IsNotNull(handlerValue);

            instance.Dispose();
            handlerValue = responseHandlerField.GetValue(instance);
            Assert.IsNotNull(handlerValue);
        }

        [TestMethod]
        public void TestDispose_ECAWebRequestHandlerr()
        {
            var disposableService = new Mock<IEcaHttpMessageHandlerService>();
            var disposable = disposableService.As<IDisposable>();
            instance = new Functions(service.Object, responseHandler.Object, disposableService.Object, settings);

            var ecaHttpMessageHandlerServiceField = typeof(Functions).GetField("ecaHttpMessageHandlerService", BindingFlags.NonPublic | BindingFlags.Instance);
            var handlerValue = ecaHttpMessageHandlerServiceField.GetValue(instance);
            Assert.IsNotNull(ecaHttpMessageHandlerServiceField);
            Assert.IsNotNull(handlerValue);

            instance.Dispose();
            handlerValue = ecaHttpMessageHandlerServiceField.GetValue(instance);
            Assert.IsNull(handlerValue);
            disposable.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_ECAWebRequestHandler_NotDisposable()
        {
            var disposableService = new Mock<IEcaHttpMessageHandlerService>();
            instance = new Functions(service.Object, responseHandler.Object, disposableService.Object, settings);

            var ecaHttpMessageHandlerServiceField = typeof(Functions).GetField("ecaHttpMessageHandlerService", BindingFlags.NonPublic | BindingFlags.Instance);
            var handlerValue = ecaHttpMessageHandlerServiceField.GetValue(instance);
            Assert.IsNotNull(ecaHttpMessageHandlerServiceField);
            Assert.IsNotNull(handlerValue);

            instance.Dispose();
            handlerValue = ecaHttpMessageHandlerServiceField.GetValue(instance);
            Assert.IsNotNull(handlerValue);
        }
        #endregion
    }
}
