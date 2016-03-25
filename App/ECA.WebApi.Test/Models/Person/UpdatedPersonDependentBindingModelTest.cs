using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Person;
using ECA.Data;
using ECA.Business.Service;
using System;
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
            int dependentId = 2;
            int personId = 1;
            var sevisId = "N000000001";
            var personTypeId = PersonType.Spouse.Id;
            var firstName = "first";
            var lastName = "last";
            var suffix = "jr";
            var passport = "first last";
            var preferred = "first last";
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var placeOfBirth = 193;
            var birthCountryReason = "";
            var permanentResidenceCountryCode = 193;
            var countriesOfCitizenship = new List<int>();
            bool isTravellingWithParticipant = true;
            bool isDeleted = false;
            bool isSevisDeleted = false;

            var model = new UpdatedPersonDependentBindingModel
            {
                DependentId = dependentId,
                PersonId = personId,
                SevisId = sevisId,
                PersonTypeId = personTypeId,
                FirstName = firstName,
                LastName = lastName,
                NameSuffix = suffix,
                PassportName = passport,
                PreferredName = preferred,
                GenderId = gender,
                DateOfBirth = dateOfBirth,
                PlaceOfBirth_LocationId = placeOfBirth,
                Residence_LocationId = permanentResidenceCountryCode,
                BirthCountryReason = birthCountryReason,
                CountriesOfCitizenship = countriesOfCitizenship,
                IsTravellingWithParticipant = isTravellingWithParticipant,
                IsDeleted = isDeleted,
                IsSevisDeleted = isSevisDeleted
            };

            var instance = model.ToUpdatePersonDependent(user);
            Assert.AreEqual(model.DependentId, instance.DependentId);
            Assert.AreEqual(model.PersonId, instance.PersonId);
            Assert.AreEqual(model.PersonTypeId, instance.PersonTypeId);
            Assert.AreEqual(model.FirstName, instance.FirstName);
            Assert.AreEqual(model.LastName, instance.LastName);
            Assert.AreEqual(model.NameSuffix, instance.NameSuffix);
            Assert.AreEqual(model.PassportName, instance.PassportName);
            Assert.AreEqual(model.PreferredName, instance.PreferredName);
            Assert.AreEqual(model.GenderId, instance.GenderId);
            Assert.AreEqual(model.DateOfBirth, instance.DateOfBirth);
            Assert.AreEqual(model.PlaceOfBirth_LocationId, instance.PlaceOfBirth_LocationId);
            Assert.AreEqual(model.Residence_LocationId, instance.Residence_LocationId);
            Assert.AreEqual(model.BirthCountryReason, instance.BirthCountryReason);
            CollectionAssert.AreEqual(model.CountriesOfCitizenship, instance.CountriesOfCitizenship);
            Assert.AreEqual(model.IsTravellingWithParticipant, instance.IsTravellingWithParticipant);
            Assert.AreEqual(model.IsDeleted, instance.IsDeleted);
            Assert.AreEqual(model.IsSevisDeleted, instance.IsSevisDeleted);
        }
        
    }
}
