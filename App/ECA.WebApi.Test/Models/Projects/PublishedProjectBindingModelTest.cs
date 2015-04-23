using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Projects;
using System.Collections.Generic;
using ECA.Data;
using ECA.Business.Service;

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
                FocusId = 1,
                GoalIds = new List<int> { 1 },
                Name = "name",
                PointsOfContactIds = new List<int> { 2 },
                Id = 3,
                ProjectStatusId = ProjectStatus.Completed.Id,
                StartDate = DateTimeOffset.UtcNow.AddDays(-1.0),
                ThemeIds = new List<int> { 4 },
            };
            var user = new User(1);
            var publishedProject = model.ToPublishedProject(user);
            Assert.AreEqual(user.Id, publishedProject.Audit.User.Id);
            Assert.AreEqual(model.Description, publishedProject.Description);
            Assert.AreEqual(model.EndDate, publishedProject.EndDate);
            Assert.AreEqual(model.FocusId, publishedProject.FocusId);
            Assert.AreEqual(model.Name, publishedProject.Name);
            Assert.AreEqual(model.Id, publishedProject.ProjectId);
            Assert.AreEqual(model.ProjectStatusId, publishedProject.ProjectStatusId);
            Assert.AreEqual(model.StartDate, publishedProject.StartDate);

            CollectionAssert.AreEqual(model.GoalIds.ToList(), publishedProject.GoalIds.ToList());
            CollectionAssert.AreEqual(model.PointsOfContactIds.ToList(), publishedProject.PointsOfContactIds.ToList());
            CollectionAssert.AreEqual(model.ThemeIds.ToList(), publishedProject.ThemeIds.ToList());
        }
    }
}
