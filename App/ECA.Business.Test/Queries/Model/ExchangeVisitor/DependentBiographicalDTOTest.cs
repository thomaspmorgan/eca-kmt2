using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Queries.Models.Persons;
using ECA.Business.Validation.Model.CreateEV;

namespace ECA.Business.Test.Queries.Model.ExchangeVisitor
{
    [TestClass]
    public class DependentBiographicalDTOTest
    {
        [TestMethod]
        public void TestGetAddDependent()
        {
            var dto = new DependentBiographicalDTO
            {
                AddressId = 1,
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@isp.com",
                EmailAddressId = 2,
                FullName = new FullNameDTO
                {
                    FirstName = "first name",
                    LastName = "last name",
                    PassportName = "passport",
                    PreferredName = "preferred name",
                    Suffix = "suffix"
                },
                Gender = "male",
                GenderId = 3,
                NumberOfCitizenships = 4,
                PermanentResidenceCountryCode = "residence code",
                PersonId = 5,
                PhoneNumber = "123-456-7890",
                PhoneNumberId = 6,
                PositionCode = "position",
                Relationship = "relationship",
                PersonTypeId = 7,
                SevisId = "sevis id"
            };
            var instance = dto.GetAddDependent();
            Assert.AreEqual(dto.BirthCity, instance.BirthCity);
            Assert.AreEqual(dto.BirthCountryCode, instance.BirthCountryCode);
            Assert.AreEqual(dto.BirthCountryReason, instance.BirthCountryReason);
            Assert.AreEqual(dto.BirthDate, instance.BirthDate);
            Assert.AreEqual(dto.CitizenshipCountryCode, instance.CitizenshipCountryCode);
            Assert.AreEqual(dto.EmailAddress, instance.EmailAddress);
            Assert.AreEqual(dto.Gender, instance.Gender);
            Assert.AreEqual(dto.PermanentResidenceCountryCode, instance.PermanentResidenceCountryCode);            
            Assert.AreEqual(dto.Relationship, instance.Relationship);
            Assert.IsNull(instance.UserDefinedA);
            Assert.IsNull(instance.UserDefinedB);
            Assert.IsFalse(instance.printForm);

            Assert.AreEqual(dto.FullName.FirstName, instance.FullName.FirstName);
            Assert.AreEqual(dto.FullName.LastName, instance.FullName.LastName);
            Assert.AreEqual(dto.FullName.PassportName, instance.FullName.PassportName);
            Assert.AreEqual(dto.FullName.PreferredName, instance.FullName.PreferredName);
            Assert.AreEqual(dto.FullName.Suffix, instance.FullName.Suffix);
        }

        [TestMethod]
        public void TestGetCreateDependent()
        {
            var dto = new DependentBiographicalDTO
            {
                AddressId = 1,
                BirthCity = "birth city",
                BirthCountryCode = "birth country code",
                BirthCountryReason = "birth country reason",
                BirthDate = DateTime.UtcNow,
                CitizenshipCountryCode = "citizenship country code",
                EmailAddress = "someone@isp.com",
                EmailAddressId = 2,
                FullName = new FullNameDTO
                {
                    FirstName = "first name",
                    LastName = "last name",
                    PassportName = "passport",
                    PreferredName = "preferred name",
                    Suffix = "suffix"
                },
                Gender = "male",
                GenderId = 3,
                NumberOfCitizenships = 4,
                PermanentResidenceCountryCode = "residence code",
                PersonId = 5,
                PhoneNumber = "123-456-7890",
                PhoneNumberId = 6,
                PositionCode = "position",
                Relationship = "relationship",
                PersonTypeId = 7,
                SevisId = "sevis id"
            };
            var instance = dto.GetCreateDependent();
            Assert.IsInstanceOfType(instance.AddTIPP, typeof(EcaAddTIPP));
            Assert.IsNotNull(instance.Dependent);

            //sanity check
            Assert.AreEqual(dto.FullName.FirstName, instance.Dependent.FullName.FirstName);

        }
    }
}
