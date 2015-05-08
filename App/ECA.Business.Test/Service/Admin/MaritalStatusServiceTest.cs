using ECA.Business.Service.Admin;
using ECA.Business.Service.Lookup;
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
    public class MaritalStatusServiceTest
    {
        private TestEcaContext context;
        private MaritalStatusService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new MaritalStatusService(context);
        }
        
        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var maritalStatus = new MaritalStatus
            {
                MaritalStatusId = 1,
                Status = "Single"
            };

            context.MaritalStatuses.Add(maritalStatus);

            Action<PagedQueryResults<SimpleLookupDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(maritalStatus.MaritalStatusId, firstResult.Id);
                Assert.AreEqual(maritalStatus.Status, firstResult.Value);
            };

            var defaultSorter = new ExpressionSorter<SimpleLookupDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleLookupDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
        }
    }
}
