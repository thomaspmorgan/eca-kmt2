using ECA.Business.Search;
using ECA.Data;
using ECA.WebApi.Models.Search;
using ECA.WebApi.Security;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            var currentUser = this.userProvider.GetCurrentUser();
            var businessUser = this.userProvider.GetBusinessUser(currentUser);
            var searchResults = await this.indexService.SearchAsync(search.ToECASearchParameters(), null);
            return Ok(new DocumentSearchResponseViewModel(searchResults));
        }

        [Route("Search/Index")]
        public IHttpActionResult PostProcess()
        {
            var currentUser = this.userProvider.GetCurrentUser();
            var businessUser = this.userProvider.GetBusinessUser(currentUser);
            var context = (EcaContext)this.Configuration.DependencyResolver.GetService(typeof(EcaContext));
            var indexService = (IIndexService)this.Configuration.DependencyResolver.GetService(typeof(IIndexService));
            var notificationService = new FileIndexNotificationService();
            var documentService = new ProgramDocumentService(context, indexService, notificationService);
            documentService.Process();
            Console.WriteLine("done...");
            documentService.Dispose();
            return Ok();
        }
    }
}
