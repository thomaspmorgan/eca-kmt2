using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class PhoneNumberErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new PhoneNumberErrorPath();
            var lookup = SevisErrorType.PhoneNumber;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
