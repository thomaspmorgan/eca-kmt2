using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Projects;
using ECA.Business.Service;
using ECA.Data;
using ECA.Business.Service.Admin;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class AdditionalLocationTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var locationName = "location name";
            var latitude = 1.0f;
            var longitude = 2.0f;
            var cityId = 2;
            var countryId = 3;
            var divisionId = 5;
            var creatorId = 1;
            var creator = new User(creatorId);
            var locationTypeId = LocationType.Place.Id;
            var model = new AdditionalLocation(creator, locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
            Assert.AreEqual(locationName, model.LocationName);
            Assert.AreEqual(latitude, model.Latitude);
            Assert.AreEqual(longitude, model.Longitude);
            Assert.AreEqual(cityId, model.CityId);
            Assert.AreEqual(countryId, model.CountryId);
            Assert.AreEqual(locationTypeId, model.LocationTypeId);
            Assert.AreEqual(divisionId, model.DivisionId);
            Assert.IsTrue(Object.ReferenceEquals(creator, model.Audit.User));
        }
    }
}
