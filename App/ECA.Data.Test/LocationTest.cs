using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECA.Data.Test
{
    [TestClass]
    public class LocationTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestLocationNameMaxLength()
        {
            var location = new Location
            {
                LocationName = new String('a', Location.NAME_MAX_LENGTH)
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(location, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(location, new ValidationContext(location), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            location.LocationName = new String('a', Location.NAME_MAX_LENGTH + 1);

            actual = Validator.TryValidateObject(location, new ValidationContext(location), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("LocationName", results.First().MemberNames.First());
        }
    }
}
