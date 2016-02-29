using ECA.Business.Service.Persons;
using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Validation.Model.Shared;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

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
        /// Validate that exchange visitor biographical info is null
        /// </summary>
        [TestMethod]
        public void TestCreateSevisValidator_NullBiographical()
        {
            var validator = new CreateExchVisitorValidator();

            var createEV = new CreateExchVisitor
            {
                ExchangeVisitor = new ExchangeVisitor
                {
                    requestID = "1",
                    userID = "1",
                    printForm = false,
                    Biographical = null,
                    PositionCode = "100",
                    PrgStartDate = new DateTime(1998, 4, 12),
                    PrgEndDate = new DateTime(2001, 4, 12),
                    CategoryCode = "05",
                    SubjectField = new SubjectField
                    {
                        SubjectFieldCode = "01.0103",
                        Remarks = "test"
                    },
                    FinancialInfo = new FinancialInfo
                    {
                        ReceivedUSGovtFunds = false,
                        OtherFunds = new OtherFunds
                        { }
                    },
                    AddSiteOfActivity = new AddSiteOfActivity
                    {
                        SiteOfActivitySOA = new SiteOfActivitySOA
                        {
                            printForm = false,
                            Address1 = "123 Some St",
                            PostalCode = "10189",
                            SiteName = "Site 1",
                            PrimarySite = true
                        },
                        SiteOfActivityExempt = new SiteOfActivityExempt
                        { }
                    }
                }
            };

            var results = validator.Validate(createEV);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.Errors.Any(o => o.ErrorMessage == "Exch. Visitor: Biographical Information is required"));
        }

        /// <summary>
        /// Validate that exchange visitor biographical info is missing three fields
        /// </summary>
        [TestMethod]
        public void TestCreateSevisValidator_BiographicalRequired_BirthDate_NULL()
        {
            var validator = new CreateExchVisitorValidator();

            var createEV = new CreateExchVisitor
            {
                ExchangeVisitor = new ExchangeVisitor
                {
                    requestID = "1",
                    userID = "1",
                    printForm = false,
                    Biographical = new Biographical
                    {
                        FullName = new FullName
                        {
                            FirstName = "Jon",
                            LastName = "Doe"
                        },
                        Gender = "M",
                        BirthCity = "Somecity",
                        BirthCountryCode = "US"
                    },
                    PositionCode = "100",
                    PrgStartDate = new DateTime(1998, 4, 12),
                    PrgEndDate = new DateTime(2001, 4, 12),
                    CategoryCode = "05",
                    SubjectField = new SubjectField
                    {
                        SubjectFieldCode = "01.0103",
                        Remarks = "test"
                    },
                    USAddress = null,
                    MailAddress = null,
                    FinancialInfo = new FinancialInfo
                    {
                        ReceivedUSGovtFunds = false,
                        OtherFunds = new OtherFunds
                        {
                            USGovt = null,
                            International = null,
                            EVGovt = null,
                            BinationalCommission = null,
                            Other = null
                        }
                    },
                    CreateDependent = null,
                    AddSiteOfActivity = new AddSiteOfActivity
                    {
                        SiteOfActivitySOA = new SiteOfActivitySOA
                        {
                            printForm = false,
                            Address1 = "123 Some St",
                            PostalCode = "10189",
                            SiteName = "Site 1",
                            PrimarySite = true
                        },
                        SiteOfActivityExempt = new SiteOfActivityExempt
                        { }
                    },
                    AddTIPP = null,
                    ResidentialAddress = null
                }
            };

            var results = validator.Validate(createEV);
            Assert.IsFalse(results.IsValid);            
            Assert.IsTrue(results.Errors.Any(o => o.ErrorMessage == "EV Biographical Info: Date of Birth is required"));
        }
        
        #endregion

    }
}
