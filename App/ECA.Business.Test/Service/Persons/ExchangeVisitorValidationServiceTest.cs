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
        private Mock<IParticipantPersonsSevisService> participantPersonSevisService;
        private ExchangeVisitorValidationService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            exchangeVisitorValidator = new Mock<AbstractValidator<ExchangeVisitor>>();
            exchangeVisitorService = new Mock<IExchangeVisitorService>();
            participantPersonSevisService = new Mock<IParticipantPersonsSevisService>();
            service = new ExchangeVisitorValidationService(
                context,
                exchangeVisitorService.Object,
                participantPersonSevisService.Object,
                exchangeVisitorValidator.Object);
        }

        [TestMethod]
        public void TestConstructor_UseDefaultParameterValues()
        {
            var testService = new ExchangeVisitorValidationService(context, exchangeVisitorService.Object, participantPersonSevisService.Object);
            Assert.IsNotNull(testService.ExchangeVisitorValidator);
            Assert.IsInstanceOfType(testService.ExchangeVisitorValidator, typeof(ExchangeVisitorValidator));
        }

        [TestMethod]
        public void TestGetValidator()
        {
            Assert.IsTrue(Object.ReferenceEquals(exchangeVisitorValidator.Object, service.GetValidator()));
        }

        #region RunParticipantSevisValiation
        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1,
                VisitorTypeId = VisitorType.ExchangeVisitor.Id
            };
            context.Projects.Add(project);
            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Participant).Name, 1);
            Action a = () => service.RunParticipantSevisValidation(project.ProjectId, 1);
            Func<Task> f = () => service.RunParticipantSevisValidationAsync(project.ProjectId, 1);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantTypeIsNotForeignTravelingParticipant()
        {
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
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

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
                Assert.IsNull(participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Never());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Never());
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantStatusIsNotInListOfTrackedParticipantStatuses()
        {
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
                ParticipantTypeId = ParticipantType.Individual.Id,
                ParticipantStatusId = ParticipantStatus.NoFunds.Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234",
                SevisValidationResult = "some string"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

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
                Assert.IsNull(participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Never());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Never());
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantDoesNotHaveParticipantStatus()
        {
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
                ParticipantTypeId = ParticipantType.Individual.Id,
                ParticipantStatusId = null
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisId = "N1234",
                SevisValidationResult = "some string"
            };

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

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
                Assert.IsNull(participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Never());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Never());
        }
        
        
        
        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantHasNullSevisId_ParticipantDoesNotHaveAnyStatuses_ValidationFails()
        {
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
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id,
                ParticipantStatusId = ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES.First().Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

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
                Assert.IsTrue(Object.ReferenceEquals(commStatus, context.ParticipantPersonSevisCommStatuses.First()));

                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.InformationRequired.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(JsonConvert.SerializeObject(
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantHasEmptySevisId_ParticipantDoesNotHaveAnyStatuses_ValidationFails()
        {
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
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id,
                ParticipantStatusId = ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES.First().Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: String.Empty,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

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
                Assert.IsTrue(Object.ReferenceEquals(commStatus, context.ParticipantPersonSevisCommStatuses.First()));

                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.InformationRequired.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(JsonConvert.SerializeObject(
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantHasWhitespaceSevisId_ParticipantDoesNotHaveAnyStatuses_ValidationFails()
        {
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
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id,
                ParticipantStatusId = ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES.First().Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: " ",
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

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
                Assert.IsTrue(Object.ReferenceEquals(commStatus, context.ParticipantPersonSevisCommStatuses.First()));

                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.InformationRequired.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(JsonConvert.SerializeObject(
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }



        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantIsNotReadyToValidate_ValidationFails()
        {
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
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id,
                ParticipantStatusId = ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES.First().Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                SevisId = "sevisId",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            var existingCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisCommStatusId = SevisCommStatus.CreatedByBatch.Id
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidate(It.IsAny<int>())).Returns(false);
            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidateAsync(It.IsAny<int>())).ReturnsAsync(false);

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
                Assert.IsFalse(Object.ReferenceEquals(existingCommStatus, commStatus));
                Assert.IsTrue(Object.ReferenceEquals(existingCommStatus, context.ParticipantPersonSevisCommStatuses.First()));
                Assert.IsTrue(Object.ReferenceEquals(commStatus, context.ParticipantPersonSevisCommStatuses.Last()));

                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.InformationRequired.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(JsonConvert.SerializeObject(
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantIsReadyToValidate_ValidationFails()
        {
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
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id,
                ParticipantStatusId = ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES.First().Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                SevisId = "sevisId",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            var existingCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisCommStatusId = SevisCommStatus.CreatedByBatch.Id
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidate(It.IsAny<int>())).Returns(true);
            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidateAsync(It.IsAny<int>())).ReturnsAsync(true);

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
                Assert.IsFalse(Object.ReferenceEquals(existingCommStatus, commStatus));
                Assert.IsTrue(Object.ReferenceEquals(existingCommStatus, context.ParticipantPersonSevisCommStatuses.First()));
                Assert.IsTrue(Object.ReferenceEquals(commStatus, context.ParticipantPersonSevisCommStatuses.Last()));

                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.NeedsValidationInfo.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(JsonConvert.SerializeObject(
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }        

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantDoesNotHaveAnyStatuses_ValidationSucceeds()
        {
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
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id,
                ParticipantStatusId = ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES.First().Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                SevisId = "sevisId",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidate(It.IsAny<int>())).Returns(false);
            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidateAsync(It.IsAny<int>())).ReturnsAsync(false);

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
                Assert.IsTrue(Object.ReferenceEquals(commStatus, context.ParticipantPersonSevisCommStatuses.First()));

                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.ReadyToSubmit.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(JsonConvert.SerializeObject(
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }


        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantDoesNotHaveSevisId_ValidationFails_CheckUsesLatestCommStatus()
        {
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
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id,
                ParticipantStatusId = ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES.First().Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                SevisId = null,
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            var existingCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisCommStatusId = SevisCommStatus.InformationRequired.Id
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidate(It.IsAny<int>())).Returns(false);
            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidateAsync(It.IsAny<int>())).ReturnsAsync(false);

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
                Assert.IsTrue(Object.ReferenceEquals(existingCommStatus, commStatus));

                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.InformationRequired.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(JsonConvert.SerializeObject(
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }


        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantIsNotReadyToValidate_ValidationFails_CheckUsesLatestCommStatus()
        {
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
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id,
                ParticipantStatusId = ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES.First().Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                SevisId = "sevisId",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            var existingCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisCommStatusId = SevisCommStatus.InformationRequired.Id
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidate(It.IsAny<int>())).Returns(false);
            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidateAsync(It.IsAny<int>())).ReturnsAsync(false);

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
                Assert.IsTrue(Object.ReferenceEquals(existingCommStatus, commStatus));

                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.InformationRequired.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(JsonConvert.SerializeObject(
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantIsReadyToValidate_ValidationFails_CheckUsesLatestCommStatus()
        {
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
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id,
                ParticipantStatusId = ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES.First().Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                SevisId = "sevisId",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            var existingCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisCommStatusId = SevisCommStatus.NeedsValidationInfo.Id
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidate(It.IsAny<int>())).Returns(true);
            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidateAsync(It.IsAny<int>())).ReturnsAsync(true);

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
                Assert.IsTrue(Object.ReferenceEquals(existingCommStatus, commStatus));

                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.NeedsValidationInfo.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(JsonConvert.SerializeObject(
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }



        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantIsNotReadyToBeValidated_ValidationSucceeds()
        {
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
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id,
                ParticipantStatusId = ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES.First().Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                SevisId = "sevisId",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            var existingCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisCommStatusId = SevisCommStatus.InformationRequired.Id
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);

            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidate(It.IsAny<int>())).Returns(false);
            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidateAsync(It.IsAny<int>())).ReturnsAsync(false);

            var validationResult = new ValidationResult(new List<ValidationFailure>());
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
                Assert.IsFalse(Object.ReferenceEquals(existingCommStatus, commStatus));

                Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.Last(), commStatus));
                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.ReadyToSubmit.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(JsonConvert.SerializeObject(
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestRunParticipantSevisValidation_ParticipantIsReadyToBeValidated_ValidationSucceeds()
        {
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
                ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id,
                ParticipantStatusId = ParticipantStatus.EXCHANGE_VISITOR_VALIDATION_PARTICIPANT_STATUSES.First().Id
            };
            var participantPerson = new ParticipantPerson
            {
                Participant = participant,
                ParticipantId = participant.ParticipantId,
                SevisValidationResult = "some string",
                SevisId = "sevisId",
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0)
            };
            var existingCommStatus = new ParticipantPersonSevisCommStatus
            {
                ParticipantId = participant.ParticipantId,
                AddedOn = DateTime.UtcNow.AddDays(-1.0),
                SevisCommStatusId = SevisCommStatus.NeedsValidationInfo.Id
            };
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: null,
                person: null,
                financialInfo: null,
                occupationCategoryCode: null,
                programEndDate: DateTime.UtcNow,
                programStartDate: DateTime.UtcNow,
                siteOfActivity: new Business.Queries.Models.Admin.AddressDTO(),
                dependents: null,
                sevisOrgId: null
                );
            exchangeVisitorService.Setup(x => x.GetExchangeVisitor(It.IsAny<int>(), It.IsAny<int>())).Returns(exchangeVisitor);
            exchangeVisitorService.Setup(x => x.GetExchangeVisitorAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(exchangeVisitor);
            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidate(It.IsAny<int>())).Returns(true);
            participantPersonSevisService.Setup(x => x.IsParticipantReadyToValidateAsync(It.IsAny<int>())).ReturnsAsync(true);
            var validationResult = new ValidationResult(new List<ValidationFailure>());
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
                Assert.IsFalse(Object.ReferenceEquals(existingCommStatus, commStatus));

                Assert.IsTrue(Object.ReferenceEquals(context.ParticipantPersonSevisCommStatuses.Last(), commStatus));
                Assert.AreEqual(participant.ParticipantId, commStatus.ParticipantId);
                Assert.AreEqual(SevisCommStatus.ReadyToValidate.Id, commStatus.SevisCommStatusId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(commStatus.AddedOn, 20000);

                Assert.AreEqual(JsonConvert.SerializeObject(
                    new SimpleValidationResult(validationResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }), participantPerson.SevisValidationResult);
            };
            context.Revert();
            var result = service.RunParticipantSevisValidation(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Once());

            context.Revert();
            result = await service.RunParticipantSevisValidationAsync(project.ProjectId, participant.ParticipantId);
            tester(result);
            exchangeVisitorValidator.Verify(x => x.Validate(It.IsAny<ExchangeVisitor>()), Times.Exactly(2));
        }

        #endregion

        #region ShouldRunValidation

        [TestMethod]
        public void TestShouldRunValidation_ParticipantTypeIsNotForeignTravelingParticipant()
        {
            int? participantStatusId = ParticipantStatus.Active.Id;
            var participantTypeId = ParticipantType.ForeignTravelingParticipant.Id;
            Func<Participant> getParticipant = () =>
            {
                return new Participant
                {
                    ParticipantStatusId = participantStatusId,
                    ParticipantTypeId = participantTypeId
                };
            };

            Assert.IsTrue(service.ShouldRunValidation(getParticipant()));
            participantTypeId = ParticipantType.LanguageOfficer.Id;
            Assert.IsFalse(service.ShouldRunValidation(getParticipant()));
        }

        [TestMethod]
        public void TestShouldRunValidation_ParticipantStatusIsNotSetOnParticipant()
        {
            int? participantStatusId = ParticipantStatus.Active.Id;
            var participantTypeId = ParticipantType.ForeignTravelingParticipant.Id;
            Func<Participant> getParticipant = () =>
            {
                return new Participant
                {
                    ParticipantStatusId = participantStatusId,
                    ParticipantTypeId = participantTypeId
                };
            };

            Assert.IsTrue(service.ShouldRunValidation(getParticipant()));
            participantStatusId = ParticipantStatus.Cancelled.Id;
            Assert.IsFalse(service.ShouldRunValidation(getParticipant()));
        }

        [TestMethod]
        public void TestShouldRunValidation_ParticipantStatusIsNull()
        {
            int? participantStatusId = ParticipantStatus.Active.Id;
            var participantTypeId = ParticipantType.ForeignTravelingParticipant.Id;
            Func<Participant> getParticipant = () =>
            {
                return new Participant
                {
                    ParticipantStatusId = participantStatusId,
                    ParticipantTypeId = participantTypeId
                };
            };

            Assert.IsTrue(service.ShouldRunValidation(getParticipant()));
            participantStatusId = null;
            Assert.IsFalse(service.ShouldRunValidation(getParticipant()));
        }

        #endregion
    }
}

