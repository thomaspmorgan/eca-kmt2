using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class PositionCodeErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new PositionCodeErrorPath();
            var lookup = SevisErrorType.PositionCode;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
