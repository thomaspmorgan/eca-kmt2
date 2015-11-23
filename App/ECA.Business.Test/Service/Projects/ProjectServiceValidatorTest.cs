using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Projects;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECA.Business.Test.Service.Projects
{
    [TestClass]
    public class ProjectServiceValidatorTest
    {
        private ProjectServiceValidator validator;

        [TestInitialize]
        public void TestInit()
        {
            validator = new ProjectServiceValidator();
        }
        #region Create
        [TestMethod]
        public void TestDoCreate_NullName()
        {
            var name = "name";
            var description = "description";
            var program = new Program();
            Func<ProjectServiceCreateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceCreateValidationEntity(
                name: name,
                description: description,
                program: program
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            name = null;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_NAME_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Name", validationErrors.First().Property);

        }

        [TestMethod]
        public void TestDoCreate_EmptyName()
        {
            var name = "name";
            var description = "description";
            var program = new Program();
            Func<ProjectServiceCreateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceCreateValidationEntity(
                name: name,
                description: description,
                program: program
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            name = String.Empty;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_NAME_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Name", validationErrors.First().Property);

        }

        [TestMethod]
        public void TestDoCreate_WhitespaceName()
        {
            var name = "name";
            var description = "description";
            var program = new Program();
            Func<ProjectServiceCreateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceCreateValidationEntity(
                name: name,
                description: description,
                program: program
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            name = " ";
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_NAME_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Name", validationErrors.First().Property);

        }

        [TestMethod]
        public void TestDoCreate_NullDescription()
        {
            var name = "name";
            var description = "description";
            var program = new Program();
            Func<ProjectServiceCreateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceCreateValidationEntity(
                name: name,
                description: description,
                program: program
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            description = null;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);

        }

        [TestMethod]
        public void TestDoCreate_EmptyDescription()
        {
            var name = "name";
            var description = "description";
            var program = new Program();
            Func<ProjectServiceCreateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceCreateValidationEntity(
                name: name,
                description: description,
                program: program
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            description = String.Empty;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);

        }

        [TestMethod]
        public void TestDoCreate_WhitespaceDescription()
        {
            var name = "name";
            var description = "description";
            var program = new Program();
            Func<ProjectServiceCreateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceCreateValidationEntity(
                name: name,
                description: description,
                program: program
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            description = " ";
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);

        }

        [TestMethod]
        public void TestDoCreate_ProgramIsNull()
        {
            var name = "name";
            var description = "description";
            var program = new Program();
            Func<ProjectServiceCreateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceCreateValidationEntity(
                name: name,
                description: description,
                program: program
                );
            };
            Assert.AreEqual(0, validator.ValidateCreate(createEntity()).Count());

            program = null;
            var entity = createEntity();
            var validationErrors = validator.DoValidateCreate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.PROGRAM_REQUIRED_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("ProgramId", validationErrors.First().Property);

        }

        #endregion

        #region Update

        [TestMethod]
        public void TestDoValidateUpdate_NullName()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    locationIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );
            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            name = null;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_NAME_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Name", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_EmptyName()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    locationIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            name = String.Empty;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_NAME_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Name", validationErrors.First().Property);
        }


        [TestMethod]
        public void TestDoValidateUpdate_WhitespaceName()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    locationIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            name = " ";
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_NAME_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Name", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_NullDescription()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    locationIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            description = null;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_EmptyDescription()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    locationIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            description = String.Empty;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);
        }


        [TestMethod]
        public void TestDoValidateUpdate_WhitespaceDescription()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    locationIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            description = " ";
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_DESCRIPTION_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("Description", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ThemesExist()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    locationIds: null,
                    objectiveIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            themesExist = false;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.THEMES_DO_NOT_EXIST_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("ThemeIds", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_PointsOfContactExist()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    locationIds: null,
                    objectiveIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            pointsOfContactsExist = false;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.CONTACTS_DO_NOT_EXIST_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("PointsOfContactIds", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_GoalsExist()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            goalsExist = false;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.GOALS_DO_NOT_EXIST_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("GoalIds", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_CategoriesExist()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            categoriesExist = false;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.CATEGORIES_DO_NOT_EXIST_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("CategoryIds", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_LocationsExist()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            locationsExist = false;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.LOCATIONS_DO_NOT_EXIST_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("LocationIds", validationErrors.First().Property);
        }


        [TestMethod]
        public void TestDoValidateUpdate_ObjectivesExist()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var officeSettings = new OfficeSettings();
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            objectivesExist = false;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.OBJECTIVES_DO_NOT_EXIST_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("ObjectiveIds", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ZeroObjectivesGiven_ObjectiveRequired()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            officeSettings.IsObjectiveRequired = true;
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            numberOfObjectives = 0;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.OBJECTIVES_REQUIRED_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("ObjectiveIds", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_LessThanMinCategories_CategoryRequied()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            officeSettings.IsCategoryRequired = false;
            officeSettings.MinimumRequiredFoci = 0;
            officeSettings.MaximumRequiredFoci = 10;
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());
            officeSettings.IsCategoryRequired = true;
            numberOfCategories = -1;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(String.Format(ProjectServiceValidator.MIN_CATEGORIES_REQUIRED_ERROR_MESSAGE, officeSettings.MinimumRequiredFoci), validationErrors.First().ErrorMessage);
            Assert.AreEqual("CategoryIds", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_LessThanMinCategories_CategoryNotRequied()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            officeSettings.IsCategoryRequired = false;
            officeSettings.MinimumRequiredFoci = 0;
            officeSettings.MaximumRequiredFoci = 10;
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());
            officeSettings.IsCategoryRequired = false;
            numberOfCategories = -1;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }


        [TestMethod]
        public void TestDoValidateUpdate_ExceedsMaxCategories_CategoryRequired()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            officeSettings.IsCategoryRequired = true;
            officeSettings.MaximumRequiredFoci = 1;
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            numberOfCategories = 2;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(String.Format(ProjectServiceValidator.MAX_CATEGORIES_REQUIRED_ERROR_MESSAGE, officeSettings.MaximumRequiredFoci), validationErrors.First().ErrorMessage);
            Assert.AreEqual("CategoryIds", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ExceedsMaxCategories_CategoryNotRequired()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            officeSettings.IsCategoryRequired = true;
            officeSettings.MaximumRequiredFoci = 1;
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            officeSettings.IsCategoryRequired = false;
            numberOfCategories = 0;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ZeroObjectivesGiven_ObjectiveNotRequired()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            officeSettings.IsObjectiveRequired = false;
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            numberOfObjectives = 0;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ZeroCategoriesGiven_CategoryNotRequired()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            officeSettings.IsCategoryRequired = false;
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            numberOfCategories = 0;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }

        [TestMethod]
        public void TestDoValidateUpdate_OneCategoryGiven_CategoryRequired()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            numberOfCategories = 1;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }

        [TestMethod]
        public void TestDoValidateUpdate_OneObjectiveGiven()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            numberOfObjectives = 1;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }

        [TestMethod]
        public void TestDoValidateUpdate_MoreThanOneCategoryGiven()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            numberOfCategories = 2;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }

        [TestMethod]
        public void TestDoValidateUpdate_MoreThanOneObjectiveGiven()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            numberOfObjectives = 2;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }


        [TestMethod]
        public void TestDoValidateUpdate_StartDateIsEqualEndDate()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            startDate = endDate;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_START_AND_END_DATE_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("StartDate", validationErrors.First().Property);
        }


        [TestMethod]
        public void TestDoValidateUpdate_StartDateIsAfterEndDate()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Completed.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Completed.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            startDate = endDate.AddDays(1.0);
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_START_AND_END_DATE_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("StartDate", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_OriginalProjectIsDraftState_UpdatedProjectIsDraftState()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Draft.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Draft.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());
        }

        [TestMethod]
        public void TestDoValidateUpdate_OriginalProjectIsNotDraftState_UpdatedProjectIsDraftState()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Active.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Active.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int>();
            var allowedObjectiveIds = new List<int>();
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: null,
                    objectiveIds: null,
                    locationIds: null,
                    regionIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            statusId = ProjectStatus.Draft.Id;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.CAN_NOT_SET_PROJECT_BACK_TO_DRAFT_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("ProjectStatusId", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_CategoryIsNotAllowed()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Active.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Active.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int> { 1 };
            var allowedObjectiveIds = new List<int> { 1 };
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: new List<int> { 1 },
                    objectiveIds: new List<int> { 1 },
                    regionIds: new List<int>(),
                    locationIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );
            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            allowedCategoryIds.Clear();
            entity = createEntity();

            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_CATEGORIES_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("CategoryIds", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ObjectiveIsNotAllowed()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Active.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Active.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int> { 1 };
            var allowedObjectiveIds = new List<int> { 1 };
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: new List<int> { 1 },
                    objectiveIds: new List<int> { 1 },
                    regionIds: new List<int>(),
                    locationIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            allowedObjectiveIds.Clear();
            entity = createEntity();

            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INVALID_OBJECTIVES_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("ObjectiveIds", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_NewInactiveLocation()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Active.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Active.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int> { 1 };
            var allowedObjectiveIds = new List<int> { 1 };
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: new List<int> { 1 },
                    objectiveIds: new List<int> { 1 },
                    regionIds: new List<int>(),
                    locationIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            newInactiveLocationIds.Add(1);
            entity = createEntity();

            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.INACTIVE_LOCATIONS_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("LocationIds", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_TwoRegionLocationTypeIds()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Active.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Active.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int> { 1 };
            var allowedObjectiveIds = new List<int> { 1 };
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: new List<int> { 1 },
                    objectiveIds: new List<int> { 1 },
                    regionIds: new List<int>(),
                    locationIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            regionLocationTypeIds.Add(1);
            regionLocationTypeIds.Add(2);
            entity = createEntity();

            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.REGION_IS_NOT_A_REGION_LOCATION_TYPE_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("RegionIds", validationErrors.First().Property);
        }

        [TestMethod]
        public void TestDoValidateUpdate_OneRegionLocationTypeIds()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Active.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Active.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int> { 1 };
            var allowedObjectiveIds = new List<int> { 1 };
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: new List<int> { 1 },
                    objectiveIds: new List<int> { 1 },
                    regionIds: new List<int>(),
                    locationIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            regionLocationTypeIds.Add(1);
            regionLocationTypeIds.Add(2);

            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validator.DoValidateUpdate(entity).Count());
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.REGION_IS_NOT_A_REGION_LOCATION_TYPE_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("RegionIds", validationErrors.First().Property);

            regionLocationTypeIds.Clear();
            entity = createEntity();

            validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }

        [TestMethod]
        public void TestDoValidateUpdate_ZeroRegionLocationTypeIds()
        {
            var user = new User(1);
            var name = "name";
            var description = "desc";
            var statusId = ProjectStatus.Active.Id;
            var startDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var endDate = DateTimeOffset.UtcNow;
            var projectToUpdate = new Project
            {
                ProjectStatusId = ProjectStatus.Active.Id,
            };
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
            var categoriesExist = true;
            var objectivesExist = true;
            var locationsExist = true;
            var numberOfCategories = 1;
            var numberOfObjectives = 1;
            var allowedCategoryIds = new List<int> { 1 };
            var allowedObjectiveIds = new List<int> { 1 };
            var newInactiveLocationIds = new List<int>();
            var regionLocationTypeIds = new List<int>();
            var officeSettings = new OfficeSettings();
            Func<PublishedProject> createUpdatedProject = () =>
            {
                return new PublishedProject(
                    updatedBy: user,
                    projectId: 1,
                    name: name,
                    description: description,
                    projectStatusId: statusId,
                    goalIds: null,
                    themeIds: null,
                    pointsOfContactIds: null,
                    categoryIds: new List<int> { 1 },
                    objectiveIds: new List<int> { 1 },
                    regionIds: new List<int>(),
                    locationIds: null,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist,
                  categoriesExist: categoriesExist,
                  objectivesExist: objectivesExist,
                  locationsExist: locationsExist,
                  numberOfCategories: numberOfCategories,
                  numberOfObjectives: numberOfObjectives,
                  allowedCategoryIds: allowedCategoryIds,
                  allowedObjectiveIds: allowedObjectiveIds,
                  officeSettings: officeSettings,
                  regionLocationTypeIds: regionLocationTypeIds,
                  newInactiveLocations: newInactiveLocationIds
                );

            };

            var entity = createEntity();
            regionLocationTypeIds.Add(1);
            regionLocationTypeIds.Add(2);
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.REGION_IS_NOT_A_REGION_LOCATION_TYPE_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("RegionIds", validationErrors.First().Property);

            regionLocationTypeIds.Clear();
            entity = createEntity();

            validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(0, validationErrors.Count);
        }
        #endregion
    }
}
