using ECA.Business.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Test.Models
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
            var programId = 2;
            var now = DateTimeOffset.UtcNow;

            var model = new DraftProject(name, description, programId, userId);
            Assert.AreEqual(name, model.Name);
            Assert.AreEqual(description, model.Description);
            Assert.AreEqual(programId, model.ProgramId);

            Assert.IsNotNull(model.History);
            Assert.AreEqual(userId, model.History.CreatorUserId);
            model.History.CreatedAndRevisedOn.Should().BeCloseTo(now, DbContextHelper.DATE_PRECISION);
        }
    }
}
