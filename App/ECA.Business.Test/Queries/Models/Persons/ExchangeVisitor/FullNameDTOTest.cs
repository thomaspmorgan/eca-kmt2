using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Validation.Sevis.Bio;

namespace ECA.Business.Test.Queries.Models.Persons.ExchangeVisitor
{
    [TestClass]
    public class FullNameDTOTest
    {
        [TestMethod]
        public void TestGetFullName_HasMiddleName()
        {
            var model = new FullNameDTO
            {
                FirstName = "first",
                LastName = "last",
                MiddleName = "middle",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "suffix"
            };
            var instance = model.GetFullName();
            Assert.AreEqual(model.FirstName + " " + model.MiddleName, instance.FirstName);
            Assert.AreEqual(model.LastName, instance.LastName);
            Assert.AreEqual(model.PassportName, instance.PassportName);
            Assert.AreEqual(model.PreferredName, instance.PreferredName);
            Assert.AreEqual(model.Suffix, instance.Suffix);
        }

        [TestMethod]
        public void TestGetFullName_DoesNotHaveMiddleName()
        {
            var model = new FullNameDTO
            {
                FirstName = "first",
                LastName = "last",
                MiddleName = null,
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "suffix"
            };
            var instance = model.GetFullName();
            Assert.AreEqual(model.FirstName, instance.FirstName);
            Assert.AreEqual(model.LastName, instance.LastName);
            Assert.AreEqual(model.PassportName, instance.PassportName);
            Assert.AreEqual(model.PreferredName, instance.PreferredName);
            Assert.AreEqual(model.Suffix, instance.Suffix);
        }

        [TestMethod]
        public void TestGetFullName_AllNamesNull()
        {
            var model = new FullNameDTO
            {
                FirstName = null,
                LastName = null,
                MiddleName = null,
                PassportName = null,
                PreferredName = null,
                Suffix = null
            };
            var instance = model.GetFullName();
            Assert.IsNull(instance.FirstName);
            Assert.IsNull(instance.LastName);
            Assert.IsNull(instance.PassportName);
            Assert.IsNull(instance.PreferredName);
            Assert.IsNull(instance.Suffix);
        }

        [TestMethod]
        public void TestGetFullName_FirstNameExceedsMaxLength()
        {
            var model = new FullNameDTO
            {
                FirstName = new string('a', FullNameValidator.FIRST_NAME_MAX_LENGTH + 1)
            };
            var instance = model.GetFullName();
            Assert.AreEqual(new string('a', FullNameValidator.FIRST_NAME_MAX_LENGTH), instance.FirstName);
        }

        [TestMethod]
        public void TestGetFullName_PreferredNameExceedsMaxLength()
        {
            var model = new FullNameDTO
            {
                PreferredName = new string('a', FullNameValidator.PREFERRED_NAME_MAX_LENGTH + 1)
            };
            var instance = model.GetFullName();
            Assert.AreEqual(new string('a', FullNameValidator.PREFERRED_NAME_MAX_LENGTH), instance.PreferredName);
        }

        [TestMethod]
        public void TestGetFullName_LastNameExceedsMaxLength()
        {
            var model = new FullNameDTO
            {
                LastName = new string('a', FullNameValidator.LAST_NAME_MAX_LENGTH + 1)
            };
            var instance = model.GetFullName();
            Assert.AreEqual(new string('a', FullNameValidator.LAST_NAME_MAX_LENGTH), instance.LastName);
        }
    }
}
