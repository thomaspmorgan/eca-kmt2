using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ECA.Core.DynamicLinq.Sorter;

namespace ECA.Core.DynamicLinq.Test.Sorter
{
    public class MultiSortTestClass
    {
        public int Int1 { get; set; }

        public int Int2 { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var testClassInstance = obj as MultiSortTestClass;
            if (testClassInstance == null)
            {
                return false;
            }
            return this.Int1 == testClassInstance.Int1 && this.Int2 == testClassInstance.Int2;
        }
    }

    public class SimpleSortTestClass
    {
        public int Int1 { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var testClassInstance = obj as SimpleSortTestClass;
            if (testClassInstance == null)
            {
                return false;
            }
            return this.Int1 == testClassInstance.Int1;
        }
    }

    public class NullableSortTestClass
    {
        public int? Id { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            var testClassInstance = obj as NullableSortTestClass;
            if (testClassInstance == null)
            {
                return false;
            }
            return this.Id == testClassInstance.Id;
        }
    }

    [TestClass]
    public class LinqSorterTest
    {
        [TestMethod]
        public void TestOrderByExension_OrderByAscending_SimpleSort()
        {
            var list = new List<SimpleSortTestClass>();
            list.Add(new SimpleSortTestClass
            {
                Int1 = 1,
            });
            list.Add(new SimpleSortTestClass
            {
                Int1 = 1,
            });
            list.Add(new SimpleSortTestClass
            {
                Int1 = 2,
            });
            list.Add(new SimpleSortTestClass
            {
                Int1 = 2,
            });

            list = list.OrderByDescending(x => x.Int1).ToList();

            var sorter = new LinqSorter<SimpleSortTestClass>("Int1", SortDirection.Ascending);
            var sortedList = list.AsQueryable().OrderBy(sorter).ToList();
            var expectedList = list.OrderBy(x => x.Int1).ToList();
            CollectionAssert.AreEqual(expectedList, sortedList);

        }

        [TestMethod]
        public void TestOrderByExension_OrderByDescending_SimpleSort()
        {
            var list = new List<SimpleSortTestClass>();
            list.Add(new SimpleSortTestClass
            {
                Int1 = 1,
            });
            list.Add(new SimpleSortTestClass
            {
                Int1 = 1,
            });
            list.Add(new SimpleSortTestClass
            {
                Int1 = 2,
            });
            list.Add(new SimpleSortTestClass
            {
                Int1 = 2,
            });

            list = list.OrderBy(x => x.Int1).ToList();

            var sorter = new LinqSorter<SimpleSortTestClass>("Int1", SortDirection.Descending);
            var sortedList = list.AsQueryable().OrderBy(sorter).ToList();
            var expectedList = list.OrderByDescending(x => x.Int1).ToList();
            CollectionAssert.AreEqual(expectedList, sortedList);

        }

        [TestMethod]
        public void TestOrderByExension_OrderByAscending_MultiSort()
        {
            var list = new List<MultiSortTestClass>();
            list.Add(new MultiSortTestClass
            {
                Int1 = 1,
                Int2 = 1,
            });
            list.Add(new MultiSortTestClass
            {
                Int1 = 1,
                Int2 = 2,
            });
            list.Add(new MultiSortTestClass
            {
                Int1 = 2,
                Int2 = 1,
            });
            list.Add(new MultiSortTestClass
            {
                Int1 = 2,
                Int2 = 2
            });

            list = list.OrderByDescending(x => x.Int1).ThenByDescending(x => x.Int2).ToList();

            var sorter1 = new LinqSorter<MultiSortTestClass>("Int1", SortDirection.Ascending);
            var sorter2 = new LinqSorter<MultiSortTestClass>("Int2", SortDirection.Ascending);
            var sortedList = list.AsQueryable().OrderBy(new List<LinqSorter<MultiSortTestClass>>{sorter1, sorter2}).ToList();
            var expectedList = list.OrderBy(x => x.Int1).ThenBy(x => x.Int2).ToList();
            CollectionAssert.AreEqual(expectedList, sortedList);

        }

        [TestMethod]
        public void TestOrderByExension_OrderByDescending_MultiSort()
        {
            var list = new List<MultiSortTestClass>();
            list.Add(new MultiSortTestClass
            {
                Int1 = 1,
                Int2 = 1,
            });
            list.Add(new MultiSortTestClass
            {
                Int1 = 1,
                Int2 = 2,
            });
            list.Add(new MultiSortTestClass
            {
                Int1 = 2,
                Int2 = 1,
            });
            list.Add(new MultiSortTestClass
            {
                Int1 = 2,
                Int2 = 2
            });

            list = list.OrderBy(x => x.Int1).ThenBy(x => x.Int2).ToList();

            var sorter1 = new LinqSorter<MultiSortTestClass>("Int1", SortDirection.Descending);
            var sorter2 = new LinqSorter<MultiSortTestClass>("Int2", SortDirection.Descending);
            var sortedList = list.AsQueryable().OrderBy(new List<LinqSorter<MultiSortTestClass>> { sorter1, sorter2 }).ToList();
            var expectedList = list.OrderByDescending(x => x.Int1).ThenByDescending(x => x.Int2).ToList();
            CollectionAssert.AreEqual(expectedList, sortedList);

        }

        [TestMethod]
        public void TestOrderByExension_SimpleSorter()
        {
            var list = new List<SimpleSortTestClass>();
            list.Add(new SimpleSortTestClass
            {
                Int1 = 1,
            });
            list.Add(new SimpleSortTestClass
            {
                Int1 = 1,
            });
            list.Add(new SimpleSortTestClass
            {
                Int1 = 2,
            });
            list.Add(new SimpleSortTestClass
            {
                Int1 = 2,
            });

            list = list.OrderByDescending(x => x.Int1).ToList();

            var simpleSorter = new SimpleSorter
            {
                Direction = SortDirection.Ascending.Value,
                Property = "Int1"
            };
            var sortedList = list.AsQueryable().OrderBy(simpleSorter).ToList();
            var expectedList = list.OrderBy(x => x.Int1).ToList();
            CollectionAssert.AreEqual(expectedList, sortedList);

        }

        [TestMethod]
        public void TestOrderByExension_MultipleSimpleSorters()
        {
            var list = new List<MultiSortTestClass>();
            list.Add(new MultiSortTestClass
            {
                Int1 = 1,
                Int2 = 1,
            });
            list.Add(new MultiSortTestClass
            {
                Int1 = 1,
                Int2 = 2,
            });
            list.Add(new MultiSortTestClass
            {
                Int1 = 2,
                Int2 = 1,
            });
            list.Add(new MultiSortTestClass
            {
                Int1 = 2,
                Int2 = 2
            });

            list = list.OrderBy(x => x.Int1).ThenBy(x => x.Int2).ToList();
            var simpleSorter1 = new SimpleSorter
            {
                Direction = SortDirection.Descending.Value,
                Property = "Int1"
            };
            var simpleSorter2 = new SimpleSorter
            {
                Direction = SortDirection.Descending.Value,
                Property = "Int2"
            };

            var sortedList = list.AsQueryable().OrderBy(new List<SimpleSorter> { simpleSorter1, simpleSorter2 }).ToList();
            var expectedList = list.OrderByDescending(x => x.Int1).ThenByDescending(x => x.Int2).ToList();
            CollectionAssert.AreEqual(expectedList, sortedList);

        }

        [TestMethod]
        public void TestOrderByExtension_NullableProperty()
        {
            var list = new List<NullableSortTestClass>();
            list.Add(new NullableSortTestClass
            {
                Id = 1
            });
            list.Add(new NullableSortTestClass
            {
                Id = null
            });

            list = list.OrderBy(x => x.Id).ToList();

            var sorter = new SimpleSorter
            {
                Direction = SortDirection.Descending.Value,
                Property = "Id"
            };
            var sortedList = list.AsQueryable().OrderBy(sorter).ToList();
            var expectedList = list.OrderByDescending(x => x.Id).ToList();
            CollectionAssert.AreEqual(expectedList, sortedList);
        }
    }
}
