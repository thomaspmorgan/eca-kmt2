using ECA.Business.Sevis.Model;
using ECA.Business.Sevis.Model.TransLog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using FluentAssertions;

namespace ECA.Business.Sevis.Model.Test
{
    [TestClass]
    public class RequestIdTest
    {  
		[TestMethod]
		public void TestConstructor_IdRequestIdTypeRequestActionType()
        {
            var id = Int32.MaxValue;
            var requestIdType = RequestIdType.FinancialInfo;
            var actionType = RequestActionType.Update;
            var instance = new RequestId(id, requestIdType, actionType);
            Assert.AreEqual(id, instance.Id);
            Assert.AreEqual(requestIdType, instance.RequestIdType);
            Assert.AreEqual(actionType, instance.RequestActionType);
        }

        [TestMethod]
        public void TestConstructor_String_InvalidRequestIdString()
        {
            var idString = "hello world";
            Action a = () => new RequestId(idString);
            a.ShouldThrow<NotSupportedException>().WithMessage("The request id string is not a valid request id.");
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var id = Int32.MaxValue;
            var requestIdType = RequestIdType.FinancialInfo;
            var actionType = RequestActionType.Update;
            var instance = new RequestId(id, requestIdType, actionType);
            Assert.AreNotEqual(0, instance.GetHashCode());
        }

        [TestMethod]
        public void TestEquals_NullObject()
        {
            var id = Int32.MaxValue;
            var requestIdType = RequestIdType.FinancialInfo;
            var actionType = RequestActionType.Update;
            var instance = new RequestId(id, requestIdType, actionType);
            Assert.IsFalse(instance.Equals(null));
        }

        [TestMethod]
        public void TestEquals_DifferentObjectType()
        {
            var id = Int32.MaxValue;
            var requestIdType = RequestIdType.FinancialInfo;
            var actionType = RequestActionType.Update;
            var instance = new RequestId(id, requestIdType, actionType);
            Assert.IsFalse(instance.Equals(1));
        }

        [TestMethod]
        public void TestEquals_SameRequestIdDifferentInstance()
        {
            var id = Int32.MaxValue;
            var requestIdType = RequestIdType.FinancialInfo;
            var actionType = RequestActionType.Update;

            var instance = new RequestId(id, requestIdType, actionType);
			var otherInstance = new RequestId(id, requestIdType, actionType);
            Assert.IsTrue(instance.Equals(otherInstance));
        }

        [TestMethod]
        public void TestEquals_SameInstance()
        {
            var id = Int32.MaxValue;
            var requestIdType = RequestIdType.FinancialInfo;
            var actionType = RequestActionType.Update;

            var instance = new RequestId(id, requestIdType, actionType);
            Assert.IsTrue(instance.Equals(instance));
        }

        [TestMethod]
        public void TestEquals_DifferentRequestId()
        {
            var id = Int32.MaxValue;
            var requestIdType = RequestIdType.FinancialInfo;
            var actionType = RequestActionType.Update;

            var firstInstance = new RequestId(id, requestIdType, actionType);
            var secondInstance = new RequestId(id + 1, requestIdType, actionType);

            Assert.IsFalse(firstInstance.Equals(secondInstance));
        }

        [TestMethod]
        public void TestToString()
        {
            var id = Int32.MaxValue;
            var requestIdType = RequestIdType.FinancialInfo;
            var actionType = RequestActionType.Update;
            var instance = new RequestId(id, requestIdType, actionType);

            var requestIdTypeAsString = ((int)requestIdType).ToString();
            var actionTypeAsString = ((int)actionType).ToString();

            var s = instance.ToString();
            Assert.IsTrue(s.Contains(id.ToString()));
            Assert.IsTrue(s.Contains(actionTypeAsString));
            Assert.IsTrue(s.Contains(requestIdTypeAsString));
            Assert.IsTrue(s.Contains("-"));
            Assert.IsTrue(s.Length <= 20);

            var parsedRequestId = new RequestId(s);
            Assert.AreEqual(id, parsedRequestId.Id);
            Assert.AreEqual(requestIdType, parsedRequestId.RequestIdType);
            Assert.AreEqual(actionType, parsedRequestId.RequestActionType);
        }


        [TestMethod]
        public void TestIsPersonDependentId()
        {
            var id = Int32.MaxValue;
            var requestIdType = RequestIdType.Dependent;
            var actionType = RequestActionType.Update;
            var instance = new RequestId(id, requestIdType, actionType);
            Assert.IsTrue(instance.IsPersonDependentId);
            Assert.IsFalse(instance.IsParticipantId);
        }

        [TestMethod]
        public void TestIsParticipantId_ParticipantRequestIdType()
        {
            var id = Int32.MaxValue;
            var requestIdType = RequestIdType.Participant;
            var actionType = RequestActionType.Update;
            var instance = new RequestId(id, requestIdType, actionType);
            Assert.IsTrue(instance.IsParticipantId);
            Assert.IsFalse(instance.IsPersonDependentId);
        }

        [TestMethod]
        public void TestIsParticipantId_FinancialInfoRequestIdType()
        {
            var id = Int32.MaxValue;
            var requestIdType = RequestIdType.FinancialInfo;
            var actionType = RequestActionType.Update;
            var instance = new RequestId(id, requestIdType, actionType);
            Assert.IsTrue(instance.IsParticipantId);
            Assert.IsFalse(instance.IsPersonDependentId);
        }

        [TestMethod]
        public void TestIsParticipantId_SubjectFieldRequestIdType()
        {
            var id = Int32.MaxValue;
            var requestIdType = RequestIdType.SubjectField;
            var actionType = RequestActionType.Update;
            var instance = new RequestId(id, requestIdType, actionType);
            Assert.IsTrue(instance.IsParticipantId);
            Assert.IsFalse(instance.IsPersonDependentId);
        }
    }
}
