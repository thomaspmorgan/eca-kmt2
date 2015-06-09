using ECA.Business.Service.Projects;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ECA.Business.Test.Service.Projects
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
