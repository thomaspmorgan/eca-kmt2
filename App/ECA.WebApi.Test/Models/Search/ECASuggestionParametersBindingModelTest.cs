using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Search;
using System.Collections.Generic;
using CAM.Business.Service;

namespace ECA.WebApi.Test.Models.Search
{
    [TestClass]
    public class ECASuggestionParametersBindingModelTest
    {
        [TestMethod]
        public void TestToECASuggestionsParameters()
        {
            var model = new ECASuggestionParametersBindingModel();
            model.HighlightPostTag = "post";
            model.HighlightPreTag = "pre";
            model.SearchTerm = "search";
            var instance = model.ToECASuggestionParameters(new List<IPermission>());
            Assert.AreEqual(model.HighlightPreTag, instance.HighlightPreTag);
            Assert.AreEqual(model.HighlightPostTag, instance.HighlightPostTag);
            Assert.AreEqual(model.SearchTerm, instance.SearchTerm);
        }
    }
}
