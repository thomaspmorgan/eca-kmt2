using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECA.Data.Test
{
    [TestClass]
    public class ProjectTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestGetId()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            Assert.AreEqual(project.ProjectId, project.GetId());
        }

        [TestMethod]
        public void TestProjectName_Unique()
        {
            var parentProgram = new Program
            {
                Name = "parent program",
                ProgramId = 2
            };
            var existingProject = new Project
            {
                Name = "  HELLO  ",
                ProgramId = 1,
                ParentProgram = parentProgram,
                ProjectId = 1
            };
            context.Projects.Add(existingProject);
            context.Programs.Add(parentProgram);

            var testProject = new Project
            {
                ProgramId = parentProgram.ProgramId + 1,
                Name = "  hello ",
                Description = "desc"

            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(testProject, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(testProject, vc, results);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());

            var expectedErrorMessage = String.Format("The project with the name [{0}] already exists.",
                        testProject.Name);
            Assert.AreEqual(expectedErrorMessage, results.First().ErrorMessage);
        }
    }
}
