using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Projects;
using ECA.Business.Service;

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
            var user = new User(1);
            var draftProject = model.ToDraftProject(user);
            Assert.AreEqual(user.Id, draftProject.Audit.User.Id);
            Assert.AreEqual(model.Description, draftProject.Description);
            Assert.AreEqual(model.Name, draftProject.Name);
            Assert.AreEqual(model.ProgramId, draftProject.ProgramId);
        }
    }
}
