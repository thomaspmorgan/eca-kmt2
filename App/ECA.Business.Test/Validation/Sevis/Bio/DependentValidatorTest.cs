using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class DependentValidatorTest
    {
        [TestMethod]
        public void TestRelationship_Null()
        {
            Func<TestDependent> createEntity = () =>
            {
                return null;
            };

            var instance = createEntity();
            var validator = new DependentValidator();
            var result = validator.Validate(instance);
            Assert.IsTrue(result.IsValid);

            result = validator.Validate(instance);
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(1, result.Errors.Count());

        }
    }
}
