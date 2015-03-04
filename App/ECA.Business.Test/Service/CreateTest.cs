using ECA.Business.Service;
using ECA.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ECA.Business.Test.Service
{
    public class NewHistoricalTestClass : IHistorical
    {
        public History History { get; set; }
    }

    [TestClass]
    public class CreateTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var now = DateTimeOffset.UtcNow;
            var userId = 1;
            var user = new User(userId);
            var create = new Create(user);
            Assert.IsTrue(user == create.User);
            Assert.AreEqual(userId, create.User.Id);
            create.Date.Should().BeCloseTo(now, DbContextHelper.DATE_PRECISION);
        }

        [TestMethod]
        public void TestSetHistory_NullHistoryProperty()
        {
            var instance = new NewHistoricalTestClass();
            Assert.IsNull(instance.History);
            var userId = 1;
            var user = new User(userId);
            var now = DateTimeOffset.UtcNow;
            var create = new Create(user);

            create.SetHistory(instance);
            Assert.IsNotNull(instance.History);
            Assert.AreEqual(userId, instance.History.CreatedBy);
            Assert.AreEqual(userId, instance.History.RevisedBy);
            instance.History.RevisedOn.Should().BeCloseTo(now, DbContextHelper.DATE_PRECISION);
            instance.History.CreatedOn.Should().BeCloseTo(now, DbContextHelper.DATE_PRECISION);
        }

        [TestMethod]
        public void TestSetHistory_NotNullHistoryProperty()
        {
            var instance = new NewHistoricalTestClass();
            instance.History = new History();
            var userId = 1;
            var user = new User(userId);
            var now = DateTimeOffset.UtcNow;
            var create = new Create(user);

            create.SetHistory(instance);
            Assert.IsNotNull(instance.History);
            Assert.AreEqual(userId, instance.History.CreatedBy);
            Assert.AreEqual(userId, instance.History.RevisedBy);
            instance.History.RevisedOn.Should().BeCloseTo(now, DbContextHelper.DATE_PRECISION);
            instance.History.CreatedOn.Should().BeCloseTo(now, DbContextHelper.DATE_PRECISION);
        }
    }
}
