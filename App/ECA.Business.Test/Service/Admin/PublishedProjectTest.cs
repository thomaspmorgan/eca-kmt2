using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using System.Collections.Generic;
using ECA.Business.Service.Admin;
using ECA.Business.Service;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class PublishedProjectTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var projectId = 10;
            var projectStatusId = ProjectStatus.Active.Id;
            var name = "name";
            var description = "description";
            var goalIds = new List<int> { 1 };
            var pocIds = new List<int> { 2 };
            var themeIds = new List<int> { 3 };
            var focusId = 4;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow.AddDays(1.0);
            var user = new User(1);
            var instance = new PublishedProject(
                updatedBy: user,
                projectId: projectId,
                name: name,
                description: description,
                projectStatusId: projectStatusId,
                goalIds: goalIds,
                themeIds: themeIds,
                pointsOfContactIds: pocIds,
                focusId: focusId,
                startDate: startDate,
                endDate: endDate
                );

            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(projectStatusId, instance.ProjectStatusId);
            Assert.AreEqual(name, instance.Name);
            Assert.AreEqual(description, instance.Description);
            Assert.AreEqual(focusId, instance.FocusId);
            Assert.AreEqual(startDate, instance.StartDate);
            Assert.AreEqual(endDate, instance.EndDate);
            Assert.IsInstanceOfType(instance.Audit, typeof(Update));

            CollectionAssert.AreEqual(goalIds.ToList(), instance.GoalIds.ToList());
            CollectionAssert.AreEqual(pocIds.ToList(), instance.PointsOfContactIds.ToList());
            CollectionAssert.AreEqual(themeIds.ToList(), instance.ThemeIds.ToList());

            var update = (Update)instance.Audit;
            Assert.AreEqual(user.Id, update.User.Id);
            DateTimeOffset.UtcNow.Should().BeCloseTo(update.Date, DbContextHelper.DATE_PRECISION);
        }

        [TestMethod]
        public void TestConstructor_DistinctEnumerables()
        {
            var projectId = 10;
            var projectStatusId = ProjectStatus.Active.Id;
            var name = "name";
            var description = "description";
            var goalIds = new List<int> { 1, 1 };
            var pocIds = new List<int> { 2, 2 };
            var themeIds = new List<int> { 3, 3 };
            var focusId = 4;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow.AddDays(1.0);
            var user = new User(1);
            var instance = new PublishedProject(
                updatedBy: user,
                projectId: projectId,
                name: name,
                description: description,
                projectStatusId: projectStatusId,
                goalIds: goalIds,
                themeIds: themeIds,
                pointsOfContactIds: pocIds,
                focusId: focusId,
                startDate: startDate,
                endDate: endDate
                );

            CollectionAssert.AreEqual(goalIds.Distinct().ToList(), instance.GoalIds.ToList());
            CollectionAssert.AreEqual(pocIds.Distinct().ToList(), instance.PointsOfContactIds.ToList());
            CollectionAssert.AreEqual(themeIds.Distinct().ToList(), instance.ThemeIds.ToList());
        }

        [TestMethod]
        public void TestConstructor_NullEnumerables()
        {
            var projectId = 10;
            var projectStatusId = ProjectStatus.Active.Id;
            var name = "name";
            var description = "description";
            var focusId = 4;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow.AddDays(1.0);
            var user = new User(1);
            var instance = new PublishedProject(
                updatedBy: user,
                projectId: projectId,
                name: name,
                description: description,
                projectStatusId: projectStatusId,
                goalIds: null,
                themeIds: null,
                pointsOfContactIds: null,
                focusId: focusId,
                startDate: startDate,
                endDate: endDate
                );
            Assert.IsNotNull(instance.ThemeIds);
            Assert.IsNotNull(instance.GoalIds);
            Assert.IsNotNull(instance.PointsOfContactIds);
        }
    }
}
