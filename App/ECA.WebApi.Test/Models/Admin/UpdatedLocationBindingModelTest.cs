using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Admin;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class UpdatedLocationBindingModelTest
    {
        [TestMethod]
        public void TestToUpdatedLocation()
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
            var model = new UpdatedLocationBindingModel
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

            var instance = model.ToUpdatedLocation(user);
            Assert.AreEqual(latitude, instance.Latitude);
            Assert.AreEqual(longitude, instance.Longitude);
            Assert.AreEqual(cityId, instance.CityId);
            Assert.AreEqual(divisionId, instance.DivisionId);
            Assert.AreEqual(countryId, instance.CountryId);
            Assert.AreEqual(regionId, instance.RegionId);
            Assert.AreEqual(name, instance.LocationName);
            Assert.AreEqual(locationTypeId, instance.LocationTypeId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
        }
    }
}
