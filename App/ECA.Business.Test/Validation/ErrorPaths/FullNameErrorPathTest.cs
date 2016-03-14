using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class FullNameErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new FullNameErrorPath();
            var lookup = SevisErrorType.FullName;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
