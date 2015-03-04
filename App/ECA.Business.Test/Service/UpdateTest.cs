using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Models;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.Business.Test.Service
{
    public class RevisedHistoricalTestClass : IHistorical
    {
        public History History { get; set; }
    }

    [TestClass]
    public class UpdateTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var now = DateTimeOffset.UtcNow;
            var userId = 1;
            var user = new User(userId);
            var update = new Update(user);
            Assert.IsTrue(user == update.User);
            Assert.AreEqual(userId, update.User.Id);
            update.Date.Should().BeCloseTo(now, DbContextHelper.DATE_PRECISION);
        }

        [TestMethod]
        public void TestSetHistory()
        {
            var createdByUserId = 1;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var updatedByuser = 2;
            var today = DateTimeOffset.UtcNow;
            var user = new User(updatedByuser);

            var instance = new RevisedHistoricalTestClass();
            instance.History = new History
            {
                CreatedOn = yesterday,
                CreatedBy = createdByUserId
            };

            var update = new Update(user);

            update.SetHistory(instance);
            Assert.IsNotNull(instance.History);
            Assert.AreEqual(createdByUserId, instance.History.CreatedBy);
            Assert.AreEqual(updatedByuser, instance.History.RevisedBy);
            instance.History.RevisedOn.Should().BeCloseTo(today, DbContextHelper.DATE_PRECISION);
            instance.History.CreatedOn.Should().BeCloseTo(yesterday, DbContextHelper.DATE_PRECISION);
        }

    }
}
