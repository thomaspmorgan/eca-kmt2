using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.Logging;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace ECA.Business.Test.Service.Persons
{
    [TestClass]
    public class PersonServiceTest
    {
        private TestEcaContext context;
        private PersonService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new PersonService(context, new TraceLogger());
        }

        #region Get Pii By Id
        [TestMethod]
        public async Task TestGetPiiById_CheckProperties()
        {
            var gender = new Gender
            {
                GenderId = 1,
                GenderName = "genderName"
            };

            var person = new Person
            {
                PersonId = 1,
                Gender = gender,
                DateOfBirth = DateTimeOffset.Now,
                FirstName = "firstName",
                LastName = "lastName",
                NamePrefix = "namePrefix",
                NameSuffix = "nameSuffix",
                GivenName = "givenName",
                FamilyName = "familyName",
                MiddleName = "middleName",
                Patronym = "patronym",
                Alias = "alias"

            };

            context.Genders.Add(gender);
            context.People.Add(person);

            Action<PiiDTO> tester = (serviceResult) => 
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(gender.GenderName, serviceResult.Gender);
                DateTimeOffset.UtcNow.Should().BeCloseTo(serviceResult.DateOfBirth, DbContextHelper.DATE_PRECISION);
                Assert.AreEqual(person.FirstName, serviceResult.FirstName);
                Assert.AreEqual(person.LastName, serviceResult.LastName);
                Assert.AreEqual(person.NamePrefix, serviceResult.NamePrefix);
                Assert.AreEqual(person.NameSuffix, serviceResult.NameSuffix);
                Assert.AreEqual(person.GivenName, serviceResult.GivenName);
                Assert.AreEqual(person.FamilyName, serviceResult.FamilyName);
                Assert.AreEqual(person.MiddleName, serviceResult.MiddleName);
                Assert.AreEqual(person.Patronym, serviceResult.Patronym);
                Assert.AreEqual(person.Alias, serviceResult.Alias);
            };

            var result = this.service.GetPiiById(person.PersonId);
            var resultAsync = await this.service.GetPiiByIdAsync(person.PersonId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetPiiById_CheckCountriesOfCitizenship()
        {
            var location = new Location
            {
                LocationId = 1,
                LocationName = "locationName"
            };

            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "locationName2"
            };

            var location3 = new Location
            {
                LocationId = 3,
                LocationName = "locationName3"
            };

            var person = new Person
            {
                PersonId = 1,
            };

            person.CountriesOfCitizenship.Add(location);
            person.CountriesOfCitizenship.Add(location2);
            person.CountriesOfCitizenship.Add(location3);

            context.Locations.Add(location);
            context.Locations.Add(location2);
            context.Locations.Add(location3);
            context.People.Add(person);

            Action<PiiDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                CollectionAssert.AreEqual(context.Locations.Select(x => x.LocationId).ToList(), serviceResult.CountriesOfCitizenship.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(context.Locations.Select(x => x.LocationName).ToList(), serviceResult.CountriesOfCitizenship.Select(x => x.Value).ToList());
            };
            var result = this.service.GetPiiById(person.PersonId);
            var resultAsync = await this.service.GetPiiByIdAsync(person.PersonId);

            tester(result);
            tester(resultAsync);
        }
        #endregion
    }
}
