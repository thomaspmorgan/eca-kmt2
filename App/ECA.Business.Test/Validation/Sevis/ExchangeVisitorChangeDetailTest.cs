using ECA.Business.Validation.Sevis;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.Sevis
{
    [TestClass]
    public class ExchangeVisitorChangeDetailTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            using (ShimsContext.Create())
            {
                var result = new KellermanSoftware.CompareNetObjects.Fakes.ShimComparisonResult
                {
                    AreEqualGet = () => false
                };

                var instance = new ExchangeVisitorChangeDetail(result);
                Assert.IsTrue(instance.HasChanges());
            }
        }
    }
}
