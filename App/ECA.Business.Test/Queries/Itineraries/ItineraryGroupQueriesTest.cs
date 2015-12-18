using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Itineraries;
using System.Collections.Generic;
using ECA.Business.Models.Itineraries;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;

namespace ECA.Business.Test.Queries.Itineraries
{
    [TestClass]
    public class ItineraryGroupQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestCreateGetItineraryGroupDTOQuery()
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
            var group = new ItineraryGroup
            {
                ItineraryGroupId = 3,
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group);


            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group);

            Action<List<ItineraryGroupDTO>> tester = (list) =>
            {
                Assert.AreEqual(1, list.Count);
                var firstResult = list.First();
                Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
                Assert.AreEqual(itinerary.ItineraryId, firstResult.ItineraryId);
                Assert.AreEqual(itinerary.Name, firstResult.ItineraryName);
                Assert.AreEqual(group.ItineraryGroupId, firstResult.ItineraryGroupId);
                Assert.AreEqual(group.Name, firstResult.ItineraryGroupName);
            };
            var results = ItineraryGroupQueries.CreateGetItineraryGroupDTOQuery(context).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetItineraryGroupDTOByItineraryIdQuery()
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
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);

            Action<List<ItineraryGroupDTO>> tester = (list) =>
            {
                Assert.AreEqual(1, list.Count);
                var firstResult = list.First();
                Assert.AreEqual(group1.ItineraryGroupId, firstResult.ItineraryGroupId);
            };
            var start = 0;
            var limit = 1;
            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(start, limit, defaultSorter);
            var results = ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(context, project.ProjectId, itinerary.ItineraryId, queryOperator).ToList();
            tester(results);
        }

        [TestMethod]
        public void TestCreateGetItineraryGroupDTOByItineraryIdQuery_ItineraryDoesNotExist()
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
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);

            Action<List<ItineraryGroupDTO>> tester = (list) =>
            {
                Assert.AreEqual(0, list.Count);
            };
            var start = 0;
            var limit = 1;
            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(start, limit, defaultSorter);
            var results = ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(context, project.ProjectId, itinerary.ItineraryId, queryOperator).ToList();
            Assert.AreEqual(1, results.Count);

            results = ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(context, project.ProjectId, 0, queryOperator).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestCreateGetItineraryGroupDTOByItineraryIdQuery_ProjectDoesNotExist()
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
                Name = "group",
                Itinerary = itinerary,
                ItineraryId = itinerary.ItineraryId
            };
            project.Itineraries.Add(itinerary);
            itinerary.ItineraryGroups.Add(group1);

            context.Projects.Add(project);
            context.Itineraries.Add(itinerary);
            context.ItineraryGroups.Add(group1);

            Action<List<ItineraryGroupDTO>> tester = (list) =>
            {
                Assert.AreEqual(0, list.Count);
            };
            var start = 0;
            var limit = 1;
            var defaultSorter = new ExpressionSorter<ItineraryGroupDTO>(x => x.ItineraryGroupName, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ItineraryGroupDTO>(start, limit, defaultSorter);
            var results = ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(context, project.ProjectId, itinerary.ItineraryId, queryOperator).ToList();
            Assert.AreEqual(1, results.Count);

            results = ItineraryGroupQueries.CreateGetItineraryGroupDTOByItineraryIdQuery(context, 0, itinerary.ItineraryId, queryOperator).ToList();
            Assert.AreEqual(0, results.Count);
        }
    }
}
