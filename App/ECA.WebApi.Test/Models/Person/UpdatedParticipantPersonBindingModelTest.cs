using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Person;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Person
{
    [TestClass]
    public class UpdatedParticipantPersonBindingModelTest
    {
        [TestMethod]
        public void TestToUpdatedParticipantPerson()
        {
            var model = new UpdatedParticipantPersonBindingModel();
            model.HomeInstitutionAddressId = 100;
            model.HomeInstitutionId = 20;
            model.HostInstitutionAddressId = 33;
            model.HostInstitutionId = 40;
            model.ParticipantId = 50;
            model.ParticipantStatusId = ParticipantStatus.Active.Id;
            model.ParticipantTypeId = ParticipantType.ForeignGovernment.Id;

            var user = new User(1);
            var projectId = 1000;
            var instance = model.ToUpdatedParticipantPerson(user, projectId);
            Assert.AreEqual(model.HomeInstitutionAddressId, instance.HomeInstitutionAddressId);
            Assert.AreEqual(model.HomeInstitutionId, instance.HomeInstitutionId);
            Assert.AreEqual(model.HostInstitutionAddressId, instance.HostInstitutionAddressId);
            Assert.AreEqual(model.HostInstitutionId, instance.HostInstitutionId);
            Assert.AreEqual(model.ParticipantId, instance.ParticipantId);
            Assert.AreEqual(model.ParticipantStatusId, instance.ParticipantStatusId);
            Assert.AreEqual(model.ParticipantTypeId, instance.ParticipantTypeId);
        }
    }
}
