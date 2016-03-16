using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    public class TestDependent : Dependent
    {
        public override object GetSevisExhangeVisitorDependentInstance()
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class DependentTest
    {
        [TestMethod]
        public void TestGetSetPersonId()
        {
            var id = 10;
            var dependent = new TestDependent();
            dependent.SetPersonId(id);
            Assert.AreEqual(id, dependent.GetPersonId());
            Assert.AreEqual(id.ToString(), dependent.UserDefinedB);
        }

        [TestMethod]
        public void TestGetPersonId_NoValueSet()
        {
            var dependent = new TestDependent();
            Assert.IsNull(dependent.UserDefinedB);
            Assert.IsNull(dependent.GetPersonId());
        }

        [TestMethod]
        public void TestGetSetParticipantIdId()
        {
            var id = 10;
            var dependent = new TestDependent();
            dependent.SetParticipantId(id);
            Assert.AreEqual(id, dependent.GetParticipantId());
            Assert.AreEqual(id.ToString(), dependent.UserDefinedA);
        }

        [TestMethod]
        public void TestGetparticipantId_NoValueSet()
        {
            var dependent = new TestDependent();
            Assert.IsNull(dependent.UserDefinedB);
            Assert.IsNull(dependent.GetPersonId());
        }
    }
}
