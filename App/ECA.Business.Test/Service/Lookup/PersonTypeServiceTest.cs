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
    public class PersonTypeServiceTest
    {
        private TestEcaContext context;
        private PersonTypeService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new PersonTypeService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }
        #region Get
        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var personType = new PersonType
            {
                IsDependentPersonType = true,
                Name = "name",
                PersonTypeId = 1,
                SevisDependentTypeCode = "code"
            };

            context.PersonTypes.Add(personType);
            Action<PagedQueryResults<PersonTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(personType.PersonTypeId, firstResult.Id);
                Assert.AreEqual(personType.IsDependentPersonType, firstResult.IsDependentPersonType);
                Assert.AreEqual(personType.Name, firstResult.Name);
                Assert.AreEqual(personType.SevisDependentTypeCode, firstResult.SevisDependentTypeCode);
            };
            var defaultSorter = new ExpressionSorter<PersonTypeDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<PersonTypeDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);

        }
        #endregion
    }
}
