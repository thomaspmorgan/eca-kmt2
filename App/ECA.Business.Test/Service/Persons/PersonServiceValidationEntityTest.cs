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
            var cityOfBirth = new Location();
            var isPlaceOfBirthUnknown = false;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();
            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isPlaceOfBirthUnknown);

            Assert.IsTrue(Object.ReferenceEquals(person, entity.Person));
            Assert.AreEqual(genderId, entity.GenderId);
            Assert.IsTrue(Object.ReferenceEquals(countriesOfCitizenship, entity.CountriesOfCitizenship));
        }
    }
}
