using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Sevis.Model.TransLog;
using System.Collections.Generic;

namespace ECA.Business.Sevis.Model.Test
{
    [TestClass]
    public class GroupedTransactionLogTypeBatchDetailProcessTest
    {
        [TestMethod]
        public void TestAllRecordsSuccessful_HasAllSuccessfulRecords()
        {
            var instance = new GroupedTransactionLogTypeBatchDetailProcess();
            var record1 = new TransactionLogTypeBatchDetailProcessRecord();
            record1.Result = new ResultType
            {
                status = true
            };

            var record2 = new TransactionLogTypeBatchDetailProcessRecord();
            record2.Result = new ResultType
            {
                status = true
            };
            instance.Records = new List<TransactionLogTypeBatchDetailProcessRecord> { record1, record2 };

            Assert.IsTrue(instance.AllRecordsSuccessful());
        }

        [TestMethod]
        public void TestAllRecordsSuccessful_HasOneErrorRecord()
        {
            var instance = new GroupedTransactionLogTypeBatchDetailProcess();
            var record1 = new TransactionLogTypeBatchDetailProcessRecord();
            record1.Result = new ResultType
            {
                status = true
            };

            var record2 = new TransactionLogTypeBatchDetailProcessRecord();
            record2.Result = new ResultType
            {
                status = false
            };
            instance.Records = new List<TransactionLogTypeBatchDetailProcessRecord> { record1, record2 };

            Assert.IsFalse(instance.AllRecordsSuccessful());
        }

        [TestMethod]
        public void TestAllRecordsSuccessful_HasOnlyErrorRecord()
        {
            var instance = new GroupedTransactionLogTypeBatchDetailProcess();

            var record1 = new TransactionLogTypeBatchDetailProcessRecord();
            record1.Result = new ResultType
            {
                status = false
            };
            instance.Records = new List<TransactionLogTypeBatchDetailProcessRecord> { record1 };
            Assert.IsFalse(instance.AllRecordsSuccessful());
        }

        [TestMethod]
        public void TestAllRecordsSuccessful_HasZeroRecords()
        {
            var instance = new GroupedTransactionLogTypeBatchDetailProcess();
            instance.Records = new List<TransactionLogTypeBatchDetailProcessRecord>();
            Assert.IsTrue(instance.AllRecordsSuccessful());
        }
    }
}
