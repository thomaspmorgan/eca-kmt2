using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using ECA.Business.Validation;
using ECA.Business.Sevis.Model;
using ECA.Business.Service.Admin;
using ECA.Business.Queries.Models.Admin;
using Newtonsoft.Json;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class UpdatedDependentTest
    {
        [TestMethod]
        public void TestConstructor()
        {
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
            var sevisId = "sevis id";
            var remarks = "remarks";
            var relationship = DependentCodeType.Item01.ToString();

            var instance = new UpdatedDependent(
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
                sevisId,
                remarks,
                personId,
                participantId
                );
            Assert.AreEqual(personId, instance.GetPersonId());
            Assert.AreEqual(participantId, instance.GetParticipantId());
            Assert.AreEqual(birthCity, instance.BirthCity);
            Assert.AreEqual(birthCountryCode, instance.BirthCountryCode);
            Assert.AreEqual(birthDate, instance.BirthDate);
            Assert.AreEqual(citizenshipCountryCode, instance.CitizenshipCountryCode);
            Assert.AreEqual(email, instance.EmailAddress);
            Assert.AreEqual(gender, instance.Gender);
            Assert.AreEqual(permanentResidenceCountryCode, instance.PermanentResidenceCountryCode);
            Assert.AreEqual(phone, instance.PhoneNumber);
            Assert.AreEqual(printForm, instance.PrintForm);
            Assert.AreEqual(birthCountryReason, instance.BirthCountryReason);
            Assert.AreEqual(sevisId, instance.SevisId);
            Assert.AreEqual(remarks, instance.Remarks);
            Assert.AreEqual(relationship, instance.Relationship);
            Assert.IsTrue(Object.ReferenceEquals(fullName, instance.FullName));
            Assert.IsTrue(Object.ReferenceEquals(mailAddress, instance.MailAddress));
            Assert.IsTrue(Object.ReferenceEquals(usAddress, instance.USAddress));

            var json = JsonConvert.SerializeObject(instance);
            var jsonObject = JsonConvert.DeserializeObject<UpdatedDependent>(json);
            Assert.AreEqual(personId, jsonObject.GetPersonId());
            Assert.AreEqual(participantId, jsonObject.GetParticipantId());
            Assert.AreEqual(birthCity, jsonObject.BirthCity);
            Assert.AreEqual(birthCountryCode, jsonObject.BirthCountryCode);
            Assert.AreEqual(birthDate, jsonObject.BirthDate);
            Assert.AreEqual(citizenshipCountryCode, jsonObject.CitizenshipCountryCode);
            Assert.AreEqual(email, jsonObject.EmailAddress);
            Assert.AreEqual(gender, jsonObject.Gender);
            Assert.AreEqual(permanentResidenceCountryCode, jsonObject.PermanentResidenceCountryCode);
            Assert.AreEqual(phone, jsonObject.PhoneNumber);
            Assert.AreEqual(printForm, jsonObject.PrintForm);
            Assert.AreEqual(birthCountryReason, jsonObject.BirthCountryReason);
            Assert.AreEqual(relationship, jsonObject.Relationship);
            Assert.AreEqual(sevisId, jsonObject.SevisId);
            Assert.AreEqual(remarks, jsonObject.Remarks);
            Assert.IsNotNull(jsonObject.FullName);
            Assert.IsNotNull(jsonObject.MailAddress);
            Assert.IsNotNull(jsonObject.USAddress);
        }

        [TestMethod]
        public void TestJsonSerialization()
        {
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
            var sevisId = "sevis id";
            var remarks = "remarks";
            var relationship = DependentCodeType.Item01.ToString();

            var instance = new UpdatedDependent(
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
                sevisId,
                remarks,
                personId,
                participantId
                );

            var json = JsonConvert.SerializeObject(instance);
            var jsonObject = JsonConvert.DeserializeObject<UpdatedDependent>(json);
            Assert.AreEqual(personId, jsonObject.GetPersonId());
            Assert.AreEqual(participantId, jsonObject.GetParticipantId());
            Assert.AreEqual(birthCity, jsonObject.BirthCity);
            Assert.AreEqual(birthCountryCode, jsonObject.BirthCountryCode);
            Assert.AreEqual(birthDate, jsonObject.BirthDate);
            Assert.AreEqual(citizenshipCountryCode, jsonObject.CitizenshipCountryCode);
            Assert.AreEqual(email, jsonObject.EmailAddress);
            Assert.AreEqual(gender, jsonObject.Gender);
            Assert.AreEqual(permanentResidenceCountryCode, jsonObject.PermanentResidenceCountryCode);
            Assert.AreEqual(phone, jsonObject.PhoneNumber);
            Assert.AreEqual(printForm, jsonObject.PrintForm);
            Assert.AreEqual(birthCountryReason, jsonObject.BirthCountryReason);
            Assert.AreEqual(relationship, jsonObject.Relationship);
            Assert.AreEqual(sevisId, jsonObject.SevisId);
            Assert.AreEqual(remarks, jsonObject.Remarks);
            Assert.IsNotNull(jsonObject.FullName);
            Assert.IsNotNull(jsonObject.MailAddress);
            Assert.IsNotNull(jsonObject.USAddress);
        }

        [TestMethod]
        public void TestGetSevisExhangeVisitorDependentInstance()
        {
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
            var sevisId = "sevis id";
            var remarks = "remarks";
            var relationship = DependentCodeType.Item01.ToString();

            var dependent = new UpdatedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReason: birthCountryReason,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                relationship: relationship,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                sevisId: sevisId,
                remarks: remarks,
                participantId: participantId,
                personId: personId
                );

            var instance = dependent.GetSevisExhangeVisitorDependentInstance();
            Assert.IsInstanceOfType(instance, typeof(SEVISEVBatchTypeExchangeVisitorDependentEdit));
            var sevisModel = (SEVISEVBatchTypeExchangeVisitorDependentEdit)instance;

            Assert.AreEqual(dependent.BirthCity, sevisModel.BirthCity);
            Assert.AreEqual(dependent.BirthCountryCode.GetBirthCntryCodeType(), sevisModel.BirthCountryCode);
            Assert.AreEqual(dependent.BirthDate, sevisModel.BirthDate);
            Assert.AreEqual(dependent.CitizenshipCountryCode.GetCountryCodeWithType(), sevisModel.CitizenshipCountryCode);
            Assert.AreEqual(dependent.EmailAddress, sevisModel.EmailAddress);
            Assert.AreEqual(dependent.Gender.GetEVGenderCodeType(), sevisModel.Gender);
            Assert.AreEqual(dependent.PermanentResidenceCountryCode.GetCountryCodeWithType(), sevisModel.PermanentResidenceCountryCode);
            Assert.AreEqual(dependent.Relationship.GetDependentCodeType(), sevisModel.Relationship);
            Assert.AreEqual(dependent.PrintForm, sevisModel.printForm);
            Assert.AreEqual(dependent.Remarks, sevisModel.Remarks);
            Assert.AreEqual(dependent.SevisId, sevisModel.dependentSevisID);

            Assert.IsTrue(sevisModel.RelationshipSpecified);            
            Assert.IsTrue(sevisModel.BirthCountryReasonSpecified);
        }

        [TestMethod]
        public void TestGetSevisExhangeVisitorDependentInstance_BirthCountryReasonNotSpecified()
        {
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
            string birthCountryReason = null;
            var sevisId = "sevis id";
            var remarks = "remarks";
            var relationship = DependentCodeType.Item01.ToString();

            var dependent = new UpdatedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReason: birthCountryReason,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                relationship: relationship,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                sevisId: sevisId,
                remarks: remarks,
                participantId: participantId,
                personId: personId
                );

            var instance = dependent.GetSevisExhangeVisitorDependentInstance();
            Assert.IsInstanceOfType(instance, typeof(SEVISEVBatchTypeExchangeVisitorDependentEdit));
            var sevisModel = (SEVISEVBatchTypeExchangeVisitorDependentEdit)instance;
            Assert.IsFalse(sevisModel.BirthCountryReasonSpecified);
        }


        [TestMethod]
        public void TestGetSevisExhangeVisitorDependentInstance_RelationshipNotSpecified()
        {
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
            string birthCountryReason = null;
            var sevisId = "sevis id";
            var remarks = "remarks";
            var relationship = DependentCodeType.Item01.ToString();

            var dependent = new UpdatedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReason: birthCountryReason,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                gender: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                relationship: relationship,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                sevisId: sevisId,
                remarks: remarks,
                participantId: participantId,
                personId: personId
                );

            var instance = dependent.GetSevisExhangeVisitorDependentInstance();
            Assert.IsInstanceOfType(instance, typeof(SEVISEVBatchTypeExchangeVisitorDependentEdit));
            var sevisModel = (SEVISEVBatchTypeExchangeVisitorDependentEdit)instance;
            Assert.IsFalse(sevisModel.BirthCountryReasonSpecified);
        }
    }
}
