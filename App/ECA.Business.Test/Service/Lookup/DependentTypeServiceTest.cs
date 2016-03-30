using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Lookup;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;

namespace ECA.Business.Test.Service.Lookup
{
    [TestClass]
    public class DependentTypeServiceTest
    {
        private TestEcaContext context;
        private DependentTypeService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new DependentTypeService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var dependentType = new DependentType
            {
                Name = "name",
                DependentTypeId = 1,
                SevisDependentTypeCode = "code"
            };

            context.DependentTypes.Add(dependentType);
            Action<PagedQueryResults<DependentTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(dependentType.DependentTypeId, firstResult.Id);
                Assert.AreEqual(dependentType.Name, firstResult.Name);
                Assert.AreEqual(dependentType.SevisDependentTypeCode, firstResult.SevisDependentTypeCode);
            };
            var defaultSorter = new ExpressionSorter<DependentTypeDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<DependentTypeDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion
    }
}
