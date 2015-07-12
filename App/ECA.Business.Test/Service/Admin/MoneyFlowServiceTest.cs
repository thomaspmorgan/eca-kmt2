using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class MoneyFlowServiceTest
    {
        private TestEcaContext context;
        private MoneyFlowService service;
        private Mock<IBusinessValidator<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity>> validator;
        private Mock<IOfficeService> officeService;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            validator = new Mock<IBusinessValidator<MoneyFlowServiceCreateValidationEntity, MoneyFlowServiceUpdateValidationEntity>>();
            officeService = new Mock<IOfficeService>();
            service = new MoneyFlowService(context, officeService.Object, validator.Object);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get MoneyFlows By Project Id
        [TestMethod]
        public async Task TestGetMoneyFlowsByProjectId_CheckProperties()
        {
            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };
            var owner1 = new Organization
            {
                OrganizationId = 1,
                Name = "owner 1",
                OfficeSymbol = "owner 1 symbol",
            };

            var program = new Program
            {
                ProgramId = 5,
                Description = "desc1",
                History = new History
                {
                    CreatedBy = 1,
                },
                Name = "program 1",
                Owner = owner1,
                OwnerId = owner1.OrganizationId,
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId,
            };

            var project = new Project
            {
                ProjectId = 1,
                Name = "project",
                Description = "description",
                ProjectStatusId = 1,
                StartDate = DateTimeOffset.Now,
                ProgramId = 2,
                ParentProgram = program

            };

            var sourceType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 1,
                TypeName = "program"
            };

            var recipientType = new MoneyFlowSourceRecipientType
            {

                MoneyFlowSourceRecipientTypeId = 2,
                TypeName = "project"
            };

            var moneyFlowType = new MoneyFlowType {
                MoneyFlowTypeId = 1,
                MoneyFlowTypeName = "program",
            };

            var moneyFlowStatus = new MoneyFlowStatus
            {
                MoneyFlowStatusId = 1,
                MoneyFlowStatusName = "test status",
            };
            var moneyFlow1 = new MoneyFlow
            {
                MoneyFlowId = 1,
                Value = 50,
                RecipientProjectId = 1,
                TransactionDate = new DateTimeOffset(),
                MoneyFlowStatus = moneyFlowStatus,
                MoneyFlowStatusId = 1,
                RecipientProject = project,
                SourceProgramId = 5,
                SourceProgram = program,
                SourceType = sourceType,
                RecipientType = recipientType,
                SourceTypeId= 1,
                RecipientTypeId = 2,
                Description = "moneyFlow",
                MoneyFlowType = moneyFlowType,
                MoneyFlowTypeId = 1
            };

            program.Projects.Add(project);

            context.Projects.Add(project);
            context.Programs.Add(program);
            context.MoneyFlowSourceRecipientTypes.Add(sourceType);
            context.MoneyFlowSourceRecipientTypes.Add(recipientType);
            context.MoneyFlowTypes.Add(moneyFlowType);
            context.MoneyFlowStatuses.Add(moneyFlowStatus);
            context.MoneyFlows.Add(moneyFlow1);

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.TransactionDate, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<MoneyFlowDTO>> tester = (serviceResult) =>
            {
                Assert.AreEqual(1, serviceResult.Total);
                var results = serviceResult.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();

                Assert.AreEqual(moneyFlow1.TransactionDate, firstResult.TransactionDate);
                Assert.AreEqual(moneyFlow1.SourceType.TypeName, firstResult.Type);
                Assert.AreEqual(program.Name, firstResult.FromTo);
                Assert.AreEqual(moneyFlow1.Value, firstResult.Amount);
                Assert.AreEqual(moneyFlow1.Description, firstResult.Description);
            };

            var result = service.GetMoneyFlowsByProjectId(project.ProjectId, queryOperator);
            var resultAsync = await service.GetMoneyFlowsByProjectIdAsync(project.ProjectId, queryOperator);

            tester(result);
            tester(resultAsync);

        }
        #endregion

        #region Get MoneyFlows By Program Id
        [TestMethod]
        public async Task TestGetMoneyFlowsByProgramId_CheckProperties()
        {
            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };
            var owner1 = new Organization
            {
                OrganizationId = 1,
                Name = "owner 1",
                OfficeSymbol = "owner 1 symbol",
            };

            var program = new Program
            {
                ProgramId = 5,
                Description = "desc1",
                History = new History
                {
                    CreatedBy = 1,
                },
                Name = "program 1",
                Owner = owner1,
                OwnerId = owner1.OrganizationId,
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId,
            };

            var project = new Project
            {
                ProjectId = 1,
                Name = "project",
                Description = "description",
                ProjectStatusId = 1,
                StartDate = DateTimeOffset.Now,
                ProgramId = 2,
                ParentProgram = program

            };

            var sourceType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = 1,
                TypeName = "program"
            };

            var recipientType = new MoneyFlowSourceRecipientType
            {

                MoneyFlowSourceRecipientTypeId = 2,
                TypeName = "project"
            };

            var moneyFlowType = new MoneyFlowType
            {
                MoneyFlowTypeId = 1,
                MoneyFlowTypeName = "program",
            };

            var moneyFlowStatus = new MoneyFlowStatus
            {
                MoneyFlowStatusId = 1,
                MoneyFlowStatusName = "test status",
            };
            var moneyFlow1 = new MoneyFlow
            {
                MoneyFlowId = 1,
                Value = 50,
                RecipientProjectId = 1,
                TransactionDate = new DateTimeOffset(),
                MoneyFlowStatus = moneyFlowStatus,
                MoneyFlowStatusId = 1,
                RecipientProject = project,
                SourceProgramId = 5,
                SourceProgram = program,
                SourceType = sourceType,
                RecipientType = recipientType,
                SourceTypeId = 1,
                RecipientTypeId = 2,
                Description = "moneyFlow",
                MoneyFlowType = moneyFlowType,
                MoneyFlowTypeId = 1
            };

            program.Projects.Add(project);

            context.Projects.Add(project);
            context.Programs.Add(program);
            context.MoneyFlowSourceRecipientTypes.Add(sourceType);
            context.MoneyFlowSourceRecipientTypes.Add(recipientType);
            context.MoneyFlowTypes.Add(moneyFlowType);
            context.MoneyFlowStatuses.Add(moneyFlowStatus);
            context.MoneyFlows.Add(moneyFlow1);

            var defaultSorter = new ExpressionSorter<MoneyFlowDTO>(x => x.TransactionDate, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<MoneyFlowDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<MoneyFlowDTO>> tester = (serviceResult) =>
            {
                Assert.AreEqual(1, serviceResult.Total);
                var results = serviceResult.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();

                Assert.AreEqual(moneyFlow1.TransactionDate, firstResult.TransactionDate);
                Assert.AreEqual(moneyFlow1.SourceType.TypeName, firstResult.Type);
                Assert.AreEqual(project.Name, firstResult.FromTo);
                Assert.AreEqual(moneyFlow1.Value, firstResult.Amount);
                Assert.AreEqual(moneyFlow1.Description, firstResult.Description);
            };

            var result = service.GetMoneyFlowsByProgramId(program.ProgramId, queryOperator);
            var resultAsync = await service.GetMoneyFlowsByProgramIdAsync(program.ProgramId, queryOperator);

            tester(result);
            tester(resultAsync);

        }
        #endregion
    }
} 