using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using System.Collections.Generic;
using ECA.Business.Service.Persons;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class PersonServiceValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var person = new Person();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location
            {
                LocationId = 1
            };
            var isPlaceOfBirthUnknown = false;
            var isDateOfBirthEstimated = false;
            var isDateOfBirthUnknown = false;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();
            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                dateOfBirth,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown);

            Assert.IsTrue(Object.ReferenceEquals(person, entity.Person));
            Assert.AreEqual(genderId, entity.GenderId);
            Assert.IsTrue(Object.ReferenceEquals(countriesOfCitizenship, entity.CountriesOfCitizenship));
            Assert.AreEqual(cityOfBirth.LocationId, entity.PlaceOfBirthId);
            Assert.AreEqual(dateOfBirth, entity.DateOfBirth);
        }

        [TestMethod]
        public void TestConstructor_CheckDateOfBirthEstimated()
        {
            var person = new Person();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location
            {
                LocationId = 1
            };
            var isPlaceOfBirthUnknown = false;
            var isDateOfBirthEstimated = true;
            var isDateOfBirthUnknown = false;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();
            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                dateOfBirth,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown);

            Assert.AreEqual(isDateOfBirthEstimated, entity.IsDateOfBirthEstimated);
            Assert.AreEqual(isDateOfBirthUnknown, entity.IsDateOfBirthUnknown);
            Assert.AreEqual(isPlaceOfBirthUnknown, entity.IsPlaceOfBirthUnknown);
        }

        [TestMethod]
        public void TestConstructor_CheckPlaceOfBirthUnknown()
        {
            var person = new Person();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location
            {
                LocationId = 1
            };
            var isPlaceOfBirthUnknown = true;
            var isDateOfBirthEstimated = false;
            var isDateOfBirthUnknown = false;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();
            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                dateOfBirth,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown);

            Assert.AreEqual(isDateOfBirthEstimated, entity.IsDateOfBirthEstimated);
            Assert.AreEqual(isDateOfBirthUnknown, entity.IsDateOfBirthUnknown);
            Assert.AreEqual(isPlaceOfBirthUnknown, entity.IsPlaceOfBirthUnknown);
        }

        [TestMethod]
        public void TestConstructor_CheckDateOfBirthUnknown()
        {
            var person = new Person();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location
            {
                LocationId = 1
            };
            var isPlaceOfBirthUnknown = false;
            var isDateOfBirthEstimated = false;
            var isDateOfBirthUnknown = true;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();
            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                dateOfBirth,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown);

            Assert.AreEqual(isDateOfBirthEstimated, entity.IsDateOfBirthEstimated);
            Assert.AreEqual(isDateOfBirthUnknown, entity.IsDateOfBirthUnknown);
            Assert.AreEqual(isPlaceOfBirthUnknown, entity.IsPlaceOfBirthUnknown);
        }
    }
}
