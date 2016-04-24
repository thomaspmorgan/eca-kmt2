// <copyright file="SevisCommTest.cs">Copyright ©  2015</copyright>
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using ECA.Core.Settings;
using ECA.Net;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Net.Tests
{
    /// <summary>This class contains parameterized unit tests for SevisComm</summary>
    [PexClass(typeof(SevisComm))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class SevisCommTest
    {
        /// <summary>Test stub for .ctor(AppSettings)</summary>
        [PexMethod]
        public SevisComm ConstructorTest(AppSettings appSettings)
        {
            SevisComm target = new SevisComm(appSettings, null);
            return target;
            // TODO: add assertions to method SevisCommTest.ConstructorTest(AppSettings)
        }

        /// <summary>Test stub for DownloadAsync(String, String, String)</summary>
        [PexMethod]
        public Task<HttpResponseMessage> DownloadAsyncTest(
            [PexAssumeUnderTest]SevisComm target,
            string BatchId,
            string OrgId,
            string UserId
        )
        {
            Task<HttpResponseMessage> result = target.DownloadAsync(BatchId, OrgId, UserId);
            return result;
            // TODO: add assertions to method SevisCommTest.DownloadAsyncTest(SevisComm, String, String, String)
        }

        /// <summary>Test stub for UploadAsync(XElement, String, String, String)</summary>
        [PexMethod]
        public Task<HttpResponseMessage> UploadAsyncTest(
            [PexAssumeUnderTest]SevisComm target,
            XElement xml,
            string BatchId,
            string OrgId,
            string UserId
        )
        {
            Task<HttpResponseMessage> result
               = target.UploadAsync(xml, BatchId, OrgId, UserId);
            return result;
            // TODO: add assertions to method SevisCommTest.UploadAsyncTest(SevisComm, XElement, String, String, String)
        }
    }
}
