using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.DynamicLinq.Filter;

namespace ECA.Core.DynamicLinq.Test.Filter
{
    public class LikeFilterTestClass
    {
        public string S { get; set; }
    }

    
    [TestClass]
    public class LikeFilterTest
    {   
        [TestMethod]
        public void TestToWhereExpression_PropertyHasValue()
        {
            var list = new List<LikeFilterTestClass>();
            var instance = new LikeFilterTestClass();
            instance.S = "Hello";
            list.Add(instance);

            var filter = new LikeFilter<LikeFilterTestClass>("S", instance.S);
            var whereExpression = filter.ToWhereExpression();
            var whereFunc = whereExpression.Compile();

            var results = list.Where(whereFunc).ToList();
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(instance.S, results.First().S);
        }

        [TestMethod]
        public void TestToWhereExpression_PropertyValueIsNull()
        {
            var list = new List<LikeFilterTestClass>();
            var instance = new LikeFilterTestClass();
            instance.S = null;
            list.Add(instance);

            var filter = new LikeFilter<LikeFilterTestClass>("S", "Hello");
            var whereExpression = filter.ToWhereExpression();
            var whereFunc = whereExpression.Compile();

            var results = list.Where(whereFunc).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_PropertyHasValue_SearchTermIsDifferent()
        {
            var list = new List<LikeFilterTestClass>();
            var instance = new LikeFilterTestClass();
            instance.S = "abc";
            list.Add(instance);

            var filter = new LikeFilter<LikeFilterTestClass>("S", "Hello");
            var whereExpression = filter.ToWhereExpression();
            var whereFunc = whereExpression.Compile();

            var results = list.Where(whereFunc).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestToWhereExpression_PropertyHasValue_CaseInsentive()
        {
            var list = new List<LikeFilterTestClass>();
            var instance = new LikeFilterTestClass();
            instance.S = "hello".ToUpper();
            list.Add(instance);

            var filter = new LikeFilter<LikeFilterTestClass>("S", instance.S.ToLower());
            var whereExpression = filter.ToWhereExpression();
            var whereFunc = whereExpression.Compile();

            var results = list.Where(whereFunc).ToList();
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(instance.S, results.First().S);
        }
    }
}
