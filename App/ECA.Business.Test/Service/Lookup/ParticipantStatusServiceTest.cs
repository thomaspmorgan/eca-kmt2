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
    public class ParticipantStatusServiceTest
    {
        private TestEcaContext context;
        private ParticipantStatusService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ParticipantStatusService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGet_CheckProperties()
        {
            var participantStatus = new ParticipantStatus
            {
                Status = ParticipantStatus.Active.Value,
                ParticipantStatusId = ParticipantStatus.Active.Id,
            };

            context.ParticipantStatuses.Add(participantStatus);
            Action<PagedQueryResults<ParticipantStatusDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(participantStatus.ParticipantStatusId, firstResult.Id);
                Assert.AreEqual(participantStatus.Status, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<ParticipantStatusDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ParticipantStatusDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

    }
}
