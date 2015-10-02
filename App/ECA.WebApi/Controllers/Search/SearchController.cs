﻿using ECA.Business.Search;
using ECA.WebApi.Models.Search;
using ECA.WebApi.Security;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers.Search
{
    /// <summary>
    /// The search controller provides a means to perform fast full text searchs of the ECA system.
    /// </summary>
    [Authorize]
    [RoutePrefix("api")]
    public class SearchController : ApiController
    {
        private readonly IIndexService indexService;
        private readonly IUserProvider userProvider;

        /// <summary>
        /// Creates a new SearchController with the given index service and user provider.
        /// </summary>
        /// <param name="indexService">The index service.</param>
        /// <param name="userProvider">The user provider.</param>
        public SearchController(IIndexService indexService, IUserProvider userProvider)
        {
            Contract.Requires(indexService != null, "The index service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.indexService = indexService;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Performs a search of the ECA system.
        /// </summary>
        /// <param name="search">The search parameters.</param>
        /// <returns>The responsive documents.</returns>
        [Route("Search")]
        [ResponseType(typeof(DocumentSearchResponseViewModel))]
        public async Task<IHttpActionResult> GetSearchDocumentsAsync([FromUri]ECASearchParametersBindingModel search)
        {
            if (ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var permissions = await this.userProvider.GetPermissionsAsync(currentUser);
                var searchResults = await this.indexService.SearchAsync(search.ToECASearchParameters(permissions), null);
                return Ok(new DocumentSearchResponseViewModel(searchResults));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Performs a search of the ECA system.
        /// </summary>
        /// <param name="id">The full id of the document.</param>
        /// <returns>The responsive documents.</returns>
        [Route("Documents/{id}")]
        [ResponseType(typeof(ECADocumentViewModel))]
        public async Task<IHttpActionResult> GetDocumentByIdAsync(string id)
        {
            //var currentUser = this.userProvider.GetCurrentUser();
            //var businessUser = this.userProvider.GetBusinessUser(currentUser);
            var document = await this.indexService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(new ECADocumentViewModel(document));
            }
        }

        /// <summary>
        /// Deletes the search index and all documents in it.
        /// </summary>
        /// <param name="id">The id of the application to delete the search index for.  KMT is 1.</param>
        /// <returns>An Ok Result.</returns>
        [Route("Index/{id:int}")]
        [ResourceAuthorize(CAM.Data.Permission.ADMINISTRATOR_VALUE, CAM.Data.ResourceType.APPLICATION_VALUE)]
        public async Task<IHttpActionResult> DeleteIndexAsync(int id)
        {
            await this.indexService.DeleteIndexAsync(IndexService.INDEX_NAME);
            return Ok();
        }
    }
}
