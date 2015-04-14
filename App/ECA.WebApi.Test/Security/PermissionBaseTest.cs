using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Security;
using ECA.WebApi.Models.Query;

namespace ECA.WebApi.Test.Security
{
  

    [TestClass]
    public class PermissionBaseTest
    {
        [TestMethod]
        public void TestParse_SingleModelPermission()
        {
            var modelTypeName = typeof(SorterBindingModel).Name;
            var idPropertyName = "id";
            var permissionName = "Read";
            var resourceType = "Program";
            var permissionAsString = new ModelPermission(idPropertyName, typeof(SorterBindingModel), permissionName, resourceType).ToString();

            var actionPermissions = PermissionBase.Parse(permissionAsString);

            Assert.AreEqual(1, actionPermissions.Count());
            Assert.IsInstanceOfType(actionPermissions.First(), typeof(ModelPermission));

            var actionPermission = (ModelPermission)actionPermissions.First();
            Assert.AreEqual(typeof(SorterBindingModel), actionPermission.ModelType);
            Assert.AreEqual(idPropertyName, actionPermission.Property);
            Assert.AreEqual(resourceType, actionPermission.ResourceType);
            Assert.AreEqual(permissionName, actionPermission.PermissionName);
        }

        [TestMethod]
        public void TestParse_SingleStaticPermission()
        {
            var id = 1;
            var permissionName = "Read";
            var resourceType = "Program";
            var permissionAsString = new StaticPermission() { PermissionName = permissionName, ResourceId = id, ResourceType = resourceType }.ToString();

            var actionPermissions = PermissionBase.Parse(permissionAsString);

            Assert.AreEqual(1, actionPermissions.Count());
            Assert.IsInstanceOfType(actionPermissions.First(), typeof(StaticPermission));

            var actionPermission = (StaticPermission)actionPermissions.First();
            Assert.AreEqual(id, actionPermission.ResourceId);
            Assert.AreEqual(permissionName, actionPermission.PermissionName);
            Assert.AreEqual(resourceType, actionPermission.ResourceType);
        }

        [TestMethod]
        public void TestParse_SingleActionPermission()
        {
            var actionArgument = "id";
            var permissionName = "Read";
            var resourceType = "Program";
            var permissionAsString = new ActionPermission { ArgumentName = actionArgument, PermissionName = permissionName, ResourceType = resourceType }.ToString();

            var actionPermissions = PermissionBase.Parse(permissionAsString);

            Assert.AreEqual(1, actionPermissions.Count());
            Assert.IsInstanceOfType(actionPermissions.First(), typeof(ActionPermission));

            var actionPermission = (ActionPermission)actionPermissions.First();
            Assert.AreEqual(actionArgument, actionPermission.ArgumentName);
            Assert.AreEqual(permissionName, actionPermission.PermissionName);
            Assert.AreEqual(resourceType, actionPermission.ResourceType);
        }

        [TestMethod]
        public void TestParse_TwoActionPermissions()
        {
            var actionArgument1 = "id";
            var permissionName1 = "Read";
            var resourceType1 = "Program";
            var permissionAsString1 = new ActionPermission { ArgumentName = actionArgument1, PermissionName = permissionName1, ResourceType = resourceType1 }.ToString();

            var actionArgument2 = "projectId";
            var permissionName2 = "Edit";
            var resourceType2 = "Project";
            var permissionAsString2 = new ActionPermission { ArgumentName = actionArgument2, PermissionName = permissionName2, ResourceType = resourceType2 }.ToString();

            var parsedPermissions = ActionPermission.Parse(String.Join(", ", permissionAsString1, permissionAsString2));
            Assert.AreEqual(2, parsedPermissions.Count());
            Assert.IsInstanceOfType(parsedPermissions.First(), typeof(ActionPermission));
            Assert.IsInstanceOfType(parsedPermissions.Last(), typeof(ActionPermission));

            var parsedPermission1 = (ActionPermission)parsedPermissions.First();
            Assert.AreEqual(actionArgument1, parsedPermission1.ArgumentName);
            Assert.AreEqual(permissionName1, parsedPermission1.PermissionName);
            Assert.AreEqual(resourceType1, parsedPermission1.ResourceType);

            var parsedPermission2 = (ActionPermission)parsedPermissions.Last();
            Assert.AreEqual(actionArgument2, parsedPermission2.ArgumentName);
            Assert.AreEqual(permissionName2, parsedPermission2.PermissionName);
            Assert.AreEqual(resourceType2, parsedPermission2.ResourceType);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestParse_InvalidFormattedPermission()
        {
            var actionArgument1 = "id";
            var permissionName1 = "Read";
            var resourceType1 = "Program";
            var permissionAsString1 = String.Format("{0},{1}-{2}", permissionName1, resourceType1, actionArgument1);
            PermissionBase.Parse(permissionAsString1).ToList();
        }

        [TestMethod]
        public void TestGetTypeByName()
        {
            var t = typeof(SorterBindingModel);
            var testType = PermissionBase.GetTypeByName(t.Name);
            Assert.AreEqual(t, testType);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestGetTypeByName_TypeDoesNotExistInApi()
        {
            var t = typeof(PermissionBaseTest);
            PermissionBase.GetTypeByName(t.Name);
        }
    }
}
