using ECA.Business.Validation;
using ECA.Business.Validation.Sevis.ErrorPaths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

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
