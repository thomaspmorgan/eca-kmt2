using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation;

namespace ECA.Business.Test.Validation
{
    [TestClass]
    public class SuccessfulValidationResultTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new SuccessfulValidationResult();
            Assert.IsTrue(instance.IsValid);
            Assert.AreEqual(0, instance.Errors.Count());
        }
    }
}
