using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using Microsoft.Azure.Search.Models;
using ECA.WebApi.Models.Search;

namespace ECA.WebApi.Test.Models.Search
{
    [TestClass]
    public class SearchResultViewModelTest
    {
        [TestMethod]
        public void TestConstructor_SearchResult()
        {
            var ecaDocument = new ECADocument();
            var searchResult = new SearchResult<ECADocument>();
            searchResult.Document = ecaDocument;
            searchResult.Highlights = new HitHighlights();
            searchResult.Score = 3;

            var model = new SearchResultViewModel(searchResult);
            Assert.IsNotNull(model.Document);
            Assert.AreEqual(searchResult.Score, model.Score);
            Assert.IsTrue(Object.ReferenceEquals(searchResult.Highlights, model.HitHighlights));
        }
    }
}
