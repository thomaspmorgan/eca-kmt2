using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Programs;
using System.Reflection;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;
using ECA.Business.Service.Admin;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class GoalServiceTest
    {
        private TestEcaContext context;
        private GoalService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new GoalService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGetGoals_CheckProperties()
        {
            var goal = new Goal
            {
                GoalId = 1,
                GoalName = "goal"
            };
            context.Goals.Add(goal);
            Action<PagedQueryResults<GoalDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(goal.GoalId, firstResult.Id);
                Assert.AreEqual(goal.GoalName, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<GoalDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<GoalDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        #endregion

    }
}
