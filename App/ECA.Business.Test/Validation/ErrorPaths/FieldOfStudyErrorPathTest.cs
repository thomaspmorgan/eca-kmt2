using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.SEVIS;
using ECA.Business.Validation.SEVIS.ErrorPaths;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class FieldOfStudyErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new FieldOfStudyErrorPath();
            var lookup = SevisErrorType.FieldOfStudy;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
