using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq;
using ECA.Data;
using System.Collections.Generic;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class JustificationObjectiveServiceTest
    {
        private TestEcaContext context;
        private JustificationObjectiveService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new JustificationObjectiveService(context);
        }

        #region Get
        [TestMethod]
        public async Task TestGetJustificationObjectivesByOfficeId_OfficeDoesNotExist()
        {
            var office = new Organization
            {
                OrganizationId = 1
            };

            var justification1 = new Justification
            {
                JustificationId = 1,
                JustificationName = "just1",
                Office = office,
                OfficeId = office.OrganizationId
                
            };
            var justification2 = new Justification
            {
                JustificationId = 2,
                JustificationName = "just2",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var objective1 = new Objective
            {
                Justification = justification1,
                JustificationId = justification1.JustificationId,
                ObjectiveId = 1,
                ObjectiveName = "obj1",
                
            };
            var objective2 = new Objective
            {
                Justification = justification1,
                JustificationId = justification1.JustificationId,
                ObjectiveId = 2,
                ObjectiveName = "obj2",
            };
            
            context.Organizations.Add(office);
            context.Objectives.Add(objective1);
            context.Objectives.Add(objective2);
            context.Justifications.Add(justification1);
            context.Justifications.Add(justification2);

            Action<PagedQueryResults<JustificationObjectiveDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Total);
                Assert.AreEqual(0, results.Results.Count);
            };

            var defaultSorter = new ExpressionSorter<JustificationObjectiveDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<JustificationObjectiveDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetJustificationObjectivesByOfficeId(office.OrganizationId - 1, queryOperator);
            var serviceResultsAsync = await service.GetJustificationObjectivesByOfficeIdAsync(office.OrganizationId - 1, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetJustificationObjectivesByOfficeId_DefaultSort()
        {
            var office = new Organization
            {
                OrganizationId = 1
            };

            var justification1 = new Justification
            {
                JustificationId = 1,
                JustificationName = "just1",
                Office = office,
                OfficeId = office.OrganizationId

            };
            var justification2 = new Justification
            {
                JustificationId = 2,
                JustificationName = "just2",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var objective1 = new Objective
            {
                Justification = justification1,
                JustificationId = justification1.JustificationId,
                ObjectiveId = 1,
                ObjectiveName = "obj1",

            };
            var objective2 = new Objective
            {
                Justification = justification1,
                JustificationId = justification1.JustificationId,
                ObjectiveId = 2,
                ObjectiveName = "obj2",
            };

            context.Organizations.Add(office);
            context.Objectives.Add(objective1);
            context.Objectives.Add(objective2);
            context.Justifications.Add(justification1);
            context.Justifications.Add(justification2);

            Action<PagedQueryResults<JustificationObjectiveDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(2, results.Results.Count);
                Assert.AreEqual(objective2.ObjectiveId, results.Results.First().Id);
                Assert.AreEqual(objective1.ObjectiveId, results.Results.Last().Id);
            };

            var defaultSorter = new ExpressionSorter<JustificationObjectiveDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<JustificationObjectiveDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetJustificationObjectivesByOfficeId(office.OrganizationId, queryOperator);
            var serviceResultsAsync = await service.GetJustificationObjectivesByOfficeIdAsync(office.OrganizationId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetJustificationObjectivesByOfficeId_Sort()
        {
            var office = new Organization
            {
                OrganizationId = 1
            };

            var justification1 = new Justification
            {
                JustificationId = 1,
                JustificationName = "just1",
                Office = office,
                OfficeId = office.OrganizationId

            };
            var justification2 = new Justification
            {
                JustificationId = 2,
                JustificationName = "just2",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var objective1 = new Objective
            {
                Justification = justification1,
                JustificationId = justification1.JustificationId,
                ObjectiveId = 1,
                ObjectiveName = "obj1",

            };
            var objective2 = new Objective
            {
                Justification = justification1,
                JustificationId = justification1.JustificationId,
                ObjectiveId = 2,
                ObjectiveName = "obj2",
            };

            context.Organizations.Add(office);
            context.Objectives.Add(objective1);
            context.Objectives.Add(objective2);
            context.Justifications.Add(justification1);
            context.Justifications.Add(justification2);

            Action<PagedQueryResults<JustificationObjectiveDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(2, results.Results.Count);
                Assert.AreEqual(objective1.ObjectiveId, results.Results.First().Id);
                Assert.AreEqual(objective2.ObjectiveId, results.Results.Last().Id);
            };

            var defaultSorter = new ExpressionSorter<JustificationObjectiveDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<JustificationObjectiveDTO>(0, 10, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<JustificationObjectiveDTO>(x => x.JustificationName, SortDirection.Descending));
            var serviceResults = service.GetJustificationObjectivesByOfficeId(office.OrganizationId, queryOperator);
            var serviceResultsAsync = await service.GetJustificationObjectivesByOfficeIdAsync(office.OrganizationId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetJustificationObjectivesByOfficeId_Filtered()
        {
            var office = new Organization
            {
                OrganizationId = 1
            };

            var justification1 = new Justification
            {
                JustificationId = 1,
                JustificationName = "just1",
                Office = office,
                OfficeId = office.OrganizationId

            };
            var justification2 = new Justification
            {
                JustificationId = 2,
                JustificationName = "just2",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var objective1 = new Objective
            {
                Justification = justification1,
                JustificationId = justification1.JustificationId,
                ObjectiveId = 1,
                ObjectiveName = "obj1",

            };
            var objective2 = new Objective
            {
                Justification = justification1,
                JustificationId = justification1.JustificationId,
                ObjectiveId = 2,
                ObjectiveName = "obj2",
            };

            context.Organizations.Add(office);
            context.Objectives.Add(objective1);
            context.Objectives.Add(objective2);
            context.Justifications.Add(justification1);
            context.Justifications.Add(justification2);

            Action<PagedQueryResults<JustificationObjectiveDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(objective1.ObjectiveId, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<JustificationObjectiveDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<JustificationObjectiveDTO>(0, 10, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<JustificationObjectiveDTO>(x => x.Name, ComparisonType.Equal, objective1.ObjectiveName));
            var serviceResults = service.GetJustificationObjectivesByOfficeId(office.OrganizationId, queryOperator);
            var serviceResultsAsync = await service.GetJustificationObjectivesByOfficeIdAsync(office.OrganizationId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetJustificationObjectivesByOfficeId_Paged()
        {
            var office = new Organization
            {
                OrganizationId = 1
            };

            var justification1 = new Justification
            {
                JustificationId = 1,
                JustificationName = "just1",
                Office = office,
                OfficeId = office.OrganizationId

            };
            var justification2 = new Justification
            {
                JustificationId = 2,
                JustificationName = "just2",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var objective1 = new Objective
            {
                Justification = justification1,
                JustificationId = justification1.JustificationId,
                ObjectiveId = 1,
                ObjectiveName = "obj1",

            };
            var objective2 = new Objective
            {
                Justification = justification1,
                JustificationId = justification1.JustificationId,
                ObjectiveId = 2,
                ObjectiveName = "obj2",
            };

            context.Organizations.Add(office);
            context.Objectives.Add(objective1);
            context.Objectives.Add(objective2);
            context.Justifications.Add(justification1);
            context.Justifications.Add(justification2);

            Action<PagedQueryResults<JustificationObjectiveDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(objective1.ObjectiveId, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<JustificationObjectiveDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<JustificationObjectiveDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetJustificationObjectivesByOfficeId(office.OrganizationId, queryOperator);
            var serviceResultsAsync = await service.GetJustificationObjectivesByOfficeIdAsync(office.OrganizationId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion
    }
}
