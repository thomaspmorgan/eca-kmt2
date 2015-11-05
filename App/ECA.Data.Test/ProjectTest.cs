using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ECA.Core.Data;

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

        #region Permissable
        [TestMethod]
        public void TestAssignPermissionToRoleOnCreate_NullRoleName()
        {
            var permissable = new Project();
            string roleName = null;
            Assert.IsFalse(permissable.AssignPermissionToRoleOnCreate(roleName, null));
        }

        [TestMethod]
        public void TestAssignPermissionToRoleOnCreate_EmptyRoleName()
        {
            var permissable = new Project();
            string roleName = String.Empty;
            Assert.IsFalse(permissable.AssignPermissionToRoleOnCreate(roleName, null));
        }


        [TestMethod]
        public void TestAssignPermissionToRoleOnCreate_WhitespaceRoleName()
        {
            var permissable = new Project();
            string roleName = " ";
            Assert.IsFalse(permissable.AssignPermissionToRoleOnCreate(roleName, null));
        }

        [TestMethod]
        public void TestAssignPermissionToRoleOnCreate_NotTheKMTSuperUserRoleName()
        {
            var permissable = new Project();
            string roleName = "role";
            Assert.IsFalse(permissable.AssignPermissionToRoleOnCreate(roleName, null));
        }

        [TestMethod]
        public void TestAssignPermissionToRoleOnCreate_KMTSuperUserRoleName()
        {
            var permissable = new Project();
            string roleName = UserAccount.KMT_SUPER_USER_ROLE_NAME;
            Assert.IsTrue(permissable.AssignPermissionToRoleOnCreate(roleName, null));
        }

        [TestMethod]
        public void TestIsExempt()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var permissable = project as IPermissable;
            Assert.IsFalse(permissable.IsExempt());
        }

        [TestMethod]
        public void TestGetId_Permissable()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var permissable = project as IPermissable;
            Assert.AreEqual(project.ProjectId, permissable.GetId());
        }

        [TestMethod]
        public void TestGetPermissableType()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var permissable = project as IPermissable;
            Assert.AreEqual(PermissableType.Project, permissable.GetPermissableType());
        }

        [TestMethod]
        public void TestGetParentPermissableType()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var permissable = project as IPermissable;
            Assert.AreEqual(PermissableType.Program, permissable.GetParentPermissableType());
        }

        [TestMethod]
        public void TestParentId_Permissable()
        {
            var project = new Project
            {
                ProjectId = 1,
                ProgramId = 2
            };
            var permissable = project as IPermissable;
            Assert.AreEqual(project.ProgramId, permissable.GetParentId());
        }

        #endregion

        [TestMethod]
        public void TestGetId_IConcurrentEntity()
        {
            var project = new Project
            {
                ProjectId = 1
            };
            var concurrent = project as IConcurrentEntity;
            Assert.AreEqual(project.ProjectId, concurrent.GetId());
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

        [TestMethod]
        public void TestNameMaxLength()
        {
            var project = new Project
            {
                ProgramId = 2,
                Name = new string('a', Project.MAX_NAME_LENGTH),
                Description = "desc",
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
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
            var project = new Project
            {
                ProgramId = 2,
                Name = new string('a', Project.MAX_NAME_LENGTH),
                Description = new string('a', Project.MAX_DESCRIPTION_LENGTH),
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
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
