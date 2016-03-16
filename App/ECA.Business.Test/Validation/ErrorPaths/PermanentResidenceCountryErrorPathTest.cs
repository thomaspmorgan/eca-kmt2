using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class PermanentResidenceCountryErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new PermanentResidenceCountryErrorPath();
            var lookup = SevisErrorType.PermanentResidenceCountry;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
