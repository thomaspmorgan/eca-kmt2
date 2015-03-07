using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;

namespace ECA.Business.Test.Service
{
    [TestClass]
    public class UserTest
    {

        [TestMethod]
        public void TestGetHashCode()
        {
            var userId = 1;
            var user = new User(userId);
            Assert.AreEqual(userId.GetHashCode(), user.GetHashCode());
        }

        [TestMethod]
        public void TestEquals()
        {
            var userId = 1;
            var user = new User(userId);
            var user2 = new User(userId);
            Assert.IsTrue(user.Equals(user2));
            Assert.IsTrue(user2.Equals(user));

        }

        [TestMethod]
        public void TestEquals_NullTestObject()
        {
            Assert.IsFalse(new User(1).Equals(null));
        }

        [TestMethod]
        public void TestEquals_DifferentTypeObject()
        {
            Assert.IsFalse(new User(1).Equals(1));
        }
    }
}
