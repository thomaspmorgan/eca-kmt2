using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Models.Programs;

namespace ECA.Business.Test.Models.Programs
{
    [TestClass]
    public class EcaProgramTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var model = new EcaProgram();
            Assert.IsNotNull(model.ContactIds);
            Assert.IsNotNull(model.CountryIds);
            Assert.IsNotNull(model.CountryIsos);
            Assert.IsNotNull(model.GoalIds);
            Assert.IsNotNull(model.ThemeIds);
        }
    }
}
