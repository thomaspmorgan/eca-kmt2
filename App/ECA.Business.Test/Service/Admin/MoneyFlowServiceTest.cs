using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class MoneyFlowServiceTest
    {
        private TestEcaContext context;
        private MoneyFlowService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new MoneyFlowService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get MoneyFlows By Project Id
        [TestMethod]
        public async Task TestGetMoneyFlowsByProjectId_CheckProperties()
        {
            var program = new Program
            {
                ProgramId = 2,
                Name = "program",
                Description = "description",
                Owner = new Organization(),
                Regions = new HashSet<Location>(),
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

            var moneyflow = new MoneyFlow
            {
                MoneyFlowId = 3,
                MoneyFlowTypeId = 4,
                Value = 50,
                TransactionDate = DateTimeOffset.Now,
                FiscalYear = 2015,
                SourceProgramId = 2,
                SourceProgram = program,
                RecipientProjectId = 1,
                SourceTypeId = 1,
                SourceType = sourceType,
                RecipientTypeId = 2,
                RecipientType = recipientType,
                Description = "description"
            };

            program.Projects.Add(project);

            context.Projects.Add(project);
            context.Programs.Add(program);
            context.MoneyFlowSourceRecipientTypes.Add(sourceType);
            context.MoneyFlowSourceRecipientTypes.Add(recipientType);
            context.MoneyFlows.Add(moneyflow);

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

                Assert.AreEqual(moneyflow.TransactionDate, firstResult.TransactionDate);
                Assert.AreEqual(moneyflow.SourceType.TypeName, firstResult.Type);
                Assert.AreEqual(program.Name, firstResult.FromTo);
                Assert.AreEqual(moneyflow.Value, firstResult.Amount);
                Assert.AreEqual(moneyflow.Description, firstResult.Description);
            };

            var result = service.GetMoneyFlowsByProjectId(project.ProjectId, queryOperator);
            var resultAsync = await service.GetMoneyFlowsByProjectIdAsync(project.ProjectId, queryOperator);

            tester(result);
            tester(resultAsync);

        }
        #endregion
    }
} 