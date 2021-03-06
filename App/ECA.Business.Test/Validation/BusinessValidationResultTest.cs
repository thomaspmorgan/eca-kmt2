﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation;

namespace ECA.Business.Test.Validation
{
    public class BusinessValidationResultTestClass
    {
        public int Id { get; set; }
    }

    [TestClass]
    public class BusinessValidationResultTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var message = "message";
            var property = "property";
            var result = new BusinessValidationResult(property, message);
            Assert.AreEqual(message, result.ErrorMessage);
            Assert.AreEqual(property, result.Property);
        }

        [TestMethod]
        public void TestTypedConstructor()
        {
            var message = "message";
            var result = new BusinessValidationResult<BusinessValidationResultTestClass>(x => x.Id, message);
            Assert.AreEqual(message, result.ErrorMessage);
            Assert.AreEqual("Id", result.Property);
        }

        [TestMethod]
        public void TestToString()
        {
            var message = "message";
            var property = "property";
            var result = new BusinessValidationResult(property, message);
            Assert.IsNotNull(result.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestTypedConstructor_ParameterIsAMethod()
        {
            var message = "message";
            var result = new BusinessValidationResult<BusinessValidationResultTestClass>(x => x.ToString(), message);            
        }
    }
}
