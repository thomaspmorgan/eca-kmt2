using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using ECA.Business.Service;
using ECA.Data;

namespace ECA.Business.Test.Service.Admin
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
            Func<ProjectServiceCreateValidationEntity> createEntity = () => {
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
            var focus = new Focus
            {

            };
            var goalsExist = true;
            var themesExist = true;
            var pointsOfContactsExist = true;
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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
        public void TestDoValidateUpdate_FocusIsNull()
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
                );

            };

            var entity = createEntity();
            Assert.AreEqual(0, validator.DoValidateUpdate(entity).Count());

            focus = null;
            entity = createEntity();
            var validationErrors = validator.DoValidateUpdate(entity).ToList();
            Assert.AreEqual(1, validationErrors.Count);
            Assert.AreEqual(ProjectServiceValidator.FOCUS_REQUIRED_ERROR_MESSAGE, validationErrors.First().ErrorMessage);
            Assert.AreEqual("FocusId", validationErrors.First().Property);
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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
                    focusId: 1,
                    startDate: startDate,
                    endDate: endDate
                    );
            };
            Func<ProjectServiceUpdateValidationEntity> createEntity = () =>
            {
                return new ProjectServiceUpdateValidationEntity(
                  updatedProject: createUpdatedProject(),
                  projectToUpdate: projectToUpdate,
                  focus: focus,
                  goalsExist: goalsExist,
                  themesExist: themesExist,
                  pointsOfContactExist: pointsOfContactsExist
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

        #endregion
    }
}
