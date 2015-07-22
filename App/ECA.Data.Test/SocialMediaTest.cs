using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECA.Data.Test
{
    [TestClass]
    public class SocialMediaTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestSocialMediaValueMaxLength()
        {
            var socialMedia = new SocialMedia
            {
                SocialMediaValue = new String('a', SocialMedia.VALUE_MAX_LENGTH)
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(socialMedia, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(socialMedia, new ValidationContext(socialMedia), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            socialMedia.SocialMediaValue = new String('a', SocialMedia.VALUE_MAX_LENGTH + 1);

            actual = Validator.TryValidateObject(socialMedia, new ValidationContext(socialMedia), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("SocialMediaValue", results.First().MemberNames.First());
        }

        [TestMethod]
        public void TestSocialMediaValueRequired()
        {
            var socialMedia = new SocialMedia
            {
                SocialMediaValue = new String('a', SocialMedia.VALUE_MAX_LENGTH)
            };
            var items = new Dictionary<object, object> { { EcaContext.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(socialMedia, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(socialMedia, new ValidationContext(socialMedia), results, true);

            Assert.IsTrue(actual);
            Assert.AreEqual(0, results.Count);
            socialMedia.SocialMediaValue = null;

            actual = Validator.TryValidateObject(socialMedia, new ValidationContext(socialMedia), results, true);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("SocialMediaValue", results.First().MemberNames.First());
        }
    }
}
