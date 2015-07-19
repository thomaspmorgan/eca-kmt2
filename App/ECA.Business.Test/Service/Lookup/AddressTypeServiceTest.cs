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
    public class AddressTypeServiceTest
    {
        private TestEcaContext context;
        private AddressTypeService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new AddressTypeService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }
        #region Get
        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var addressType = new AddressType
            {
                AddressName = AddressType.Business.Value,
                AddressTypeId = AddressType.Business.Id
            };

            context.AddressTypes.Add(addressType);
            Action<PagedQueryResults<AddressTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(addressType.AddressTypeId, firstResult.Id);
                Assert.AreEqual(addressType.AddressName, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<AddressTypeDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<AddressTypeDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion
    }
}
