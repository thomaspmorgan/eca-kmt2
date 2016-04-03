using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using ECA.Business.Sevis.Model;
using ECA.Business.Validation;
using ECA.Business.Service.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Validation.Sevis;
using PhoneNumbers;
using Newtonsoft.Json;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class PersonTest
    {
        [TestMethod]
        public void TestConstructor()
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
            var birthCountryReasonId = 1;
            var remarks = "remarks";
            var programCataegoryCode = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);

            var person = new Business.Validation.Sevis.Bio.Person(
                fullName,
                birthCity,
                birthCountryCode,
                birthCountryReasonId,
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
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            Assert.AreEqual(birthCity, person.BirthCity);
            Assert.AreEqual(birthCountryCode, person.BirthCountryCode);
            Assert.AreEqual(birthCountryReasonId, person.BirthCountryReasonId);
            Assert.AreEqual(birthDate, person.BirthDate);
            Assert.AreEqual(citizenshipCountryCode, person.CitizenshipCountryCode);
            Assert.AreEqual(email, person.EmailAddress);
            Assert.AreEqual(gender, person.Gender);
            Assert.AreEqual(permanentResidenceCountryCode, person.PermanentResidenceCountryCode);
            Assert.AreEqual(phone, person.PhoneNumber);
            Assert.AreEqual(remarks, person.Remarks);
            Assert.AreEqual(positionCode, (short)Int32.Parse(person.PositionCode));
            Assert.AreEqual(programCataegoryCode, person.ProgramCategoryCode);
            Assert.AreEqual(printForm, person.PrintForm);
            Assert.AreEqual(personId, person.PersonId);
            Assert.AreEqual(participantId, person.ParticipantId);

            Assert.IsTrue(Object.ReferenceEquals(subjectField, person.SubjectField));
            Assert.IsTrue(Object.ReferenceEquals(fullName, person.FullName));
            Assert.IsTrue(Object.ReferenceEquals(mailAddress, person.MailAddress));
            Assert.IsTrue(Object.ReferenceEquals(usAddress, person.USAddress));
        }

        [TestMethod]
        public void TestJsonSerialization()
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
            var birthCountryReasonId = 1;
            var remarks = "remarks";
            var programCataegoryCode = "1D";

            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);

            var person = new Business.Validation.Sevis.Bio.Person(
                fullName,
                birthCity,
                birthCountryCode,
                birthCountryReasonId,
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
                mailAddress,
                usAddress,
                printForm,
                personId,
                participantId);
            var json = JsonConvert.SerializeObject(person);
            var jsonObject = JsonConvert.DeserializeObject<Business.Validation.Sevis.Bio.Person>(json);
            Assert.IsNotNull(jsonObject.FullName);
            Assert.IsNotNull(jsonObject.MailAddress);
            Assert.IsNotNull(jsonObject.USAddress);
            Assert.IsNotNull(jsonObject.SubjectField);
            Assert.AreEqual(birthCity, jsonObject.BirthCity);
            Assert.AreEqual(birthCountryCode, jsonObject.BirthCountryCode);
            Assert.AreEqual(birthCountryReasonId, jsonObject.BirthCountryReasonId);
            Assert.AreEqual(citizenshipCountryCode, jsonObject.CitizenshipCountryCode);
            Assert.AreEqual(email, jsonObject.EmailAddress);
            Assert.AreEqual(gender, jsonObject.Gender);
            Assert.AreEqual(phone, jsonObject.PhoneNumber);
            Assert.AreEqual(remarks, jsonObject.Remarks);
            Assert.AreEqual(positionCode.ToString(), jsonObject.PositionCode);
            Assert.AreEqual(programCataegoryCode, jsonObject.ProgramCategoryCode);
            Assert.AreEqual(printForm, jsonObject.PrintForm);
            Assert.AreEqual(personId, jsonObject.PersonId);
            Assert.AreEqual(participantId, jsonObject.ParticipantId);
        }

        [TestMethod]
        public void TestGetEVPersonTypeBiographical()
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
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReasonId = 1;
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);
            
            var person = new Business.Validation.Sevis.Bio.Person
                (birthCity: birthCity,
                fullName: fullName,
                birthCountryCode: birthCountryCode,
                birthCountryReasonId: birthCountryReasonId,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                remarks: remarks,
                positionCode: positionCode.ToString(),

                programCategoryCode: programCataegoryCode,
                subjectField: subjectField,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                personId: personId,
                participantId: participantId
                );

            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var pn = phoneNumberUtil.Parse(phone, ECA.Data.PhoneNumber.US_PHONE_NUMBER_REGION_KEY);
            var expectedPhone = pn.NationalNumber.ToString();
            Assert.AreEqual("8505551212", expectedPhone);

            var instance = person.GetEVPersonTypeBiographical();
            Assert.AreEqual(person.BirthCity, instance.BirthCity);
            Assert.AreEqual(person.BirthCountryCode.GetBirthCntryCodeType(), instance.BirthCountryCode);
            Assert.AreEqual(person.BirthDate, instance.BirthDate);
            Assert.AreEqual(person.CitizenshipCountryCode.GetCountryCodeWithType(), instance.CitizenshipCountryCode);
            Assert.AreEqual(person.EmailAddress, instance.EmailAddress);
            Assert.AreEqual(person.Gender.GetEVGenderCodeType(), instance.Gender);
            Assert.AreEqual(person.PermanentResidenceCountryCode.GetCountryCodeWithType(), instance.PermanentResidenceCountryCode);
            Assert.AreEqual(expectedPhone, instance.PhoneNumber);
            Assert.IsFalse(instance.BirthCountryReasonSpecified);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorBiographical()
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
            var phone = "18505551212";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReasonId = 1;
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);


            var person = new Business.Validation.Sevis.Bio.Person
                (birthCity: birthCity,
                fullName: fullName,
                birthCountryCode: birthCountryCode,
                birthCountryReasonId: birthCountryReasonId,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                remarks: remarks,
                positionCode: positionCode.ToString(),
                programCategoryCode: programCataegoryCode,
                subjectField: subjectField,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                personId: personId,
                participantId: participantId
                );

            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            var pn = phoneNumberUtil.Parse(phone, ECA.Data.PhoneNumber.US_PHONE_NUMBER_REGION_KEY);
            var expectedPhone = pn.NationalNumber.ToString();
            Assert.AreEqual("8505551212", expectedPhone);

            var instance = person.GetSEVISEVBatchTypeExchangeVisitorBiographical();
            Assert.AreEqual(person.BirthCity, instance.BirthCity);
            Assert.AreEqual(person.BirthCountryCode.GetBirthCntryCodeType(), instance.BirthCountryCode);
            Assert.AreEqual(person.BirthDate, instance.BirthDate);
            Assert.AreEqual(person.CitizenshipCountryCode.GetCountryCodeWithType(), instance.CitizenshipCountryCode);
            Assert.AreEqual(person.EmailAddress, instance.EmailAddress);
            Assert.AreEqual(person.Gender.GetGenderCodeType(), instance.Gender);
            Assert.AreEqual(person.PermanentResidenceCountryCode.GetCountryCodeWithType(), instance.PermanentResidenceCountryCode);
            Assert.AreEqual(expectedPhone, instance.PhoneNumber);
            Assert.AreEqual(person.Remarks, instance.Remarks);
            Assert.AreEqual(person.PrintForm, instance.printForm);
            Assert.AreEqual((short)Int32.Parse(person.PositionCode), instance.PositionCode);
            Assert.IsNotNull(instance.MailAddress);
            Assert.IsNotNull(instance.USAddress);

            Assert.IsTrue(instance.PositionCodeSpecified);
            Assert.IsTrue(instance.BirthCountryCodeSpecified);
            Assert.IsTrue(instance.BirthDateSpecified);
            Assert.IsTrue(instance.CitizenshipCountryCodeSpecified);
            Assert.IsTrue(instance.GenderSpecified);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorBiographical_PositionCodeNotSpecified()
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
            string positionCode = null;
            var printForm = true;
            var birthCountryReasonId = 1;
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);


            var person = new Business.Validation.Sevis.Bio.Person
                (birthCity: birthCity,
                fullName: fullName,
                birthCountryCode: birthCountryCode,
                birthCountryReasonId: birthCountryReasonId,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                remarks: remarks,
                positionCode: positionCode,
                programCategoryCode: programCataegoryCode,
                subjectField: subjectField,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                personId: personId,
                participantId: participantId
                );

            var instance = person.GetSEVISEVBatchTypeExchangeVisitorBiographical();
            Assert.IsFalse(instance.PositionCodeSpecified);
            Assert.IsTrue(instance.BirthCountryCodeSpecified);
            Assert.IsTrue(instance.BirthDateSpecified);
            Assert.IsTrue(instance.CitizenshipCountryCodeSpecified);
            Assert.IsTrue(instance.GenderSpecified);
            Assert.AreEqual(default(short), instance.PositionCode);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorBiographical_BirthCountryCodeNotSpecified()
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
            string birthCountryCode = null;
            var birthDate = DateTime.UtcNow;
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "123-456-7890";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReasonId = 1;
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);


            var person = new Business.Validation.Sevis.Bio.Person
                (birthCity: birthCity,
                fullName: fullName,
                birthCountryCode: birthCountryCode,
                birthCountryReasonId: birthCountryReasonId,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                remarks: remarks,
                positionCode: positionCode.ToString(),
                programCategoryCode: programCataegoryCode,
                subjectField: subjectField,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                personId: personId,
                participantId: participantId
                );

            var instance = person.GetSEVISEVBatchTypeExchangeVisitorBiographical();
            Assert.IsTrue(instance.PositionCodeSpecified);
            Assert.IsFalse(instance.BirthCountryCodeSpecified);
            Assert.IsTrue(instance.BirthDateSpecified);
            Assert.IsTrue(instance.CitizenshipCountryCodeSpecified);
            Assert.IsTrue(instance.GenderSpecified);
            Assert.AreEqual(default(BirthCntryCodeType), instance.BirthCountryCode);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorBiographical_BirthDateNotSpecified()
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
            var citizenshipCountryCode = "FR";
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "123-456-7890";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReasonId = 1;
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);


            var person = new Business.Validation.Sevis.Bio.Person
                (birthCity: birthCity,
                fullName: fullName,
                birthCountryCode: birthCountryCode,
                birthCountryReasonId: birthCountryReasonId,
                birthDate: default(DateTime?),
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                remarks: remarks,
                positionCode: positionCode.ToString(),
                programCategoryCode: programCataegoryCode,
                subjectField: subjectField,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                personId: personId,
                participantId: participantId
                );

            var instance = person.GetSEVISEVBatchTypeExchangeVisitorBiographical();
            Assert.IsTrue(instance.PositionCodeSpecified);
            Assert.IsTrue(instance.BirthCountryCodeSpecified);
            Assert.IsFalse(instance.BirthDateSpecified);
            Assert.IsTrue(instance.CitizenshipCountryCodeSpecified);
            Assert.IsTrue(instance.GenderSpecified);
            Assert.AreEqual(default(DateTime), instance.BirthDate);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorBiographical_CitizenshipCountryCodeNotSpecified()
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
            string citizenshipCountryCode = null;
            var email = "someone@isp.com";
            var gender = Gender.SEVIS_MALE_GENDER_CODE_VALUE;
            var permanentResidenceCountryCode = "MX";
            var phone = "123-456-7890";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReasonId = 1;
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);


            var person = new Business.Validation.Sevis.Bio.Person
                (birthCity: birthCity,
                fullName: fullName,
                birthCountryCode: birthCountryCode,
                birthCountryReasonId: birthCountryReasonId,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                remarks: remarks,
                positionCode: positionCode.ToString(),
                programCategoryCode: programCataegoryCode,
                subjectField: subjectField,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                personId: personId,
                participantId: participantId
                );

            var instance = person.GetSEVISEVBatchTypeExchangeVisitorBiographical();
            Assert.IsTrue(instance.PositionCodeSpecified);
            Assert.IsTrue(instance.BirthCountryCodeSpecified);
            Assert.IsTrue(instance.BirthDateSpecified);
            Assert.IsFalse(instance.CitizenshipCountryCodeSpecified);
            Assert.IsTrue(instance.GenderSpecified);
            Assert.AreEqual(default(CntryCodeWithoutType), instance.CitizenshipCountryCode);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorBiographical_GenderNotSpecified()
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
            string gender = null;
            var permanentResidenceCountryCode = "MX";
            var phone = "123-456-7890";
            short positionCode = 120;
            var printForm = true;
            var birthCountryReasonId = 1;
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);


            var person = new Business.Validation.Sevis.Bio.Person
                (birthCity: birthCity,
                fullName: fullName,
                birthCountryCode: birthCountryCode,
                birthCountryReasonId: birthCountryReasonId,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                remarks: remarks,
                positionCode: positionCode.ToString(),
                programCategoryCode: programCataegoryCode,
                subjectField: subjectField,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                personId: personId,
                participantId: participantId
                );

            var instance = person.GetSEVISEVBatchTypeExchangeVisitorBiographical();
            Assert.IsTrue(instance.PositionCodeSpecified);
            Assert.IsTrue(instance.BirthCountryCodeSpecified);
            Assert.IsTrue(instance.BirthDateSpecified);
            Assert.IsTrue(instance.CitizenshipCountryCodeSpecified);
            Assert.IsFalse(instance.GenderSpecified);
            Assert.AreEqual(default(GenderCodeType), instance.Gender);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorBiographical_MailAddressIsNull()
        {
            var state = "TN";
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
            var birthCountryReasonId = 1;
            var remarks = "remarks";
            var programCataegoryCode = "1D";
            var subjectFieldCode = "01.0102";
            var subjectField = new SubjectField(subjectFieldCode, null, null, null);

            var person = new Business.Validation.Sevis.Bio.Person
                (birthCity: birthCity,
                fullName: fullName,
                birthCountryCode: birthCountryCode,
                birthCountryReasonId: birthCountryReasonId,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                remarks: remarks,
                positionCode: positionCode.ToString(),
                programCategoryCode: programCataegoryCode,
                subjectField: subjectField,
                mailAddress: null,
                usAddress: usAddress,
                printForm: printForm,
                personId: personId,
                participantId: participantId
                );

            var instance = person.GetSEVISEVBatchTypeExchangeVisitorBiographical();
            Assert.IsNull(instance.MailAddress);
        }
    }
}
