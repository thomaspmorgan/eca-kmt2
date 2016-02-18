using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Business.Service;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class PersonSevisServiceValidatorTest
    {
        private TestEcaContext context;
        private PersonSevisServiceValidator validator;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            validator = new PersonSevisServiceValidator(context);
        }

        #region ValidateSevisCreateEV

        [TestMethod]
        public async Task TestValidateSevisCreateEV_ParticipantDoesNotExist()
        {
            var participantId = 1;
            var projectId = 1;
            var user = new User(1);
            var message = String.Format("The model type [{0}] with Id [{1}] was not found.", typeof(Participant).Name, participantId);

            Assert.AreEqual(0, context.Participants.Count());
            Action a = () => validator.ValidateSevisCreateEV(projectId, participantId, user);
            Func<Task> f = () => validator.ValidateSevisCreateEVAsync(projectId, participantId, user);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestValidateSevisCreateEV_ParticipantDoesNotBelongToProject()
        {
            var participantId = 1;
            var projectId = 1;
            var participant = new Participant
            {
                ParticipantId = participantId,
                ProjectId = projectId
            };

            var user = new User(1);
            var message = String.Format("The user with id [{0}] attempted to validate a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        user.Id,
                        participant.ParticipantId,
                        projectId + 1);

            Assert.AreEqual(0, context.Participants.Count());
            context.Participants.Add(participant);
            Action a = () => validator.ValidateSevisCreateEV(projectId + 1, participantId, user);
            Func<Task> f = () => validator.ValidateSevisCreateEVAsync(projectId + 1, participantId, user);
            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        #endregion

        #region ValidateSevisUpdateEV

        [TestMethod]
        public async Task TestValidateSevisUpdateEV_ParticipantDoesNotExist()
        {
            var participantId = 1;
            var projectId = 1;
            var user = new User(1);
            var message = String.Format("The model type [{0}] with Id [{1}] was not found.", typeof(Participant).Name, participantId);

            Assert.AreEqual(0, context.Participants.Count());
            Action a = () => validator.ValidateSevisUpdateEV(projectId, participantId, user);
            Func<Task> f = () => validator.ValidateSevisUpdateEVAsync(projectId, participantId, user);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestValidateSevisUpdateEV_ParticipantDoesNotBelongToProject()
        {
            var participantId = 1;
            var projectId = 1;
            var participant = new Participant
            {
                ParticipantId = participantId,
                ProjectId = projectId
            };

            var user = new User(1);
            var message = String.Format("The user with id [{0}] attempted to validate a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        user.Id,
                        participant.ParticipantId,
                        projectId + 1);

            Assert.AreEqual(0, context.Participants.Count());
            context.Participants.Add(participant);
            Action a = () => validator.ValidateSevisUpdateEV(projectId + 1, participantId, user);
            Func<Task> f = () => validator.ValidateSevisUpdateEVAsync(projectId + 1, participantId, user);
            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        #endregion
    }
}
