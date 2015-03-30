using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class DraftProjectTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var name = "name";
            var description = "description";
            var program = new Program
            {
                ProgramId = 1
            };
            var user = new User(1);
            var draftProject = new DraftProject(
                createdBy: user,
                name: name,
                description: description,
                programId: program.ProgramId
                );

            Assert.AreEqual(name, draftProject.Name);
            Assert.AreEqual(description, draftProject.Description);
            Assert.AreEqual(program.ProgramId, draftProject.ProgramId);
            Assert.AreEqual(ProjectStatus.Draft.Id, draftProject.StatusId);
            Assert.IsInstanceOfType(draftProject.Audit, typeof(Create));

            var create = draftProject.Audit as Create;
            Assert.AreEqual(user.Id, create.User.Id);
            DateTimeOffset.UtcNow.Should().BeCloseTo(create.Date, DbContextHelper.DATE_PRECISION);
        }
    }
}
