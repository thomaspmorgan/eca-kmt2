using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Projects;
using ECA.Core.Exceptions;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Business.Test.Service.Projects
{
    [TestClass]
    public class ProjectServiceUpdateValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                projectId: 1,
                name: "name",
                description: "description",
                projectStatusId: ProjectStatus.Other.Id,
                themeIds: null,
                goalIds: null,
                pointsOfContactIds: null,
                categoryIds: new List<int> { 1 },
                objectiveIds: new List<int> { 2 },
                locationIds: new List<int> { 3 },
                regionIds: new List<int> { 4 },
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow,
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null);
                
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Draft.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int> { 1 };
            var allowedObjectiveIds = new List<int> { 2 };
            var newInactiveLocationIds = new List<int> { 3 };
            List<int> regionLocationTypeIds = new List<int> { LocationType.Region.Id };
            var allowedThemeIds = new List<int> { 1 };
            var allowedGoalIds = new List<int> { 1 };

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,                
                project,
                newInactiveLocationIds,
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                allowedCategoryIds,
                allowedObjectiveIds,
                regionLocationTypeIds,
                officeSettings,
                allowedThemeIds,
                allowedGoalIds);
            Assert.AreEqual(updatedProject.Name, instance.Name);
            Assert.AreEqual(updatedProject.Description, instance.Description);
            Assert.AreEqual(updatedProject.StartDate, instance.StartDate);
            Assert.AreEqual(updatedProject.EndDate, instance.EndDate);
            Assert.AreEqual(updatedProject.ProjectStatusId, instance.UpdatedProjectStatusId);
            Assert.AreEqual(project.ProjectStatusId, instance.OriginalProjectStatusId);
            Assert.AreEqual(goalsExist, instance.GoalsExist);
            Assert.AreEqual(themesExist, instance.ThemesExist);
            Assert.AreEqual(pointsOfContactExist, instance.PointsOfContactExist);
            Assert.AreEqual(numberOfCategories, instance.NumberOfCategories);
            Assert.AreEqual(numberOfObjectives, instance.NumberOfObjectives);
            CollectionAssert.AreEqual(allowedCategoryIds.ToList(), instance.AllowedCategoryIds.ToList());
            CollectionAssert.AreEqual(allowedObjectiveIds.ToList(), instance.AllowedObjectiveIds.ToList());
            CollectionAssert.AreEqual(updatedProject.ObjectiveIds.ToList(), instance.ObjectiveIds.ToList());
            CollectionAssert.AreEqual(updatedProject.CategoryIds.ToList(), instance.CategoryIds.ToList());
            CollectionAssert.AreEqual(newInactiveLocationIds, instance.NewInactiveLocationIds.ToList());
            Assert.IsTrue(Object.ReferenceEquals(officeSettings, instance.OfficeSettings));
            CollectionAssert.AreEqual(allowedThemeIds.ToList(), instance.AllowedThemeIds.ToList());
            CollectionAssert.AreEqual(allowedGoalIds.ToList(), instance.AllowedGoalIds.ToList());
        }

        [TestMethod]
        public void TestConstructor_NullAllowedCategoryIds()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                projectId: 1,
                name: "name",
                description: "description",
                projectStatusId: ProjectStatus.Other.Id,
                themeIds: null,
                goalIds: null,
                pointsOfContactIds: null,
                locationIds: null,
                categoryIds: null,
                objectiveIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow,
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Draft.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;
            var officeSettings = new OfficeSettings();
            List<int> allowedCategoryIds = null;
            List<int> allowedObjectiveIds = new List<int>();
            List<int> newInactiveLocationIds = new List<int>();
            List<int> regionLocationTypeIds = new List<int> { LocationType.Region.Id };
            List<int> allowedThemeIds = new List<int>();
            List<int> allowedGoalIds = new List<int>();

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                newInactiveLocationIds,
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                allowedCategoryIds,
                allowedObjectiveIds,
                regionLocationTypeIds,
                officeSettings,
                allowedThemeIds,
                allowedGoalIds);
            Assert.IsNotNull(instance.AllowedCategoryIds);
        }

        [TestMethod]
        public void TestConstructor_DistinctAllowedCategoryIds()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                projectId: 1,
                name: "name",
                description: "description",
                projectStatusId: ProjectStatus.Other.Id,
                themeIds: null,
                goalIds: null,
                pointsOfContactIds: null,
                locationIds: null,
                categoryIds: null,
                objectiveIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow,
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Draft.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;
            var officeSettings = new OfficeSettings();
            List<int> allowedCategoryIds = new List<int> { 1, 1 };
            List<int> allowedObjectiveIds = new List<int>();
            List<int> newInactiveLocationIds = new List<int>();
            List<int> regionLocationTypeIds = new List<int> { LocationType.Region.Id };
            List<int> allowedThemeIds = new List<int>();
            List<int> allowedGoalIds = new List<int>();

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                newInactiveLocationIds,
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                allowedCategoryIds,
                allowedObjectiveIds,
                regionLocationTypeIds,
                officeSettings,
                allowedThemeIds,
                allowedGoalIds);
            CollectionAssert.AreEqual(allowedCategoryIds.Distinct().ToList(), instance.AllowedCategoryIds.ToList());
        }


        [TestMethod]
        public void TestConstructor_DistinctAllowedObjectiveIds()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                projectId: 1,
                name: "name",
                description: "description",
                projectStatusId: ProjectStatus.Other.Id,
                themeIds: null,
                goalIds: null,
                pointsOfContactIds: null,
                locationIds: null,
                categoryIds: null,
                objectiveIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow,
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Draft.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;
            var officeSettings = new OfficeSettings();
            List<int> allowedCategoryIds = new List<int>();
            List<int> allowedObjectiveIds = new List<int> { 1, 1 };
            List<int> newInactiveLocationIds = new List<int>();
            List<int> regionLocationTypeIds = new List<int> { LocationType.Region.Id };
            List<int> allowedThemeIds = new List<int>();
            List<int> allowedGoalIds = new List<int>();

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                newInactiveLocationIds,
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                allowedCategoryIds,
                allowedObjectiveIds,
                regionLocationTypeIds,
                officeSettings,
                allowedThemeIds,
                allowedGoalIds);
            CollectionAssert.AreEqual(allowedObjectiveIds.Distinct().ToList(), instance.AllowedObjectiveIds.ToList());
        }

        [TestMethod]
        public void TestConstructor_NullAllowedObjectiveIds()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                projectId: 1,
                name: "name",
                description: "description",
                projectStatusId: ProjectStatus.Other.Id,
                themeIds: null,
                goalIds: null,
                pointsOfContactIds: null,
                locationIds: null,
                categoryIds: null,
                objectiveIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow,
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Draft.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;
            var officeSettings = new OfficeSettings();
            List<int> allowedCategoryIds = new List<int>();
            List<int> allowedObjectiveIds = null;
            List<int> newInactiveLocationIds = new List<int>();
            List<int> regionLocationTypeIds = new List<int> { LocationType.Region.Id };
            List<int> allowedThemeIds = new List<int>();
            List<int> allowedGoalIds = new List<int>();

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                newInactiveLocationIds,
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                allowedCategoryIds,
                allowedObjectiveIds,
                regionLocationTypeIds,
                officeSettings,
                allowedThemeIds,
                allowedGoalIds);
            Assert.IsNotNull(instance.AllowedObjectiveIds);
        }

        [TestMethod]
        public void TestConstructor_DistinctNewLocationInactiveLocationIds()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                projectId: 1,
                name: "name",
                description: "description",
                projectStatusId: ProjectStatus.Other.Id,
                themeIds: null,
                goalIds: null,
                pointsOfContactIds: null,
                locationIds: null,
                categoryIds: null,
                objectiveIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow,
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Draft.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;
            var officeSettings = new OfficeSettings();
            List<int> allowedCategoryIds = new List<int>();
            List<int> allowedObjectiveIds = new List<int>();
            List<int> newInactiveLocationIds = new List<int> { 1, 1 };
            List<int> regionLocationTypeIds = new List<int> { LocationType.Region.Id };
            List<int> allowedThemeIds = new List<int>();
            List<int> allowedGoalIds = new List<int>();

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                newInactiveLocationIds,
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                allowedCategoryIds,
                allowedObjectiveIds,
                regionLocationTypeIds,
                officeSettings,
                allowedThemeIds,
                allowedGoalIds);
            CollectionAssert.AreEqual(newInactiveLocationIds.Distinct().ToList(), instance.NewInactiveLocationIds.ToList());
        }

        [TestMethod]
        public void TestConstructor_NullNewInactiveLocationIds()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                projectId: 1,
                name: "name",
                description: "description",
                projectStatusId: ProjectStatus.Other.Id,
                themeIds: null,
                goalIds: null,
                pointsOfContactIds: null,
                locationIds: null,
                categoryIds: null,
                objectiveIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow,
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Draft.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;
            var officeSettings = new OfficeSettings();
            List<int> allowedCategoryIds = new List<int>();
            List<int> allowedObjectiveIds = new List<int>();
            List<int> newInactiveLocationIds = null;
            List<int> regionLocationTypeIds = new List<int> { LocationType.Region.Id };
            List<int> allowedThemeIds = new List<int>();
            List<int> allowedGoalIds = new List<int>();

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                newInactiveLocationIds,
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                allowedCategoryIds,
                allowedObjectiveIds,
                regionLocationTypeIds,
                officeSettings,
                allowedThemeIds,
                allowedGoalIds);
            Assert.IsNotNull(instance.NewInactiveLocationIds);
        }

        [TestMethod]
        public void TestConstructor_DistinctRegionLocationTypeIds()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                projectId: 1,
                name: "name",
                description: "description",
                projectStatusId: ProjectStatus.Other.Id,
                themeIds: null,
                goalIds: null,
                pointsOfContactIds: null,
                locationIds: null,
                categoryIds: null,
                objectiveIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow,
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Draft.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;
            var officeSettings = new OfficeSettings();
            List<int> allowedCategoryIds = new List<int>();
            List<int> allowedObjectiveIds = new List<int>();
            List<int> newInactiveLocationIds = new List<int>();
            List<int> regionLocationTypeIds = new List<int> { LocationType.Region.Id, LocationType.Region.Id };
            List<int> allowedThemeIds = new List<int>();
            List<int> allowedGoalIds = new List<int>();

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                newInactiveLocationIds,
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                allowedCategoryIds,
                allowedObjectiveIds,
                regionLocationTypeIds,
                officeSettings,
                allowedThemeIds,
                allowedGoalIds);
            CollectionAssert.AreEqual(regionLocationTypeIds.Distinct().ToList(), instance.RegionLocationTypeIds.ToList());
        }

        [TestMethod]
        public void TestConstructor_NullRegionLocationTypeIds()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                projectId: 1,
                name: "name",
                description: "description",
                projectStatusId: ProjectStatus.Other.Id,
                themeIds: null,
                goalIds: null,
                pointsOfContactIds: null,
                locationIds: null,
                categoryIds: null,
                objectiveIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow,
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Draft.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;
            var officeSettings = new OfficeSettings();
            List<int> allowedCategoryIds = new List<int>();
            List<int> allowedObjectiveIds = new List<int>();
            List<int> newInactiveLocationIds = new List<int>();
            List<int> regionLocationTypeIds = null;
            List<int> allowedThemeIds = new List<int>();
            List<int> allowedGoalIds = new List<int>();

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                newInactiveLocationIds,
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                allowedCategoryIds,
                allowedObjectiveIds,
                regionLocationTypeIds,
                officeSettings,
                allowedThemeIds,
                allowedGoalIds);
            Assert.IsNotNull(instance.RegionLocationTypeIds);
        }

        [TestMethod]
        public void TestConstructor_CheckGoalsExistBool()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                1,
                "name",
                "description",
                1,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                1,
                null,
                null,
                null,
                null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Other.Id
            };
            var goalsExist = false;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                new List<int>(),
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new OfficeSettings(),
                new List<int>(),
                new List<int>());
            Assert.AreEqual(pointsOfContactExist, instance.PointsOfContactExist);
            Assert.AreEqual(themesExist, instance.ThemesExist);
            Assert.AreEqual(goalsExist, instance.GoalsExist);
            Assert.AreEqual(locationsExist, instance.LocationsExist);
        }

        [TestMethod]
        public void TestConstructor_CheckThemesExistBool()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                1,
                "name",
                "description",
                1,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                1,
                null,
                null,
                null,
                null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Other.Id
            };
            var goalsExist = true;
            var themesExist = false;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                new List<int>(),
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new OfficeSettings(),
                new List<int>(),
                new List<int>());
            Assert.AreEqual(pointsOfContactExist, instance.PointsOfContactExist);
            Assert.AreEqual(themesExist, instance.ThemesExist);
            Assert.AreEqual(goalsExist, instance.GoalsExist);
            Assert.AreEqual(locationsExist, instance.LocationsExist);
        }

        [TestMethod]
        public void TestConstructor_CheckPointsOfContactExistBool()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                1,
                "name",
                "description",
                1,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                1,
                null,
                null,
                null,
                null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Other.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = false;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                new List<int>(),
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new OfficeSettings(),
                new List<int>(),
                new List<int>());
            Assert.AreEqual(pointsOfContactExist, instance.PointsOfContactExist);
            Assert.AreEqual(themesExist, instance.ThemesExist);
            Assert.AreEqual(goalsExist, instance.GoalsExist);
            Assert.AreEqual(locationsExist, instance.LocationsExist);
        }

        [TestMethod]
        public void TestConstructor_CheckLocationsExistBool()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                1,
                "name",
                "description",
                1,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                1,
                null,
                null,
                null,
                null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Other.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = false;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                new List<int>(),
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new OfficeSettings(),
                new List<int>(),
                new List<int>());
            Assert.AreEqual(pointsOfContactExist, instance.PointsOfContactExist);
            Assert.AreEqual(themesExist, instance.ThemesExist);
            Assert.AreEqual(goalsExist, instance.GoalsExist);
            Assert.AreEqual(locationsExist, instance.LocationsExist);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownStaticLookupException))]
        public void TestConstructor_UnknownProjectStatusLookupId()
        {
            var updatedProject = new PublishedProject(
                 new User(1),
                 projectId: 1,
                 name: "name",
                 description: "description",
                 projectStatusId: 0,
                 themeIds: null,
                 goalIds: null,
                 pointsOfContactIds: null,
                 locationIds: null,
                 categoryIds: null,
                 objectiveIds: null,
                 regionIds: null,
                 startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                 endDate: DateTimeOffset.UtcNow,
                 visitorTypeId: 1,
                 usParticipantsEst: null,
                 nonUsParticipantsEst: null,
                 usParticipantsActual: null,
                 nonUsParticipantsActual: null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Other.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = false;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                new List<int>(),
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new OfficeSettings(),
                new List<int>(),
                new List<int>());
        }

        [TestMethod]
        public void TestConstructor_CheckThemesGoalsNull()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                1,
                "name",
                "description",
                1,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                1,
                null,
                null,
                null,
                null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Other.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = false;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                new List<int>(),
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new OfficeSettings(),
                null,
                null);
            Assert.IsNotNull(instance.AllowedThemeIds);
            Assert.IsNotNull(instance.AllowedGoalIds);
        }

        [TestMethod]
        public void TestConstructor_CheckThemesGoalsDistinct()
        {
            var updatedProject = new PublishedProject(
                new User(1),
                1,
                "name",
                "description",
                1,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                1,
                null,
                null,
                null,
                null);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Other.Id
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = false;
            var numberOfCategories = 10;
            var numberOfObjectives = 20;

            var instance = new ProjectServiceUpdateValidationEntity(
                updatedProject,
                project,
                new List<int>(),
                goalsExist,
                themesExist,
                pointsOfContactExist,
                categoriesExist,
                objectivesExist,
                locationsExist,
                numberOfObjectives,
                numberOfCategories,
                new List<int>(),
                new List<int>(),
                new List<int>(),
                new OfficeSettings(),
                new List<int> { 1, 1 },
                new List<int> { 2, 2});
            CollectionAssert.AreEqual(new List<int> { 1 }, instance.AllowedThemeIds.ToList());
            CollectionAssert.AreEqual(new List<int> { 2 }, instance.AllowedGoalIds.ToList());
        }
    }
}
