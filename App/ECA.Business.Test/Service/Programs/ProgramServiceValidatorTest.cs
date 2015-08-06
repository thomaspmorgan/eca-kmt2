﻿using ECA.Business.Service.Admin;
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
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();


            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, " ", locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
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
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, String.Empty, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds,  owner, officeSettings, parentProgramId, parentProgram);
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
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, null, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
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
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(" ", description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
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
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(String.Empty, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
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
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(null, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
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
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            locationTypeIds.Add(LocationType.Division.Id);
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
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            locationTypeIds.Add(LocationType.Division.Id);
            locationTypeIds.Add(LocationType.Region.Id);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);

            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NOT_ALL_LOCATIONS_ARE_REGIONS_ERROR_MESSAGE, validationResult.ErrorMessage);

        }
        
        [TestMethod]
        public void TestValidate_OwnerOrganizationIsNull()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, null, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.ORGANIZATION_DOES_NOT_EXIST_ERROR_MESSAGE, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestValidate_OwnerRequiresCategories_EmptyCategoryIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            officeSettings.IsCategoryRequired = true;
            categoryIds.Clear();
            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NO_CATEGORIES_GIVEN_ERROR_MESSAGE, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestValidate_OwnerRequiresCategories_NullCategoryIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            officeSettings.IsCategoryRequired = true;
            categoryIds = null;
            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NO_CATEGORIES_GIVEN_ERROR_MESSAGE, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestValidate_OwnerRequiresObjectives_EmptyObjectiveIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            officeSettings.IsObjectiveRequired = true;
            objectiveIds.Clear();
            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NO_OBJECTIVES_GIVEN_ERROR_MESSAGE, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestValidate_OwnerRequiresObjectives_NullObjectiveIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            officeSettings.IsObjectiveRequired = true;
            objectiveIds.Clear();
            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);
            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NO_OBJECTIVES_GIVEN_ERROR_MESSAGE, validationResult.ErrorMessage);
        }

        [TestMethod]
        public void TestValidate_ParentProgramIdHasValue_ParentProgramIsNull()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, null);
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
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, null, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

        }

        [TestMethod]
        public void TestValidate_NullContactIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            contactIds = null;
            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);

            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NO_POINTS_OF_CONTACT_GIVEN_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_NoContactIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            contactIds = new List<int>();
            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);

            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NO_POINTS_OF_CONTACT_GIVEN_ERROR_MESSAGE, validationResult.ErrorMessage);

        }


        [TestMethod]
        public void TestValidate_NullThemeIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            themeIds = null;
            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);

            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NO_THEMES_GIVEN_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_NoThemeIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            themeIds = new List<int>();
            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);

            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NO_THEMES_GIVEN_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_NullGoalIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            goalIds = null;
            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);

            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NO_GOALS_GIVEN_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_NoGoalIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            goalIds = new List<int>();
            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);

            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NO_GOALS_GIVEN_ERROR_MESSAGE, validationResult.ErrorMessage);

        }


        [TestMethod]
        public void TestValidate_NullRegionIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            regionIds = null;
            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);

            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NO_REGIONS_GIVEN_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestValidate_NoRegionIds()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.Validate(entity).ToList();
            Assert.AreEqual(0, results.Count);

            regionIds = new List<int>();
            entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            results = validator.Validate(entity).ToList();
            Assert.AreEqual(1, results.Count);

            var validationResult = results.First();
            Assert.AreEqual(ProgramServiceValidator.NO_REGIONS_GIVEN_ERROR_MESSAGE, validationResult.ErrorMessage);

        }

        [TestMethod]
        public void TestDoValidateCreate()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(0, results.Count);

        }

        [TestMethod]
        public void TestDoValidateUpdate()
        {
            var validator = new ProgramServiceValidator();
            var locationTypeIds = new List<int>();
            var owner = new Organization();
            var parentProgramId = 1;
            var parentProgram = new Program();
            var name = "name";
            var description = "description";
            var contactIds = new List<int> { 1 };
            var themeIds = new List<int> { 1 };
            var goalIds = new List<int> { 1 };
            var regionIds = new List<int> { 1 };
            var categoryIds = new List<int> { 1 };
            var objectiveIds = new List<int> { 1 };
            var officeSettings = new OfficeSettings();

            var entity = new ProgramServiceValidationEntity(name, description, locationTypeIds, contactIds, themeIds, goalIds, regionIds, categoryIds, objectiveIds, owner, officeSettings, parentProgramId, parentProgram);
            var results = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, results.Count);

        }
    }
}
