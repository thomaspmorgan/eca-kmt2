using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Business.Service;
using ECA.Business.Validation;
using Moq;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class ParticipantPersonServiceTest
    {
        private TestEcaContext context;
        private ParticipantPersonService service;
        private Mock<IBusinessValidator<Object, UpdatedParticipantPersonValidationEntity>> validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new Mock<IBusinessValidator<object, UpdatedParticipantPersonValidationEntity>>();
            context = new TestEcaContext();
            service = new ParticipantPersonService(context, validator.Object);
        }

        #region Get

        [TestMethod]
        public async Task TestGetParticipantPersons_CheckProperties()
        {
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = "N0000000001",
                StudyProject = "studyProject",
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = participantPerson.ParticipantId,
                ProjectId = project.ProjectId,
                Project = project
            };
            participantPerson.Participant = participant;
            project.Participants.Add(participant);

            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            Action<PagedQueryResults<SimpleParticipantPersonDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantPersonResult = results.Results.First();

                Assert.AreEqual(participantPerson.SevisId, participantPersonResult.SevisId);
                Assert.AreEqual(participantPerson.StudyProject, participantPersonResult.StudyProject);
                Assert.AreEqual(project.ProjectId, participantPersonResult.ProjectId);

                Assert.IsNull(participantPersonResult.FieldOfStudy);
                Assert.IsNull(participantPersonResult.ProgramCategory);
                Assert.IsNull(participantPersonResult.Position);
                Assert.IsNull(participantPersonResult.HostInstitution);
                Assert.IsNull(participantPersonResult.HomeInstitution);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantPersonDTO>(x => x.RevisedOn, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantPersonDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipantPersons(queryOperator);
            var serviceResultsAsync = await service.GetParticipantPersonsAsync(queryOperator);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantPersons_Empty()
        {
            Action<PagedQueryResults<SimpleParticipantPersonDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Total);
                Assert.AreEqual(0, results.Results.Count);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantPersonDTO>(x => x.RevisedOn, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantPersonDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetParticipantPersons(queryOperator);
            var serviceResultsAsync = await service.GetParticipantPersonsAsync(queryOperator);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantPersonsByProjectId_CheckProperties()
        {
            var project = new Project
            {
                ProjectId = 1
            };

            var participant = new Participant
            {
                ParticipantId = 1,
                Project = project,
                ProjectId = project.ProjectId
            };

            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = "N0000000001",
                StudyProject = "studyProject",
            };

            participantPerson.Participant = participant;
            project.Participants.Add(participant);
            participant.Project = project;
            participant.ProjectId = project.ProjectId;

            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);
            context.Projects.Add(project);

            Action<PagedQueryResults<SimpleParticipantPersonDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var participantPersonResult = results.Results.First();

                Assert.AreEqual(project.ProjectId, participantPersonResult.ProjectId);
                Assert.AreEqual(participantPerson.SevisId, participantPersonResult.SevisId);
                Assert.AreEqual(participantPerson.StudyProject, participantPersonResult.StudyProject);

                Assert.IsNull(participantPersonResult.FieldOfStudy);
                Assert.IsNull(participantPersonResult.ProgramCategory);
                Assert.IsNull(participantPersonResult.Position);
                Assert.IsNull(participantPersonResult.HostInstitution);
                Assert.IsNull(participantPersonResult.HomeInstitution);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantPersonDTO>(x => x.RevisedOn, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantPersonDTO>(0, 1, defaultSorter);


            var serviceResults = service.GetParticipantPersonsByProjectId(1, queryOperator);
            var serviceResultsAsync = await service.GetParticipantPersonsByProjectIdAsync(1, queryOperator);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantPersonsByProjectId_Empty()
        {
            var project = new Project
            {
                ProjectId = 1
            };

            context.Projects.Add(project);

            Action<PagedQueryResults<SimpleParticipantPersonDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Total);
                Assert.AreEqual(0, results.Results.Count);
            };

            var defaultSorter = new ExpressionSorter<SimpleParticipantPersonDTO>(x => x.RevisedOn, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleParticipantPersonDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetParticipantPersonsByProjectId(1, queryOperator);
            var serviceResultsAsync = await service.GetParticipantPersonsByProjectIdAsync(1, queryOperator);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantPersonDTOById_CheckProperties()
        {
            var participantPerson = new ParticipantPerson
            {
                ParticipantId = 1,
                SevisId = "N0000000001",
                StudyProject = "studyProject",
            };
            var project = new Project
            {
                ProjectId = 1
            };
            var participant = new Participant
            {
                ParticipantId = participantPerson.ParticipantId,
                ProjectId = project.ProjectId,
                Project = project
            };
            participantPerson.Participant = participant;
            project.Participants.Add(participant);

            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.ParticipantPersons.Add(participantPerson);

            Action<SimpleParticipantPersonDTO> tester = (results) =>
            {
                Assert.AreEqual(participantPerson.SevisId, results.SevisId);
                Assert.AreEqual(participantPerson.StudyProject, results.StudyProject);
                Assert.AreEqual(project.ProjectId, results.ProjectId);

                Assert.IsNull(results.FieldOfStudy);
                Assert.IsNull(results.ProgramCategory);
                Assert.IsNull(results.Position);
                Assert.IsNull(results.HostInstitution);
                Assert.IsNull(results.HomeInstitution);
            };

            var serviceResults = service.GetParticipantPersonById(1);
            var serviceResultsAsync = await service.GetParticipantPersonByIdAsync(1);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetParticipantPersonDTOById_Empty()
        {
            Action<SimpleParticipantPersonDTO> tester = (results) =>
            {
                Assert.IsNull(results);
            };

            var serviceResults = service.GetParticipantPersonById(1);
            var serviceResultsAsync = await service.GetParticipantPersonByIdAsync(1);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        #endregion


        #region Update
        [TestMethod]
        public async Task TestCreateOrUpdate_ParticipantPersonDoesNotExists_CheckProperties()
        {
            var participantId = 1;
            Participant participant = null;
            ParticipantType individual = new ParticipantType
            {
                IsPerson = true,
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            ParticipantStatus status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id,
                Status = ParticipantStatus.Active.Value
            };
            Organization host = new Organization
            {
                OrganizationId = 1
            };
            Organization home = new Organization
            {
                OrganizationId = 2
            };
            var hostAddress = new Address
            {
                AddressId = 1,
                OrganizationId = host.OrganizationId,
                Organization = host
            };
            var homeAddress = new Address
            {
                AddressId = 2,
                Organization = home,
                OrganizationId = home.OrganizationId
            };
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var createrId = 1;
            var updaterId = 2;
            var updater = new User(updaterId);

            context.SetupActions.Add(() =>
            {
                participant = new Participant
                {
                    ParticipantId = participantId,
                };
                participant.History.CreatedBy = createrId;
                participant.History.RevisedBy = createrId;
                participant.History.CreatedOn = yesterday;
                participant.History.RevisedOn = yesterday;

                context.Participants.Add(participant);
                context.ParticipantStatuses.Add(status);
                context.ParticipantTypes.Add(individual);
                context.Organizations.Add(home);
                context.Organizations.Add(host);
                context.Addresses.Add(hostAddress);
                context.Addresses.Add(homeAddress);
            });

            var updatedPersonParticipant = new UpdatedParticipantPerson(
                updater: updater,
                homeInstitutionAddressId: homeAddress.AddressId,
                homeInstitutionId: home.OrganizationId,
                hostInstitutionAddressId: hostAddress.AddressId,
                hostInstitutionId: host.OrganizationId,
                participantId: participantId,
                participantStatusId: status.ParticipantStatusId,
                participantTypeId: individual.ParticipantTypeId
                );
            Action beforeUpdateTester = () =>
            {
                Assert.AreEqual(1, context.Participants.Count());
                Assert.AreEqual(0, context.ParticipantPersons.Count());
                Assert.AreEqual(2, context.Addresses.Count());
                Assert.AreEqual(2, context.Organizations.Count());
                Assert.AreEqual(yesterday, participant.History.RevisedOn);
                Assert.AreEqual(yesterday, participant.History.CreatedOn);
                Assert.AreEqual(createrId, participant.History.RevisedBy);
                Assert.AreEqual(createrId, participant.History.CreatedBy);

                Assert.IsFalse(participant.ParticipantStatusId.HasValue);
            };

            Action tester = () =>
            {
                Assert.AreEqual(1, context.Participants.Count());
                Assert.AreEqual(1, context.ParticipantPersons.Count());
                Assert.AreEqual(2, context.Addresses.Count());
                Assert.AreEqual(2, context.Organizations.Count());

                var addedParticipantPerson = context.ParticipantPersons.First();

                Assert.AreEqual(createrId, participant.History.CreatedBy);
                Assert.AreEqual(updaterId, participant.History.RevisedBy);
                Assert.AreEqual(yesterday, participant.History.CreatedOn);
                DateTimeOffset.UtcNow.Should().BeCloseTo(participant.History.RevisedOn, 20000);                

                Assert.AreEqual(updaterId, addedParticipantPerson.History.CreatedBy);
                Assert.AreEqual(updaterId, addedParticipantPerson.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedParticipantPerson.History.RevisedOn, 20000);
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedParticipantPerson.History.CreatedOn, 20000);                

                Assert.AreEqual(home.OrganizationId, addedParticipantPerson.HomeInstitutionId);
                Assert.AreEqual(host.OrganizationId, addedParticipantPerson.HostInstitutionId);
                Assert.AreEqual(homeAddress.AddressId, addedParticipantPerson.HomeInstitutionAddressId);
                Assert.AreEqual(hostAddress.AddressId, addedParticipantPerson.HostInstitutionAddressId);
            };

            Action<UpdatedParticipantPersonValidationEntity> validationEntityTester = (entity) =>
            {
                Assert.IsNotNull(entity);
                Assert.IsTrue(Object.ReferenceEquals(individual, entity.ParticipantType));
            };
            validator.Setup(x => x.ValidateUpdate(It.IsAny<UpdatedParticipantPersonValidationEntity>())).Callback(validationEntityTester);

            context.Revert();
            beforeUpdateTester();
            service.CreateOrUpdate(updatedPersonParticipant);
            tester();
            validator.Verify(x => x.ValidateUpdate(It.IsAny<UpdatedParticipantPersonValidationEntity>()), Times.Once());

            context.Revert();
            beforeUpdateTester();
            await service.CreateOrUpdateAsync(updatedPersonParticipant);
            tester();
            validator.Verify(x => x.ValidateUpdate(It.IsAny<UpdatedParticipantPersonValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestCreateOrUpdate_ParticipantPersonExists_CheckProperties()
        {
            var participantId = 1;
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            ParticipantType individual = new ParticipantType
            {
                IsPerson = true,
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            ParticipantStatus status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id,
                Status = ParticipantStatus.Active.Value
            };
            Organization host = new Organization
            {
                OrganizationId = 1
            };
            Organization home = new Organization
            {
                OrganizationId = 2
            };
            var hostAddress = new Address
            {
                AddressId = 1,
                OrganizationId = host.OrganizationId,
                Organization = host
            };
            var homeAddress = new Address
            {
                AddressId = 2,
                Organization = home,
                OrganizationId = home.OrganizationId
            };
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var createrId = 1;
            var updaterId = 2;
            var updater = new User(updaterId);

            context.SetupActions.Add(() =>
            {
                participant = new Participant
                {
                    ParticipantId = participantId,
                };
                participant.History.CreatedBy = createrId;
                participant.History.RevisedBy = createrId;
                participant.History.CreatedOn = yesterday;
                participant.History.RevisedOn = yesterday;

                participantPerson = new ParticipantPerson
                {
                    Participant = participant,
                    ParticipantId = participantId,
                };
                participantPerson.History.CreatedBy = createrId;
                participantPerson.History.RevisedBy = createrId;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedOn = yesterday;

                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantStatuses.Add(status);
                context.ParticipantTypes.Add(individual);
                context.Organizations.Add(home);
                context.Organizations.Add(host);
                context.Addresses.Add(hostAddress);
                context.Addresses.Add(homeAddress);
            });

            var updatedPersonParticipant = new UpdatedParticipantPerson(
                updater: updater,
                homeInstitutionAddressId: homeAddress.AddressId,
                homeInstitutionId: home.OrganizationId,
                hostInstitutionAddressId: hostAddress.AddressId,
                hostInstitutionId: host.OrganizationId,
                participantId: participantId,
                participantStatusId: status.ParticipantStatusId,
                participantTypeId: individual.ParticipantTypeId
                );
            Action beforeUpdateTester = () =>
            {
                Assert.AreEqual(1, context.Participants.Count());
                Assert.AreEqual(1, context.ParticipantPersons.Count());
                Assert.AreEqual(2, context.Addresses.Count());
                Assert.AreEqual(2, context.Organizations.Count());
                Assert.AreEqual(yesterday, participant.History.RevisedOn);
                Assert.AreEqual(yesterday, participant.History.CreatedOn);
                Assert.AreEqual(createrId, participant.History.RevisedBy);
                Assert.AreEqual(createrId, participant.History.CreatedBy);

                Assert.AreEqual(yesterday, participantPerson.History.RevisedOn);
                Assert.AreEqual(yesterday, participantPerson.History.CreatedOn);
                Assert.AreEqual(createrId, participantPerson.History.RevisedBy);
                Assert.AreEqual(createrId, participantPerson.History.CreatedBy);

                Assert.IsFalse(participant.ParticipantStatusId.HasValue);
                Assert.IsFalse(participantPerson.HomeInstitutionAddressId.HasValue);
                Assert.IsFalse(participantPerson.HostInstitutionAddressId.HasValue);
                Assert.IsFalse(participantPerson.HomeInstitutionId.HasValue);
                Assert.IsFalse(participantPerson.HostInstitutionId.HasValue);

            };

            Action tester = () =>
            {
                Assert.AreEqual(1, context.Participants.Count());
                Assert.AreEqual(1, context.ParticipantPersons.Count());
                Assert.AreEqual(2, context.Addresses.Count());
                Assert.AreEqual(2, context.Organizations.Count());

                Assert.AreEqual(createrId, participant.History.CreatedBy);
                Assert.AreEqual(yesterday, participant.History.CreatedOn);
                Assert.AreEqual(createrId, participantPerson.History.CreatedBy);
                Assert.AreEqual(yesterday, participantPerson.History.CreatedOn);                
                

                DateTimeOffset.UtcNow.Should().BeCloseTo(participant.History.RevisedOn, 20000);
                Assert.AreEqual(updaterId, participant.History.RevisedBy);

                DateTimeOffset.UtcNow.Should().BeCloseTo(participantPerson.History.RevisedOn, 20000);              
                Assert.AreEqual(updaterId, participantPerson.History.RevisedBy);

                Assert.AreEqual(home.OrganizationId, participantPerson.HomeInstitutionId);
                Assert.AreEqual(host.OrganizationId, participantPerson.HostInstitutionId);
                Assert.AreEqual(homeAddress.AddressId, participantPerson.HomeInstitutionAddressId);
                Assert.AreEqual(hostAddress.AddressId, participantPerson.HostInstitutionAddressId);
            };

            Action<UpdatedParticipantPersonValidationEntity> validationEntityTester = (entity) =>
            {
                Assert.IsNotNull(entity);
                Assert.IsTrue(Object.ReferenceEquals(individual, entity.ParticipantType));
            };
            validator.Setup(x => x.ValidateUpdate(It.IsAny<UpdatedParticipantPersonValidationEntity>())).Callback(validationEntityTester);

            context.Revert();
            beforeUpdateTester();
            service.CreateOrUpdate(updatedPersonParticipant);
            tester();
            validator.Verify(x => x.ValidateUpdate(It.IsAny<UpdatedParticipantPersonValidationEntity>()), Times.Once());

            context.Revert();
            beforeUpdateTester();
            await service.CreateOrUpdateAsync(updatedPersonParticipant);
            tester();
            validator.Verify(x => x.ValidateUpdate(It.IsAny<UpdatedParticipantPersonValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestCreateOrUpdate_DoesNotHaveHomeOrHostInstitutions()
        {
            var participantId = 1;
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            ParticipantType individual = new ParticipantType
            {
                IsPerson = true,
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            ParticipantStatus status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id,
                Status = ParticipantStatus.Active.Value
            };
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var createrId = 1;
            var updaterId = 2;
            var updater = new User(updaterId);

            context.SetupActions.Add(() =>
            {
                participant = new Participant
                {
                    ParticipantId = participantId,
                };
                participant.History.CreatedBy = createrId;
                participant.History.RevisedBy = createrId;
                participant.History.CreatedOn = yesterday;
                participant.History.RevisedOn = yesterday;

                participantPerson = new ParticipantPerson
                {
                    Participant = participant,
                    ParticipantId = participantId,
                };
                participantPerson.History.CreatedBy = createrId;
                participantPerson.History.RevisedBy = createrId;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedOn = yesterday;

                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantStatuses.Add(status);
                context.ParticipantTypes.Add(individual);
            });

            var updatedPersonParticipant = new UpdatedParticipantPerson(
                updater: updater,
                homeInstitutionAddressId: null,
                homeInstitutionId: null,
                hostInstitutionAddressId: null,
                hostInstitutionId: null,
                participantId: participantId,
                participantStatusId: status.ParticipantStatusId,
                participantTypeId: individual.ParticipantTypeId
                );

            Action tester = () =>
            {
                Assert.IsFalse(participantPerson.HomeInstitutionId.HasValue);
                Assert.IsFalse(participantPerson.HostInstitutionId.HasValue);
                Assert.IsFalse(participantPerson.HomeInstitutionAddressId.HasValue);
                Assert.IsFalse(participantPerson.HostInstitutionAddressId.HasValue);
            };
            context.Revert();
            service.CreateOrUpdate(updatedPersonParticipant);
            tester();

            context.Revert();
            await service.CreateOrUpdateAsync(updatedPersonParticipant);
            tester();
        }

        [TestMethod]
        public async Task TestCreateOrUpdate_HasHomeAndHostNoAddresses()
        {
            var participantId = 1;
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            ParticipantType individual = new ParticipantType
            {
                IsPerson = true,
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            ParticipantStatus status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id,
                Status = ParticipantStatus.Active.Value
            };
            Organization host = new Organization
            {
                OrganizationId = 1
            };
            Organization home = new Organization
            {
                OrganizationId = 2
            };
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var createrId = 1;
            var updaterId = 2;
            var updater = new User(updaterId);

            context.SetupActions.Add(() =>
            {
                participant = new Participant
                {
                    ParticipantId = participantId,
                };
                participant.History.CreatedBy = createrId;
                participant.History.RevisedBy = createrId;
                participant.History.CreatedOn = yesterday;
                participant.History.RevisedOn = yesterday;

                participantPerson = new ParticipantPerson
                {
                    Participant = participant,
                    ParticipantId = participantId,
                };
                participantPerson.History.CreatedBy = createrId;
                participantPerson.History.RevisedBy = createrId;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedOn = yesterday;

                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantStatuses.Add(status);
                context.ParticipantTypes.Add(individual);
                context.Organizations.Add(home);
                context.Organizations.Add(host);
            });

            var updatedPersonParticipant = new UpdatedParticipantPerson(
                updater: updater,
                homeInstitutionAddressId: null,
                homeInstitutionId: home.OrganizationId,
                hostInstitutionAddressId: null,
                hostInstitutionId: host.OrganizationId,
                participantId: participantId,
                participantStatusId: status.ParticipantStatusId,
                participantTypeId: individual.ParticipantTypeId
                );

            Action tester = () =>
            {   
                Assert.AreEqual(home.OrganizationId, participantPerson.HomeInstitutionId);
                Assert.AreEqual(host.OrganizationId, participantPerson.HostInstitutionId);
                Assert.IsFalse(participantPerson.HomeInstitutionAddressId.HasValue);
                Assert.IsFalse(participantPerson.HostInstitutionAddressId.HasValue);
            };
            

            context.Revert();
            service.CreateOrUpdate(updatedPersonParticipant);
            tester();

            context.Revert();
            await service.CreateOrUpdateAsync(updatedPersonParticipant);
            tester();
        }

        [TestMethod]
        public async Task TestCreateOrUpdate_ParticipantDoesNotExist()
        {
            Assert.AreEqual(0, context.Participants.Count());
            ParticipantType individual = new ParticipantType
            {
                IsPerson = true,
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            context.ParticipantTypes.Add(individual);

            var participantId = 1;
            var updatedPersonParticipant = new UpdatedParticipantPerson(
                updater: new User(1),
                homeInstitutionAddressId: null,
                homeInstitutionId: null,
                hostInstitutionAddressId: null,
                hostInstitutionId: null,
                participantId: participantId,
                participantStatusId: null,
                participantTypeId: individual.ParticipantTypeId
                );

            context.Revert();
            Action a = () => service.CreateOrUpdate(updatedPersonParticipant);
            Func<Task> f = () =>
            {
                return service.CreateOrUpdateAsync(updatedPersonParticipant);
            };
            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Participant).Name, updatedPersonParticipant.ParticipantId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestCreateOrUpdate_HomeInstitutionDoesNotExist()
        {
            var participantId = 1;
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            ParticipantType individual = new ParticipantType
            {
                IsPerson = true,
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            ParticipantStatus status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id,
                Status = ParticipantStatus.Active.Value
            };
            Organization host = new Organization
            {
                OrganizationId = 1
            };
            var hostAddress = new Address
            {
                AddressId = 1,
                OrganizationId = host.OrganizationId,
                Organization = host
            };
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var createrId = 1;
            var updaterId = 2;
            var updater = new User(updaterId);

            context.SetupActions.Add(() =>
            {
                participant = new Participant
                {
                    ParticipantId = participantId,
                };
                participant.History.CreatedBy = createrId;
                participant.History.RevisedBy = createrId;
                participant.History.CreatedOn = yesterday;
                participant.History.RevisedOn = yesterday;

                participantPerson = new ParticipantPerson
                {
                    Participant = participant,
                    ParticipantId = participantId,
                };
                participantPerson.History.CreatedBy = createrId;
                participantPerson.History.RevisedBy = createrId;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedOn = yesterday;

                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantStatuses.Add(status);
                context.ParticipantTypes.Add(individual);
                context.Organizations.Add(host);
                context.Addresses.Add(hostAddress);
            });

            var updatedPersonParticipant = new UpdatedParticipantPerson(
                updater: updater,
                homeInstitutionAddressId: null,
                homeInstitutionId: -1,
                hostInstitutionAddressId: hostAddress.AddressId,
                hostInstitutionId: host.OrganizationId,
                participantId: participantId,
                participantStatusId: status.ParticipantStatusId,
                participantTypeId: individual.ParticipantTypeId
                );

            context.Revert();
            Action a = () => service.CreateOrUpdate(updatedPersonParticipant);
            Func<Task> f = () =>
            {
                return service.CreateOrUpdateAsync(updatedPersonParticipant);
            };
            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Organization).Name, updatedPersonParticipant.HomeInstitutionId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestCreateOrUpdate_HostInstitutionDoesNotExist()
        {
            var participantId = 1;
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            ParticipantType individual = new ParticipantType
            {
                IsPerson = true,
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            ParticipantStatus status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id,
                Status = ParticipantStatus.Active.Value
            };
            Organization home = new Organization
            {
                OrganizationId = 2
            };
            var homeAddress = new Address
            {
                AddressId = 2,
                Organization = home,
                OrganizationId = home.OrganizationId
            };
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var createrId = 1;
            var updaterId = 2;
            var updater = new User(updaterId);

            context.SetupActions.Add(() =>
            {
                participant = new Participant
                {
                    ParticipantId = participantId,
                };
                participant.History.CreatedBy = createrId;
                participant.History.RevisedBy = createrId;
                participant.History.CreatedOn = yesterday;
                participant.History.RevisedOn = yesterday;

                participantPerson = new ParticipantPerson
                {
                    Participant = participant,
                    ParticipantId = participantId,
                };
                participantPerson.History.CreatedBy = createrId;
                participantPerson.History.RevisedBy = createrId;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedOn = yesterday;

                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantStatuses.Add(status);
                context.ParticipantTypes.Add(individual);
                context.Organizations.Add(home);
                context.Addresses.Add(homeAddress);
            });

            var updatedPersonParticipant = new UpdatedParticipantPerson(
                updater: updater,
                homeInstitutionAddressId: homeAddress.AddressId,
                homeInstitutionId: home.OrganizationId,
                hostInstitutionAddressId: null,
                hostInstitutionId: -1,
                participantId: participantId,
                participantStatusId: status.ParticipantStatusId,
                participantTypeId: individual.ParticipantTypeId
                );

            context.Revert();
            Action a = () => service.CreateOrUpdate(updatedPersonParticipant);
            Func<Task> f = () =>
            {
                return service.CreateOrUpdateAsync(updatedPersonParticipant);
            };
            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Organization).Name, updatedPersonParticipant.HostInstitutionId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestCreateOrUpdate_HostInstitutionAddressDoesNotExist()
        {
            var participantId = 1;
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            ParticipantType individual = new ParticipantType
            {
                IsPerson = true,
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            ParticipantStatus status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id,
                Status = ParticipantStatus.Active.Value
            };
            Organization host = new Organization
            {
                OrganizationId = 1
            };
            Organization home = new Organization
            {
                OrganizationId = 2
            };
            var homeAddress = new Address
            {
                AddressId = 2,
                Organization = home,
                OrganizationId = home.OrganizationId
            };
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var createrId = 1;
            var updaterId = 2;
            var updater = new User(updaterId);

            context.SetupActions.Add(() =>
            {
                participant = new Participant
                {
                    ParticipantId = participantId,
                };
                participant.History.CreatedBy = createrId;
                participant.History.RevisedBy = createrId;
                participant.History.CreatedOn = yesterday;
                participant.History.RevisedOn = yesterday;

                participantPerson = new ParticipantPerson
                {
                    Participant = participant,
                    ParticipantId = participantId,
                };
                participantPerson.History.CreatedBy = createrId;
                participantPerson.History.RevisedBy = createrId;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedOn = yesterday;

                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantStatuses.Add(status);
                context.ParticipantTypes.Add(individual);
                context.Organizations.Add(home);
                context.Organizations.Add(host);
                context.Addresses.Add(homeAddress);
            });

            var updatedPersonParticipant = new UpdatedParticipantPerson(
                updater: updater,
                homeInstitutionAddressId: homeAddress.AddressId,
                homeInstitutionId: home.OrganizationId,
                hostInstitutionAddressId: -1,
                hostInstitutionId: host.OrganizationId,
                participantId: participantId,
                participantStatusId: status.ParticipantStatusId,
                participantTypeId: individual.ParticipantTypeId
                );

            context.Revert();
            Action a = () => service.CreateOrUpdate(updatedPersonParticipant);
            Func<Task> f = () =>
            {
                return service.CreateOrUpdateAsync(updatedPersonParticipant);
            };
            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Address).Name, updatedPersonParticipant.HostInstitutionAddressId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestCreateOrUpdate_HomeInstitutionAddressDoesNotExist()
        {
            var participantId = 1;
            Participant participant = null;
            ParticipantPerson participantPerson = null;
            ParticipantType individual = new ParticipantType
            {
                IsPerson = true,
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            ParticipantStatus status = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Active.Id,
                Status = ParticipantStatus.Active.Value
            };
            Organization host = new Organization
            {
                OrganizationId = 1
            };
            Organization home = new Organization
            {
                OrganizationId = 2
            };
            var hostAddress = new Address
            {
                AddressId = 1,
                OrganizationId = host.OrganizationId,
                Organization = host
            };
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var createrId = 1;
            var updaterId = 2;
            var updater = new User(updaterId);

            context.SetupActions.Add(() =>
            {
                participant = new Participant
                {
                    ParticipantId = participantId,
                };
                participant.History.CreatedBy = createrId;
                participant.History.RevisedBy = createrId;
                participant.History.CreatedOn = yesterday;
                participant.History.RevisedOn = yesterday;

                participantPerson = new ParticipantPerson
                {
                    Participant = participant,
                    ParticipantId = participantId,
                };
                participantPerson.History.CreatedBy = createrId;
                participantPerson.History.RevisedBy = createrId;
                participantPerson.History.CreatedOn = yesterday;
                participantPerson.History.RevisedOn = yesterday;

                context.ParticipantPersons.Add(participantPerson);
                context.Participants.Add(participant);
                context.ParticipantStatuses.Add(status);
                context.ParticipantTypes.Add(individual);
                context.Organizations.Add(home);
                context.Organizations.Add(host);
                context.Addresses.Add(hostAddress);
            });

            var updatedPersonParticipant = new UpdatedParticipantPerson(
                updater: updater,
                homeInstitutionAddressId: -1,
                homeInstitutionId: home.OrganizationId,
                hostInstitutionAddressId: hostAddress.AddressId,
                hostInstitutionId: host.OrganizationId,
                participantId: participantId,
                participantStatusId: status.ParticipantStatusId,
                participantTypeId: individual.ParticipantTypeId
                );

            context.Revert();
            Action a = () => service.CreateOrUpdate(updatedPersonParticipant);
            Func<Task> f = () =>
            {
                return service.CreateOrUpdateAsync(updatedPersonParticipant);
            };
            var message = String.Format("The model of type [{0}] with id [{1}] was not found.", typeof(Address).Name, updatedPersonParticipant.HomeInstitutionAddressId);
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }
        
        #endregion
    }
}
