﻿using ECA.Business.Exceptions;
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

        public const string MODEL_HAS_LESS_THAN_OR_MORE_THAN_ONE_RESOURCE_ID_ERROR = "The model must contain exactly one resource id.";
        public const string BOOKMARK_ALREADY_EXISTS_ERROR = "The bookmark cannot be created, it already exists.";
        public const string RESOURCE_DOES_NOT_EXIST_ERROR = "The specified resource id does not exist.";

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
                throw new EcaBusinessException();
            }

            var resourceExists = await ResourceExists(newBookmark);

            if (!resourceExists)
            {
                throw new EcaBusinessException(BOOKMARK_ALREADY_EXISTS_ERROR);
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
        public async Task DeleteBookmarkAsync(int id)
        {
            var bookmark = Context.Bookmarks.Find(id);
            if (bookmark != null)
            {
                Context.Bookmarks.Remove(bookmark);
            }
        }
    }
}
