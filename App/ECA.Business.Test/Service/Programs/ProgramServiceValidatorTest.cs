using ECA.Business.Service.Programs;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Programs
{
    [TestClass]
    public class ProgramServiceValidatorTest
    {
        [TestMethod]
        public void TestValidate_WhitespaceDescription()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, " ", locationTypeIds, focus, owner, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_EmptyDescription()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, String.Empty, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_NullDescription()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, null, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationResult.ErrorMessage);

        }




        [TestMethod]
        public void TestValidate_WhitespaceName()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(" ", description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.INVALID_NAME_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_EmptyName()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(String.Empty, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.INVALID_NAME_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_NullName()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(null, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.INVALID_NAME_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_NotRegionLocationTypeId()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            locationTypeIds.Add(LocationType.State.Id);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);

            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.GIVEN_LOCATION_IS_NOT_A_REGION_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_MultipleRegionLocationTypeIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            locationTypeIds.Add(LocationType.State.Id);
            locationTypeIds.Add(LocationType.Region.Id);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);

            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NOT_ALL_LOCATIONS_ARE_REGIONS_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_FocusDoesNotExist()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, null, owner, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.FOCUS_DOES_NOT_EXIST_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_OwnerOrganizationIsNull()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, null, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.ORGANIZATION_DOES_NOT_EXIST_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_ParentProgramIdHasValue_ParentProgramIsNull()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, null);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.PARENT_PROGRAM_DOES_NOT_EXIST_ERROR_MESSAGE, validationResult.ErrorMessage);

        }


        [TestMethod]
        public void TestValidate_ParentProgramIdDoesNotHaveValue()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, null, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

        }

        [TestMethod]
        public void TestDoValidateCreate()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(0, results.Count);

        }

        [TestMethod]
        public void TestDoValidateUpdate()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var focus = new Focus();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, focus, owner, parentProgramId, parentProgram);
            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, results.Count);

        }
    }
}
