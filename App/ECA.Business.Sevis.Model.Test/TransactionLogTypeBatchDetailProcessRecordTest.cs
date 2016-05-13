using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Sevis.Model.TransLog;

namespace ECA.Business.Sevis.Model.Test
{
    [TestClass]
    public class TransactionLogTypeBatchDetailProcessRecordTest
    {
        [TestMethod]
        public void TestGetRequestId_RequestIdIsNull()
        {
            var instance = new TransactionLogTypeBatchDetailProcessRecord();
            instance.requestID = null;
            Assert.IsNull(instance.GetRequestId());
        }

        [TestMethod]
        public void TestGetRequestId_RequestIdIsEmtpy()
        {
            var instance = new TransactionLogTypeBatchDetailProcessRecord();
            instance.requestID = String.Empty;
            Assert.IsNull(instance.GetRequestId());
        }

        [TestMethod]
        public void TestGetRequestId_RequestIdIsWhitespace()
        {
            var instance = new TransactionLogTypeBatchDetailProcessRecord();
            instance.requestID = " ";
            Assert.IsNull(instance.GetRequestId());
        }
    }
}
