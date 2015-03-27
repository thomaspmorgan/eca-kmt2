using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Service.Admin;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class ProjectServiceCreateValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var name = "name";
            var description = "desc";
            var program = new Program();
            var entity = new ProjectServiceCreateValidationEntity(
                name: name,
                description: description,
                program: program
                );
            Assert.AreEqual(name, entity.Name);
            Assert.AreEqual(description, entity.Description);
            Assert.IsTrue(Object.ReferenceEquals(program, entity.Program));
        }
    }
}
