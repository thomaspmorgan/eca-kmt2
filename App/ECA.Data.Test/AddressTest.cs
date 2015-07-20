using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECA.Data.Test
{
    [TestClass]
    public class AddressTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestDisplayNameMaxLength()
        {
            var address = new Address
            {
                DisplayName = new String('a', Address.ADDRESS_DISPLAY_NAME_MAX_LENGTH)
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(address, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(address, new ValidationContext(address), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            address.DisplayName = new String('a', Address.ADDRESS_DISPLAY_NAME_MAX_LENGTH + 1);

            actual = Validator.TryValidateObject(address, new ValidationContext(address), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("DisplayName", results.First().MemberNames.First());
        }

        [TestMethod]
        public void TestDisplayNameRequired()
        {
            var address = new Address
            {
                DisplayName = new String('a', Address.ADDRESS_DISPLAY_NAME_MAX_LENGTH)
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(address, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(address, new ValidationContext(address), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            address.DisplayName = null;

            actual = Validator.TryValidateObject(address, new ValidationContext(address), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("DisplayName", results.First().MemberNames.First());
        }
    }
}
