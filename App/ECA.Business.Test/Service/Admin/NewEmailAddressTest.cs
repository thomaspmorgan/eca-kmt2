using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Service.Admin;
using ECA.Business.Service;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.Admin
{
    public class TestEmailAddressableClass : NewEmailAddress<Person>, IEmailAddressable
    {
        public TestEmailAddressableClass(User user, int emailAddressTypeId, string address, bool isPrimary)
            : base(user, emailAddressTypeId, address, isPrimary)
        {
            this.EmailAddresses = new HashSet<EmailAddress>();
        }

        public ICollection<EmailAddress> EmailAddresses { get; set; }

        public override IQueryable<EmailAddress> CreateGetEmailAddressesQuery(EcaContext context)
        {
            throw new NotImplementedException();
        }

        public override int GetEmailAddressableEntityId()
        {
            return 1;
        }
    }

    [TestClass]
    public class NewEmailAddressTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var emailTypeId = EmailAddressType.Home.Id;
            var email = "someone@isp.com";
            var isPrimary = true;
            var user = new User(1);
            var model = new NewEmailAddress(user, emailTypeId, email, isPrimary);
            Assert.AreEqual(emailTypeId, model.EmailAddressTypeId);
            Assert.AreEqual(email, model.Address);
            Assert.AreEqual(isPrimary, model.IsPrimary);
            Assert.AreEqual(user.Id, model.Audit.User.Id);
            Assert.IsInstanceOfType(model.Audit, typeof(Create));
        }

        [TestMethod]
        public void TestConstructor_OneGenericArgument()
        {
            var emailTypeId = EmailAddressType.Home.Id;
            var email = "someone@isp.com";
            var isPrimary = true;
            var user = new User(1);

            var model = new TestEmailAddressableClass(user, emailTypeId, email, isPrimary);
            Assert.AreEqual(emailTypeId, model.EmailAddressTypeId);
            Assert.AreEqual(email, model.Address);
            Assert.AreEqual(isPrimary, model.IsPrimary);
            Assert.AreEqual(user.Id, model.Audit.User.Id);
            Assert.IsInstanceOfType(model.Audit, typeof(Create));
        }

        [TestMethod]
        public void TestAddEmailAddress()
        {
            var emailTypeId = EmailAddressType.Home.Id;
            var email = "someone@isp.com";
            var isPrimary = true;
            var user = new User(1);

            var model = new TestEmailAddressableClass(user, emailTypeId, email, isPrimary);
            var person = new Person();
            model.AddEmailAddress(person);
            Assert.AreEqual(1, person.EmailAddresses.Count);
            var firstEmailAddress = person.EmailAddresses.First();
            Assert.AreEqual(emailTypeId, firstEmailAddress.EmailAddressTypeId);
            Assert.AreEqual(email, firstEmailAddress.Address);
            Assert.AreEqual(isPrimary, firstEmailAddress.IsPrimary);

            Assert.AreEqual(user.Id, firstEmailAddress.History.CreatedBy);
            Assert.AreEqual(user.Id, firstEmailAddress.History.RevisedBy);
            DateTimeOffset.Now.Should().BeCloseTo(firstEmailAddress.History.CreatedOn, 20000);
            DateTimeOffset.Now.Should().BeCloseTo(firstEmailAddress.History.RevisedOn, 20000);

        }
    }
}
