using ECA.Core.DynamicLinq.Filter;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class ContainsFilterTestClass
    {
        public ContainsFilterTestClass()
        {
            this.Ids = new List<int>();
            this.LongIds = new List<long>();
            this.Strings = new List<string>();
            this.Chars = new List<char>();

        }
        public string A { get; set; }

        public Char c { get; set; }

        public IEnumerable<string> Strings { get; set; }

        public IEnumerable<int> Ids { get; set; }

        public IEnumerable<long> LongIds { get; set; }

        public IEnumerable<char> Chars { get; set; }
    }

    [TestClass]
    public class ContainsAnyFilterTest
    {
        [TestMethod]
        public void TestPropertyCollectionType()
        {
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Ids", new List<int>());
            Assert.AreEqual(typeof(int), filter.PropertyCollectionType);
        }

        [TestMethod]
        public void TestPropertyCollectionType_PropertyIsLong_ValueIsInt()
        {
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("LongIds", new List<int>());
            Assert.AreEqual(typeof(Int64), filter.ValueCollectionType);
        }

        [TestMethod]
        public void TestValueCollectionType()
        {
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Ids", new List<int>());
            Assert.AreEqual(typeof(int), filter.ValueCollectionType);
        }

        [TestMethod]
        public void TestPropertyCollectionType_PropertyIsInt_ValueIsLong()
        {
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Ids", new List<long>());
            Assert.AreEqual(typeof(Int32), filter.ValueCollectionType);
        }

        #region String Property
        [TestMethod]
        public void TestToWhereExpression_StringProperty_SinglePropertyValue()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Strings = new List<string> { "A" }
            });

            var testStrings = new List<string> { "A" };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Strings", testStrings);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_StringProperty_MultiPropertyValues()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Strings = new List<string> { "A", "B" }
            });

            var testStrings = new List<string> { "A" };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Strings", testStrings);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_StringProperty_SingleFilterValue()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Strings = new List<string> { "A", "B" }
            });

            var testStrings = new List<string> { "A" };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Strings", testStrings);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_StringProperty_MultipleFilterValues()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Strings = new List<string> { "A", "B" }
            });

            var testStrings = new List<string> { "A", "B" };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Strings", testStrings);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }


        [TestMethod]
        public void TestToWhereExpression_StringProperty_MultipleFilterValues_MultiplePropertyValues()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Strings = new List<string> { "A", "B" }
            });

            var testStrings = new List<string> { "A", "B" };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Strings", testStrings);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_StringProperty_EmptyPropertyValues()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Strings = new List<string>()
            });

            var testStrings = new List<string> { "A" };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Strings", testStrings);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_StringProperty_EmptyFilterValues()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Strings = new List<string> { "A", "B" }
            });

            var testStrings = new List<string>();
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Strings", testStrings);
            filter.Invoking(x => x.ToWhereExpression()).ShouldThrow<NotSupportedException>()
                .WithMessage("There must be at least one value to filter on.");

        }
        #endregion

        #region Int Property
        [TestMethod]
        public void TestToWhereExpression_IntProperty_SinglePropertyValue()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Ids = new List<int> { 1 }
            });

            var testIds = new List<int> { 1 };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Ids", testIds);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_IntProperty_MultiPropertyValues()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Ids = new List<int> { 1, 2 }
            });

            var testIds = new List<int> { 1 };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Ids", testIds);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_IntProperty_SingleFilterValue()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Ids = new List<int> { 1, 2 }
            });

            var testIds = new List<int> { 1 };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Ids", testIds);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_IntProperty_MultipleFilterValues()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Ids = new List<int> { 1 }
            });

            var testIds = new List<int> { 1, 2 };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Ids", testIds);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_IntProperty_MultipleFilterValues_MultiplePropertyValues()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Ids = new List<int> { 1, 2 }
            });

            var testIds = new List<int> { 1, 2 };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Ids", testIds);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_IntProperty_EmptyPropertyValues()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Ids = new List<int>()
            });

            var testIds = new List<int> { 1, 2 };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Ids", testIds);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_IntProperty_EmptyFilterValues()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Ids = new List<int> { 1, 2}
            });

            var testIds = new List<int>();
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Ids", testIds);
            filter.Invoking(x => x.ToWhereExpression()).ShouldThrow<NotSupportedException>()
                .WithMessage("There must be at least one value to filter on.");
            
        }

        #endregion

        #region Different Value Type
        [TestMethod]
        public void TestToWhereExpression_LongProperty_IntValues()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                LongIds = new List<long> { 1L }
            });

            var testIds = new List<int> { 1 };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("LongIds", testIds);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_IntProperty_LongValues()
        {
            var list = new List<ContainsFilterTestClass>();
            list.Add(new ContainsFilterTestClass
            {
                Ids = new List<int> { 1 }
            });

            var testIds = new List<long> { 1L };
            var filter = new ContainsAnyFilter<ContainsFilterTestClass>("Ids", testIds);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);
        }
        #endregion

        [TestMethod]
        public void TestToWhereExpression_ExceededMaxTerms()
        {
            var ints = new List<int>();
            for (var i = 0; i <= ContainsAnyFilter<ContainsFilterTestClass>.MAX_TERMS; i++)
            {
                ints.Add(i);
            }
            var instance = new ContainsAnyFilter<ContainsFilterTestClass>("Ids", ints);
            instance.Invoking(x => x.ToWhereExpression())
                .ShouldThrow<NotSupportedException>()
                .WithMessage(String.Format("The maximum number of terms to filter with is [{0}].", 
                ContainsAnyFilter<ContainsFilterTestClass>.MAX_TERMS));
        }

        [TestMethod]
        public void TestToWhereExpression_EnumerableStringProperty_NumericCollection()
        {
            var ids = new List<int>();
            Action a = () => new ContainsAnyFilter<ContainsFilterTestClass>("Strings", ids);
            a.ShouldThrow<NotSupportedException>().WithMessage("The collection type is numeric but the property collection type is not numeric.");
        }

        [TestMethod]
        public void TestToWhereExpression_NumericProperty_StringCollection()
        {
            var values = new List<string>();
            Action a = () => new ContainsAnyFilter<ContainsFilterTestClass>("Ids", values);
            a.ShouldThrow<NotSupportedException>().WithMessage("The collection type is not numeric but the property collection type is numeric.");
        }

        [TestMethod]
        public void TestToWhereExpression_NonStringProperty_NonStringCollection()
        {
            var values = new List<char>();
            Action a = () => new ContainsAnyFilter<ContainsFilterTestClass>("Chars", values);
            a.ShouldThrow<NotSupportedException>().WithMessage("The collection type and property type must both be either a string or a numeric value.");
        }



        [TestMethod]
        public void TestToWhereExpression_CharCollection()
        {
            var chars = new List<char> { 'a', 'b', 'c' };
            Action a = () => new ContainsAnyFilter<ContainsFilterTestClass>("Chars", chars);
            a.ShouldThrow<NotSupportedException>().WithMessage("The collection type and property type must both be either a string or a numeric value.");
        }

        [TestMethod]
        public void TestToWhereExpression_StringProperty()
        {
            var ids = new List<int>();
            Action a = () => new ContainsAnyFilter<ContainsFilterTestClass>("A", ids);
            a.ShouldThrow<NotSupportedException>().WithMessage("The property is not an enumerable.");
        }

        [TestMethod]
        public void TestToWhereExpression_NotEnumerableProperty()
        {
            var ids = new List<int>();
            Action a = () => new ContainsAnyFilter<ContainsFilterTestClass>("C", ids);
            a.ShouldThrow<NotSupportedException>().WithMessage("The property is not an enumerable.");
        }

        [TestMethod]
        public void TestToWhereExpression_ValueTypeIsNotEnumerable()
        {
            Action a = () => new ContainsAnyFilter<ContainsFilterTestClass>("Ids", 1);
            a.ShouldThrow<NotSupportedException>().WithMessage("The value is not an enumerable.");
        }

        [TestMethod]
        public void TestToWhereExpression_ValueTypeIsString()
        {
            Action a = () => new ContainsAnyFilter<ContainsFilterTestClass>("Ids", "a");
            a.ShouldThrow<NotSupportedException>().WithMessage("The value is not an enumerable.");
        }
    }
}
