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
            context = DbContextHelper.GetInMemoryContext();
            service = new GoalService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGetThemes_CheckProperties()
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

            var serviceResults = service.GetGoals(queryOperator);
            var serviceResultsAsync = await service.GetGoalsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_DefaultSorter()
        {
            var goal1 = new Goal
            {
                GoalId = 1,
                GoalName = "g1"
            };
            var goal2 = new Goal
            {
                GoalId = 2,
                GoalName = "g2"
            };
            context.Goals.Add(goal1);
            context.Goals.Add(goal2);
            Action<PagedQueryResults<GoalDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(goal2.GoalId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<GoalDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<GoalDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetGoals(queryOperator);
            var serviceResultsAsync = await service.GetGoalsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_Filter()
        {
            var goal1 = new Goal
            {
                GoalId = 1,
                GoalName = "g1"
            };
            var goal2 = new Goal
            {
                GoalId = 2,
                GoalName = "g2"
            };
            context.Goals.Add(goal1);
            context.Goals.Add(goal2);
            Action<PagedQueryResults<GoalDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(goal1.GoalId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<GoalDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<GoalDTO>(0, 1, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<GoalDTO>(x => x.Id, ComparisonType.Equal, goal1.GoalId));

            var serviceResults = service.GetGoals(queryOperator);
            var serviceResultsAsync = await service.GetGoalsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_Sort()
        {
            var goal1 = new Goal
            {
                GoalId = 1,
                GoalName = "g1"
            };
            var goal2 = new Goal
            {
                GoalId = 2,
                GoalName = "g2"
            };
            context.Goals.Add(goal2);
            context.Goals.Add(goal1);

            Action<PagedQueryResults<GoalDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(goal2.GoalId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<GoalDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<GoalDTO>(0, 1, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<GoalDTO>(x => x.Id, SortDirection.Descending));

            var serviceResults = service.GetGoals(queryOperator);
            var serviceResultsAsync = await service.GetGoalsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_Paging()
        {
            var goal1 = new Goal
            {
                GoalId = 1,
                GoalName = "g1"
            };
            var goal2 = new Goal
            {
                GoalId = 2,
                GoalName = "g2"
            };
            context.Goals.Add(goal2);
            context.Goals.Add(goal1);

            Action<PagedQueryResults<GoalDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
            };
            var defaultSorter = new ExpressionSorter<GoalDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<GoalDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetGoals(queryOperator);
            var serviceResultsAsync = await service.GetGoalsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Dispose
        [TestMethod]
        public void TestDispose_Context()
        {
            var testContext = DbContextHelper.GetInMemoryContext();
            var testService = new GoalService(testContext);

            var contextField = typeof(GoalService).GetField("context", BindingFlags.Instance | BindingFlags.NonPublic);
            var contextValue = contextField.GetValue(testService);
            Assert.IsNotNull(contextField);
            Assert.IsNotNull(contextValue);

            testService.Dispose();
            contextValue = contextField.GetValue(testService);
            Assert.IsNull(contextValue);
            Assert.IsTrue(testContext.IsDisposed);

        }
        #endregion
    }
}
