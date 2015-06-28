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
    public class ParticipantTypeServiceTest
    {
        private TestEcaContext context;
        private ParticipantTypeService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ParticipantTypeService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var participantType = new ParticipantType
            {
                Name = ParticipantType.ForeignEducationalInstitution.Value,
                ParticipantTypeId = ParticipantType.ForeignEducationalInstitution.Id
            };

            context.ParticipantTypes.Add(participantType);
            Action<PagedQueryResults<ParticipantTypeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(participantType.ParticipantTypeId, firstResult.Id);
                Assert.AreEqual(participantType.Name, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<ParticipantTypeDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ParticipantTypeDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

    }
}
