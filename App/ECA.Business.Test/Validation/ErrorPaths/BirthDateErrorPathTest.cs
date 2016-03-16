using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class BirthDateErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new BirthDateErrorPath();
            var lookup = SevisErrorType.BirthDate;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
