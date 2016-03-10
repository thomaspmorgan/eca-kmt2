using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Validation.Model;
using ECA.Business.Queries.Models.Admin;

namespace ECA.Business.Test.Queries.Model.ExchangeVisitor
{
    [TestClass]
    public class BiographicalDTOTest
    {
        [TestMethod]
        public void TestGetPerson()
        {
            var fullName = new FullNameDTO
            {
                FirstName = "first",
                LastName = "last",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "suffix"
            };
            var mailAddress = new AddressDTO();
            mailAddress.AddressId = 50;

            var residenceAddress = new AddressDTO();
            residenceAddress.AddressId = 60;

            var usAddress = new AddressDTO();
            usAddress.AddressId = 70;

            var model = new BiographicalDTO
            {
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@ispl.com",
                FullName = fullName,
                Gender = "male",
                PermanentResidenceCountryCode = "perm resident country code",
                PersonId = 2,
                PhoneNumber = "123-456-7890",
                PositionCode = "position code",
                EmailAddressId = 3,
                GenderId = 4,
                MailAddress = mailAddress,
                NumberOfCitizenships = 2000,
                PermanentResidenceAddressId = residenceAddress.AddressId,
                PhoneNumberId = 5,
                USAddress = usAddress
            };

            var person = model.GetPerson();
            Assert.IsNotNull(person);
            Assert.AreEqual(model.BirthCity, person.BirthCity);
            Assert.AreEqual(model.BirthCountryCode, person.BirthCountryCode);
            Assert.AreEqual(model.BirthCountryReason, person.BirthCountryReason);
            Assert.AreEqual(model.BirthDate, person.BirthDate);
            Assert.AreEqual(model.CitizenshipCountryCode, person.CitizenshipCountryCode);
            Assert.AreEqual(model.EmailAddress, person.EmailAddress);
            Assert.AreEqual(model.Gender, person.Gender);
            Assert.AreEqual(model.PermanentResidenceCountryCode, person.PermanentResidenceCountryCode);

            Assert.AreEqual(fullName.FirstName, person.FullName.FirstName);
            Assert.AreEqual(fullName.LastName, person.FullName.LastName);
            Assert.AreEqual(fullName.PassportName, person.FullName.PassportName);
            Assert.AreEqual(fullName.PreferredName, person.FullName.PreferredName);
            Assert.AreEqual(fullName.Suffix, person.FullName.Suffix);
        }
    }
}
