using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Data;
using ECA.Core.Data;
using CAM.Business.Service;

namespace CAM.Business.Test.Service
{
    [TestClass]
    public class PermissableTypeExtensionsTest
    {
        [TestMethod]
        public void TestGetResourceTypeId()
        {
            Assert.AreEqual(ResourceType.Application.Id, PermissableType.Application.GetResourceTypeId());
            Assert.AreEqual(ResourceType.Office.Id, PermissableType.Office.GetResourceTypeId());
            Assert.AreEqual(ResourceType.Program.Id, PermissableType.Program.GetResourceTypeId());
            Assert.AreEqual(ResourceType.Project.Id, PermissableType.Project.GetResourceTypeId());
        }
    }
}
