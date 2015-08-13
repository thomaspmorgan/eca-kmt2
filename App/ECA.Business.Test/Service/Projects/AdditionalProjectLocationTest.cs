using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Projects;
using ECA.Business.Service;
using ECA.Data;

namespace ECA.Business.Test.Service.Projects
{
    [TestClass]
    public class AdditionalProjectLocationTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var locationName = "location name";
            var latitude = 1.0f;
            var longitude = 2.0f;
            var cityId = 2;
            var countryId = 3;
            var creatorId = 1;
            var creator = new User(creatorId);
            var projectId = 4;
            var model = new AdditionalProjectLocation(creator, locationName, cityId, countryId, latitude, longitude, projectId);
            Assert.AreEqual(locationName, model.LocationName);
            Assert.AreEqual(latitude, model.Latitude);
            Assert.AreEqual(longitude, model.Longitude);
            Assert.AreEqual(cityId, model.CityId);
            Assert.AreEqual(countryId, model.CountryId);
            Assert.AreEqual(projectId, model.ProjectId);
            Assert.AreEqual(LocationType.Place.Id, model.LocationTypeId);
            Assert.IsTrue(Object.ReferenceEquals(creator, model.Audit.User));
        }
    }
}
