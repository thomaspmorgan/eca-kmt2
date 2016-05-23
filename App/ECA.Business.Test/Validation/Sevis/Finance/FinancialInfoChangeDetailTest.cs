using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.QualityTools.Testing.Fakes;
using ECA.Business.Validation.Sevis.Finance;

namespace ECA.Business.Test.Validation.Sevis.Finance
{
    [TestClass]
    public class FinancialInfoChangeDetailTest
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

                var instance = new FinancialInfoChangeDetail(result);
                Assert.IsTrue(instance.HasChanges());
            }
        }
    }
}
