using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Projects;
using ECA.Business.Service;
using ECA.Data;
using ECA.Business.Service.Admin;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class EcaLocationTest
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
            Assert.AreEqual(locationName, model.LocationName);
            Assert.AreEqual(latitude, model.Latitude);
            Assert.AreEqual(longitude, model.Longitude);
            Assert.AreEqual(cityId, model.CityId);
            Assert.AreEqual(countryId, model.CountryId);
            Assert.AreEqual(divisionId, model.DivisionId);
            Assert.AreEqual(locationTypeId, model.LocationTypeId);
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownStaticLookupException))]
        public void TestConstructor_LocationTypeUnknown()
        {
            var locationName = "location name";
            var latitude = 1.0f;
            var longitude = 2.0f;
            var cityId = 2;
            var countryId = 3;
            var divisionId = 4;
            var locationTypeId = -1;
            var model = new EcaLocation(locationName, cityId, countryId, divisionId, latitude, longitude, locationTypeId);
        }
    }
}
