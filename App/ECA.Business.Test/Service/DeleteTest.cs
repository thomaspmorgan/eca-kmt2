using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.Data;

namespace ECA.Business.Test.Service
{
    [TestClass]
    public class DeleteTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var now = DateTimeOffset.UtcNow;
            var userId = 1;
            var user = new User(userId);
            var delete = new Delete(user);
            Assert.IsTrue(user == delete.User);
            Assert.AreEqual(userId, delete.User.Id);
            delete.Date.Should().BeCloseTo(now, DbContextHelper.DATE_PRECISION);
        }

        [TestMethod]
        public void TestSetHistory_NullHistoryProperty()
        {
            var instance = new NewHistoricalTestClass();
            Assert.IsNull(instance.History);
            var userId = 1;
            var user = new User(userId);
            var now = DateTimeOffset.UtcNow;
            var delete = new Delete(user);

            delete.SetHistory(instance);
            Assert.IsNull(instance.History);
        }

        [TestMethod]
        public void TestSetHistory_NotNullHistoryProperty()
        {
            var instance = new NewHistoricalTestClass();
            instance.History = new History();
            var userId = 1;
            var user = new User(userId);
            var now = DateTimeOffset.UtcNow;
            var delete = new Delete(user);

            delete.SetHistory(instance);
            Assert.IsNotNull(instance.History);
            Assert.AreEqual(0, instance.History.CreatedBy);
            Assert.AreEqual(0, instance.History.RevisedBy);
            instance.History.RevisedOn.Should().NotBe(now);
            instance.History.CreatedOn.Should().NotBe(now);
        }
    }
}
