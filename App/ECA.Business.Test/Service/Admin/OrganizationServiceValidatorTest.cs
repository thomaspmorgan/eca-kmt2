using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Data;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class OrganizationServiceValidatorTest
    {
        private OrganizationServiceValidator validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new OrganizationServiceValidator();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void TestDoValidateCreate_NotImplemented()
        {
            validator.DoValidateCreate(null);
        }

        [TestMethod]
        public void TestDoValidateUpdate_CheckNameNotNull()
        {
            var name = "name";
            var entity = new UpdateOrganizationValidationEntity(name, 1);
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            name = null;
            entity = new UpdateOrganizationValidationEntity(name, 1);
            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("Name", results.First().Property);
            Assert.AreEqual(OrganizationServiceValidator.INVALID_ORGANIZATION_NAME_ERROR_MESSAGE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestUpdate_CheckNameNotWhitespace()
        {
            var name = "name";
            var entity = new UpdateOrganizationValidationEntity(name, 1);
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            name = " ";
            entity = new UpdateOrganizationValidationEntity(name, 1);
            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("Name", results.First().Property);
            Assert.AreEqual(OrganizationServiceValidator.INVALID_ORGANIZATION_NAME_ERROR_MESSAGE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestUpdate_CheckNameNotEmpty()
        {
            var name = "name";
            var entity = new UpdateOrganizationValidationEntity(name, 1);
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            name = String.Empty;
            entity = new UpdateOrganizationValidationEntity(name, 1);
            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("Name", results.First().Property);
            Assert.AreEqual(OrganizationServiceValidator.INVALID_ORGANIZATION_NAME_ERROR_MESSAGE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestUpdate_CheckType()
        {
            var entity = new UpdateOrganizationValidationEntity("name", OrganizationType.Office.Id);
            Assert.AreEqual(1, validator.DoValidateUpdate(entity).Count());
            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(OrganizationServiceValidator.INVALID_ORGANIZATION_TYPE_ERROR_MESSAGE, results.First().ErrorMessage);
        }
    }
}
