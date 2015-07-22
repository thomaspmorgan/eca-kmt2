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
    public class SocialMediaTypeServiceTest
    {
        private TestEcaContext context;
        private SocialMediaTypeService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new SocialMediaTypeService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }
        #region Get
        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var facebook = new SocialMediaType
            {
                SocialMediaTypeId = SocialMediaType.Facebook.Id,
                SocialMediaTypeName = SocialMediaType.Facebook.Value
            };

            context.SocialMediaTypes.Add(facebook);
            Action<PagedQueryResults<SocialMediaTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(facebook.SocialMediaTypeId, firstResult.Id);
                Assert.AreEqual(facebook.SocialMediaTypeName, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<SocialMediaTypeDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<SocialMediaTypeDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion
    }
}
