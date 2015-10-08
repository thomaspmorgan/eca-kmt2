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
        public void TestDoValidateUpdate_CheckNameNotNull()
        {
            var name = "name";
            var entity = new OrganizationValidationEntity(name, OrganizationType.USEducationalInstitution.Id);
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            name = null;
            entity = new OrganizationValidationEntity(name, OrganizationType.USEducationalInstitution.Id);
            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("Name", results.First().Property);
            Assert.AreEqual(OrganizationServiceValidator.INVALID_ORGANIZATION_NAME_ERROR_MESSAGE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestUpdate_CheckNameNotWhitespace()
        {
            var name = "name";
            var entity = new OrganizationValidationEntity(name, OrganizationType.USEducationalInstitution.Id);
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            name = " ";
            entity = new OrganizationValidationEntity(name, OrganizationType.USEducationalInstitution.Id);
            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("Name", results.First().Property);
            Assert.AreEqual(OrganizationServiceValidator.INVALID_ORGANIZATION_NAME_ERROR_MESSAGE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestUpdate_CheckNameNotEmpty()
        {
            var name = "name";
            var entity = new OrganizationValidationEntity(name, OrganizationType.USEducationalInstitution.Id);
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            name = String.Empty;
            entity = new OrganizationValidationEntity(name, OrganizationType.USEducationalInstitution.Id);
            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("Name", results.First().Property);
            Assert.AreEqual(OrganizationServiceValidator.INVALID_ORGANIZATION_NAME_ERROR_MESSAGE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestUpdate_CheckType()
        {
            var entity = new OrganizationValidationEntity("name", OrganizationType.Office.Id);
            Assert.AreEqual(1, validator.DoValidateUpdate(entity).Count());
            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(OrganizationServiceValidator.INVALID_ORGANIZATION_TYPE_ERROR_MESSAGE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestDoValidateCreate_CheckNameNotNull()
        {
            var name = "name";
            var entity = new OrganizationValidationEntity(name, OrganizationType.USEducationalInstitution.Id);
            Assert.AreEqual(0, validator.DoValidateCreate(entity).Count());

            name = null;
            entity = new OrganizationValidationEntity(name, OrganizationType.USEducationalInstitution.Id);
            var results = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("Name", results.First().Property);
            Assert.AreEqual(OrganizationServiceValidator.INVALID_ORGANIZATION_NAME_ERROR_MESSAGE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestCreate_CheckNameNotWhitespace()
        {
            var name = "name";
            var entity = new OrganizationValidationEntity(name, OrganizationType.USEducationalInstitution.Id);
            Assert.AreEqual(0, validator.DoValidateCreate(entity).Count());

            name = " ";
            entity = new OrganizationValidationEntity(name, OrganizationType.USEducationalInstitution.Id);
            var results = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("Name", results.First().Property);
            Assert.AreEqual(OrganizationServiceValidator.INVALID_ORGANIZATION_NAME_ERROR_MESSAGE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestCreate_CheckNameNotEmpty()
        {
            var name = "name";
            var entity = new OrganizationValidationEntity(name, OrganizationType.USEducationalInstitution.Id);
            Assert.AreEqual(0, validator.DoValidateCreate(entity).Count());

            name = String.Empty;
            entity = new OrganizationValidationEntity(name, OrganizationType.USEducationalInstitution.Id);
            var results = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("Name", results.First().Property);
            Assert.AreEqual(OrganizationServiceValidator.INVALID_ORGANIZATION_NAME_ERROR_MESSAGE, results.First().ErrorMessage);
        }

        [TestMethod]
        public void TestCreate_CheckType()
        {
            var entity = new OrganizationValidationEntity("name", OrganizationType.Office.Id);
            Assert.AreEqual(1, validator.DoValidateCreate(entity).Count());
            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(OrganizationServiceValidator.INVALID_ORGANIZATION_TYPE_ERROR_MESSAGE, results.First().ErrorMessage);
        }
    }
}
