using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.SEVIS.ErrorPaths;
using ECA.Business.Validation;

namespace ECA.Business.Test.Validation
{
    [TestClass]
    public class SimpleValidationFailureTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var errorMessage = "Error Message";
            var propertyName = "Property";
            var errorPath = new ErrorPath();
            var instance = new SimpleValidationFailure(errorPath, errorMessage, propertyName);
            Assert.IsTrue(Object.ReferenceEquals(errorMessage, instance.ErrorMessage));
            Assert.IsTrue(Object.ReferenceEquals(propertyName, instance.PropertyName));
            Assert.IsTrue(Object.ReferenceEquals(errorPath, instance.CustomState));
        }
    }
}
