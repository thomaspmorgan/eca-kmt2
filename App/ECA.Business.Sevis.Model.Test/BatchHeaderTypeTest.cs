using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Sevis.Model.Test
{
    [TestClass]
    public class BatchHeaderTypeTest
    {
        [TestMethod]
        public void TestGetBatchId_BatchIdIsNull()
        {
            var instance = new BatchHeaderType();
            instance.BatchID = null;
            Assert.IsNull(instance.GetBatchId());
        }

        [TestMethod]
        public void TestGetBatchId_HasBatchId()
        {
            var expectedBatchId = BatchId.NewBatchId();
            var instance = new BatchHeaderType();
            instance.BatchID = expectedBatchId.ToString();
            Assert.AreEqual(expectedBatchId, instance.GetBatchId());
        }

        [TestMethod]
        public void TestSetBatchId_NullValue()
        {   
            var instance = new BatchHeaderType();
            instance.BatchID = "batch id";
            instance.SetBatchId(null);
            Assert.IsNull(instance.BatchID);
        }

        [TestMethod]
        public void TestSetBatchId()
        {
            var expectedBatchId = BatchId.NewBatchId();
            var instance = new BatchHeaderType();
            instance.BatchID = "batch id";
            instance.SetBatchId(expectedBatchId);
            Assert.AreEqual(expectedBatchId.ToString(), instance.BatchID);
        }
    }
}
