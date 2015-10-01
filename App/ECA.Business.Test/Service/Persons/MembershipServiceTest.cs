using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using System.Threading.Tasks;
using ECA.Data;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class MembershipServiceTest
    {
        private TestEcaContext context;
        private MembershipService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new MembershipService(context);
        }


        #region Get
        [TestMethod]
        public async Task TestGetMemberships_CheckProperties()
        {
            var membership = new Membership
            {
                MembershipId = 1,
                Name = "name"
            };
            context.Memberships.Add(membership);
            Action<PagedQueryResults<MembershipDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(membership.MembershipId, firstResult.Id);
                Assert.AreEqual(membership.Name, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<MembershipDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<MembershipDTO>(0, 10, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetMemberships_DefaultSorter()
        {
            var membership1 = new Membership
            {
                MembershipId = 1
            };
            var membership2 = new Membership
            {
                MembershipId = 2
            };
            context.Memberships.Add(membership1);
            context.Memberships.Add(membership2);
            Action<PagedQueryResults<MembershipDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(membership2.MembershipId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<MembershipDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<MembershipDTO>(0, 1, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetMemberships_Filter()
        {
            var membership1 = new Membership
            {
                MembershipId = 1,
            };
            var membership2 = new Membership
            {
                MembershipId = 2,
            };
            context.Memberships.Add(membership1);
            context.Memberships.Add(membership2);
            Action<PagedQueryResults<MembershipDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(membership1.MembershipId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<MembershipDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<MembershipDTO>(0, 1, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<MembershipDTO>(x => x.Id, ComparisonType.Equal, membership1.MembershipId));

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetMemberships_Sort()
        {
            var membership1 = new Membership
            {
                MembershipId = 1,
            };
            var membership2 = new Membership
            {
                MembershipId = 2,
            };
            context.Memberships.Add(membership2);
            context.Memberships.Add(membership1);

            Action<PagedQueryResults<MembershipDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(membership2.MembershipId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<MembershipDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<MembershipDTO>(0, 1, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<MembershipDTO>(x => x.Id, SortDirection.Descending));

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetMemberships_Paging()
        {
            var membership1 = new Membership
            {
                MembershipId = 1,
            };
            var membership2 = new Membership
            {
                MembershipId = 2,
            };
            context.Memberships.Add(membership2);
            context.Memberships.Add(membership1);

            Action<PagedQueryResults<MembershipDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
            };
            var defaultSorter = new ExpressionSorter<MembershipDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<MembershipDTO>(0, 1, defaultSorter);

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion
    }
}
