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
    public class EmailAddressTypeServiceTest
    {
        private TestEcaContext context;
        private EmailAddressTypeService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new EmailAddressTypeService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }
        #region Get
        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var business = new EmailAddressType
            {
                EmailAddressTypeId = EmailAddressType.Business.Id,
                EmailAddressTypeName = EmailAddressType.Business.Value
            };

            context.EmailAddressTypes.Add(business);
            Action<PagedQueryResults<EmailAddressTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(business.EmailAddressTypeId, firstResult.Id);
                Assert.AreEqual(business.EmailAddressTypeName, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<EmailAddressTypeDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<EmailAddressTypeDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion
    }
}