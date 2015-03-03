using ECA.Business.Service;
using ECA.Business.Service.Admin;
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
            var userId = 1;
            var user = new User(userId);
            var programId = 2;
            var now = DateTimeOffset.UtcNow;

            var model = new DraftProject(user, name, description, programId);
            Assert.AreEqual(name, model.Name);
            Assert.AreEqual(description, model.Description);
            Assert.AreEqual(programId, model.ProgramId);

            Assert.IsNotNull(model.History);
            Assert.AreEqual(userId, model.History.CreatedBy.Id);
            model.History.CreatedAndRevisedOn.Should().BeCloseTo(now, DbContextHelper.DATE_PRECISION);
        }
    }
}
