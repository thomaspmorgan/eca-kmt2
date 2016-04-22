using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Sevis.Model.Test
{
    [TestClass]
    public class BatchIdTest
    {

        [TestMethod]
        public void TestNewBatchId()
        {
            var batchId = BatchId.NewBatchId();
            Assert.IsNotNull(batchId.ToString());
            Assert.IsTrue(batchId.ToString().Length > 0);    
        }

        [TestMethod]
        public void TestConstructor_String_CheckDefaultPaddingChar()
        {
            var instance = BatchId.NewBatchId();

            var batchId = new BatchId(instance.ToString());
            Assert.AreEqual(BatchId.DEFAULT_PADDING_CHARACTER, batchId.PaddingChar);
        }

        [TestMethod]
        public void TestConstructor_Long_CheckDefaultPaddingChar()
        {
            var batchId = new BatchId(1L);
            Assert.AreEqual(BatchId.DEFAULT_PADDING_CHARACTER, batchId.PaddingChar);
        }

        [TestMethod]
        public void TestConstructor_String_CheckUsesGivenPaddingChar()
        {
            var instance = BatchId.NewBatchId();

            var batchId = new BatchId(instance.ToString(), '|');
            Assert.AreEqual('|', batchId.PaddingChar);
        }

        [TestMethod]
        public void TestConstructor_Long_CheckUsesGivenPaddingChar()
        {
            var batchId = new BatchId(1L, '|');
            Assert.AreEqual('|', batchId.PaddingChar);
        }

        [TestMethod]
        public void TestConstructor_BatchIdString()
        {
            var batchId = BatchId.NewBatchId();
            var batchIdAsString = batchId.ToString();

            var instance = new BatchId(batchIdAsString);
            Assert.AreEqual(batchId.Id, instance.Id);
        }

        [TestMethod]
        public void TestConstructor_Id_MaxLongValue()
        {
            var maxLong = Int64.MaxValue;
            var instance = new BatchId(maxLong);
            Assert.AreEqual(instance.Id, instance.Id);
            Assert.AreEqual(BatchId.REQUEST_ID_LENGTH, instance.ToString().Length);
        }

        [TestMethod]
        public void TestConstructor_Id_ZeroValue()
        {
            var maxLong = 0L;
            var instance = new BatchId(maxLong);
            Assert.AreEqual(instance.Id, instance.Id);
            Assert.AreEqual(BatchId.REQUEST_ID_LENGTH, instance.ToString().Length);
        }

        [TestMethod]
        public void TestToString()
        {
            var maxLong = Int64.MaxValue;
            var instance = new BatchId(maxLong);
            Assert.AreEqual(instance.Id, instance.Id);
            Assert.IsNotNull(instance.ToString());
        }

        [TestMethod]
        public void TestToString_CheckUsesPadding()
        {
            var instance = new BatchId(0L);
            Assert.AreEqual(instance.Id, instance.Id);
            Assert.IsNotNull(instance.ToString());
            Assert.IsTrue(instance.ToString().Contains(BatchId.DEFAULT_PADDING_CHARACTER.ToString()));
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var maxLong = Int64.MaxValue;
            var instance = new BatchId(maxLong);
            Assert.AreEqual(maxLong.GetHashCode(), instance.GetHashCode());
        }

        [TestMethod]
        public void TestEquals_NullObject()
        {
            var maxLong = Int64.MaxValue;
            var instance = new BatchId(maxLong);
            Assert.IsFalse(instance.Equals(null));
        }

        [TestMethod]
        public void TestEquals_DifferentObjectType()
        {
            var maxLong = Int64.MaxValue;
            var instance = new BatchId(maxLong);
            Assert.IsFalse(instance.Equals(1));
        }

        [TestMethod]
        public void TestEquals_DifferentBatchId()
        {
            var maxLong = Int64.MaxValue;
            var instance = new BatchId(maxLong);

            var otherInstance = new BatchId(maxLong - 1L);
            Assert.IsFalse(instance.Equals(otherInstance));
        }

        [TestMethod]
        public void TestEquals_SameInstance()
        {
            var maxLong = Int64.MaxValue;
            var instance = new BatchId(maxLong);
            Assert.IsTrue(instance.Equals(instance));
        }

        [TestMethod]
        public void TestEquals_DifferentInstanceSameId()
        {
            var maxLong = Int64.MaxValue;
            var instance = new BatchId(maxLong);
            var otherInstance = new BatchId(maxLong);
            Assert.IsTrue(instance.Equals(otherInstance));
        }
    }
}
