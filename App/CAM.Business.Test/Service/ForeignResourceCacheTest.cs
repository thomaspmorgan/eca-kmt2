using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Service;

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
            var resourceTypeId = 3;
            var parentForeignResourceId = 4;
            var parentResourceId = 5;
            var parentResourceTypeId = 6;
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
            var resourceTypeId = 3;
            var parentForeignResourceId = 4;
            var parentResourceId = 5;
            var parentResourceTypeId = 6;
            var cache = new ForeignResourceCache(foreignResourceId, resourceId, resourceTypeId, parentForeignResourceId, parentResourceId, parentResourceTypeId);
            Assert.IsNotNull(cache.ToString());
        }
    }
}
