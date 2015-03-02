using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Sorter;

namespace ECA.Core.DynamicLinq.Test.Sorter
{
    public class SimpleSorterTestClass
    {
        public int Id { get; set; }
    }

    [TestClass]
    public class SimpleSorterTest
    {
        [TestMethod]
        public void TestToLinqSorter_Ascending()
        {
            var simpleSorter = new SimpleSorter
            {
                Direction = SortDirection.Ascending.Value,
                Property = "Id"
            };

            var linqSorter = simpleSorter.ToLinqSorter<SimpleSorterTestClass>();
            Assert.AreEqual(SortDirection.Ascending, linqSorter.Direction);
            Assert.AreEqual(simpleSorter.Property, linqSorter.PropertyInfo.Name);
        }

        [TestMethod]
        public void TestToLinqSorter_Descending()
        {
            var simpleSorter = new SimpleSorter
            {
                Direction = SortDirection.Descending.Value,
                Property = "Id"
            };

            var linqSorter = simpleSorter.ToLinqSorter<SimpleSorterTestClass>();
            Assert.AreEqual(SortDirection.Descending, linqSorter.Direction);
            Assert.AreEqual(simpleSorter.Property, linqSorter.PropertyInfo.Name);
        }

        [TestMethod]
        public void TestToLinqSorter_ToString()
        {
            var simpleSorter = new SimpleSorter
            {
                Direction = SortDirection.Descending.Value,
                Property = "Id"
            };
            Assert.IsNotNull(simpleSorter.ToString());
        }
    }
}
