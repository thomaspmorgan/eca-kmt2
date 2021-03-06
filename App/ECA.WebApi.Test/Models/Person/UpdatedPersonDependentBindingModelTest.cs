﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Person;
using ECA.Data;
using ECA.Business.Service;
using System;
using System.Collections.Generic;
using ECA.Business.Queries.Models.Persons;

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
            var dependentTypeId = DependentType.Child.Id;
            var firstName = "first";
            var lastName = "last";
            var suffix = "jr";
            var passport = "first last";
            var preferred = "first last";
            var gender = Gender.Female.Id;
            var dateOfBirth = DateTime.Now;
            var placeOfBirth = 193;
            var birthCountryReasonId = 0;
            var permanentResidenceCountryCode = 193;
            var emailAddress = "test@test.com";
            var countriesOfCitizenship = new List<PersonDependentCitizenCountry>();
            bool isTravellingWithParticipant = true;
            bool isDeleted = false;
            bool isSevisDeleted = false;

            var countries = new List<CitizenCountryDTO>();
            foreach (var citizenship in countriesOfCitizenship)
            {
                countries.Add(new CitizenCountryDTO { LocationId = citizenship.LocationId, LocationName = citizenship.Location.LocationName, IsPrimary = citizenship.IsPrimary });
            }

            var model = new UpdatedPersonDependentBindingModel
            {
                DependentId = dependentId,
                PersonId = personId,
                SevisId = sevisId,
                DependentTypeId = dependentTypeId,
                FirstName = firstName,
                LastName = lastName,
                NameSuffix = suffix,
                PassportName = passport,
                PreferredName = preferred,
                GenderId = gender,
                DateOfBirth = dateOfBirth,
                PlaceOfBirthId = placeOfBirth,
                PlaceOfResidenceId = permanentResidenceCountryCode,
                BirthCountryReasonId = birthCountryReasonId,
                EmailAddress = emailAddress,
                CountriesOfCitizenship = countries,
                IsTravellingWithParticipant = isTravellingWithParticipant,
                IsDeleted = isDeleted,
                IsSevisDeleted = isSevisDeleted
            };

            var instance = model.ToUpdatePersonDependent(user);
            Assert.AreEqual(model.DependentId, instance.DependentId);
            Assert.AreEqual(model.PersonId, instance.PersonId);
            Assert.AreEqual(model.DependentTypeId, instance.DependentTypeId);
            Assert.AreEqual(model.FirstName, instance.FirstName);
            Assert.AreEqual(model.LastName, instance.LastName);
            Assert.AreEqual(model.NameSuffix, instance.NameSuffix);
            Assert.AreEqual(model.PassportName, instance.PassportName);
            Assert.AreEqual(model.PreferredName, instance.PreferredName);
            Assert.AreEqual(model.GenderId, instance.GenderId);
            Assert.AreEqual(model.DateOfBirth, instance.DateOfBirth);
            Assert.AreEqual(model.PlaceOfBirthId, instance.PlaceOfBirthId);
            Assert.AreEqual(model.PlaceOfResidenceId, instance.PlaceOfResidenceId);
            Assert.AreEqual(model.BirthCountryReasonId, instance.BirthCountryReasonId);
            Assert.AreEqual(model.EmailAddress, instance.EmailAddress);
            CollectionAssert.AreEqual(model.CountriesOfCitizenship, instance.CountriesOfCitizenship);
            Assert.AreEqual(model.IsTravellingWithParticipant, instance.IsTravellingWithParticipant);
            Assert.AreEqual(model.IsDeleted, instance.IsDeleted);
            Assert.AreEqual(model.IsSevisDeleted, instance.IsSevisDeleted);
        }
        
    }
}
