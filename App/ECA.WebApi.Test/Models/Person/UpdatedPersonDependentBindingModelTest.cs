using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Person;
using ECA.Data;
using ECA.Business.Service;
using System;

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
                CityOfBirth = 1,
                CountryOfBirth = 2,
                CountriesOfCitizenship = new System.Collections.Generic.List<Location>(),
                EmailAddress = "email@domain.com",
                Gender = Gender.Male.Id,
                PermanentResidenceCountryCode = 2,
                PersonTypeId = PersonType.Spouse.Id,
                BirthCountryReason = "rebel"
            };
            var user = new User(1);
            var instance = model.ToUpdatedPersonDependent(user);
            Assert.AreEqual(model.FullName, instance.FullName);
            Assert.AreEqual(model.DateOfBirth, instance.DateOfBirth);
            Assert.AreEqual(model.CityOfBirth, instance.CityOfBirth);
            Assert.AreEqual(model.CountryOfBirth, instance.CountryOfBirth);
            CollectionAssert.AreEqual(model.CountriesOfCitizenship, instance.CountriesOfCitizenship);
            Assert.AreEqual(model.EmailAddress, instance.EmailAddress);
            Assert.AreEqual(model.Gender, instance.Gender);
            Assert.AreEqual(model.PermanentResidenceCountryCode, instance.PermanentResidenceCountryCode);
            Assert.AreEqual(model.PersonTypeId, instance.PersonTypeId);
            Assert.AreEqual(model.BirthCountryReason, instance.BirthCountryReason);
        }



    }
}
