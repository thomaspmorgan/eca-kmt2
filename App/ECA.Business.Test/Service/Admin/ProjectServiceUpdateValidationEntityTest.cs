using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;
using ECA.Business.Service;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Admin
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
                focusId: 1,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Draft.Id
            };
            var focus = new Focus();
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = true;

            var instance = new ProjectServiceUpdateValidationEntity(updatedProject, project, focus, goalsExist, themesExist, pointsOfContactExist);
            Assert.IsTrue(object.ReferenceEquals(focus, instance.Focus));
            Assert.AreEqual(updatedProject.Name, instance.Name);
            Assert.AreEqual(updatedProject.Description, instance.Description);
            Assert.AreEqual(updatedProject.StartDate, instance.StartDate);
            Assert.AreEqual(updatedProject.EndDate, instance.EndDate);
            Assert.AreEqual(updatedProject.ProjectStatusId, instance.UpdatedProjectStatusId);
            Assert.AreEqual(project.ProjectStatusId, instance.OriginalProjectStatusId);
            Assert.AreEqual(goalsExist, instance.GoalsExist);
            Assert.AreEqual(themesExist, instance.ThemesExist);
            Assert.AreEqual(pointsOfContactExist, instance.PointsOfContactExist);
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
                1,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Other.Id
            };
            var focus = new Focus();
            var goalsExist = false;
            var themesExist = true;
            var pointsOfContactExist = true;

            var instance = new ProjectServiceUpdateValidationEntity(updatedProject, project, focus, goalsExist, themesExist, pointsOfContactExist);
            Assert.AreEqual(pointsOfContactExist, instance.PointsOfContactExist);
            Assert.AreEqual(themesExist, instance.ThemesExist);
            Assert.AreEqual(goalsExist, instance.GoalsExist);
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
                1,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Other.Id
            };
            var focus = new Focus();
            var goalsExist = true;
            var themesExist = false;
            var pointsOfContactExist = true;

            var instance = new ProjectServiceUpdateValidationEntity(updatedProject, project, focus, goalsExist, themesExist, pointsOfContactExist);
            Assert.AreEqual(pointsOfContactExist, instance.PointsOfContactExist);
            Assert.AreEqual(themesExist, instance.ThemesExist);
            Assert.AreEqual(goalsExist, instance.GoalsExist);
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
                1,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Other.Id
            };
            var focus = new Focus();
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = false;

            var instance = new ProjectServiceUpdateValidationEntity(updatedProject, project, focus, goalsExist, themesExist, pointsOfContactExist);
            Assert.AreEqual(pointsOfContactExist, instance.PointsOfContactExist);
            Assert.AreEqual(themesExist, instance.ThemesExist);
            Assert.AreEqual(goalsExist, instance.GoalsExist);
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
                 focusId: 1,
                 startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                 endDate: DateTimeOffset.UtcNow);
            var project = new Project
            {
                ProjectStatusId = ProjectStatus.Other.Id
            };
            var focus = new Focus();
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactExist = false;

            var instance = new ProjectServiceUpdateValidationEntity(updatedProject, project, focus, goalsExist, themesExist, pointsOfContactExist);
        }
    }
}
