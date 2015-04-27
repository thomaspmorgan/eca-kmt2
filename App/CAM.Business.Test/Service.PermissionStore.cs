using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business;
using CAM.Data;
using System.Collections.Generic;

namespace CAM.Business.Test.Service
{
    [TestClass]
    public class PermissionStoreTest
    {
        [TestMethod]
        public void TestSinglePermission()
        {
            var model = new TestInMemoryCamModel();
            Business.Service.PermissionStore permissionStore = new Business.Service.PermissionStore(model);
            permissionStore.Permissions.Add(new Business.Service.Permission(1,1,1));
            Assert.IsTrue(permissionStore.HasPermission(1,1,1));
        }
    }
}
