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
    public class FocusCategoryServiceTest
    {
        private TestEcaContext context;
        private FocusCategoryService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new FocusCategoryService(context);
        }

        #region Get
        [TestMethod]
        public async Task TestGetFocusCategoriesByOfficeId_OfficeDoesNotExist()
        {
            var office = new Organization
            {
                OrganizationId = 1
            };

            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "focus1",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "focus2",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "cat1",
                Focus = focus1,
                FocusId = focus1.FocusId,

            };
            var category2 = new Category
            {
                CategoryId = 2,
                CategoryName = "cat2",
                Focus = focus2,
                FocusId = focus2.FocusId,
            };
            context.Organizations.Add(office);
            context.Foci.Add(focus1);
            context.Foci.Add(focus2);
            context.Categories.Add(category1);
            context.Categories.Add(category2);

            Action<PagedQueryResults<FocusCategoryDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Total);
                Assert.AreEqual(0, results.Results.Count);
            };

            var defaultSorter = new ExpressionSorter<FocusCategoryDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusCategoryDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetFocusCategoriesByOfficeId(office.OrganizationId - 1, queryOperator);
            var serviceResultsAsync = await service.GetFocusCategoriesByOfficeIdAsync(office.OrganizationId - 1, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFocusCategoriesByOfficeId_DefaultSort()
        {
            var office = new Organization
            {
                OrganizationId = 1
            };

            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "focus1",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "focus2",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "cat1",
                Focus = focus1,
                FocusId = focus1.FocusId,

            };
            var category2 = new Category
            {
                CategoryId = 2,
                CategoryName = "cat2",
                Focus = focus2,
                FocusId = focus2.FocusId,
            };
            context.Organizations.Add(office);
            context.Foci.Add(focus1);
            context.Foci.Add(focus2);
            context.Categories.Add(category1);
            context.Categories.Add(category2);

            Action<PagedQueryResults<FocusCategoryDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(2, results.Results.Count);
                Assert.AreEqual(category2.CategoryId, results.Results.First().Id);
                Assert.AreEqual(category1.CategoryId, results.Results.Last().Id);
            };

            var defaultSorter = new ExpressionSorter<FocusCategoryDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusCategoryDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetFocusCategoriesByOfficeId(office.OrganizationId, queryOperator);
            var serviceResultsAsync = await service.GetFocusCategoriesByOfficeIdAsync(office.OrganizationId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFocusCategoriesByOfficeId_Sort()
        {
            var office = new Organization
            {
                OrganizationId = 1
            };

            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "focus1",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "focus2",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "cat1",
                Focus = focus1,
                FocusId = focus1.FocusId,

            };
            var category2 = new Category
            {
                CategoryId = 2,
                CategoryName = "cat2",
                Focus = focus2,
                FocusId = focus2.FocusId,
            };
            context.Organizations.Add(office);
            context.Foci.Add(focus2);
            context.Foci.Add(focus1);
            context.Categories.Add(category2);
            context.Categories.Add(category1);            

            Action<PagedQueryResults<FocusCategoryDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(2, results.Results.Count);
                Assert.AreEqual(category2.CategoryId, results.Results.First().Id);
                Assert.AreEqual(category1.CategoryId, results.Results.Last().Id);
            };

            var defaultSorter = new ExpressionSorter<FocusCategoryDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusCategoryDTO>(0, 10, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<FocusCategoryDTO>(x => x.FocusName, SortDirection.Descending));
            var serviceResults = service.GetFocusCategoriesByOfficeId(office.OrganizationId, queryOperator);
            var serviceResultsAsync = await service.GetFocusCategoriesByOfficeIdAsync(office.OrganizationId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFocusCategoriesByOfficeId_Filtered()
        {
            var office = new Organization
            {
                OrganizationId = 1
            };

            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "focus1",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "focus2",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "cat1",
                Focus = focus1,
                FocusId = focus1.FocusId,

            };
            var category2 = new Category
            {
                CategoryId = 2,
                CategoryName = "cat2",
                Focus = focus2,
                FocusId = focus2.FocusId,
            };
            context.Organizations.Add(office);
            context.Foci.Add(focus2);
            context.Foci.Add(focus1);
            context.Categories.Add(category2);
            context.Categories.Add(category1);

            Action<PagedQueryResults<FocusCategoryDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(category2.CategoryId, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<FocusCategoryDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusCategoryDTO>(0, 10, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<FocusCategoryDTO>(x => x.Name, ComparisonType.Equal, category2.CategoryName));
            var serviceResults = service.GetFocusCategoriesByOfficeId(office.OrganizationId, queryOperator);
            var serviceResultsAsync = await service.GetFocusCategoriesByOfficeIdAsync(office.OrganizationId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFocusCategoriesByOfficeId_Paged()
        {
            var office = new Organization
            {
                OrganizationId = 1
            };

            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "focus1",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "focus2",
                Office = office,
                OfficeId = office.OrganizationId
            };
            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "cat1",
                Focus = focus1,
                FocusId = focus1.FocusId,

            };
            var category2 = new Category
            {
                CategoryId = 2,
                CategoryName = "cat2",
                Focus = focus2,
                FocusId = focus2.FocusId,
            };
            context.Organizations.Add(office);
            context.Foci.Add(focus2);
            context.Foci.Add(focus1);
            context.Categories.Add(category2);
            context.Categories.Add(category1);

            Action<PagedQueryResults<FocusCategoryDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                Assert.AreEqual(category2.CategoryId, results.Results.First().Id);
            };

            var defaultSorter = new ExpressionSorter<FocusCategoryDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusCategoryDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetFocusCategoriesByOfficeId(office.OrganizationId, queryOperator);
            var serviceResultsAsync = await service.GetFocusCategoriesByOfficeIdAsync(office.OrganizationId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFocusCategoriesByProgramId()
        {
            var program = new Program
            {
                ProgramId = 1
            };

            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "focus1",
            };
            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "cat1",
                Focus = focus1,
                FocusId = focus1.FocusId,

            };
            program.Categories.Add(category1);
            context.Programs.Add(program);
            context.Foci.Add(focus1);
            context.Categories.Add(category1);

            Action<PagedQueryResults<FocusCategoryDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var firstResult = results.Results.First();
                Assert.AreEqual(focus1.FocusName, firstResult.FocusName);
                Assert.AreEqual(focus1.FocusId, firstResult.Id);
                Assert.AreEqual(category1.CategoryName, firstResult.Name);
            };

            var defaultSorter = new ExpressionSorter<FocusCategoryDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusCategoryDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetFocusCategoriesByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetFocusCategoriesByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFocusCategoriesByProgramId_ProgramDoesExist()
        {
            var program = new Program
            {
                ProgramId = 1
            };

            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "focus1",
            };
            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "cat1",
                Focus = focus1,
                FocusId = focus1.FocusId,
            };
            program.Categories.Add(category1);
            context.Programs.Add(program);
            context.Foci.Add(focus1);
            context.Categories.Add(category1);

            Action<PagedQueryResults<FocusCategoryDTO>> tester = (results) =>
            {
                Assert.AreEqual(0, results.Total);
                Assert.AreEqual(0, results.Results.Count);
            };

            var defaultSorter = new ExpressionSorter<FocusCategoryDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusCategoryDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetFocusCategoriesByProgramId(program.ProgramId - 1, queryOperator);
            var serviceResultsAsync = await service.GetFocusCategoriesByProgramIdAsync(program.ProgramId - 1, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFocusCategoriesByProgramId_Sort()
        {
            var program = new Program
            {
                ProgramId = 1
            };

            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "focus1",
            };
            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "cat1",
                Focus = focus1,
                FocusId = focus1.FocusId,
            };

            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "focus2",
            };
            var category2 = new Category
            {
                CategoryId = 2,
                CategoryName = "cat2",
                Focus = focus2,
                FocusId = focus2.FocusId,
            };
            program.Categories.Add(category1);
            program.Categories.Add(category2);

            context.Programs.Add(program);
            context.Foci.Add(focus1);
            context.Categories.Add(category1);
            context.Foci.Add(focus2);
            context.Categories.Add(category2);

            Action<PagedQueryResults<FocusCategoryDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(2, results.Results.Count);
                var firstResult = results.Results.First();
                Assert.AreEqual(category1.CategoryName, firstResult.Name);
            };

            var defaultSorter = new ExpressionSorter<FocusCategoryDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusCategoryDTO>(0, 10, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<FocusCategoryDTO>(x => x.Name, SortDirection.Ascending));
            var serviceResults = service.GetFocusCategoriesByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetFocusCategoriesByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFocusCategoriesByProgramId_Filter()
        {
            var program = new Program
            {
                ProgramId = 1
            };

            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "focus1",
            };
            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "cat1",
                Focus = focus1,
                FocusId = focus1.FocusId,
            };

            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "focus2",
            };
            var category2 = new Category
            {
                CategoryId = 2,
                CategoryName = "cat2",
                Focus = focus2,
                FocusId = focus2.FocusId,
            };
            program.Categories.Add(category1);
            program.Categories.Add(category2);

            context.Programs.Add(program);
            context.Foci.Add(focus1);
            context.Categories.Add(category1);
            context.Foci.Add(focus2);
            context.Categories.Add(category2);

            Action<PagedQueryResults<FocusCategoryDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var firstResult = results.Results.First();
                Assert.AreEqual(category2.FocusId, firstResult.Id);
            };

            var defaultSorter = new ExpressionSorter<FocusCategoryDTO>(x => x.Name, SortDirection.Descending);
            var queryOperator = new QueryableOperator<FocusCategoryDTO>(0, 10, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<FocusCategoryDTO>(x => x.Name, ComparisonType.Equal, category2.CategoryName));
            var serviceResults = service.GetFocusCategoriesByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetFocusCategoriesByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFocusCategoriesByProgramId_DefaultSort()
        {
            var program = new Program
            {
                ProgramId = 1
            };

            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "focus1",
            };
            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "cat1",
                Focus = focus1,
                FocusId = focus1.FocusId,
            };

            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "focus2",
            };
            var category2 = new Category
            {
                CategoryId = 2,
                CategoryName = "cat2",
                Focus = focus2,
                FocusId = focus2.FocusId,
            };
            program.Categories.Add(category1);
            program.Categories.Add(category2);

            context.Programs.Add(program);
            context.Foci.Add(focus1);
            context.Categories.Add(category1);
            context.Foci.Add(focus2);
            context.Categories.Add(category2);

            Action<PagedQueryResults<FocusCategoryDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(2, results.Results.Count);
                var firstResult = results.Results.First();
                Assert.AreEqual(category1.FocusId, firstResult.Id);
            };

            var defaultSorter = new ExpressionSorter<FocusCategoryDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<FocusCategoryDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetFocusCategoriesByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetFocusCategoriesByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetFocusCategoriesByProgramId_Paging()
        {
            var program = new Program
            {
                ProgramId = 1
            };

            var focus1 = new Focus
            {
                FocusId = 1,
                FocusName = "focus1",
            };
            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "cat1",
                Focus = focus1,
                FocusId = focus1.FocusId,
            };

            var focus2 = new Focus
            {
                FocusId = 2,
                FocusName = "focus2",
            };
            var category2 = new Category
            {
                CategoryId = 2,
                CategoryName = "cat2",
                Focus = focus2,
                FocusId = focus2.FocusId,
            };
            program.Categories.Add(category1);
            program.Categories.Add(category2);

            context.Programs.Add(program);
            context.Foci.Add(focus1);
            context.Categories.Add(category1);
            context.Foci.Add(focus2);
            context.Categories.Add(category2);

            Action<PagedQueryResults<FocusCategoryDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
                var firstResult = results.Results.First();
                Assert.AreEqual(category1.FocusId, firstResult.Id);
            };

            var defaultSorter = new ExpressionSorter<FocusCategoryDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<FocusCategoryDTO>(0, 1, defaultSorter);
            var serviceResults = service.GetFocusCategoriesByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetFocusCategoriesByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion
    }
}
