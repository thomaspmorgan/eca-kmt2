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

            var serviceResults = service.Get(queryOperator);
            var serviceResultsAsync = await service.GetAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

    }
}
