using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class CountryOfBirthErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new CountryOfBirthErrorPath();
            var lookup = SevisErrorType.CountryOfBirth;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
