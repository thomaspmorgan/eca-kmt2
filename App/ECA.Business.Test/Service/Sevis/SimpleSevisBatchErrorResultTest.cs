using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Sevis.Model.TransLog;
using ECA.Business.Service.Sevis;

namespace ECA.Business.Test.Service.Sevis
{
    [TestClass]
    public class SimpleSevisBatchErrorResultTest
    {
        [TestMethod]
        public void TestConstructor_ResultType()
        {
            var resultType = new ResultType
            {
                ErrorCode = "code",
                ErrorMessage = "message"
            };
            var instance = new SimpleSevisBatchErrorResult(resultType);
            Assert.AreEqual(resultType.ErrorCode, instance.ErrorCode);
            Assert.AreEqual(resultType.ErrorMessage, instance.ErrorMessage);
        }

        [TestMethod]
        public void TestConstructor_ErrorCodeErrorMessage()
        {
            var code = "code";
            var message = "message";
            var instance = new SimpleSevisBatchErrorResult(code, message);
            Assert.AreEqual(code, instance.ErrorCode);
            Assert.AreEqual(message, instance.ErrorMessage);
        }
    }
}
