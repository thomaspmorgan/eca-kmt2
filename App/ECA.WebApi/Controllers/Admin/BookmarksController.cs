﻿using ECA.Business.Service.Admin;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace ECA.WebApi.Controllers.Admin
{ 
    /// <summary>
    /// Bookmarks controller
    /// </summary>
    [RoutePrefix("api/bookmarks")]
    public class BookmarksController : ApiController
    {

        private IUserProvider userProvider;
        private IBookmarkService service;

        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="userProvider">The user provider service</param>
        /// <param name="service">The bookmarks service</param>
        public BookmarksController(IUserProvider userProvider, IBookmarkService service)
        {
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            Contract.Requires(service != null, "The bookmarks service must not be null.");
            this.userProvider = userProvider;
            this.service = service;
        }

        /// <summary>
        /// Post a bookmark
        /// </summary>
        /// <param name="model">The model to create</param>
        /// <returns>OkResult</returns>
        [ResponseType(typeof(OkResult))]
        public async Task<IHttpActionResult> PostBookmarkAsync(BookmarkBindingModel model) {
            if(ModelState.IsValid) {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await service.CreateBookmarkAsync(model.ToNewBookmark(businessUser));
                await service.SaveChangesAsync();
                return Ok();
            } else {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Delete a bookmark
        /// </summary>
        /// <param name="id">The id to delete</param>
        /// <returns>OkResult</returns>
        [ResponseType(typeof(OkResult))]
        public async Task<IHttpActionResult> DeleteBookmarkAsync(int id) {
            await service.DeleteBookmarkAsync(id);
            await service.SaveChangesAsync();
            return Ok();
        }

    }
}
