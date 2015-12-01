using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Security;

namespace ECA.WebApi.Test.Models.Security
{
    [TestClass]
    public class ResourcePermissionViewModelTest
    {
        [TestMethod]
        public void TestGetHashCode()
        {
            var permissionId = 1;
            var model = new ResourcePermissionViewModel();
            model.PermissionId = permissionId;
            Assert.AreEqual(permissionId.GetHashCode(), model.GetHashCode());
        }

        [TestMethod]
        public void TestEquals_SamePermissionId()
        {
            var permissionId = 1;
            var model = new ResourcePermissionViewModel();
            model.PermissionId = permissionId;

            var model2 = new ResourcePermissionViewModel();
            model2.PermissionId = permissionId;

            Assert.IsTrue(model.Equals(model2));
            Assert.IsTrue(model2.Equals(model));
        }

        [TestMethod]
        public void TestEquals_DifferentPermissionId()
        {
            var permissionId = 1;
            var model = new ResourcePermissionViewModel();
            model.PermissionId = permissionId;

            var model2 = new ResourcePermissionViewModel();
            model2.PermissionId = permissionId +1;

            Assert.IsFalse(model.Equals(model2));
            Assert.IsFalse(model2.Equals(model));
        }

        [TestMethod]
        public void TestEquals_NullTestObject()
        {
            Assert.IsFalse(new ResourcePermissionViewModel().Equals(null));
        }

        [TestMethod]
        public void TestEquals_DifferentTypeObject()
        {
            Assert.IsFalse(new ResourcePermissionViewModel().Equals(1));
        }
    }
}
