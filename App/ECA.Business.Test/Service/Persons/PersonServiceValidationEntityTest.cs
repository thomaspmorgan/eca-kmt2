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
            var participant = new Participant();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();
            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                participant,
                genderId,
                dateOfBirth,
                cityOfBirth,
                countriesOfCitizenship);

            Assert.IsTrue(Object.ReferenceEquals(person, entity.Person));
            Assert.IsTrue(Object.ReferenceEquals(participant, entity.Participant));
            Assert.AreEqual(genderId, entity.GenderId);
            Assert.AreEqual(dateOfBirth, entity.DateOfBirth);
            Assert.IsTrue(Object.ReferenceEquals(cityOfBirth, entity.CityOfBirth));
            Assert.IsTrue(Object.ReferenceEquals(countriesOfCitizenship, entity.CountriesOfCitizenship));
        }
    }
}
