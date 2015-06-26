using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Service;
using CAM.Data;

namespace CAM.Business.Test.Service
{
    [TestClass]
    public class ForeignResourceCacheTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var foreignResourceId = 1;
            var resourceId = 2;
            var resourceTypeId = ResourceType.Project.Id;
            var parentForeignResourceId = 4;
            var parentResourceId = 5;
            var parentResourceTypeId = ResourceType.Program.Id;
            var cache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);

            Assert.AreEqual(foreignResourceId, cache.ForeignResourceId);
            Assert.AreEqual(resourceId, cache.ResourceId);
            Assert.AreEqual(resourceTypeId, cache.ResourceTypeId);
            Assert.AreEqual(parentForeignResourceId, cache.ParentForeignResourceId);
            Assert.AreEqual(parentResourceId, cache.ParentResourceId);
            Assert.AreEqual(parentResourceTypeId, cache.ParentResourceTypeId);
        }

        [TestMethod]
        public void TestToString()
        {
            var foreignResourceId = 1;
            var resourceId = 2;
            var resourceTypeId = ResourceType.Project.Id;
            var parentForeignResourceId = 4;
            var parentResourceId = 5;
            var parentResourceTypeId = ResourceType.Program.Id;
            var cache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
            Assert.IsNotNull(cache.ToString());
        }

        [TestMethod]
        public void TestToString_UnknownParentResourceType()
        {
            var foreignResourceId = 1;
            var resourceId = 2;
            var resourceTypeId = ResourceType.Project.Id;
            var parentForeignResourceId = 4;
            var parentResourceId = 5;
            var parentResourceTypeId = -1;
            var cache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
            Assert.IsNotNull(cache.ToString());
        }

        [TestMethod]
        public void TestToString_UnknownResourceType()
        {
            var foreignResourceId = 1;
            var resourceId = 2;
            var resourceTypeId = -1;
            var parentForeignResourceId = 4;
            var parentResourceId = 5;
            var parentResourceTypeId = ResourceType.Program.Id;
            var cache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
            Assert.IsNotNull(cache.ToString());
        }
    }
}
