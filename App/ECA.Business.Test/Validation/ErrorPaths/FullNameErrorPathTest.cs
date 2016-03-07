﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using ECA.Business.Validation.SEVIS;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class FullNameErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new FullNameErrorPath();
            var lookup = SevisErrorType.FullName;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}