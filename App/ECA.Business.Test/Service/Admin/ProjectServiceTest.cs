using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Logging;
using ECA.Core.Query;
using ECA.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class ProjectServiceTest
    {
        private TestEcaContext context;
        private ProjectService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new ProjectService(context, new TraceLogger());
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
            };

            var createdProject = service.Create(draftProject);
            var createdProjectAsync = await service.CreateAsync(draftProject);

            tester(createdProject);
            tester(createdProjectAsync);
        }

        [TestMethod]
        public async Task TestCreate_CheckFocus()
        {
            var focus = new Focus
            {
                FocusId = 1,
                FocusName = "focusName"
            };
            
            var program = new Program
            {
                ProgramId = 1,
                Themes = new List<Theme>(),
                Goals = new List<Goal>(),
                Contacts = new List<Contact>(),
                Regions = new List<Location>(),
                Focus = focus
            };

            context.Foci.Add(focus);
            context.Programs.Add(program);

            var draftProject = new DraftProject(new User(1), "name", "description", program.ProgramId);

            Action<Project> tester = (project) => 
            {
                Assert.IsNotNull(project);
                Assert.AreEqual(context.Foci.Select(x => x.FocusName).FirstOrDefault(), project.Focus.FocusName);
            };
           
            var createdProject = service.Create(draftProject);
            var createdProjectAsync = await service.CreateAsync(draftProject);

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
                Themes = new List<Theme>(),
                Goals = new List<Goal>(),
                Contacts = new List<Contact>(),
                Regions = new List<Location>() 
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
                Themes = new List<Theme>(),
                Goals = new List<Goal>(),
                Contacts = new List<Contact>(),
                Regions = new List<Location>() 
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
                Themes = new List<Theme>(),
                Goals = new List<Goal>(),
                Contacts = new List<Contact>(),
                Regions = new List<Location>() 
            };

            program.Contacts.Add(contact);

            context.Contacts.Add(contact);
            context.Programs.Add(program);

            var draftProject = new DraftProject(new User(1), "name", "description", program.ProgramId);

            Action<Project> tester = (project) =>
            {
                Assert.IsNotNull(project);
                CollectionAssert.AreEqual(context.Contacts.Select(x => x.ContactId).ToList(), project.Contacts.Select(x => x.ContactId).ToList());
            };

            var createdProject = service.Create(draftProject);
            var createdProjectAsync = await service.CreateAsync(draftProject);

            tester(createdProject);
            tester(createdProjectAsync);
        }

        [TestMethod]
        public async Task TestCreate_CheckRegions()
        {
            var region = new Location
            {
                LocationId = 1,
                LocationName = "locationName"
            };

            var program = new Program
            {
                ProgramId = 1,
                Themes = new List<Theme>(),
                Goals = new List<Goal>(),
                Contacts = new List<Contact>(),
                Regions = new List<Location>() 
            };

            program.Regions.Add(region);

            context.Locations.Add(region);
            context.Programs.Add(program);

            var draftProject = new DraftProject(new User(1), "name", "description", program.ProgramId);

            Action<Project> tester = (project) =>
            {
                Assert.IsNotNull(project);
                CollectionAssert.AreEqual(context.Locations.Select(x => x.LocationId).ToList(), project.Regions.Select(x => x.LocationId).ToList());
            };

            var createdProject = service.Create(draftProject);
            var createdProjectAsync = await service.CreateAsync(draftProject);
        }

        [TestMethod]
        public async Task TestCreate_CheckAudit()
        {
            var program = new Program
            {
                ProgramId = 1,
                Themes = new List<Theme>(),
                Goals = new List<Goal>(),
                Contacts = new List<Contact>(),
                Regions = new List<Location>()
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

        #region Get Projects By Program Id
        [TestMethod]
        public async Task TestGetProjectsByProgramId_CheckProperties()
        {
            var now = DateTimeOffset.UtcNow;
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
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                StartDate = now,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
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

                Assert.AreEqual(program.ProgramId, firstResult.ProgramId);
                Assert.AreEqual(project.Name, firstResult.ProjectName);
                Assert.AreEqual(project.ProjectId, firstResult.ProjectId);
                Assert.AreEqual(project.StartDate, firstResult.StartDate);
                Assert.AreEqual(project.StartDate.Year, firstResult.StartYear);
                Assert.AreEqual(project.StartDate.Year.ToString(), firstResult.StartYearAsString);
                Assert.AreEqual(status.Status, firstResult.ProjectStatusName);
                Assert.AreEqual(status.ProjectStatusId, firstResult.ProjectStatusId);

                Assert.IsTrue(firstResult.LocationNames.Contains(location1.LocationName));
                Assert.IsTrue(firstResult.LocationNames.Contains(location2.LocationName));
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
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
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
            var project = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
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
            var project1 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project1",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
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
                ProjectStatusId = status.ProjectStatusId
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
            var project1 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project1",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
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
            var project1 = new Project
            {
                ProgramId = program.ProgramId,
                Name = "project1",
                ParentProgram = program,
                ProjectId = 100,
                Status = status,
                ProjectStatusId = status.ProjectStatusId
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
                ProjectStatusId = status.ProjectStatusId
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

        #region Get Project By Id
        [TestMethod]
        public async Task TestGetProjectById_CheckProperties()
        {
            var theme = new Theme
            {
                ThemeId = 1,
                ThemeName = "theme"
            };

            var location = new Location
            {
                LocationId = 1,
                LocationName = "country",
                LocationIso = "countryIso",
                LocationTypeId = LocationType.Country.Id
            };

            var region = new Location
            {
                LocationName = "region",
                LocationId = 3,
                LocationIso = "locationIso",
                LocationTypeId = LocationType.Region.Id
            };
            location.Region = region;

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

            var focus = new Focus
            {
                FocusId = 1,
                FocusName = "focusName"
            };

            var contact = new Contact
            {
                ContactId = 1,
                FullName = "fullName"
            };

            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Focus = focus,
                Themes = new HashSet<Theme>(),
                Locations = new HashSet<Location>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = status,
                Contacts = new HashSet<Contact>()
            };

            project.Themes.Add(theme);
            project.Locations.Add(location);
            project.Regions.Add(region);
            project.Goals.Add(goal);
            project.Contacts.Add(contact);

            context.Themes.Add(theme);
            context.Locations.Add(location);
            context.Projects.Add(project);
            context.Locations.Add(region);
            context.Goals.Add(goal);
            context.ProjectStatuses.Add(status);
            context.Foci.Add(focus);
            context.Contacts.Add(contact);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.AreEqual(project.Name, serviceResult.Name);
                Assert.AreEqual(project.Description, serviceResult.Description);
                CollectionAssert.AreEqual(context.Themes.Select(x => x.ThemeName).ToList(),
                    serviceResult.Themes.Select(x => x.Value).ToList());
                CollectionAssert.AreEqual(context.Locations.Where(x => x.LocationTypeId == LocationType.Country.Id).Select(x => x.LocationIso).ToList(),
                    serviceResult.CountryIsos.Select(x => x.Value).ToList());
                CollectionAssert.AreEqual(context.Goals.Select(x => x.GoalName).ToList(),
                    serviceResult.Goals.Select(x => x.Value).ToList());
                Assert.AreEqual(context.ProjectStatuses.Select(x => x.Status).FirstOrDefault(), serviceResult.Status);
                Assert.AreEqual(context.Foci.Select(x => x.FocusName).FirstOrDefault(), serviceResult.Focus);
                CollectionAssert.AreEqual(context.Contacts.Select(x => x.ContactId).ToList(), serviceResult.Contacts.Select(x => x.Id).ToList());
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);
            
            tester(result);
            tester(resultAsync);
        }

        [TestMethod]
        public async Task TestGetProjectById_EmptyCollections()
        {
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Focus = new Focus(),
                Themes = new HashSet<Theme>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>(),
                Status = new ProjectStatus()
            };

            context.Projects.Add(project);

            Action<ProjectDTO> tester = (serviceResult) =>
            {
                Assert.AreEqual(project.Name, serviceResult.Name);
                Assert.AreEqual(project.Description, serviceResult.Description);
                CollectionAssert.AreEqual(context.Themes.Select(x => x.ThemeName).ToList(),
                    serviceResult.Themes.Select(x => x.Value).ToList());
                CollectionAssert.AreEqual(context.Locations.Where(x => x.LocationTypeId == LocationType.Country.Id).Select(x => x.LocationIso).ToList(),
                    serviceResult.CountryIsos.Select(x => x.Value).ToList());
                CollectionAssert.AreEqual(context.Goals.Select(x => x.GoalName).ToList(),
                    serviceResult.Goals.Select(x => x.Value).ToList());
            };

            var result = service.GetProjectById(project.ProjectId);
            var resultAsync = await service.GetProjectByIdAsync(project.ProjectId);

            tester(result);
            tester(resultAsync);
        } 

        [TestMethod]
        public async Task TestGetProjectById_WrongId()
        {
            var project = new Project
            {
                ProjectId = 1,
                Name = "name",
                Description = "description",
                Focus = new Focus(),
                Themes = new HashSet<Theme>(),
                Regions = new HashSet<Location>(),
                Goals = new HashSet<Goal>()
            };

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
    }
}



