using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Query;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.WebApi.Test.Models.Query
{
    public class FilterBindingModelTestClass
    {
        public string S { get; set; }
    }

    [TestClass]
    public class FilterBindingModelTest
    {
        [TestMethod]
        public void TestToIFilter()
        {
            var model = new FilterBindingModel
            {
                Comparison = ComparisonType.Equal.Value,
                Property = "S",
                Value = "hello"
            };
            var iFilter = model.ToIFilter();
            Assert.IsInstanceOfType(iFilter, typeof(SimpleFilter));
            var simpleFilter = (SimpleFilter)iFilter;
            Assert.AreEqual(model.Comparison, simpleFilter.Comparison);
            Assert.AreEqual(model.Property, simpleFilter.Property);
            Assert.AreEqual(model.Value, simpleFilter.Value);
        }
    }
}
