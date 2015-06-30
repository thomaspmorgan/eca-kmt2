using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CAM.Data.Test
{
    [TestClass]
    public class UserAccountTest
    {
        private TestInMemoryCamModel context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestInMemoryCamModel();
        }

        [TestMethod]
        public void TestUserAccount_Unique()
        {
            var existingUser = new UserAccount
            {
                AdGuid = Guid.NewGuid(),
                PrincipalId = 1
            };
            context.UserAccounts.Add(existingUser);
            var newUser = new UserAccount
            {
                AdGuid = existingUser.AdGuid,
                DisplayName = "display",
                FirstName = "first"
            };
            var items = new Dictionary<object, object> { { CamModel.VALIDATABLE_CONTEXT_KEY, context } };
            var vc = new ValidationContext(newUser, null, items);
            var results = new List<ValidationResult>();
            var actual = Validator.TryValidateObject(newUser, vc, results);
            Assert.IsFalse(actual);
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("AdGuid", results.First().MemberNames.First());

            var expectedErrorMessage = String.Format("The user with ad id [{0}] already exists.",
                        newUser.AdGuid);
            Assert.AreEqual(expectedErrorMessage, results.First().ErrorMessage);
        }
    }
}
