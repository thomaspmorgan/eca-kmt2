using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class NullFilterTestClass
    {
        public string S { get; set; }
    }

    /// <summary>
    /// Summary description for NullFilterTest
    /// </summary>
    [TestClass]
    public class NullFilterTest
    {
        public NullFilterTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestConstructor_IsNotNullTrue()
        {
            var filter = new NullFilter<NullFilterTestClass>("S", true);
            Assert.IsTrue(filter.IsNotNull);
        }

        [TestMethod]
        public void TestConstructor_IsNotNullFalse()
        {
            var filter = new NullFilter<NullFilterTestClass>("S", false);
            Assert.IsFalse(filter.IsNotNull);
        }

        [TestMethod]
        public void TestConstructor_DefaultIsNotNull()
        {
            var filter = new NullFilter<NullFilterTestClass>("S");
            Assert.IsFalse(filter.IsNotNull);
        }


        [TestMethod]
        public void TestGetWhereExpression_IsNotNullCheck_NullablePropertyHasValue()
        {
            var list = new List<NullFilterTestClass>();
            var instance = new NullFilterTestClass
            {
                S = "hello"
            };
            list.Add(instance);
            var filter = new NullFilter<NullFilterTestClass>("S", true);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);

        }

        [TestMethod]
        public void TestGetWhereExpression_IsNotNullCheck_NullablePropertyIsNull()
        {
            var list = new List<NullFilterTestClass>();
            var instance = new NullFilterTestClass
            {
                S = null
            };
            list.Add(instance);
            var filter = new NullFilter<NullFilterTestClass>("S", true);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);

        }

        [TestMethod]
        public void TestGetWhereExpression_IsNullCheck_NullablePropertyHasValue()
        {
            var list = new List<NullFilterTestClass>();
            var instance = new NullFilterTestClass
            {
                S = "hello"
            };
            list.Add(instance);
            var filter = new NullFilter<NullFilterTestClass>("S", false);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(0, results.Count);

        }

        [TestMethod]
        public void TestGetWhereExpression_IsNullCheck_NullablePropertyIsNull()
        {
            var list = new List<NullFilterTestClass>();
            var instance = new NullFilterTestClass
            {
                S = null
            };
            list.Add(instance);
            var filter = new NullFilter<NullFilterTestClass>("S", false);
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);

        }

        [TestMethod]
        public void TestGetWhereExpression_DefaultIsNullCheck_NullablePropertyIsNull()
        {
            var list = new List<NullFilterTestClass>();
            var instance = new NullFilterTestClass
            {
                S = null
            };
            list.Add(instance);
            var filter = new NullFilter<NullFilterTestClass>("S");
            var where = filter.ToWhereExpression();
            var results = list.Where(where.Compile()).ToList();
            Assert.AreEqual(1, results.Count);

        }
    }
}
