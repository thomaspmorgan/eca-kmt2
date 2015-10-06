using ECA.Business.Search;
using System.Linq;
using ECA.WebApi.Models.Search;
using ECA.WebApi.Security;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Collections.Generic;

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
        public async Task<IHttpActionResult> PostSearchDocumentsAsync([FromUri]ECASearchParametersBindingModel search)
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
        /// <param name="model">The suggestion model.</param>
        /// <returns>The responsive documents.</returns>
        [Route("Search/Suggest")]
        [ResponseType(typeof(DocumentSearchResponseViewModel))]
        public async Task<IHttpActionResult> PostSuggestionsAsync([FromUri]ECASuggestionParametersBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var searchResults = await this.indexService.GetSuggestionsAsync(model.ToECASuggestionParameters(null), null);                
                return Ok(new DocumentSuggestResponseViewModel(searchResults));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the names of the document fields.
        /// </summary>
        /// <returns>The names of the document fields.</returns>
        [Route("Documents/Fields")]
        [ResponseType(typeof(List<string>))]
        public IHttpActionResult GetDocumentFieldNames()
        {
            return Ok(this.indexService.GetDocumentFieldNames().ToList());
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
        /// <param name="model">The model representing the client request to delete.</param>
        /// <returns>An Ok Result.</returns>
        [Route("Index")]
        [ResourceAuthorize(CAM.Data.Permission.ADMINISTRATOR_VALUE, CAM.Data.ResourceType.APPLICATION_VALUE, typeof(DeleteIndexBindingModel), "ApplicationId")]
        public async Task<IHttpActionResult> DeleteIndexAsync(DeleteIndexBindingModel model)
        {
            await this.indexService.DeleteIndexAsync(model.IndexName);
            return Ok();
        }
    }
}
