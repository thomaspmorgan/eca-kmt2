using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Projects;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Admin
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
            var locationTypeId = LocationType.Place.Id;
            var locationId = 5;
            var divisionId = 6;
            var model = new UpdatedLocation(creator, locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId, locationId);
            Assert.AreEqual(locationName, model.LocationName);
            Assert.AreEqual(latitude, model.Latitude);
            Assert.AreEqual(longitude, model.Longitude);
            Assert.AreEqual(cityId, model.CityId);
            Assert.AreEqual(countryId, model.CountryId);
            Assert.AreEqual(locationTypeId, model.LocationTypeId);
            Assert.AreEqual(locationId, model.LocationId);
            Assert.AreEqual(LocationType.Place.Id, model.LocationTypeId);
            Assert.AreEqual(divisionId, model.DivisionId);
            Assert.IsTrue(Object.ReferenceEquals(creator, model.Audit.User));
        }
    }
}
