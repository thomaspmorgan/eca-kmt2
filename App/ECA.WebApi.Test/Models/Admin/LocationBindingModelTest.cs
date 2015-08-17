using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service;
using ECA.WebApi.Models.Admin;
using ECA.Data;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class LocationBindingModelTest
    {
        [TestMethod]
        public void TestToAdditionalLocation()
        {
            var userId = 1;
            var user = new User(userId);
            var latitude = 1.0f;
            var longitude = 2.0f;
            var cityId = 3;
            var divisionId = 4;
            var countryId = 5;
            var regionId = 6;
            var name = "name";
            var locationTypeId = LocationType.Building.Id;
            var model = new LocationBindingModel
            {
                CityId = cityId,
                CountryId = countryId,
                DivisionId = divisionId,
                RegionId = regionId,
                Latitude = latitude,
                Name = name,
                LocationTypeId = locationTypeId,
                Longitude = longitude
            };

            var instance = model.ToAdditionalLocation(user);
            Assert.AreEqual(latitude, instance.Latitude);
            Assert.AreEqual(longitude, instance.Longitude);
            Assert.AreEqual(cityId, instance.CityId);
            Assert.AreEqual(divisionId, instance.DivisionId);
            Assert.AreEqual(regionId, instance.RegionId);
            Assert.AreEqual(countryId, instance.CountryId);
            Assert.AreEqual(name, instance.LocationName);
            Assert.AreEqual(locationTypeId, instance.LocationTypeId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
        }
    }
}
