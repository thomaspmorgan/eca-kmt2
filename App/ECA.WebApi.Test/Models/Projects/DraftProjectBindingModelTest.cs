using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Projects;

namespace ECA.WebApi.Test.Models.Projects
{
    [TestClass]
    public class DraftProjectBindingModelTest
    {
        [TestMethod]
        public void TestToDraftProject()
        {
            var model = new DraftProjectBindingModel
            {
                Description = "desc",
                Name = "name",
                ProgramId = 1
            };
            var userId = 1;
            var draftProject = model.ToDraftProject(userId);
            Assert.AreEqual(userId, draftProject.Audit.User.Id);
            Assert.AreEqual(model.Description, draftProject.Description);
            Assert.AreEqual(model.Name, draftProject.Name);
            Assert.AreEqual(model.ProgramId, draftProject.ProgramId);
        }
    }
}
