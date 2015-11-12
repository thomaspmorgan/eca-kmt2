using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ECA.Core.Data;

namespace ECA.Data.Test
{
    [TestClass]
    public class ProgramTest
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
            var program = new Program
            {
                ProgramId = 1
            };
            Assert.AreEqual(program.ProgramId, program.GetId());
        }

        #region Permissable
        [TestMethod]
        public void TestAssignPermissionToRoleOnCreate_NullRoleName()
        {
            var permissable = new Program();
            string roleName = null;
            Assert.IsFalse(permissable.AssignPermissionToRoleOnCreate(roleName, null));
        }

        [TestMethod]
        public void TestAssignPermissionToRoleOnCreate_EmptyRoleName()
        {
            var permissable = new Program();
            string roleName = String.Empty;
            Assert.IsFalse(permissable.AssignPermissionToRoleOnCreate(roleName, null));
        }


        [TestMethod]
        public void TestAssignPermissionToRoleOnCreate_WhitespaceRoleName()
        {
            var permissable = new Program();
            string roleName = " ";
            Assert.IsFalse(permissable.AssignPermissionToRoleOnCreate(roleName, null));
        }

        [TestMethod]
        public void TestAssignPermissionToRoleOnCreate_NotTheKMTSuperUserRoleName()
        {
            var permissable = new Program();
            string roleName = "role";
            Assert.IsFalse(permissable.AssignPermissionToRoleOnCreate(roleName, null));
        }

        [TestMethod]
        public void TestAssignPermissionToRoleOnCreate_KMTSuperUserRoleName()
        {
            var permissable = new Program();
            string roleName = UserAccount.KMT_SUPER_USER_ROLE_NAME;
            Assert.IsTrue(permissable.AssignPermissionToRoleOnCreate(roleName, null));
        }

        [TestMethod]
        public void TestIsExempt()
        {
            var program = new Program
            {
                ProgramId = 1
            };
            var permissable = program as IPermissable;
            Assert.IsFalse(permissable.IsExempt());
        }

        [TestMethod]
        public void TestGetId_Permissable()
        {
            var program = new Program
            {
                ProgramId = 1
            };
            var permissable = program as IPermissable;
            Assert.AreEqual(program.ProgramId, permissable.GetId());
        }

        [TestMethod]
        public void TestGetPermissableType()
        {
            var program = new Program
            {
                ProgramId = 1
            };
            var permissable = program as IPermissable;
            Assert.AreEqual(PermissableType.Program, permissable.GetPermissableType());
        }

        [TestMethod]
        public void TestGetParentPermissableType()
        {
            var program = new Program
            {
                ProgramId = 1
            };
            var permissable = program as IPermissable;
            Assert.AreEqual(PermissableType.Office, permissable.GetParentPermissableType());
        }

        [TestMethod]
        public void TestParentId_Permissable_HasParentOrg()
        {
            var program = new Program
            {
                ProgramId = 1,
                OwnerId = 2
            };
            var permissable = program as IPermissable;
            Assert.AreEqual(program.OwnerId, permissable.GetParentId());
        }

        #endregion

        [TestMethod]
        public void TestGetId_ConcurrentEntity()
        {
            var program = new Program
            {
                ProgramId = 1
            };
            var concurrent = program as IConcurrentEntity;
            Assert.AreEqual(program.ProgramId, concurrent.GetId());
        }

        [TestMethod]
        public void TestNameIsUnique()
        {
            var existingProgram = new Program
            {
                Name = "  HELLO  ",
                ProgramId = 1
            };
            context.Programs.Add(existingProgram);

            var testProgram = new Program
            {
                ProgramId = 2,
                Name = "  hello ",
                Description = "desc",
                Owner = new Organization()
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(testProgram, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(testProgram, vc, results);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());
            Assert.AreEqual(String.Format("The program with the name [{0}] already exists.", testProgram.Name), results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestNameMaxLength()
        {
            var owner = new Organization();
            var program = new Program
            {
                ProgramId = 2,
                Name = new string('a', Program.MAX_NAME_LENGTH),
                Description = "desc",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(program, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(program, vc, results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            program.Name = new string('a', Program.MAX_NAME_LENGTH + 1);

            actual = Validator.TryValidateObject(program, vc, results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Name", results.First().MemberNames.First());
        }

        [TestMethod]
        public void TestDescriptionMaxLength()
        {
            var owner = new Organization();
            var program = new Program
            {
                ProgramId = 2,
                Name = new string('a', Program.MAX_NAME_LENGTH),
                Description = new string('a', Program.MAX_DESCRIPTION_LENGTH),
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(program, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(program, vc, results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            program.Description = new string('a', Program.MAX_DESCRIPTION_LENGTH + 1);

            actual = Validator.TryValidateObject(program, vc, results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Description", results.First().MemberNames.First());
        }
    }
}
