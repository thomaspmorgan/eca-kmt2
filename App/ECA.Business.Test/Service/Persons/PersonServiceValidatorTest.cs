using ECA.Business.Service.Persons;
using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class PersonServiceValidatorTest
    {
        private TestEcaContext context;
        private SevisValidationService sevisService;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            sevisService = new SevisValidationService();
        }

        [TestMethod]
        public void TestValid()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var participant = new Participant();
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
            var isPlaceOfBirthUnknown = false;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();

            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                null,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isPlaceOfBirthUnknown);

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

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.GENDER_NOT_FOUND, validationResult.ErrorMessage);
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
            var isPlaceOfBirthUnknown = false;

            var entity = new PersonServiceValidationEntity(
                person,
                genderId,
                null,
                cityOfBirth.LocationId,
                isPlaceOfBirthUnknown);

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
            var isPlaceOfBirthUnknown = false;
            var countriesOfCitizenship = new List<Location>();

            var entity = new PersonServiceValidationEntity(
                person,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isPlaceOfBirthUnknown);

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.COUNTRIES_OF_CITIZENSHIP_REQUIRED, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestPlaceOfBirthUnknownCityNotNull()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var participant = new Participant();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
            cityOfBirth.LocationId = 3265;
            var isPlaceOfBirthUnknown = true;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();
            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isPlaceOfBirthUnknown);

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.PLACE_OF_BIRTH_ERROR, validationResult.ErrorMessage);
        }
        
        [TestMethod]
        public void TestSevisValidator_NullStudent()
        {
            var batchHeader = new BatchHeader
            {
                BatchID = "1",
                OrgID = "1"
            };
            var createStudent = new CreateStudent
            {
                student = null
            };
            var updateStudent = new SEVISBatchCreateUpdateStudent
            {
                userID = "1",
                batchHeader = batchHeader,
                createStudent = createStudent
            };

            var validator = new SEVISBatchCreateUpdateStudentValidator();
            var results = validator.Validate(updateStudent);
            Assert.AreEqual(1, results.Errors.Count());
        }

        [TestMethod]
        public void TestSevisValidator_NullStudentIssueReason()
        {
            var batchHeader = new BatchHeader
            {
                BatchID = "1",
                OrgID = "1"
            };
            var student = new Student
            {
                requestID = "1",
                userID = "1",
                printForm = false,
                UserDefinedA = "2",
                UserDefinedB = "3",
                IssueReason = null
            };
            var createStudent = new CreateStudent
            {
                student = student
            };
            var updateStudent = new SEVISBatchCreateUpdateStudent
            {
                userID = "1",
                batchHeader = batchHeader,
                createStudent = createStudent
            };

            var validator = new SEVISBatchCreateUpdateStudentValidator();
            var results = validator.Validate(updateStudent);
            Assert.AreEqual(1, results.Errors.Count());
        }

    }
}
