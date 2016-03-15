using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class FundingErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new FundingErrorPath();
            var lookup = SevisErrorType.Funding;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
