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
        public async Task TestUpdateParticipantSevisValidationAsync_ParticipantIsNotValidatable()
        {
            using (ShimsContext.Create())
            {
                ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetValidatableParticipantsQueryEcaContext = (ctx) =>
                {
                    return new List<ValidatableExchangeVisitorParticipantDTO>().AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<ValidatableExchangeVisitorParticipantDTO>((src) =>
                {

                    return Task<ValidatableExchangeVisitorParticipantDTO>.FromResult(src.FirstOrDefault());
                });
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
                };
                context.Revert();
                var result = service.UpdateParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
                tester(result);

                context.Revert();
                result = await service.UpdateParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
                tester(result);
            }
        }

        [TestMethod]
        public async Task TestUpdateParticipantSevisValidationAsync_ParticipantIsValidatableAndHasASevisId_ValidationSucceeds()
        {
            using (ShimsContext.Create())
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
                ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetValidatableParticipantsQueryEcaContext = (ctx) =>
                {
                    var list = new List<ValidatableExchangeVisitorParticipantDTO>();
                    list.Add(new ValidatableExchangeVisitorParticipantDTO
                    {
                        ParticipantId = participant.ParticipantId,
                        PersonId = 1,
                        ProjectId = project.ProjectId
                    });
                    return list.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<ValidatableExchangeVisitorParticipantDTO>((src) =>
                {
                    return Task<ValidatableExchangeVisitorParticipantDTO>.FromResult(src.FirstOrDefault());
                });

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
                var result = service.UpdateParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
                tester(result);
                updateExchVisitorValidator.Verify(x => x.Validate(It.IsAny<UpdateExchVisitor>()), Times.Once());

                context.Revert();
                result = await service.UpdateParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
                tester(result);
                updateExchVisitorValidator.Verify(x => x.Validate(It.IsAny<UpdateExchVisitor>()), Times.Exactly(2));
            }
        }

        [TestMethod]
        public async Task TestUpdateParticipantSevisValidationAsync_ParticipantIsValidatableHasASevisId_ValidationFails()
        {
            using (ShimsContext.Create())
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
                ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetValidatableParticipantsQueryEcaContext = (ctx) =>
                {
                    var list = new List<ValidatableExchangeVisitorParticipantDTO>();
                    list.Add(new ValidatableExchangeVisitorParticipantDTO
                    {
                        ParticipantId = participant.ParticipantId,
                        PersonId = 1,
                        ProjectId = project.ProjectId
                    });
                    return list.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<ValidatableExchangeVisitorParticipantDTO>((src) =>
                {
                    return Task<ValidatableExchangeVisitorParticipantDTO>.FromResult(src.FirstOrDefault());
                });

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

                    Assert.AreEqual(participantPerson.SevisValidationResult, JsonConvert.SerializeObject(validationResult));
                };
                context.Revert();
                var result = service.UpdateParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
                tester(result);
                updateExchVisitorValidator.Verify(x => x.Validate(It.IsAny<UpdateExchVisitor>()), Times.Once());

                context.Revert();
                result = await service.UpdateParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
                tester(result);
                updateExchVisitorValidator.Verify(x => x.Validate(It.IsAny<UpdateExchVisitor>()), Times.Exactly(2));
            }
        }

        [TestMethod]
        public async Task TestUpdateParticipantSevisValidationAsync_ParticipantIsValidatableAndDoesNotHaveASevisId_ValidationSucceeds()
        {
            using (ShimsContext.Create())
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
                ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetValidatableParticipantsQueryEcaContext = (ctx) =>
                {
                    var list = new List<ValidatableExchangeVisitorParticipantDTO>();
                    list.Add(new ValidatableExchangeVisitorParticipantDTO
                    {
                        ParticipantId = participant.ParticipantId,
                        PersonId = 1,
                        ProjectId = project.ProjectId
                    });
                    return list.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<ValidatableExchangeVisitorParticipantDTO>((src) =>
                {
                    return Task<ValidatableExchangeVisitorParticipantDTO>.FromResult(src.FirstOrDefault());
                });

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
                var result = service.UpdateParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
                tester(result);
                createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Once());

                context.Revert();
                result = await service.UpdateParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
                tester(result);
                createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Exactly(2));
            }
        }

        [TestMethod]
        public async Task TestUpdateParticipantSevisValidationAsync_ParticipantIsValidatableAndDoesNotHaveASevisId_ValidationFails()
        {
            using (ShimsContext.Create())
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
                ECA.Business.Queries.Persons.Fakes.ShimExchangeVisitorQueries.CreateGetValidatableParticipantsQueryEcaContext = (ctx) =>
                {
                    var list = new List<ValidatableExchangeVisitorParticipantDTO>();
                    list.Add(new ValidatableExchangeVisitorParticipantDTO
                    {
                        ParticipantId = participant.ParticipantId,
                        PersonId = 1,
                        ProjectId = project.ProjectId
                    });
                    return list.AsQueryable();
                };
                System.Data.Entity.Fakes.ShimQueryableExtensions.FirstOrDefaultAsyncOf1IQueryableOfM0<ValidatableExchangeVisitorParticipantDTO>((src) =>
                {
                    return Task<ValidatableExchangeVisitorParticipantDTO>.FromResult(src.FirstOrDefault());
                });

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

                    Assert.AreEqual(participantPerson.SevisValidationResult, JsonConvert.SerializeObject(validationResult));
                };
                context.Revert();
                var result = service.UpdateParticipantSevisValidation(user, project.ProjectId, participant.ParticipantId);
                tester(result);
                createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Once());

                context.Revert();
                result = await service.UpdateParticipantSevisValidationAsync(user, project.ProjectId, participant.ParticipantId);
                tester(result);
                createExchVisitorValidator.Verify(x => x.Validate(It.IsAny<CreateExchVisitor>()), Times.Exactly(2));
            }
        }
    }
}
