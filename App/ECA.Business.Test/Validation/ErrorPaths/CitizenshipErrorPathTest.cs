using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class CitizenshipErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new CitizenshipErrorPath();
            var lookup = SevisErrorType.Citizenship;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
