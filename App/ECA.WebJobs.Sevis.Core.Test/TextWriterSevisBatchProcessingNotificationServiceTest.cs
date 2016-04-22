using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Sevis.Model;
using FluentAssertions;
using ECA.Business.Service.Sevis;
using System.Collections.Generic;

namespace ECA.WebJobs.Sevis.Core.Test
{
    [TestClass]
    public class TextWriterSevisBatchProcessingNotificationServiceTest
    {
        [TestMethod]
        public void TestNotifyFinishedProcessingSevisBatchDetails()
        {
            var instance = new TextWriterSevisBatchProcessingNotificationService();
            Action a = () => instance.NotifyFinishedProcessingSevisBatchDetails("batchId", DispositionCode.BatchNeverSubmitted);
            a.ShouldNotThrow();
        }

        [TestMethod]
        public void TestNotifyDownloadedBatchProcessed()
        {
            var instance = new TextWriterSevisBatchProcessingNotificationService();
            Action a = () => instance.NotifyDownloadedBatchProcessed("batchId", DispositionCode.BatchNeverSubmitted);
            a.ShouldNotThrow();
        }

        [TestMethod]
        public void TestNotifyInvalidExchangeVisitor()
        {
            var instance = new TextWriterSevisBatchProcessingNotificationService();
            Action a = () => instance.NotifyInvalidExchangeVisitor(null);
            a.ShouldNotThrow();
        }

        [TestMethod]
        public void TestNotifyNumberOfParticipantsToStage()
        {
            var instance = new TextWriterSevisBatchProcessingNotificationService();
            Action a = () => instance.NotifyNumberOfParticipantsToStage(1);
            a.ShouldNotThrow();
        }

        [TestMethod]
        public void TestNotifyStagedSevisBatchCreated()
        {
            var instance = new TextWriterSevisBatchProcessingNotificationService();
            Action a = () => instance.NotifyStagedSevisBatchCreated(new StagedSevisBatch(BatchId.NewBatchId(), "", ""));
            a.ShouldNotThrow();
        }

        [TestMethod]
        public void TestNotifyStagedSevisBatchesFinished()
        {
            var instance = new TextWriterSevisBatchProcessingNotificationService();
            instance.NotifyNumberOfParticipantsToStage(1);
            Action a = () => instance.NotifyStagedSevisBatchesFinished(new List<StagedSevisBatch>());
            a.ShouldNotThrow();
        }

        [TestMethod]
        public void TestNotifyUploadedBatchProcessed()
        {
            var instance = new TextWriterSevisBatchProcessingNotificationService();
            Action a = () => instance.NotifyUploadedBatchProcessed("batchId", DispositionCode.BatchNeverSubmitted);
            a.ShouldNotThrow();
        }

        [TestMethod]
        public void TestNotifyStartedProcessingSevisBatchDetails()
        {
            var instance = new TextWriterSevisBatchProcessingNotificationService();
            Action a = () => instance.NotifyStartedProcessingSevisBatchDetails("batchId", 1, 1);
            a.ShouldNotThrow();
        }

        [TestMethod]
        public void TestNotifyDeletedSevisBatchProcessing()
        {
            var instance = new TextWriterSevisBatchProcessingNotificationService();
            Action a = () => instance.NotifyDeletedSevisBatchProcessing(1, "batchId");
            a.ShouldNotThrow();
        }

        [TestMethod]
        public void TestNotifyCancelledSevisBatch()
        {
            var instance = new TextWriterSevisBatchProcessingNotificationService();
            Action a = () => instance.NotifyCancelledSevisBatch("batchId", "reason");
            a.ShouldNotThrow();
        }
    }
}
