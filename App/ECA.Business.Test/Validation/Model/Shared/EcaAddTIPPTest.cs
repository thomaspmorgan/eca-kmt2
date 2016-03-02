using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Test.Validation.Model.Shared
{
    [TestClass]
    public class EcaAddTIPPTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new EcaAddTIPP();
            Assert.IsFalse(instance.print7002);
            Assert.IsNull(instance.ParticipantInfo);
            Assert.IsNull(instance.TippExemptProgram);
            Assert.IsNull(instance.TippSite);
        }
    }
}
