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
            var stateIso = "TN";
            return new AddressDTO
            {
                Street1 = "street 1",
                Street2 = "street 2",
                City = "city",
                DivisionIso = stateIso,
                Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME,
                LocationName = "location name",
                PostalCode = "postal code",
            };
        }

        public Business.Validation.Sevis.Bio.Person GetPerson(DateTime birthDate, bool setMailAddress = true, bool setUSAddress = true)
        {
            if (birthDate == null)
            {
                birthDate = DateTime.UtcNow;
            }
            var mailAddressState = "TN";
            var mailAddress = new AddressDTO();
            mailAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            mailAddress.DivisionIso = mailAddressState;
            mailAddress.Street1 = "street1";
            mailAddress.Street2 = "street2";
            mailAddress.Street3 = "street3";
            mailAddress.City = "Nashville";
            mailAddress.PostalCode = "12345";

            var usAddressState = "DC";
            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.DivisionIso = usAddressState;

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
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "123-456-7890";
            short positionCode = 120;
            var printForm = true;
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);

            var person = new Business.Validation.Sevis.Bio.Person(
                fullName,
                birthCity,
                birthCountryCode,
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
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            var sevisId = "sevis id";
            var sevisOrgId = "abcde1234567890";
            var isValidated = true;

            var exchangeVisitor = new ExchangeVisitor(
                sevisId,
                sevisOrgId,
                isValidated,
                person,
                financialInfo,
                occupationCategoryCode,
                endDate,
                startDate,
                siteOfActivity,
                dependents);
            Assert.IsTrue(Object.ReferenceEquals(person, exchangeVisitor.Person));
            Assert.IsTrue(Object.ReferenceEquals(financialInfo, exchangeVisitor.FinancialInfo));
            Assert.IsTrue(Object.ReferenceEquals(dependents, exchangeVisitor.Dependents));
            Assert.IsTrue(Object.ReferenceEquals(siteOfActivity, exchangeVisitor.SiteOfActivity));

            Assert.AreEqual(occupationCategoryCode, exchangeVisitor.OccupationCategoryCode);
            Assert.AreEqual(endDate, exchangeVisitor.ProgramEndDate);
            Assert.AreEqual(startDate, exchangeVisitor.ProgramStartDate);
            Assert.AreEqual(sevisId, exchangeVisitor.SevisId);
            Assert.AreEqual(sevisOrgId, exchangeVisitor.SevisOrgId);
            Assert.AreEqual(isValidated, exchangeVisitor.IsValidated);
        }

        [TestMethod]
        public void TestToJson()
        {
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            var sevisId = "sevis id";
            var sevisOrgId = "abcde12347890";
            var isValidated = true;


            var exchangeVisitor = new ExchangeVisitor(
                sevisId,
                sevisOrgId,
                isValidated,
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
            Assert.IsNotNull(jsonObject.Person);
            Assert.IsNotNull(jsonObject.FinancialInfo);
            Assert.IsNotNull(jsonObject.Dependents);
            Assert.IsNotNull(jsonObject.SiteOfActivity);

            Assert.AreEqual(sevisId, jsonObject.SevisId);
            Assert.AreEqual(endDate, jsonObject.ProgramEndDate);
            Assert.AreEqual(startDate, jsonObject.ProgramStartDate);
            Assert.AreEqual(occupationCategoryCode, jsonObject.OccupationCategoryCode);
            Assert.AreEqual(sevisOrgId, exchangeVisitor.SevisOrgId);
            Assert.AreEqual(isValidated, exchangeVisitor.IsValidated);
        }

        [TestMethod]
        public void TestGetExchangeVisitor_CheckDependents()
        {
            var addedDependent = new AddedDependent(null, null, null, null, null, null, null, null, null, null, null, null, null, true, 1, 2, true, false);

            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            dependents.Add(addedDependent);
            var sevisId = "sevis id";
            var sevisOrgId = "abcde12347890";
            var isValidated = true;

            var exchangeVisitor = new ExchangeVisitor(
                sevisId,
                sevisOrgId,
                isValidated,
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
            var updatedDependent = new UpdatedDependent(null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, 1, 2, true, true);

            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            dependents.Add(updatedDependent);
            var sevisId = "sevis id";
            var sevisOrgId = "abcde12347890";
            var isValidated = true;

            var exchangeVisitor = new ExchangeVisitor(
                sevisId,
                sevisOrgId,
                isValidated,
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
            var updatedDependent = new UpdatedDependent(null, null, null, null, null, null, null, null, null, null, null, null, null, true, null, null, 1, 2, true, true);
            var addedDependent = new AddedDependent(null, null, null, null, null, null, null, null, null, null, null, null, null, true, 1, 2, true, false);

            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            dependents.Add(updatedDependent);
            dependents.Add(addedDependent);
            var sevisId = "sevis id";
            var sevisOrgId = "abcde12347890";
            var isValidated = true;

            var exchangeVisitor = new ExchangeVisitor(
                sevisId,
                sevisOrgId,
                isValidated,
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
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            List<Dependent> dependents = null;
            var sevisId = "sevis id";
            var sevisOrgId = "abcde12347890";
            var isValidated = true;

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
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
            var sevisId = "sevis id";
            var sevisUserId = "sevisUserId";
            var person = GetPerson(DateTime.UtcNow);
            var sevisOrgId = "abcde12347890";
            var isValidated = true;
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor(sevisUserId);
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
            Assert.AreEqual(sevisUserId, instance.userID);
            Assert.AreEqual(new RequestId(exchangeVisitor.Person.ParticipantId, RequestIdType.Participant, RequestActionType.Create).ToString(), instance.requestID);
            Assert.IsNotNull(instance.UserDefinedA);
            Assert.IsNotNull(instance.UserDefinedB);

            var key = new ParticipantSevisKey(instance.UserDefinedA, instance.UserDefinedB);
            Assert.AreEqual(person.ParticipantId, key.ParticipantId);
            Assert.AreEqual(person.PersonId, key.PersonId);
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_OccupationCategoryCodeIsNull()
        {
            var sevisId = "sevis id";
            var sevisOrgId = "abcde12347890";
            var sevisUserId = "sevisUserId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            string occupationCategoryCode = null;
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor(sevisUserId);
            Assert.IsFalse(instance.OccupationCategoryCodeSpecified);
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_MailAddressIsNull()
        {
            var sevisId = "sevis id";
            var sevisOrgId = "abcde12347890";
            var sevisUserId = "sevisUserId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow, false, true);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor(sevisUserId);
            Assert.IsNotNull(instance);
            Assert.IsNull(instance.MailAddress);
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_USAddressIsNull()
        {
            var sevisId = "sevis id";
            var sevisUserId = "sevisUserId";
            var sevisOrgId = "abcde12347890";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow, true, false);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor(sevisUserId);
            Assert.IsNotNull(instance);
            Assert.IsNull(instance.USAddress);
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_CheckItems()
        {
            var sevisId = "sevis id";
            var sevisUserId = "sevisUserId";
            var sevisOrgId = "abcde12347890";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor(sevisUserId);
            Assert.IsNotNull(instance);
            Assert.IsNotNull(instance.Items);
            Assert.AreEqual(1, instance.Items.Count());
            var siteOfActivityObject = instance.Items.First();
            Assert.IsInstanceOfType(siteOfActivityObject, typeof(AddSiteOfActivity));
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_CheckDependents_DependentIsNotDeleted()
        {
            var sevisId = "sevis id";
            var sevisUserId = "sevisUserId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
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
            var birthCountryReasonCode = USBornReasonType.Item01.ToString();
            var relationship = DependentCodeType.Item01.ToString();
            var isTravelingWithParticipant = true;
            var isDeleted = false;
            var sevisOrgId = "abcde12347890";

            var addedDependent = new AddedDependent(
                fullName,
                birthCity,
                birthCountryCode,
                birthCountryReasonCode,
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
                participantId,
                isTravelingWithParticipant,
                isDeleted: isDeleted
                );
            dependents.Add(addedDependent);

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor(sevisUserId);
            Assert.IsNotNull(instance);
            Assert.AreEqual(1, instance.CreateDependent.Count());
            var firstDependent = instance.CreateDependent.First();
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_CheckDependents_DependentIsDeleted()
        {
            var sevisId = "sevis id";
            var sevisUserId = "sevisUserId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
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
            var birthCountryReasonCode = USBornReasonType.Item01.ToString();
            var relationship = DependentCodeType.Item01.ToString();
            var isTravelingWithParticipant = true;
            var isDeleted = true;
            var sevisOrgId = "abcde12347890";

            var addedDependent = new AddedDependent(
                fullName,
                birthCity,
                birthCountryCode,
                birthCountryReasonCode,
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
                participantId,
                isTravelingWithParticipant,
                isDeleted: isDeleted
                );
            dependents.Add(addedDependent);

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor(sevisUserId);
            Assert.IsNotNull(instance);
            Assert.IsNull(instance.CreateDependent);
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_CheckDependents_NoDependents()
        {
            var sevisId = "sevis id";
            var sevisUserId = "sevisUserId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            var sevisOrgId = "abcde12347890";

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor(sevisUserId);
            Assert.IsNotNull(instance);
            Assert.IsNull(instance.CreateDependent);
        }

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_CheckDependents_CheckMustBeAddedDepenent()
        {
            var sevisId = "sevis id";
            var sevisUserId = "sevisUserId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
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
            var birthCountryReasonCode = USBornReasonType.Item01.ToString();
            var dependentSevisId = "sevis id";
            var remarks = "remarks";
            var relationship = DependentCodeType.Item01.ToString();
            var isTravelingWithParticipant = true;
            var isDeleted = false;
            var sevisOrgId = "abcde12347890";

            var updatedDependent = new UpdatedDependent(
                fullName,
                birthCity,
                birthCountryCode,
                birthCountryReasonCode,
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
                participantId,
                isTravelingWithParticipant,
                isDeleted
                );
            dependents.Add(updatedDependent);
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            Action a = () => exchangeVisitor.GetSEVISBatchTypeExchangeVisitor(sevisUserId);
            a.ShouldThrow<NotSupportedException>().WithMessage("The dependent must be an added dependent.");
        }

        #endregion

        #region Site of Activity
        [TestMethod]
        public void TestGetSOA()
        {
            var sevisId = "sevis id";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            var sevisOrgId = "abcde12347890";

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
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
            Assert.AreEqual(siteOfActivity.DivisionIso.GetStateCodeType(), instance.State);
            Assert.AreEqual(siteOfActivity.LocationName, instance.SiteName);
            Assert.IsTrue(instance.StateSpecified);
            Assert.IsFalse(instance.ExplanationCodeSpecified);
            Assert.IsNull(instance.ExplanationCode);
            Assert.IsNull(instance.Explanation);
        }

        [TestMethod]
        public void TestGetAddSiteOfActivity()
        {
            var sevisId = "sevis id";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            var sevisOrgId = "abcde12347890";

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
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
        public void TestGetSEVISEVBatchTypeExchangeVisitor1Collection_SameInstance()
        {
            var sevisUserId = "sevisUserId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            List<Dependent> dependents = new List<Dependent>();
            var testDependent = new TestDependent();
            dependents.Add(testDependent);
            var sevisId = "sevis id";
            var sevisOrgId = "sevisOrgId";

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            Assert.AreEqual(0, exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUserId, exchangeVisitor).Count());
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitor1Collection_PersonHasChanges()
        {
            var sevisUserId = "sevisUserId";
            var birthDate = DateTime.UtcNow;
            var person = GetPerson(birthDate);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            List<Dependent> dependents = new List<Dependent>();
            var testDependent = new TestDependent();
            dependents.Add(testDependent);
            var sevisId = "sevis id";
            var sevisOrgId = "abcde12347890";
            var isValidated = true;

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var otherPerson = GetPerson(birthDate);
            var previousExchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: otherPerson,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity
                );

            Action<SEVISEVBatchTypeExchangeVisitor1> propertyTester = (visitor) =>
            {
                Assert.IsNotNull(visitor.requestID);
                Assert.AreEqual(sevisId, visitor.sevisID);
                Assert.AreEqual(sevisUserId, visitor.userID);
                Assert.IsFalse(visitor.statusCodeSpecified);
            };
            var list = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUserId, previousExchangeVisitor).ToList();
            Assert.AreEqual(0, list.Count);

            var property = typeof(ECA.Business.Validation.Sevis.Bio.Person).GetProperty(nameof(ECA.Business.Validation.Sevis.Bio.Person.BirthCity));
            property.SetValue(otherPerson, "hello world");

            list = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUserId, previousExchangeVisitor).ToList();
            Assert.AreEqual(1, list.Count);
            list.ForEach(x => propertyTester(x));

            var personVisitorItem = CreateGetItemQuery<SEVISEVBatchTypeExchangeVisitorBiographical>(list).FirstOrDefault();
            Assert.IsNotNull(personVisitorItem);
            Assert.AreEqual(new RequestId(person.ParticipantId, RequestIdType.Participant, RequestActionType.Update).ToString(), personVisitorItem.requestID);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitor1Collection_FinancialInfoHasChanges()
        {
            var sevisUserId = "sevisUserId";
            var isValidated = true;
            var birthDate = DateTime.UtcNow;
            var person = GetPerson(birthDate);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            List<Dependent> dependents = new List<Dependent>();
            var testDependent = new TestDependent();
            dependents.Add(testDependent);
            var sevisId = "sevis id";
            var sevisOrgId = "sevisOrgId";

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var otherFinancialInfo = GetFinancialInfo();
            var previousExchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: otherFinancialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity
                );

            Action<SEVISEVBatchTypeExchangeVisitor1> propertyTester = (visitor) =>
            {
                Assert.IsNotNull(visitor.requestID);
                Assert.AreEqual(sevisId, visitor.sevisID);
                Assert.AreEqual(sevisUserId, visitor.userID);
                Assert.IsFalse(visitor.statusCodeSpecified);
            };
            var list = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUserId, previousExchangeVisitor).ToList();
            Assert.AreEqual(0, list.Count);

            var property = typeof(FinancialInfo).GetProperty(nameof(FinancialInfo.ReceivedUSGovtFunds));
            property.SetValue(otherFinancialInfo, !financialInfo.ReceivedUSGovtFunds);

            list = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUserId, previousExchangeVisitor).ToList();
            Assert.AreEqual(1, list.Count);
            list.ForEach(x => propertyTester(x));

            var updatedItem = CreateGetItemQuery<SEVISEVBatchTypeExchangeVisitorFinancialInfo>(list).FirstOrDefault();
            Assert.IsNotNull(updatedItem);
            Assert.AreEqual(new RequestId(person.ParticipantId, RequestIdType.FinancialInfo, RequestActionType.Update).ToString(), updatedItem.requestID);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitor1Collection_SubjectFieldHasChanges()
        {
            var sevisUserId = "sevisUserId";
            var birthDate = DateTime.UtcNow;
            var person = GetPerson(birthDate);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            List<Dependent> dependents = new List<Dependent>();
            var testDependent = new TestDependent();
            dependents.Add(testDependent);
            var sevisId = "sevis id";
            var sevisOrgId = "sevisOrgId";
            var isValidated = true;

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var otherPerson = GetPerson(birthDate);
            var previousExchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: otherPerson,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity
                );

            Action<SEVISEVBatchTypeExchangeVisitor1> propertyTester = (visitor) =>
            {
                Assert.IsNotNull(visitor.requestID);
                Assert.AreEqual(sevisId, visitor.sevisID);
                Assert.AreEqual(sevisUserId, visitor.userID);
                Assert.IsFalse(visitor.statusCodeSpecified);
            };
            var list = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUserId, previousExchangeVisitor).ToList();
            Assert.AreEqual(0, list.Count);

            var property = typeof(SubjectField).GetProperty(nameof(SubjectField.ForeignFieldOfStudy));
            property.SetValue(otherPerson.SubjectField, "hello world");

            list = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUserId, previousExchangeVisitor).ToList();
            Assert.AreEqual(1, list.Count);
            list.ForEach(x => propertyTester(x));
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
            Assert.AreEqual(new RequestId(person.ParticipantId, RequestIdType.SubjectField, RequestActionType.Update).ToString(), exchangeVisitorPrograms.First().requestID);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitor1Collection_DependentHasBeenAdded()
        {
            var isValidated = true;
            var sevisUserId = "sevisUserId";
            var birthDate = DateTime.UtcNow;
            var person = GetPerson(birthDate);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            List<Dependent> dependents = new List<Dependent>();
            var testDependent = new TestDependent();
            dependents.Add(testDependent);
            var sevisId = "sevis id";
            var sevisOrgId = "sevisOrgId";

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);


            var previousExchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: new List<Dependent>(),
                siteOfActivity: siteOfActivity
                );

            Action<SEVISEVBatchTypeExchangeVisitor1> propertyTester = (visitor) =>
            {
                Assert.IsNotNull(visitor.requestID);
                Assert.AreEqual(sevisId, visitor.sevisID);
                Assert.AreEqual(sevisUserId, visitor.userID);
                Assert.IsFalse(visitor.statusCodeSpecified);
            };

            var list = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUserId, previousExchangeVisitor).ToList();
            Assert.AreEqual(1, list.Count);
            list.ForEach(x => propertyTester(x));

            var dependentVisitorItem = CreateGetItemQuery<SEVISEVBatchTypeExchangeVisitorDependent>(list).FirstOrDefault();
            Assert.IsNotNull(dependentVisitorItem);
            Assert.IsNotNull(dependentVisitorItem.Item);
            Assert.IsNull(dependentVisitorItem.UserDefinedA);
            Assert.IsNull(dependentVisitorItem.UserDefinedB);
            Assert.AreEqual(testDependent.GetRequestId().ToString(), dependentVisitorItem.requestID);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitor1Collection_DependentHasBeenModified()
        {
            var sevisUserId = "sevisUserId";
            var birthDate = DateTime.UtcNow;
            var person = GetPerson(birthDate);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var testDependent = new TestDependent();
            var previousTestDependent = new TestDependent();
            var sevisId = "sevis id";
            var sevisOrgId = "sevisOrgId";
            var isValidated = true;

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: new List<Dependent> { testDependent },
                siteOfActivity: siteOfActivity);


            var previousExchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: new List<Dependent> { previousTestDependent },
                siteOfActivity: siteOfActivity
                );

            Action<SEVISEVBatchTypeExchangeVisitor1> propertyTester = (visitor) =>
            {
                Assert.IsNotNull(visitor.requestID);
                Assert.AreEqual(sevisId, visitor.sevisID);
                Assert.AreEqual(sevisUserId, visitor.userID);
                Assert.IsFalse(visitor.statusCodeSpecified);
            };

            var list = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUserId, previousExchangeVisitor).ToList();
            Assert.AreEqual(0, list.Count);

            var property = typeof(Dependent).GetProperty(nameof(Dependent.BirthCity));
            property.SetValue(testDependent, "hello world");

            list = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUserId, previousExchangeVisitor).ToList();
            Assert.AreEqual(1, list.Count);
            list.ForEach(x => propertyTester(x));

            var dependentVisitorItem = CreateGetItemQuery<SEVISEVBatchTypeExchangeVisitorDependent>(list).FirstOrDefault();
            Assert.IsNotNull(dependentVisitorItem);
            Assert.IsNotNull(dependentVisitorItem.Item);
            Assert.IsNull(dependentVisitorItem.UserDefinedA);
            Assert.IsNull(dependentVisitorItem.UserDefinedB);
            Assert.AreEqual(testDependent.GetRequestId().ToString(), dependentVisitorItem.requestID);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitor1Collection_NoChanges()
        {
            var sevisUserId = "sevisUserId";
            var birthDate = DateTime.UtcNow;
            var person = GetPerson(birthDate);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var testDependent = new TestDependent();
            var previousTestDependent = new TestDependent();
            var sevisId = "sevis id";
            var sevisOrgId = "sevisOrgId";
            var isValidated = true;

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: new List<Dependent> { testDependent },
                siteOfActivity: siteOfActivity);


            var previousExchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: new List<Dependent> { previousTestDependent },
                siteOfActivity: siteOfActivity
                );

            Action<SEVISEVBatchTypeExchangeVisitor1> propertyTester = (visitor) =>
            {
                Assert.IsNotNull(visitor.requestID);
                Assert.AreEqual(sevisId, visitor.sevisID);
                Assert.AreEqual(sevisUserId, visitor.userID);
                Assert.IsFalse(visitor.statusCodeSpecified);
            };

            var list = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUserId, previousExchangeVisitor).ToList();
            Assert.AreEqual(0, list.Count);
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
            var sevisId = "sevis id";
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            var sevisOrgId = "abcde12347890";
            var isValidated = true;

            var validator = new Mock<AbstractValidator<ExchangeVisitor>>();
            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
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

        #region GetSEVISEVBatchTypeExchangeVisitorValidate
        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorValidate()
        {
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            List<Dependent> dependents = null;
            var sevisId = "sevis id";
            var username = "username";
            var isValidated = true;
            var sevisOrgId = "abcde12347890";

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitorValidate(username);
            Assert.IsNotNull(instance);
            Assert.AreEqual(sevisId, instance.sevisID);
            Assert.AreEqual(username, instance.userID);
            Assert.AreEqual(new RequestId(person.ParticipantId, RequestIdType.Validate, RequestActionType.Update).ToString(), instance.requestID);
            Assert.IsFalse(instance.statusCodeSpecified);

            Assert.IsNotNull(instance.Item);
            Assert.IsInstanceOfType(instance.Item, typeof(SEVISEVBatchTypeExchangeVisitorValidate));
            var item = (SEVISEVBatchTypeExchangeVisitorValidate)instance.Item;
            Assert.AreEqual(person.EmailAddress, item.EmailAddress);
            Assert.AreEqual(person.GetUSPhoneNumber(person.PhoneNumber), item.PhoneNumber);
            Assert.AreEqual(person.MailAddress.Street1, item.USAddress.Address1);
            Assert.AreEqual(person.MailAddress.Street2, item.USAddress.Address2);
            Assert.AreEqual(person.MailAddress.City, item.USAddress.City);
            Assert.AreEqual(person.MailAddress.PostalCode, item.USAddress.PostalCode);
            Assert.AreEqual(person.MailAddress.DivisionIso.GetStateCodeType(), item.USAddress.State);
            Assert.IsTrue(item.USAddress.StateSpecified);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorValidate_MailAddressIsNull()
        {
            var person = GetPerson(DateTime.UtcNow, false, false);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            List<Dependent> dependents = null;
            var sevisId = "sevis id";
            var username = "username";
            var isValidated = true;
            var sevisOrgId = "abcde12347890";

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitorValidate(username);
            Assert.IsNotNull(instance);
            Assert.AreEqual(sevisId, instance.sevisID);
            Assert.AreEqual(username, instance.userID);
            Assert.AreEqual(new RequestId(person.ParticipantId, RequestIdType.Validate, RequestActionType.Update).ToString(), instance.requestID);
            Assert.IsFalse(instance.statusCodeSpecified);

            Assert.IsNotNull(instance.Item);
            Assert.IsInstanceOfType(instance.Item, typeof(SEVISEVBatchTypeExchangeVisitorValidate));
            var item = (SEVISEVBatchTypeExchangeVisitorValidate)instance.Item;
            Assert.IsNull(item.USAddress);
        }
        #endregion

        #region Compare
        [TestMethod]
        public void TestGetChangeDetail_SameInstance()
        {
            var sevisId = "sevis id";
            var sevisOrgId = "sevisOrgId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var detail = exchangeVisitor.GetChangeDetail(exchangeVisitor);
            Assert.IsFalse(detail.HasChanges());
        }

        [TestMethod]
        public void TestCompare_ShouldIgnoreSevisId()
        {
            var sevisId = "sevis id";
            var sevisOrgId = "sevisOrgId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var otherExchangeVisitor = new ExchangeVisitor(
                sevisId: "other sevis id",
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);
            var detail = exchangeVisitor.GetChangeDetail(otherExchangeVisitor);
            Assert.IsFalse(detail.HasChanges());
        }

        [TestMethod]
        public void TestCompare_ShouldIgnoreIsValidated()
        {
            var sevisId = "sevis id";
            var sevisOrgId = "sevisOrgId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var otherExchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: !isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);
            var detail = exchangeVisitor.GetChangeDetail(otherExchangeVisitor);
            Assert.IsFalse(detail.HasChanges());
        }

        [TestMethod]
        public void TestCompare_HasChanges()
        {
            var sevisId = "sevis id";
            var sevisOrgId = "sevisOrgId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var otherExchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated:isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate.AddDays(1.0),
                dependents: dependents,
                siteOfActivity: siteOfActivity);
            var detail = exchangeVisitor.GetChangeDetail(otherExchangeVisitor);
            Assert.IsTrue(detail.HasChanges());
        }
        #endregion

        [TestMethod]
        public void TestGetSEVISBatchTypeExchangeVisitor_CheckSerialization()
        {
            var sevisId = "sevis id";
            var sevisUserId = "sevisUserId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            var sevisOrgId = "abcde12347890";

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISBatchTypeExchangeVisitor(sevisUserId);
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
            var sevisId = "sevis id";
            var sevisUserId = "sevisUserId";
            var isValidated = true;
            var person = GetPerson(DateTime.UtcNow);
            var financialInfo = GetFinancialInfo();
            var occupationCategoryCode = "99";
            var endDate = DateTime.UtcNow.AddDays(1.0);
            var startDate = DateTime.UtcNow.AddDays(-1.0);
            var siteOfActivity = GetSOAAsAddressDTO();
            var dependents = new List<Dependent>();
            var sevisOrgId = "abcde12347890";

            var exchangeVisitor = new ExchangeVisitor(
                sevisId: sevisId,
                sevisOrgId: sevisOrgId,
                isValidated: isValidated,
                person: person,
                financialInfo: financialInfo,
                occupationCategoryCode: occupationCategoryCode,
                programEndDate: endDate,
                programStartDate: startDate,
                dependents: dependents,
                siteOfActivity: siteOfActivity);

            var instance = exchangeVisitor.GetSEVISEVBatchTypeExchangeVisitor1Collection(sevisUserId, exchangeVisitor);
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
