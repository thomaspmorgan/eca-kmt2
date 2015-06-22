using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business;
using CAM.Data;
using System.Collections.Generic;
using CAM.Business.Service;
using Moq;

namespace CAM.Business.Test.Service
{
    [TestClass]
    public class PermissionStoreTest
    {
        [TestMethod]
        public void TestSinglePermission()
        {
            var model = new TestInMemoryCamModel();

            var resourceService = new ResourceService(model);
            var permissionModelService = new PermissionModelService(model);
            Business.Service.PermissionStore permissionStore = new Business.Service.PermissionStore(model, permissionModelService, resourceService);
            permissionStore.Permissions.Add(new Business.Service.SimplePermission(1,1,1));
            Assert.IsTrue(permissionStore.HasPermission(1,1,1));
        }
    }
}
