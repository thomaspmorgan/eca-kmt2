using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class UpdateOrganizationValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var name = "name";
            var entity = new OrganizationValidationEntity(name, 1);
            Assert.AreEqual(name, entity.Name);
            Assert.AreEqual(1, entity.OrganizationTypeId);
        }
    }
}
