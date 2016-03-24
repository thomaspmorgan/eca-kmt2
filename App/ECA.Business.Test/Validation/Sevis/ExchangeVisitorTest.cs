using ECA.Business.Queries.Models.Admin;
using FluentAssertions;
using System.Linq;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Sevis.Model;
using ECA.Business.Validation;
using ECA.Business.Validation.Sevis;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Business.Validation.Sevis.Finance;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using FluentValidation;
using Moq;
using ECA.Business.Test.Validation.Sevis.Bio;
using Newtonsoft.Json;

namespace ECA.Business.Test.Validation.Sevis
{
    [TestClass]
    public class ExchangeVisitorTest
    {
        public AddressDTO GetSOAAsAddressDTO()
        {
            var stateName = "TN";
            return new AddressDTO
            {
                Street1 = "street 1",
                Street2 = "street 2",
                City = "city",
                Division = stateName,
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME,
                LocationName = "location name",
                PostalCode = "postal code",
            };
        }

        public Business.Validation.Sevis.Bio.Person GetPerson(bool setMailAddress = true, bool setUSAddress = true)
        {
            var state = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.Division = state;

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;

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
            var phone = "123-456-7890";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReason = "reason";
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);

            var person = new Business.Validation.Sevis.Bio.Person(
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
                programCataegoryCode,
                subjectField,
                setMailAddress ? mailAddress : null,
                setUSAddress ? usAddress : null,
                printForm,
                personId,
                participantId);
            return person;
        }

        public FinancialInfo GetFinancialInfo()
        {
            var other = new Other(null, null);
            var usGovt = new USGovernmentFunding(null, null, null, null, null, null);
            var international = new InternationalFunding(null, null, null, null, null, null);
            var otherFunds = new OtherFunds(null, null, null, usGovt, international, other);

            var programSponsorFunds = "100";
            var receivedUsGovtFunds = true;
            var printForm = true;
            var financialInfo = new FinancialInfo(
                    printForm: printForm,
                    receivedUSGovtFunds: receivedUsGovtFunds,
                    programSponsorFunds: programSponsorFunds,
                    otherFunds: otherFunds);
            return financialInfo;
        }

        [TestMethod]
        public void TestConstructor()
        {
            var userId = 10;
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            var sevisId = "sevis id";

            var exchangeVisitor = new ExchangeVisitor(
                user,
                sevisId,
                person,
                financialInfo,
                occupationCategoryCode,
                endDate,
                startDate,
                siteOfActivity,
                dependents);
            Assert.IsTrue(Object.ReferenceEquals(user, exchangeVisitor.User));
            Assert.IsTrue(Object.ReferenceEquals(person, exchangeVisitor.Person));
            Assert.IsTrue(Object.ReferenceEquals(financialInfo, exchangeVisitor.FinancialInfo));
            Assert.IsTrue(Object.ReferenceEquals(dependents, exchangeVisitor.Dependents));
            Assert.IsTrue(Object.ReferenceEquals(siteOfActivity, exchangeVisitor.SiteOfActivity));

            Assert.AreEqual(occupationCategoryCode, exchangeVisitor.OccupationCategoryCode);
            Assert.AreEqual(endDate, exchangeVisitor.ProgramEndDate);
            Assert.AreEqual(startDate, exchangeVisitor.ProgramStartDate);
            Assert.AreEqual(sevisId, exchangeVisitor.SevisId);
        }

        [TestMethod]
        public void TestToJson()
        {
            var userId = 10;
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            var sevisId = "sevis id";

            var exchangeVisitor = new ExchangeVisitor(
                user,
                sevisId,
                person,
                financialInfo,
                occupationCategoryCode,
                endDate,
                startDate,
                siteOfActivity,
                dependents);

            var json = exchangeVisitor.ToJson();
            Assert.IsNotNull(json);

            var jsonObject = JsonConvert.DeserializeObject<ExchangeVisitor>(json);
            Assert.IsNotNull(jsonObject.User);
            Assert.IsNotNull(jsonObject.Person);
            Assert.IsNotNull(jsonObject.FinancialInfo);
            Assert.IsNotNull(jsonObject.Dependents);
            Assert.IsNotNull(jsonObject.SiteOfActivity);
           
            Assert.AreEqual(sevisId, jsonObject.SevisId);
            Assert.AreEqual(endDate, jsonObject.ProgramEndDate);
            Assert.AreEqual(startDate, jsonObject.ProgramStartDate);
            Assert.AreEqual(occupationCategoryCode, jsonObject.OccupationCategoryCode);
            Assert.AreEqual(userId, jsonObject.User.Id);
        }

        [TestMethod]
        public void TestGetExchangeVisitor_HasAddedDependent()
        {
            var addedDependent = new AddedDependent(null, null, null, null, null, null, null, null, null, null, null, null, null, true, 1, 2);

            var userId = 10;
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            dependents.Add(addedDependent);
            var sevisId = "sevis id";

            var exchangeVisitor = new ExchangeVisitor(
                user,
                sevisId,
                person,
                financialInfo,
                occupationCategoryCode,
                endDate,
                startDate,
                siteOfActivity,
                dependents);

            var json = exchangeVisitor.ToJson();
            Assert.IsNotNull(json);

            var jsonObject = ExchangeVisitor.GetExchangeVisitor(json);
            Assert.IsNotNull(jsonObject.Dependents);
            Assert.AreEqual(1, jsonObject.Dependents.Count());

            var firstJsonDependent = jsonObject.Dependents.First();
            Assert.IsInstanceOfType(firstJsonDependent, typeof(AddedDependent));
        }

        [TestMethod]
        public void TestGetExchangeVisitor_HasUpdatedDependent()
        {
            var updatedDependent = new UpdatedDependent(null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, 1, 2);

            var userId = 10;
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            dependents.Add(updatedDependent);
            var sevisId = "sevis id";

            var exchangeVisitor = new ExchangeVisitor(
                user,
                sevisId,
                person,
                financialInfo,
                occupationCategoryCode,
                endDate,
                startDate,
                siteOfActivity,
                dependents);

            var json = exchangeVisitor.ToJson();
            Assert.IsNotNull(json);

            var jsonObject = ExchangeVisitor.GetExchangeVisitor(json);
            Assert.IsNotNull(jsonObject.Dependents);
            Assert.AreEqual(1, jsonObject.Dependents.Count());

            var firstJsonDependent = jsonObject.Dependents.First();
            Assert.IsInstanceOfType(firstJsonDependent, typeof(UpdatedDependent));
        }

        [TestMethod]
        public void TestGetExchangeVisitor_HasMultipleDependents()
        {
            var updatedDependent = new UpdatedDependent(null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, 1, 2);
            var addedDependent = new AddedDependent(null, null, null, null, null, null, null, null, null, null, null, null, null, true, 1, 2);

            var userId = 10;
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            dependents.Add(updatedDependent);
            dependents.Add(addedDependent);
            var sevisId = "sevis id";

            var exchangeVisitor = new ExchangeVisitor(
                user,
                sevisId,
                person,
                financialInfo,
                occupationCategoryCode,
                endDate,
                startDate,
                siteOfActivity,
                dependents);

            var json = exchangeVisitor.ToJson();
            Assert.IsNotNull(json);

            var jsonObject = ExchangeVisitor.GetExchangeVisitor(json);
            Assert.IsNotNull(jsonObject.Dependents);
            Assert.AreEqual(2, jsonObject.Dependents.Count());

            var firstJsonDependent = jsonObject.Dependents.First();
            Assert.IsInstanceOfType(firstJsonDependent, typeof(UpdatedDependent));

            var lastJsonDependent = jsonObject.Dependents.Last();
            Assert.IsInstanceOfType(lastJsonDependent, typeof(AddedDependent));
        }


        [TestMethod]
        public void TestConstructor_NullDependents()
        {
            var userId = 10;
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            List<Dependent> dependents = null;
            var sevisId = "sevis id";

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            Assert.IsNotNull(exchangeVisitor.Dependents);
            Assert.AreEqual(0, exchangeVisitor.Dependents.Count());
        }

        #region GetSEVISBatchTypeExchangeVisitor

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_CheckProperties()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor();
            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Biographical);
            Assert.IsNotNull(instance.SubjectField);
            Assert.IsNotNull(instance.FinancialInfo);
            Assert.IsNotNull(instance.MailAddress);
            Assert.IsNotNull(instance.USAddress);

            Assert.IsNull(instance.ResidentialAddress);

            Assert.AreEqual(person.ProgramCategoryCode.GetEVCategoryCodeType(), instance.CategoryCode);
            Assert.AreEqual(occupationCategoryCode.GetEVOccupationCategoryCodeType(), instance.OccupationCategoryCode);
            Assert.IsTrue(instance.OccupationCategoryCodeSpecified);
            Assert.AreEqual((short)Int32.Parse(person.PositionCode), instance.PositionCode);
            Assert.AreEqual(startDate, instance.PrgStartDate);
            Assert.AreEqual(endDate, instance.PrgEndDate);
            Assert.IsTrue(instance.printForm);
            Assert.AreEqual(person.ParticipantId.ToString(), instance.requestID);
            Assert.IsNotNull(instance.UserDefinedA);
            Assert.IsNotNull(instance.UserDefinedB);

            var key = new ParticipantSevisKey(instance.UserDefinedA, instance.UserDefinedB);
            Assert.AreEqual(person.ParticipantId, key.ParticipantId);
            Assert.AreEqual(person.PersonId, key.PersonId);
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_MailAddressIsNull()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson(false, true);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor();
            Assert.IsNotNull(instance);
            Assert.IsNull(instance.MailAddress);
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_USAddressIsNull()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson(true, false);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor();
            Assert.IsNotNull(instance);
            Assert.IsNull(instance.USAddress);
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_CheckItems()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor();
            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Items);
            Assert.AreEqual(2, instance.Items.Count());
            var addTippObject = instance.Items.First();
            var siteOfActivityObject = instance.Items.Last();
            Assert.IsInstanceOfType(addTippObject, typeof(AddTIPP));
            Assert.IsInstanceOfType(siteOfActivityObject, typeof(AddSiteOfActivity));
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_CheckDependents()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

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
            var phone = "123-456-7890";
            var mailAddress = new AddressDTO
            {
                AddressId = 1,
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME
            };
            var usAddress = new AddressDTO
            {
                AddressId = 2,
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME
            };
            var printForm = true;
            var birthCountryReason = "reason";
            var relationship = DependentCodeType.Item01.ToString();
            var addedDependent = new AddedDependent(
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
                relationship,
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId
                );
            dependents.Add(addedDependent);

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor();
            Assert.IsNotNull(instance);
            Assert.AreEqual(1, instance.CreateDependent.Count());
            var firstDependent = instance.CreateDependent.First();
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_CheckDependents_NoDependents()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor();
            Assert.IsNotNull(instance);
            Assert.AreEqual(0, instance.CreateDependent.Count());
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_CheckDependents_CheckMustBeAddedDepenent()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
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
            var phone = "123-456-7890";
            var mailAddress = new AddressDTO
            {
                AddressId = 1,
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME
            };
            var usAddress = new AddressDTO
            {
                AddressId = 2,
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME
            };
            var printForm = true;
            var birthCountryReason = "reason";
            var dependentSevisId = "sevis id";
            var remarks = "remarks";
            var relationship = DependentCodeType.Item01.ToString();

            var updatedDependent = new UpdatedDependent(
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
                relationship,
                mailAddress,
                usAddress,
                printForm,
                dependentSevisId,
                remarks,
                personId,
                participantId
                );
            dependents.Add(updatedDependent);
            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            Action a = () => exchangeVisitor.GetSEVISBatchTypeExchangeVisitor();
            a.ShouldThrow<NotSupportedException>().WithMessage("The dependent must be an added dependent.");
        }

        #endregion

        #region AddTIPP
        [TestMethod]
        public void TestAddTIPP()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);
            var instance = exchangeVisitor.GetAddTipp();
            Assert.IsFalse(instance.print7002);
            Assert.IsNotNull(instance.Items);
            Assert.AreEqual(0, instance.Items.Count());
        }
        #endregion

        #region Site of Activity
        [TestMethod]
        public void TestGetSOA()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSOA();
            Assert.IsNotNull(instance);
            Assert.AreEqual(siteOfActivity.Street1, instance.Address1);
            Assert.AreEqual(siteOfActivity.Street2, instance.Address2);
            Assert.AreEqual(siteOfActivity.City, instance.City);
            Assert.AreEqual(siteOfActivity.Division.GetStateCodeType(), instance.State);
            Assert.AreEqual(siteOfActivity.LocationName, instance.SiteName);
            Assert.IsTrue(instance.StateSpecified);
            Assert.IsFalse(instance.ExplanationCodeSpecified);
            Assert.IsNull(instance.ExplanationCode);
            Assert.IsNull(instance.Explanation);
        }

        [TestMethod]
        public void TestGetAddSiteOfActivity()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetAddSiteOfActivity();
            Assert.AreEqual(1, instance.SiteOfActivity.Count());
            Assert.IsInstanceOfType(instance.SiteOfActivity.First(), typeof(SOA));
        }

        #endregion Check Serialization

        #region GetSEVISEVBatchTypeExchangeVisitor1Collection
        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitor1Collection()
        {
            var userId = 10;
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            List<Dependent> dependents = new List<Dependent>();
            var testDependent = new TestDependent();
            dependents.Add(testDependent);
            var sevisId = "sevis id";

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            Action<SEVISEVBatchTypeExchangeVisitor1> propertyTester = (visitor) =>
            {
                Assert.AreEqual(person.ParticipantId.ToString(), visitor.requestID);
                Assert.AreEqual(sevisId, visitor.sevisID);
                Assert.AreEqual(userId.ToString(), visitor.userID);
                Assert.IsFalse(visitor.statusCodeSpecified);
            };

            var list = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection().ToList();
            Assert.AreEqual(4, list.Count);
            list.ForEach(x => propertyTester(x));

            var personVisitorItem = CreateGetItemQuery<SEVISEVBatchTypeExchangeVisitorBiographical>(list).FirstOrDefault();

            var financialVisitorItem = CreateGetItemQuery<SEVISEVBatchTypeExchangeVisitorFinancialInfo>(list).FirstOrDefault();
            Assert.IsNotNull(financialVisitorItem);

            var dependentVisitorItemsCount = CreateGetItemQuery<SEVISEVBatchTypeExchangeVisitorDependent>(list).Count();
            Assert.AreEqual(1, dependentVisitorItemsCount);

            var dependentVisitorItem = CreateGetItemQuery<SEVISEVBatchTypeExchangeVisitorDependent>(list).FirstOrDefault();
            Assert.IsNotNull(dependentVisitorItem);
            Assert.IsNotNull(dependentVisitorItem.Item);
            Assert.IsNull(dependentVisitorItem.UserDefinedA);
            Assert.IsNull(dependentVisitorItem.UserDefinedB);

            var exchangeVisitorPrograms = CreateGetItemQuery<SEVISEVBatchTypeExchangeVisitorProgram>(list).ToList();
            Assert.AreEqual(1, exchangeVisitorPrograms.Count);
            

            var exchangeVisitorProgramItems = exchangeVisitorPrograms
                .Select(x => x.Item)
                .Where(x => x.GetType() == typeof(SEVISEVBatchTypeExchangeVisitorProgram))
                .Select(x => (SEVISEVBatchTypeExchangeVisitorProgram)x)
                .ToList();

            Assert.AreEqual(1, exchangeVisitorPrograms.Count);
            var editSubjectExchangeVisitorProgramItem = exchangeVisitorProgramItems
                .ToList<SEVISEVBatchTypeExchangeVisitorProgram>()
                .Where(x => x.Item.GetType() == typeof(SEVISEVBatchTypeExchangeVisitorProgramEditSubject))
                .FirstOrDefault();
            Assert.IsNotNull(editSubjectExchangeVisitorProgramItem);
        }

        private IQueryable<SEVISEVBatchTypeExchangeVisitor1> CreateGetItemQuery<T>(List<SEVISEVBatchTypeExchangeVisitor1> items)
        {
            return items.Where(x => x.Item.GetType() == typeof(T)).AsQueryable();
        }
        #endregion

        #region Validate
        [TestMethod]
        public void TestValidate()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var validator = new Mock<AbstractValidator<ExchangeVisitor>>();
            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            exchangeVisitor.Validate(validator.Object);
            validator.Verify(x => x.Validate(It.Is<ExchangeVisitor>(y => y == exchangeVisitor)), Times.Once());
        }
        #endregion

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_CheckSerialization()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor();
            using (var textWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(instance.GetType());
                serializer.Serialize(textWriter, instance);
                var xml = textWriter.ToString();
                Assert.IsNotNull(xml);
            }
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitor1Collection_CheckSerialization()
        {
            var userId = 10;
            var sevisId = "sevis id";
            var user = new User(userId);
            var person = GetPerson();
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                user: user,
                sevisId: sevisId,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection();
            using (var textWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(instance.GetType());
                serializer.Serialize(textWriter, instance);
                var xml = textWriter.ToString();
                Assert.IsNotNull(xml);
            }
        }
    }
}
