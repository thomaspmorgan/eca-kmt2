using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ECA.Business.Storage;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using ECA.WebApi.Custom.Storage;
using Microsoft.QualityTools.Testing.Fakes;
using System.IO;
using System.Net.Http.Headers;

namespace ECA.WebApi.Test.Custom.Storage
{
    [TestClass]
    public class FileStorageHandlerTest
    {
        private Mock<IFileStorageService> fileStorageService;
        private FileStorageHandler fileStorageHandler;

        [TestInitialize]
        public void TestInit()
        {
            fileStorageService = new Mock<IFileStorageService>();
            fileStorageHandler = new FileStorageHandler(fileStorageService.Object);
        }

        [TestMethod]
        public async Task TestGetFileAsync()
        {
            using (ShimsContext.Create())
            {
                var byteArray = new byte[1] { (byte)1 };

                var blobShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {
                   
                };


                var shimContainer = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer
                {
                    GetBlockBlobReferenceString = (blbnm) =>
                    {
                        return blobShim;
                    }
                };


                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.NameGet = (blob) =>
                {
                    return "name";
                };

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.ExistsBlobRequestOptionsOperationContext = (blob, opts, contxt) =>
                {
                    return true;
                };

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.OpenReadAsync = (bShim) =>
                {
                    return Task.FromResult<Stream>(new MemoryStream(byteArray));
                };

                var propertiesShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimBlobProperties
                {
                    LengthGet = () =>
                    {
                        return byteArray.Length;
                    },
                    ContentTypeGet = () =>
                    {
                        return "application/pdf";
                    }

                };

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.PropertiesGet = (bShim) =>
                {
                    return propertiesShim;
                };

                fileStorageService.Setup(x => x.GetBlob(It.IsAny<string>(), It.IsAny<string>())).Returns(blobShim);
                var response = await fileStorageHandler.GetFileAsync("test", "test");

                Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
                CollectionAssert.AreEqual(byteArray, await response.Content.ReadAsByteArrayAsync());
                Assert.AreEqual(byteArray.Length, response.Content.Headers.ContentLength);
                Assert.AreEqual(new MediaTypeHeaderValue("application/pdf"), response.Content.Headers.ContentType);
                Assert.AreEqual(new ContentDispositionHeaderValue("attachment") { FileName = "name" }, response.Content.Headers.ContentDisposition);
            }
        }

        [TestMethod]
        public async Task TestGetFileAsync_NullBlob()
        {
            fileStorageService.Setup(x => x.GetBlob(It.IsAny<string>(), It.IsAny<string>())).Returns<CloudBlockBlob>(null);
            var response = await fileStorageHandler.GetFileAsync("test", "test");
            Assert.IsNull(response);
        }
    }
}
