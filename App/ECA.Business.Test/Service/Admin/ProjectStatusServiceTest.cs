using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Core.Logging;
using System.Threading.Tasks;
using ECA.Data;
using System.Collections.Generic;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class ProjectStatusServiceTest
    {
        private TestEcaContext context;
        private ProjectStatusService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ProjectStatusService(context, new TraceLogger());
        }

        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var status = new ProjectStatus
            {
                ProjectStatusId= 1,
                Status = "f"
            };
            context.ProjectStatuses.Add(status);
            Action<PagedQueryResults<ProjectStatusDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(status.ProjectStatusId, firstResult.Id);
                Assert.AreEqual(status.Status, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<ProjectStatusDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ProjectStatusDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        
    }
}
