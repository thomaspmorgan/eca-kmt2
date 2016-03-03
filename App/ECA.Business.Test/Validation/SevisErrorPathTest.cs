using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.SEVIS;

namespace ECA.Business.Test.Validation
{
    [TestClass]
    public class SevisErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new SevisErrorPath();
            Assert.AreEqual(ElementCategory.Project.ToString(), instance.Category);
            Assert.AreEqual(ElementCategorySub.Participant.ToString(), instance.CategorySub);            
            Assert.AreEqual(ElementCategorySectionTab.Sevis.ToString(), instance.Tab);

            Assert.IsNull(instance.Section);
        }
    }
}
