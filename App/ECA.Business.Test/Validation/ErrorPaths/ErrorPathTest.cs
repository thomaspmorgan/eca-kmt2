using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Core.Generation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class ErrorPathTest
    {
        [TestMethod]
        public void TestSetByStaticLookup()
        {
            var id = 1;
            var value = "value";
            var lookup = new StaticLookup(value, id);
            var instance = new ErrorPath();
            instance.SetByStaticLookup(lookup);
            Assert.AreEqual(id, instance.SevisErrorTypeId);
            Assert.AreEqual(value, instance.SevisErrorTypeName);
        }
    }
}
