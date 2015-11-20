using ECA.Business.Models.Fundings;
using ECA.Business.Queries.Models.Fundings;
using ECA.Business.Service;
using ECA.Business.Service.Fundings;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Exceptions;
using ECA.Core.Generation;
using ECA.Core.Query;
using ECA.Data;
using FluentAssertions;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Fundings
{
    [TestClass]
    public class MoneyFlowServiceTest
    {
        private TestEcaContext context;
        private MoneyFlowService service;
        private Mock<IBusinessValidator<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity>> validator;
        private Mock<IMoneyFlowSourceRecipientTypeService> moneyFlowSourceRecipientTypeService;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            validator = new Mock<IBusinessValidator<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity>>();
            moneyFlowSourceRecipientTypeService = new Mock<IMoneyFlowSourceRecipientTypeService>();
            service = new MoneyFlowService(context, moneyFlowSourceRecipientTypeService.Object, validator.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {


        }
        #region Get Source Money Flows
        [TestMethod]
        public async Task TestGetSourceMoneyFlowDTOById()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "Recip proj"
            };
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProject = recipientProject,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 100.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
            };
            var childMoneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProject = recipientProject,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,

                MoneyFlowId = 100,
                ParentMoneyFlowId = moneyFlow.MoneyFlowId,
                Parent = moneyFlow
            };
            context.MoneyFlowStatuses.Add(actual);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);
            context.MoneyFlows.Add(moneyFlow);
            context.MoneyFlows.Add(childMoneyFlow);
            context.Projects.Add(sourceProject);
            context.Projects.Add(recipientProject);

            Action<SourceMoneyFlowDTO> tester = (dto) =>
            {
                Assert.IsNotNull(dto);
                Assert.AreEqual(moneyFlow.MoneyFlowId, dto.Id);
            };
            var results = service.GetSourceMoneyFlowDTOById(childMoneyFlow.MoneyFlowId);
            var resultsAsync = await service.GetSourceMoneyFlowDTOByIdAsync(childMoneyFlow.MoneyFlowId);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowDTOById_MoneyFlowDoesNotExist()
        {
            Action<SourceMoneyFlowDTO> tester = (dto) =>
            {
                Assert.IsNull(dto);
            };
            var results = service.GetSourceMoneyFlowDTOById(1);
            var resultsAsync = await service.GetSourceMoneyFlowDTOByIdAsync(1);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowDTOById_DoesNotHaveParent()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "Recip proj"
            };
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProject = recipientProject,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 100.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
            };
            context.MoneyFlowStatuses.Add(actual);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Projects.Add(recipientProject);

            Action<SourceMoneyFlowDTO> tester = (dto) =>
            {
                Assert.IsNull(dto);
            };
            var results = service.GetSourceMoneyFlowDTOById(moneyFlow.MoneyFlowId);
            var resultsAsync = await service.GetSourceMoneyFlowDTOByIdAsync(moneyFlow.MoneyFlowId);
            tester(results);
            tester(resultsAsync);
        }


        [TestMethod]
        public async Task TestGetSourceMoneyFlowDTOsByOrganizationId()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientOrg = new Organization
            {
                OrganizationId = recipientId,
                Name = "Recip office"
            };
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            var organizationType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Organization.Id,
                TypeName = MoneyFlowSourceRecipientType.Organization.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientOrganizationId = recipientId,
                SourceProject = sourceProject,
                RecipientOrganization = recipientOrg,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = organizationType,
                RecipientTypeId = organizationType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 100.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
            };
            context.MoneyFlowStatuses.Add(actual);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);
            context.MoneyFlowSourceRecipientTypes.Add(organizationType);
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Organizations.Add(recipientOrg);

            Action<List<SourceMoneyFlowDTO>> tester = (dtos) =>
            {
                Assert.AreEqual(1, dtos.Count);
            };
            var results = service.GetSourceMoneyFlowsByOrganizationId(recipientId);
            var resultsAsync = await service.GetSourceMoneyFlowsByOrganizationIdAsync(recipientId);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowDTOsByOrganizationId_ZeroRemainingAmount()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientOrg = new Organization
            {
                OrganizationId = recipientId,
                Name = "Recip office"
            };
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            var organizationType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Organization.Id,
                TypeName = MoneyFlowSourceRecipientType.Organization.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientOrganizationId = recipientId,
                SourceProject = sourceProject,
                RecipientOrganization = recipientOrg,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = organizationType,
                RecipientTypeId = organizationType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 0m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
            };
            context.MoneyFlowStatuses.Add(actual);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);
            context.MoneyFlowSourceRecipientTypes.Add(organizationType);
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Organizations.Add(recipientOrg);

            Action<List<SourceMoneyFlowDTO>> tester = (dtos) =>
            {
                Assert.AreEqual(0, dtos.Count);
            };
            var results = service.GetSourceMoneyFlowsByOrganizationId(recipientId);
            var resultsAsync = await service.GetSourceMoneyFlowsByOrganizationIdAsync(recipientId);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowDTOsByOfficeId()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientOrg = new Organization
            {
                OrganizationId = recipientId,
                Name = "Recip office"
            };
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            var officeType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Office.Id,
                TypeName = MoneyFlowSourceRecipientType.Office.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientOrganizationId = recipientId,
                SourceProject = sourceProject,
                RecipientOrganization = recipientOrg,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = officeType,
                RecipientTypeId = officeType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 100.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
            };
            context.MoneyFlowStatuses.Add(actual);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);
            context.MoneyFlowSourceRecipientTypes.Add(officeType);
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Organizations.Add(recipientOrg);

            Action<List<SourceMoneyFlowDTO>> tester = (dtos) =>
            {
                Assert.AreEqual(1, dtos.Count);
            };
            var results = service.GetSourceMoneyFlowsByOfficeId(recipientId);
            var resultsAsync = await service.GetSourceMoneyFlowsByOfficeIdAsync(recipientId);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowDTOsByOfficeId_ZeroRemainingAmount()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientOrg = new Organization
            {
                OrganizationId = recipientId,
                Name = "Recip office"
            };
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            var officeType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Office.Id,
                TypeName = MoneyFlowSourceRecipientType.Office.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientOrganizationId = recipientId,
                SourceProject = sourceProject,
                RecipientOrganization = recipientOrg,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = officeType,
                RecipientTypeId = officeType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 0m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
            };
            context.MoneyFlowStatuses.Add(actual);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);
            context.MoneyFlowSourceRecipientTypes.Add(officeType);
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Organizations.Add(recipientOrg);

            Action<List<SourceMoneyFlowDTO>> tester = (dtos) =>
            {
                Assert.AreEqual(0, dtos.Count);
            };
            var results = service.GetSourceMoneyFlowsByOfficeId(recipientId);
            var resultsAsync = await service.GetSourceMoneyFlowsByOfficeIdAsync(recipientId);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowDTOsByProgramId()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Recip prog"
            };
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            var programType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Program.Id,
                TypeName = MoneyFlowSourceRecipientType.Program.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 100.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
            };
            context.MoneyFlowStatuses.Add(actual);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);
            context.MoneyFlowSourceRecipientTypes.Add(programType);
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Programs.Add(recipientProgram);

            Action<List<SourceMoneyFlowDTO>> tester = (dtos) =>
            {
                Assert.AreEqual(1, dtos.Count);
            };
            var results = service.GetSourceMoneyFlowsByProgramId(recipientId);
            var resultsAsync = await service.GetSourceMoneyFlowsByProgramIdAsync(recipientId);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowDTOsByProgramId_ZeroRemainingAmount()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Recip prog"
            };
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            var programType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Program.Id,
                TypeName = MoneyFlowSourceRecipientType.Program.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 0m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
            };
            context.MoneyFlowStatuses.Add(actual);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);
            context.MoneyFlowSourceRecipientTypes.Add(programType);
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Programs.Add(recipientProgram);

            Action<List<SourceMoneyFlowDTO>> tester = (dtos) =>
            {
                Assert.AreEqual(0, dtos.Count);
            };
            var results = service.GetSourceMoneyFlowsByProgramId(recipientId);
            var resultsAsync = await service.GetSourceMoneyFlowsByProgramIdAsync(recipientId);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowDTOsByProjectId()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "Recip proj"
            };
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProject = recipientProject,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 100.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
            };
            context.MoneyFlowStatuses.Add(actual);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Projects.Add(recipientProject);

            Action<List<SourceMoneyFlowDTO>> tester = (dtos) =>
            {
                Assert.AreEqual(1, dtos.Count);
            };
            var results = service.GetSourceMoneyFlowsByProjectId(recipientId);
            var resultsAsync = await service.GetSourceMoneyFlowsByProjectIdAsync(recipientId);
            tester(results);
            tester(resultsAsync);
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowDTOsByProjectId_ZeroRemainingAmount()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "Recip proj"
            };
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProject = recipientProject,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 0m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
            };
            context.MoneyFlowStatuses.Add(actual);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Projects.Add(recipientProject);

            Action<List<SourceMoneyFlowDTO>> tester = (dtos) =>
            {
                Assert.AreEqual(0, dtos.Count);
            };
            var results = service.GetSourceMoneyFlowsByProjectId(recipientId);
            var resultsAsync = await service.GetSourceMoneyFlowsByProjectIdAsync(recipientId);
            tester(results);
            tester(resultsAsync);
        }

        #endregion

        #region Get MoneyFlows By Person Id
        [TestMethod]
        public async Task TestGetMoneyFlowsByPersonId_CheckProperties()
        {
            var outgoing = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Outgoing.Id,
                MoneyFlowTypeName = MoneyFlowType.Outgoing.Value
            };
            var participantType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Participant.Id,
                TypeName = MoneyFlowSourceRecipientType.Participant.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };

            var sourceId = 1;
            var recipientId = 2;
            var sourcePersonId = 3;
            var recipientPersonId = 4;
            var sourcePerson = new Person
            {
                PersonId = sourcePersonId,
                FirstName = "source first",
                Gender = gender,
                GenderId = gender.GenderId
            };
            var recipientPerson = new Person
            {
                PersonId = recipientPersonId,
                FirstName = "recipient first",
                Gender = gender,
                GenderId = gender.GenderId
            };
            var sourceParticipant = new Participant
            {
                ParticipantId = sourceId,
                PersonId = sourcePersonId,
                Person = sourcePerson
            };

            var recipientParticipant = new Participant
            {
                ParticipantId = recipientId,
                PersonId = recipientPersonId,
                Person = recipientPerson
            };

            var moneyFlow = new MoneyFlow
            {
                SourceParticipantId = sourceId,
                RecipientParticipantId = recipientId,
                SourceParticipant = sourceParticipant,
                RecipientParticipant = recipientParticipant,
                SourceType = participantType,
                SourceTypeId = participantType.MoneyFlowSourceRecipientTypeId,
                RecipientType = participantType,
                RecipientTypeId = participantType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.Genders.Add(gender);
            context.MoneyFlows.Add(moneyFlow);
            context.People.Add(sourcePerson);
            context.People.Add(recipientPerson);
            context.Participants.Add(sourceParticipant);
            context.Participants.Add(recipientParticipant);
            context.MoneyFlowTypes.Add(outgoing);
            context.MoneyFlowSourceRecipientTypes.Add(participantType);
            context.MoneyFlowStatuses.Add(actual);

            Action<PagedQueryResults<MoneyFlowDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.IsNotNull(results.Results.First());
                Assert.AreEqual(moneyFlow.MoneyFlowId, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetMoneyFlowsByPersonId(sourcePersonId, queryOperator);
            var serviceResultsAsync = await service.GetMoneyFlowsByPersonIdAsync(sourcePersonId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Get MoneyFlows By Office Id
        [TestMethod]
        public async Task TestGetMoneyFlowsByOfficeId_CheckProperties()
        {
            var outgoing = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Outgoing.Id,
                MoneyFlowTypeName = MoneyFlowType.Outgoing.Value
            };
            var officeType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Office.Id,
                TypeName = MoneyFlowSourceRecipientType.Office.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var sourceId = 1;
            var recipientId = 2;
            var sourceOrg = new Organization
            {
                OrganizationId = sourceId,
                Name = "Outgoing"
            };

            var recipientOrg = new Organization
            {
                OrganizationId = recipientId,
                Name = "Incoming"

            };

            var moneyFlow = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientOrganizationId = recipientId,
                SourceOrganization = sourceOrg,
                RecipientOrganization = recipientOrg,
                SourceType = officeType,
                SourceTypeId = officeType.MoneyFlowSourceRecipientTypeId,
                RecipientType = officeType,
                RecipientTypeId = officeType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Organizations.Add(sourceOrg);
            context.Organizations.Add(recipientOrg);
            context.MoneyFlowTypes.Add(outgoing);
            context.MoneyFlowSourceRecipientTypes.Add(officeType);
            context.MoneyFlowStatuses.Add(actual);

            Action<PagedQueryResults<MoneyFlowDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.IsNotNull(results.Results.First());
                Assert.AreEqual(moneyFlow.MoneyFlowId, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetMoneyFlowsByOfficeId(sourceOrg.OrganizationId, queryOperator);
            var serviceResultsAsync = await service.GetMoneyFlowsByOfficeIdAsync(sourceOrg.OrganizationId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Get MoneyFlows By Organization Id
        [TestMethod]
        public async Task TestGetMoneyFlowsByOrganizationId_CheckProperties()
        {
            var outgoing = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Outgoing.Id,
                MoneyFlowTypeName = MoneyFlowType.Outgoing.Value
            };
            var organizationType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Organization.Id,
                TypeName = MoneyFlowSourceRecipientType.Organization.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var sourceId = 1;
            var recipientId = 2;
            var sourceOrg = new Organization
            {
                OrganizationId = sourceId,
                Name = "Outgoing"
            };

            var recipientOrg = new Organization
            {
                OrganizationId = recipientId,
                Name = "Incoming"

            };

            var moneyFlow = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientOrganizationId = recipientId,
                SourceOrganization = sourceOrg,
                RecipientOrganization = recipientOrg,
                SourceType = organizationType,
                SourceTypeId = organizationType.MoneyFlowSourceRecipientTypeId,
                RecipientType = organizationType,
                RecipientTypeId = organizationType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Organizations.Add(sourceOrg);
            context.Organizations.Add(recipientOrg);
            context.MoneyFlowTypes.Add(outgoing);
            context.MoneyFlowSourceRecipientTypes.Add(organizationType);
            context.MoneyFlowStatuses.Add(actual);

            Action<PagedQueryResults<MoneyFlowDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.IsNotNull(results.Results.First());
                Assert.AreEqual(moneyFlow.MoneyFlowId, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetMoneyFlowsByOrganizationId(sourceOrg.OrganizationId, queryOperator);
            var serviceResultsAsync = await service.GetMoneyFlowsByOrganizationIdAsync(sourceOrg.OrganizationId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);

        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByOrganizationId_OrganizationIsOffice()
        {
            var outgoing = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Outgoing.Id,
                MoneyFlowTypeName = MoneyFlowType.Outgoing.Value
            };
            var organizationType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Organization.Id,
                TypeName = MoneyFlowSourceRecipientType.Organization.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var sourceId = 1;
            var recipientId = 2;

            var officeType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Office.Id
            };
            var sourceOrg = new Organization
            {
                OrganizationId = sourceId,
                Name = "Outgoing",
                OrganizationType = officeType,
                OrganizationTypeId = officeType.OrganizationTypeId
            };

            var recipientOrg = new Organization
            {
                OrganizationId = recipientId,
                Name = "Incoming",
                OrganizationType = officeType,
                OrganizationTypeId = officeType.OrganizationTypeId

            };

            var moneyFlow = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientOrganizationId = recipientId,
                SourceOrganization = sourceOrg,
                RecipientOrganization = recipientOrg,
                SourceType = organizationType,
                SourceTypeId = organizationType.MoneyFlowSourceRecipientTypeId,
                RecipientType = organizationType,
                RecipientTypeId = organizationType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Organizations.Add(sourceOrg);
            context.Organizations.Add(recipientOrg);
            context.MoneyFlowTypes.Add(outgoing);
            context.MoneyFlowSourceRecipientTypes.Add(organizationType);
            context.MoneyFlowStatuses.Add(actual);

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);
            var message = String.Format("The organization with the given id [{0}] is an office named [{1}].  This office must be accessed using office related methods only.",
                        sourceId,
                        sourceOrg.Name);
            Func<Task> f = () =>
            {
                return service.GetMoneyFlowsByOrganizationIdAsync(sourceOrg.OrganizationId, queryOperator);
            };
            service.Invoking(x => x.GetMoneyFlowsByOrganizationId(sourceOrg.OrganizationId, queryOperator)).ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByOrganizationId_OrganizationIsBranch()
        {
            var outgoing = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Outgoing.Id,
                MoneyFlowTypeName = MoneyFlowType.Outgoing.Value
            };
            var organizationType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Organization.Id,
                TypeName = MoneyFlowSourceRecipientType.Organization.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var sourceId = 1;
            var recipientId = 2;

            var branchType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Branch.Id
            };
            var sourceOrg = new Organization
            {
                OrganizationId = sourceId,
                Name = "Outgoing",
                OrganizationType = branchType,
                OrganizationTypeId = branchType.OrganizationTypeId
            };

            var recipientOrg = new Organization
            {
                OrganizationId = recipientId,
                Name = "Incoming",
                OrganizationType = branchType,
                OrganizationTypeId = branchType.OrganizationTypeId

            };

            var moneyFlow = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientOrganizationId = recipientId,
                SourceOrganization = sourceOrg,
                RecipientOrganization = recipientOrg,
                SourceType = organizationType,
                SourceTypeId = organizationType.MoneyFlowSourceRecipientTypeId,
                RecipientType = organizationType,
                RecipientTypeId = organizationType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Organizations.Add(sourceOrg);
            context.Organizations.Add(recipientOrg);
            context.MoneyFlowTypes.Add(outgoing);
            context.MoneyFlowSourceRecipientTypes.Add(organizationType);
            context.MoneyFlowStatuses.Add(actual);

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);
            var message = String.Format("The organization with the given id [{0}] is an office named [{1}].  This office must be accessed using office related methods only.",
                        sourceId,
                        sourceOrg.Name);
            Func<Task> f = () =>
            {
                return service.GetMoneyFlowsByOrganizationIdAsync(sourceOrg.OrganizationId, queryOperator);
            };
            service.Invoking(x => x.GetMoneyFlowsByOrganizationId(sourceOrg.OrganizationId, queryOperator)).ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByOrganizationId_OrganizationIsDivision()
        {
            var outgoing = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Outgoing.Id,
                MoneyFlowTypeName = MoneyFlowType.Outgoing.Value
            };
            var organizationType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Organization.Id,
                TypeName = MoneyFlowSourceRecipientType.Organization.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var sourceId = 1;
            var recipientId = 2;

            var divisonType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Division.Id
            };
            var sourceOrg = new Organization
            {
                OrganizationId = sourceId,
                Name = "Outgoing",
                OrganizationType = divisonType,
                OrganizationTypeId = divisonType.OrganizationTypeId
            };

            var recipientOrg = new Organization
            {
                OrganizationId = recipientId,
                Name = "Incoming",
                OrganizationType = divisonType,
                OrganizationTypeId = divisonType.OrganizationTypeId

            };

            var moneyFlow = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientOrganizationId = recipientId,
                SourceOrganization = sourceOrg,
                RecipientOrganization = recipientOrg,
                SourceType = organizationType,
                SourceTypeId = organizationType.MoneyFlowSourceRecipientTypeId,
                RecipientType = organizationType,
                RecipientTypeId = organizationType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Organizations.Add(sourceOrg);
            context.Organizations.Add(recipientOrg);
            context.MoneyFlowTypes.Add(outgoing);
            context.MoneyFlowSourceRecipientTypes.Add(organizationType);
            context.MoneyFlowStatuses.Add(actual);

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);
            var message = String.Format("The organization with the given id [{0}] is an office named [{1}].  This office must be accessed using office related methods only.",
                        sourceId,
                        sourceOrg.Name);
            Func<Task> f = () =>
            {
                return service.GetMoneyFlowsByOrganizationIdAsync(sourceOrg.OrganizationId, queryOperator);
            };
            service.Invoking(x => x.GetMoneyFlowsByOrganizationId(sourceOrg.OrganizationId, queryOperator)).ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }
        #endregion

        #region Get MoneyFlows By Project Id
        [TestMethod]
        public async Task TestGetMoneyFlowsByProjectId()
        {
            var outgoing = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Outgoing.Id,
                MoneyFlowTypeName = MoneyFlowType.Outgoing.Value
            };
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Outgoing"
            };

            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "Incoming"
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProjectId = recipientId,
                SourceProject = sourceProject,
                RecipientProject = recipientProject,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Projects.Add(recipientProject);
            context.MoneyFlowTypes.Add(outgoing);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);
            context.MoneyFlowStatuses.Add(actual);

            Action<PagedQueryResults<MoneyFlowDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.IsNotNull(results.Results.First());
                Assert.AreEqual(moneyFlow.MoneyFlowId, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetMoneyFlowsByProjectId(sourceProject.ProjectId, queryOperator);
            var serviceResultsAsync = await service.GetMoneyFlowsByProjectIdAsync(sourceProject.ProjectId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Get MoneyFlows By Program Id
        [TestMethod]
        public async Task TestGetMoneyFlowsByProgramId_CheckProperties()
        {
            var outgoing = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Outgoing.Id,
                MoneyFlowTypeName = MoneyFlowType.Outgoing.Value
            };
            var programType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Program.Id,
                TypeName = MoneyFlowSourceRecipientType.Program.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var sourceId = 1;
            var recipientId = 2;
            var sourceProgram = new Program
            {
                ProgramId = sourceId,
                Name = "Outgoing"
            };

            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Incoming"

            };

            var moneyFlow = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProgramId = recipientId,
                SourceProgram = sourceProgram,
                RecipientProgram = recipientProgram,
                SourceType = programType,
                SourceTypeId = programType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Programs.Add(sourceProgram);
            context.Programs.Add(recipientProgram);
            context.MoneyFlowTypes.Add(outgoing);
            context.MoneyFlowSourceRecipientTypes.Add(programType);
            context.MoneyFlowStatuses.Add(actual);

            Action<PagedQueryResults<MoneyFlowDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.IsNotNull(results.Results.First());
                Assert.AreEqual(moneyFlow.MoneyFlowId, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetMoneyFlowsByProgramId(sourceProgram.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetMoneyFlowsByProgramIdAsync(sourceProgram.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);

        }
        #endregion

        #region Get Maximum
        [TestMethod]
        public async Task TestGetMoneyFlowWithdrawalMaximum()
        {
            var projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value,

            };
            var programType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Program.Id,
                TypeName = MoneyFlowSourceRecipientType.Program.Value,
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var sourceId = 1;
            var recipientId = 2;
            var sourceProject = new Project
            {
                ProjectId = sourceId,
                Name = "Project"
            };
            var recipientProgram = new Program
            {
                ProgramId = recipientId,
                Name = "Program"
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 100.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
            };
            var childMoneyFlow = new MoneyFlow
            {
                SourceProjectId = sourceId,
                RecipientProgramId = recipientId,
                SourceProject = sourceProject,
                RecipientProgram = recipientProgram,
                SourceType = projectType,
                SourceTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                RecipientType = programType,
                RecipientTypeId = programType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 20,
                Value = 10.0m,
                Parent = moneyFlow,
                ParentMoneyFlowId = moneyFlow.MoneyFlowId
            };
            context.MoneyFlows.Add(childMoneyFlow);
            context.MoneyFlows.Add(moneyFlow);
            context.Projects.Add(sourceProject);
            context.Programs.Add(recipientProgram);
            context.MoneyFlowSourceRecipientTypes.Add(programType);
            context.MoneyFlowSourceRecipientTypes.Add(projectType);

            var results = service.GetMoneyFlowWithdrawalMaximum(moneyFlow.MoneyFlowId);
            var resultsAsync = await service.GetMoneyFlowWithdrawalMaximumAsync(moneyFlow.MoneyFlowId);
            Assert.AreEqual(moneyFlow.Value - childMoneyFlow.Value, results);
            Assert.AreEqual(moneyFlow.Value - childMoneyFlow.Value, resultsAsync);
        }

        [TestMethod]
        public async Task TestGetMoneyFlowWithdrawalMaximum_MoneyFlowDoesNotExist()
        {
            var results = service.GetMoneyFlowWithdrawalMaximum(-1);
            var resultsAsync = await service.GetMoneyFlowWithdrawalMaximumAsync(-1);
            Assert.IsNull(results);
            Assert.IsNull(resultsAsync);
        }
        #endregion

        #region Create
        [TestMethod]
        public async Task TestCreate_CheckProperties_DoesNotHaveParent()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetType = (c, type) =>
                {
                    Debug.Assert(type == typeof(Project));
                    var dbSet = new System.Data.Entity.Fakes.StubDbSet();
                    dbSet.FindObjectArray = (keys) =>
                    {
                        return (Object)context.Projects.Find(keys);
                    };
                    dbSet.FindAsyncObjectArray = (keys) =>
                    {
                        return Task.FromResult<object>(dbSet.Find(keys));
                    };
                    return dbSet;
                };
                var sourceProject = new Project
                {
                    ProjectId = 1,
                };
                var recipientProject = new Project
                {
                    ProjectId = 2
                };
                context.SetupActions.Add(() =>
                {
                    context.Projects.Add(sourceProject);
                    context.Projects.Add(recipientProject);
                });
                var userId = 1;
                var creator = new User(userId);
                var description = "description";
                decimal value = 1.90m;
                int moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
                var now = DateTimeOffset.UtcNow;
                var fiscalYear = 2015;
                var projectMoneyFlowTypeId = MoneyFlowSourceRecipientType.Project.Id;

                var additionalMoneyFlow = new AdditionalMoneyFlow(
                    createdBy: creator,
                    parentMoneyFlowId: null,
                    description: description,
                    value: value,
                    moneyFlowStatusId: moneyFlowStatusId,
                    transactionDate: now,
                    fiscalYear: fiscalYear,
                    sourceEntityId: sourceProject.ProjectId,
                    sourceEntityTypeId: projectMoneyFlowTypeId,
                    recipientEntityId: recipientProject.ProjectId,
                    recipientEntityTypeId: projectMoneyFlowTypeId
                    );
                var allowedSourceEntityTypes = new List<MoneyFlowSourceRecipientTypeDTO>
                {
                    new MoneyFlowSourceRecipientTypeDTO
                    {
                        Id = MoneyFlowSourceRecipientType.Organization.Id
                    }
                };
                var allowedRecipientEntityTypes = new List<MoneyFlowSourceRecipientTypeDTO>
                {
                    new MoneyFlowSourceRecipientTypeDTO
                    {
                        Id = MoneyFlowSourceRecipientType.Office.Id
                    }
                };
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetRecipientMoneyFlowTypes(It.IsAny<int>())).Returns(allowedRecipientEntityTypes);
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetRecipientMoneyFlowTypesAsync(It.IsAny<int>())).ReturnsAsync(allowedRecipientEntityTypes);
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetSourceMoneyFlowTypes(It.IsAny<int>())).Returns(allowedSourceEntityTypes);
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetSourceMoneyFlowTypesAsync(It.IsAny<int>())).ReturnsAsync(allowedSourceEntityTypes);
                Action tester = () =>
                {
                    Assert.AreEqual(1, context.MoneyFlows.Count());
                    var moneyFlow = context.MoneyFlows.First();
                    Assert.AreEqual(description, moneyFlow.Description);
                    Assert.AreEqual(value, moneyFlow.Value);
                    Assert.AreEqual(moneyFlowStatusId, moneyFlow.MoneyFlowStatusId);
                    Assert.AreEqual(now, moneyFlow.TransactionDate);
                    Assert.AreEqual(fiscalYear, moneyFlow.FiscalYear);
                    Assert.AreEqual(sourceProject.ProjectId, moneyFlow.SourceProjectId);
                    Assert.AreEqual(recipientProject.ProjectId, moneyFlow.RecipientProjectId);
                    Assert.AreEqual(userId, moneyFlow.History.CreatedBy);
                    Assert.AreEqual(userId, moneyFlow.History.RevisedBy);
                    DateTimeOffset.UtcNow.Should().BeCloseTo(moneyFlow.History.CreatedOn, 2000);
                    DateTimeOffset.UtcNow.Should().BeCloseTo(moneyFlow.History.RevisedOn, 2000);

                    Assert.IsFalse(moneyFlow.SourceItineraryStopId.HasValue);
                    Assert.IsFalse(moneyFlow.SourceOrganizationId.HasValue);
                    Assert.IsFalse(moneyFlow.SourceParticipantId.HasValue);
                    Assert.IsFalse(moneyFlow.SourceProgramId.HasValue);
                    Assert.IsFalse(moneyFlow.RecipientAccommodationId.HasValue);
                    Assert.IsFalse(moneyFlow.RecipientItineraryStopId.HasValue);
                    Assert.IsFalse(moneyFlow.RecipientOrganizationId.HasValue);
                    Assert.IsFalse(moneyFlow.RecipientParticipantId.HasValue);
                    Assert.IsFalse(moneyFlow.RecipientProgramId.HasValue);
                    Assert.IsFalse(moneyFlow.RecipientTransportationId.HasValue);
                    Assert.IsFalse(moneyFlow.ParentMoneyFlowId.HasValue);
                };
                Action<MoneyFlowServiceCreateValidationEntity> createValidatorTester = (validationEntity) =>
                {
                    CollectionAssert.AreEqual(allowedSourceEntityTypes.Select(x => x.Id).ToList(), validationEntity.AllowedSourceEntityTypeIds.ToList());
                    CollectionAssert.AreEqual(allowedRecipientEntityTypes.Select(x => x.Id).ToList(), validationEntity.AllowedRecipientEntityTypeIds.ToList());
                    Assert.AreEqual(0, validationEntity.AllowedProjectParticipantIds.Count());

                    Assert.IsNull(validationEntity.ParentMoneyFlowWithdrawlMaximum);
                    Assert.AreEqual(additionalMoneyFlow.Description, validationEntity.Description);
                    Assert.AreEqual(additionalMoneyFlow.Value, validationEntity.Value);
                    Assert.AreEqual(additionalMoneyFlow.FiscalYear, validationEntity.FiscalYear);

                    Assert.IsTrue(validationEntity.HasRecipientEntityType);
                    Assert.IsTrue(validationEntity.HasSourceEntityType);
                    Assert.AreEqual(recipientProject.ProjectId, validationEntity.RecipientEntityId);
                    Assert.AreEqual(sourceProject.ProjectId, validationEntity.SourceEntityId);
                    Assert.AreEqual(projectMoneyFlowTypeId, validationEntity.SourceEntityTypeId);
                    Assert.AreEqual(projectMoneyFlowTypeId, validationEntity.RecipientEntityTypeId);
                };
                validator.Setup(x => x.ValidateCreate(It.IsAny<MoneyFlowServiceCreateValidationEntity>())).Callback(createValidatorTester);

                context.Revert();
                service.Create(additionalMoneyFlow);
                tester();

                context.Revert();
                await service.CreateAsync(additionalMoneyFlow);
                tester();

                validator.Verify(x => x.ValidateCreate(It.IsAny<MoneyFlowServiceCreateValidationEntity>()), Times.Exactly(2));
                moneyFlowSourceRecipientTypeService.Verify(x => x.GetRecipientMoneyFlowTypes(It.IsAny<int>()), Times.Once());
                moneyFlowSourceRecipientTypeService.Verify(x => x.GetRecipientMoneyFlowTypesAsync(It.IsAny<int>()), Times.Once());
            }
        }

        [TestMethod]
        public async Task TestCreate_CheckProperties_DoesHaveParent()
        {
            using (ShimsContext.Create())
            {
                var sourceProject = new Project
                {
                    ProjectId = 1,
                };
                var recipientProject = new Project
                {
                    ProjectId = 2
                };
                var parentMoneyFlow = new MoneyFlow
                {
                    MoneyFlowId = 3
                };
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetType = (c, type) =>
                {
                    Debug.Assert(type == typeof(Project));
                    var dbSet = new System.Data.Entity.Fakes.StubDbSet();
                    dbSet.FindObjectArray = (keys) =>
                    {
                        return (Object)context.Projects.Find(keys);
                    };
                    dbSet.FindAsyncObjectArray = (keys) =>
                    {
                        return Task.FromResult<object>(dbSet.Find(keys));
                    };
                    return dbSet;
                };
                var maximum = 100m;
                ECA.Business.Service.Fundings.Fakes.ShimMoneyFlowService.AllInstances.GetMoneyFlowWithdrawalMaximumAsyncInt32 = (s, mfId) =>
                {
                    return Task.FromResult<decimal?>(maximum);
                };
                ECA.Business.Service.Fundings.Fakes.ShimMoneyFlowService.AllInstances.GetMoneyFlowWithdrawalMaximumInt32 = (s, mfId) =>
                {
                    return maximum;
                };

                context.SetupActions.Add(() =>
                {
                    context.MoneyFlows.Add(parentMoneyFlow);
                    context.Projects.Add(sourceProject);
                    context.Projects.Add(recipientProject);
                });
                var userId = 1;
                var creator = new User(userId);
                var description = "description";
                decimal value = 1.90m;
                int moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
                var now = DateTimeOffset.UtcNow;
                var fiscalYear = 2015;
                var projectMoneyFlowTypeId = MoneyFlowSourceRecipientType.Project.Id;

                var additionalMoneyFlow = new AdditionalMoneyFlow(
                    createdBy: creator,
                    parentMoneyFlowId: parentMoneyFlow.MoneyFlowId,
                    description: description,
                    value: value,
                    moneyFlowStatusId: moneyFlowStatusId,
                    transactionDate: now,
                    fiscalYear: fiscalYear,
                    sourceEntityId: sourceProject.ProjectId,
                    sourceEntityTypeId: projectMoneyFlowTypeId,
                    recipientEntityId: recipientProject.ProjectId,
                    recipientEntityTypeId: projectMoneyFlowTypeId
                    );
                var allowedSourceEntityTypes = new List<MoneyFlowSourceRecipientTypeDTO>
                {
                    new MoneyFlowSourceRecipientTypeDTO
                    {
                        Id = MoneyFlowSourceRecipientType.Organization.Id
                    }
                };
                var allowedRecipientEntityTypes = new List<MoneyFlowSourceRecipientTypeDTO>
                {
                    new MoneyFlowSourceRecipientTypeDTO
                    {
                        Id = MoneyFlowSourceRecipientType.Office.Id
                    }
                };

                moneyFlowSourceRecipientTypeService.Setup(x => x.GetRecipientMoneyFlowTypes(It.IsAny<int>())).Returns(allowedRecipientEntityTypes);
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetRecipientMoneyFlowTypesAsync(It.IsAny<int>())).ReturnsAsync(allowedRecipientEntityTypes);
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetSourceMoneyFlowTypes(It.IsAny<int>())).Returns(allowedSourceEntityTypes);
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetSourceMoneyFlowTypesAsync(It.IsAny<int>())).ReturnsAsync(allowedSourceEntityTypes);
                Action tester = () =>
                {
                    Assert.AreEqual(2, context.MoneyFlows.Count());
                    var addedMoneyFlow = context.MoneyFlows.Where(x => x.MoneyFlowId != parentMoneyFlow.MoneyFlowId).First();
                    Assert.AreEqual(parentMoneyFlow.MoneyFlowId, addedMoneyFlow.ParentMoneyFlowId);
                };
                Action<MoneyFlowServiceCreateValidationEntity> createValidatorTester = (validationEntity) =>
                {
                    CollectionAssert.AreEqual(allowedSourceEntityTypes.Select(x => x.Id).ToList(), validationEntity.AllowedSourceEntityTypeIds.ToList());
                    CollectionAssert.AreEqual(allowedRecipientEntityTypes.Select(x => x.Id).ToList(), validationEntity.AllowedRecipientEntityTypeIds.ToList());
                    Assert.AreEqual(0, validationEntity.AllowedProjectParticipantIds.Count());

                    Assert.AreEqual(maximum, validationEntity.ParentMoneyFlowWithdrawlMaximum);
                    Assert.AreEqual(additionalMoneyFlow.Description, validationEntity.Description);
                    Assert.AreEqual(additionalMoneyFlow.Value, validationEntity.Value);
                    Assert.AreEqual(additionalMoneyFlow.FiscalYear, validationEntity.FiscalYear);

                    Assert.IsTrue(validationEntity.HasRecipientEntityType);
                    Assert.IsTrue(validationEntity.HasSourceEntityType);
                    Assert.AreEqual(recipientProject.ProjectId, validationEntity.RecipientEntityId);
                    Assert.AreEqual(sourceProject.ProjectId, validationEntity.SourceEntityId);
                    Assert.AreEqual(projectMoneyFlowTypeId, validationEntity.SourceEntityTypeId);
                    Assert.AreEqual(projectMoneyFlowTypeId, validationEntity.RecipientEntityTypeId);
                };
                validator.Setup(x => x.ValidateCreate(It.IsAny<MoneyFlowServiceCreateValidationEntity>())).Callback(createValidatorTester);

                context.Revert();
                service.Create(additionalMoneyFlow);
                tester();

                context.Revert();
                await service.CreateAsync(additionalMoneyFlow);
                tester();

                validator.Verify(x => x.ValidateCreate(It.IsAny<MoneyFlowServiceCreateValidationEntity>()), Times.Exactly(2));
                moneyFlowSourceRecipientTypeService.Verify(x => x.GetRecipientMoneyFlowTypes(It.IsAny<int>()), Times.Once());
                moneyFlowSourceRecipientTypeService.Verify(x => x.GetRecipientMoneyFlowTypesAsync(It.IsAny<int>()), Times.Once());
            }
        }

        [TestMethod]
        public async Task TestCreate_ParentMoneyFlowDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                var sourceProject = new Project
                {
                    ProjectId = 1,
                };
                var recipientProject = new Project
                {
                    ProjectId = 2
                };
                context.SetupActions.Add(() =>
                {
                    context.Projects.Add(sourceProject);
                    context.Projects.Add(recipientProject);
                });
                var userId = 1;
                var creator = new User(userId);
                var description = "description";
                decimal value = 1.90m;
                int moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
                var now = DateTimeOffset.UtcNow;
                var fiscalYear = 2015;
                var projectMoneyFlowTypeId = MoneyFlowSourceRecipientType.Project.Id;

                var additionalMoneyFlow = new AdditionalMoneyFlow(
                    createdBy: creator,
                    parentMoneyFlowId: -1,
                    description: description,
                    value: value,
                    moneyFlowStatusId: moneyFlowStatusId,
                    transactionDate: now,
                    fiscalYear: fiscalYear,
                    sourceEntityId: sourceProject.ProjectId,
                    sourceEntityTypeId: projectMoneyFlowTypeId,
                    recipientEntityId: recipientProject.ProjectId,
                    recipientEntityTypeId: projectMoneyFlowTypeId
                    );
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetRecipientMoneyFlowTypes(It.IsAny<int>())).Returns(new List<MoneyFlowSourceRecipientTypeDTO>());
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetRecipientMoneyFlowTypesAsync(It.IsAny<int>())).ReturnsAsync(new List<MoneyFlowSourceRecipientTypeDTO>());
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetSourceMoneyFlowTypes(It.IsAny<int>())).Returns(new List<MoneyFlowSourceRecipientTypeDTO>());
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetSourceMoneyFlowTypesAsync(It.IsAny<int>())).ReturnsAsync(new List<MoneyFlowSourceRecipientTypeDTO>());
                context.Revert();
                var message = String.Format("The entity of type [{0}] with id [{1}] was not found.", typeof(MoneyFlow).Name, additionalMoneyFlow.ParentMoneyFlowId);
                Func<Task> f = () =>
                {
                    return service.CreateAsync(additionalMoneyFlow);
                };

                service.Invoking(x => x.Create(additionalMoneyFlow)).ShouldThrow<ModelNotFoundException>().WithMessage(message);
                f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            }
        }

        [TestMethod]
        public async Task TestCreate_SourceDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetType = (c, type) =>
                {
                    Debug.Assert(type == typeof(Project));
                    var dbSet = new System.Data.Entity.Fakes.StubDbSet();
                    dbSet.FindObjectArray = (keys) =>
                    {
                        return (Object)context.Projects.Find(keys);
                    };
                    dbSet.FindAsyncObjectArray = (keys) =>
                    {
                        return Task.FromResult<object>(dbSet.Find(keys));
                    };
                    return dbSet;
                };
                var recipientProject = new Project
                {
                    ProjectId = 2
                };
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetRecipientMoneyFlowTypes(It.IsAny<int>())).Returns(new List<MoneyFlowSourceRecipientTypeDTO>());
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetRecipientMoneyFlowTypesAsync(It.IsAny<int>())).ReturnsAsync(new List<MoneyFlowSourceRecipientTypeDTO>());
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetSourceMoneyFlowTypes(It.IsAny<int>())).Returns(new List<MoneyFlowSourceRecipientTypeDTO>());
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetSourceMoneyFlowTypesAsync(It.IsAny<int>())).ReturnsAsync(new List<MoneyFlowSourceRecipientTypeDTO>());
                context.SetupActions.Add(() =>
                {
                    context.Projects.Add(recipientProject);
                });
                var userId = 1;
                var creator = new User(userId);
                var description = "description";
                decimal value = 1.90m;
                int moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
                var now = DateTimeOffset.UtcNow;
                var fiscalYear = 2015;
                var projectMoneyFlowTypeId = MoneyFlowSourceRecipientType.Project.Id;

                var additionalMoneyFlow = new AdditionalMoneyFlow(
                    createdBy: creator,
                    parentMoneyFlowId: null,
                    description: description,
                    value: value,
                    moneyFlowStatusId: moneyFlowStatusId,
                    transactionDate: now,
                    fiscalYear: fiscalYear,
                    sourceEntityId: -1,
                    sourceEntityTypeId: projectMoneyFlowTypeId,
                    recipientEntityId: recipientProject.ProjectId,
                    recipientEntityTypeId: projectMoneyFlowTypeId
                    );
                context.Revert();
                Func<Task> f = () =>
                {
                    return service.CreateAsync(additionalMoneyFlow);
                };

                service.Invoking(x => x.Create(additionalMoneyFlow)).ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The entity of type [{0}] with id [{1}] was not found.", "Project", -1));
                f.ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The entity of type [{0}] with id [{1}] was not found.", "Project", -1));
            }
        }

        [TestMethod]
        public async Task TestCreate_RecipientDoesNotExist()
        {
            using (ShimsContext.Create())
            {
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetType = (c, type) =>
                {
                    Debug.Assert(type == typeof(Project));
                    var dbSet = new System.Data.Entity.Fakes.StubDbSet();
                    dbSet.FindObjectArray = (keys) =>
                    {
                        return (Object)context.Projects.Find(keys);
                    };
                    dbSet.FindAsyncObjectArray = (keys) =>
                    {
                        return Task.FromResult<object>(dbSet.Find(keys));
                    };
                    return dbSet;
                };
                var sourceProject = new Project
                {
                    ProjectId = 1
                };
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetRecipientMoneyFlowTypes(It.IsAny<int>())).Returns(new List<MoneyFlowSourceRecipientTypeDTO>());
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetRecipientMoneyFlowTypesAsync(It.IsAny<int>())).ReturnsAsync(new List<MoneyFlowSourceRecipientTypeDTO>());
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetSourceMoneyFlowTypes(It.IsAny<int>())).Returns(new List<MoneyFlowSourceRecipientTypeDTO>());
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetSourceMoneyFlowTypesAsync(It.IsAny<int>())).ReturnsAsync(new List<MoneyFlowSourceRecipientTypeDTO>());
                context.SetupActions.Add(() =>
                {
                    context.Projects.Add(sourceProject);
                });
                var userId = 1;
                var creator = new User(userId);
                var description = "description";
                decimal value = 1.90m;
                int moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
                var now = DateTimeOffset.UtcNow;
                var fiscalYear = 2015;
                var projectMoneyFlowTypeId = MoneyFlowSourceRecipientType.Project.Id;

                var additionalMoneyFlow = new AdditionalMoneyFlow(
                    createdBy: creator,
                    parentMoneyFlowId: null,
                    description: description,
                    value: value,
                    moneyFlowStatusId: moneyFlowStatusId,
                    transactionDate: now,
                    fiscalYear: fiscalYear,
                    sourceEntityId: sourceProject.ProjectId,
                    sourceEntityTypeId: projectMoneyFlowTypeId,
                    recipientEntityId: -1,
                    recipientEntityTypeId: projectMoneyFlowTypeId
                    );
                context.Revert();
                Func<Task> f = () =>
                {
                    return service.CreateAsync(additionalMoneyFlow);
                };

                service.Invoking(x => x.Create(additionalMoneyFlow)).ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The entity of type [{0}] with id [{1}] was not found.", "Project", -1));
                f.ShouldThrow<ModelNotFoundException>()
                    .WithMessage(String.Format("The entity of type [{0}] with id [{1}] was not found.", "Project", -1));
            }
        }

        [TestMethod]
        public async Task TestCreate_SourceProject_RecipientParticipant()
        {
            using (ShimsContext.Create())
            {
                var sourceProjectId = 1;
                var recipientPersonId = 2;
                var recipientParticipantId = 3;
                System.Data.Entity.Fakes.ShimDbContext.AllInstances.SetType = (c, type) =>
                {
                    var dbSet = new System.Data.Entity.Fakes.StubDbSet();
                    dbSet.FindObjectArray = (keys) =>
                    {
                        var id = (int)keys[0];
                        if (id == sourceProjectId)
                        {
                            return (Object)context.Projects.Find(keys);
                        }
                        if (id == recipientPersonId)
                        {
                            return (Object)context.People.Find(keys);
                        }
                        if (id == recipientParticipantId)
                        {
                            return (Object)context.Participants.Find(keys);
                        }
                        throw new Exception();

                    };
                    dbSet.FindAsyncObjectArray = (keys) =>
                    {
                        var id = (int)keys[0];
                        if (id == sourceProjectId)
                        {
                            return Task.FromResult<object>((Object)context.Projects.Find(keys));
                        }
                        if (id == recipientPersonId)
                        {
                            return Task.FromResult<object>((Object)context.People.Find(keys));
                        }
                        if (id == recipientParticipantId)
                        {
                            return Task.FromResult<object>((Object)context.Participants.Find(keys));
                        }
                        throw new Exception();
                    };
                    return dbSet;
                };
                var sourceProject = new Project
                {
                    ProjectId = sourceProjectId,
                };
                var personParticipantType = new ParticipantType
                {
                    ParticipantTypeId = ParticipantType.Individual.Id,

                };
                var person = new Person
                {
                    PersonId = recipientPersonId,
                    FirstName = "First Name",
                };
                var recipientParticipant = new Participant
                {
                    ParticipantId = recipientParticipantId,
                    ProjectId = sourceProject.ProjectId,
                    Person = person,
                    PersonId = person.PersonId,
                    ParticipantType = personParticipantType,
                    ParticipantTypeId = personParticipantType.ParticipantTypeId
                };
                context.SetupActions.Add(() =>
                {
                    context.Projects.Add(sourceProject);
                    context.Participants.Add(recipientParticipant);
                    context.People.Add(person);
                    context.ParticipantTypes.Add(personParticipantType);
                });
                var userId = 1;
                var creator = new User(userId);
                var description = "description";
                decimal value = 1.90m;
                int moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
                var now = DateTimeOffset.UtcNow;
                var fiscalYear = 2015;
                var projectMoneyFlowTypeId = MoneyFlowSourceRecipientType.Project.Id;
                var participantMoneyFlowTypeId = MoneyFlowSourceRecipientType.Participant.Id;

                var additionalMoneyFlow = new AdditionalMoneyFlow(
                    createdBy: creator,
                    parentMoneyFlowId: null,
                    description: description,
                    value: value,
                    moneyFlowStatusId: moneyFlowStatusId,
                    transactionDate: now,
                    fiscalYear: fiscalYear,
                    sourceEntityId: sourceProject.ProjectId,
                    sourceEntityTypeId: projectMoneyFlowTypeId,
                    recipientEntityId: recipientParticipant.ParticipantId,
                    recipientEntityTypeId: participantMoneyFlowTypeId
                    );
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetRecipientMoneyFlowTypes(It.IsAny<int>())).Returns(new List<MoneyFlowSourceRecipientTypeDTO>());
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetRecipientMoneyFlowTypesAsync(It.IsAny<int>())).ReturnsAsync(new List<MoneyFlowSourceRecipientTypeDTO>());
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetSourceMoneyFlowTypes(It.IsAny<int>())).Returns(new List<MoneyFlowSourceRecipientTypeDTO>());
                moneyFlowSourceRecipientTypeService.Setup(x => x.GetSourceMoneyFlowTypesAsync(It.IsAny<int>())).ReturnsAsync(new List<MoneyFlowSourceRecipientTypeDTO>());
                Action<MoneyFlowServiceCreateValidationEntity> createValidatorTester = (validationEntity) =>
                {
                    Assert.AreEqual(1, validationEntity.AllowedProjectParticipantIds.Count());
                    Assert.AreEqual(recipientParticipant.ParticipantId, validationEntity.AllowedProjectParticipantIds.First());                    
                };
                validator.Setup(x => x.ValidateCreate(It.IsAny<MoneyFlowServiceCreateValidationEntity>())).Callback(createValidatorTester);

                Action tester = () =>
                    {
                        Assert.AreEqual(1, context.MoneyFlows.Count());
                        var moneyFlow = context.MoneyFlows.First();
                        Assert.AreEqual(description, moneyFlow.Description);
                        Assert.AreEqual(value, moneyFlow.Value);
                        Assert.AreEqual(moneyFlowStatusId, moneyFlow.MoneyFlowStatusId);
                        Assert.AreEqual(now, moneyFlow.TransactionDate);
                        Assert.AreEqual(fiscalYear, moneyFlow.FiscalYear);
                        Assert.AreEqual(sourceProject.ProjectId, moneyFlow.SourceProjectId);
                        Assert.AreEqual(recipientParticipant.ParticipantId, moneyFlow.RecipientParticipantId);
                        Assert.AreEqual(userId, moneyFlow.History.CreatedBy);
                        Assert.AreEqual(userId, moneyFlow.History.RevisedBy);
                        DateTimeOffset.UtcNow.Should().BeCloseTo(moneyFlow.History.CreatedOn, 2000);
                        DateTimeOffset.UtcNow.Should().BeCloseTo(moneyFlow.History.RevisedOn, 2000);

                        Assert.IsFalse(moneyFlow.SourceItineraryStopId.HasValue);
                        Assert.IsFalse(moneyFlow.SourceOrganizationId.HasValue);
                        Assert.IsFalse(moneyFlow.SourceParticipantId.HasValue);
                        Assert.IsFalse(moneyFlow.SourceProgramId.HasValue);
                        Assert.IsFalse(moneyFlow.RecipientAccommodationId.HasValue);
                        Assert.IsFalse(moneyFlow.RecipientItineraryStopId.HasValue);
                        Assert.IsFalse(moneyFlow.RecipientOrganizationId.HasValue);
                        Assert.IsFalse(moneyFlow.RecipientProjectId.HasValue);
                        Assert.IsFalse(moneyFlow.RecipientProgramId.HasValue);
                        Assert.IsFalse(moneyFlow.RecipientTransportationId.HasValue);
                    };

                context.Revert();
                service.Create(additionalMoneyFlow);
                tester();

                context.Revert();
                await service.CreateAsync(additionalMoneyFlow);
                tester();

                validator.Verify(x => x.ValidateCreate(It.IsAny<MoneyFlowServiceCreateValidationEntity>()), Times.Exactly(2));
                moneyFlowSourceRecipientTypeService.Verify(x => x.GetRecipientMoneyFlowTypes(It.IsAny<int>()), Times.Once());
                moneyFlowSourceRecipientTypeService.Verify(x => x.GetRecipientMoneyFlowTypesAsync(It.IsAny<int>()), Times.Once());
            }
        }
        #endregion

        #region Fiscal Year Summaries
        [TestMethod]
        public void TestAddMissingFiscalYearSummaryDTOs_EmptyList()
        {
            var moneyFlowStatii = new List<MoneyFlowStatus>();
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            moneyFlowStatii.Add(actual);

            var list = new List<FiscalYearSummaryDTO>();
            var result = service.AddMissingFiscalYearSummaryDTOs(list, moneyFlowStatii);
            Assert.IsTrue(Object.ReferenceEquals(list, result));
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void TestAddMissingFiscalYearSummaryDTOs_OneFiscalYearMissing()
        {
            var moneyFlowStatii = new List<MoneyFlowStatus>();
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            moneyFlowStatii.Add(actual);

            var list = new List<FiscalYearSummaryDTO>();
            var summary1 = new FiscalYearSummaryDTO
            {
                EntityId = 1,
                EntityTypeId = MoneyFlowSourceRecipientType.Project.Id,
                FiscalYear = 2000,
                IncomingAmount = 20.0m,
                OutgoingAmount = 10.0m,
                RemainingAmount = 10.0m,
                Status = actual.MoneyFlowStatusName,
                StatusId = actual.MoneyFlowStatusId
                
            };
            var summary2 = new FiscalYearSummaryDTO
            {
                EntityId = summary1.EntityId,
                EntityTypeId = summary1.EntityTypeId,
                FiscalYear = 2002,
                IncomingAmount = summary1.IncomingAmount,
                OutgoingAmount = summary1.OutgoingAmount,
                RemainingAmount = summary1.RemainingAmount,
                Status = summary1.Status,
                StatusId = summary1.StatusId
            };
            list.Add(summary1);
            list.Add(summary2);
            list = list.OrderBy(x => x.FiscalYear).ToList();//just reverse the sorting for fun...

            var results = service.AddMissingFiscalYearSummaryDTOs(list, moneyFlowStatii);
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual(summary2.FiscalYear, results.First().FiscalYear);
            Assert.AreEqual(summary1.FiscalYear, results.Last().FiscalYear);

            var addedSummary = results[1];
            Assert.AreEqual(summary1.FiscalYear + 1, addedSummary.FiscalYear);
            Assert.AreEqual(actual.MoneyFlowStatusId, addedSummary.StatusId);
            Assert.AreEqual(actual.MoneyFlowStatusName, addedSummary.Status);
            Assert.AreEqual(summary1.EntityId, addedSummary.EntityId);
            Assert.AreEqual(summary1.EntityTypeId, addedSummary.EntityTypeId);
            Assert.AreEqual(0.0m, addedSummary.IncomingAmount);
            Assert.AreEqual(0.0m, addedSummary.OutgoingAmount);
            Assert.AreEqual(0.0m, addedSummary.RemainingAmount);
            Assert.IsTrue(addedSummary.IsEmpty);
        }

        [TestMethod]
        public void TestAddMissingFiscalYearSummaryDTOs_TwoFiscalYearsMissing()
        {
            var moneyFlowStatii = new List<MoneyFlowStatus>();
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            moneyFlowStatii.Add(actual);

            var list = new List<FiscalYearSummaryDTO>();
            var summary1 = new FiscalYearSummaryDTO
            {
                EntityId = 1,
                EntityTypeId = MoneyFlowSourceRecipientType.Project.Id,
                FiscalYear = 2000,
                IncomingAmount = 20.0m,
                OutgoingAmount = 10.0m,
                RemainingAmount = 10.0m,
                Status = actual.MoneyFlowStatusName,
                StatusId = actual.MoneyFlowStatusId

            };
            var summary2 = new FiscalYearSummaryDTO
            {
                EntityId = summary1.EntityId,
                EntityTypeId = summary1.EntityTypeId,
                FiscalYear = 2003,
                IncomingAmount = summary1.IncomingAmount,
                OutgoingAmount = summary1.OutgoingAmount,
                RemainingAmount = summary1.RemainingAmount,
                Status = summary1.Status,
                StatusId = summary1.StatusId
            };
            list.Add(summary1);
            list.Add(summary2);
            list = list.OrderBy(x => x.FiscalYear).ToList();

            var results = service.AddMissingFiscalYearSummaryDTOs(list, moneyFlowStatii);
            Assert.AreEqual(4, results.Count);
            Assert.AreEqual(summary2.FiscalYear, results.First().FiscalYear);
            Assert.AreEqual(summary1.FiscalYear, results.Last().FiscalYear);
            Assert.IsTrue(results[1].IsEmpty);
            Assert.IsTrue(results[2].IsEmpty);
        }

        [TestMethod]
        public void TestAddMissingFiscalYearSummaryDTOs_NoFiscalYearsMissing()
        {
            var moneyFlowStatii = new List<MoneyFlowStatus>();
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            moneyFlowStatii.Add(actual);

            var list = new List<FiscalYearSummaryDTO>();
            var summary1 = new FiscalYearSummaryDTO
            {
                EntityId = 1,
                EntityTypeId = MoneyFlowSourceRecipientType.Project.Id,
                FiscalYear = 2000,
                IncomingAmount = 20.0m,
                OutgoingAmount = 10.0m,
                RemainingAmount = 10.0m,
                Status = actual.MoneyFlowStatusName,
                StatusId = actual.MoneyFlowStatusId

            };
            var summary2 = new FiscalYearSummaryDTO
            {
                EntityId = summary1.EntityId,
                EntityTypeId = summary1.EntityTypeId,
                FiscalYear = 2001,
                IncomingAmount = summary1.IncomingAmount,
                OutgoingAmount = summary1.OutgoingAmount,
                RemainingAmount = summary1.RemainingAmount,
                Status = summary1.Status,
                StatusId = summary1.StatusId
            };
            list.Add(summary1);
            list.Add(summary2);
            list = list.OrderBy(x => x.FiscalYear).ToList();

            var results = service.AddMissingFiscalYearSummaryDTOs(list, moneyFlowStatii);
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(summary2.FiscalYear, results.First().FiscalYear);
            Assert.AreEqual(summary1.FiscalYear, results.Last().FiscalYear);
        }


        [TestMethod]
        public async Task TestGetFiscalYearSummariesByProjectId()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProgram = new Program
            {
                ProgramId = sourceId,
                Name = "program"
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "project"
            };
            var sourceTypeLookup = MoneyFlowSourceRecipientType.Program;
            var recipientTypeLookup = MoneyFlowSourceRecipientType.Project;
            var sourceType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = sourceTypeLookup.Id,
                TypeName = sourceTypeLookup.Value
            };
            var recipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = recipientTypeLookup.Id,
                TypeName = recipientTypeLookup.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProjectId = recipientId,
                SourceProgram = sourceProgram,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Programs.Add(sourceProgram);
            context.Projects.Add(recipientProject);
            context.MoneyFlowStatuses.Add(actual);
            Action<List<FiscalYearSummaryDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count);
            };
            var serviceResults = service.GetFiscalYearSummariesByProjectId(recipientId);
            var serviceResultsAsync = await service.GetFiscalYearSummariesByProjectIdAsync(recipientId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFiscalYearSummariesByProjectId_CheckMissingFiscalYearSummaries()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProgram = new Program
            {
                ProgramId = sourceId,
                Name = "program"
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "project"
            };
            var sourceTypeLookup = MoneyFlowSourceRecipientType.Program;
            var recipientTypeLookup = MoneyFlowSourceRecipientType.Project;
            var sourceType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = sourceTypeLookup.Id,
                TypeName = sourceTypeLookup.Value
            };
            var recipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = recipientTypeLookup.Id,
                TypeName = recipientTypeLookup.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProjectId = recipientId,
                SourceProgram = sourceProgram,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            var moneyFlow2 = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProjectId = recipientId,
                SourceProgram = sourceProgram,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = moneyFlow.FiscalYear + 2,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            context.MoneyFlows.Add(moneyFlow);
            context.MoneyFlows.Add(moneyFlow2);
            context.Programs.Add(sourceProgram);
            context.Projects.Add(recipientProject);
            context.MoneyFlowStatuses.Add(actual);
            Action<List<FiscalYearSummaryDTO>> tester = (results) =>
            {
                Assert.AreEqual(3, results.Count);
                Assert.AreEqual(1, results.Where(x => x.FiscalYear == moneyFlow.FiscalYear + 1).Count());
            };
            var serviceResults = service.GetFiscalYearSummariesByProjectId(recipientId);
            var serviceResultsAsync = await service.GetFiscalYearSummariesByProjectIdAsync(recipientId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFiscalYearSummariesByProgramId()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProgram = new Program
            {
                ProgramId = sourceId,
                Name = "program"
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "project"
            };
            var sourceTypeLookup = MoneyFlowSourceRecipientType.Program;
            var recipientTypeLookup = MoneyFlowSourceRecipientType.Project;
            var sourceType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = sourceTypeLookup.Id,
                TypeName = sourceTypeLookup.Value
            };
            var recipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = recipientTypeLookup.Id,
                TypeName = recipientTypeLookup.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProjectId = recipientId,
                SourceProgram = sourceProgram,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Programs.Add(sourceProgram);
            context.Projects.Add(recipientProject);
            context.MoneyFlowStatuses.Add(actual);
            Action<List<FiscalYearSummaryDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count);
            };
            var serviceResults = service.GetFiscalYearSummariesByProgramId(sourceId);
            var serviceResultsAsync = await service.GetFiscalYearSummariesByProgramIdAsync(sourceId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFiscalYearSummariesByProgramId_CheckMissingFiscalYearSummaries()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceProgram = new Program
            {
                ProgramId = sourceId,
                Name = "program"
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "project"
            };
            var sourceTypeLookup = MoneyFlowSourceRecipientType.Program;
            var recipientTypeLookup = MoneyFlowSourceRecipientType.Project;
            var sourceType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = sourceTypeLookup.Id,
                TypeName = sourceTypeLookup.Value
            };
            var recipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = recipientTypeLookup.Id,
                TypeName = recipientTypeLookup.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var moneyFlow = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProjectId = recipientId,
                SourceProgram = sourceProgram,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            var moneyFlow2 = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProjectId = recipientId,
                SourceProgram = sourceProgram,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = moneyFlow.FiscalYear + 2,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            context.MoneyFlows.Add(moneyFlow);
            context.MoneyFlows.Add(moneyFlow2);
            context.Programs.Add(sourceProgram);
            context.Projects.Add(recipientProject);
            context.MoneyFlowStatuses.Add(actual);
            Action<List<FiscalYearSummaryDTO>> tester = (results) =>
            {
                Assert.AreEqual(3, results.Count);
                Assert.AreEqual(1, results.Where(x => x.FiscalYear == moneyFlow.FiscalYear + 1).Count());
            };
            var serviceResults = service.GetFiscalYearSummariesByProgramId(sourceId);
            var serviceResultsAsync = await service.GetFiscalYearSummariesByProgramIdAsync(sourceId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFiscalYearSummariesByOfficeId()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceOffice = new Organization
            {
                OrganizationId = sourceId,
                Name = "office",
                OrganizationTypeId = OrganizationType.Office.Id
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "project"
            };
            var sourceTypeLookup = MoneyFlowSourceRecipientType.Office;
            var recipientTypeLookup = MoneyFlowSourceRecipientType.Project;
            var sourceType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = sourceTypeLookup.Id,
                TypeName = sourceTypeLookup.Value
            };
            var recipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = recipientTypeLookup.Id,
                TypeName = recipientTypeLookup.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var moneyFlow = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientProjectId = recipientId,
                SourceOrganization = sourceOffice,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Organizations.Add(sourceOffice);
            context.Projects.Add(recipientProject);
            context.MoneyFlowStatuses.Add(actual);
            Action<List<FiscalYearSummaryDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count);
            };
            var serviceResults = service.GetFiscalYearSummariesByOfficeId(sourceId);
            var serviceResultsAsync = await service.GetFiscalYearSummariesByOfficeIdAsync(sourceId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFiscalYearSummariesByOfficeId_CheckMissingFiscalYearSummaries()
        {
            var sourceId = 1;
            var recipientId = 2;
            var sourceOffice = new Organization
            {
                OrganizationId = sourceId,
                Name = "office",
                OrganizationTypeId = OrganizationType.Office.Id
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "project"
            };
            var sourceTypeLookup = MoneyFlowSourceRecipientType.Office;
            var recipientTypeLookup = MoneyFlowSourceRecipientType.Project;
            var sourceType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = sourceTypeLookup.Id,
                TypeName = sourceTypeLookup.Value
            };
            var recipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = recipientTypeLookup.Id,
                TypeName = recipientTypeLookup.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var moneyFlow = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientProjectId = recipientId,
                SourceOrganization = sourceOffice,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };

            var moneyFlow2 = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientProjectId = recipientId,
                SourceOrganization = sourceOffice,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = moneyFlow.FiscalYear + 2,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            context.MoneyFlows.Add(moneyFlow);
            context.MoneyFlows.Add(moneyFlow2);
            context.Organizations.Add(sourceOffice);
            context.Projects.Add(recipientProject);
            context.MoneyFlowStatuses.Add(actual);
            Action<List<FiscalYearSummaryDTO>> tester = (results) =>
            {
                Assert.AreEqual(3, results.Count);
                Assert.AreEqual(1, results.Where(x => x.FiscalYear == moneyFlow.FiscalYear + 1).Count());
            };
            var serviceResults = service.GetFiscalYearSummariesByOfficeId(sourceId);
            var serviceResultsAsync = await service.GetFiscalYearSummariesByOfficeIdAsync(sourceId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFiscalYearSummariesByOrganizationId()
        {

            var sourceTypeLookup = MoneyFlowSourceRecipientType.Organization;
            var recipientTypeLookup = MoneyFlowSourceRecipientType.Project;
            var sourceType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = sourceTypeLookup.Id,
                TypeName = sourceTypeLookup.Value
            };
            var recipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = recipientTypeLookup.Id,
                TypeName = recipientTypeLookup.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var sourceId = 1;
            var recipientId = 2;
            var sourceOrganization = new Organization
            {
                OrganizationId = sourceId,
                Name = "program",
                OrganizationTypeId = OrganizationType.Other.Id
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "project"
            };
            var moneyFlow = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientProjectId = recipientId,
                SourceOrganization = sourceOrganization,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            
            context.MoneyFlowSourceRecipientTypes.Add(sourceType);
            context.MoneyFlowSourceRecipientTypes.Add(recipientType);
            context.MoneyFlows.Add(moneyFlow);
            context.Organizations.Add(sourceOrganization);
            context.Projects.Add(recipientProject);
            context.MoneyFlowStatuses.Add(actual);
            Action<List<FiscalYearSummaryDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count);
            };
            var serviceResults = service.GetFiscalYearSummariesByOrganizationId(sourceId);
            var serviceResultsAsync = await service.GetFiscalYearSummariesByOrganizationIdAsync(sourceId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFiscalYearSummariesByOrganizationId_CheckMissingFiscalYearSummaries()
        {

            var sourceTypeLookup = MoneyFlowSourceRecipientType.Organization;
            var recipientTypeLookup = MoneyFlowSourceRecipientType.Project;
            var sourceType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = sourceTypeLookup.Id,
                TypeName = sourceTypeLookup.Value
            };
            var recipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = recipientTypeLookup.Id,
                TypeName = recipientTypeLookup.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };
            var sourceId = 1;
            var recipientId = 2;
            var sourceOrganization = new Organization
            {
                OrganizationId = sourceId,
                Name = "program",
                OrganizationTypeId = OrganizationType.Other.Id
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "project"
            };
            var moneyFlow = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientProjectId = recipientId,
                SourceOrganization = sourceOrganization,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            var moneyFlow2 = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientProjectId = recipientId,
                SourceOrganization = sourceOrganization,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = moneyFlow.FiscalYear + 2,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            context.MoneyFlowSourceRecipientTypes.Add(sourceType);
            context.MoneyFlowSourceRecipientTypes.Add(recipientType);
            context.MoneyFlows.Add(moneyFlow);
            context.MoneyFlows.Add(moneyFlow2);
            context.Organizations.Add(sourceOrganization);
            context.Projects.Add(recipientProject);
            context.MoneyFlowStatuses.Add(actual);
            Action<List<FiscalYearSummaryDTO>> tester = (results) =>
            {
                Assert.AreEqual(3, results.Count);
                Assert.AreEqual(1, results.Where(x => x.FiscalYear == moneyFlow.FiscalYear + 1).Count());
            };
            var serviceResults = service.GetFiscalYearSummariesByOrganizationId(sourceId);
            var serviceResultsAsync = await service.GetFiscalYearSummariesByOrganizationIdAsync(sourceId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFiscalYearSummariesByOrganizationId_OrganizationIsAnOffice()
        {
            var outgoing = new MoneyFlowType
            {
                MoneyFlowTypeId = MoneyFlowType.Outgoing.Id,
                MoneyFlowTypeName = MoneyFlowType.Outgoing.Value
            };
            var organizationType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Organization.Id,
                TypeName = MoneyFlowSourceRecipientType.Organization.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var sourceId = 1;
            var recipientId = 2;

            var officeType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Office.Id
            };
            var sourceOrg = new Organization
            {
                OrganizationId = sourceId,
                Name = "Outgoing",
                OrganizationType = officeType,
                OrganizationTypeId = officeType.OrganizationTypeId
            };

            var recipientOrg = new Organization
            {
                OrganizationId = recipientId,
                Name = "Incoming",
                OrganizationType = officeType,
                OrganizationTypeId = officeType.OrganizationTypeId
            };

            var moneyFlow = new MoneyFlow
            {
                SourceOrganizationId = sourceId,
                RecipientOrganizationId = recipientId,
                SourceOrganization = sourceOrg,
                RecipientOrganization = recipientOrg,
                SourceType = organizationType,
                SourceTypeId = organizationType.MoneyFlowSourceRecipientTypeId,
                RecipientType = organizationType,
                RecipientTypeId = organizationType.MoneyFlowSourceRecipientTypeId,

                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10
            };
            context.MoneyFlows.Add(moneyFlow);
            context.Organizations.Add(sourceOrg);
            context.Organizations.Add(recipientOrg);
            context.MoneyFlowTypes.Add(outgoing);
            context.MoneyFlowSourceRecipientTypes.Add(organizationType);
            context.MoneyFlowStatuses.Add(actual);

            var message = String.Format("The organization with the given id [{0}] is an office named [{1}].  This office must be accessed using office related methods only.",
                        sourceId,
                        sourceOrg.Name);
            Func<Task> f = () =>
            {
                return service.GetFiscalYearSummariesByOrganizationIdAsync(sourceOrg.OrganizationId);
            };
            service.Invoking(x => x.GetFiscalYearSummariesByOrganizationId(sourceOrg.OrganizationId)).ShouldThrow<BusinessSecurityException>().WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestGetFiscalYearSummariesByPersonId()
        {
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 10,
                FullName = "Full Name",
                Gender = gender,
                GenderId = gender.GenderId
            };
            var sourceId = 1;
            var recipientId = 2;
            var sourceParticipant = new Participant
            {
                ParticipantId = sourceId,
                Person = person,
                PersonId = person.PersonId
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "project"
            };
            var sourceTypeLookup = MoneyFlowSourceRecipientType.Participant;
            var recipientTypeLookup = MoneyFlowSourceRecipientType.Project;
            var sourceType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = sourceTypeLookup.Id,
                TypeName = sourceTypeLookup.Value
            };
            var recipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = recipientTypeLookup.Id,
                TypeName = recipientTypeLookup.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var moneyFlow = new MoneyFlow
            {
                SourceParticipantId = sourceId,
                RecipientProjectId = recipientId,
                SourceParticipant = sourceParticipant,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            context.Genders.Add(gender);
            context.MoneyFlows.Add(moneyFlow);
            context.Participants.Add(sourceParticipant);
            context.People.Add(person);
            context.Projects.Add(recipientProject);
            context.MoneyFlowStatuses.Add(actual);
            Action<List<FiscalYearSummaryDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Count);
            };
            var serviceResults = service.GetFiscalYearSummariesByPersonId(person.PersonId);
            var serviceResultsAsync = await service.GetFiscalYearSummariesByPersonIdAsync(person.PersonId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFiscalYearSummariesByPersonId_CheckMissingFiscalYearSummaries()
        {
            var gender = new Gender
            {
                GenderId = Gender.Male.Id,
                GenderName = Gender.Male.Value
            };
            var person = new Person
            {
                PersonId = 10,
                FullName = "Full Name",
                Gender = gender,
                GenderId = gender.GenderId
            };
            var sourceId = 1;
            var recipientId = 2;
            var sourceParticipant = new Participant
            {
                ParticipantId = sourceId,
                Person = person,
                PersonId = person.PersonId
            };
            var recipientProject = new Project
            {
                ProjectId = recipientId,
                Name = "project"
            };
            var sourceTypeLookup = MoneyFlowSourceRecipientType.Participant;
            var recipientTypeLookup = MoneyFlowSourceRecipientType.Project;
            var sourceType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = sourceTypeLookup.Id,
                TypeName = sourceTypeLookup.Value
            };
            var recipientType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = recipientTypeLookup.Id,
                TypeName = recipientTypeLookup.Value
            };
            var actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var moneyFlow = new MoneyFlow
            {
                SourceParticipantId = sourceId,
                RecipientProjectId = recipientId,
                SourceParticipant = sourceParticipant,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            var moneyFlow2 = new MoneyFlow
            {
                SourceParticipantId = sourceId,
                RecipientProjectId = recipientId,
                SourceParticipant = sourceParticipant,
                RecipientProject = recipientProject,
                SourceType = sourceType,
                SourceTypeId = sourceType.MoneyFlowSourceRecipientTypeId,
                RecipientType = recipientType,
                RecipientTypeId = recipientType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                Description = "desc",
                FiscalYear = moneyFlow.FiscalYear + 2,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            context.Genders.Add(gender);
            context.MoneyFlows.Add(moneyFlow);
            context.MoneyFlows.Add(moneyFlow2);
            context.Participants.Add(sourceParticipant);
            context.People.Add(person);
            context.Projects.Add(recipientProject);
            context.MoneyFlowStatuses.Add(actual);
            Action<List<FiscalYearSummaryDTO>> tester = (results) =>
            {
                Assert.AreEqual(3, results.Count);
                Assert.AreEqual(1, results.Where(x => x.FiscalYear == moneyFlow.FiscalYear + 1).Count());
            };
            var serviceResults = service.GetFiscalYearSummariesByPersonId(person.PersonId);
            var serviceResultsAsync = await service.GetFiscalYearSummariesByPersonIdAsync(person.PersonId);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        [TestMethod]
        public void TestGetMoneyFlowType()
        {
            var expectedMapping = new Dictionary<int, Type>();
            expectedMapping.Add(MoneyFlowSourceRecipientType.ItineraryStop.Id, typeof(ItineraryStop));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Office.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Organization.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Participant.Id, typeof(Participant));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Program.Id, typeof(Program));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Project.Id, typeof(Project));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Post.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Accomodation.Id, typeof(Accommodation));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Transportation.Id, typeof(Transportation));

            expectedMapping.Add(MoneyFlowSourceRecipientType.Expense.Id, null);

            var staticProperties = typeof(MoneyFlowSourceRecipientType).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allStaticLookups = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            foreach (var staticLookup in allStaticLookups)
            {
                Assert.IsTrue(expectedMapping.ContainsKey(staticLookup.Id), "The money flow source recipient type " + staticLookup.Value + " is not tested.");
            }

            foreach (var key in expectedMapping.Keys)
            {
                Assert.AreEqual(expectedMapping[key], service.GetMoneyFlowType(key), "The key " + key + " is not mapped.");
            }
        }

        [TestMethod]
        public void TestIsMoneyFlowType()
        {
            var expectedMapping = new Dictionary<int, Type>();
            expectedMapping.Add(MoneyFlowSourceRecipientType.ItineraryStop.Id, typeof(ItineraryStop));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Office.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Organization.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Participant.Id, typeof(Participant));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Program.Id, typeof(Program));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Project.Id, typeof(Project));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Post.Id, typeof(Organization));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Accomodation.Id, typeof(Accommodation));
            expectedMapping.Add(MoneyFlowSourceRecipientType.Transportation.Id, typeof(Transportation));

            expectedMapping.Add(MoneyFlowSourceRecipientType.Expense.Id, null);

            var staticProperties = typeof(MoneyFlowSourceRecipientType).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allStaticLookups = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            foreach (var staticLookup in allStaticLookups)
            {
                Assert.IsTrue(expectedMapping.ContainsKey(staticLookup.Id), "The money flow source recipient type " + staticLookup.Value + " is not tested.");
            }

            foreach (var key in expectedMapping.Keys)
            {
                var type = service.GetMoneyFlowType(key);
                var isExpectedToBeAType = type != null;
                Assert.AreEqual(isExpectedToBeAType, service.IsMoneyFlowType(key), "The type with key " + key + " did not have the expected IsIFundableType result " + isExpectedToBeAType);
            }
        }

        #region Delete
        [TestMethod]
        public async Task TestDelete()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var userId = 1;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            MoneyFlow moneyFlowToDelete = null;
            var entityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            context.SetupActions.Add(() =>
            {
                moneyFlowToDelete = new MoneyFlow
                {
                    MoneyFlowId = moneyFlowId,
                    Description = "old desc",
                    FiscalYear = 1900,
                    MoneyFlowStatusId = -1,
                    MoneyFlowTypeId = -1,
                    TransactionDate = lastWeek,
                    Value = -1.0m,
                    SourceProjectId = sourceEntityId,
                    SourceTypeId = entityTypeId
                };
                moneyFlowToDelete.History.CreatedBy = userId;
                moneyFlowToDelete.History.RevisedBy = userId;
                moneyFlowToDelete.History.CreatedOn = yesterday;
                moneyFlowToDelete.History.RevisedOn = yesterday;
                context.MoneyFlows.Add(moneyFlowToDelete);
            });
            Action beforeTester = () =>
            {
                Assert.AreEqual(1, context.MoneyFlows.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(0, context.MoneyFlows.Count());
            };
            var instance = new DeletedMoneyFlow(new User(userId), moneyFlowId, sourceEntityId, entityTypeId);

            context.Revert();
            beforeTester();
            service.Delete(instance);
            afterTester();

            context.Revert();
            beforeTester();
            await service.DeleteAsync(instance);
            afterTester();
        }

        [TestMethod]
        public async Task TestDelete_DifferentMoneyFlowId()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var userId = 1;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            MoneyFlow moneyFlowToDelete = null;
            MoneyFlow otherMoneyFlow = null;
            var entityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            context.SetupActions.Add(() =>
            {
                moneyFlowToDelete = new MoneyFlow
                {
                    MoneyFlowId = moneyFlowId,
                    Description = "old desc",
                    FiscalYear = 1900,
                    MoneyFlowStatusId = -1,
                    MoneyFlowTypeId = -1,
                    TransactionDate = lastWeek,
                    Value = -1.0m,
                    SourceProjectId = sourceEntityId,
                    SourceTypeId = entityTypeId
                };
                otherMoneyFlow = new MoneyFlow
                {
                    MoneyFlowId = moneyFlowId + 1,
                    Description = "old desc",
                    FiscalYear = 1900,
                    MoneyFlowStatusId = -1,
                    MoneyFlowTypeId = -1,
                    TransactionDate = lastWeek,
                    Value = -1.0m,
                    SourceProjectId = sourceEntityId + 1,
                    SourceTypeId = entityTypeId
                };
                moneyFlowToDelete.History.CreatedBy = userId;
                moneyFlowToDelete.History.RevisedBy = userId;
                moneyFlowToDelete.History.CreatedOn = yesterday;
                moneyFlowToDelete.History.RevisedOn = yesterday;

                otherMoneyFlow.History.CreatedBy = userId;
                otherMoneyFlow.History.RevisedBy = userId;
                otherMoneyFlow.History.CreatedOn = yesterday;
                otherMoneyFlow.History.RevisedOn = yesterday;
                context.MoneyFlows.Add(moneyFlowToDelete);
                context.MoneyFlows.Add(otherMoneyFlow);
            });
            Action beforeTester = () =>
            {
                Assert.AreEqual(2, context.MoneyFlows.Count());
            };
            Action afterTester = () =>
            {
                Assert.AreEqual(1, context.MoneyFlows.Count());
                Assert.IsTrue(Object.ReferenceEquals(otherMoneyFlow, context.MoneyFlows.First()));
            };
            var instance = new DeletedMoneyFlow(new User(userId), moneyFlowId, sourceEntityId, entityTypeId);

            context.Revert();
            beforeTester();
            service.Delete(instance);
            afterTester();

            context.Revert();
            beforeTester();
            await service.DeleteAsync(instance);
            afterTester();
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithSourceProjectIdDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceProjectId = -1,
                SourceTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, sourceEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithSourceProgramIdDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Program.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceProgramId = -1,
                SourceTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, sourceEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithSourceItineraryDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.ItineraryStop.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceItineraryStopId = -1,
                SourceTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, sourceEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithSourceOrganizationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Organization.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceOrganizationId = -1
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, sourceEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithSourceOfficeDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Office.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceOrganizationId = -1
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, sourceEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithSourceParticipantDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Participant.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceParticipantId = -1,
                SourceTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, sourceEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientAccommodationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Accomodation.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientAccommodationId = -1,
                RecipientTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientItineraryStopDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Accomodation.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientItineraryStopId = -1,
                RecipientTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientOrganizationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Organization.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientOrganizationId = -1,
                RecipientTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientOfficeDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Office.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientOrganizationId = -1,
                RecipientTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);
            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientParticipantDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Participant.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientParticipantId = -1,
                RecipientTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);

            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientProgramDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Program.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientProgramId = -1,
                RecipientTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);

            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientProjectDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientProjectId = -1,
                RecipientTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);

            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestDelete_MoneyFlowWithRecipientTransportationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Transportation.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientTransportationId = -1,
                RecipientTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);

            var deletedMoneyFlow = new DeletedMoneyFlow(new User(revisorId), moneyFlowId, recipientEntityId, entityTypeId);

            Func<Task> f = () =>
            {
                return service.DeleteAsync(deletedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Delete(deletedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }


        #endregion

        #region Update
        [TestMethod]
        public async Task TestUpdate_DoesNotHaveParent()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var creatorId = 1;
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            MoneyFlow moneyFlowToUpdate = null;
            var entityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            context.SetupActions.Add(() =>
            {
                moneyFlowToUpdate = new MoneyFlow
                {
                    MoneyFlowId = moneyFlowId,
                    Description = "old desc",
                    FiscalYear = 1900,
                    MoneyFlowStatusId = -1,
                    MoneyFlowTypeId = -1,
                    TransactionDate = lastWeek,
                    Value = -1.0m,
                    SourceProjectId = sourceEntityId,
                    SourceTypeId = entityTypeId
                };
                moneyFlowToUpdate.History.CreatedBy = creatorId;
                moneyFlowToUpdate.History.RevisedBy = creatorId;
                moneyFlowToUpdate.History.CreatedOn = yesterday;
                moneyFlowToUpdate.History.RevisedOn = yesterday;
                context.MoneyFlows.Add(moneyFlowToUpdate);
            });

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015,
                sourceOrRecipientEntityId: sourceEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId
                );

            Action tester = () =>
            {
                Assert.AreEqual(1, context.MoneyFlows.Count());
                Assert.AreEqual(updatedMoneyFlow.Description, moneyFlowToUpdate.Description);
                Assert.AreEqual(updatedMoneyFlow.Value, moneyFlowToUpdate.Value);
                Assert.AreEqual(updatedMoneyFlow.MoneyFlowStatusId, moneyFlowToUpdate.MoneyFlowStatusId);
                Assert.AreEqual(updatedMoneyFlow.TransactionDate, moneyFlowToUpdate.TransactionDate);
                Assert.AreEqual(updatedMoneyFlow.FiscalYear, moneyFlowToUpdate.FiscalYear);
                Assert.AreEqual(creatorId, moneyFlowToUpdate.History.CreatedBy);
                Assert.AreEqual(revisorId, moneyFlowToUpdate.History.RevisedBy);
                Assert.AreEqual(yesterday, moneyFlowToUpdate.History.CreatedOn);
                DateTimeOffset.UtcNow.Should().BeCloseTo(moneyFlowToUpdate.History.RevisedOn, 2000);
            };
            Action<MoneyFlowServiceUpdateValidationEntity> updateValidatorTester = (validationEntity) =>
            {
                Assert.IsNull(validationEntity.ParentMoneyFlowWithdrawlMaximum);
                Assert.AreEqual(updatedMoneyFlow.Description, validationEntity.Description);
                Assert.AreEqual(updatedMoneyFlow.Value, validationEntity.Value);
                Assert.AreEqual(updatedMoneyFlow.FiscalYear, validationEntity.FiscalYear);
            };
            validator.Setup(x => x.ValidateUpdate(It.IsAny<MoneyFlowServiceUpdateValidationEntity>())).Callback(updateValidatorTester);
            context.Revert();
            service.Update(updatedMoneyFlow);
            tester();
            validator.Verify(x => x.ValidateUpdate(It.IsAny<MoneyFlowServiceUpdateValidationEntity>()), Times.Once());


            context.Revert();
            await service.UpdateAsync(updatedMoneyFlow);
            tester();
            validator.Verify(x => x.ValidateUpdate(It.IsAny<MoneyFlowServiceUpdateValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestUpdate_HasParent()
        {
            using (ShimsContext.Create())
            {
                var maximum = 100m;
                ECA.Business.Service.Fundings.Fakes.ShimMoneyFlowService.AllInstances.GetMoneyFlowWithdrawalMaximumAsyncInt32 = (s, mfId) =>
                {
                    return Task.FromResult<decimal?>(maximum);
                };
                ECA.Business.Service.Fundings.Fakes.ShimMoneyFlowService.AllInstances.GetMoneyFlowWithdrawalMaximumInt32 = (s, mfId) =>
                {
                    return maximum;
                };
                var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
                var lastWeek = DateTime.UtcNow.AddDays(-7.0);
                var creatorId = 1;
                var revisorId = 2;
                var moneyFlowId = 1;
                var parentMoneyFlowId = 2;
                var sourceEntityId = 3;
                MoneyFlow moneyFlowToUpdate = null;
                MoneyFlow parentMoneyFlow = null;

                var parentDescription = "parent desc";
                var parentFiscalYear = -1;
                var parentMoneyFlowStatusId = -2;
                var value = -3.0m;

                var entityTypeId = MoneyFlowSourceRecipientType.Project.Id;
                context.SetupActions.Add(() =>
                {
                    parentMoneyFlow = new MoneyFlow
                    {
                        MoneyFlowId = parentMoneyFlowId,
                        Description = parentDescription,
                        FiscalYear = parentFiscalYear,
                        MoneyFlowStatusId = parentMoneyFlowStatusId,
                        TransactionDate = lastWeek,
                        Value = value
                    };
                    moneyFlowToUpdate = new MoneyFlow
                    {
                        MoneyFlowId = moneyFlowId,
                        Description = "old desc",
                        FiscalYear = 1900,
                        MoneyFlowStatusId = -1,
                        MoneyFlowTypeId = -1,
                        TransactionDate = lastWeek,
                        Value = -1.0m,
                        SourceProjectId = sourceEntityId,
                        SourceTypeId = entityTypeId,
                        Parent = parentMoneyFlow,
                        ParentMoneyFlowId = parentMoneyFlowId
                    };
                    moneyFlowToUpdate.History.CreatedBy = creatorId;
                    moneyFlowToUpdate.History.RevisedBy = creatorId;
                    moneyFlowToUpdate.History.CreatedOn = yesterday;
                    moneyFlowToUpdate.History.RevisedOn = yesterday;

                    parentMoneyFlow.History.CreatedBy = creatorId;
                    parentMoneyFlow.History.RevisedBy = creatorId;
                    parentMoneyFlow.History.CreatedOn = yesterday;
                    parentMoneyFlow.History.RevisedOn = yesterday;
                    context.MoneyFlows.Add(moneyFlowToUpdate);
                    context.MoneyFlows.Add(parentMoneyFlow);
                });

                var updatedMoneyFlow = new UpdatedMoneyFlow(
                    updator: new User(revisorId),
                    id: moneyFlowId,
                    description: "new description",
                    value: 10.00m,
                    moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                    transactionDate: DateTimeOffset.UtcNow,
                    fiscalYear: 2015,
                    sourceOrRecipientEntityId: sourceEntityId,
                    sourceOrRecipientEntityTypeId: entityTypeId
                    );

                Action tester = () =>
                {
                    Assert.AreEqual(2, context.MoneyFlows.Count());
                    Assert.AreEqual(parentDescription, parentMoneyFlow.Description);
                    Assert.AreEqual(value, parentMoneyFlow.Value);
                    Assert.AreEqual(parentMoneyFlowStatusId, parentMoneyFlow.MoneyFlowStatusId);
                    Assert.AreEqual(lastWeek, parentMoneyFlow.TransactionDate);
                    Assert.AreEqual(parentFiscalYear, parentMoneyFlow.FiscalYear);
                    Assert.AreEqual(creatorId, parentMoneyFlow.History.CreatedBy);
                    Assert.AreEqual(creatorId, parentMoneyFlow.History.RevisedBy);
                    Assert.AreEqual(yesterday, parentMoneyFlow.History.CreatedOn);
                    Assert.AreEqual(yesterday, parentMoneyFlow.History.RevisedOn);
                };
                Action<MoneyFlowServiceUpdateValidationEntity> updateValidatorTester = (validationEntity) =>
                {
                    Assert.AreEqual(maximum + moneyFlowToUpdate.Value, validationEntity.ParentMoneyFlowWithdrawlMaximum);
                    Assert.AreEqual(updatedMoneyFlow.Description, validationEntity.Description);
                    Assert.AreEqual(updatedMoneyFlow.Value, validationEntity.Value);
                    Assert.AreEqual(updatedMoneyFlow.FiscalYear, validationEntity.FiscalYear);
                };
                validator.Setup(x => x.ValidateUpdate(It.IsAny<MoneyFlowServiceUpdateValidationEntity>())).Callback(updateValidatorTester);

                context.Revert();
                service.Update(updatedMoneyFlow);
                tester();
                validator.Verify(x => x.ValidateUpdate(It.IsAny<MoneyFlowServiceUpdateValidationEntity>()), Times.Once());


                context.Revert();
                await service.UpdateAsync(updatedMoneyFlow);
                tester();
                validator.Verify(x => x.ValidateUpdate(It.IsAny<MoneyFlowServiceUpdateValidationEntity>()), Times.Exactly(2));
            }
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithGivenIdDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                sourceOrRecipientEntityTypeId: entityTypeId,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The money flow with id [{0}] and source entity id [{1}] was not found.", updatedMoneyFlow.Id, updatedMoneyFlow.SourceOrRecipientEntityId));
            f.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The money flow with id [{0}] and source entity id [{1}] was not found.", updatedMoneyFlow.Id, updatedMoneyFlow.SourceOrRecipientEntityId));
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithSourceProjectIdDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceProjectId = -1,
                SourceTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithSourceProgramIdDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Program.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceProgramId = -1,
                SourceTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithSourceItineraryDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.ItineraryStop.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceItineraryStopId = -1,
                SourceTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithSourceOrganizationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Organization.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceOrganizationId = -1,
                SourceTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithSourceOfficeDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Office.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceOrganizationId = -1,
                SourceTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithSourceParticipantDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var sourceEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Participant.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                SourceParticipantId = -1,
                SourceTypeId = entityTypeId
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: sourceEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientAccommodationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Accomodation.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientAccommodationId = -1,
                RecipientTypeId = entityTypeId,
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientItineraryStopDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.ItineraryStop.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientItineraryStopId = -1,
                RecipientTypeId = entityTypeId,
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientOrganizationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Organization.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientOrganizationId = -1,
                RecipientTypeId = entityTypeId,
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientOfficeDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Office.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientOrganizationId = -1,
                RecipientTypeId = entityTypeId,
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientParticipantDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Participant.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientParticipantId = -1,
                RecipientTypeId = entityTypeId,
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientProgramDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Program.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientProgramId = -1,
                RecipientTypeId = entityTypeId,
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientProjectDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientProjectId = -1,
                RecipientTypeId = entityTypeId,
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_MoneyFlowWithRecipientTransportationDoesNotExist()
        {
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var lastWeek = DateTime.UtcNow.AddDays(-7.0);
            var revisorId = 2;
            var moneyFlowId = 1;
            var recipientEntityId = 3;
            var entityTypeId = MoneyFlowSourceRecipientType.Transportation.Id;
            var moneyFlow = new MoneyFlow
            {
                MoneyFlowId = moneyFlowId,
                RecipientTransportationId = -1,
                RecipientTypeId = entityTypeId,
            };
            context.MoneyFlows.Add(moneyFlow);

            var updatedMoneyFlow = new UpdatedMoneyFlow(
                updator: new User(revisorId),
                sourceOrRecipientEntityId: recipientEntityId,
                sourceOrRecipientEntityTypeId: entityTypeId,
                id: moneyFlowId,
                description: "new description",
                value: 10.00m,
                moneyFlowStatusId: MoneyFlowStatus.Appropriated.Id,
                transactionDate: DateTimeOffset.UtcNow,
                fiscalYear: 2015
                );

            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedMoneyFlow);
            };
            var message = String.Format("The user with id [{0}] attempted edit a money flow with id [{1}] but should have been denied access.",
                        revisorId,
                        moneyFlow.MoneyFlowId);
            service.Invoking(x => x.Update(updatedMoneyFlow)).ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
            f.ShouldThrow<BusinessSecurityException>()
                .WithMessage(message);
        }
        #endregion
    }
}
