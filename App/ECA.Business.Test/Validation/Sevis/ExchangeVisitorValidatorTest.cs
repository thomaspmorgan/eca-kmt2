using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Persons;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Validation.Sevis.ErrorPaths;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Business.Test.Validation.Sevis
{
    [TestClass]
    public class ExchangeVisitorValidatorTest
    {
        [TestInitialize]
        public void TestInit()
        {

        }

        [TestMethod]
        public void TestPerson_Null()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                fullName,
                birthCity,
                birthCountryCode,
                birthCountryReason,
                birthDate,
                citizenshipCountryCode,
                email,
                gender,
                permanentResidenceCountryCode,
                phone,
                remarks,
                positionCode.ToString(),
                programCategory,
                subjectField,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };

            Func<ExchangeVisitor> createEntityWithoutPerson = () =>
            {
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: null,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };

            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            person = null;
            instance = createEntityWithoutPerson();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.PERSON_INFORMATION_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void TestPerson_ShouldRunValidator()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            fullName.LastName = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
        }

        [TestMethod]
        public void TestProgramStartDate_DefaultValue()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            startDate = default(DateTime);
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.PROGRAM_START_DATE_REQUIRED_ERROR_MESSAGE, results.Errors.Last().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(StartDateErrorPath));
        }

        [TestMethod]
        public void TestProgramEndDate_DefaultValue()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            endDate = default(DateTime);
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(2, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.PROGRAM_END_DATE_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.AreEqual(ExchangeVisitorValidator.PROGRAM_END_DATE_MUST_BE_AFTER_START_DATE_ERROR, results.Errors.Last().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(EndDateErrorPath));
            Assert.IsInstanceOfType(results.Errors.Last().CustomState, typeof(EndDateErrorPath));
        }

        [TestMethod]
        public void TestProgramEndDate_ProgramEndDateIsBeforeProggramStartDate()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            startDate = DateTime.UtcNow;
            endDate = DateTime.UtcNow.AddDays(-1.0);

            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.PROGRAM_END_DATE_MUST_BE_AFTER_START_DATE_ERROR, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(EndDateErrorPath));
        }


        [TestMethod]
        public void TestOccupationCategoryCode_ExceedsLength()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            occupationCategoryCode = new string('c', ExchangeVisitorValidator.OCCUPATION_CATEGORY_CODE_LENGTH + 1);
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.OCCUPATION_CATEGORY_CODE_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsNull(results.Errors.First().CustomState);
        }


        [TestMethod]
        public void TestOccupationCategoryCode_Whitespace()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            occupationCategoryCode = " ";
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.OCCUPATION_CATEGORY_CODE_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsNull(results.Errors.First().CustomState);
        }

        [TestMethod]
        public void TestOccupationCategoryCode_Null()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            occupationCategoryCode = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);
        }

        [TestMethod]
        public void TestOccupationCategoryCode_Empty()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            occupationCategoryCode = String.Empty;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.OCCUPATION_CATEGORY_CODE_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsNull(results.Errors.First().CustomState);
        }


        [TestMethod]
        public void TestUSAddress_ShouldRunValidator()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            usAddress = new AddressDTO();
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestMailAddress_ShouldRunValidator()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            mailAddress = new AddressDTO();
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(AddressErrorPath));
        }

        [TestMethod]
        public void TestFinancialInfo_ShouldRunValidator()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            financialInfo = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.FINANCIAL_INFO_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsInstanceOfType(results.Errors.First().CustomState, typeof(FundingErrorPath));
        }

        [TestMethod]
        public void TestAddSiteOfActivity_ShouldRunValidator()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            usAddress = null;
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
            Assert.AreEqual(ExchangeVisitorValidator.SITE_OF_ACTIVITY_REQUIRED_ERROR_MESSAGE, results.Errors.First().ErrorMessage);
            Assert.IsNull(results.Errors.First().CustomState);
        }

        [TestMethod]
        public void TestDependents_ShouldRunValidator()
        {
            var exchangeVisitorSevisId = "sevis id";
            User user = new User(1);
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;
            mailAddress.Street1 = "mailing street 1";
            mailAddress.PostalCode = "11111";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            usAddress.Street1 = "us address";
            usAddress.PostalCode = "22222";

            var personId = 100;
            var participantId = 200;

            var firstName = "first";
            var lastName = "last";
            var passport = "passport";
            var preferred = "preferred";
            var suffix = "Jr.";
            var fullName = new FullName(firstName, lastName, passport, preferred, suffix);

            var birthCity = "birth city";
            var birthCountryCode = "CN";
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "ab";
            var remarks = "remarks";
            var programCategory = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, "remarks");
            FinancialInfo financialInfo = new FinancialInfo(true, true, "1", null);
            string occupationCategoryCode = ExchangeVisitorService.EXCHANGE_VISITOR_OCCUPATION_CATEGORY_CODE;
            DateTime startDate = DateTime.UtcNow.AddDays(-1.0);
            DateTime endDate = DateTime.UtcNow.AddDays(1.0);
            List<Dependent> dependents = new List<Dependent>();
            Business.Validation.Sevis.Bio.Person person = null;

            var isTravelingWithParticipant = true;
            Func<ExchangeVisitor> createEntity = () =>
            {
                person = new Business.Validation.Sevis.Bio.Person(
                    fullName,
                    birthCity,
                    birthCountryCode,
                    birthCountryReason,
                    birthDate,
                    citizenshipCountryCode,
                    email,
                    gender,
                    permanentResidenceCountryCode,
                    phone,
                    remarks,
                    positionCode.ToString(),
                    programCategory,
                    subjectField,
                    mailAddress,
                    usAddress,
                    printForm,
                    personId,
                    participantId);
                return new ExchangeVisitor(user: user,
                    sevisId: exchangeVisitorSevisId,
                    person: person,
                    financialInfo: financialInfo,
                    occupationCategoryCode: occupationCategoryCode,
                    programEndDate: endDate,
                    programStartDate: startDate,
                    siteOfActivity: usAddress,
                    dependents: dependents
                    );
            };
            
            
            Func<AddedDependent> createDependentEntity = () =>
            {
                var badPhoneNumber = "abc";
                return new AddedDependent(
                    fullName: fullName,
                    birthCity: birthCity,
                    birthCountryCode: birthCountryCode,
                    birthCountryReason: birthCountryReason,
                    birthDate: birthDate,
                    citizenshipCountryCode: citizenshipCountryCode,
                    emailAddress: "someone@isp.com",
                    gender: gender,
                    permanentResidenceCountryCode: permanentResidenceCountryCode,
                    phoneNumber: badPhoneNumber,
                    relationship: "relationship",
                    mailAddress: mailAddress,
                    usAddress: usAddress,
                    printForm: true,
                    personId: 10,
                    participantId: 20,
                    isTravelingWithParticipant: isTravelingWithParticipant);
            };

            var instance = createEntity();
            var validator = new ExchangeVisitorValidator();
            var results = validator.Validate(instance);
            Assert.IsTrue(results.IsValid);

            dependents.Add(createDependentEntity());
            instance = createEntity();
            results = validator.Validate(instance);
            Assert.IsFalse(results.IsValid);
            Assert.AreEqual(1, results.Errors.Count);
        }
    }
}
