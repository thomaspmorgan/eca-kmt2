using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ECA.Business.Sevis.Model.Test
{
    [TestClass]
    public class SEVISEVBatchTypeTest
    {
        [TestMethod]
        public void TestContainsDeletedParticipantDependent_UpdateEVArrayIsNull()
        {
            var dependentSevisId = "sevis id";
            var batch = new SEVISEVBatchType();
            Assert.IsNull(batch.UpdateEV);
            Assert.IsFalse(batch.ContainsDeletedParticipantDependent(dependentSevisId));
        }

        [TestMethod]
        public void TestContainsDeletedParticipantDependent_UpdateEVArrayIsEmpty()
        {
            var dependentSevisId = "sevis id";
            var batch = new SEVISEVBatchType();
            batch.UpdateEV = new List<SEVISEVBatchTypeExchangeVisitor1>().ToArray();
            Assert.AreEqual(0, batch.UpdateEV.Count());
            Assert.IsFalse(batch.ContainsDeletedParticipantDependent(dependentSevisId));
        }

        [TestMethod]
        public void TestContainsDeletedParticipantDependent_HasDependent()
        {
            var dependentSevisId = "sevis id";
            var batch = new SEVISEVBatchType();
            var updatedExchangeVisitors = new List<SEVISEVBatchTypeExchangeVisitor1>();
            var dependent = new SEVISEVBatchTypeExchangeVisitorDependent
            {
                Item = new SEVISEVBatchTypeExchangeVisitorDependentDelete
                {
                    dependentSevisID = dependentSevisId
                }
            };
            var updatedExchangeVisitor = new SEVISEVBatchTypeExchangeVisitor1
            {
                Item = dependent
            };
            updatedExchangeVisitors.Add(updatedExchangeVisitor);

            batch.UpdateEV = updatedExchangeVisitors.ToArray();
            Assert.IsTrue(batch.ContainsDeletedParticipantDependent(dependentSevisId));
        }

        [TestMethod]
        public void TestContainsDeletedParticipantDependent_DifferentDependentSevisId()
        {
            var dependentSevisId = "sevis id";
            var batch = new SEVISEVBatchType();
            var updatedExchangeVisitors = new List<SEVISEVBatchTypeExchangeVisitor1>();
            var dependent = new SEVISEVBatchTypeExchangeVisitorDependent
            {
                Item = new SEVISEVBatchTypeExchangeVisitorDependentDelete
                {
                    dependentSevisID = dependentSevisId
                }
            };
            var updatedExchangeVisitor = new SEVISEVBatchTypeExchangeVisitor1
            {
                Item = dependent
            };
            updatedExchangeVisitors.Add(updatedExchangeVisitor);

            batch.UpdateEV = updatedExchangeVisitors.ToArray();
            Assert.IsTrue(batch.ContainsDeletedParticipantDependent(dependentSevisId));
            Assert.IsFalse(batch.ContainsDeletedParticipantDependent("other sevis id"));
        }

        [TestMethod]
        public void TestContainsDeletedParticipantDependent_DependentSevisIdIsNull()
        {
            var dependentSevisId = "sevis id";
            var batch = new SEVISEVBatchType();
            var updatedExchangeVisitors = new List<SEVISEVBatchTypeExchangeVisitor1>();
            var dependent = new SEVISEVBatchTypeExchangeVisitorDependent
            {
                Item = new SEVISEVBatchTypeExchangeVisitorDependentDelete
                {
                    dependentSevisID = dependentSevisId
                }
            };
            var updatedExchangeVisitor = new SEVISEVBatchTypeExchangeVisitor1
            {
                Item = dependent
            };
            updatedExchangeVisitors.Add(updatedExchangeVisitor);

            batch.UpdateEV = updatedExchangeVisitors.ToArray();
            Assert.IsTrue(batch.ContainsDeletedParticipantDependent(dependentSevisId));
            Assert.IsFalse(batch.ContainsDeletedParticipantDependent(null));
        }
    }
}
