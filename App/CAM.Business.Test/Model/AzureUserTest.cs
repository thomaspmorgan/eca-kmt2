using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CAM.Business.Model;

namespace CAM.Business.Test.Model
{
    [TestClass]
    public class AzureUserTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var firstName = "first";
            var lastName = "lastName";
            var email = "email";
            var id = Guid.NewGuid();
            var displayName = "display";
            var newUser = new AzureUser(
                id: id,
                email: email,
                firstName: firstName,
                lastName: lastName,
                displayName: displayName
                );
            Assert.AreEqual(firstName, newUser.FirstName);
            Assert.AreEqual(lastName, newUser.LastName);
            Assert.AreEqual(email, newUser.Email);
            Assert.AreEqual(id, newUser.Id);
            Assert.AreEqual(displayName, newUser.DisplayName);
        }
    }
}
