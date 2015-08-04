using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
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
            var bookmark = new Bookmark
            {
                OfficeId = newBookmark.OfficeId,
                ProgramId = newBookmark.ProgramId,
                ProjectId = newBookmark.ProjectId,
                OrganizationId = newBookmark.OrganizationId,
                PrincipalId = newBookmark.PrincipalId,
                AddedOn = new DateTimeOffset(),
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
        public async Task DeleteBookmark(int id)
        {
            var bookmark = Context.Bookmarks.Find(id);
            if (bookmark != null)
            {
                Context.Bookmarks.Remove(bookmark);
            }
        }
    }
}
