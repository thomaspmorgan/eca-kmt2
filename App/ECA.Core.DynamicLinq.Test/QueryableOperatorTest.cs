using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test
{
    public class QueryableOperatorTestClass
    {
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var otherType = obj as QueryableOperatorTestClass;
            if (otherType == null)
            {
                return false;
            }
            return this.Id == otherType.Id;
        }
    }

    [TestClass]
    public class QueryableOperatorTest
    {
        [TestMethod]
        public void TestExtensions_Apply_OnlyHasDefaultSorter()
        {
            var list = new List<QueryableOperatorTestClass>();
            list.Add(new QueryableOperatorTestClass
            {
                Id = 1
            });
            list.Add(new QueryableOperatorTestClass
            {
                Id = 2
            });
            list = list.OrderByDescending(x => x.Id).ToList();

            var defaultSorter = new ExpressionSorter<QueryableOperatorTestClass>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<QueryableOperatorTestClass>(0, 10, defaultSorter);
            var query = list.AsQueryable().Apply(queryOperator);
            var testList = query.ToList();
            var expectedList = list.OrderBy(x => x.Id).ToList();
            CollectionAssert.AreEqual(expectedList, testList);
        }

        [TestMethod]
        public void TestExtensions_Apply_HasSorter()
        {
            var list = new List<QueryableOperatorTestClass>();
            list.Add(new QueryableOperatorTestClass
            {
                Id = 2
            });
            list.Add(new QueryableOperatorTestClass
            {
                Id = 1
            });
            list = list.OrderBy(x => x.Id).ToList();

            var defaultSorter = new ExpressionSorter<QueryableOperatorTestClass>(x => x.Id, SortDirection.Ascending);
            var actualSorter = new ExpressionSorter<QueryableOperatorTestClass>(x => x.Id, SortDirection.Descending);

            var queryOperator = new QueryableOperator<QueryableOperatorTestClass>(0, 10, defaultSorter, null, new List<ISorter> { actualSorter });
            var query = list.AsQueryable().Apply(queryOperator);
            var testList = query.ToList();
            var expectedList = list.OrderByDescending(x => x.Id).ToList();
            CollectionAssert.AreEqual(expectedList, testList);
        }

        [TestMethod]
        public void TestExtensions_Apply_HasFilter()
        {
            var list = new List<QueryableOperatorTestClass>();
            list.Add(new QueryableOperatorTestClass
            {
                Id = 2
            });
            list.Add(new QueryableOperatorTestClass
            {
                Id = 1
            });
            list = list.OrderBy(x => x.Id).ToList();

            var filterValue = 1;
            var defaultSorter = new ExpressionSorter<QueryableOperatorTestClass>(x => x.Id, SortDirection.Ascending);
            var filter = new ExpressionFilter<QueryableOperatorTestClass>(x => x.Id, ComparisonType.Equal, filterValue);

            var queryOperator = new QueryableOperator<QueryableOperatorTestClass>(0, 10, defaultSorter, new List<IFilter> { filter }, null);
            var query = list.AsQueryable().Apply(queryOperator);
            var testList = query.ToList();
            Assert.AreEqual(1, testList.Count);
            Assert.AreEqual(filterValue, testList.First().Id);

        }

        [TestMethod]
        public void TestToString_DefaultSorterOnly()
        {
            var defaultSorter = new ExpressionSorter<QueryableOperatorTestClass>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<QueryableOperatorTestClass>(0, 10, defaultSorter, null, null);
            Assert.IsNotNull(queryOperator.ToString());
        }

        [TestMethod]
        public void TestToString_HasFiltersAndSorters()
        {
            var filter1 = new ExpressionFilter<QueryableOperatorTestClass>(x => x.Id, ComparisonType.Equal, 1);
            var filter2 = new ExpressionFilter<QueryableOperatorTestClass>(x => x.Id, ComparisonType.LessThan, 2);

            var sorter1 = new ExpressionSorter<QueryableOperatorTestClass>(x => x.Id, SortDirection.Descending);
            var sorter2 = new ExpressionSorter<QueryableOperatorTestClass>(x => x.Id, SortDirection.Ascending);

            var defaultSorter = new ExpressionSorter<QueryableOperatorTestClass>(x => x.Id, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<QueryableOperatorTestClass>(0, 10, defaultSorter, new List<IFilter>{ filter1, filter2}, new List<ISorter>{sorter1, sorter2});
            Assert.IsNotNull(queryOperator.ToString());
        }
    }
}
