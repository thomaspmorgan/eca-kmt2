using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Sevis.Model.TransLog;

namespace ECA.Business.Sevis.Model.Test
{
    [TestClass]
    public class TransactionLogTypeBatchDetailProcessTest
    {
        [TestMethod]
        public void TestGetGroupedProcessRecords_AllForOneParticipantRecords()
        {
            var participantId = 10;
            var financialInfoRequestId = new RequestId(participantId, RequestIdType.FinancialInfo, RequestActionType.Update);
            var financialInfoRecord = new TransactionLogTypeBatchDetailProcessRecord
            {
                
            };
            financialInfoRecord.requestID = financialInfoRequestId.ToString();

            var subjectFieldRequestId = new RequestId(participantId, RequestIdType.SubjectField, RequestActionType.Update);
            var subjectFieldRecord = new TransactionLogTypeBatchDetailProcessRecord
            {

            };
            subjectFieldRecord.requestID = subjectFieldRequestId.ToString();

            var biographicalRecordRequestId = new RequestId(participantId, RequestIdType.Participant, RequestActionType.Update);
            var biographicalRecord = new TransactionLogTypeBatchDetailProcessRecord
            {

            };
            biographicalRecord.requestID = biographicalRecordRequestId.ToString();

            var instance = new TransactionLogTypeBatchDetailProcess();
            instance.Record = new TransactionLogTypeBatchDetailProcessRecord[] { financialInfoRecord, subjectFieldRecord, biographicalRecord };

            var grouped = instance.GetGroupedProcessRecords().ToList();
            Assert.AreEqual(1, grouped.Count);
            var first = grouped.First();
            Assert.IsTrue(first.IsParticipant);
            Assert.IsFalse(first.IsPersonDependent);
            Assert.AreEqual(participantId, first.ObjectId);
            Assert.AreEqual(3, first.Records.Count());
        }

        [TestMethod]
        public void TestGetGroupedProcessRecords_DifferentParticipantRecords()
        {
            var firstParticipantId = 10;
            var firstParticipantRequestId = new RequestId(firstParticipantId, RequestIdType.Participant, RequestActionType.Create);
            var firstParticipantRecord = new TransactionLogTypeBatchDetailProcessRecord
            {

            };
            firstParticipantRecord.requestID = firstParticipantRequestId.ToString();

            var secondParticipantId = 20;
            var secondParticipantRequestId = new RequestId(secondParticipantId, RequestIdType.Participant, RequestActionType.Create);
            var secondParticipantRecord = new TransactionLogTypeBatchDetailProcessRecord
            {

            };
            secondParticipantRecord.requestID = secondParticipantRequestId.ToString();

            var instance = new TransactionLogTypeBatchDetailProcess();
            instance.Record = new TransactionLogTypeBatchDetailProcessRecord[] { firstParticipantRecord, secondParticipantRecord };

            var grouped = instance.GetGroupedProcessRecords().ToList();
            Assert.AreEqual(2, grouped.Count);
            var first = grouped.First();
            Assert.AreEqual(firstParticipantId, first.ObjectId);

            var second = grouped.Last();
            Assert.AreEqual(secondParticipantId, second.ObjectId);
        }

        [TestMethod]
        public void TestGetGroupedProcessRecords_HasPersonDependentRecord()
        {
            var firstParticipantId = 10;
            var firstParticipantRequestId = new RequestId(firstParticipantId, RequestIdType.Participant, RequestActionType.Create);
            var firstParticipantRecord = new TransactionLogTypeBatchDetailProcessRecord
            {

            };
            firstParticipantRecord.requestID = firstParticipantRequestId.ToString();

            var personDependentId = 1;
            var personDependentRequestId = new RequestId(personDependentId, RequestIdType.Dependent, RequestActionType.Create);
            var personDependentRecord = new TransactionLogTypeBatchDetailProcessRecord
            {

            };
            personDependentRecord.requestID = personDependentRequestId.ToString();


            var instance = new TransactionLogTypeBatchDetailProcess();
            instance.Record = new TransactionLogTypeBatchDetailProcessRecord[] { personDependentRecord, firstParticipantRecord };

            var grouped = instance.GetGroupedProcessRecords().ToList();
            Assert.AreEqual(2, grouped.Count);
            var first = grouped.First();
            Assert.AreEqual(personDependentId, first.ObjectId);
            Assert.IsTrue(first.IsPersonDependent);
            Assert.IsFalse(first.IsParticipant);

            var second = grouped.Last();
            Assert.AreEqual(firstParticipantId, second.ObjectId);
            Assert.IsFalse(second.IsPersonDependent);
            Assert.IsTrue(second.IsParticipant);
        }

    }
}
