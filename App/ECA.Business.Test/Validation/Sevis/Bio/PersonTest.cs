using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Validation.Sevis.Bio;
using ECA.Data;
using ECA.Business.Sevis.Model;
using ECA.Business.Validation;

namespace ECA.Business.Test.Validation.Sevis.Bio
{
    [TestClass]
    public class PersonTest
    {
        [TestMethod]
        public void TestGetEVPersonTypeBiographical()
        {
            var person = new Business.Validation.Sevis.Bio.Person
            {
                BirthCity = "birth city",
                BirthCountryCode = "US",
                BirthCountryReason = "re",
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
                PhoneNumber = "phone number"
            };

            var instance = person.GetEVPersonTypeBiographical();
            Assert.AreEqual(person.BirthCity, instance.BirthCity);
            Assert.AreEqual(person.BirthCountryCode.GetBirthCntryCodeType(), instance.BirthCountryCode);
            Assert.AreEqual(person.BirthDate, instance.BirthDate);
            Assert.AreEqual(person.CitizenshipCountryCode.GetCountryCodeWithType(), instance.CitizenshipCountryCode);
            Assert.AreEqual(person.EmailAddress, instance.EmailAddress);
            Assert.AreEqual(person.Gender.GetEVGenderCodeType(), instance.Gender);
            Assert.AreEqual(person.PermanentResidenceCountryCode.GetCountryCodeWithType(), instance.PermanentResidenceCountryCode);
            Assert.AreEqual(person.PhoneNumber, instance.PhoneNumber);
            Assert.IsFalse(instance.BirthCountryReasonSpecified);
        }
    }
}
