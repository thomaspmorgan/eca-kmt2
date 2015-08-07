using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ECA.Business.Service.Admin;
using ECA.Business.Service;
using ECA.Data;
using FluentAssertions;
using ECA.Business.Exceptions;
using ECA.Core.Exceptions;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.DynamicLinq;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class BookmarkServiceTest
    {
        private TestEcaContext context;
        private BookmarkService service;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            service = new BookmarkService(context);
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public async Task TestCreateBookmarkAsync()
        {
            var organizationType = new OrganizationType
            {
                OrganizationTypeId = OrganizationType.Office.Id,
                OrganizationTypeName = OrganizationType.Office.Value
            };

            var office = new Organization
            {
                OrganizationId = 1,
                OrganizationType = organizationType
            };

            context.Organizations.Add(office);

            var newBookmark = new NewBookmark(1, null, null, null, null, 1, false);
            var bookmark = await service.CreateBookmarkAsync(newBookmark);

            Assert.AreEqual(newBookmark.OfficeId, bookmark.OfficeId);
            Assert.AreEqual(newBookmark.ProgramId, bookmark.ProgramId);
            Assert.AreEqual(newBookmark.ProjectId, bookmark.ProjectId);
            Assert.AreEqual(newBookmark.PersonId, bookmark.PersonId);
            Assert.AreEqual(newBookmark.OrganizationId, bookmark.OrganizationId);
            Assert.AreEqual(newBookmark.PrincipalId, bookmark.PrincipalId);
            Assert.AreEqual(newBookmark.Automatic, bookmark.Automatic);
            bookmark.AddedOn.Should().BeCloseTo(new DateTimeOffset());
        }

        [TestMethod]
        public async Task TestCreateBookmarkAsync_ModelHasNoResourceId()
        {
            var newBookmark = new NewBookmark(null, null, null, null, null, 1, false);
            Func<Task> act = async () => { await service.CreateBookmarkAsync(newBookmark); };
            act.ShouldThrow<EcaBusinessException>()
                .WithMessage(BookmarkService.MODEL_HAS_LESS_THAN_OR_MORE_THAN_ONE_RESOURCE_ID_ERROR);
        }

        [TestMethod]
        public async Task TestCreateBookmarkAsync_ModelHasMoreThanOneResourceId()
        {
            var newBookmark = new NewBookmark(1, 1, null, null, null, 1, false);
            Func<Task> act = async () => { await service.CreateBookmarkAsync(newBookmark); };
            act.ShouldThrow<EcaBusinessException>()
                .WithMessage(BookmarkService.MODEL_HAS_LESS_THAN_OR_MORE_THAN_ONE_RESOURCE_ID_ERROR);
        }
        
        [TestMethod]
        public async Task TestCreateBookmarkAsync_BookmarkAlreadyExists()
        {
            var bookmark = new Bookmark
            {
                OfficeId = 1,
                PrincipalId = 1,
                Automatic = false
            };
            context.Bookmarks.Add(bookmark);

            var newBookmark = new NewBookmark(1, null, null, null, null, 1, false);

            Func<Task> act = async () => { await service.CreateBookmarkAsync(newBookmark); };
            act.ShouldThrow<EcaBusinessException>()
                .WithMessage(BookmarkService.BOOKMARK_ALREADY_EXISTS_ERROR);
        }

        [TestMethod]
        public async Task TestCreateBookmarkAsync_ResourceDoesNotExist()
        {
            var newBookmark = new NewBookmark(1, null, null, null, null, 1, false);

            Func<Task> act = async () => { await service.CreateBookmarkAsync(newBookmark); };
            act.ShouldThrow<ModelNotFoundException>()
                .WithMessage(BookmarkService.RESOURCE_DOES_NOT_EXIST_ERROR);
        }

        [TestMethod]
        public async Task TestDeleteBookmarkAsync()
        {
            var bookmark = new Bookmark
            {
                BookmarkId = 1,
                OfficeId = 1,
                PrincipalId = 1,
                Automatic = false
            };
            context.Bookmarks.Add(bookmark);

            await service.DeleteBookmarkAsync(1);

            Assert.AreEqual(0, context.Bookmarks.Count());
        }

        [TestMethod]
        public async Task TestDeleteBookmarkAsync_IdDoesNotExist()
        {
            Func<Task> act = async () => { await service.DeleteBookmarkAsync(1); };
            act.ShouldThrow<ModelNotFoundException>()
                .WithMessage(BookmarkService.BOOKMARK_NOT_FOUND_ERROR);
        }

        [TestMethod]
        public async Task TestGetBookmarksAsync()
        {
            var bookmark = new Bookmark
            {
                BookmarkId = 1,
                OfficeId = 1,
                PrincipalId = 1,
                Automatic = false
            };

            context.Bookmarks.Add(bookmark);

            var defaultSorter = new ExpressionSorter<BookmarkDTO>(x => x.AddedOn, SortDirection.Ascending);
            var queryOperator = new QueryableOperator<BookmarkDTO>(0, 10, defaultSorter);

            var response = await service.GetBookmarksAsync(queryOperator);

            Assert.AreEqual(1, response.Total);
            Assert.AreEqual(1, response.Results.Count);

            var firstResult = response.Results.First();
            Assert.AreEqual(bookmark.BookmarkId, firstResult.BookmarkId);
            Assert.AreEqual(bookmark.OfficeId, firstResult.OfficeId);
            Assert.AreEqual(bookmark.PrincipalId, firstResult.PrincipalId);
            Assert.AreEqual(bookmark.Automatic, firstResult.Automatic);
        }
    }
}
