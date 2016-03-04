﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using ECA.Business.Validation.SEVIS;

namespace ECA.Business.Test.Validation.ErrorPaths
{
    [TestClass]
    public class PermanentResidenceCountryErrorPathTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new PermanentResidenceCountryErrorPath();
            var lookup = SevisErrorType.PermanentResidenceCountry;
            Assert.AreEqual(lookup.Id, instance.SevisErrorTypeId);
            Assert.AreEqual(lookup.Value, instance.SevisErrorTypeName);
        }
    }
}
