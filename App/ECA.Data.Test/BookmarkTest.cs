using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ECA.Data.Test
{
    [TestClass]
    public class BookmarkTest
    {
        private TestEcaContext context;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
        }

        [TestMethod]
        public void TestBookMarkWithOffice()
        {
            var bookmark = new Bookmark
            {
                BookmarkId = 1,
                OfficeId = 2,
            };

            var office = new Organization
            {
                OrganizationId = 2,
                Name = "Test Office"
            };

            bookmark.Office = office;

            Assert.AreEqual(bookmark.Office.Name, office.Name);
            Assert.AreEqual(bookmark.Office.OrganizationId, office.OrganizationId);

        }


        [TestMethod]
        public void TestBookMarkWithProgram()
        {
            var bookmark = new Bookmark
            {
                BookmarkId = 1,
                ProgramId = 2,
            };

            var program = new Program
            {
                ProgramId = 2,
                Name = "Test Project"
            };

            bookmark.Program = program;

            Assert.AreEqual(bookmark.Program.Name, program.Name);
            Assert.AreEqual(bookmark.Program.ProgramId, program.ProgramId);

        }


        [TestMethod]
        public void TestBookMarkWithProject()
        {
            var bookmark = new Bookmark
            {
                BookmarkId = 1,
                ProjectId = 2,
            };

            var project = new Project
            {
                ProjectId = 2,
                Name = "Test Project"
            };

            bookmark.Project = project;

            Assert.AreEqual(bookmark.Project.Name, project.Name);
            Assert.AreEqual(bookmark.Project.ProjectId, project.ProjectId);

        }


        [TestMethod]
        public void TestBookMarkWithOrganization()
        {
            var bookmark = new Bookmark
            {
                BookmarkId = 1,
                OrganizationId = 2,
            };

            var org = new Organization
            {
                OrganizationId = 2,
                Name = "Test Org"
            };

            bookmark.Organization = org;

            Assert.AreEqual(bookmark.Organization.Name, org.Name);
            Assert.AreEqual(bookmark.Organization.OrganizationId, org.OrganizationId);

        }


        [TestMethod]
        public void TestBookMarkWithPrincipal()
        {
            var bookmark = new Bookmark
            {
                BookmarkId = 1,
                PrincipalId = 2,
            };

            var user = new UserAccount
            {
                PrincipalId = 2,
                FirstName = "Jack"
            };

            bookmark.User = user;

            Assert.AreEqual(bookmark.User.FirstName, user.FirstName);
            Assert.AreEqual(bookmark.User.PrincipalId, user.PrincipalId);

        }


        [TestMethod]
        public void TestBookMarkWithAutomatic()
        {
            var bookmark = new Bookmark
            {
                BookmarkId = 1
            };
            // default is Automatic = false
            Assert.IsFalse(bookmark.Automatic);
            bookmark.Automatic = true;
            Assert.IsTrue(bookmark.Automatic);

        }

    }
}