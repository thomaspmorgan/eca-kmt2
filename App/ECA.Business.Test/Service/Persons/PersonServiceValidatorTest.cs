using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using ECA.Data;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class PersonServiceValidatorTest
    {
        [TestMethod]
        public void TestValid()
        {
            var validator = new PersonServiceValidator();
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
                genderId,
                dateOfBirth,
                cityOfBirth,
                countriesOfCitizenship);

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestNullPerson()
        {
            var validator = new PersonServiceValidator();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();

            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                null,
                genderId,
                dateOfBirth,
                cityOfBirth,
                countriesOfCitizenship);

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.PERSON_NOT_FOUND, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestGenderIdNotFound()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var genderId = 0;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();

            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                genderId,
                dateOfBirth,
                cityOfBirth,
                countriesOfCitizenship);

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.GENDER_NOT_FOUND, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestDateOfBirthGreaterThanToday()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var genderId = 1;
            var dateOfBirth = DateTime.Now.AddDays(1);
            var cityOfBirth = new Location();
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();

            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                genderId,
                dateOfBirth,
                cityOfBirth,
                countriesOfCitizenship);

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.DATE_OF_BIRTH_GREATER_THAN_TODAY, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestDateRequired()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var genderId = 1;
            var dateOfBirth = DateTime.MinValue;
            var cityOfBirth = new Location();
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();

            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                genderId,
                dateOfBirth,
                cityOfBirth,
                countriesOfCitizenship);

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.DATE_OF_BIRTH_REQUIRED, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestNullCityOfBirth()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var participant = new Participant();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();

            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                genderId,
                dateOfBirth,
                null,
                countriesOfCitizenship);

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.CITY_OF_BIRTH_NOT_FOUND, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestNullCountriesOfCitizenship()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var participant = new Participant();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();

            var entity = new PersonServiceValidationEntity(
                person,
                genderId,
                dateOfBirth,
                cityOfBirth,
                null);

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.COUNTRIES_OF_CITIZENSHIP_NOT_FOUND, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestCountriesOfCitizenshipRequired()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var participant = new Participant();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
            var countriesOfCitizenship = new List<Location>();

            var entity = new PersonServiceValidationEntity(
                person,
                genderId,
                dateOfBirth,
                cityOfBirth,
                countriesOfCitizenship);

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.COUNTRIES_OF_CITIZENSHIP_REQUIRED, validationResult.ErrorMessage);
        }
    }
}
