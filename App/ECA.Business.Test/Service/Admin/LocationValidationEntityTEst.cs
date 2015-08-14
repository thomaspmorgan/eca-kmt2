using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class LocationValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var locationName = "location name";
            var latitude = 1.0f;
            var longitude = 2.0f;
            var cityId = 2;
            var countryId = 3;
            var divisionId = 4;
            var locationTypeId = LocationType.Place.Id;
            var model = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
            var country = new Location();
            var city = new Location();
            var division = new Location();

            var entity = new LocationValidationEntity(model, country, division, city);
            Assert.AreEqual(model.LocationName, entity.LocationName);
            Assert.AreEqual(model.LocationTypeId, entity.LocationTypeId);
            Assert.AreEqual(model.Latitude, entity.Latitude);
            Assert.AreEqual(model.Longitude, entity.Longitude);
            Assert.IsTrue(Object.ReferenceEquals(city, entity.City));
            Assert.IsTrue(Object.ReferenceEquals(division, entity.Division));
            Assert.IsTrue(Object.ReferenceEquals(country, entity.Country));
        }
    }
}
