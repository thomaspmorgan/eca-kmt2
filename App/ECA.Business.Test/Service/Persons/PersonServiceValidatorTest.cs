using ECA.Business.Service.Persons;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.Create;
using ECA.Business.Validation.Model.Shared;
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
        //private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            //context = new TestEcaContext();
        }

        #region DoValidateCreate

        [TestMethod]
        public void TestDoValidateCreate_Valid()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var participant = new Participant();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
            var isPlaceOfBirthUnknown = false;
            var isDateOfBirthEstimated = false;
            var isDateOfBirthUnknown = false;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();

            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                DateTime.Now,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown
                );

            var results = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestDoValidateCreate_NullPerson()
        {
            var validator = new PersonServiceValidator();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
            var isPlaceOfBirthUnknown = false;
            var isDateOfBirthEstimated = false;
            var isDateOfBirthUnknown = false;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();

            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                null,
                dateOfBirth,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown);

            var results = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.PERSON_NOT_FOUND, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_GenderIdNotFound()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var genderId = 0;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
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

            var results = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.GENDER_NOT_FOUND, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_PlaceOfBirthUnknownCityNotNull()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var participant = new Participant();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
            cityOfBirth.LocationId = 3265;
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

            var results = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.PLACE_OF_BIRTH_ERROR, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_DateOfBirthEstimatedAndNoDateOfBirthGiven()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var participant = new Participant();
            var genderId = 1;
            var cityOfBirth = new Location();
            cityOfBirth.LocationId = 3265;
            var isPlaceOfBirthUnknown = false;
            var isDateOfBirthEstimated = true;
            var isDateOfBirthUnknown = false;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();
            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                null,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown);

            var results = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.DATE_OF_BIRTH_ESTIMATED_BUT_NO_DATE_OF_BIRTH_GIVEN, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_DateOfBirthUnknownAndDateOfBirthGiven()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var dateOfBirth = DateTime.Now;
            var participant = new Participant();
            var genderId = 1;
            var cityOfBirth = new Location();
            cityOfBirth.LocationId = 3265;
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

            var results = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.DATE_OF_BIRTH_UNKONWN_BUT_DATE_OF_BIRTH_GIVEN, validationResult.ErrorMessage);
        }
        #endregion


        #region DoValidateUpdate

        [TestMethod]
        public void TestDoValidateUpdate_Valid()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var participant = new Participant();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
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
                isPlaceOfBirthUnknown
                );

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void TestDoValidateUpdate_NullPerson()
        {
            var validator = new PersonServiceValidator();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
            var isPlaceOfBirthUnknown = false;
            var isDateOfBirthEstimated = false;
            var isDateOfBirthUnknown = false;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();

            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                null,
                dateOfBirth,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown);

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.PERSON_NOT_FOUND, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_GenderIdNotFound()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var genderId = 0;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
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

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.GENDER_NOT_FOUND, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_PlaceOfBirthUnknownCityNotNull()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var participant = new Participant();
            var genderId = 1;
            var dateOfBirth = DateTime.Now;
            var cityOfBirth = new Location();
            cityOfBirth.LocationId = 3265;
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

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.PLACE_OF_BIRTH_ERROR, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_DateOfBirthEstimatedAndNoDateOfBirthGiven()
        {
            var validator = new PersonServiceValidator();
            var participant = new Participant();
            var person = new Person();
            var genderId = 1;
            var cityOfBirth = new Location();
            cityOfBirth.LocationId = 3265;
            var isPlaceOfBirthUnknown = false;
            var isDateOfBirthEstimated = true;
            var isDateOfBirthUnknown = false;
            var countriesOfCitizenship = new List<Location>();
            var countryOfCitizenship = new Location();
            countriesOfCitizenship.Add(countryOfCitizenship);

            var entity = new PersonServiceValidationEntity(
                person,
                null,
                genderId,
                countriesOfCitizenship,
                cityOfBirth.LocationId,
                isDateOfBirthUnknown,
                isDateOfBirthEstimated,
                isPlaceOfBirthUnknown);

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.DATE_OF_BIRTH_ESTIMATED_BUT_NO_DATE_OF_BIRTH_GIVEN, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateUpdate_DateOfBirthUnknownAndDateOfBirthGiven()
        {
            var validator = new PersonServiceValidator();
            var person = new Person();
            var dateOfBirth = DateTime.Now;
            var participant = new Participant();
            var genderId = 1;
            var cityOfBirth = new Location();
            cityOfBirth.LocationId = 3265;
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

            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(PersonServiceValidator.DATE_OF_BIRTH_UNKONWN_BUT_DATE_OF_BIRTH_GIVEN, validationResult.ErrorMessage);
        }
        #endregion

        #region Sevis Validation

        /// <summary>
        /// Validate that student record is null
        /// </summary>
        [TestMethod]
        public void TestSevisValidator_NullStudent()
        {
            var validator = new CreateStudentValidator();

            var createStudent = new CreateStudent
            {
                student = null
            };

            var results = validator.Validate(createStudent);            
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.Errors.Any(o => o.ErrorMessage == "Student information is required"));
        }
        
        /// <summary>
        /// Validate student record issue reason is null
        /// </summary>
        [TestMethod]
        public void TestSevisValidator_NullStudentIssueReason()
        {
            var validator = new StudentValidator();

            var student = new Student
            {
                requestID = "1",
                userID = "1",
                printForm = false,
                IssueReason = null
            };

            var results = validator.Validate(student);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.Errors.Any(o => o.ErrorMessage == "Student: Issue Reason is required"));
        }

        #endregion

    }
}
