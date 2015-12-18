using ECA.Business.Queries.Models.Itineraries;
using ECA.Business.Service.Itineraries;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Itineraries
{
    [TestClass]
    public class ItineraryGroupServiceTest
    {
        private TestEcaContext context;
        private ItineraryGroupService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ItineraryGroupService(context);
        }

        [TestMethod]
        public async Task TestGetItineraryGroupsByItineraryId_ProjectDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group1 = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group1",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            var group2 = new ItineraryGroup
            {
                ItineraryGroupId = 4,
                Name = "group2",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);
            itinerary.ItineraryGroups.Add(group2);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);
            context.ItineraryGroups.Add(group2);

            Action<PagedQueryResults<ItineraryGroupDTO>> beforeTester = (results) =>
            {
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(2, results.Total);
            };

            Action<PagedQueryResults<ItineraryGroupDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Results.Count);
                Assert.AreEqual(0, results.Total);
            };

            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetItineraryGroupsByItineraryId(project.ProjectId, itinerary.ItineraryId, queryOperator);
            var serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(project.ProjectId, itinerary.ItineraryId, queryOperator);
            beforeTester(serviceResults);
            beforeTester(serviceResultsAsync);

            serviceResults = service.GetItineraryGroupsByItineraryId(0, itinerary.ItineraryId, queryOperator);
            serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(0, itinerary.ItineraryId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetItineraryGroupsByItineraryId_ItineraryDoesNotExist()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group1 = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group1",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            var group2 = new ItineraryGroup
            {
                ItineraryGroupId = 4,
                Name = "group2",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);
            itinerary.ItineraryGroups.Add(group2);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);
            context.ItineraryGroups.Add(group2);

            Action<PagedQueryResults<ItineraryGroupDTO>> beforeTester = (results) =>
            {
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(2, results.Total);
            };

            Action<PagedQueryResults<ItineraryGroupDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Results.Count);
                Assert.AreEqual(0, results.Total);
            };

            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetItineraryGroupsByItineraryId(project.ProjectId, itinerary.ItineraryId, queryOperator);
            var serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(project.ProjectId, itinerary.ItineraryId, queryOperator);
            beforeTester(serviceResults);
            beforeTester(serviceResultsAsync);

            serviceResults = service.GetItineraryGroupsByItineraryId(project.ProjectId, 0, queryOperator);
            serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(project.ProjectId, 0, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetItineraryGroupsByItineraryId_Paged()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group1 = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group1",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            var group2 = new ItineraryGroup
            {
                ItineraryGroupId = 4,
                Name = "group2",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);
            itinerary.ItineraryGroups.Add(group2);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);
            context.ItineraryGroups.Add(group2);

            Action<PagedQueryResults<ItineraryGroupDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(group1.ItineraryGroupId, results.Results.First().ItineraryGroupId);
            };

            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetItineraryGroupsByItineraryId(project.ProjectId, itinerary.ItineraryId, queryOperator);
            var serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(project.ProjectId, itinerary.ItineraryId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetItineraryGroupsByItineraryId_Sorted()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group1 = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group1",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            var group2 = new ItineraryGroup
            {
                ItineraryGroupId = 4,
                Name = "group2",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);
            itinerary.ItineraryGroups.Add(group2);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);
            context.ItineraryGroups.Add(group2);

            Action<PagedQueryResults<ItineraryGroupDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Results.Count);
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(group1.ItineraryGroupId, results.Results.Last().ItineraryGroupId);
                Assert.AreEqual(group2.ItineraryGroupId, results.Results.First().ItineraryGroupId);
            };

            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var sorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(0, 10, defaultSorter, null, new List<ISorter> { sorter });
            var serviceResults = service.GetItineraryGroupsByItineraryId(project.ProjectId, itinerary.ItineraryId, queryOperator);
            var serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(project.ProjectId, itinerary.ItineraryId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetItineraryGroupsByItineraryId_Filtered()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var itinerary = new Itinerary
            {
                ItineraryId = 2,
                Name = "itinerary",
                Project = project,
                ProjectId = project.ProjectId
            };
            var group1 = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group1",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            var group2 = new ItineraryGroup
            {
                ItineraryGroupId = 4,
                Name = "group2",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);
            itinerary.ItineraryGroups.Add(group2);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);
            context.ItineraryGroups.Add(group2);

            Action<PagedQueryResults<ItineraryGroupDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(group1.ItineraryGroupId, results.Results.First().ItineraryGroupId);
            };

            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var filter = new ExpressionFilter<ItineraryGroupDTO>(x => x.ItineraryGroupName, ComparisonType.Equal, group1.Name);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(0, 10, defaultSorter, new List<IFilter> { filter }, null);
            var serviceResults = service.GetItineraryGroupsByItineraryId(project.ProjectId, itinerary.ItineraryId, queryOperator);
            var serviceResultsAsync = await service.GetItineraryGroupsByItineraryIdAsync(project.ProjectId, itinerary.ItineraryId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
    }
}
