using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Person;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Person
{
    [TestClass]
    public class PiiBindingModelTest
    {
        [TestMethod]
        public void TestToUpdatePii()
        {
            var model = new PiiBindingModel();
            model.Alias = "alias";
            model.CityOfBirthId = 1;
            model.CountriesOfCitizenship = new System.Collections.Generic.List<int>();
            model.DateOfBirth = DateTime.Now;
            model.Ethnicity = "ethnicity";
            model.FamilyName = "family";
            model.FirstName = "first";
            model.GenderId = Gender.Female.Id;
            model.GivenName = "given";
            model.LastName = "last";
            model.MaritalStatusId = MaritalStatus.Divorced.Id;
            model.MedicalConditions = "medical";
            model.MiddleName = "middle";
            model.NamePrefix = "prefix";
            model.NameSuffix = "suffix";
            model.Patronym = "patronym";
            model.PersonId = 2;
            model.IsDateOfBirthEstimated = true;
            model.IsDateOfBirthUnknown = true;
            model.IsPlaceOfBirthUnknown = true;

            var user = new User(1);
            var instance = model.ToUpdatePii(user);
            Assert.AreEqual(model.Alias, instance.Alias);
            Assert.AreEqual(model.CityOfBirthId, instance.CityOfBirthId);
            Assert.IsTrue(Object.ReferenceEquals(model.CountriesOfCitizenship, instance.CountriesOfCitizenship));
            Assert.AreEqual(model.DateOfBirth, instance.DateOfBirth);
            Assert.AreEqual(model.Ethnicity, instance.Ethnicity);
            Assert.AreEqual(model.FamilyName, instance.FamilyName);
            Assert.AreEqual(model.FirstName, instance.FirstName);
            Assert.AreEqual(model.GenderId, instance.GenderId);
            Assert.AreEqual(model.GivenName, instance.GivenName);
            Assert.AreEqual(model.LastName, instance.LastName);
            Assert.AreEqual(model.MaritalStatusId, instance.MaritalStatusId);
            Assert.AreEqual(model.MedicalConditions, instance.MedicalConditions);
            Assert.AreEqual(model.MiddleName, instance.MiddleName);
            Assert.AreEqual(model.NamePrefix, instance.NamePrefix);
            Assert.AreEqual(model.NameSuffix, instance.NameSuffix);
            Assert.AreEqual(model.Patronym, instance.Patronym);
            Assert.AreEqual(model.PersonId, instance.PersonId);
            Assert.AreEqual(model.IsDateOfBirthEstimated, instance.IsDateOfBirthEstimated);
            Assert.AreEqual(model.IsDateOfBirthUnknown, instance.IsDateOfBirthUnknown);
            Assert.AreEqual(model.IsPlaceOfBirthUnknown, instance.IsPlaceOfBirthUnknown);
        }

        [TestMethod]
        public void TestToUpdatePii_CheckDateOfBirthEstimated()
        {
            var model = new PiiBindingModel();
            model.IsDateOfBirthEstimated = true;
            model.IsDateOfBirthUnknown = false;
            model.IsPlaceOfBirthUnknown = false;

            var user = new User(1);
            var instance = model.ToUpdatePii(user);
            Assert.AreEqual(model.IsDateOfBirthEstimated, instance.IsDateOfBirthEstimated);
            Assert.AreEqual(model.IsDateOfBirthUnknown, instance.IsDateOfBirthUnknown);
            Assert.AreEqual(model.IsPlaceOfBirthUnknown, instance.IsPlaceOfBirthUnknown);
        }

        [TestMethod]
        public void TestToUpdatePii_CheckDateOfBirthUnknown()
        {
            var model = new PiiBindingModel();
            model.IsDateOfBirthEstimated = false;
            model.IsDateOfBirthUnknown = true;
            model.IsPlaceOfBirthUnknown = false;

            var user = new User(1);
            var instance = model.ToUpdatePii(user);
            Assert.AreEqual(model.IsDateOfBirthEstimated, instance.IsDateOfBirthEstimated);
            Assert.AreEqual(model.IsDateOfBirthUnknown, instance.IsDateOfBirthUnknown);
            Assert.AreEqual(model.IsPlaceOfBirthUnknown, instance.IsPlaceOfBirthUnknown);
        }

        [TestMethod]
        public void TestToUpdatePii_CheckPlaceOfBirthUnknown()
        {
            var model = new PiiBindingModel();
            model.IsDateOfBirthEstimated = false;
            model.IsDateOfBirthUnknown = true;
            model.IsPlaceOfBirthUnknown = false;

            var user = new User(1);
            var instance = model.ToUpdatePii(user);
            Assert.AreEqual(model.IsDateOfBirthEstimated, instance.IsDateOfBirthEstimated);
            Assert.AreEqual(model.IsDateOfBirthUnknown, instance.IsDateOfBirthUnknown);
            Assert.AreEqual(model.IsPlaceOfBirthUnknown, instance.IsPlaceOfBirthUnknown);
        }
    }
}
