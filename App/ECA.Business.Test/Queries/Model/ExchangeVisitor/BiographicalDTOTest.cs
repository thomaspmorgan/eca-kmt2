using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Validation.Model;

namespace ECA.Business.Test.Queries.Model.ExchangeVisitor
{
    [TestClass]
    public class BiographicalDTOTest
    {
        [TestMethod]
        public void TestGetBiographical()
        {
            var fullName = new FullNameDTO
            {
                FirstName = "first",
                LastName = "last",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "suffix"
            };

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
            };

            var biography = model.GetBiographical();
            Assert.IsNotNull(biography);
            Assert.AreEqual(model.BirthCity, biography.BirthCity);
            Assert.AreEqual(model.BirthCountryCode, biography.BirthCountryCode);
            Assert.AreEqual(model.BirthCountryReason, biography.BirthCountryReason);
            Assert.AreEqual(model.BirthDate, biography.BirthDate);
            Assert.AreEqual(model.CitizenshipCountryCode, biography.CitizenshipCountryCode);
            Assert.AreEqual(model.EmailAddress, biography.EmailAddress);
            Assert.AreEqual(model.Gender, biography.Gender);
            Assert.AreEqual(model.PermanentResidenceCountryCode, biography.PermanentResidenceCountryCode);

            Assert.AreEqual(fullName.FirstName, biography.FullName.FirstName);
            Assert.AreEqual(fullName.LastName, biography.FullName.LastName);
            Assert.AreEqual(fullName.PassportName, biography.FullName.PassportName);
            Assert.AreEqual(fullName.PreferredName, biography.FullName.PreferredName);
            Assert.AreEqual(fullName.Suffix, biography.FullName.Suffix);

        }

        [TestMethod]
        public void TestGetBiographicalUpdate()
        {
            var fullName = new FullNameDTO
            {
                FirstName = "first",
                LastName = "last",
                PassportName = "passport",
                PreferredName = "preferred",
                Suffix = "suffix"
            };

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
            };


            var biography = model.GetBiographicalUpdate();
            Assert.IsNotNull(biography);
            Assert.AreEqual(model.BirthCity, biography.BirthCity);
            Assert.AreEqual(model.BirthCountryCode, biography.BirthCountryCode);
            Assert.AreEqual(model.BirthCountryReason, biography.BirthCountryReason);
            Assert.AreEqual(model.BirthDate, biography.BirthDate);
            Assert.AreEqual(model.CitizenshipCountryCode, biography.CitizenshipCountryCode);
            Assert.AreEqual(model.EmailAddress, biography.EmailAddress);
            Assert.AreEqual(model.Gender, biography.Gender);
            Assert.AreEqual(model.PermanentResidenceCountryCode, biography.PermanentResidenceCountryCode);

            Assert.AreEqual(model.PhoneNumber, biography.PhoneNumber);
            Assert.AreEqual(model.PositionCode, biography.PositionCode);

            Assert.AreEqual(fullName.FirstName, biography.FullName.FirstName);
            Assert.AreEqual(fullName.LastName, biography.FullName.LastName);
            Assert.AreEqual(fullName.PassportName, biography.FullName.PassportName);
            Assert.AreEqual(fullName.PreferredName, biography.FullName.PreferredName);
            Assert.AreEqual(fullName.Suffix, biography.FullName.Suffix);

            Assert.IsTrue(biography.printForm);

        }
    }
}
