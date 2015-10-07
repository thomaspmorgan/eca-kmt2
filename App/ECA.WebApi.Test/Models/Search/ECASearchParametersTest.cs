using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Search;
using System.Collections.Generic;
using CAM.Business.Service;

namespace ECA.WebApi.Test.Models.Search
{
    [TestClass]
    public class ECASearchParametersTest
    {
        [TestMethod]
        public void Test()
        {
            var model = new ECASearchParametersBindingModel();
            model.Facets = new List<string>();
            model.SelectFields = new List<string>();
            model.Filter = "filter";
            model.Limit = 10;
            model.SearchTerm = "search";
            model.Start = 1;
            model.HightlightPreTag = "pre";
            model.HighlightPostTag = "post";

            var permissions = new List<IPermission>();
            var instance = model.ToECASearchParameters(permissions);
            Assert.AreEqual(model.Limit, instance.Limit);
            Assert.AreEqual(model.Start, instance.Start);
            Assert.AreEqual(model.Filter, instance.Filter);
            Assert.AreEqual(model.HightlightPreTag, instance.HighlightPreTag);
            Assert.AreEqual(model.HighlightPostTag, instance.HighlightPostTag);
            Assert.IsTrue(Object.ReferenceEquals(model.Facets, instance.Facets));
            Assert.IsTrue(Object.ReferenceEquals(model.SelectFields, instance.SelectFields));
        }
    }
}
