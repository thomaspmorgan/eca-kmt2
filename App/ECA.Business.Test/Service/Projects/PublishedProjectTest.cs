﻿using ECA.Business.Service;
using ECA.Business.Service.Projects;
using ECA.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Business.Test.Service.Projects
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
            var sevisOrgId = "abcde1234567890";
            var goalIds = new List<int> { 1 };
            var pocIds = new List<int> { 2 };
            var themeIds = new List<int> { 3 };
            var categoryIds = new List<int> { 4 };
            var objectiveIds = new List<int> { 5 };
            var locationIds = new List<int> { 6 };
            var regionIds = new List<int> { 7 };
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow.AddDays(1.0);
            var user = new User(1);
            var usParticipantsEst = 5;
            var nonUsParticipantsEst = 3;
            var usParticipantsActual = 4;
            var nonUsParticipantsActual = 2;
            var instance = new PublishedProject(
                user,
                projectId,
                name,
                description,
                projectStatusId,
                sevisOrgId,
                goalIds,
                themeIds,
                pocIds,
                categoryIds,
                objectiveIds,
                locationIds,
                regionIds,
                startDate,
                endDate,
                1,
                usParticipantsEst,
                nonUsParticipantsEst,
                usParticipantsActual,
                nonUsParticipantsActual);

            Assert.AreEqual(projectId, instance.ProjectId);
            Assert.AreEqual(projectStatusId, instance.ProjectStatusId);
            Assert.AreEqual(sevisOrgId, instance.SevisOrgId);
            Assert.AreEqual(name, instance.Name);
            Assert.AreEqual(description, instance.Description);
            Assert.AreEqual(startDate, instance.StartDate);
            Assert.AreEqual(endDate, instance.EndDate);
            Assert.IsInstanceOfType(instance.Audit, typeof(Update));

            CollectionAssert.AreEqual(locationIds.ToList(), instance.LocationIds.ToList());
            CollectionAssert.AreEqual(goalIds.ToList(), instance.GoalIds.ToList());
            CollectionAssert.AreEqual(pocIds.ToList(), instance.PointsOfContactIds.ToList());
            CollectionAssert.AreEqual(themeIds.ToList(), instance.ThemeIds.ToList());
            CollectionAssert.AreEqual(categoryIds.Distinct().ToList(), instance.CategoryIds.ToList());
            CollectionAssert.AreEqual(objectiveIds.Distinct().ToList(), instance.ObjectiveIds.ToList());
            CollectionAssert.AreEqual(regionIds.Distinct().ToList(), instance.RegionIds.ToList());

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
            var sevisOrgId = "abcde1234567890";
            var goalIds = new List<int> { 1, 1 };
            var pocIds = new List<int> { 2, 2 };
            var themeIds = new List<int> { 3, 3 };
            var categoryIds = new List<int> { 4, 4 };
            var objectiveIds = new List<int> { 5, 5 };
            var locationIds = new List<int> { 6, 6 };
            var regionIds = new List<int> { 7, 7 };
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow.AddDays(1.0);
            var user = new User(1);
            var instance = new PublishedProject(
                updatedBy: user,
                projectId: projectId,
                name: name,
                description: description,
                projectStatusId: projectStatusId,
                sevisOrgId: sevisOrgId,
                goalIds: goalIds,
                themeIds: themeIds,
                pointsOfContactIds: pocIds,
                locationIds: locationIds,
                categoryIds: categoryIds,
                objectiveIds: objectiveIds,
                regionIds: regionIds,
                startDate: startDate,
                endDate: endDate,
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null
                );

            CollectionAssert.AreEqual(locationIds.Distinct().ToList(), instance.LocationIds.ToList());
            CollectionAssert.AreEqual(goalIds.Distinct().ToList(), instance.GoalIds.ToList());
            CollectionAssert.AreEqual(pocIds.Distinct().ToList(), instance.PointsOfContactIds.ToList());
            CollectionAssert.AreEqual(themeIds.Distinct().ToList(), instance.ThemeIds.ToList());
            CollectionAssert.AreEqual(categoryIds.Distinct().ToList(), instance.CategoryIds.ToList());
            CollectionAssert.AreEqual(objectiveIds.Distinct().ToList(), instance.ObjectiveIds.ToList());
            CollectionAssert.AreEqual(regionIds.Distinct().ToList(), instance.RegionIds.ToList());
        }

        [TestMethod]
        public void TestConstructor_NullEnumerables()
        {
            var projectId = 10;
            var projectStatusId = ProjectStatus.Active.Id;
            var name = "name";
            var description = "description";
            var sevisOrgId = "abcde1234567890";
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow.AddDays(1.0);
            var user = new User(1);
            var instance = new PublishedProject(
                updatedBy: user,
                projectId: projectId,
                name: name,
                description: description,
                projectStatusId: projectStatusId,
                sevisOrgId: sevisOrgId,
                goalIds: null,
                themeIds: null,
                pointsOfContactIds: null,
                categoryIds: null,
                objectiveIds: null,
                locationIds: null,
                regionIds: null,
                startDate: startDate,
                endDate: endDate,
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null
                );
            Assert.IsNotNull(instance.LocationIds);
            Assert.IsNotNull(instance.ThemeIds);
            Assert.IsNotNull(instance.GoalIds);
            Assert.IsNotNull(instance.PointsOfContactIds);
            Assert.IsNotNull(instance.CategoryIds);
            Assert.IsNotNull(instance.ObjectiveIds);
            Assert.IsNotNull(instance.RegionIds);
        }
    }
}
