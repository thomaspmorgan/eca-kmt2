﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis;
using Microsoft.QualityTools.Testing.Fakes;
using ECA.Business.Validation.Sevis.Bio;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class FullNameChangeDetailTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            using (ShimsContext.Create())
            {
                var result = new KellermanSoftware.CompareNetObjects.Fakes.ShimComparisonResult
                {
                    AreEqualGet = () => false
                };

                var instance = new FullNameChangeDetail(result);
                Assert.IsTrue(instance.HasChanges());
            }
        }
    }
}