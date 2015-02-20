using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Query;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;

namespace ECA.WebApi.Test.Models.Query
{

    public class SorterBindingModelTestClass
    {
        public string S { get; set; }
    }
    [TestClass]
    public class SorterBindingModelTest
    {
        [TestMethod]
        public void TestToISorter()
        {
            var model = new SorterBindingModel
            {
                Direction = SortDirection.Ascending.Value,
                Property = "S"
            };
            var iSorter = model.ToISorter();
            Assert.IsInstanceOfType(iSorter, typeof(SimpleSorter));
            var simpleSorter = (SimpleSorter)iSorter;
            Assert.AreEqual(model.Direction, simpleSorter.Direction);
            Assert.AreEqual(model.Property, simpleSorter.Property);
        }
    }
}
