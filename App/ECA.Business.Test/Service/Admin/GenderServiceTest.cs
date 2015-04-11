using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Core.Logging;
using ECA.Data;
using ECA.Core.Query;
using ECA.Business.Service.Lookup;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq;
using System.Linq;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class GenderServiceTest
    {
        private TestEcaContext context;
        private GenderService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new GenderService(context, new TraceLogger());
        }

        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "genderName"
            };

            context.Genders.Add(gender);

            Action<PagedQueryResults<SimpleLookupDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(gender.GenderId, firstResult.Id);
                Assert.AreEqual(gender.GenderName, firstResult.Value);
            };

            var defaultSorter = new ExpressionSorter<SimpleLookupDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SimpleLookupDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
        }
    }
}
