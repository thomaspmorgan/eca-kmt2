using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    public class TestDependent : Dependent
    {
        public TestDependent() 
            : base(
                  fullName: null,
                  birthCity: null,
                  birthCountryCode: null,
                  birthCountryReason: null,
                  birthDate: null,
                  citizenshipCountryCode: null,
                  emailAddress: null,
                  genderCode: null,
                  permanentResidenceCountryCode: null,
                  phoneNumber: null,
                  relationship: null,
                  mailAddress: null,
                  usAddress: null,
                  printForm: true,
                  personId: 10,
                  participantId:20
                  )
        {

        }

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
            var id = 100;
            var dependent = new TestDependent();
            dependent.SetPersonId(id);
            Assert.AreEqual(id, dependent.GetPersonId());
            Assert.AreEqual(id.ToString(), dependent.UserDefinedB);
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
    }
}
