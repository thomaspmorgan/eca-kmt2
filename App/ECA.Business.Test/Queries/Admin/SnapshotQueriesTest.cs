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
            Assert.AreEqual("todo", "todo");
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
            Assert.AreEqual(3, results.Count());
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
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramProminenceCountQuery()
        {
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramBudgetByYearQuery()
        {
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramMostFundedCountriesQuery()
        {
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramTopThemesQuery()
        {
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramParticipantsByLocationQuery()
        {
            Assert.AreEqual("todo", "todo");
        }

        [TestMethod]
        public void TestCreateGetProgramParticipantsByYearQuery()
        {
            Assert.AreEqual("todo", "todo");
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
