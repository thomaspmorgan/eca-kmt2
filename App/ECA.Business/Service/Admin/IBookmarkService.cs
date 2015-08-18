using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECA.Business.Service.Admin
{
    /// <summary>
    /// Bookmark service interface
    /// </summary>
    public interface IBookmarkService : ISaveable
    {
        /// <summary>
        /// Creates a new bookmark
        /// </summary>
        /// <param name="newBookmark">The model to create</param>
        /// <returns>The bookmark to create</returns>
        Task<Bookmark> CreateBookmarkAsync(NewBookmark newBookmark);

        /// <summary>
        /// Deletes a bookmark 
        /// </summary>
        /// <param name="id">The id to delete</param>
        /// <returns></returns>
        Task DeleteBookmarkAsync(int id);

        /// <summary>
        /// Gets a list of bookmarks
        /// </summary>
        /// <param name="principalId">The principal id to query</param>
        /// <param name="queryOperator">The query operator to apply</param>
        /// <returns></returns>
        Task<PagedQueryResults<BookmarkDTO>> GetBookmarksAsync(QueryableOperator<BookmarkDTO> queryOperator);
    }
}
