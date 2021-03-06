﻿using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Projects;
using ECA.Business.Validation;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Projects
{
    public class NotSupportedAdditonalProjectParticipant : AdditionalProjectParticipant
    {
        public NotSupportedAdditonalProjectParticipant(User projectOwner, int projectId, int participantTypeId)
            : base(projectOwner, projectId, participantTypeId)
        {

        }

        protected override void UpdateParticipantDetails(Participant participant, VisitorType visitorType, DefaultExchangeVisitorFunding defaultExchangeVisitorFunding)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class ProjectServiceTest
    {
        private TestEcaContext context;
        private ProjectService service;
        private Mock<IBusinessValidator<ProjectServiceCreateValidationEntity, ProjectServiceUpdateValidationEntity>> validator;
        private Mock<IOfficeService> officeService;
        private Action<AdditionalPersonProjectParticipant, Participant, User> addAdditionalPersonProjectParticipantTester;
        private Action<AdditionalOrganizationProjectParticipant, Participant, User> addAdditionalOrganizationProjectParticipantTester;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            validator = new Mock<IBusinessValidator<ProjectServiceCreateValidationEntity, ProjectServiceUpdateValidationEntity>>();
            officeService = new Mock<IOfficeService>();
            service = new ProjectService(context, officeService.Object, validator.Object);
            addAdditionalPersonProjectParticipantTester = (personParticipant, addedParticipant, projectOwner) =>
            {
                Assert.IsFalse(addedParticipant.OrganizationId.HasValue);
                Assert.AreEqual(personParticipant.PersonId, addedParticipant.PersonId);
                Assert.AreEqual(projectOwner.Id, addedParticipant.History.CreatedBy);
                Assert.AreEqual(projectOwner.Id, addedParticipant.History.RevisedBy);
                Assert.AreEqual(personParticipant.ParticipantTypeId, addedParticipant.ParticipantTypeId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedParticipant.History.CreatedOn, 2000);
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedParticipant.History.RevisedOn, 2000);
                Assert.AreEqual(personParticipant.ProjectId, addedParticipant.ProjectId);
            };

            addAdditionalOrganizationProjectParticipantTester = (organizationParticipant, addedParticipant, projectOwner) =>
            {
                Assert.IsFalse(addedParticipant.PersonId.HasValue);
                Assert.AreEqual(organizationParticipant.OrganizationId, addedParticipant.OrganizationId);
                Assert.AreEqual(projectOwner.Id, addedParticipant.History.CreatedBy);
                Assert.AreEqual(projectOwner.Id, addedParticipant.History.RevisedBy);
                Assert.AreEqual(organizationParticipant.ParticipantTypeId, addedParticipant.ParticipantTypeId);
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedParticipant.History.CreatedOn, 2000);
                DateTimeOffset.UtcNow.Should().BeCloseTo(addedParticipant.History.RevisedOn, 2000);
                Assert.AreEqual(organizationParticipant.ProjectId, addedParticipant.ProjectId);
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        #region Create Draft Project
        [TestMethod]
        public async Task TestCreate_CheckProperties()
        {
            var program = new Program
            {
                ProgramId = 1
            };

            context.Programs.Add(program);

            User user = new User(1);
            var name = "name";
            var description = "description";
            var draftProject = new DraftProject(user, name, description, program.ProgramId);

            Action<Project> tester = (project) =>
            {
                Assert.IsNotNull(project);
                Assert.AreEqual(name, project.Name);
                Assert.AreEqual(description, project.Description);
                DateTimeOffset.UtcNow.Should().BeCloseTo(project.StartDate, DbContextHelper.DATE_PRECISION);
                Assert.AreEqual(program.ProgramId, project.ProgramId);
                Assert.AreEqual(VisitorType.NotApplicable.Id, project.VisitorTypeId);
            };

            var createdProject = service.Create(draftProject);
            var createdProjectAsync = await service.CreateAsync(draftProject);
            validator.Verify(x => x.ValidateCreate(It.IsAny<ProjectServiceCreateValidationEntity>()), Times.Exactly(2));
            tester(createdProject);
            tester(createdProjectAsync);
        }

        [TestMethod]
        public async Task TestCreate_CheckThemes()
        {
            var theme = new Theme
            {
                ThemeId = 1,
                ThemeName = "themeName"
            };

            var program = new Program
            {
                ProgramId = 1,
            };

            program.Themes.Add(theme);

            context.Themes.Add(theme);
            context.Programs.Add(program);

            var draftProject = new DraftProject(new User(1), "name", "description", program.ProgramId);

            Action<Project> tester = (project) =>
            {
                Assert.IsNotNull(project);
                CollectionAssert.AreEqual(context.Themes.Select(x => x.ThemeId).ToList(), project.Themes.Select(x => x.ThemeId).ToList());
            };

            var createdProject = service.Create(draftProject);
            var createdProjectAsync = await service.CreateAsync(draftProject);

            tester(createdProject);
            tester(createdProjectAsync);
        }

        [TestMethod]
        public async Task TestCreate_CheckGoals()
        {
            var goal = new Goal
            {
                GoalId = 1,
                GoalName = "goalName"
            };

            var program = new Program
            {
                ProgramId = 1,
            };

            program.Goals.Add(goal);

            context.Goals.Add(goal);
            context.Programs.Add(program);

            var draftProject = new DraftProject(new User(1), "name", "description", program.ProgramId);

            Action<Project> tester = (project) =>
            {
                Assert.IsNotNull(project);
                CollectionAssert.AreEqual(context.Goals.Select(x => x.GoalId).ToList(), project.Goals.Select(x => x.GoalId).ToList());
            };

            var createdProject = service.Create(draftProject);
            var createdProjectAsync = await service.CreateAsync(draftProject);
            tester(createdProject);
            tester(createdProjectAsync);
        }

        [TestMethod]
        public async Task TestCreate_CheckContacts()
        {
            var contact = new Contact
            {
                ContactId = 1,
                FullName = "fullName"
            };

            var program = new Program
            {
                ProgramId = 1,
            };

            program.Contacts.Add(contact);

            context.Contacts.Add(contact);
            context.Programs.Add(program);

            var draftProject = new DraftProject(new User(1), "name", "description", program.ProgramId);

            Action<Project> tester = (project) =>
            {
                Assert.IsNotNull(project);
                Assert.AreEqual(0, project.Contacts.Count);
            };

            var createdProject = service.Create(draftProject);
            var createdProjectAsync = await service.CreateAsync(draftProject);

            tester(createdProject);
            tester(createdProjectAsync);
        }

        [TestMethod]
        public async Task TestCreate_CheckCategories()
        {
            var category = new Category
            {
                CategoryId = 1,
                CategoryName = "name"
            };

            var program = new Program
            {
                ProgramId = 1,
            };

            program.Categories.Add(category);

            context.Categories.Add(category);
            context.Programs.Add(program);

            var draftProject = new DraftProject(new User(1), "name", "description", program.ProgramId);

            Action<Project> tester = (project) =>
            {
                Assert.IsNotNull(project);
                Assert.AreEqual(0, project.Categories.Count);
            };

            var createdProject = service.Create(draftProject);
            var createdProjectAsync = await service.CreateAsync(draftProject);
            tester(createdProject);
            tester(createdProjectAsync);
        }

        [TestMethod]
        public async Task TestCreate_CheckObjectives()
        {
            var objective = new Objective
            {
                ObjectiveId = 1,
                ObjectiveName = "name"
            };

            var program = new Program
            {
                ProgramId = 1,
            };

            program.Objectives.Add(objective);

            context.Objectives.Add(objective);
            context.Programs.Add(program);

            var draftProject = new DraftProject(new User(1), "name", "description", program.ProgramId);

            Action<Project> tester = (project) =>
            {
                Assert.IsNotNull(project);
                Assert.AreEqual(0, project.Objectives.Count);
            };

            var createdProject = service.Create(draftProject);
            var createdProjectAsync = await service.CreateAsync(draftProject);
            tester(createdProject);
            tester(createdProjectAsync);
        }

        [TestMethod]
        public async Task TestCreate_CheckAudit()
        {
            var program = new Program
            {
                ProgramId = 1,
            };

            context.Programs.Add(program);

            var draftProject = new DraftProject(new User(1), "name", "description", program.ProgramId);

            Action<Project> tester = (project) =>
            {
                Assert.IsNotNull(project);
                Assert.AreEqual(1, project.History.CreatedBy);
                Assert.AreEqual(1, project.History.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(project.History.CreatedOn, DbContextHelper.DATE_PRECISION);
                DateTimeOffset.UtcNow.Should().BeCloseTo(project.History.RevisedOn, DbContextHelper.DATE_PRECISION);
            };

            var createdProject = service.Create(draftProject);
            var createdProjectAsync = await service.CreateAsync(draftProject);

            tester(createdProject);
            tester(createdProjectAsync);
        }
        #endregion

        #region Get Projects
        [TestMethod]
        public async Task TestGetProjects_CheckProperties()
        {
            var now = DateTimeOffset.UtcNow;

            var program = new Program
            {
                ProgramId = 1,
                Name = "program"
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                StartDate = now,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            context.ProjectStatuses.Add(status);
            context.Projects.Add(project);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();

                Assert.AreEqual(program.Name, firstResult.ProgramName);
                Assert.AreEqual(program.ProgramId, firstResult.ProgramId);
                Assert.AreEqual(project.Name, firstResult.ProjectName);
                Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
                Assert.AreEqual(project.StartDate, firstResult.StartDate);
                Assert.AreEqual(project.StartDate.Year, firstResult.StartYear);
                Assert.AreEqual(project.StartDate.Year.ToString(), firstResult.StartYearAsString);
                Assert.AreEqual(status.Status, firstResult.ProjectStatusName);
                Assert.AreEqual(status.ProjectStatusId, firstResult.ProjectStatusId);
            };

            var serviceResults = service.GetProjects(queryOperator);
            var serviceResultsAsync = await service.GetProjectsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjects_CheckCountries()
        {
            var now = DateTimeOffset.UtcNow;
            var countryType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };
            var region = new Location
            {
                LocationId = 1,
                LocationName = "region",

            };
            var country1 = new Location
            {
                LocationId = 2,
                LocationName = "country1",
                LocationTypeId = countryType.LocationTypeId,
                LocationType = countryType
            };

            var country2 = new Location
            {
                LocationId = 4,
                LocationName = "country2",
                LocationTypeId = countryType.LocationTypeId,
                LocationType = countryType
            };
            country1.Region = region;
            country1.RegionId = region.LocationId;
            country2.Region = region;
            country2.RegionId = region.LocationId;

            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                StartDate = now,
                Status = status,
                ProjectStatusId = status.ProjectStatusId, 
                VisitorType = visitorType
            };
            project.Regions.Add(region);
            context.ProjectStatuses.Add(status);
            context.Projects.Add(project);
            context.Locations.Add(country1);
            context.Locations.Add(country2);
            context.Locations.Add(region);
            context.Programs.Add(program);
            context.LocationTypes.Add(countryType);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(2, firstResult.CountryIds.Count());
                Assert.AreEqual(2, firstResult.CountryNames.Count());
                Assert.IsTrue(firstResult.CountryIds.Contains(country1.LocationId));
                Assert.IsTrue(firstResult.CountryIds.Contains(country2.LocationId));
                Assert.IsTrue(firstResult.CountryNames.Contains(country1.LocationName));
                Assert.IsTrue(firstResult.CountryNames.Contains(country2.LocationName));
            };

            var serviceResults = service.GetProjects(queryOperator);
            var serviceResultsAsync = await service.GetProjectsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjects_CheckRegions()
        {
            var now = DateTimeOffset.UtcNow;
            var countryType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };
            var region1 = new Location
            {
                LocationId = 1,
                LocationName = "region1",

            };
            var region2 = new Location
            {
                LocationId = 2,
                LocationName = "region2",

            };

            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                StartDate = now,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project.Regions.Add(region1);
            project.Regions.Add(region2);
            context.ProjectStatuses.Add(status);
            context.Projects.Add(project);
            context.Locations.Add(region1);
            context.Locations.Add(region2);
            context.Programs.Add(program);
            context.LocationTypes.Add(countryType);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(2, firstResult.RegionIds.Count());
                Assert.AreEqual(2, firstResult.RegionNames.Count());
                Assert.IsTrue(firstResult.RegionIds.Contains(region1.LocationId));
                Assert.IsTrue(firstResult.RegionIds.Contains(region2.LocationId));
                Assert.IsTrue(firstResult.RegionNames.Contains(region1.LocationName));
                Assert.IsTrue(firstResult.RegionNames.Contains(region2.LocationName));
            };

            var serviceResults = service.GetProjects(queryOperator);
            var serviceResultsAsync = await service.GetProjectsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjects_DefaultSorterOnly()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };

            project.Locations = new List<Location>();
            project.Locations.Add(location1);
            project.Locations.Add(location2);

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
            };

            var serviceResults = service.GetProjects(queryOperator);
            var serviceResultsAsync = await service.GetProjectsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjects_Paging()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project1 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project1",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project1.Locations = new List<Location>();
            project1.Locations.Add(location1);
            project1.Locations.Add(location2);


            var project2 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project2",
                ParentProgram = program,
                ProjectId = 200,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project2.Locations = new List<Location>();

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project1);
            context.Projects.Add(project2);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 1;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(2, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(project1.ProjectId, firstResult.ProjectId);
            };

            var serviceResults = service.GetProjects(queryOperator);
            var serviceResultsAsync = await service.GetProjectsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjects_Filter()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project1 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project1",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project1.Locations = new List<Location>();
            project1.Locations.Add(location1);
            project1.Locations.Add(location2);

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project1);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 1;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<SimpleProjectDTO>(x => x.ProjectName, ComparisonType.Like, project1.Name));

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(project1.ProjectId, firstResult.ProjectId);
            };

            var serviceResults = service.GetProjects(queryOperator);
            var serviceResultsAsync = await service.GetProjectsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjects_Sorted()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project1 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project1",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project1.Locations = new List<Location>();
            project1.Locations.Add(location1);
            project1.Locations.Add(location2);


            var project2 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project2",
                ParentProgram = program,
                ProjectId = 200,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project2.Locations = new List<Location>();

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project1);
            context.Projects.Add(project2);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 1;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Descending));

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(2, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(project2.ProjectId, firstResult.ProjectId);
            };

            var serviceResults = service.GetProjects(queryOperator);
            var serviceResultsAsync = await service.GetProjectsAsync(queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }



        #endregion

        #region Get Projects By Program Id
        [TestMethod]
        public async Task TestGetProjectsByProgramId_CheckProperties()
        {
            var now = DateTimeOffset.UtcNow;

            var program = new Program
            {
                ProgramId = 1,
                Name = "program"
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                StartDate = now,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            context.ProjectStatuses.Add(status);
            context.Projects.Add(project);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();

                Assert.AreEqual(program.Name, firstResult.ProgramName);
                Assert.AreEqual(program.ProgramId, firstResult.ProgramId);
                Assert.AreEqual(project.Name, firstResult.ProjectName);
                Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
                Assert.AreEqual(project.StartDate, firstResult.StartDate);
                Assert.AreEqual(project.StartDate.Year, firstResult.StartYear);
                Assert.AreEqual(project.StartDate.Year.ToString(), firstResult.StartYearAsString);
                Assert.AreEqual(status.Status, firstResult.ProjectStatusName);
                Assert.AreEqual(status.ProjectStatusId, firstResult.ProjectStatusId);
            };

            var serviceResults = service.GetProjectsByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramId_CheckCountries()
        {
            var now = DateTimeOffset.UtcNow;
            var countryType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };
            var region = new Location
            {
                LocationId = 1,
                LocationName = "region",

            };
            var country1 = new Location
            {
                LocationId = 2,
                LocationName = "country1",
                LocationTypeId = countryType.LocationTypeId,
                LocationType = countryType
            };

            var country2 = new Location
            {
                LocationId = 4,
                LocationName = "country2",
                LocationTypeId = countryType.LocationTypeId,
                LocationType = countryType
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            country1.Region = region;
            country1.RegionId = region.LocationId;
            country2.Region = region;
            country2.RegionId = region.LocationId;

            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                StartDate = now,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project.Regions.Add(region);
            context.ProjectStatuses.Add(status);
            context.Projects.Add(project);
            context.Locations.Add(country1);
            context.Locations.Add(country2);
            context.Locations.Add(region);
            context.Programs.Add(program);
            context.LocationTypes.Add(countryType);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(2, firstResult.CountryIds.Count());
                Assert.AreEqual(2, firstResult.CountryNames.Count());
                Assert.IsTrue(firstResult.CountryIds.Contains(country1.LocationId));
                Assert.IsTrue(firstResult.CountryIds.Contains(country2.LocationId));
                Assert.IsTrue(firstResult.CountryNames.Contains(country1.LocationName));
                Assert.IsTrue(firstResult.CountryNames.Contains(country2.LocationName));
            };

            var serviceResults = service.GetProjectsByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramId_CheckRegions()
        {
            var now = DateTimeOffset.UtcNow;
            var countryType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };
            var region1 = new Location
            {
                LocationId = 1,
                LocationName = "region1",

            };
            var region2 = new Location
            {
                LocationId = 2,
                LocationName = "region2",

            };

            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                StartDate = now,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project.Regions.Add(region1);
            project.Regions.Add(region2);
            context.ProjectStatuses.Add(status);
            context.Projects.Add(project);
            context.Locations.Add(region1);
            context.Locations.Add(region2);
            context.Programs.Add(program);
            context.LocationTypes.Add(countryType);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(2, firstResult.RegionIds.Count());
                Assert.AreEqual(2, firstResult.RegionNames.Count());
                Assert.IsTrue(firstResult.RegionIds.Contains(region1.LocationId));
                Assert.IsTrue(firstResult.RegionIds.Contains(region2.LocationId));
                Assert.IsTrue(firstResult.RegionNames.Contains(region1.LocationName));
                Assert.IsTrue(firstResult.RegionNames.Contains(region2.LocationName));
            };

            var serviceResults = service.GetProjectsByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramId_NoProgramsWithGivenId()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType  = visitorType
            };

            project.Locations = new List<Location>();
            project.Locations.Add(location1);
            project.Locations.Add(location2);

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(0, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(0, results.Count);
            };

            var otherProgramId = -1;
            var serviceResults = service.GetProjectsByProgramId(otherProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(otherProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramId_DefaultSorterOnly()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };

            project.Locations = new List<Location>();
            project.Locations.Add(location1);
            project.Locations.Add(location2);

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 10;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
            };

            var serviceResults = service.GetProjectsByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramId_Paging()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project1 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project1",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project1.Locations = new List<Location>();
            project1.Locations.Add(location1);
            project1.Locations.Add(location2);


            var project2 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project2",
                ParentProgram = program,
                ProjectId = 200,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project2.Locations = new List<Location>();

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project1);
            context.Projects.Add(project2);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 1;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(2, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(project1.ProjectId, firstResult.ProjectId);
            };

            var serviceResults = service.GetProjectsByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramId_Filter()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project1 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project1",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project1.Locations = new List<Location>();
            project1.Locations.Add(location1);
            project1.Locations.Add(location2);

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project1);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 1;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);
            queryOperator.Filters.Add(new ExpressionFilter<SimpleProjectDTO>(x => x.ProjectName, ComparisonType.Like, project1.Name));

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(project1.ProjectId, firstResult.ProjectId);
            };

            var serviceResults = service.GetProjectsByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByProgramId_Sorted()
        {
            var location1 = new Location
            {
                LocationId = 1,
                LocationName = "location1"
            };
            var location2 = new Location
            {
                LocationId = 2,
                LocationName = "location2"
            };
            var program = new Program
            {
                ProgramId = 1,
            };
            var status = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project1 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project1",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project1.Locations = new List<Location>();
            project1.Locations.Add(location1);
            project1.Locations.Add(location2);


            var project2 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project2",
                ParentProgram = program,
                ProjectId = 200,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                VisitorType = visitorType
            };
            project2.Locations = new List<Location>();

            context.ProjectStatuses.Add(status);
            context.Projects.Add(project1);
            context.Projects.Add(project2);
            context.Locations.Add(location1);
            context.Locations.Add(location2);
            context.Programs.Add(program);

            var defaultSorter = new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Ascending);
            var start = 0;
            var limit = 1;
            var queryOperator = new QueryableOperator<SimpleProjectDTO>(start, limit, defaultSorter);
            queryOperator.Sorters.Add(new ExpressionSorter<SimpleProjectDTO>(x => x.ProjectId, SortDirection.Descending));

            Action<PagedQueryResults<SimpleProjectDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(2, queryResults.Total);
                var results = queryResults.Results;
                Assert.AreEqual(1, results.Count);
                var firstResult = results.First();
                Assert.AreEqual(project2.ProjectId, firstResult.ProjectId);
            };

            var serviceResults = service.GetProjectsByProgramId(program.ProgramId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByProgramIdAsync(program.ProgramId, queryOperator);
            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Get Projects By Person Id
        [TestMethod]
        public async Task TestGetProjectsByPersonIdAsync()
        {
            var person = new Person
            {
                PersonId = 1
            };

            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                StartDate = new DateTime(2013, 5, 1, 06, 32, 00),
                EndDate = new DateTime(2017, 5, 1, 06, 32, 00),
                Description = "description"
            };

            var participant = new Participant
            {
                ParticipantId = 1,
                Person = person,
                PersonId = person.PersonId,
                Project = project,
                ProjectId = project.ProjectId
            };

            context.Participants.Add(participant);

            Action<PagedQueryResults<ParticipantTimelineDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(1, queryResults.Results.Count());

                var projectResult = queryResults.Results.FirstOrDefault();

                Assert.AreEqual(project.ProjectId, projectResult.ProjectId);
                Assert.AreEqual(project.Name, projectResult.Name);
                Assert.AreEqual(project.StartDate, projectResult.StartDate);
                Assert.AreEqual(project.EndDate, projectResult.EndDate);
                Assert.AreEqual(project.Description, projectResult.Description);

                Assert.IsNull(projectResult.OfficeSymbol);
                Assert.IsNull(projectResult.Status);
            };

            var defaultSorter = new ExpressionSorter<ParticipantTimelineDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ParticipantTimelineDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetProjectsByPersonId(person.PersonId, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByPersonIdAsync(person.PersonId, queryOperator);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }

        [TestMethod]
        public async Task TestGetProjectsByPersonIdAsync_NoProjects()
        {
            Action<PagedQueryResults<ParticipantTimelineDTO>> tester = (queryResults) =>
            {
                Assert.AreEqual(0, queryResults.Results.Count());
            };

            var defaultSorter = new ExpressionSorter<ParticipantTimelineDTO>(x => x.Name, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<ParticipantTimelineDTO>(0, 10, defaultSorter);
            var serviceResults = service.GetProjectsByPersonId(1, queryOperator);
            var serviceResultsAsync = await service.GetProjectsByPersonIdAsync(1, queryOperator);

            tester(serviceResults);
            tester(serviceResultsAsync);
        }
        #endregion

        #region Get Project By Id
        [TestMethod]
        public async Task TestGetProjectById_CheckProperties()
        {
            var now = DateTimeOffset.UtcNow;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var revisedOn = DateTimeOffset.UtcNow.AddDays(-2.0);
            var createdOn = DateTimeOffset.UtcNow.AddDays(-3.0);

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };
            var owner = new Organization
            {
                OrganizationId = 20,
                Name = "owner",
                OfficeSymbol = "xyz"
            };
            var program = new Program
            {
                ProgramId = 10,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            owner.OwnerPrograms.Add(program);
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Themes = new HashSet<Theme>(),
                StartDate = yesterday,
                EndDate = now,
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>(),
                History = new History
                {
                    RevisedOn = revisedOn,
                    CreatedOn = createdOn
                },
                ProgramId = program.ProgramId,
                ParentProgram = program,
                VisitorType = visitorType
            };
            context.Organizations.Add(owner);
            context.Projects.Add(project);
            context.ProjectStatuses.Add(status);
            context.Programs.Add(program);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.AreEqual(project.ProjectId, serviceResult.Id);
                Assert.AreEqual(owner.Name, serviceResult.OwnerName);
                Assert.AreEqual(owner.OrganizationId, serviceResult.OwnerId);
                Assert.AreEqual(owner.OfficeSymbol, serviceResult.OwnerOfficeSymbol);
                Assert.AreEqual(program.Name, serviceResult.ProgramName);
                Assert.AreEqual(program.ProgramId, serviceResult.ProgramId);
                Assert.AreEqual(project.Name, serviceResult.Name);
                Assert.AreEqual(project.Description, serviceResult.Description);
                Assert.AreEqual(status.ProjectStatusId, serviceResult.ProjectStatusId);
                Assert.AreEqual(status.Status, serviceResult.Status);
                Assert.AreEqual(yesterday, serviceResult.StartDate);
                Assert.AreEqual(now, serviceResult.EndDate);
                Assert.AreEqual(revisedOn, serviceResult.RevisedOn);
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_CheckCountryIsosByLocations_LocationsAreNotCountries()
        {
            var now = DateTimeOffset.UtcNow;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var revisedOn = DateTimeOffset.UtcNow.AddDays(-2.0);
            var createdOn = DateTimeOffset.UtcNow.AddDays(-3.0);

            var countryType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id
            };
            var placeType = new LocationType
            {
                LocationTypeId = LocationType.Place.Id
            };
            var country = new Location
            {
                LocationId = 1,
                LocationName = "country",
                LocationIso = "countryIso",
                LocationIso2 = "iso2",
                LocationTypeId = countryType.LocationTypeId,
                LocationType = countryType
            };
            var projectLocation1 = new Location
            {
                LocationId = 2,
                Country = country,
                CountryId = country.LocationId,
                LocationName = "project location1",
                LocationType = placeType,
                LocationTypeId = placeType.LocationTypeId
            };
            var projectLocation2 = new Location
            {
                LocationId = 3,
                Country = country,
                CountryId = country.LocationId,
                LocationName = "project location2",
                LocationType = placeType,
                LocationTypeId = placeType.LocationTypeId
            };
            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };
            var owner = new Organization
            {
                OrganizationId = 20,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 10,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            owner.OwnerPrograms.Add(program);
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Themes = new HashSet<Theme>(),
                StartDate = yesterday,
                EndDate = now,
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>(),
                History = new History
                {
                    RevisedOn = revisedOn,
                    CreatedOn = createdOn
                },
                ProgramId = program.ProgramId,
                ParentProgram = program,
                VisitorType = visitorType
            };
            project.Locations.Add(projectLocation1);
            project.Locations.Add(projectLocation2);

            context.Organizations.Add(owner);
            context.Locations.Add(country);
            context.Locations.Add(projectLocation1);
            context.Projects.Add(project);
            context.LocationTypes.Add(countryType);
            context.LocationTypes.Add(placeType);
            context.ProjectStatuses.Add(status);
            context.Programs.Add(program);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(1, serviceResult.CountryIsosByLocations.Count());
                Assert.AreEqual(country.LocationIso, serviceResult.CountryIsosByLocations.First().Value);
                Assert.AreEqual(country.LocationId, serviceResult.CountryIsosByLocations.First().Id);
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_CheckCountryIsosByLocations_LocationsAreCountries()
        {
            var now = DateTimeOffset.UtcNow;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var revisedOn = DateTimeOffset.UtcNow.AddDays(-2.0);
            var createdOn = DateTimeOffset.UtcNow.AddDays(-3.0);

            var countryType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id
            };
            var placeType = new LocationType
            {
                LocationTypeId = LocationType.Place.Id
            };
            var country1 = new Location
            {
                LocationId = 1,
                LocationName = "country1",
                LocationIso = "countryIso1",
                LocationIso2 = "iso2-1",
                LocationTypeId = countryType.LocationTypeId,
                LocationType = countryType
            };
            var country2 = new Location
            {
                LocationId = 2,
                LocationName = "country2",
                LocationIso = "countryIso2",
                LocationIso2 = "iso2-2",
                LocationTypeId = countryType.LocationTypeId,
                LocationType = countryType
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };
            var owner = new Organization
            {
                OrganizationId = 20,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 10,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            owner.OwnerPrograms.Add(program);
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Themes = new HashSet<Theme>(),
                StartDate = yesterday,
                EndDate = now,
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>(),
                History = new History
                {
                    RevisedOn = revisedOn,
                    CreatedOn = createdOn
                },
                ProgramId = program.ProgramId,
                ParentProgram = program,
                VisitorType = visitorType
            };
            project.Locations.Add(country1);
            project.Locations.Add(country2);

            context.Organizations.Add(owner);
            context.Locations.Add(country1);
            context.Locations.Add(country2);
            context.Projects.Add(project);
            context.LocationTypes.Add(countryType);
            context.LocationTypes.Add(placeType);
            context.ProjectStatuses.Add(status);
            context.Programs.Add(program);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(2, serviceResult.CountryIsosByLocations.Count());
                Assert.AreEqual(country1.LocationIso, serviceResult.CountryIsosByLocations.First().Value);
                Assert.AreEqual(country1.LocationId, serviceResult.CountryIsosByLocations.First().Id);
                Assert.AreEqual(country2.LocationIso, serviceResult.CountryIsosByLocations.Last().Value);
                Assert.AreEqual(country2.LocationId, serviceResult.CountryIsosByLocations.Last().Id);
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_CheckCountryIsosByLocations_CheckRegions()
        {
            var now = DateTimeOffset.UtcNow;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var revisedOn = DateTimeOffset.UtcNow.AddDays(-2.0);
            var createdOn = DateTimeOffset.UtcNow.AddDays(-3.0);

            var countryType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id
            };
            var regionType = new LocationType
            {
                LocationTypeId = LocationType.Region.Id
            };
            var region = new Location
            {
                LocationId = 1,
                LocationName = "region",
                LocationIso = "regionIso",
                LocationIso2 = "regionIso2",
                LocationType = regionType,
                LocationTypeId = regionType.LocationTypeId

            };
            var country1 = new Location
            {
                LocationId = 2,
                LocationName = "country1",
                LocationIso = "countryIso1",
                LocationIso2 = "iso2-1",
                LocationTypeId = countryType.LocationTypeId,
                LocationType = countryType,
                Region = region,
                RegionId = region.LocationId
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };
            var owner = new Organization
            {
                OrganizationId = 20,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 10,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            owner.OwnerPrograms.Add(program);
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Themes = new HashSet<Theme>(),
                StartDate = yesterday,
                EndDate = now,
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>(),
                History = new History
                {
                    RevisedOn = revisedOn,
                    CreatedOn = createdOn
                },
                ProgramId = program.ProgramId,
                ParentProgram = program,
                VisitorType = visitorType
            };
            project.Locations.Add(region);

            context.Organizations.Add(owner);
            context.Locations.Add(country1);
            context.Locations.Add(region);
            context.Projects.Add(project);
            context.LocationTypes.Add(countryType);
            context.LocationTypes.Add(regionType);
            context.ProjectStatuses.Add(status);
            context.Programs.Add(program);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(1, serviceResult.CountryIsosByLocations.Count());
                Assert.AreEqual(country1.LocationIso, serviceResult.CountryIsosByLocations.First().Value);
                Assert.AreEqual(country1.LocationId, serviceResult.CountryIsosByLocations.First().Id);
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_CheckCountries_CheckDistinctCountriesOnly()
        {
            var now = DateTimeOffset.UtcNow;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var revisedOn = DateTimeOffset.UtcNow.AddDays(-2.0);
            var createdOn = DateTimeOffset.UtcNow.AddDays(-3.0);

            var countryType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id
            };
            var cityType = new LocationType
            {
                LocationTypeId = LocationType.City.Id
            };
            var place = new LocationType
            {
                LocationTypeId = LocationType.Place.Id
            };
            var regionType = new LocationType
            {
                LocationTypeId = LocationType.Region.Id
            };
            var region = new Location
            {
                LocationId = 1,
                LocationName = "region",
                LocationIso = "regionIso",
                LocationIso2 = "regioniso2",
                LocationTypeId = regionType.LocationTypeId,
                LocationType = regionType
            };
            var country = new Location
            {
                LocationId = 2,
                LocationName = "country",
                LocationIso = "countryIso",
                LocationIso2 = "iso2",
                LocationTypeId = countryType.LocationTypeId,
                LocationType = countryType,
                RegionId = region.LocationId,
                Region = region,
            };
            var city = new Location
            {
                LocationId = 3,
                LocationName = "city",
                LocationTypeId = cityType.LocationTypeId,
                LocationType = cityType,
                Country = country,
                CountryId = country.LocationId,
                Region = region,
                RegionId = region.LocationId
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };
            var owner = new Organization
            {
                OrganizationId = 20,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 10,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            owner.OwnerPrograms.Add(program);
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Themes = new HashSet<Theme>(),
                StartDate = yesterday,
                EndDate = now,
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>(),
                History = new History
                {
                    RevisedOn = revisedOn,
                    CreatedOn = createdOn
                },
                ProgramId = program.ProgramId,
                ParentProgram = program,
                VisitorType = visitorType
            };
            project.Locations.Add(region);
            project.Locations.Add(country);
            project.Locations.Add(city);

            context.Organizations.Add(owner);
            context.Locations.Add(region);
            context.Locations.Add(country);
            context.Locations.Add(city);
            context.Projects.Add(project);
            context.LocationTypes.Add(countryType);
            context.LocationTypes.Add(cityType);
            context.LocationTypes.Add(regionType);
            context.LocationTypes.Add(place);
            context.ProjectStatuses.Add(status);
            context.Programs.Add(program);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(1, serviceResult.CountryIsosByLocations.Count());
                Assert.AreEqual(country.LocationIso, serviceResult.CountryIsosByLocations.First().Value);
                Assert.AreEqual(country.LocationId, serviceResult.CountryIsosByLocations.First().Id);
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_CheckCountries_CheckRegions()
        {
            var now = DateTimeOffset.UtcNow;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var revisedOn = DateTimeOffset.UtcNow.AddDays(-2.0);
            var createdOn = DateTimeOffset.UtcNow.AddDays(-3.0);

            var regionType = new LocationType
            {
                LocationTypeId = LocationType.Region.Id
            };
            var region = new Location
            {
                LocationId = 1,
                LocationName = "region",
                LocationIso = "regionIso",
                LocationIso2 = "regioniso2",
                LocationTypeId = regionType.LocationTypeId,
                LocationType = regionType
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };
            var owner = new Organization
            {
                OrganizationId = 20,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 10,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            owner.OwnerPrograms.Add(program);
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Themes = new HashSet<Theme>(),
                StartDate = yesterday,
                EndDate = now,
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>(),
                History = new History
                {
                    RevisedOn = revisedOn,
                    CreatedOn = createdOn
                },
                ProgramId = program.ProgramId,
                ParentProgram = program,
                VisitorType = visitorType
            };
            project.Regions.Add(region);

            context.Organizations.Add(owner);
            context.Locations.Add(region);
            context.Projects.Add(project);
            context.LocationTypes.Add(regionType);
            context.ProjectStatuses.Add(status);
            context.Programs.Add(program);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(1, serviceResult.Regions.Count());
                Assert.AreEqual(region.LocationName, serviceResult.Regions.First().Name);
                Assert.AreEqual(region.LocationId, serviceResult.Regions.First().Id);
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }


        [TestMethod]
        public async Task TestGetProjectById_CheckLocations()
        {
            var now = DateTimeOffset.UtcNow;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var revisedOn = DateTimeOffset.UtcNow.AddDays(-2.0);
            var createdOn = DateTimeOffset.UtcNow.AddDays(-3.0);

            var countryType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id
            };
            var cityType = new LocationType
            {
                LocationTypeId = LocationType.City.Id
            };
            var place = new LocationType
            {
                LocationTypeId = LocationType.Place.Id
            };
            var country = new Location
            {
                LocationId = 1,
                LocationName = "country",
                LocationIso = "countryIso",
                LocationIso2 = "iso2",
                LocationTypeId = countryType.LocationTypeId,
                LocationType = countryType
            };
            var city = new Location
            {
                LocationId = 2,
                LocationName = "city",
                LocationTypeId = cityType.LocationTypeId,
                LocationType = cityType
            };
            var projectLocation = new Location
            {
                LocationId = 2,
                Country = country,
                CountryId = country.LocationId,
                City = city,
                CityId = city.LocationId,
                LocationName = "project location",
                Longitude = 2.0f,
                Latitude = 1.0f,
                LocationType = place,
                LocationTypeId = place.LocationTypeId
            };
            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };
            var owner = new Organization
            {
                OrganizationId = 20,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 10,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            owner.OwnerPrograms.Add(program);
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Themes = new HashSet<Theme>(),
                StartDate = yesterday,
                EndDate = now,
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>(),
                History = new History
                {
                    RevisedOn = revisedOn,
                    CreatedOn = createdOn
                },
                ProgramId = program.ProgramId,
                ParentProgram = program,
                VisitorType = visitorType
            };
            project.Locations.Add(projectLocation);

            context.Organizations.Add(owner);
            context.Locations.Add(country);
            context.Projects.Add(project);
            context.Locations.Add(projectLocation);
            context.LocationTypes.Add(countryType);
            context.LocationTypes.Add(cityType);
            context.LocationTypes.Add(place);
            context.ProjectStatuses.Add(status);
            context.Programs.Add(program);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(1, serviceResult.Locations.Count());
                var testProjectLocation = serviceResult.Locations.First();
                Assert.AreEqual(projectLocation.LocationId, testProjectLocation.Id);
                Assert.AreEqual(projectLocation.LocationName, testProjectLocation.Name);
                Assert.AreEqual(country.LocationId, testProjectLocation.CountryId);
                Assert.AreEqual(country.LocationName, testProjectLocation.Country);
                Assert.AreEqual(city.LocationId, testProjectLocation.CityId);
                Assert.AreEqual(city.LocationName, testProjectLocation.City);
                Assert.AreEqual(projectLocation.Longitude, testProjectLocation.Longitude);
                Assert.AreEqual(projectLocation.Latitude, projectLocation.Latitude);
                Assert.AreEqual(place.LocationTypeId, testProjectLocation.LocationTypeId);
                Assert.AreEqual(place.LocationTypeName, testProjectLocation.LocationTypeName);
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_CheckRegions()
        {
            var now = DateTimeOffset.UtcNow;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var revisedOn = DateTimeOffset.UtcNow.AddDays(-2.0);
            var createdOn = DateTimeOffset.UtcNow.AddDays(-3.0);

            var regionType = new LocationType
            {
                LocationTypeId = LocationType.Region.Id,
                LocationTypeName = LocationType.Region.Value
            };
            var region = new Location
            {
                LocationId = 1,
                LocationName = "region",
                LocationIso = "regionIso",
                LocationIso2 = "iso2",
                LocationTypeId = regionType.LocationTypeId,
                LocationType = regionType
            };
            
            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };
            var owner = new Organization
            {
                OrganizationId = 20,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 10,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            owner.OwnerPrograms.Add(program);
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Themes = new HashSet<Theme>(),
                StartDate = yesterday,
                EndDate = now,
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>(),
                History = new History
                {
                    RevisedOn = revisedOn,
                    CreatedOn = createdOn
                },
                ProgramId = program.ProgramId,
                ParentProgram = program,
                VisitorType = visitorType
            };
            project.Regions.Add(region);

            context.Organizations.Add(owner);
            context.Projects.Add(project);
            context.Locations.Add(region);
            context.LocationTypes.Add(regionType);
            context.ProjectStatuses.Add(status);
            context.Programs.Add(program);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(1, serviceResult.Regions.Count());
                var testProjectLocation = serviceResult.Regions.First();
                Assert.AreEqual(region.LocationId, testProjectLocation.Id);
                Assert.AreEqual(region.LocationName, testProjectLocation.Name);
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_CheckCountryIsosByRegions()
        {
            var now = DateTimeOffset.UtcNow;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var revisedOn = DateTimeOffset.UtcNow.AddDays(-2.0);
            var createdOn = DateTimeOffset.UtcNow.AddDays(-3.0);

            var regionType = new LocationType
            {
                LocationTypeId = LocationType.Region.Id,
                LocationTypeName = LocationType.Region.Value
            };
            var countryType = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };
            var region = new Location
            {
                LocationId = 1,
                LocationName = "region",
                LocationIso = "regionIso",
                LocationIso2 = "iso2",
                LocationTypeId = regionType.LocationTypeId,
                LocationType = regionType
            };
            var country = new Location
            {
                LocationId = 2,
                LocationName = "country",
                LocationIso = "country iso",
                LocationIso2 = "country iso2",
                LocationType = countryType,
                LocationTypeId = countryType.LocationTypeId,
                Region = region,
                RegionId = region.LocationId
            };
            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };
            var owner = new Organization
            {
                OrganizationId = 20,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 10,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            owner.OwnerPrograms.Add(program);
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Themes = new HashSet<Theme>(),
                StartDate = yesterday,
                EndDate = now,
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>(),
                History = new History
                {
                    RevisedOn = revisedOn,
                    CreatedOn = createdOn
                },
                ProgramId = program.ProgramId,
                ParentProgram = program,
                VisitorType = visitorType
            };
            project.Regions.Add(region);

            context.Locations.Add(country);
            context.Organizations.Add(owner);
            context.Projects.Add(project);
            context.Locations.Add(region);
            context.LocationTypes.Add(regionType);
            context.ProjectStatuses.Add(status);
            context.Programs.Add(program);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.IsNotNull(serviceResult);
                Assert.AreEqual(1, serviceResult.CountryIsosByRegions.Count());
                var testProjectLocation = serviceResult.CountryIsosByRegions.First();
                Assert.AreEqual(country.LocationIso, testProjectLocation.Value);
                Assert.AreEqual(country.LocationId, testProjectLocation.Id);
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_CheckContacts()
        {
            var now = DateTimeOffset.UtcNow;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var revisedOn = DateTimeOffset.UtcNow.AddDays(-2.0);
            var createdOn = DateTimeOffset.UtcNow.AddDays(-3.0);

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };

            var contact = new Contact
            {
                ContactId = 1,
                FullName = "fullName",
                Position = "Position"
            };
            var owner = new Organization
            {
                OrganizationId = 20,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 10,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };

            owner.OwnerPrograms.Add(program);
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Themes = new HashSet<Theme>(),
                StartDate = yesterday,
                EndDate = now,
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>(),
                History = new History
                {
                    RevisedOn = revisedOn,
                    CreatedOn = createdOn
                },
                ProgramId = program.ProgramId,
                ParentProgram = program,
                VisitorType = visitorType
            };

            project.Contacts.Add(contact);

            context.Organizations.Add(owner);
            context.Projects.Add(project);
            context.ProjectStatuses.Add(status);
            context.Contacts.Add(contact);
            context.Programs.Add(program);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                CollectionAssert.AreEqual(context.Contacts.Select(x => x.ContactId).ToList(), serviceResult.Contacts.Select(x => x.Id).ToList());
                CollectionAssert.AreEqual(context.Contacts.Select(x => x.FullName + " (" + x.Position + ")").ToList(), serviceResult.Contacts.Select(x => x.FullName + " (" + x.Position + ")").ToList());
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_CheckThemes()
        {
            var now = DateTimeOffset.UtcNow;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var revisedOn = DateTimeOffset.UtcNow.AddDays(-2.0);
            var createdOn = DateTimeOffset.UtcNow.AddDays(-3.0);
            var theme = new Theme
            {
                ThemeId = 1,
                ThemeName = "theme"
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };

            var contact = new Contact
            {
                ContactId = 1,
                FullName = "fullName",
                Position = "Position"
            };
            var owner = new Organization
            {
                OrganizationId = 20,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 10,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            owner.OwnerPrograms.Add(program);
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Themes = new HashSet<Theme>(),
                StartDate = yesterday,
                EndDate = now,
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>(),
                History = new History
                {
                    RevisedOn = revisedOn,
                    CreatedOn = createdOn
                },
                ProgramId = program.ProgramId,
                ParentProgram = program,
                VisitorType = visitorType
            };

            project.Themes.Add(theme);
            project.Contacts.Add(contact);

            context.Organizations.Add(owner);
            context.Themes.Add(theme);
            context.Projects.Add(project);
            context.ProjectStatuses.Add(status);
            context.Contacts.Add(contact);
            context.Programs.Add(program);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                CollectionAssert.AreEqual(context.Themes.Select(x => x.ThemeName).ToList(),
                    serviceResult.Themes.Select(x => x.Value).ToList());
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_CheckGoals()
        {
            var now = DateTimeOffset.UtcNow;
            var yesterday = DateTimeOffset.UtcNow.AddDays(-1.0);
            var revisedOn = DateTimeOffset.UtcNow.AddDays(-2.0);
            var createdOn = DateTimeOffset.UtcNow.AddDays(-3.0);

            var goal = new Goal
            {
                GoalId = 1,
                GoalName = "goal"
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };

            var contact = new Contact
            {
                ContactId = 1,
                FullName = "fullName",
                Position = "Position"
            };
            var owner = new Organization
            {
                OrganizationId = 20,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 10,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            owner.OwnerPrograms.Add(program);
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Themes = new HashSet<Theme>(),
                StartDate = yesterday,
                EndDate = now,
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>(),
                History = new History
                {
                    RevisedOn = revisedOn,
                    CreatedOn = createdOn
                },
                ProgramId = program.ProgramId,
                ParentProgram = program,
                VisitorType = visitorType
            };

            project.Goals.Add(goal);
            project.Contacts.Add(contact);

            context.Organizations.Add(owner);
            context.Projects.Add(project);
            context.Goals.Add(goal);
            context.ProjectStatuses.Add(status);
            context.Contacts.Add(contact);
            context.Programs.Add(program);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                CollectionAssert.AreEqual(context.Goals.Select(x => x.GoalName).ToList(),
                    serviceResult.Goals.Select(x => x.Value).ToList());
                Assert.AreEqual(context.ProjectStatuses.Select(x => x.Status).FirstOrDefault(), serviceResult.Status);
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_EmptyCollections()
        {
            var owner = new Organization
            {
                OrganizationId = 1,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            owner.OwnerPrograms.Add(program);
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                ParentProgram = program,
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Status = new ProjectStatus(),
                VisitorType = visitorType
            };
            program.Projects.Add(project);
            context.Programs.Add(program);
            context.Organizations.Add(owner);
            context.Projects.Add(project);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.AreEqual(project.Name, serviceResult.Name);
                Assert.AreEqual(project.Description, serviceResult.Description);
                CollectionAssert.AreEqual(context.Themes.Select(x => x.ThemeName).ToList(),
                    serviceResult.Themes.Select(x => x.Value).ToList());
                CollectionAssert.AreEqual(context.Locations.Where(x => x.LocationTypeId == LocationType.Country.Id).Select(x => x.LocationIso).ToList(),
                    serviceResult.CountryIsosByLocations.Select(x => x.Value).ToList());
                CollectionAssert.AreEqual(context.Goals.Select(x => x.GoalName).ToList(),
                    serviceResult.Goals.Select(x => x.Value).ToList());
                CollectionAssert.AreEqual(context.Objectives.Select(x => x.ObjectiveName).ToList(),
                    serviceResult.Objectives.Select(x => x.Name).ToList());
                CollectionAssert.AreEqual(context.Categories.Select(x => x.CategoryName).ToList(),
                    serviceResult.Categories.Select(x => x.Name).ToList());
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_WrongId()
        {
            var owner = new Organization
            {
                OrganizationId = 1,
                Name = "owner"
            };
            var program = new Program
            {
                ProgramId = 1,
                Name = "program",
                Owner = owner,
                OwnerId = owner.OrganizationId
            };
            owner.OwnerPrograms.Add(program);
            var visitorType = new VisitorType
            {
                VisitorTypeId = 1,
                VisitorTypeName = "test"
            };
            var project = new Project
            {
                ProgramId = program.ProgramId,
                ParentProgram = program,
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Status = new ProjectStatus(),
                VisitorType = visitorType
            };
            program.Projects.Add(project);
            context.Programs.Add(program);
            context.Organizations.Add(owner);
            context.Projects.Add(project);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.IsNull(serviceResult);
            };

            var result = service.GetProjectById(2);
            var resultAsync = await service.GetProjectByIdAsync(2);

            tester(result);
            tester(resultAsync);
        }
        #endregion

        #region Update

        [TestMethod]
        public async Task TestUpdate_ProjectDoesNotExist()
        {
            var updatedProject = new PublishedProject(
                updatedBy: new User(1),
                projectId: 1,
                name: "new name",
                description: "new description",
                projectStatusId: ProjectStatus.Pending.Id,
                sevisOrgId: "abcde1234567890",
                goalIds: null,
                themeIds: null,
                pointsOfContactIds: null,
                categoryIds: null,
                objectiveIds: null,
                locationIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow.AddDays(3.0),
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null
                );
            var message = String.Format("The project with id [{0}] was not found.", updatedProject.ProjectId);
            Action a = () => service.Update(updatedProject);
            Func<Task> f = () =>
            {
                return service.UpdateAsync(updatedProject);
            };
            a.ShouldThrow<ModelNotFoundException>().WithMessage(message);
            f.ShouldThrow<ModelNotFoundException>().WithMessage(message);
        }

        [TestMethod]
        public async Task TestUpdate_CheckProperties()
        {
            var oldName = "old name";
            var oldDescription = "old desc";
            var oldStartDate = DateTimeOffset.UtcNow.AddDays(-2.0);
            var oldEndDate = DateTime.UtcNow.AddDays(-1.0);
            var projectStatusId = ProjectStatus.Draft.Id;
            var createdDate = DateTimeOffset.UtcNow.AddDays(-1.0);
            var projectId = 1;
            var updaterId = 2;
            var oldHistory = new History
            {
                CreatedBy = 1,
                CreatedOn = createdDate,
                RevisedBy = 1,
                RevisedOn = createdDate
            };
            Project projectToUpdate = null;
            Program program = null;
            Organization office = null;
            context.SetupActions.Add(() =>
            {
                projectToUpdate = new Project
                {
                    Description = oldDescription,
                    EndDate = oldEndDate,
                    History = oldHistory,
                    Name = oldName,
                    ProjectId = projectId,
                    StartDate = oldStartDate,
                    ProjectStatusId = ProjectStatus.Other.Id

                };
                program = new Program
                {
                    ProgramId = 1
                };
                office = new Organization();
                office.OwnerPrograms.Add(program);
                program.Owner = office;
                context.Organizations.Add(office);
                projectToUpdate.ProgramId = program.ProgramId;
                projectToUpdate.ParentProgram = program;
                program.Projects.Add(projectToUpdate);
                context.Programs.Add(program);
                context.Projects.Add(projectToUpdate);
            });

            var updater = new User(updaterId);
            var updatedProject = new PublishedProject(
                updatedBy: updater,
                projectId: projectId,
                name: "new name",
                description: "new description",
                projectStatusId: ProjectStatus.Pending.Id,
                sevisOrgId: "abcde1234567890",
                goalIds: null,
                themeIds: null,
                pointsOfContactIds: null,
                categoryIds: null,
                objectiveIds: null,
                locationIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow.AddDays(3.0),
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null
                );

            Action tester = () =>
            {
                Assert.AreEqual(oldHistory.CreatedBy, projectToUpdate.History.CreatedBy);
                Assert.AreEqual(oldHistory.CreatedOn, projectToUpdate.History.CreatedOn);
                Assert.AreEqual(updater.Id, oldHistory.RevisedBy);
                DateTimeOffset.UtcNow.Should().BeCloseTo(oldHistory.RevisedOn, DbContextHelper.DATE_PRECISION);

                Assert.AreEqual(updatedProject.SevisOrgId, projectToUpdate.SevisOrgId);
                Assert.AreEqual(updatedProject.Description, projectToUpdate.Description);
                Assert.AreEqual(updatedProject.EndDate, projectToUpdate.EndDate);
                Assert.AreEqual(updatedProject.Name, projectToUpdate.Name);
                Assert.AreEqual(updatedProject.ProjectId, projectToUpdate.ProjectId);
                Assert.AreEqual(updatedProject.ProjectStatusId, projectToUpdate.ProjectStatusId);
                Assert.AreEqual(updatedProject.StartDate, projectToUpdate.StartDate);
                Assert.IsNull(projectToUpdate.DefaultExchangeVisitorFunding);
            };
            context.Revert();
            service.Update(updatedProject);
            tester();

            context.Revert();
            await service.UpdateAsync(updatedProject);
            tester();
            validator.Verify(x => x.ValidateUpdate(It.IsAny<ProjectServiceUpdateValidationEntity>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestUpdate_CheckContacts()
        {
            var projectToUpdateId = 1;
            Project projectToUpdate = null;
            Program program = null;
            Organization office = null;
            context.SetupActions.Add(() =>
            {
                office = new Organization();
                projectToUpdate = new Project
                {
                    ProjectId = projectToUpdateId,
                    ProjectStatusId = ProjectStatus.Other.Id
                };
                program = new Program
                {
                    ProgramId = 1
                };
                office.OwnerPrograms.Add(program);
                program.Owner = office;
                context.Organizations.Add(office);
                projectToUpdate.ProgramId = program.ProgramId;
                projectToUpdate.ParentProgram = program;
                program.Projects.Add(projectToUpdate);
                context.Programs.Add(program);
                context.Projects.Add(projectToUpdate);

            });


            var contactIds = new List<int> { 1 };
            var updater = new User(1);
            var updatedProject = new PublishedProject(
                updatedBy: updater,
                projectId: projectToUpdateId,
                name: "new name",
                description: "new description",
                projectStatusId: ProjectStatus.Pending.Id,
                sevisOrgId: "abcde1234567890",
                goalIds: null,
                themeIds: null,
                pointsOfContactIds: contactIds,
                categoryIds: null,
                objectiveIds: null,
                locationIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow.AddDays(3.0),
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null
                );
            Action tester = () =>
             {
                 Assert.AreEqual(1, projectToUpdate.Contacts.Count);
                 Assert.AreEqual(contactIds.First(), projectToUpdate.Contacts.First().ContactId);
             };
            context.Revert();
            service.Update(updatedProject);
            tester();

            context.Revert();
            await service.UpdateAsync(updatedProject);
            tester();

        }

        [TestMethod]
        public async Task TestUpdate_CheckThemes()
        {
            var projectToUpdateId = 1;
            Project projectToUpdate = null;
            Program program = null;
            Organization office = null;
            context.SetupActions.Add(() =>
            {
                office = new Organization();
                projectToUpdate = new Project
                {
                    ProjectId = projectToUpdateId,
                    ProjectStatusId = ProjectStatus.Other.Id
                };
                program = new Program
                {
                    ProgramId = 1
                };
                office.OwnerPrograms.Add(program);
                program.Owner = office;
                context.Organizations.Add(office);
                projectToUpdate.ProgramId = program.ProgramId;
                projectToUpdate.ParentProgram = program;
                program.Projects.Add(projectToUpdate);
                context.Programs.Add(program);
                context.Projects.Add(projectToUpdate);

            });


            var themeIds = new List<int> { 1 };
            var updater = new User(1);
            var updatedProject = new PublishedProject(
                updatedBy: updater,
                projectId: projectToUpdateId,
                name: "new name",
                description: "new description",
                projectStatusId: ProjectStatus.Pending.Id,
                sevisOrgId: "abcde1234567890",
                goalIds: null,
                themeIds: themeIds,
                pointsOfContactIds: null,
                categoryIds: null,
                objectiveIds: null,
                locationIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow.AddDays(3.0),
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null
                );

            Action tester = () =>
            {
                Assert.AreEqual(1, projectToUpdate.Themes.Count);
                Assert.AreEqual(themeIds.First(), projectToUpdate.Themes.First().ThemeId);
            };

            context.Revert();
            service.Update(updatedProject);
            tester();

            context.Revert();
            await service.UpdateAsync(updatedProject);
            tester();
        }


        [TestMethod]
        public async Task TestUpdate_CheckGoals()
        {
            var projectToUpdateId = 1;
            Project projectToUpdate = null;
            Program program = null;
            Organization office = null;
            context.SetupActions.Add(() =>
            {
                office = new Organization();
                projectToUpdate = new Project
                {
                    ProjectId = projectToUpdateId,
                    ProjectStatusId = ProjectStatus.Other.Id
                };
                program = new Program
                {
                    ProgramId = 1
                };
                office.OwnerPrograms.Add(program);
                program.Owner = office;
                context.Organizations.Add(office);
                projectToUpdate.ProgramId = program.ProgramId;
                projectToUpdate.ParentProgram = program;
                program.Projects.Add(projectToUpdate);
                context.Programs.Add(program);
                context.Projects.Add(projectToUpdate);
            });

            var goalIds = new List<int> { 1 };
            var updater = new User(1);
            var updatedProject = new PublishedProject(
                updatedBy: updater,
                projectId: projectToUpdateId,
                name: "new name",
                description: "new description",
                projectStatusId: ProjectStatus.Pending.Id,
                sevisOrgId: "abcde1234567890",
                goalIds: goalIds,
                themeIds: null,
                pointsOfContactIds: null,
                categoryIds: null,
                objectiveIds: null,
                locationIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow.AddDays(3.0),
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null
                );

            Action tester = () =>
            {
                Assert.AreEqual(1, projectToUpdate.Goals.Count);
                Assert.AreEqual(goalIds.First(), projectToUpdate.Goals.First().GoalId);
            };
            context.Revert();
            service.Update(updatedProject);
            tester();

            context.Revert();
            await service.UpdateAsync(updatedProject);
            tester();

        }

        [TestMethod]
        public async Task TestUpdate_CheckCategories()
        {
            var projectToUpdateId = 1;
            Project projectToUpdate = null;
            Program program = null;
            Organization office = null;
            context.SetupActions.Add(() =>
            {
                office = new Organization();
                projectToUpdate = new Project
                {
                    ProjectId = projectToUpdateId,
                    ProjectStatusId = ProjectStatus.Other.Id
                };
                program = new Program
                {
                    ProgramId = 1
                };
                office.OwnerPrograms.Add(program);
                program.Owner = office;
                context.Organizations.Add(office);
                projectToUpdate.ProgramId = program.ProgramId;
                projectToUpdate.ParentProgram = program;
                program.Projects.Add(projectToUpdate);
                context.Programs.Add(program);
                context.Projects.Add(projectToUpdate);

            });

            var categoryIds = new List<int> { 1 };
            var updater = new User(1);
            var updatedProject = new PublishedProject(
                updatedBy: updater,
                projectId: projectToUpdateId,
                name: "new name",
                description: "new description",
                projectStatusId: ProjectStatus.Pending.Id,
                sevisOrgId: "abcde1234567890",
                goalIds: null,
                themeIds: null,
                pointsOfContactIds: null,
                categoryIds: categoryIds,
                objectiveIds: null,
                locationIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow.AddDays(3.0),
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null
                );

            Action tester = () =>
            {
                Assert.AreEqual(1, projectToUpdate.Categories.Count);
                Assert.AreEqual(categoryIds.First(), projectToUpdate.Categories.First().CategoryId);
            };
            context.Revert();
            service.Update(updatedProject);
            tester();

            context.Revert();
            await service.UpdateAsync(updatedProject);
            tester();

        }

        [TestMethod]
        public async Task TestUpdate_CheckObjectives()
        {
            var projectToUpdateId = 1;
            Project projectToUpdate = null;
            Program program = null;
            Organization office = null;
            context.SetupActions.Add(() =>
            {
                office = new Organization();
                projectToUpdate = new Project
                {
                    ProjectId = projectToUpdateId,
                    ProjectStatusId = ProjectStatus.Other.Id
                };
                program = new Program
                {
                    ProgramId = 1
                };
                office.OwnerPrograms.Add(program);
                program.Owner = office;
                context.Organizations.Add(office);
                projectToUpdate.ProgramId = program.ProgramId;
                projectToUpdate.ParentProgram = program;
                program.Projects.Add(projectToUpdate);
                context.Programs.Add(program);
                context.Projects.Add(projectToUpdate);

            });

            var objectiveIds = new List<int> { 1 };
            var updater = new User(1);
            var updatedProject = new PublishedProject(
                updatedBy: updater,
                projectId: projectToUpdateId,
                name: "new name",
                description: "new description",
                projectStatusId: ProjectStatus.Pending.Id,
                sevisOrgId: "abcde1234567890",
                goalIds: null,
                themeIds: null,
                pointsOfContactIds: null,
                categoryIds: null,
                locationIds: null,
                objectiveIds: objectiveIds,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow.AddDays(3.0),
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null
                );

            Action tester = () =>
            {
                Assert.AreEqual(1, projectToUpdate.Objectives.Count);
                Assert.AreEqual(objectiveIds.First(), projectToUpdate.Objectives.First().ObjectiveId);
            };
            context.Revert();
            service.Update(updatedProject);
            tester();

            context.Revert();
            await service.UpdateAsync(updatedProject);
            tester();

        }

        [TestMethod]
        public async Task TestUpdate_CheckLocations()
        {
            var projectToUpdateId = 1;
            Project projectToUpdate = null;
            Program program = null;
            Organization office = null;
            context.SetupActions.Add(() =>
            {
                office = new Organization();
                projectToUpdate = new Project
                {
                    ProjectId = projectToUpdateId,
                    ProjectStatusId = ProjectStatus.Other.Id
                };
                program = new Program
                {
                    ProgramId = 1
                };
                office.OwnerPrograms.Add(program);
                program.Owner = office;
                context.Organizations.Add(office);
                projectToUpdate.ProgramId = program.ProgramId;
                projectToUpdate.ParentProgram = program;
                program.Projects.Add(projectToUpdate);
                context.Programs.Add(program);
                context.Projects.Add(projectToUpdate);

            });

            var locationIds = new List<int> { 1 };
            var updater = new User(1);
            var updatedProject = new PublishedProject(
                updatedBy: updater,
                projectId: projectToUpdateId,
                name: "new name",
                description: "new description",
                projectStatusId: ProjectStatus.Pending.Id,
                sevisOrgId: "abcde1234567890",
                goalIds: null,
                themeIds: null,
                pointsOfContactIds: null,
                categoryIds: null,
                locationIds: locationIds,
                objectiveIds: null,
                regionIds: null,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow.AddDays(3.0),
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null
                );

            Action tester = () =>
            {
                Assert.AreEqual(1, projectToUpdate.Locations.Count);
                Assert.AreEqual(locationIds.First(), projectToUpdate.Locations.First().LocationId);
            };
            context.Revert();
            service.Update(updatedProject);
            tester();

            context.Revert();
            await service.UpdateAsync(updatedProject);
            tester();
        }

        [TestMethod]
        public async Task TestUpdate_CheckRegions()
        {
            var projectToUpdateId = 1;
            Project projectToUpdate = null;
            Program program = null;
            Organization office = null;
            context.SetupActions.Add(() =>
            {
                office = new Organization();
                projectToUpdate = new Project
                {
                    ProjectId = projectToUpdateId,
                    ProjectStatusId = ProjectStatus.Other.Id
                };
                program = new Program
                {
                    ProgramId = 1
                };
                office.OwnerPrograms.Add(program);
                program.Owner = office;
                context.Organizations.Add(office);
                projectToUpdate.ProgramId = program.ProgramId;
                projectToUpdate.ParentProgram = program;
                program.Projects.Add(projectToUpdate);
                context.Programs.Add(program);
                context.Projects.Add(projectToUpdate);

            });

            var regionIds = new List<int> { 1 };
            var updater = new User(1);
            var updatedProject = new PublishedProject(
                updatedBy: updater,
                projectId: projectToUpdateId,
                name: "new name",
                description: "new description",
                projectStatusId: ProjectStatus.Pending.Id,
                sevisOrgId: "abcde1234567890",
                goalIds: null,
                themeIds: null,
                pointsOfContactIds: null,
                categoryIds: null,
                locationIds: null,
                objectiveIds: null,
                regionIds: regionIds,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow.AddDays(3.0),
                visitorTypeId: 1,
                usParticipantsEst: null,
                nonUsParticipantsEst: null,
                usParticipantsActual: null,
                nonUsParticipantsActual: null
                );

            Action tester = () =>
            {
                Assert.AreEqual(1, projectToUpdate.Regions.Count);
                Assert.AreEqual(regionIds.First(), projectToUpdate.Regions.First().LocationId);
            };
            context.Revert();
            service.Update(updatedProject);
            tester();

            context.Revert();
            await service.UpdateAsync(updatedProject);
            tester();
        }

        [TestMethod]
        public async Task TestUpdate_CheckParticipantsEstActual()
        {
            var projectToUpdateId = 1;
            Project projectToUpdate = null;
            Program program = null;
            Organization office = null;
            context.SetupActions.Add(() =>
            {
                office = new Organization();
                projectToUpdate = new Project
                {
                    ProjectId = projectToUpdateId,
                    ProjectStatusId = ProjectStatus.Other.Id
                };
                program = new Program
                {
                    ProgramId = 1
                };
                office.OwnerPrograms.Add(program);
                program.Owner = office;
                context.Organizations.Add(office);
                projectToUpdate.ProgramId = program.ProgramId;
                projectToUpdate.ParentProgram = program;
                program.Projects.Add(projectToUpdate);
                context.Programs.Add(program);
                context.Projects.Add(projectToUpdate);

            });

            var regionIds = new List<int> { 1 };
            var updater = new User(1);
            var updatedProject = new PublishedProject(
                updatedBy: updater,
                projectId: projectToUpdateId,
                name: "new name",
                description: "new description",
                projectStatusId: ProjectStatus.Pending.Id,
                sevisOrgId: "abcde1234567890",
                goalIds: null,
                themeIds: null,
                pointsOfContactIds: null,
                categoryIds: null,
                locationIds: null,
                objectiveIds: null,
                regionIds: regionIds,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow.AddDays(3.0),
                visitorTypeId: 1,
                usParticipantsEst: 3,
                nonUsParticipantsEst: 4,
                usParticipantsActual: 5,
                nonUsParticipantsActual: 6
                );

            Action tester = () =>
            {
                Assert.AreEqual(3, projectToUpdate.UsParticipantsEst);
                Assert.AreEqual(4, projectToUpdate.NonUsParticipantsEst);
                Assert.AreEqual(5, projectToUpdate.UsParticipantsActual);
                Assert.AreEqual(6, projectToUpdate.NonUsParticipantsActual);
            };
            context.Revert();
            service.Update(updatedProject);
            tester();

            context.Revert();
            await service.UpdateAsync(updatedProject);
            tester();
        }

        [TestMethod]
        public async Task TestUpdate_CheckDefaultExchangeVisitorFunding()
        {
            var projectToUpdateId = 1;
            Project projectToUpdate = null;
            Program program = null;
            Organization office = null;
            context.SetupActions.Add(() =>
            {
                office = new Organization();
                projectToUpdate = new Project
                {
                    ProjectId = projectToUpdateId,
                    ProjectStatusId = ProjectStatus.Other.Id,
                };
                program = new Program
                {
                    ProgramId = 1
                };
                office.OwnerPrograms.Add(program);
                program.Owner = office;
                context.Organizations.Add(office);
                projectToUpdate.ProgramId = program.ProgramId;
                projectToUpdate.ParentProgram = program;
                program.Projects.Add(projectToUpdate);
                context.Programs.Add(program);
                context.Projects.Add(projectToUpdate);

            });

            var regionIds = new List<int> { 1 };
            var updater = new User(1);
            var updatedProject = new PublishedProject(
                updatedBy: updater,
                projectId: projectToUpdateId,
                name: "new name",
                description: "new description",
                projectStatusId: ProjectStatus.Pending.Id,
                sevisOrgId: "abcde1234567890",
                goalIds: null,
                themeIds: null,
                pointsOfContactIds: null,
                categoryIds: null,
                locationIds: null,
                objectiveIds: null,
                regionIds: regionIds,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow.AddDays(3.0),
                visitorTypeId: VisitorType.ExchangeVisitor.Id,
                usParticipantsEst: 3,
                nonUsParticipantsEst: 4,
                usParticipantsActual: 5,
                nonUsParticipantsActual: 6
                );

            Action tester = () =>
            {
                Assert.IsNotNull(projectToUpdate.DefaultExchangeVisitorFunding);
            };
            context.Revert();
            service.Update(updatedProject);
            tester();

            context.Revert();
            await service.UpdateAsync(updatedProject);
            tester();
        }

        [TestMethod]
        public async Task TestUpdate_ParticipantExchangeVisitor()
        {
            var projectToUpdateId = 1;
            Project projectToUpdate = null;
            Program program = null;
            Organization office = null;
            Participant participant = null;
            ParticipantType participantType = null;
            context.SetupActions.Add(() =>
            {
                office = new Organization();

                participantType = new ParticipantType
                {
                    ParticipantTypeId = ParticipantType.ForeignTravelingParticipant.Id,
                    Name = ParticipantType.ForeignTravelingParticipant.Value,
                    IsPerson = true
                };
                participant = new Participant
                {
                    ParticipantId = 1,
                    ParticipantTypeId = participantType.ParticipantTypeId,
                    ParticipantType = participantType,
                    ProjectId = projectToUpdateId
                };
                projectToUpdate = new Project
                {
                    ProjectId = projectToUpdateId,
                    ProjectStatusId = ProjectStatus.Other.Id,
                    VisitorTypeId = VisitorType.NotApplicable.Id
                };
                program = new Program
                {
                    ProgramId = 1
                };
                office.OwnerPrograms.Add(program);
                program.Owner = office;
                context.Organizations.Add(office);
                context.ParticipantTypes.Add(participantType);
                context.Participants.Add(participant);
                projectToUpdate.ProgramId = program.ProgramId;
                projectToUpdate.ParentProgram = program;
                projectToUpdate.Participants.Add(participant);
                program.Projects.Add(projectToUpdate);
                context.Programs.Add(program);
                context.Projects.Add(projectToUpdate);

            });

            var regionIds = new List<int> { 1 };
            var updater = new User(1);
            var updatedProject = new PublishedProject(
                updatedBy: updater,
                projectId: projectToUpdateId,
                name: "new name",
                description: "new description",
                projectStatusId: ProjectStatus.Pending.Id,
                sevisOrgId: "abcde1234567890",
                goalIds: null,
                themeIds: null,
                pointsOfContactIds: null,
                categoryIds: null,
                locationIds: null,
                objectiveIds: null,
                regionIds: regionIds,
                startDate: DateTimeOffset.UtcNow.AddDays(1.0),
                endDate: DateTimeOffset.UtcNow.AddDays(3.0),
                visitorTypeId: VisitorType.ExchangeVisitor.Id,
                usParticipantsEst: 3,
                nonUsParticipantsEst: 4,
                usParticipantsActual: 5,
                nonUsParticipantsActual: 6
                );

            Action tester = () =>
            {
                Assert.IsNotNull(context.ParticipantPersons.FirstOrDefault());
                Assert.IsNotNull(context.ParticipantExchangeVisitors.FirstOrDefault());
                Assert.AreEqual(1, context.ParticipantPersons.Count());
                Assert.AreEqual(1, context.ParticipantExchangeVisitors.Count());
            };
            context.Revert();
            service.Update(updatedProject);
            tester();

            context.Revert();
            await service.UpdateAsync(updatedProject);
            tester();
        }
        #endregion

        #region Participants
        [TestMethod]
        public async Task TestAddParticipant_ParticipantBusinessEntityNotSupported()
        {
            var user = new User(1);
            var projectId = 1;
            var participantTypeId = ParticipantType.Individual.Id;

            var notSupportedParticipant = new NotSupportedAdditonalProjectParticipant(user, projectId, participantTypeId);
            Func<Task> addNotSupportedParticipant = () =>
            {
                return service.AddParticipantAsync(notSupportedParticipant);
            };

            service.Invoking(x => x.AddParticipant(notSupportedParticipant)).ShouldThrow<NotSupportedException>()
                .WithMessage(String.Format("The additional participant is not supported.", projectId));
            addNotSupportedParticipant.ShouldThrow<NotSupportedException>()
                .WithMessage(String.Format("The additional participant is not supported.", projectId));
        }

        [TestMethod]
        public async Task TestAddPariticipant_ProjectDoesNotExist()
        {
            var user = new User(1);
            var projectId = 1;
            var organizationId = 2;
            var personId = 3;
            var participantTypeId = ParticipantType.Individual.Id;
            var person = new Person
            {
                PersonId = personId
            };
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            context.People.Add(person);
            context.Organizations.Add(organization);

            var personParticipant = new AdditionalPersonProjectParticipant(user, projectId, personId, participantTypeId);
            var organizationParticipant = new AdditionalOrganizationProjectParticipant(user, projectId, organizationId, participantTypeId);

            service.Invoking(x => x.AddParticipant(personParticipant)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The project with id [{0}] does not exist.", projectId));
            service.Invoking(x => x.AddParticipant(organizationParticipant)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The project with id [{0}] does not exist.", projectId));

            Func<Task> addPerson = () =>
            {
                return service.AddParticipantAsync(personParticipant);
            };
            Func<Task> addOrg = () =>
            {
                return service.AddParticipantAsync(organizationParticipant);
            };
            addPerson.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The project with id [{0}] does not exist.", projectId));
            addOrg.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The project with id [{0}] does not exist.", projectId));
        }

        [TestMethod]
        public async Task TestAddPariticipant_PersonDoesNotExist()
        {
            var user = new User(1);
            var projectId = 1;
            var personId = 3;
            var participantTypeId = ParticipantType.Individual.Id;
            var project = new Project
            {
                ProjectId = projectId
            };
            context.Projects.Add(project);

            var personParticipant = new AdditionalPersonProjectParticipant(user, projectId, personId, participantTypeId);

            service.Invoking(x => x.AddParticipant(personParticipant)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The person with id [{0}] does not exist.", personId));

            Func<Task> addPerson = () =>
            {
                return service.AddParticipantAsync(personParticipant);
            };
            addPerson.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The person with id [{0}] does not exist.", personId));

        }

        [TestMethod]
        public async Task TestAddPariticipant_OrganizationDoesNotExist()
        {
            var user = new User(1);
            var projectId = 1;
            var organizationId = 2;
            var participantTypeId = ParticipantType.Individual.Id;
            var project = new Project
            {
                ProjectId = projectId
            };
            context.Projects.Add(project);

            var personParticipant = new AdditionalOrganizationProjectParticipant(user, projectId, organizationId, participantTypeId);

            service.Invoking(x => x.AddParticipant(personParticipant)).ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The organization with id [{0}] does not exist.", organizationId));

            Func<Task> addPerson = () =>
            {
                return service.AddParticipantAsync(personParticipant);
            };
            addPerson.ShouldThrow<ModelNotFoundException>()
                .WithMessage(String.Format("The organization with id [{0}] does not exist.", organizationId));
        }

        [TestMethod]
        public async Task TestAddPariticipant_ParticipantTypeDoesNotExist()
        {
            var user = new User(1);
            var projectId = 1;
            var organizationId = 2;
            var personId = 3;
            var participantTypeId = ParticipantType.Individual.Id;
            var person = new Person
            {
                PersonId = personId
            };
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            var project = new Project
            {
                ProjectId = projectId
            };
            context.People.Add(person);
            context.Organizations.Add(organization);
            context.Projects.Add(project);

            var personParticipant = new AdditionalPersonProjectParticipant(user, projectId, personId, participantTypeId);
            var organizationParticipant = new AdditionalOrganizationProjectParticipant(user, projectId, organizationId, participantTypeId);

            service.Invoking(x => x.AddParticipant(personParticipant)).ShouldThrow<ModelNotFoundException>()
                .WithMessage("The participant type does not exist.");
            service.Invoking(x => x.AddParticipant(organizationParticipant)).ShouldThrow<ModelNotFoundException>()
                .WithMessage("The participant type does not exist.");

            Func<Task> addPerson = () =>
            {
                return service.AddParticipantAsync(personParticipant);
            };
            Func<Task> addOrg = () =>
            {
                return service.AddParticipantAsync(organizationParticipant);
            };
            addPerson.ShouldThrow<ModelNotFoundException>()
                .WithMessage("The participant type does not exist.");
            addOrg.ShouldThrow<ModelNotFoundException>()
                .WithMessage("The participant type does not exist.");
        }

        [TestMethod]
        public async Task TestAddParticipant_PersonParticipantAlreadyExists()
        {
            var user = new User(1);
            var projectId = 1;
            var organizationId = 2;
            var personId = 3;
            var person = new Person
            {
                PersonId = personId
            };
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            var project = new Project
            {
                ProjectId = projectId
            };
            var participantType = new ParticipantType
            {
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                PersonId = person.PersonId,
                ProjectId = project.ProjectId,
                Project = project
            };
            context.SetupActions.Add(() =>
            {
                context.People.Add(person);
                context.Organizations.Add(organization);
                context.Projects.Add(project);
                context.ParticipantTypes.Add(participantType);
                context.Participants.Add(participant);
            });
            context.Revert();
            Assert.AreEqual(1, context.Participants.Count());

            Action<AdditionalPersonProjectParticipant> tester = (personParticipant) =>
            {
                Assert.AreEqual(1, context.Participants.Count());
            };
            var additionalParticipant = new AdditionalPersonProjectParticipant(user, projectId, personId, participantType.ParticipantTypeId);

            context.Revert();
            service.AddParticipant(additionalParticipant);
            tester(additionalParticipant);

            context.Revert();
            await service.AddParticipantAsync(additionalParticipant);
            tester(additionalParticipant);
        }

        [TestMethod]
        public async Task TestAddParticipant_OrganizationParticipantAlreadyExists()
        {
            var user = new User(1);
            var projectId = 1;
            var organizationId = 2;
            var personId = 3;
            var person = new Person
            {
                PersonId = personId
            };
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            var project = new Project
            {
                ProjectId = projectId
            };
            var participantType = new ParticipantType
            {
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            var participant = new Participant
            {
                ParticipantId = 1,
                OrganizationId = organizationId,
                ProjectId = project.ProjectId,
                Project = project
            };
            context.SetupActions.Add(() =>
            {
                context.People.Add(person);
                context.Organizations.Add(organization);
                context.Projects.Add(project);
                context.ParticipantTypes.Add(participantType);
                context.Participants.Add(participant);
            });
            context.Revert();
            Assert.AreEqual(1, context.Participants.Count());

            Action<AdditionalOrganizationProjectParticipant> tester = (organizationParticipant) =>
            {
                Assert.AreEqual(1, context.Participants.Count());
            };
            var additionalParticipant = new AdditionalOrganizationProjectParticipant(user, projectId, organizationId, participantType.ParticipantTypeId);

            context.Revert();
            service.AddParticipant(additionalParticipant);
            tester(additionalParticipant);

            context.Revert();
            await service.AddParticipantAsync(additionalParticipant);
            tester(additionalParticipant);
        }

        [TestMethod]
        public async Task TestAddParticipant_PersonParticipant()
        {
            var user = new User(1);
            var projectId = 1;
            var organizationId = 2;
            var personId = 3;
            var person = new Person
            {
                PersonId = personId
            };
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            var project = new Project
            {
                ProjectId = projectId
            };
            var participantType = new ParticipantType
            {
                Name = ParticipantType.Individual.Value,
                ParticipantTypeId = ParticipantType.Individual.Id
            };
            context.SetupActions.Add(() =>
            {
                context.People.Add(person);
                context.Organizations.Add(organization);
                context.Projects.Add(project);
                context.ParticipantTypes.Add(participantType);
            });

            Assert.AreEqual(0, context.Participants.Count());

            Action<AdditionalPersonProjectParticipant> tester = (personParticipant) =>
            {
                Assert.AreEqual(1, context.Participants.Count());
                var addedParticipant = context.Participants.First();
                addAdditionalPersonProjectParticipantTester(personParticipant, addedParticipant, user);
            };
            var additionalParticipant = new AdditionalPersonProjectParticipant(user, projectId, personId, participantType.ParticipantTypeId);

            context.Revert();
            service.AddParticipant(additionalParticipant);
            tester(additionalParticipant);

            context.Revert();
            await service.AddParticipantAsync(additionalParticipant);
            tester(additionalParticipant);
        }

        [TestMethod]
        public async Task TestAddParticipant_OrganizationParticipant()
        {
            var user = new User(1);
            var projectId = 1;
            var organizationId = 2;
            var personId = 3;
            var person = new Person
            {
                PersonId = personId
            };
            var organization = new Organization
            {
                OrganizationId = organizationId
            };
            var project = new Project
            {
                ProjectId = projectId
            };
            var participantType = new ParticipantType
            {
                Name = ParticipantType.ForeignNonTravelingParticipant.Value,
                ParticipantTypeId = ParticipantType.ForeignNonTravelingParticipant.Id
            };
            context.SetupActions.Add(() =>
            {
                context.People.Add(person);
                context.Organizations.Add(organization);
                context.Projects.Add(project);
                context.ParticipantTypes.Add(participantType);
            });

            Assert.AreEqual(0, context.Participants.Count());

            Action<AdditionalOrganizationProjectParticipant> tester = (organizationParticipant) =>
            {
                Assert.AreEqual(1, context.Participants.Count());
                var addedParticipant = context.Participants.First();
                addAdditionalOrganizationProjectParticipantTester(organizationParticipant, addedParticipant, user);
            };
            var additionalParticipant = new AdditionalOrganizationProjectParticipant(user, projectId, organizationId, participantType.ParticipantTypeId);

            context.Revert();
            service.AddParticipant(additionalParticipant);
            tester(additionalParticipant);

            context.Revert();
            await service.AddParticipantAsync(additionalParticipant);
            tester(additionalParticipant);
        }
        #endregion

    }
}



