using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Projects;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Projects
{
    [TestClass]
    public class ProjectLocationBindingModelTest
    {
        [TestMethod]
        public void TestToAdditionalProjectLocation()
        {
            var model = new ProjectLocationBindingModel();
            model.ProjectId = 1;
            model.CityId = 2;
            model.CountryId = 3;
            model.Latitude = 1.0f;
            model.LocationName = "name";
            model.Longitude = 2.0f;

            var user = new User(1);
            var instance = model.ToAdditionalProjectLocation(user);
            Assert.AreEqual(model.ProjectId, instance.ProjectId);
            Assert.AreEqual(model.CityId, instance.CityId);
            Assert.AreEqual(model.CountryId, instance.CountryId);
            Assert.AreEqual(model.Latitude, instance.Latitude);
            Assert.AreEqual(model.LocationName, instance.LocationName);
            Assert.AreEqual(model.Longitude, instance.Longitude);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
        }
    }
}
