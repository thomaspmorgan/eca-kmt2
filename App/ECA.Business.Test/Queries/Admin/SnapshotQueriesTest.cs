using System.Linq;
using ECA.Business.Queries.Admin;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace ECA.Business.Test.Queries.Admin
{
    [TestClass]
    public class SnapshotQueriesTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestCreateGetProgramRelatedProjectsCountQuery()
        {
            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };

            var endDate = DateTime.UtcNow;
            
            var org = new Organization
            {
                OrganizationId = 1,
                OfficeSymbol = "ABCDE"
            };

            var program = new Program
            {
                ProgramId = 1,
                Owner = org,
                OwnerId = org.OrganizationId,
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId
            };
            
            var project = new Project
            {
                ProjectId = 1,
                ParentProgram = program,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                EndDate = endDate
            };

            var project1 = new Project
            {
                ProjectId = 2,
                ParentProgram = program,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                EndDate = endDate
            };

            var project2 = new Project
            {
                ProjectId = 3,
                ParentProgram = program,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                EndDate = endDate
            };

            var project3 = new Project
            {
                ProjectId = 4,
                ParentProgram = program,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                EndDate = endDate
            };

            program.Projects.Add(project);
            program.Projects.Add(project1);
            program.Projects.Add(project2);
            program.Projects.Add(project3);

            project.RelatedProjects.Add(project1);
            project.RelatedProjects.Add(project2);
            project.RelatedProjects.Add(project3);

            context.Programs.Add(program);
            context.Projects.Add(project1);
            context.Projects.Add(project2);
            context.Projects.Add(project3);
            context.Projects.Add(project);

            List<int> programIds = new List<int>();
            programIds.Add(program.ProgramId);

            var results = SnapshotQueries.CreateGetProgramRelatedProjectsCountQuery(context, programIds);
            Assert.AreEqual(3, results.DataValue);
        }

        [TestMethod]
        public void TestCreateGetProgramParticipantsCountQuery()
        {
            var person = new Person
            {
                PersonId = 1
            };

            var person2 = new Person
            {
                PersonId = 2
            };

            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };

            var endDate = DateTime.UtcNow;

            var org = new Organization
            {
                OrganizationId = 1,
                OfficeSymbol = "ABCDE"
            };

            var program = new Program
            {
                ProgramId = 1,
                Owner = org,
                OwnerId = org.OrganizationId,
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId
            };

            var project = new Project
            {
                ProjectId = 1,
                ParentProgram = program,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                EndDate = endDate
            };

            var participant = new Participant
            {
                ParticipantId = 1,
                Person = person,
                PersonId = person.PersonId,
                Project = project,
                ProjectId = project.ProjectId
            };

            var participant2 = new Participant
            {
                ParticipantId = 2,
                Person = person2,
                PersonId = person2.PersonId,
                Project = project,
                ProjectId = project.ProjectId
            };

            project.Participants.Add(participant);
            project.Participants.Add(participant2);
            program.Projects.Add(project);
            
            context.Programs.Add(program);
            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.Participants.Add(participant2);

            List<int> programIds = new List<int>();
            programIds.Add(program.ProgramId);

            var results = SnapshotQueries.CreateGetProgramParticipantsCountQuery(context, programIds);
            Assert.AreEqual(2, results.DataValue);
        }

        [TestMethod]
        public void TestCreateGetProgramBudgetTotalQuery()
        {
            var sourceId = 1;
            var recipientId = 2;
            MoneyFlowSourceRecipientType programType;
            MoneyFlowSourceRecipientType projectType;
            MoneyFlowStatus actual;
            programType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Program.Id,
                TypeName = MoneyFlowSourceRecipientType.Program.Value
            };
            projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var endDate = DateTime.UtcNow;

            var program = new Program
            {
                ProgramId = sourceId,
                Name = "program"
            };
            var project = new Project
            {
                ProjectId = recipientId,
                Name = "project",
                EndDate = endDate
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProjectId = recipientId,
                SourceProgram = program,
                RecipientProject = project,
                SourceType = programType,
                SourceTypeId = programType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 100.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            var moneyFlow2 = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProjectId = recipientId,
                SourceProgram = program,
                RecipientProject = project,
                SourceType = programType,
                SourceTypeId = programType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 150.00m,
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowId = 11,
                ParentMoneyFlowId = 110
            };
            project.RecipientProjectMoneyFlows.Add(moneyFlow);
            project.RecipientProjectMoneyFlows.Add(moneyFlow2);
            program.Projects.Add(project);

            context.Programs.Add(program);
            context.Projects.Add(project);
            context.MoneyFlows.Add(moneyFlow);
            context.MoneyFlows.Add(moneyFlow2);

            List<int> programIds = new List<int>();
            programIds.Add(program.ProgramId);
            var results = SnapshotQueries.CreateGetProgramBudgetTotalQuery(context, programIds);
            Assert.AreEqual(0, results.DataValue);
        }

        [TestMethod]
        public void TestCreateGetProgramFundingSourceCountQuery()
        {
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramCountryCountQuery()
        {
            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };

            var typeRegion = new LocationType
            {
                LocationTypeId = LocationType.Region.Id,
                LocationTypeName = LocationType.Region.Value
            };
            var typeCountry = new LocationType
            {
                LocationTypeId = LocationType.Country.Id,
                LocationTypeName = LocationType.Country.Value
            };

            var region1 = new Location()
            {
                LocationId = 1,
                LocationType = typeRegion,
                LocationTypeId = typeRegion.LocationTypeId
            };

            var country1 = new Location()
            {
                LocationId = 2,
                RegionId = 1,
                LocationType = typeCountry,
                LocationTypeId = typeCountry.LocationTypeId
            };

            var country2 = new Location()
            {
                LocationId = 3,
                RegionId = 1,
                LocationType = typeCountry,
                LocationTypeId = typeCountry.LocationTypeId
            };

            var country3 = new Location()
            {
                LocationId = 4,
                RegionId = 1,
                LocationType = typeCountry,
                LocationTypeId = typeCountry.LocationTypeId
            };

            var program1 = new Program
            {
                ProgramId = 1,
                Name = "program 1",
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId
            };

            program1.Regions.Add(region1);
            program1.Locations.Add(country1);
            program1.Locations.Add(country2);
            program1.Locations.Add(country3);
            context.ProgramStatuses.Add(active);
            context.Programs.Add(program1);
            context.LocationTypes.Add(typeRegion);
            context.LocationTypes.Add(typeCountry);
            context.Locations.Add(region1);
            context.Locations.Add(country1);
            context.Locations.Add(country2);
            context.Locations.Add(country3);

            List<int> programIds = new List<int>();
            programIds.Add(program1.ProgramId);

            var results = SnapshotQueries.CreateGetProgramCountriesByProgramIdsQuery(context, programIds);
            Assert.AreEqual(3, results.Select(x => x).Count());
        }

        [TestMethod]
        public void TestCreateGetProgramBeneficiaryCountQuery()
        {
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramImpactStoryCountQuery()
        {
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramAlumniCountQuery()
        {
            var person = new Person
            {
                PersonId = 1
            };

            var person2 = new Person
            {
                PersonId = 2
            };

            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };

            var alumni = new ParticipantStatus
            {
                ParticipantStatusId = ParticipantStatus.Alumnus.Id,
                Status = ParticipantStatus.Alumnus.Value
            };

            var endDate = DateTime.UtcNow;

            var org = new Organization
            {
                OrganizationId = 1,
                OfficeSymbol = "ABCDE"
            };

            var program = new Program
            {
                ProgramId = 1,
                Owner = org,
                OwnerId = org.OrganizationId,
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId
            };

            var project = new Project
            {
                ProjectId = 1,
                ParentProgram = program,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                EndDate = endDate
            };

            var participant = new Participant
            {
                ParticipantId = 1,
                Person = person,
                PersonId = person.PersonId,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantStatusId = alumni.ParticipantStatusId,
                StatusDate = endDate
            };

            var participant2 = new Participant
            {
                ParticipantId = 2,
                Person = person2,
                PersonId = person2.PersonId,
                Project = project,
                ProjectId = project.ProjectId,
                ParticipantStatusId = alumni.ParticipantStatusId,
                StatusDate = endDate
            };

            project.Participants.Add(participant);
            project.Participants.Add(participant2);
            program.Projects.Add(project);

            context.Programs.Add(program);
            context.Projects.Add(project);
            context.Participants.Add(participant);
            context.Participants.Add(participant2);

            List<int> programIds = new List<int>();
            programIds.Add(program.ProgramId);

            var results = SnapshotQueries.CreateGetProgramAlumniCountQuery(context, programIds);
            Assert.AreEqual(2, results.DataValue);
        }

        [TestMethod]
        public void TestCreateGetProgramProminenceCountQuery()
        {
            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };

            var endDate = DateTime.UtcNow;

            var org = new Organization
            {
                OrganizationId = 1,
                OfficeSymbol = "ABCDE"
            };

            var program = new Program
            {
                ProgramId = 1,
                Owner = org,
                OwnerId = org.OrganizationId,
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId
            };

            var project = new Project
            {
                ProjectId = 1,
                ParentProgram = program,
                ProgramId = program.ProgramId,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                EndDate = endDate
            };

            List<Program> programs = new List<Program>();
            programs.Add(program);
            List<Project> projects = new List<Project>();
            projects.Add(project);

            var category1 = new Category
            {
                CategoryId = 1,
                CategoryName = "Cat1",
                Programs = programs,
                Projects = projects
            };

            var category2 = new Category
            {
                CategoryId = 2,
                CategoryName = "Cat1",
                Programs = programs,
                Projects = projects
            };

            var category3 = new Category
            {
                CategoryId = 3,
                CategoryName = "Cat1",
                Programs = programs,
                Projects = projects
            };

            var category4 = new Category
            {
                CategoryId = 4,
                CategoryName = "Cat2",
                Programs = programs,
                Projects = projects
            };

            program.Projects.Add(project);
            project.Categories.Add(category1);
            project.Categories.Add(category2);
            project.Categories.Add(category3);
            project.Categories.Add(category4);

            context.Programs.Add(program);
            context.Projects.Add(project);
            context.Categories.Add(category1);
            context.Categories.Add(category2);
            context.Categories.Add(category3);
            context.Categories.Add(category4);

            List<int> programIds = new List<int>();
            programIds.Add(program.ProgramId);

            List<int> query = SnapshotQueries.CreateGetProgramProminenceCountQuery(context, programIds).ToList();
            var catIds = query.Select(x => x).Distinct().Count();
            Assert.AreEqual(4, catIds);
        }

        [TestMethod]
        public void TestCreateGetProgramBudgetByYearQuery()
        {
            var sourceId = 1;
            var recipientId = 2;
            MoneyFlowSourceRecipientType programType;
            MoneyFlowSourceRecipientType projectType;
            MoneyFlowStatus actual;
            programType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Program.Id,
                TypeName = MoneyFlowSourceRecipientType.Program.Value
            };
            projectType = new MoneyFlowSourceRecipientType
            {
                MoneyFlowSourceRecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                TypeName = MoneyFlowSourceRecipientType.Project.Value
            };
            actual = new MoneyFlowStatus
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                MoneyFlowStatusName = MoneyFlowStatus.Actual.Value
            };

            var endDate = DateTime.UtcNow;

            var program = new Program
            {
                ProgramId = sourceId,
                Name = "program"
            };
            var project = new Project
            {
                ProjectId = recipientId,
                Name = "project",
                ProgramId = program.ProgramId,
                EndDate = endDate
            };
            var moneyFlow = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProjectId = recipientId,
                SourceProgram = program,
                RecipientProject = project,
                SourceType = programType,
                SourceTypeId = programType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 100.00m,
                Description = "desc",
                FiscalYear = 2013,
                MoneyFlowId = 10,
                ParentMoneyFlowId = 100
            };
            var moneyFlow2 = new MoneyFlow
            {
                SourceProgramId = sourceId,
                RecipientProjectId = recipientId,
                SourceProgram = program,
                RecipientProject = project,
                SourceType = programType,
                SourceTypeId = programType.MoneyFlowSourceRecipientTypeId,
                RecipientType = projectType,
                RecipientTypeId = projectType.MoneyFlowSourceRecipientTypeId,
                MoneyFlowStatus = actual,
                MoneyFlowStatusId = actual.MoneyFlowStatusId,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 150.00m,
                Description = "desc",
                FiscalYear = 2014,
                MoneyFlowId = 11,
                ParentMoneyFlowId = 110
            };
            project.RecipientProjectMoneyFlows.Add(moneyFlow);
            project.RecipientProjectMoneyFlows.Add(moneyFlow2);
            program.Projects.Add(project);
            context.Programs.Add(program);
            context.Projects.Add(project);
            context.MoneyFlows.Add(moneyFlow);
            context.MoneyFlows.Add(moneyFlow2);

            List<int> programIds = new List<int>();
            programIds.Add(program.ProgramId);
            var results = SnapshotQueries.CreateGetProgramBudgetByYearQuery(context, programIds);
            Assert.AreEqual(0, results.Result.Sum(x => x.Value));
            Assert.AreEqual(results.Result.Where(y => y.Key == 2013).Select(v => v.Value).FirstOrDefault(), 0);
            Assert.AreEqual(results.Result.Where(y => y.Key == 2014).Select(v => v.Value).FirstOrDefault(), 0);
        }

        [TestMethod]
        public void TestCreateGetProgramMostFundedCountriesQuery()
        {
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramTopThemesQuery()
        {
            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };

            var endDate = DateTime.UtcNow;

            var org = new Organization
            {
                OrganizationId = 1,
                OfficeSymbol = "ABCDE"
            };

            var program = new Program
            {
                ProgramId = 1,
                Owner = org,
                OwnerId = org.OrganizationId,
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId
            };

            List<Program> programs = new List<Program>();
            programs.Add(program);

            var theme1 = new Theme
            {
                ThemeId = 1,
                ThemeName = "Theme 1",
                Programs = programs                
            };

            var theme2 = new Theme
            {
                ThemeId = 2,
                ThemeName = "Theme 1",
                Programs = programs
            };

            var theme3 = new Theme
            {
                ThemeId = 3,
                ThemeName = "Theme 1",
                Programs = programs
            };

            var theme4 = new Theme
            {
                ThemeId = 4,
                ThemeName = "Theme 2",
                Programs = programs
            };

            program.Themes.Add(theme1);
            program.Themes.Add(theme2);
            program.Themes.Add(theme3);
            program.Themes.Add(theme4);
            context.Programs.Add(program);
            context.Themes.Add(theme1);
            context.Themes.Add(theme2);
            context.Themes.Add(theme3);
            context.Themes.Add(theme4);
            
            List<int> programIds = new List<int>();
            programIds.Add(program.ProgramId);

            List<string> results = SnapshotQueries.CreateGetProgramTopThemesQuery(context, programIds).Result.ToList();
            Assert.AreEqual(0, results.FindIndex(t => t == "Theme 1"));
            Assert.AreEqual(1, results.FindIndex(t => t == "Theme 2"));
        }

        [TestMethod]
        public void TestCreateGetProgramParticipantsByLocationQuery()
        {
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramParticipantsByYearQuery()
        {
            var person = new Person
            {
                PersonId = 1
            };

            var person2 = new Person
            {
                PersonId = 2
            };

            var person3 = new Person
            {
                PersonId = 3
            };

            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };

            var status = new ProjectStatus
            {
                ProjectStatusId = 1,
                Status = "status"
            };

            DateTime endDate = DateTime.UtcNow;
            DateTime oldestDate = DateTime.UtcNow.AddYears(-5);
            DateTime project1StartDate = oldestDate.AddYears(2);
            DateTime project2StartDate = oldestDate.AddYears(3);

            var org = new Organization
            {
                OrganizationId = 1,
                OfficeSymbol = "ABCDE"
            };

            var program = new Program
            {
                ProgramId = 1,
                Owner = org,
                OwnerId = org.OrganizationId,
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId
            };

            var project = new Project
            {
                ProjectId = 1,
                ParentProgram = program,
                ProgramId = program.ProgramId,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                StartDate = project1StartDate,
                EndDate = endDate
            };

            var project2 = new Project
            {
                ProjectId = 2,
                ParentProgram = program,
                ProgramId = program.ProgramId,
                Status = status,
                ProjectStatusId = status.ProjectStatusId,
                StartDate = project2StartDate,
                EndDate = endDate
            };

            var participant = new Participant
            {
                ParticipantId = 1,
                Person = person,
                PersonId = person.PersonId,
                Project = project,
                ProjectId = project.ProjectId
            };

            var participant2 = new Participant
            {
                ParticipantId = 2,
                Person = person2,
                PersonId = person2.PersonId,
                Project = project,
                ProjectId = project.ProjectId
            };

            var participant3 = new Participant
            {
                ParticipantId = 3,
                Person = person3,
                PersonId = person3.PersonId,
                Project = project2,
                ProjectId = project2.ProjectId
            };

            project.Participants.Add(participant);
            project.Participants.Add(participant2);
            project2.Participants.Add(participant3);
            program.Projects.Add(project);
            program.Projects.Add(project2);

            context.Programs.Add(program);
            context.Projects.Add(project);
            context.Projects.Add(project2);
            context.Participants.Add(participant);
            context.Participants.Add(participant2);
            context.Participants.Add(participant3);

            List<int> programIds = new List<int>();
            programIds.Add(program.ProgramId);

            var results = SnapshotQueries.CreateGetProgramParticipantsByYearQuery(context, programIds);
            Assert.IsTrue(results.Result.values.Where(x => x.Key == project1StartDate.Year).Select(v => v.Value).FirstOrDefault() == 2);
            Assert.IsTrue(results.Result.values.Where(x => x.Key == project2StartDate.Year).Select(v => v.Value).FirstOrDefault() == 1);
        }

        [TestMethod]
        public void TestCreateGetProgramParticipantGenderQuery()
        {
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramParticipantAgeQuery()
        {
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramParticipantEducationQuery()
        {
            Assert.AreEqual("todo", "todo");
        }
        
    }
}
