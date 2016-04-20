using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Sevis.Model.Test
{
    [TestClass]
    public class SEVISEVBatchTypeExchangeVisitorTest
    {
        [TestMethod]
        public void TestSetRequestId()
        {
            var participantId = 10;
            var instance = new SEVISEVBatchTypeExchangeVisitor();
            Assert.IsNull(instance.requestID);

            instance.SetRequestId(participantId);
            Assert.IsNotNull(instance.requestID);

            var expectedRequestId = new RequestId(participantId, RequestIdType.Participant, RequestActionType.Create);
            Assert.AreEqual(expectedRequestId.ToString(), instance.requestID);
        }

        [TestMethod]
        public void TestGetRequestId_HasRequestId()
        {
            var participantId = 10;
            var instance = new SEVISEVBatchTypeExchangeVisitor();
            Assert.IsNull(instance.requestID);

            instance.SetRequestId(participantId);
            Assert.IsNotNull(instance.requestID);

            var expectedRequestId = new RequestId(participantId, RequestIdType.Participant, RequestActionType.Create);
            Assert.AreEqual(expectedRequestId.ToString(), instance.GetRequestId().ToString());
        }

        [TestMethod]
        public void TestGetRequestId_RequestIdIsNull()
        {
            var instance = new SEVISEVBatchTypeExchangeVisitor();
            instance.requestID = null;            
            Assert.IsNull(instance.GetRequestId());
        }

        [TestMethod]
        public void TestGetRequestId_RequestIdIsEmtpy()
        {
            var instance = new SEVISEVBatchTypeExchangeVisitor();
            instance.requestID = String.Empty;
            Assert.IsNull(instance.GetRequestId());
        }

        [TestMethod]
        public void TestGetRequestId_RequestIdIsWhitespace()
        {
            var instance = new SEVISEVBatchTypeExchangeVisitor();
            instance.requestID = " ";
            Assert.IsNull(instance.GetRequestId());
        }
    }
}
