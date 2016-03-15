using ECA.Business.Service;
using ECA.Business.Service.Persons;
using ECA.Business.Validation;
using ECA.Business.Validation.Sevis;
using ECA.Core.Exceptions;
using ECA.Data;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ExchangeVisitorValidationServiceTest
    {
        private TestEcaContext context;
        private Mock<AbstractValidator<ExchangeVisitor>> exchangeVisitorValidator;
        private Mock<IExchangeVisitorService> exchangeVisitorService;
        private ExchangeVisitorValidationService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            exchangeVisitorValidator = new Mock<AbstractValidator<ExchangeVisitor>>();
            exchangeVisitorService = new Mock<IExchangeVisitorService>();
            service = new ExchangeVisitorValidationService(
                context,
                exchangeVisitorService.Object,
                exchangeVisitorValidator.Object);
        }

        [TestMethod]
        public void TestConstructor_UseDefaultParameterValues()
        {
            var testService = new ExchangeVisitorValidationService(context, exchangeVisitorService.Object);
            Assert.IsNotNull(testService.ExchangeVisitorValidator);
            Assert.IsInstanceOfType(testService.ExchangeVisitorValidator, typeof(ExchangeVisitorValidator));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantDoesNotExist()
        {

            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
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
                Project = project,
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id
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
        public async Task TestRunParticipantSevisValidation_ParticipantTypeIsNotForeignTravelingParticipant()
        {
            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234",
                SevisValidationResult = "some string"
            };

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
				sevisId: null,
                person: null,
				financialInfo: null,
				occupationCategoryCode: null,
				programEndDate: DateTime.UtcNow,
				programStartDate: DateTime.UtcNow,
				siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
				dependents: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure>());
            exchangeVisitorValidator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(validationResult);
            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });
            Action<ParticipantPersonSevisCommStatus> tester = (commStatus) =>
            {
                Assert.IsNull(commStatus);
                Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Never());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Never());
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ProjectIsNotExchangeVisitor()
        {
            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.NotApplicable.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project,
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234",
                SevisValidationResult = "some string"
            };

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure>());
            exchangeVisitorValidator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(validationResult);
            context.SetupActions.Add(() =>
            {
                context.Projects.Add(project);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });
            Action<ParticipantPersonSevisCommStatus> tester = (commStatus) =>
            {
                Assert.IsNull(commStatus);
                Assert.AreEqual(0, context.ParticipantPersonSevisCommStatuses.Count());
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Never());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Never());
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ValidationSucceeds()
        {
            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project,
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234",
                SevisValidationResult = "some string"
            };

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure>());
            exchangeVisitorValidator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(validationResult);
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
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ValidationFails()
        {

            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project,
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234",
                SevisValidationResult = "some string"
            };

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("property", "error") });
            exchangeVisitorValidator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(validationResult);
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
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }
        

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_DoesNotHaveACommStatus_ValidationFails()
        {

            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project,
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string"
            };
            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("property", "error") });
            exchangeVisitorValidator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(validationResult);
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
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_DoesHaveAnInformationRequiredCommStatus_ValidationFails()
        {

            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project,
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string"
            };
            var existingCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisCommStatusId = SevisCommStatus.InformationRequired.Id

            };
            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("property", "error") });
            exchangeVisitorValidator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(validationResult);
            context.SetupActions.Add(() =>
            {
                context.ParticipantPersonSevisCommStatuses.Add(existingCommStatus);
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
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_DoesNotHaveAnInformationRequiredCommStatus_ValidationFails()
        {
            var yesterday = DateTime.UtcNow.AddDays(-1.0);
            var user = new User(1);
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                ProjectId = project.ProjectId,
                Project = project,
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string"
            };
            var existingCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                AddedOn = yesterday,
                SevisCommStatusId = SevisCommStatus.ReadyToSubmit.Id

            };
            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            var validationResult = new ValidationResult(new List<ValidationFailure> { new ValidationFailure("property", "error") });
            exchangeVisitorValidator.Setup(x => x.Validate(It.IsAny<ExchangeVisitor>())).Returns(validationResult);
            context.SetupActions.Add(() =>
            {
                context.ParticipantPersonSevisCommStatuses.Add(existingCommStatus);
                context.Projects.Add(project);
                context.Participants.Add(participant);
                context.ParticipantPersons.Add(participantPerson);
            });
            Action<ParticipantPersonSevisCommStatus> tester = (commStatus) =>
            {
                Assert.IsNotNull(commStatus);
                Assert.AreEqual(2, context.ParticipantPersonSevisCommStatuses.Count());
                Assert.IsTrue(Object.ReferenceEquals(existingCommStatus, context.ParticipantPersonSevisCommStatuses.First()));
                Assert.IsTrue(Object.ReferenceEquals(commStatus, context.ParticipantPersonSevisCommStatuses.Last()));
                Assert.AreEqual(yesterday, existingCommStatus.AddedOn);
                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.InformationRequired.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(participantPerson.SevisValidationResult, JsonConvert.SerializeObject(
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }
        
    }
}

