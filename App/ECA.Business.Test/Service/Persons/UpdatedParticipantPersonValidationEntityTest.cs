using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Service.Persons;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class UpdatedParticipantPersonValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var participantType = new ParticipantType();
            var instance = new UpdatedParticipantPersonValidationEntity(participantType);
            Assert.IsTrue(object.ReferenceEquals(participantType, instance.ParticipantType));
        }
    }
}
