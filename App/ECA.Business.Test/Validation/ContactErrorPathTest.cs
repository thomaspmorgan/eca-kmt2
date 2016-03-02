using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.SEVIS;

namespace ECA.Business.Test.Validation
{
    [TestClass]
    public class ContactErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new ContactErrorPath();
            Assert.AreEqual(ElementCategory.Person.ToString(), instance.Category);
            Assert.AreEqual(ElementCategorySub.PersonalInfo.ToString(), instance.CategorySub);
            Assert.AreEqual(ElementCategorySection.Contact.ToString(), instance.Section);
            Assert.AreEqual(ElementCategorySectionTab.PersonalInfo.ToString(), instance.Tab);
        }
    }
}
