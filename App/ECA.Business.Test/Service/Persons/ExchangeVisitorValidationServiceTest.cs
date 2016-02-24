using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using Moq;
using Microsoft.QualityTools.Testing.Fakes;
using ECA.Business.Queries.Models.Persons.ExchangeVisitor;
using System.Collections.Generic;
using ECA.Data;
using ECA.Business.Service;
using System.Threading.Tasks;
using ECA.Business.Validation.Model;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using ECA.Core.Exceptions;
using Newtonsoft.Json.Serialization;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ExchangeVisitorValidationServiceTest
    {
        private TestEcaContext context;
        private Mock<AbstractValidator<UpdateExchVisitor>> updateExchVisitorValidator;
        private Mock<AbstractValidator<CreateExchVisitor>> createExchVisitorValidator;
        private Mock<IExchangeVisitorService> exchangeVisitorService;
        private ExchangeVisitorValidationService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            updateExchVisitorValidator = new Mock<AbstractValidator<UpdateExchVisitor>>();
            createExchVisitorValidator = new Mock<AbstractValidator<CreateExchVisitor>>();
            exchangeVisitorService = new Mock<IExchangeVisitorService>();
            service = new ExchangeVisitorValidationService(
                context,
                exchangeVisitorService.Object,
                updateExchVisitorValidator.Object,
                createExchVisitorValidator.Object);
        }

        [TestMethod]
        public void TestConstructor_UseDefaultParameterValues()
        {
            var testService = new ExchangeVisitorValidationService(context, exchangeVisitorService.Object);
            Assert.IsNotNull(testService.UpdateExchangeVisitorValidator);
            Assert.IsNotNull(testService.CreateExchangeVisitorValidator);
            Assert.IsInstanceOfType(testService.UpdateExchangeVisitorValidator, typeof(UpdateExchVisitorValidator));
            Assert.IsInstanceOfType(testService.CreateExchangeVisitorValidator, typeof(CreateExchVisitorValidator));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantDoesNotExist()
        {

            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1
            };
            context.Projects.Add(project);
            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Participant).Name, 1);
            Action a = () => service.RunParticipantSevisValidation(user, project.ProjectId, 1);
            Func<Task> f = () => service.RunParticipantSevisValidationAsync(user, project.ProjectId, 1);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantDoesNotBelondToProject()
        {

            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId
            };
            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            var message = String.Format("The user with id [{0}] attempted to validate a participant with id [{1}] and project id [{2}] but should have been denied access.",
                        user.Id,
                        participant.ParticipantId,
                        project.ProjectId + 1);
            Action a = () => service.RunParticipantSevisValidation(user, project.ProjectId + 1, participant.ParticipantId);
            Func<Task> f = () => service.RunParticipantSevisValidationAsync(user, project.ProjectId + 1, participant.ParticipantId);
            a.ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantHasASevisId_ValidationSucceeds()
        {
            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234",
                SevisValidationResult = "some string"
            };

            var updateExchVisitor = new UpdateExchVisitor();
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(updateExchVisitor);
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(updateExchVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure>());
            updateExchVisitorValidator.Setup(x => x.Validate(It.IsAny<UpdateExchVisitor>())).Returns(validationResult);
            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });
            Action<ParticipantPersonSevisCommStatus> tester = (commStatus) =>
            {
                Assert.IsNotNull(commStatus);
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), commStatus));
                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.ReadyToSubmit.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.IsNull(participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            updateExchVisitorValidator.Verify(x => x.Validate(It.IsAny<UpdateExchVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            updateExchVisitorValidator.Verify(x => x.Validate(It.IsAny<UpdateExchVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantHasASevisId_ValidationFails()
        {

            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234",
                SevisValidationResult = "some string"
            };

            var updateExchVisitor = new UpdateExchVisitor();
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(updateExchVisitor);
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(updateExchVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("property", "error") });
            updateExchVisitorValidator.Setup(x => x.Validate(It.IsAny<UpdateExchVisitor>())).Returns(validationResult);
            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });
            Action<ParticipantPersonSevisCommStatus> tester = (commStatus) =>
            {
                Assert.IsNotNull(commStatus);
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), commStatus));
                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.InformationRequired.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(participantPerson.SevisValidationResult, JsonConvert.SerializeObject(
                    validationResult,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            updateExchVisitorValidator.Verify(x => x.Validate(It.IsAny<UpdateExchVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            updateExchVisitorValidator.Verify(x => x.Validate(It.IsAny<UpdateExchVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantDoesNotHaveASevisId_ValidationSucceeds()
        {
            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string"
            };

            var updateExchVisitor = new UpdateExchVisitor();
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(updateExchVisitor);
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(updateExchVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure>());
            createExchVisitorValidator.Setup(x => x.Validate(It.IsAny<CreateExchVisitor>())).Returns(validationResult);
            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });
            Action<ParticipantPersonSevisCommStatus> tester = (commStatus) =>
            {
                Assert.IsNotNull(commStatus);
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), commStatus));
                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.ReadyToSubmit.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.IsNull(participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantDoesNotHaveASevisId_ValidationFails()
        {

            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string"
            };
            var updateExchVisitor = new UpdateExchVisitor();
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(updateExchVisitor);
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(updateExchVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("property", "error") });
            createExchVisitorValidator.Setup(x => x.Validate(It.IsAny<CreateExchVisitor>())).Returns(validationResult);
            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });
            Action<ParticipantPersonSevisCommStatus> tester = (commStatus) =>
            {
                Assert.IsNotNull(commStatus);
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), commStatus));
                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.InformationRequired.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(participantPerson.SevisValidationResult, JsonConvert.SerializeObject(
                    validationResult,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_SevisIdIsNull()
        {
            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                SevisId = null
            };

            var updateExchVisitor = new UpdateExchVisitor();
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(updateExchVisitor);
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(updateExchVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure>());
            createExchVisitorValidator.Setup(x => x.Validate(It.IsAny<CreateExchVisitor>())).Returns(validationResult);
            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });
            Action<ParticipantPersonSevisCommStatus> tester = (commStatus) =>
            {
                Assert.IsNotNull(commStatus);
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), commStatus));
                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.ReadyToSubmit.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.IsNull(participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_SevisIdIsWhitespace()
        {

            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                SevisId = " "
            };
            var updateExchVisitor = new UpdateExchVisitor();
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(updateExchVisitor);
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(updateExchVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure>());
            createExchVisitorValidator.Setup(x => x.Validate(It.IsAny<CreateExchVisitor>())).Returns(validationResult);
            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });
            Action<ParticipantPersonSevisCommStatus> tester = (commStatus) =>
            {
                Assert.IsNotNull(commStatus);
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), commStatus));
                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.ReadyToSubmit.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.IsNull(participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Exactly(2));
        }
        [TestMethod]
        public async Task TestRunParticipantSevisValidation_SevisIdIsEmpty()
        {
            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                SevisId = String.Empty
            };
            var updateExchVisitor = new UpdateExchVisitor();
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(updateExchVisitor);
            exchangeVisitorService.Setup(x => x.GetUpdateExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(updateExchVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure>());
            createExchVisitorValidator.Setup(x => x.Validate(It.IsAny<CreateExchVisitor>())).Returns(validationResult);
            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });
            Action<ParticipantPersonSevisCommStatus> tester = (commStatus) =>
            {
                Assert.IsNotNull(commStatus);
                Assert.AreEqual(1, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.First(), commStatus));
                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.ReadyToSubmit.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.IsNull(participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Exactly(2));
        }
    }
}

