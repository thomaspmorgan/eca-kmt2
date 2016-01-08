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
using System.Xml.Linq;
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
            Assert.IsTrue(results.Errors.Any(o => o.ErrorMessage == "Exchange Visitor: Biographical Information is required"));
        }

        /// <summary>
        /// Validate that exchange visitor biographical info is missing three fields
        /// </summary>
        [TestMethod]
        public void TestCreateSevisValidator_MissingBiographicalRequiredFields()
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
                            FirsName = "Jon",
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
            Assert.IsTrue(results.Errors.Count == 3);
        }

        /// <summary>
        /// Validate that object is serialized to valid XML
        /// </summary>
        [TestMethod]
        public void TestCreateSevisValidator_ReturnValidXML()
        {
            var createEV = new CreateExchVisitor
            {
                ExchangeVisitor = new ExchangeVisitor
                {
                    requestID = "123456789",
                    userID = "1",
                    PositionCode = "100",
                    PrgStartDate = new DateTime(1998, 4, 12),
                    PrgEndDate = new DateTime(2001, 4, 12),
                    CategoryCode = "05",
                    OccupationCategoryCode = "99",
                    Biographical = new Biographical
                    {
                        FullName = new FullName
                        {
                            LastName = "Doe"
                        },
                        BirthDate = new DateTime(1988, 2, 23),
                        Gender = "1",
                        BirthCity = "Arlington",
                        BirthCountryCode = "US",
                        CitizenshipCountryCode = "US",
                        ResidentialAddress = null
                    },
                    SubjectField = new SubjectField
                    {
                        SubjectFieldCode = "100",
                        ForeignDegreeLevel = "",
                        ForeignFieldOfStudy = "",
                        Remarks = "subject field"
                    },
                    USAddress = null,
                    MailAddress = null,
                    FinancialInfo = new FinancialInfo
                    {
                        ReceivedUSGovtFunds = false,
                        ProgramSponsorFunds = "23000",
                        OtherFunds = new OtherFunds
                        { }
                    },
                    CreateDependent = null,
                    AddTIPP = new AddTIPP
                    {
                        print7002 = false,
                        TippExemptProgram = null,
                        ParticipantInfo = null,
                        TippSite = null
                    },
                    ResidentialAddress = null,
                    AddSiteOfActivity = new AddSiteOfActivity
                    {
                        SiteOfActivitySOA = new SiteOfActivitySOA
                        {
                            printForm = false,
                            Address1 = "2201 C St NW",
                            City = "Washington",
                            State = "DC",
                            PostalCode = "20520",
                            SiteName = "US Department of State",
                            PrimarySite = true,
                            Remarks = ""
                        },
                        SiteOfActivityExempt = new SiteOfActivityExempt
                        {
                            Remarks = ""
                        }
                    }
                }
            };
            
            List<CreateExchVisitor> createEVs = new List<CreateExchVisitor>();
            createEVs.Add(createEV);

            // create batch header
            var batchHeader = new BatchHeader
            {
                BatchID = DateTime.Today.ToString(),
                OrgID = "453"
            };
            // create batch
            var createEVBatch = new SEVISBatchCreateUpdateEV
            {
                userID = "1",
                BatchHeader = batchHeader,
                UpdateEV = null,
                CreateEV = createEVs
            };

            var batchXML = GetSevisBatchXml(createEVBatch);
            
            Assert.IsTrue(CheckXml(batchXML));
        }

        /// <summary>
        /// Check that XMl is well-formed
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private bool CheckXml(string xml)
        {
            using (XmlReader xr = XmlReader.Create(new StringReader(xml)))
            {
                try
                {
                    while (xr.Read()) { }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Retrieve XMl of sevis batch object
        /// </summary>
        /// <param name="validationEntity"></param>
        /// <returns></returns>
        private string GetSevisBatchXml(SEVISBatchCreateUpdateEV validationEntity)
        {
            XmlSerializer serializer = new XmlSerializer(validationEntity.GetType());
            var settings = new XmlWriterSettings
            {
                NewLineHandling = NewLineHandling.Entitize,
                Encoding = System.Text.Encoding.UTF8,
                DoNotEscapeUriAttributes = true
            };
            using (var stream = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, validationEntity);
                    return stream.ToString();
                }
            }
        }


        #endregion

    }
}
