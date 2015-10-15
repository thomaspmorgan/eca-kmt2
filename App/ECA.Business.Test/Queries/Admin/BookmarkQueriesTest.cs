using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Queries.Admin;
using FluentAssertions;

namespace ECA.Business.Test.Queries.Admin
{
    [TestClass]
    public class BookmarkQueriesTest
    {

        private TestEcaContext context;
        
        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestCreateGetBookmarksQuery_Office() {

            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Office.Id,
                OrganizationTypeName = OrganizationType.Office.Value
            };

            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                OfficeSymbol = "officeSymbol",
                Name = "name"
            };
            var now = DateTimeOffset.Now;
            var bookmark = new Bookmark
            {
                BookmarkId = 1,
                OfficeId = office.OrganizationId,
                PrincipalId = 1,
                AddedOn = now,
                Automatic = false
            };

            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(office);
            context.Bookmarks.Add(bookmark);
            
            var bookmarks = BookmarkQueries.CreateGetBookmarksQuery(context);
            var firstResult = bookmarks.First();

            Assert.AreEqual(bookmark.BookmarkId, firstResult.BookmarkId);
            Assert.AreEqual(bookmark.OfficeId, firstResult.OfficeId);
            Assert.AreEqual(bookmark.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(bookmark.Automatic, firstResult.Automatic);
            Assert.AreEqual(now, bookmark.AddedOn);
            Assert.AreEqual("Office", firstResult.Type);
            Assert.AreEqual(office.OfficeSymbol, firstResult.OfficeSymbolOrStatus);
            Assert.AreEqual(office.Name, firstResult.Name);
        }

        [TestMethod]
        public void TestCreateGetBookmarksQuery_Program() {

            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };

            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Office.Id,
                OrganizationTypeName = OrganizationType.Office.Value
            };

            var owner1 = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                Name = "owner 1",
                OfficeSymbol = "owner 1 symbol",
            };

            var program1 = new Program
            {
                ProgramId = 1,
                Description = "desc1",
                History = new History
                {
                    CreatedBy = 1,
                },
                Name = "program 1",
                Owner = owner1,
                OwnerId = owner1.OrganizationId,
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId,
            };

            var bookmark = new Bookmark
            {
                BookmarkId = 1,
                ProgramId = program1.ProgramId,
                PrincipalId = 1,
                AddedOn = DateTimeOffset.Now,
                Automatic = false
            };

            context.OrganizationTypes.Add(organizationType);
            context.Programs.Add(program1);
            context.Organizations.Add(owner1);
            context.ProgramStatuses.Add(active);
            context.Bookmarks.Add(bookmark);
            
            var bookmarks = BookmarkQueries.CreateGetBookmarksQuery(context);
            var firstResult = bookmarks.First();

            Assert.AreEqual(bookmark.BookmarkId, firstResult.BookmarkId);
            Assert.AreEqual(bookmark.ProgramId, firstResult.ProgramId);
            Assert.AreEqual(bookmark.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(bookmark.Automatic, firstResult.Automatic);
            bookmark.AddedOn.Should().BeCloseTo(DateTimeOffset.Now, 3000);
            Assert.AreEqual("Program", firstResult.Type);
            Assert.AreEqual(owner1.OfficeSymbol, firstResult.OfficeSymbolOrStatus);
            Assert.AreEqual(program1.Name, firstResult.Name);
        }

        [TestMethod]
        public void TestCreateGetBookmarksQuery_Project() {

            var active = new ProgramStatus
            {
                ProgramStatusId = ProgramStatus.Active.Id,
                Status = ProgramStatus.Active.Value
            };

            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Office.Id,
                OrganizationTypeName = OrganizationType.Office.Value
            };

            var owner1 = new Organization
            {
                OrganizationId = 1,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                Name = "owner 1",
                OfficeSymbol = "owner 1 symbol",
            };

            var program1 = new Program
            {
                ProgramId = 1,
                Description = "desc1",
                History = new History
                {
                    CreatedBy = 1,
                },
                Name = "program 1",
                Owner = owner1,
                OwnerId = owner1.OrganizationId,
                ProgramStatus = active,
                ProgramStatusId = active.ProgramStatusId,
            };

            var projectStatus = new ProjectStatus
            {
                Status = "status",
                ProjectStatusId = 1
            };
            var project = new Project
            {
                ProgramId = program1.ProgramId,
                Name = "project",
                ParentProgram = program1,
                ProjectId = 1,
                StartDate = DateTimeOffset.Now,
                Status = projectStatus,
                ProjectStatusId = projectStatus.ProjectStatusId
            };

            var bookmark = new Bookmark
            {
                BookmarkId = 1,
                OfficeId = owner1.OrganizationId,
                ProgramId = program1.ProgramId,
                ProjectId = project.ProjectId,
                PrincipalId = 1,
                AddedOn = DateTimeOffset.Now,
                Automatic = false
            };

            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(owner1);
            context.Programs.Add(program1);
            context.ProgramStatuses.Add(active);
            context.Projects.Add(project);
            context.ProjectStatuses.Add(projectStatus);
            context.Bookmarks.Add(bookmark);
            
            var bookmarks = BookmarkQueries.CreateGetBookmarksQuery(context);
            var firstResult = bookmarks.First();

            Assert.AreEqual(bookmark.BookmarkId, firstResult.BookmarkId);
            Assert.AreEqual(bookmark.OfficeId, firstResult.OfficeId);
            Assert.AreEqual(bookmark.ProgramId, firstResult.ProgramId);
            Assert.AreEqual(bookmark.ProjectId, firstResult.ProjectId);
            Assert.AreEqual(bookmark.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(bookmark.Automatic, firstResult.Automatic);
            bookmark.AddedOn.Should().BeCloseTo(DateTimeOffset.Now, 3000);
            Assert.AreEqual("Project", firstResult.Type);
            Assert.AreEqual(owner1.OfficeSymbol, firstResult.OfficeSymbolOrStatus);
            Assert.AreEqual(project.Name, firstResult.Name);
        }

        [TestMethod]
        public void TestCreateGetBookmarksQuery_Person()
        {
            var status = new ParticipantStatus {
                ParticipantStatusId = 1,
                Status = "status"
            };

            var participation = new Participant
            {
                ParticipantId = 1,
                ParticipantStatusId = status.ParticipantStatusId,
                Status = status
            };

            var person = new Person
            {
                PersonId = 1,
                Alias = "alias",
                FamilyName = "family",
                FirstName = "firstName",
                GivenName = "givenName",
                LastName = "lastName",
                MiddleName = "middleName",
                NamePrefix = "Mr.",
                NameSuffix = "III",
                Patronym = "patronym",
                Gender = new Gender
                {
                    GenderId = Gender.Female.Id,
                    GenderName = Gender.Female.Value
                }
            };

            person.Participations.Add(participation);

            var bookmark = new Bookmark
            {
                BookmarkId = 1,
                PersonId = person.PersonId,
                PrincipalId = 1,
                AddedOn = DateTimeOffset.Now,
                Automatic = false
            };

            var expectedName = String.Format("{0} {1} {2} {3} {4} {5} ({6})",
                person.NamePrefix,
                person.FirstName,
                person.MiddleName,
                person.LastName,
                person.Patronym,
                person.NameSuffix,
                person.Alias);

            context.ParticipantStatuses.Add(status);
            context.Participants.Add(participation);
            context.People.Add(person);
            context.Bookmarks.Add(bookmark);

            var bookmarks = BookmarkQueries.CreateGetBookmarksQuery(context);
            var firstResult = bookmarks.First();

            Assert.AreEqual(bookmark.BookmarkId, firstResult.BookmarkId);
            Assert.AreEqual(bookmark.PersonId, firstResult.PersonId);
            Assert.AreEqual(bookmark.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(bookmark.Automatic, firstResult.Automatic);
            bookmark.AddedOn.Should().BeCloseTo(DateTimeOffset.Now, 3000);
            Assert.AreEqual("Person", firstResult.Type);
            Assert.AreEqual(status.Status, firstResult.OfficeSymbolOrStatus);
            Assert.AreEqual(expectedName, firstResult.Name);
        }

        [TestMethod]
        public void TestCreateGetBookmarksQuery_Organization()
        {
            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Other.Id,
                OrganizationTypeName = OrganizationType.Other.Value
            };

            var organization = new Organization
            {
                OrganizationId = 1,
                OrganizationType = organizationType,
                OrganizationTypeId = organizationType.OrganizationTypeId,
                Name = "name",
                Status = "status"
            };

            var bookmark = new Bookmark
            {
                BookmarkId = 1,
                OrganizationId = organization.OrganizationId,
                PrincipalId = 1,
                AddedOn = DateTimeOffset.Now,
                Automatic = false
            };

            context.OrganizationTypes.Add(organizationType);
            context.Organizations.Add(organization);
            context.Bookmarks.Add(bookmark);

            var bookmarks = BookmarkQueries.CreateGetBookmarksQuery(context);
            var firstResult = bookmarks.First();

            Assert.AreEqual(bookmark.BookmarkId, firstResult.BookmarkId);
            Assert.AreEqual(bookmark.OrganizationId, firstResult.OrganizationId);
            Assert.AreEqual(bookmark.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(bookmark.Automatic, firstResult.Automatic);
            bookmark.AddedOn.Should().BeCloseTo(DateTimeOffset.Now, 3000);
            Assert.AreEqual("Organization", firstResult.Type);
            Assert.AreEqual(organization.Status, firstResult.OfficeSymbolOrStatus);
            Assert.AreEqual(organization.Name, firstResult.Name);
        }
    }
}
