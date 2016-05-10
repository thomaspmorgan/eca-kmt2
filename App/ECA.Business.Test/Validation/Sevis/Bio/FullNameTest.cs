using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Sevis.Model;
using Newtonsoft.Json;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class FullNameTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);
            var json = JsonConvert.SerializeObject(fullName);
            var jsonObject = JsonConvert.DeserializeObject<FullName>(json);

            Assert.AreEqual(fullName.FirstName, jsonObject.FirstName);
            Assert.AreEqual(fullName.LastName, jsonObject.LastName);
            Assert.AreEqual(fullName.PassportName, jsonObject.PassportName);
            Assert.AreEqual(fullName.PreferredName, jsonObject.PreferredName);
            Assert.AreEqual(fullName.Suffix, jsonObject.Suffix);
        }

        [TestMethod]
        public void TestJsonSerialization()
        {
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);
            var json = JsonConvert.SerializeObject(fullName);
            var jsonObject = JsonConvert.DeserializeObject<FullName>(json);

            Assert.AreEqual(fullName.FirstName, jsonObject.FirstName);
            Assert.AreEqual(fullName.LastName, jsonObject.LastName);
            Assert.AreEqual(fullName.PassportName, jsonObject.PassportName);
            Assert.AreEqual(fullName.PreferredName, jsonObject.PreferredName);
            Assert.AreEqual(fullName.Suffix, jsonObject.Suffix);
        }

        [TestMethod]
        public void TestGetNameType_HasSuffixWithPeriod()
        {
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);
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
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);
            var instance = fullName.GetNameType();
            Assert.AreEqual(NameSuffixCodeType.Jr, instance.Suffix);
            Assert.IsTrue(instance.SuffixSpecified);
        }

        [TestMethod]
        public void TestGetNameType_DoesNotHaveSuffix()
        {
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var fullName = new FullName(firstName, lastName, passport, preferred, null);
            var instance = fullName.GetNameType();
            Assert.IsFalse(instance.SuffixSpecified);
        }

        [TestMethod]
        public void TestGetNameNullableType()
        {
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);
            var instance = fullName.GetNameNullableType();
            Assert.AreEqual(fullName.FirstName, instance.FirstName);
            Assert.AreEqual(fullName.LastName, instance.LastName);
            Assert.AreEqual(fullName.PassportName, instance.PassportName);
            Assert.AreEqual(fullName.PreferredName, instance.PreferredName);
            Assert.AreEqual(fullName.Suffix, instance.Suffix);
        }

        #region GetchangeDetail
        [TestMethod]
        public void TestGetChangeDetail_SameInstance()
        {
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var detail = fullName.GetChangeDetail(fullName);
            Assert.IsFalse(detail.HasChanges());
            Assert.IsFalse(detail.FirstNameChanged);
            Assert.IsFalse(detail.LastNameChanged);
            Assert.IsFalse(detail.PassportNameChanged);
            Assert.IsFalse(detail.PreferredNameChanged);
            Assert.IsFalse(detail.SuffixChanged);
        }

        [TestMethod]
        public void TestGetChangeDetail_FirstNameChanged()
        {
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var otherFirstName = "other first";
            var otherLastName = "last";
            var otherPassport = "passport";
            var otherPreferred = "preferred";
            var otherSuffix = "Jr.";
            var otherFullName = new FullName(otherFirstName, otherLastName, otherPassport, otherPreferred, otherSuffix);

            var detail = fullName.GetChangeDetail(otherFullName);
            Assert.IsTrue(detail.HasChanges());
            Assert.IsTrue(detail.FirstNameChanged);
            Assert.IsFalse(detail.LastNameChanged);
            Assert.IsFalse(detail.PassportNameChanged);
            Assert.IsFalse(detail.PreferredNameChanged);
            Assert.IsFalse(detail.SuffixChanged);
        }

        [TestMethod]
        public void TestGetChangeDetail_LastNameChanged()
        {
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var otherFirstName = "first";
            var otherLastName = "other last";
            var otherPassport = "passport";
            var otherPreferred = "preferred";
            var otherSuffix = "Jr.";
            var otherFullName = new FullName(otherFirstName, otherLastName, otherPassport, otherPreferred, otherSuffix);

            var detail = fullName.GetChangeDetail(otherFullName);
            Assert.IsTrue(detail.HasChanges());
            Assert.IsFalse(detail.FirstNameChanged);
            Assert.IsTrue(detail.LastNameChanged);
            Assert.IsFalse(detail.PassportNameChanged);
            Assert.IsFalse(detail.PreferredNameChanged);
            Assert.IsFalse(detail.SuffixChanged);
        }

        [TestMethod]
        public void TestGetChangeDetail_PassportNameChanged()
        {
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var otherFirstName = "first";
            var otherLastName = "last";
            var otherPassport = "other passport";
            var otherPreferred = "preferred";
            var otherSuffix = "Jr.";
            var otherFullName = new FullName(otherFirstName, otherLastName, otherPassport, otherPreferred, otherSuffix);

            var detail = fullName.GetChangeDetail(otherFullName);
            Assert.IsTrue(detail.HasChanges());
            Assert.IsFalse(detail.FirstNameChanged);
            Assert.IsFalse(detail.LastNameChanged);
            Assert.IsTrue(detail.PassportNameChanged);
            Assert.IsFalse(detail.PreferredNameChanged);
            Assert.IsFalse(detail.SuffixChanged);
        }

        [TestMethod]
        public void TestGetChangeDetail_PreferredNameChanged()
        {
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var otherFirstName = "first";
            var otherLastName = "last";
            var otherPassport = "passport";
            var otherPreferred = "other preferred";
            var otherSuffix = "Jr.";
            var otherFullName = new FullName(otherFirstName, otherLastName, otherPassport, otherPreferred, otherSuffix);

            var detail = fullName.GetChangeDetail(otherFullName);
            Assert.IsTrue(detail.HasChanges());
            Assert.IsFalse(detail.FirstNameChanged);
            Assert.IsFalse(detail.LastNameChanged);
            Assert.IsFalse(detail.PassportNameChanged);
            Assert.IsTrue(detail.PreferredNameChanged);
            Assert.IsFalse(detail.SuffixChanged);
        }

        [TestMethod]
        public void TestGetChangeDetail_SuffixNameChanged()
        {
            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var otherFirstName = "first";
            var otherLastName = "last";
            var otherPassport = "passport";
            var otherPreferred = "preferred";
            var otherSuffix = "other Jr.";
            var otherFullName = new FullName(otherFirstName, otherLastName, otherPassport, otherPreferred, otherSuffix);

            var detail = fullName.GetChangeDetail(otherFullName);
            Assert.IsTrue(detail.HasChanges());
            Assert.IsFalse(detail.FirstNameChanged);
            Assert.IsFalse(detail.LastNameChanged);
            Assert.IsFalse(detail.PassportNameChanged);
            Assert.IsFalse(detail.PreferredNameChanged);
            Assert.IsTrue(detail.SuffixChanged);
        }
        #endregion
    }
}
