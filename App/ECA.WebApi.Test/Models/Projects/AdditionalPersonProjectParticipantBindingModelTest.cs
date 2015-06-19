using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Projects;
using ECA.Business.Service;
using ECA.Data;

namespace ECA.WebApi.Test.Models.Projects
{
    [TestClass]
    public class AdditionalPersonProjectParticipantBindingModelTest
    {
        [TestMethod]
        public void TestToAdditionalPersonProjectParticipant()
        {
            var model = new AdditionalPersonProjectParticipantBindingModel();
            model.PersonId = 1;
            model.ProjectId = 2;
            model.ParticipantTypeId = ParticipantType.Individual.Id;
            var user = new User(10);
            var businessEntity = model.ToAdditionalPersonProjectParticipant(user);
            Assert.AreEqual(model.PersonId, businessEntity.PersonId);
            Assert.AreEqual(model.ProjectId, businessEntity.ProjectId);
            Assert.AreEqual(model.ParticipantTypeId, businessEntity.ParticipantTypeId);
            Assert.IsTrue(Object.ReferenceEquals(user, businessEntity.Audit.User));
        }
    }
}
