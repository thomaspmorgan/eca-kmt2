﻿using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class EmailErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new EmailErrorPath();
            var lookup = SevisErrorType.Email;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
