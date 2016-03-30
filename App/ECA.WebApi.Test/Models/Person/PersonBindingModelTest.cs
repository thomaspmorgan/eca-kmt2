using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Person;
using System.Collections.Generic;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Person
{
    [TestClass]
    public class PersonBindingModelTest
    {
        [TestMethod]
        public void TestToNewPerson()
        {
            var model = new PersonBindingModel
            {
                CityOfBirth = 1,
                CountriesOfCitizenship = new List<int> { 1 },
                DateOfBirth = DateTime.Now,
                FirstName = "first",
                Gender = Gender.Male.Id,
                IsDateOfBirthEstimated = true,
                IsDateOfBirthUnknown = true,
                IsPlaceOfBirthUnknown = true,
                LastName = "last", 
                ProjectId = 5
            };
            var user = new User(1);
            var instance = model.ToNewPerson(user);
            Assert.AreEqual(model.CityOfBirth, instance.CityOfBirth);
            CollectionAssert.AreEqual(model.CountriesOfCitizenship, instance.CountriesOfCitizenship);
            Assert.AreEqual(model.DateOfBirth, instance.DateOfBirth);
            Assert.AreEqual(model.FirstName, instance.FirstName);
            Assert.AreEqual(model.LastName, instance.LastName);
            Assert.AreEqual(model.ParticipantTypeId, instance.ParticipantTypeId);
            Assert.AreEqual(model.ProjectId, instance.ProjectId);
        }

        [TestMethod]
        public void TestToNewPerson_CheckIsDateOfBirthEstimated()
        {
            var model = new PersonBindingModel
            {
                IsDateOfBirthEstimated = true,
                IsDateOfBirthUnknown = false,
                IsPlaceOfBirthUnknown = false
            };
            var user = new User(1);
            var instance = model.ToNewPerson(user);
            Assert.AreEqual(model.IsDateOfBirthUnknown, instance.IsDateOfBirthUnknown);
            Assert.AreEqual(model.IsPlaceOfBirthUnknown, instance.IsPlaceOfBirthUnknown);
            Assert.AreEqual(model.IsDateOfBirthEstimated, instance.IsDateOfBirthEstimated);
        }

        [TestMethod]
        public void TestToNewPerson_CheckIsDateOfBirthUnknown()
        {
            var model = new PersonBindingModel
            {
                IsDateOfBirthEstimated = false,
                IsDateOfBirthUnknown = true,
                IsPlaceOfBirthUnknown = false
            };
            var user = new User(1);
            var instance = model.ToNewPerson(user);
            Assert.AreEqual(model.IsDateOfBirthUnknown, instance.IsDateOfBirthUnknown);
            Assert.AreEqual(model.IsPlaceOfBirthUnknown, instance.IsPlaceOfBirthUnknown);
            Assert.AreEqual(model.IsDateOfBirthEstimated, instance.IsDateOfBirthEstimated);
        }

        [TestMethod]
        public void TestToNewPerson_CheckIsPlaceOfBirthUnknown()
        {
            var model = new PersonBindingModel
            {
                IsDateOfBirthEstimated = false,
                IsDateOfBirthUnknown = false,
                IsPlaceOfBirthUnknown = true
            };
            var user = new User(1);
            var instance = model.ToNewPerson(user);
            Assert.AreEqual(model.IsDateOfBirthUnknown, instance.IsDateOfBirthUnknown);
            Assert.AreEqual(model.IsPlaceOfBirthUnknown, instance.IsPlaceOfBirthUnknown);
            Assert.AreEqual(model.IsDateOfBirthEstimated, instance.IsDateOfBirthEstimated);
        }
    }
}
