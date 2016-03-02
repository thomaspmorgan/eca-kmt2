using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.SEVIS;

namespace ECA.Business.Test.Validation
{
    [TestClass]
    public class PiiErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new PiiErrorPath();
            Assert.AreEqual(ElementCategory.Person.ToString(), instance.Category);
            Assert.AreEqual(ElementCategorySub.PersonalInfo.ToString(), instance.CategorySub);
            Assert.AreEqual(ElementCategorySection.PII.ToString(), instance.Section);
            Assert.AreEqual(ElementCategorySectionTab.PersonalInfo.ToString(), instance.Tab);
        }
    }
}
