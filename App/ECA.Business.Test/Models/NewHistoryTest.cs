﻿using FluentAssertions;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Models;
using ECA.Data;

namespace ECA.Business.Test.Models
{
    public class NewHistoricalTestClass : IHistorical
    {
        public History History { get; set; }
    }

    [TestClass]
    public class NewHistoryTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var now = DateTimeOffset.UtcNow;
            var userId = 1;
            var newHistory = new NewHistory(userId);
            Assert.AreEqual(1, newHistory.CreatorUserId);
            newHistory.CreatedAndRevisedOn.Should().BeCloseTo(now, DbContextHelper.DATE_PRECISION);
        }

        [TestMethod]
        public void TestSetHistory_NullHistoryProperty()
        {
            var instance = new NewHistoricalTestClass();
            Assert.IsNull(instance.History);
            var userId = 1;
            var now = DateTimeOffset.UtcNow;
            var newHistory = new NewHistory(userId);

            newHistory.SetHistory(instance);
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
            var now = DateTimeOffset.UtcNow;
            var newHistory = new NewHistory(userId);

            newHistory.SetHistory(instance);
            Assert.IsNotNull(instance.History);
            Assert.AreEqual(userId, instance.History.CreatedBy);
            Assert.AreEqual(userId, instance.History.RevisedBy);
            instance.History.RevisedOn.Should().BeCloseTo(now, DbContextHelper.DATE_PRECISION);
            instance.History.CreatedOn.Should().BeCloseTo(now, DbContextHelper.DATE_PRECISION);
        }
    }
}
