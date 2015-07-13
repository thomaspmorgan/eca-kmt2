using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Admin;
using System.Collections.Generic;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class UpdatedOrganizationBindingModelTest
    {
        [TestMethod]
        public void TestToEcaOrganization()
        {
            var model = new UpdatedOrganizationBindingModel();
            model.Description = "desc";
            model.Name = "name";
            model.OrganizationId = 1;
            model.OrganizationTypeId = OrganizationType.USEducationalInstitution.Id;
            model.ParentOrganizationId = 3;
            model.PointsOfContactIds = new List<int>();
            model.Website = "website";
            var user = new User(1);

            var instance = model.ToEcaOrganization(user);
            Assert.AreEqual(model.Description, instance.Description);
            Assert.AreEqual(model.Name, instance.Name);
            Assert.AreEqual(model.OrganizationId, instance.OrganizationId);
            Assert.AreEqual(model.OrganizationTypeId, instance.OrganizationTypeId);
            Assert.AreEqual(model.ParentOrganizationId, instance.ParentOrganizationId);
            Assert.AreEqual(model.Website, instance.Website);
            CollectionAssert.AreEqual(model.PointsOfContactIds.ToList(), instance.ContactIds.ToList());
        }
    }
}
