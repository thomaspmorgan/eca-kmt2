using ECA.Business.Queries.Models.Lookup;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Lookup
{
    [TestClass]
    public class OrganizationTypeServiceTest
    {
        private TestEcaContext context;
        private OrganizationTypeService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new OrganizationTypeService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGetOrganizationTypes_CheckProperties()
        {
            var orgType = new OrganizationType
            {
                OrganizationTypeId = 1,
                OrganizationTypeName = "theme"
            };
            context.OrganizationTypes.Add(orgType);
            Action<PagedQueryResults<OrganizationTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(orgType.OrganizationTypeId, firstResult.Id);
                Assert.AreEqual(orgType.OrganizationTypeName, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<OrganizationTypeDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<OrganizationTypeDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

    }
}
