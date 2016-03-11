using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Sevis.Model;
using ECA.Business.Validation;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class UpdatedPersonTest
    {
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
            var person = new Business.Validation.Sevis.Bio.UpdatedPerson
            {
                BirthCity = "birth city",
                BirthCountryCode = "US",
                BirthCountryReason = "re",
                BirthCountryCodeSpecified = true,
                BirthDateSpecified = true,
                CitizenshipCountryCodeSpecified = true,
                GenderSpecified = true,
                PermanentResidenceCountryCodeSpecified = true,
                PrintForm = true,
                Remarks = "remarks",
                MailAddress = mailAddress,
                USAddress = usAddress,
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "UK",
                EmailAddress = "email@isp.com",
                FullName = new FullName
                {
                    FirstName = "first name",
                    LastName = "last name",
                    PassportName = "passport name",
                    PreferredName = "preferred name",
                    Suffix = FullNameValidator.SECOND_SUFFIX
                },
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                PermanentResidenceCountryCode = "FR",
                PhoneNumber = "phone number",
                PositionCode = 1,
                PositionCodeSpecified = true,
            };

            var instance = person.GetSEVISEVBatchTypeExchangeVisitorBiographical();
            Assert.AreEqual(person.BirthCity, instance.BirthCity);
            Assert.AreEqual(person.BirthCountryCode.GetBirthCntryCodeType(), instance.BirthCountryCode);
            Assert.AreEqual(person.BirthDate, instance.BirthDate);
            Assert.AreEqual(person.CitizenshipCountryCode.GetCountryCodeWithType(), instance.CitizenshipCountryCode);
            Assert.AreEqual(person.EmailAddress, instance.EmailAddress);
            Assert.AreEqual(person.Gender.GetGenderCodeType(), instance.Gender);
            Assert.AreEqual(person.PermanentResidenceCountryCode.GetCountryCodeWithType(), instance.PermanentResidenceCountryCode);
            Assert.AreEqual(person.PhoneNumber, instance.PhoneNumber);

            Assert.AreEqual(person.BirthCountryCodeSpecified, instance.BirthCountryCodeSpecified);
            Assert.AreEqual(person.BirthDateSpecified, instance.BirthDateSpecified);
            Assert.AreEqual(person.CitizenshipCountryCodeSpecified, instance.CitizenshipCountryCodeSpecified);
            Assert.AreEqual(person.GenderSpecified, instance.GenderSpecified);
            Assert.AreEqual(person.Remarks, instance.Remarks);
            Assert.AreEqual(person.PrintForm, instance.printForm);
            Assert.AreEqual(person.PositionCode, instance.PositionCode);
            Assert.AreEqual(person.PositionCodeSpecified, instance.PositionCodeSpecified);
            Assert.IsNotNull(instance.MailAddress);
            Assert.IsNotNull(instance.USAddress);
        }

        [TestMethod]
        public void TestGetSEVISEVBatchTypeExchangeVisitorBiographical_MailAddressIsNull()
        {
            var state = "TN";

            var usAddress = new AddressDTO();
            usAddress.Country = LocationServiceAddressValidator.UNITED_STATES_COUNTRY_NAME;
            usAddress.Division = state;
            var person = new Business.Validation.Sevis.Bio.UpdatedPerson
            {
                BirthCity = "birth city",
                BirthCountryCode = "US",
                BirthCountryReason = "re",
                BirthCountryCodeSpecified = true,
                BirthDateSpecified = true,
                CitizenshipCountryCodeSpecified = true,
                GenderSpecified = true,
                PermanentResidenceCountryCodeSpecified = true,
                PrintForm = true,
                Remarks = "remarks",
                MailAddress = null,
                USAddress = usAddress,
                BirthDate = DateTime.Now,
                CitizenshipCountryCode = "UK",
                EmailAddress = "email@isp.com",
                FullName = new FullName
                {
                    FirstName = "first name",
                    LastName = "last name",
                    PassportName = "passport name",
                    PreferredName = "preferred name",
                    Suffix = FullNameValidator.SECOND_SUFFIX
                },
                Gender = Gender.SEVIS_FEMALE_GENDER_CODE_VALUE,
                PermanentResidenceCountryCode = "FR",
                PhoneNumber = "phone number",
                PositionCode = 1,
                PositionCodeSpecified = true,
            };

            var instance = person.GetSEVISEVBatchTypeExchangeVisitorBiographical();
            Assert.IsNull(instance.MailAddress);
        }
    }
}
