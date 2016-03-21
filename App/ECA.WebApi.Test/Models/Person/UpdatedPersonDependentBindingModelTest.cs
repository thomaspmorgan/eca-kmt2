using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Person;
using ECA.Data;
using ECA.Business.Service;
using System;
using ECA.Business.Queries.Models.Admin;
using System.Collections.Generic;

namespace ECA.WebApi.Test.Models.Person
{
    [TestClass]
    public class UpdatedPersonDependentBindingModelTest
    {

        [TestMethod]
        public void TestToUpdatedPersonDependent()
        {
            User user = new User(1);
            int personId = 1;
            var firstName = "first";
            var lastName = "last";
            var suffix = "jr";
            var passport = "first last";
            var preferred = "first last";
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var placeOfBirth = new LocationDTO { CityId = 5, CountryId = 193 };
            var personTypeId = PersonType.Spouse.Id;
            var countriesOfCitizenship = new List<int>();
            var permanentResidenceCountryCode = 193;
            var birthCountryReason = "";

            var model = new UpdatedPersonDependentBindingModel
            {
                PersonId = personId,
                FirstName = firstName,
                LastName = lastName,
                NameSuffix = suffix,
                PassportName = passport,
                PreferredName = preferred,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                CityOfBirth = placeOfBirth.CityId,
                PersonTypeId = personTypeId,
                CountriesOfCitizenship = countriesOfCitizenship,
                PermanentResidenceCountryCode = permanentResidenceCountryCode,
                BirthCountryReason = birthCountryReason
            };

            var instance = model.ToUpdatePersonDependent(user);
            Assert.AreEqual(model.FirstName, instance.FirstName);
            Assert.AreEqual(model.LastName, instance.LastName);
            Assert.AreEqual(model.PassportName, instance.PassportName);
            Assert.AreEqual(model.PreferredName, instance.PreferredName);
            Assert.AreEqual(model.Gender, instance.Gender);
            Assert.AreEqual(model.DateOfBirth, instance.DateOfBirth);
            Assert.AreEqual(model.CityOfBirth, instance.CityOfBirth);
            Assert.AreEqual(model.PersonTypeId, instance.PersonTypeId);
            CollectionAssert.AreEqual(model.CountriesOfCitizenship, instance.CountriesOfCitizenship);
            Assert.AreEqual(model.PermanentResidenceCountryCode, instance.PermanentResidenceCountryCode);
            Assert.AreEqual(model.BirthCountryReason, instance.BirthCountryReason);
        }
        
    }
}
