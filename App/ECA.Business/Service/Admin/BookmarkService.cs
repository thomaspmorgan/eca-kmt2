using ECA.Business.Exceptions;
using ECA.Business.Queries.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Exceptions;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Bookmark service implementation
    /// </summary>
    public class BookmarkService : DbContextService<EcaContext>, IBookmarkService
    {
        
        /// <summary>
        /// Model has less than or more than one resource error
        /// </summary>
        public const string MODEL_HAS_LESS_THAN_OR_MORE_THAN_ONE_RESOURCE_ID_ERROR = "The model must contain exactly one resource id.";

        /// <summary>
        /// Bookmark already exists error
        /// </summary>
        public const string BOOKMARK_ALREADY_EXISTS_ERROR = "The bookmark cannot be created, it already exists.";

        /// <summary>
        /// Resource does not exist error
        /// </summary>
        public const string RESOURCE_DOES_NOT_EXIST_ERROR = "The specified resource id does not exist.";

        /// <summary>
        /// Bookmark not found error
        /// </summary>
        public const string BOOKMARK_NOT_FOUND_ERROR = "The bookmark could not be found.";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context">The context to query</param>
        public BookmarkService(EcaContext context) : base(context)
        {
          Contract.Requires(context != null, "The context must not be null");
        }

        /// <summary>
        /// Creates a bookmark asyncronously
        /// </summary>
        /// <param name="newBookmark">The model to create</param>
        /// <returns>The created bookmark</returns>
        public async Task<Bookmark> CreateBookmarkAsync(NewBookmark newBookmark)
        {
            var modelHasOneResourceId = ModelHasOneResourceId(newBookmark);

            if (!modelHasOneResourceId)
            {
                throw new EcaBusinessException(MODEL_HAS_LESS_THAN_OR_MORE_THAN_ONE_RESOURCE_ID_ERROR);
            }

            var bookmarkAlreadyExists = BookmarkAlreadyExists(newBookmark);

            if (bookmarkAlreadyExists)
            {
                throw new EcaBusinessException(BOOKMARK_ALREADY_EXISTS_ERROR);
            }

            var resourceExists = await ResourceExists(newBookmark);

            if (!resourceExists)
            {
                throw new ModelNotFoundException(RESOURCE_DOES_NOT_EXIST_ERROR); 
            }
            
            var bookmark = DoCreate(newBookmark);
            return bookmark;
        }

        private bool ModelHasOneResourceId(NewBookmark newBookmark)
        {
            var hasOneId = false;

            if ((newBookmark.OfficeId != null && newBookmark.ProgramId == null && newBookmark.ProjectId == null && newBookmark.PersonId == null && newBookmark.OrganizationId == null) ||
                (newBookmark.OfficeId == null && newBookmark.ProgramId != null && newBookmark.ProjectId == null && newBookmark.PersonId == null && newBookmark.OrganizationId == null) ||
                (newBookmark.OfficeId == null && newBookmark.ProgramId == null && newBookmark.ProjectId != null && newBookmark.PersonId == null && newBookmark.OrganizationId == null) ||
                (newBookmark.OfficeId == null && newBookmark.ProgramId == null && newBookmark.ProjectId == null && newBookmark.PersonId != null && newBookmark.OrganizationId == null) ||
                (newBookmark.OfficeId == null && newBookmark.ProgramId == null && newBookmark.ProjectId == null && newBookmark.PersonId == null && newBookmark.OrganizationId != null))
            {
                hasOneId = true;
            }

            return hasOneId;
        }

        private bool BookmarkAlreadyExists(NewBookmark newBookmark)
        {
            var bookmark = Context.Bookmarks.Where(x => x.OfficeId == newBookmark.OfficeId && 
                                                        x.ProgramId == newBookmark.ProgramId &&
                                                        x.ProjectId == newBookmark.ProjectId &&
                                                        x.PersonId == newBookmark.PersonId &&
                                                        x.OrganizationId == newBookmark.OrganizationId &&
                                                        x.PrincipalId == newBookmark.PrincipalId).FirstOrDefault();
            return bookmark != null;
        }

        private async Task<bool> ResourceExists(NewBookmark newBookmark) {

            Object resource = null;

            if (newBookmark.OfficeId != null)
            {
                resource = await Context.Organizations.FindAsync(newBookmark.OfficeId);
            }
            else if (newBookmark.ProgramId != null)
            {
                resource = await Context.Programs.FindAsync(newBookmark.ProgramId);
            }
            else if (newBookmark.ProjectId != null)
            {
                resource = await Context.Projects.FindAsync(newBookmark.ProjectId);
            }
            else if (newBookmark.PersonId != null)
            {
                resource = await Context.People.FindAsync(newBookmark.PersonId);
            }
            else if (newBookmark.OrganizationId != null)
            {
                resource = await Context.Organizations.FindAsync(newBookmark.OrganizationId);
            }

            return resource != null;

        }

        private Bookmark DoCreate(NewBookmark newBookmark)
        {
            var bookmark = new Bookmark
            {
                OfficeId = newBookmark.OfficeId,
                ProgramId = newBookmark.ProgramId,
                ProjectId = newBookmark.ProjectId,
                PersonId = newBookmark.PersonId,
                OrganizationId = newBookmark.OrganizationId,
                PrincipalId = newBookmark.PrincipalId,
                AddedOn = DateTimeOffset.Now,
                Automatic = newBookmark.Automatic
            };

            this.Context.Bookmarks.Add(bookmark);

            return bookmark;
        }

        /// <summary>
        /// Deletes a bookmark asyncronously
        /// </summary>
        /// <param name="id">The id to delete</param>
        /// <returns></returns>
        public async Task DeleteBookmarkAsync(int id)
        {
            var bookmark = await Context.Bookmarks.FindAsync(id);

            if (bookmark != null)
            {
                Context.Bookmarks.Remove(bookmark);
            }
            else
            {
                throw new ModelNotFoundException(BOOKMARK_NOT_FOUND_ERROR);
            }
        }

        /// <summary>
        /// Get bookmarks asyncronously
        /// </summary>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns>List of bookmarks</returns>
        public Task<PagedQueryResults<BookmarkDTO>> GetBookmarksAsync(QueryableOperator<BookmarkDTO> queryOperator)
        {
            var query = BookmarkQueries.CreateGetBookmarksQuery(Context);
            query = query.Apply(queryOperator);
            return query.ToPagedQueryResultsAsync(queryOperator.Start, queryOperator.Limit);
        }

    }
}
