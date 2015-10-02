using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using ECA.WebApi.Models.Search;
using Microsoft.Azure.Search.Models;

namespace ECA.WebApi.Test.Models.Search
{
    [TestClass]
    public class DocumentSearchResponseViewModelTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var documentTypeId = Guid.NewGuid();
            var id = 1;
            var key = new DocumentKey(documentTypeId, id);
            var ecaDocument = new ECADocument();
            ecaDocument.SetKey(key);

            var response = new DocumentSearchResponse<ECADocument>();
            response.Count = 1;
            response.Coverage = 2;

            var searchResult = new SearchResult<ECADocument>();
            searchResult.Document = ecaDocument;
            searchResult.Highlights = new HitHighlights();
            searchResult.Score = 3;
            response.Results.Add(searchResult);
            var model = new DocumentSearchResponseViewModel(response);

            Assert.AreEqual(response.Count, model.Count);
            Assert.AreEqual(response.Coverage, model.Coverage);
            Assert.AreEqual(1, response.Results.Count);

            var firstResult = response.Results.First();
            Assert.AreEqual(searchResult.Score, firstResult.Score);
            Assert.IsTrue(Object.ReferenceEquals(ecaDocument, searchResult.Document));
            Assert.IsTrue(Object.ReferenceEquals(searchResult.Highlights, firstResult.Highlights));
        }
    }
}
