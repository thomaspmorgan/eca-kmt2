using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Sevis.Model;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class FullNameTest
    {
        [TestMethod]
        public void TestGetNameType_HasSuffixWithPeriod()
        {
            var fullName = new FullName
            {
                FirstName = "first",
                LastName = "Last",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "Jr."
            };
            var instance = fullName.GetNameType();
            Assert.AreEqual(fullName.FirstName, instance.FirstName);
            Assert.AreEqual(fullName.LastName, instance.LastName);
            Assert.AreEqual(fullName.PassportName, instance.PassportName);
            Assert.AreEqual(fullName.PreferredName, instance.PreferredName);
            Assert.AreEqual(NameSuffixCodeType.Jr, instance.Suffix);
            Assert.IsTrue(instance.SuffixSpecified);
        }

        [TestMethod]
        public void TestGetNameType_HasSuffixWithoutPeriod()
        {
            var fullName = new FullName
            {
                Suffix = "Jr"
            };
            var instance = fullName.GetNameType();
            Assert.AreEqual(NameSuffixCodeType.Jr, instance.Suffix);
            Assert.IsTrue(instance.SuffixSpecified);
        }

        [TestMethod]
        public void TestGetNameType_DoesNotHaveSuffix()
        {
            var fullName = new FullName
            {
            };
            var instance = fullName.GetNameType();
            Assert.IsFalse(instance.SuffixSpecified);
        }

        [TestMethod]
        public void TestGetNameNullableType()
        {
            var fullName = new FullName
            {
                FirstName = "first",
                LastName = "Last",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "Jr."
            };
            var instance = fullName.GetNameNullableType();
            Assert.AreEqual(fullName.FirstName, instance.FirstName);
            Assert.AreEqual(fullName.LastName, instance.LastName);
            Assert.AreEqual(fullName.PassportName, instance.PassportName);
            Assert.AreEqual(fullName.PreferredName, instance.PreferredName);
            Assert.AreEqual(fullName.Suffix, instance.Suffix);
        }
    }
}
