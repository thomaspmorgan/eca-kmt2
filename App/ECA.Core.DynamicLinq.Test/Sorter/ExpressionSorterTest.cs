using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Sorter;

namespace ECA.Core.DynamicLinq.Test.Sorter
{
    public class ExpresssionSorterTestClass
    {
        public int Id { get; set; }
    }

    [TestClass]
    public class ExpressionSorterTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var direction = SortDirection.Ascending;
            var sorter = new ExpressionSorter<ExpresssionSorterTestClass>(x => x.Id, direction);
            Assert.AreEqual("Id", sorter.Property);
            Assert.AreEqual(direction.Value, sorter.Direction);

        }
    }
}
