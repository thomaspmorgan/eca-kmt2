using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Service;

namespace CAM.Business.Test.Service
{
    [TestClass]
    public class PermissionModelServiceTest
    {
        private PermissionModelService service;
        private TestInMemoryCamModel context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestInMemoryCamModel();
            service = new PermissionModelService(context);
        }

        [TestMethod]
        public void TestGetPermissionIdByName()
        {
            var permission = CAM.Data.Permission.EditOffice;
            var testPermission = service.GetPermissionIdByName(permission.Value);
            Assert.AreEqual(permission.Id, testPermission);
        }

        [TestMethod]
        public void TestGetPermissionNameById()
        {
            var permission = CAM.Data.Permission.EditOffice;
            var testPermission = service.GetPermissionNameById(permission.Id);
            Assert.AreEqual(permission.Value, testPermission);
        }

    }
}
