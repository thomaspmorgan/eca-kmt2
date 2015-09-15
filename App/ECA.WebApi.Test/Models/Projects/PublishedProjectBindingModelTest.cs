using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Projects;
using System.Collections.Generic;
using ECA.Data;
using ECA.Business.Service;
using System.ComponentModel.DataAnnotations;

namespace ECA.WebApi.Test.Models.Projects
{
    [TestClass]
    public class PublishedProjectBindingModelTest
    {
        [TestMethod]
        public void TestToPublishedProject()
        {
            var model = new PublishedProjectBindingModel
            {
                Description = "desc",
                EndDate = DateTimeOffset.UtcNow.AddDays(1.0),
                GoalIds = new List<int> { 1 },
                Name = "name",
                PointsOfContactIds = new List<int> { 2 },
                Id = 3,
                ProjectStatusId = ProjectStatus.Completed.Id,
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0),
                ThemeIds = new List<int> { 4 },
                ObjectiveIds = new List<int> { 5 },
                CategoryIds = new List<int> { 6 },
                LocationIds = new List<int> { 7 }
            };
            var user = new User(1);
            var publishedProject = model.ToPublishedProject(user);
            Assert.AreEqual(user.Id, publishedProject.Audit.User.Id);
            Assert.AreEqual(model.Description, publishedProject.Description);
            Assert.AreEqual(model.EndDate, publishedProject.EndDate);
            Assert.AreEqual(model.Name, publishedProject.Name);
            Assert.AreEqual(model.Id, publishedProject.ProjectId);
            Assert.AreEqual(model.ProjectStatusId, publishedProject.ProjectStatusId);
            Assert.AreEqual(model.StartDate, publishedProject.StartDate);

            CollectionAssert.AreEqual(model.GoalIds.ToList(), publishedProject.GoalIds.ToList());
            CollectionAssert.AreEqual(model.PointsOfContactIds.ToList(), publishedProject.PointsOfContactIds.ToList());
            CollectionAssert.AreEqual(model.ThemeIds.ToList(), publishedProject.ThemeIds.ToList());
            CollectionAssert.AreEqual(model.ObjectiveIds.ToList(), publishedProject.ObjectiveIds.ToList());
            CollectionAssert.AreEqual(model.CategoryIds.ToList(), publishedProject.CategoryIds.ToList());
            CollectionAssert.AreEqual(model.LocationIds.ToList(), publishedProject.LocationIds.ToList());
        }



        [TestMethod]
        public void TestNameMaxLength()
        {
            var project = new PublishedProjectBindingModel
            {
                Name = new string('a', Project.MAX_NAME_LENGTH),
                Description = "desc",
                GoalIds = new List<int> { 1 },
                PointsOfContactIds = new List<int> { 2 },
                ThemeIds = new List<int> { 4 },
                ObjectiveIds = new List<int> { 5 },
                CategoryIds = new List<int> { 6 },
                LocationIds = new List<int> { 7 }
            };
            var items = new Dictionary<object, object>();
            var vc = new ValidationContext(project, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(project, vc, results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            project.Name = new string('a', Project.MAX_NAME_LENGTH + 1);

            actual = Validator.TryValidateObject(project, vc, results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());
        }

        [TestMethod]
        public void TestDescriptionMaxLength()
        {
            var project = new PublishedProjectBindingModel
            {
                Name = new string('a', Project.MAX_NAME_LENGTH),
                Description = new string('a', Project.MAX_DESCRIPTION_LENGTH),
                GoalIds = new List<int> { 1 },
                PointsOfContactIds = new List<int> { 2 },
                ThemeIds = new List<int> { 4 },
                ObjectiveIds = new List<int> { 5 },
                CategoryIds = new List<int> { 6 },
                LocationIds = new List<int> { 7 }
            };
            var items = new Dictionary<object, object>();
            var vc = new ValidationContext(project, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(project, vc, results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            project.Description = new string('a', Project.MAX_DESCRIPTION_LENGTH + 1);

            actual = Validator.TryValidateObject(project, vc, results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Description", results.First().MemberNames.First());
        }
    }
}
