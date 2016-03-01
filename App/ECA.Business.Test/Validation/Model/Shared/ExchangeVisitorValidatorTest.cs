using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Model.CreateEV;
using ECA.Business.Validation.Model;
using ECA.Business.Validation.Model.Shared;
using ECA.Data;
using ECA.Business.Service.Persons;

namespace ECA.Business.Test.Validation.Model.Shared
{
    [TestClass]
    public class ExchangeVisitorValidatorTest
    {
        private TestEcaContext context;
        private ExchangeVisitorService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ExchangeVisitorService(context);
        }

        private ExchangeVisitor GetValidExchangeVisitor()
        {
            var instance = new ExchangeVisitor
            {
                AddSiteOfActivity = null,
                AddTIPP = null,
                Biographical = new Biographical
                {
                    FullName = new FullName
                    {
                        LastName = "last"
                    },
                    BirthCity = "pensacola",
                    BirthDate = DateTime.Now,
                    BirthCountryCode = "UK",
                    Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                    CitizenshipCountryCode = "UK",
                    PermanentResidenceCountryCode = "UK",
                    BirthCountryReason = "aa",
                    EmailAddress = "someone@isp.com"

                },
                CategoryCode = "cc",
                CreateDependent = null,
                FinancialInfo = new FinancialInfo
                {
                    ReceivedUSGovtFunds = true,
                    OtherFunds = new OtherFunds
                    {
                        USGovt = null,
                        International = null,
                        Other = null
                    },
                    
                },
                MailAddress = null,
                OccupationCategoryCode = null,
                PositionCode = "aaa",
                PrgEndDate = DateTime.Now.AddDays(1.0),
                PrgStartDate = DateTime.Now,
                ResidentialAddress = null,
                SubjectField = new SubjectField
                {
                    Remarks = "remarks",
                    SubjectFieldCode = "00.0000"
                },
                USAddress = null,
                requestID = "request",
                userID = "1"
            };
            service.SetAddSiteOfActivity(instance);
            return instance;
        }

        [TestMethod]
        public void TestBiographical_Null()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.Biographical = null;
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.BIOGRAPHICAL_INFORMATION_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestBiographical_ShouldRunValidator()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.Biographical.FullName.LastName = null;
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
        }

        [TestMethod]
        public void TestPositionCode_DoesNotHaveRequiredLength()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.PositionCode = new string('c', ExchangeVisitorValidator.POSITION_CODE_LENGTH - 1);
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.POSITION_CODE_LENGTH_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestPositionCode_ExceedsMaxLength()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.PositionCode = new string('c', ExchangeVisitorValidator.POSITION_CODE_LENGTH + 1);
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.POSITION_CODE_LENGTH_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestPositionCode_Null()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.PositionCode = null;
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.POSITION_CODE_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestProgramStartDate_DefaultValue()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.PrgStartDate = default(DateTime);
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.PROGRAM_START_DATE_REQUIRED_ERROR_MESSAGE, results.Errors.Last().ErrorMessage);
        }

        [TestMethod]
        public void TestProgramEndDate_DefaultValue()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.PrgEndDate = default(DateTime);
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(2, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.PROGRAM_END_DATE_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.AreEqual(ExchangeVisitorValidator.PROGRAM_END_DATE_MUST_BE_AFTER_START_DATE_ERROR, results.Errors.Last().ErrorMessage);
        }

        [TestMethod]
        public void TestProgramEndDate_PrgEndDateIsBeforePrgStartDate()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.PrgStartDate = DateTime.UtcNow;
            instance.PrgEndDate = DateTime.UtcNow.AddDays(-1.0);

            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.PROGRAM_END_DATE_MUST_BE_AFTER_START_DATE_ERROR, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestCategoryCode_Null()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.CategoryCode = null;
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.CATEGORY_CODE_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestCategoryCode_ExceedsLength()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.CategoryCode = new string('c', ExchangeVisitorValidator.CATEGORY_CODE_LENGTH + 1);
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.PROGRAM_CATEGORY_CODE_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestOccupationCategoryCode_ExceedsLength()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.OccupationCategoryCode = new string('c', ExchangeVisitorValidator.CATEGORY_CODE_LENGTH + 1);
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.OCCUPATION_CATEGORY_CODE_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestOccupationCategoryCode_Whitespace()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.OccupationCategoryCode = " ";
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.OCCUPATION_CATEGORY_CODE_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestOccupationCategoryCode_Null()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.OccupationCategoryCode = null;
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestOccupationCategoryCode_Empty()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.OccupationCategoryCode = String.Empty;
            results = validator.Validate(instance);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.OCCUPATION_CATEGORY_CODE_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestSubjectField_ShouldRunValidator()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.SubjectField = null;
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.SUBJECT_FIELD_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }


        [TestMethod]
        public void TestUSAddress_ShouldRunValidator()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.USAddress = new USAddress();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void TestMailAddress_ShouldRunValidator()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.MailAddress = new USAddress();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
        }

        [TestMethod]
        public void TestFinancialInfo_ShouldRunValidator()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.FinancialInfo = null;
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.FINANCIAL_INFO_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestAddSiteOfActivity_ShouldRunValidator()
        {
            var instance = GetValidExchangeVisitor();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            instance.AddSiteOfActivity = null;
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.SITE_OF_ACTIVITY_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }
    }
}
