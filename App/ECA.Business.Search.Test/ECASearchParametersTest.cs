using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ECA.Business.Search.Test
{
    [TestClass]
    public class ECASearchParametersTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var start = 1;
            var limit = 2;
            var filter = "filter";
            var fields = new List<string>();
            var facets = new List<string>();
            var searchTerm = "term";
            var instance = new ECASearchParameters(start, limit, filter, facets, fields, searchTerm);
            Assert.AreEqual(start, instance.Start);
            Assert.AreEqual(limit, instance.Limit);
            Assert.AreEqual(filter, instance.Filter);
            Assert.IsTrue(Object.ReferenceEquals(fields, instance.Fields));
            Assert.IsTrue(Object.ReferenceEquals(facets, instance.Facets));

        }
    }
}
