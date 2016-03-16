using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Models.Persons;

namespace ECA.Business.Test.Queries.Models.Persons.ExchangeVisitor
{
    [TestClass]
    public class FullNameDTOTest
    {
        [TestMethod]
        public void TestGetFullName()
        {
            var model = new FullNameDTO
            {
                FirstName = "first",
                LastName = "last",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "suffix"
            };
            var instance = model.GetFullName();
            Assert.AreEqual(model.FirstName, instance.FirstName);
            Assert.AreEqual(model.LastName, instance.LastName);
            Assert.AreEqual(model.PassportName, instance.PassportName);
            Assert.AreEqual(model.PreferredName, instance.PreferredName);
            Assert.AreEqual(model.Suffix, instance.Suffix);
        }
    }
}
