using ECA.Business.Sevis.Model;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ECA.Business.Validation;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Validation.Sevis;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class AddedDependentTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var personId = 100;
            var participantId = 200;
            var fullName = new FullName
            {
                FirstName = "first name",
                LastName = "last name",
                PassportName = "passport name",
                PreferredName = "preferred name",
                Suffix = FullNameValidator.SECOND_SUFFIX
            };
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
            var instance = new AddedDependent(
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
            Assert.AreEqual(relationship, instance.Relationship);
            Assert.IsTrue(Object.ReferenceEquals(fullName, instance.FullName));
            Assert.IsTrue(Object.ReferenceEquals(mailAddress, instance.MailAddress));
            Assert.IsTrue(Object.ReferenceEquals(usAddress, instance.USAddress));
        }


        [TestMethod]
        public void TestGetSevisExhangeVisitorDependentInstance()
        {
            var personId = 100;
            var participantId = 200;
            var fullName = new FullName
            {
                FirstName = "first name",
                LastName = "last name",
                PassportName = "passport name",
                PreferredName = "preferred name",
                Suffix = FullNameValidator.SECOND_SUFFIX
            };
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
            var dependent = new AddedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReason: birthCountryReason,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                genderCode: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                relationship: relationship,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                participantId: participantId,
                personId: personId
                );

            var instance = dependent.GetSevisExhangeVisitorDependentInstance();
            Assert.IsInstanceOfType(instance, typeof(SEVISEVBatchTypeExchangeVisitorDependentAdd));
            var sevisModel = (SEVISEVBatchTypeExchangeVisitorDependentAdd)instance;

            Assert.AreEqual(dependent.BirthCity, sevisModel.BirthCity);
            Assert.AreEqual(dependent.BirthCountryCode.GetBirthCntryCodeType(), sevisModel.BirthCountryCode);
            Assert.AreEqual(dependent.BirthDate, sevisModel.BirthDate);
            Assert.AreEqual(dependent.CitizenshipCountryCode.GetCountryCodeWithType(), sevisModel.CitizenshipCountryCode);
            Assert.AreEqual(dependent.EmailAddress, sevisModel.EmailAddress);
            Assert.AreEqual(dependent.Gender.GetEVGenderCodeType(), sevisModel.Gender);
            Assert.AreEqual(dependent.PermanentResidenceCountryCode.GetCountryCodeWithType(), sevisModel.PermanentResidenceCountryCode);
            Assert.AreEqual(dependent.Relationship.GetDependentCodeType(), sevisModel.Relationship);
            Assert.AreEqual(dependent.PrintForm, sevisModel.printForm);
            Assert.AreEqual(EVPrintReasonType.Item06, sevisModel.FormPurpose);
            Assert.IsFalse(sevisModel.BirthCountryReasonSpecified);
        }

        [TestMethod]
        public void TestGetEVPersonTypeDependent()
        {
            var personId = 100;
            var participantId = 200;
            var fullName = new FullName
            {
                FirstName = "first name",
                LastName = "last name",
                PassportName = "passport name",
                PreferredName = "preferred name",
                Suffix = FullNameValidator.SECOND_SUFFIX
            };
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
            var dependent = new AddedDependent(
                fullName: fullName,
                birthCity: birthCity,
                birthCountryCode: birthCountryCode,
                birthCountryReason: birthCountryReason,
                birthDate: birthDate,
                citizenshipCountryCode: citizenshipCountryCode,
                emailAddress: email,
                genderCode: gender,
                permanentResidenceCountryCode: permanentResidenceCountryCode,
                phoneNumber: phone,
                relationship: relationship,
                mailAddress: mailAddress,
                usAddress: usAddress,
                printForm: printForm,
                participantId: participantId,
                personId: personId
                );
            var instance = dependent.GetEVPersonTypeDependent();
            Assert.AreEqual(participantId.ToString(), instance.UserDefinedA);
            Assert.AreEqual(personId.ToString(), instance.UserDefinedB);
            
            Assert.AreEqual(dependent.BirthCity, instance.BirthCity);
            Assert.AreEqual(dependent.BirthCountryCode.GetBirthCntryCodeType(), instance.BirthCountryCode);
            Assert.AreEqual(dependent.BirthDate, instance.BirthDate);
            Assert.AreEqual(dependent.CitizenshipCountryCode.GetCountryCodeWithType(), instance.CitizenshipCountryCode);
            Assert.AreEqual(dependent.EmailAddress, instance.EmailAddress);
            Assert.AreEqual(dependent.Gender.GetEVGenderCodeType(), instance.Gender);
            Assert.AreEqual(dependent.PermanentResidenceCountryCode.GetCountryCodeWithType(), instance.PermanentResidenceCountryCode);
            Assert.AreEqual(dependent.Relationship.GetDependentCodeType(), instance.Relationship);
            Assert.AreEqual(dependent.UserDefinedA, instance.UserDefinedA);
            Assert.AreEqual(dependent.UserDefinedB, instance.UserDefinedB);
            Assert.IsFalse(instance.BirthCountryReasonSpecified);
        }
    }
}
