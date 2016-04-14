using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Models.Sevis;
using ECA.Business.Sevis.Model;

namespace ECA.Business.Test.Queries.Models.Sevis
{
    [TestClass]
    public class SevisBatchProcessDTOTest
    {
        [TestMethod]
        public void TestGetUploadDispositionCodeAsCode()
        {
            var code = DispositionCode.Success;
            var dto = new SevisBatchProcessingDTO
            {
                UploadDispositionCode = code.Code
            };
            Assert.AreEqual(code, dto.GetUploadDispositionCodeAsCode());
        }

        [TestMethod]
        public void TestGetUploadDispositionCodeAsCode_CodeIsNull()
        {
            var dto = new SevisBatchProcessingDTO
            {
                UploadDispositionCode = null
            };
            Assert.IsNull(dto.GetUploadDispositionCodeAsCode());
        }

        [TestMethod]
        public void TestGetDownloadDispositionCodeAsCode()
        {
            var code = DispositionCode.Success;
            var dto = new SevisBatchProcessingDTO
            {
                DownloadDispositionCode = code.Code
            };
            Assert.AreEqual(code, dto.GetDownloadDispositionCodeAsCode());
        }

        [TestMethod]
        public void TestGetDownloadDispositionCodeAsCode_CodeIsNull()
        {
            var dto = new SevisBatchProcessingDTO
            {
                DownloadDispositionCode = null
            };
            Assert.IsNull(dto.GetDownloadDispositionCodeAsCode());
        }

        [TestMethod]
        public void TestGetProcessDispositionCodeAsCode()
        {
            var code = DispositionCode.Success;
            var dto = new SevisBatchProcessingDTO
            {
                ProcessDispositionCode = code.Code
            };
            Assert.AreEqual(code, dto.GetProcessDispositionCodeAsCode());
        }

        [TestMethod]
        public void TestGetProcessDispositionCodeAsCode_ProcessCodeIsNull()
        {
            var dto = new SevisBatchProcessingDTO
            {
                ProcessDispositionCode = null
            };
            Assert.IsNull(dto.GetProcessDispositionCodeAsCode());
        }
    }
}
