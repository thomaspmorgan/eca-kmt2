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
    public class FocusServiceTest
    {
        private TestEcaContext context;
        private FocusService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new FocusService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGetFocusById()
        {
            var focus = new Focus
            {
                FocusId = 1,
                FocusName = "focus"
            };
            context.Foci.Add(focus);
            Action<FocusDTO> tester = (result) =>
            {
                Assert.IsNotNull(result);
                Assert.AreEqual(focus.FocusId, result.Id);
                Assert.AreEqual(focus.FocusName, result.Name);
            };

            var serviceResult = service.GetFocusById(focus.FocusId);
            var serviceResultAsync = await service.GetFocusByIdAsync(focus.FocusId);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGetFocusById_FocusDoesNotExist()
        {
            Action<FocusDTO> tester = (result) =>
            {
                Assert.AreEqual(0, context.Foci.Count());
                Assert.IsNull(result);
                
            };

            var serviceResult = service.GetFocusById(1);
            var serviceResultAsync = await service.GetFocusByIdAsync(1);
            tester(serviceResult);
            tester(serviceResultAsync);
        }

        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var focus = new Focus
            {
                FocusId = 1,
                FocusName = "f"
            };
            context.Foci.Add(focus);
            Action<PagedQueryResults<FocusDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(focus.FocusId, firstResult.Id);
                Assert.AreEqual(focus.FocusName, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<FocusDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<FocusDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

    }
}
