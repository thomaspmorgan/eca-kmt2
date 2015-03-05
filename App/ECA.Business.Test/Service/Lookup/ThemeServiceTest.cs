using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Logging;
using ECA.Core.Query;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Lookup
{
    [TestClass]
    public class ThemeServiceTest
    {
        private TestEcaContext context;
        private ThemeService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ThemeService(context, new TraceLogger());
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Get
        [TestMethod]
        public async Task TestGetThemes_CheckProperties()
        {
            var theme = new Theme
            {
                ThemeId = 1,
                ThemeName = "theme"
            };
            context.Themes.Add(theme);
            Action<PagedQueryResults<ThemeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(theme.ThemeId, firstResult.Id);
                Assert.AreEqual(theme.ThemeName, firstResult.Name);
            };
            var defaultSorter = new ExpressionSorter<ThemeDTO>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ThemeDTO>(0, 10, defaultSorter);

            var serviceResults = service.GetThemes(queryOperator);
            var serviceResultsAsync = await service.GetThemesAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_DefaultSorter()
        {
            var theme1 = new Theme
            {
                ThemeId = 1,
                ThemeName = "theme"
            };
            var theme2 = new Theme
            {
                ThemeId = 2,
                ThemeName = "theme2"
            };
            context.Themes.Add(theme1);
            context.Themes.Add(theme2);
            Action<PagedQueryResults<ThemeDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(theme2.ThemeId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<ThemeDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ThemeDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetThemes(queryOperator);
            var serviceResultsAsync = await service.GetThemesAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_Filter()
        {
            var theme1 = new Theme
            {
                ThemeId = 1,
                ThemeName = "theme"
            };
            var theme2 = new Theme
            {
                ThemeId = 2,
                ThemeName = "theme2"
            };
            context.Themes.Add(theme1);
            context.Themes.Add(theme2);
            Action<PagedQueryResults<ThemeDTO>> tester = (results) =>
            {
                Assert.AreEqual(1, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(theme1.ThemeId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<ThemeDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ThemeDTO>(0, 1, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<ThemeDTO>(x => x.Id, ComparisonType.Equal, theme1.ThemeId));

            var serviceResults = service.GetThemes(queryOperator);
            var serviceResultsAsync = await service.GetThemesAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_Sort()
        {
            var theme1 = new Theme
            {
                ThemeId = 1,
                ThemeName = "theme"
            };
            var theme2 = new Theme
            {
                ThemeId = 2,
                ThemeName = "theme2"
            };
            context.Themes.Add(theme2);
            context.Themes.Add(theme1);
            
            Action<PagedQueryResults<ThemeDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);

                var firstResult = results.Results.First();
                Assert.AreEqual(theme2.ThemeId, firstResult.Id);
            };
            var defaultSorter = new ExpressionSorter<ThemeDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ThemeDTO>(0, 1, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<ThemeDTO>(x => x.Id, SortDirection.Descending));

            var serviceResults = service.GetThemes(queryOperator);
            var serviceResultsAsync = await service.GetThemesAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetThemes_Paging()
        {
            var theme1 = new Theme
            {
                ThemeId = 1,
                ThemeName = "theme"
            };
            var theme2 = new Theme
            {
                ThemeId = 2,
                ThemeName = "theme2"
            };
            context.Themes.Add(theme2);
            context.Themes.Add(theme1);

            Action<PagedQueryResults<ThemeDTO>> tester = (results) =>
            {
                Assert.AreEqual(2, results.Total);
                Assert.AreEqual(1, results.Results.Count);
            };
            var defaultSorter = new ExpressionSorter<ThemeDTO>(x => x.Id, SortDirection.Descending);
            var queryOperator = new QueryableOperator<ThemeDTO>(0, 1, defaultSorter);

            var serviceResults = service.GetThemes(queryOperator);
            var serviceResultsAsync = await service.GetThemesAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

    }
}
