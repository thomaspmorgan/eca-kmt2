using ECA.Business.Service;
using ECA.Business.Service.Projects;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Projects
{
    [TestClass]
    public class UpdatedProjectLocationTest
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
            var locationId = 5;
            var model = new UpdatedProjectLocation(creator, locationName, cityId, countryId, latitude, longitude, projectId, locationId);
            Assert.AreEqual(locationName, model.LocationName);
            Assert.AreEqual(latitude, model.Latitude);
            Assert.AreEqual(longitude, model.Longitude);
            Assert.AreEqual(cityId, model.CityId);
            Assert.AreEqual(countryId, model.CountryId);
            Assert.AreEqual(projectId, model.ProjectId);
            Assert.AreEqual(locationId, model.LocationId);
            Assert.AreEqual(LocationType.Place.Id, model.LocationTypeId);
            Assert.IsTrue(Object.ReferenceEquals(creator, model.Audit.User));
        }
    }
}
