using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Projects;
using ECA.Business.Service;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ECA.Data;

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

        [TestMethod]
        public void TestNameMaxLength()
        {
            var project = new DraftProjectBindingModel
            {
                Name = new string('a', Project.MAX_NAME_LENGTH),
                Description = "desc",
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
            var project = new DraftProjectBindingModel
            {
                Name = new string('a', Project.MAX_NAME_LENGTH),
                Description = new string('a', Project.MAX_DESCRIPTION_LENGTH),
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
