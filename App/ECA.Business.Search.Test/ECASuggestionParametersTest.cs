using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Search.Test
{
    [TestClass]
    public class ECASuggestionParametersTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var searchTerm = "term";
            var preTag = "pre";
            var postTag = "post";
            var instance = new ECASuggestionParameters(searchTerm, preTag, postTag);
            Assert.AreEqual(searchTerm, instance.SearchTerm);
            Assert.AreEqual(preTag, instance.HighlightPreTag);
            Assert.AreEqual(postTag, instance.HighlightPostTag);
        }
    }
}
