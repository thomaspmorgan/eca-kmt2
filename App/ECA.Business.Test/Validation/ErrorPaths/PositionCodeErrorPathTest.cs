using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.SEVIS;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class PositionCodeErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new PositionCodeErrorPath();
            var lookup = SevisErrorType.PositionCode;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
