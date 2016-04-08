using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Core.Generation;

namespace ECA.Core.Test.Generation
{
    [TestClass]
    public class StaticLookupTest
    {
        [TestMethod]
        public void TestToString()
        {
            var value = "A";
            var id = 1;
            var lookup1 = new StaticLookup(value, id);
            Assert.IsNotNull(lookup1.ToString());
            Assert.IsTrue(lookup1.ToString().Contains(value));
            Assert.IsTrue(lookup1.ToString().Contains(id.ToString()));
        }

        [TestMethod]
        public void TestEquals_TwoObjectsAreEqual()
        {
            var value = "A";
            var id = 1;
            var lookup1 = new StaticLookup(value, id);
            var lookup2 = new StaticLookup(value, id);
            Assert.IsTrue(lookup1.Equals(lookup2));
            Assert.IsTrue(lookup2.Equals(lookup1));
        }

        [TestMethod]
        public void TestEquals_NullInstance()
        {
            var value = "A";
            var id = 1;
            var lookup1 = new StaticLookup(value, id);
            Assert.IsFalse(lookup1.Equals(null));
        }

        [TestMethod]
        public void TestEquals_DifferentObjectTypeInstance()
        {
            var value = "A";
            var id = 1;
            var lookup1 = new StaticLookup(value, id);
            Assert.IsFalse(lookup1.Equals(1));
        }

        [TestMethod]
        public void TestEquals_SameInstance()
        {
            var value = "A";
            var id = 1;
            var lookup1 = new StaticLookup(value, id);
            Assert.IsTrue(lookup1.Equals(lookup1));
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var value = "A";
            var id = 1;
            var lookup1 = new StaticLookup(value, id);
            Assert.AreEqual(1.GetHashCode(), lookup1.GetHashCode());
        }

        [TestMethod]
        public void TestEqualOperator_TwoEqualObjects()
        {
            var value = "A";
            var id = 1;
            var lookup1 = new StaticLookup(value, id);
            var lookup2 = new StaticLookup(value, id);
            Assert.IsTrue(lookup1 == lookup2);
        }

        [TestMethod]
        public void TestNotEqualOperator_TwoEqualObjects()
        {
            var value = "A";
            var id = 1;
            var lookup1 = new StaticLookup(value, id);
            var lookup2 = new StaticLookup(value, id);
            Assert.IsFalse(lookup1 != lookup2);
        }

        [TestMethod]
        public void TestEqualOperator_TwoDifferentsObjects()
        {
            var value1 = "A";
            var id1 = 1;

            var value2 = "B";
            var id2 = 2;
            var lookup1 = new StaticLookup(value1, id1);
            var lookup2 = new StaticLookup(value2, id2);
            Assert.IsFalse(lookup1 == lookup2);
        }

        [TestMethod]
        public void TestNotEqualOperator_TwoDifferentsObjects()
        {
            var value1 = "A";
            var id1 = 1;

            var value2 = "B";
            var id2 = 2;
            var lookup1 = new StaticLookup(value1, id1);
            var lookup2 = new StaticLookup(value2, id2);
            Assert.IsTrue(lookup1 != lookup2);
        }

        [TestMethod]
        public void TestEqualOperator_SameInstance()
        {
            var value = "A";
            var id = 1;
            var lookup1 = new StaticLookup(value, id);
            Assert.IsTrue(lookup1 == lookup1);
        }

        [TestMethod]
        public void TestNotEqualOperator_SameInstance()
        {
            var value = "A";
            var id = 1;
            var lookup1 = new StaticLookup(value, id);
            Assert.IsFalse(lookup1 != lookup1);
        }

        [TestMethod]
        public void TestEqualOperator_CheckIdsAreEqualityComparer()
        {
            var value = "A";
            var id1 = 1;
            var id2 = 2;
            var lookup1 = new StaticLookup(value, id1);
            var lookup2 = new StaticLookup(value, id2);
            Assert.IsFalse(lookup1 == lookup2);
        }

        [TestMethod]
        public void TestNotEqualOperator_CheckIdsAreEqualityComparer()
        {
            var value = "A";
            var id1 = 1;
            var id2 = 2;
            var lookup1 = new StaticLookup(value, id1);
            var lookup2 = new StaticLookup(value, id2);
            Assert.IsTrue(lookup1 != lookup2);
        }
    }
}
