using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Person;
using ECA.Data;
using ECA.Business.Service;
using System;
using ECA.Business.Service.Lookup;
using ECA.Business.Queries.Models.Admin;

namespace ECA.WebApi.Test.Models.Person
{
    [TestClass]
    public class UpdatedPersonDependentBindingModelTest
    {

        [TestMethod]
        public void TestToUpdatedPersonDependent()
        {
            var model = new UpdatedPersonDependentBindingModel
            {
                FullName = new Business.Queries.Models.Persons.FullNameDTO
                {
                    FirstName = "firstname",
                    LastName = "lastname",
                    PreferredName = "prefname",
                    PassportName = "passname",
                    Suffix = "suffix"
                },
                DateOfBirth = DateTime.Now,
                PlaceOfBirth = new LocationDTO { CityId = 5, CountryId = 193 },
                CountriesOfCitizenship = new System.Collections.Generic.List<int>(),
                EmailAddress = "email@domain.com",
                GenderId = Gender.Male.Id,
                PermanentResidenceCountryCode = 2,
                PersonTypeId = PersonType.Spouse.Id,
                BirthCountryReason = "rebel"
            };
            var user = new User(1);
            var instance = model.ToUpdatePersonDependent(user);
            Assert.AreEqual(model.FullName, instance.FullName);
            Assert.AreEqual(model.DateOfBirth, instance.DateOfBirth);
            Assert.AreEqual(model.PlaceOfBirth, instance.PlaceOfBirth);
            CollectionAssert.AreEqual(model.CountriesOfCitizenship, instance.CountriesOfCitizenship);
            Assert.AreEqual(model.EmailAddress, instance.EmailAddress);
            Assert.AreEqual(model.GenderId, instance.GenderId);
            Assert.AreEqual(model.PermanentResidenceCountryCode, instance.PermanentResidenceCountryCode);
            Assert.AreEqual(model.PersonTypeId, instance.PersonTypeId);
            Assert.AreEqual(model.BirthCountryReason, instance.BirthCountryReason);
        }



    }
}
