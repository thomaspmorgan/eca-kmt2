using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ECA.Core.Settings;
using System.Collections.Specialized;
using System.Configuration;
using Moq;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace ECA.Business.Storage.Test
{
    [TestClass]
    public class FileStorageServieTest
    {
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;
        private BlobStorageSettings blobStorageSettings;
        private Mock<IBlobStorageSettings> iBlobStorageSettings;
        private FileStorageService service;
        private Mock<IFileStorageService> iService;

        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            blobStorageSettings = new BlobStorageSettings(settings);
            iBlobStorageSettings = new Mock<IBlobStorageSettings>();
            service = new FileStorageService(settings, iBlobStorageSettings.Object);
            iService = new Mock<IFileStorageService>();
        }

        [TestMethod]
        public void TestBlobContainer()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;

                var expectedUriString = "http://wwww.google.com";
                var expectedUri = new Uri(expectedUriString);

                string containerString = "ds2019";

                var containerShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer
                {

                };

                var blobClientShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobClient
                {
                    GetContainerReferenceString = (cntnrName) =>
                    {
                        Assert.AreEqual(containerString, cntnrName);
                        return containerShim;
                    }
                };

                var storageAccountShim = new Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount
                {
                    CreateCloudBlobClient = () =>
                    {
                        return blobClientShim;
                    }
                };

                var serviceShim = Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount.ParseString = (conString) =>
                {
                    return storageAccountShim;
                };

                CloudBlobContainer actualContainer = blobStorageSettings.BlobContainer("Storage Container");

                Assert.IsTrue(object.ReferenceEquals(containerShim.Instance, actualContainer));
            }
        }

        [TestMethod]
        public void TestBlobContainer_InvalidContainerName()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;

                var expectedUriString = "http://wwww.google.com";
                var expectedUri = new Uri(expectedUriString);

                string containerString = "ds2019";

                var containerShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer
                {

                };

                var blobClientShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobClient
                {
                    GetContainerReferenceString = (cntnrName) =>
                    {
                        Assert.AreEqual(containerString, cntnrName);
                        return containerShim;
                    }
                };

                var storageAccountShim = new Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount
                {
                    CreateCloudBlobClient = () =>
                    {
                        return blobClientShim;
                    }
                };

                var serviceShim = Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount.ParseString = (conString) =>
                {
                    return storageAccountShim;
                };

                CloudBlobContainer actualContainer = blobStorageSettings.BlobContainer("");

                Assert.IsFalse(object.ReferenceEquals(containerShim.Instance, actualContainer));
            }
        }

        //[TestMethod]
        //public void TestCreateBlockBlob()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        appSettings.Add(AppSettings.STORAGE_CONTAINER, "Storage Container");
        //        appSettings.Add(AppSettings.STORAGE_CONNECTION_STRING, "key=value");
        //        string containerString = settings.StorageContainer;
        //        string connectionString = settings.StorageConnectionString;

        //        string blobName = "BlobName";
        //        string contentType = "application/pdf";

        //        var blockBlobShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
        //        {

        //        };

        //        var containerShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer
        //        {
        //            GetBlockBlobReferenceString = (blbNm) =>
        //            {
        //                return blockBlobShim;
        //            }
        //        };

        //        var blobClientShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobClient
        //        {
        //            GetContainerReferenceString = (cntnrName) =>
        //            {
        //                Assert.AreEqual(containerString, cntnrName);
        //                return containerShim;
        //            }
        //        };

        //        var storageAccountShim = new Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount
        //        {
        //            CreateCloudBlobClient = () =>
        //            {
        //                return blobClientShim;
        //            }
        //        };

        //        var serviceShim = Microsoft.WindowsAzure.Storage.Fakes.ShimCloudStorageAccount.ParseString = (conString) =>
        //        {
        //            return storageAccountShim;
        //        };

        //        iBlobStorageSettings.Setup(x => x.BlobContainer())
        //            .Returns(containerShim);

        //        CloudBlockBlob actualBlob = blobStorageSettings.CreateBlockBlob(contentType, blobName);

        //        Assert.IsTrue(object.ReferenceEquals(blockBlobShim.Instance, actualBlob));

        //        //var propertyShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimBlobProperties
        //        //{

        //        //};

        //        //Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimBlobProperties.AllInstances.ContentTypeSetString = (blbprp, cntntTyp) =>
        //        //{

        //        //};

        //        //Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimBlobProperties.AllInstances.CacheControlSetString = (blbprp, chCntrls) =>
        //        //{

        //        //};
        //    }
        //}

        [TestMethod]
        public async Task TestUploadBlobAsync()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;
                var expectedUriString = "http://wwww.google.com";
                var expectedUri = new Uri(expectedUriString);

                byte[] blobData = new byte[1] { (byte)1 };

                string contentType = "application/pdf";
                string blobName = "Test_Async_" + new Guid().ToString();
                string containerName = "ds2019";

                var shim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {
                    UploadFromByteArrayAsyncByteArrayInt32Int32 = (buffer, index, count) =>
                    {
                        CollectionAssert.AreEqual(blobData, buffer);
                        return Task.FromResult<Object>(null);
                    },
                };
                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.UriGet = (blb) =>
                {
                    return expectedUri;
                };
                Action<string, string, string> callbackTester = (cntType, bName, contName) =>
                {
                    Assert.AreEqual(contentType, cntType);
                    Assert.AreEqual(blobName, bName);
                };

                iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(shim)
                    .Callback(callbackTester);
                Uri blobUploadUri = service.UploadBlob(blobData, contentType, blobName, containerName);
                Assert.AreEqual(expectedUri, blobUploadUri);

                blobUploadUri = await service.UploadBlobAsync(blobData, contentType, blobName, containerName);
                Assert.AreEqual(expectedUri, blobUploadUri);
            }

            //Assert.AreEqual("https://" + settings.StorageName + ".blob.core.windows.net/" + settings.StorageContainer + "/" + blobName, blobUploadUri);
            //Assert.AreEqual(blobUploadUri, service.GetBlobLocation(blobName));
            //Assert.IsTrue(await service.DeleteBlobAsync(blobName));
        }

        [TestMethod]
        public async Task TestUploadBlobAsync_Stream()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;
                var expectedUriString = "http://wwww.google.com";
                var expectedUri = new Uri(expectedUriString);

                Stream blobData = new System.IO.MemoryStream();
                string contentType = "application/pdf";
                string blobName = "Test_Async_" + new Guid().ToString();
                string containerName = "ds2019";

                var shim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {
                    UploadFromStreamAsyncStream = (blbData) =>
                    {
                        return Task.FromResult<Object>(null);
                    }
                };
                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.UriGet = (blb) =>
                {
                    return expectedUri;
                };
                Action<string, string, string> callbackTester = (cntType, bName, contName) =>
                {
                    Assert.AreEqual(contentType, cntType);
                    Assert.AreEqual(blobName, bName);
                };

                iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(shim)
                    .Callback(callbackTester);

                Uri blobUploadUri = service.UploadBlob(blobData, contentType, blobName, containerName);
                Assert.AreEqual(expectedUri, blobUploadUri);

                blobUploadUri = await service.UploadBlobAsync(blobData, contentType, blobName, containerName);
                Assert.AreEqual(expectedUri, blobUploadUri);

                
            }
        }

        [TestMethod]
        public async Task TestBlobUploadAsync_EmptyArray()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;
                var expectedUriString = "http://wwww.google.com";
                var expectedUri = new Uri(expectedUriString);

                byte[] blobData = new byte[0];
                string contentType = "application/pdf";
                string blobName = "Test_Async_" + new Guid().ToString();
                string containerName = "ds2019";

                var shim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {
                    UploadFromByteArrayAsyncByteArrayInt32Int32 = (buffer, index, count) =>
                    {
                        CollectionAssert.AreEqual(blobData, buffer);
                        return Task.FromResult<Object>(null);
                    },
                };
                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.UriGet = (blb) =>
                {
                    return expectedUri;
                };
                Action<string, string> callbackTester = (cntType, bName) =>
                {
                    Assert.AreEqual(contentType, cntType);
                    Assert.AreEqual(blobName, bName);
                };

                iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(shim)
                    .Callback(callbackTester);

                Uri blobUploadUri = service.UploadBlob(blobData, contentType, blobName, containerName);
                Assert.AreNotEqual(expectedUri, blobUploadUri);

                blobUploadUri = await service.UploadBlobAsync(blobData, contentType, blobName, containerName);
                Assert.AreNotEqual(expectedUri, blobUploadUri);

                //Assert.AreEqual("Invalid Array", await service.UploadBlobAsync(blobData, contentType, blobName));
            }
        }

        [TestMethod]
        public async Task TestBlobUploadAsync_InvalidContentType()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;
                var expectedUriString = "http://wwww.google.com";
                var expectedUri = new Uri(expectedUriString);

                byte[] blobData = new byte[1] { (byte)1 };

                string contentType = "pdf";
                string blobName = "Test_Async_" + new Guid().ToString();
                string containerName = "ds2019";

                var shim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {
                    UploadFromByteArrayAsyncByteArrayInt32Int32 = (buffer, index, count) =>
                    {
                        CollectionAssert.AreEqual(blobData, buffer);
                        return Task.FromResult<Object>(null);
                    },
                };
                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.UriGet = (blb) =>
                {
                    return expectedUri;
                };
                Action<string, string> callbackTester = (cntType, bName) =>
                {
                    Assert.AreEqual(contentType, cntType);
                    Assert.AreEqual(blobName, bName);
                };

                iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(shim)
                    .Callback(callbackTester);
                Uri blobUploadUri = service.UploadBlob(blobData, contentType, blobName, containerName);
                Assert.AreNotEqual(expectedUri, blobUploadUri);

                blobUploadUri = await service.UploadBlobAsync(blobData, contentType, blobName, containerName);
                Assert.AreNotEqual(expectedUri, blobUploadUri);

                //Assert.AreEqual("Invalid Content Type", service.UploadBlobAsync(blobData, contentType, blobName));
            }
        }

        [TestMethod]
        public async Task TestBlobUploadAsync_InvalidContainerName()
        {
            using (ShimsContext.Create()) 
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;
                var expectedUriString = "http://wwww.google.com";
                var expectedUri = new Uri(expectedUriString);

                byte[] blobData = new byte[1] { (byte)1 };

                string contentType = "application/pdf";
                string blobName = "Test_Async_" + new Guid().ToString();
                string containerName = "";

                var shim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {
                    UploadFromByteArrayAsyncByteArrayInt32Int32 = (buffer, index, count) =>
                    {
                        CollectionAssert.AreEqual(blobData, buffer);
                        return Task.FromResult<Object>(null);
                    },
                };
                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.UriGet = (blb) =>
                {
                    return expectedUri;
                };
                Action<string, string> callbackTester = (cntType, bName) =>
                {
                    Assert.AreEqual(contentType, cntType);
                    Assert.AreEqual(blobName, bName);
                };

                iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(shim)
                    .Callback(callbackTester);
                Uri blobUploadUri = service.UploadBlob(blobData, contentType, blobName, containerName);
                Assert.AreNotEqual(expectedUri, blobUploadUri);

                blobUploadUri = await service.UploadBlobAsync(blobData, contentType, blobName, containerName);
                Assert.AreNotEqual(expectedUri, blobUploadUri);

                //Assert.AreEqual("Invalid Content Type", service.UploadBlobAsync(blobData, contentType, blobName));
            }
        }

        //[TestMethod]
        //public void TestUploadBlob_Stream_BadContentType()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
        //        string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;
        //        var expectedUriString = "http://wwww.google.com";
        //        var expectedUri = new Uri(expectedUriString);

        //        Stream blobData = new System.IO.MemoryStream();
        //        string contentType = "pdf";
        //        string blobName = "Test_" + new Guid().ToString();

        //        var shim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
        //        {
        //            UploadFromStreamStreamAccessConditionBlobRequestOptionsOperationContext = (blbData, accCond, reqOpts, opCntxt) =>
        //            {

        //            }

        //        };
        //        Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.UriGet = (blb) =>
        //        {
        //            return expectedUri;
        //        };
        //        Action<string, string> callbackTester = (cntType, bName) =>
        //        {
        //            Assert.AreEqual(contentType, cntType);
        //            Assert.AreEqual(blobName, bName);
        //        };

        //        iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>()))
        //            .Returns(shim)
        //            .Callback(callbackTester);
        //        string blobUploadUri = service.UploadBlob(blobData, contentType, blobName, containerName);
        //        Assert.AreEqual(expectedUri, blobUploadUri);
        //    }

        //}

        //[TestMethod]
        //public async Task TestUploadBlobAsync_Stream_BadContentType()
        //{
        //    using (ShimsContext.Create())
        //    {
        //        connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
        //        string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;
        //        var expectedUriString = "http://wwww.google.com";
        //        var expectedUri = new Uri(expectedUriString);

        //        Stream blobData = new System.IO.MemoryStream();
        //        string contentType = "pdf";
        //        string blobName = "Test_Async_" + new Guid().ToString();

        //        var shim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
        //        {
        //            UploadFromStreamAsyncStream = (blbData) =>
        //            {
        //                return Task.FromResult<Object>(null);
        //            }
        //        };
        //        Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.UriGet = (blb) =>
        //        {
        //            return expectedUri;
        //        };
        //        Action<string, string> callbackTester = (cntType, bName) =>
        //        {
        //            Assert.AreEqual(contentType, cntType);
        //            Assert.AreEqual(blobName, bName);
        //        };

        //        iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>()))
        //            .Returns(shim)
        //            .Callback(callbackTester);
        //        string blobUploadUri = await service.UploadBlobAsync(blobData, contentType, blobName, containerName);
        //        Assert.AreEqual(expectedUri, blobUploadUri);
        //    }
        //}

        [TestMethod]
        public async Task TestDeleteBlobAsync()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;

                bool deleted = false;

                string blobName = "TestBlob";
                string containerName = "ds2019";

                var blobShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {

                };

                iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(blobShim);

                var shimContainer = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer
                {
                    GetBlockBlobReferenceString = (blbnm) =>
                    {
                        return blobShim;
                    }
                };

                iBlobStorageSettings.Setup(x => x.BlobContainer("ds2019"))
                    .Returns(shimContainer);

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.DeleteDeleteSnapshotsOptionAccessConditionBlobRequestOptionsOperationContext = (cBlob, snapOption, accCond, reqOpts, opsCont) =>
                {
                    deleted = true;
                };

                Assert.IsFalse(deleted);
                service.DeleteBlob(blobName, containerName);
                Assert.IsTrue(deleted);

                deleted = false;

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.DeleteAsync = (cBlob) =>
                {
                    deleted = true;
                    return Task.FromResult<Object>(null);
                };

                Assert.IsFalse(deleted);
                await service.DeleteBlobAsync(blobName, containerName);
                Assert.IsTrue(deleted);
            }
        }

        [TestMethod]
        public async Task TestDeleteBlobAsync_InvalidName()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;

                bool deleted = true;

                string blobName = "TestBlob";
                string containerName = "ds2019";

                var blobShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {

                };

                iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(blobShim);

                var shimContainer = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer
                {
                    GetBlockBlobReferenceString = (blbnm) =>
                    {
                        return blobShim;
                    }
                };

                iBlobStorageSettings.Setup(x => x.BlobContainer("ds2019"))
                    .Returns(shimContainer);

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.DeleteDeleteSnapshotsOptionAccessConditionBlobRequestOptionsOperationContext = (cBlob, snapOption, accCond, reqOpts, opsCont) =>
                {
                    deleted = false;
                };

                service.DeleteBlob(blobName, containerName);
                Assert.IsFalse(deleted);

                deleted = true;

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.DeleteAsync = (cBlob) =>
                {
                    deleted = false;
                    return Task.FromResult<Object>(null);
                };

                await service.DeleteBlobAsync(blobName, containerName);
                Assert.IsFalse(deleted);

            }
            //Assert.IsFalse(await service.DeleteBlobAsync("This blob doesn't exist"));
        }

        [TestMethod]
        public void TestGetBlob()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;

                string blobName = "TestBlob";
                string containerName = "ds2019";

                var blobShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {

                };

                iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(blobShim);

                var shimContainer = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer
                {
                    GetBlockBlobReferenceString = (blbnm) =>
                    {
                        return blobShim;
                    }
                };

                iBlobStorageSettings.Setup(x => x.BlobContainer("ds2019"))
                    .Returns(shimContainer);

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.ExistsBlobRequestOptionsOperationContext = (blob, opts, contxt) =>
                {
                    return true;
                };

                var testInstance = service.GetBlob(blobName, containerName);

                Assert.IsNotNull(testInstance);
            }
        }

        [TestMethod]
        public void TestGetBlobLocation()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;

                string blobName = "TestBlob";
                string containerName = "ds2019";

                string blobUriString = "http://www.google.com";

                var blobShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {

                };

                iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(blobShim);

                var shimContainer = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer
                {
                    GetBlockBlobReferenceString = (blbnm) =>
                    {
                        return blobShim;
                    }
                };

                iBlobStorageSettings.Setup(x => x.BlobContainer("ds2019"))
                    .Returns(shimContainer);

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.ExistsBlobRequestOptionsOperationContext = (blob, opts, contxt) =>
                {
                    return true;
                };

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.UriGet = (blb) =>
                {
                    return new Uri(blobUriString);
                };

                var testInstance = service.GetBlobLocation(blobName, containerName);

                Assert.IsNotNull(testInstance);
            }
        }

        [TestMethod]
        public void TestGetBlob_InvalidName()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;

                string blobName = "This Blob Doesn't Exist";
                string containerName = "ds2019";

                string blobUriString = "http://www.google.com"; //Blob location
                var blobUri = new Uri(blobUriString);

                var blobShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {

                };

                iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(blobShim);

                var shimContainer = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer
                {
                    GetBlockBlobReferenceString = (blbnm) =>
                    {
                        return blobShim;
                    }
                };

                iBlobStorageSettings.Setup(x => x.BlobContainer("ds2019"))
                    .Returns(shimContainer);

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.ExistsBlobRequestOptionsOperationContext = (blb, opts, contxt) =>
                {
                    return false;
                };

                CloudBlob blob = service.GetBlob(blobName, containerName);
                Assert.IsNull(blob);
            }

            //Assert.IsNull(service.GetBlob("This blob doesn't exist"));
        }

        [TestMethod]
        public void TestGetBlobLocation_InvalidName()
        {
            using (ShimsContext.Create())
            {
                appSettings.Add(AppSettings.SEVIS_DS2019_STORAGE_CONTAINER, "Storage Container");
                string containerString = settings.DS2019FileStorageContainer;

                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;

                string blobName = "Null Blob";
                string containerName = "ds2019";

                string blobUriString = "http://www.google.com";
                var blobUri = new Uri(blobUriString);

                var expectedBlobShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {

                };

                var containerShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer
                {
                    GetBlockBlobReferenceString = (name) =>
                    {
                        return null;
                    }
                };

                var existsShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob
                {
                    ExistsBlobRequestOptionsOperationContext = (exBlob, options) =>
                    {
                        return false;
                    }
                };

                iBlobStorageSettings.Setup(x => x.BlobContainer("ds2019")).Returns(containerShim);

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer.AllInstances.GetBlobReferenceString = (container, blbnam) =>
                {
                    return expectedBlobShim;
                };

                string blobLocation = service.GetBlobLocation(blobName, containerName);
                Assert.AreNotEqual(blobUriString, blobLocation);
            }

            //Assert.AreEqual("Invalid Blob Name", service.GetBlobLocation("This blob doesn't exist"));
        }

        [TestMethod]
        public void TestGetBlob_InvalidContainer()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;

                string blobName = "TestBlob";
                string containerName = "";

                var blobShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {

                };

                iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(blobShim);

                var shimContainer = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer
                {
                    GetBlockBlobReferenceString = (blbnm) =>
                    {
                        return blobShim;
                    }
                };

                iBlobStorageSettings.Setup(x => x.BlobContainer("ds2019"))
                    .Returns(shimContainer);

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.ExistsBlobRequestOptionsOperationContext = (blob, opts, contxt) =>
                {
                    return true;
                };

                var testInstance = service.GetBlob(blobName, containerName);

                Assert.IsNull(testInstance);
            }
        }

        [TestMethod]
        public void TestGetBlobLocation_InvalidContainer()
        {
            using (ShimsContext.Create())
            {
                connectionStrings.Add(new ConnectionStringSettings(AppSettings.SEVIS_DS2019_STORAGE_CONNECTION_STRING_KEY, "connection string"));
                string connectionString = settings.DS2019FileStorageConnectionString.ConnectionString;

                string blobName = "TestBlob";
                string containerName = "";

                string blobUriString = "http://www.google.com";

                var blobShim = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlockBlob
                {

                };

                iBlobStorageSettings.Setup(x => x.CreateBlockBlob(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(blobShim);

                var shimContainer = new Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlobContainer
                {
                    GetBlockBlobReferenceString = (blbnm) =>
                    {
                        return blobShim;
                    }
                };

                iBlobStorageSettings.Setup(x => x.BlobContainer("ds2019"))
                    .Returns(shimContainer);

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.ExistsBlobRequestOptionsOperationContext = (blob, opts, contxt) =>
                {
                    return true;
                };

                Microsoft.WindowsAzure.Storage.Blob.Fakes.ShimCloudBlob.AllInstances.UriGet = (blb) =>
                {
                    return new Uri(blobUriString);
                };

                var testInstance = service.GetBlobLocation(blobName, containerName);

                Assert.IsNull(testInstance);
            }
        }
    }
}
