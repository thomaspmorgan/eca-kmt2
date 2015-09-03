using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Programs;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class ProgramStatusServiceTest
    {
        private TestEcaContext context;
        private ProgramStatusService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ProgramStatusService(context);
        }

        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var status = new ProgramStatus
            {
                ProgramStatusId = 1,
                Status = "f"
            };
            context.ProgramStatuses.Add(status);
            Action<PagedQueryResults<ProgramStatusDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(status.ProgramStatusId, firstResult.Id);
                Assert.AreEqual(status.Status, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<ProgramStatusDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ProgramStatusDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

    }
}
