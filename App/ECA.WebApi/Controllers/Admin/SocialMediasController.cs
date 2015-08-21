using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Models.Query;
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
    /// The SocialMediasController is capapble of handling updates on social media within the ECA system.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/SocialMedias")]
    public class SocialMediasController : ApiController
    {
        private static ExpressionSorter<SocialMediaTypeDTO> DEFAULT_SORTER = new ExpressionSorter<SocialMediaTypeDTO>(x => x.Name, SortDirection.Ascending);
        private readonly ISocialMediaTypeService socialMediaTypeService;
        private readonly ISocialMediaService socialMediaService;
        private readonly IUserProvider userProvider;

        /// <summary>
        /// Creates a new instance of the controller.
        /// </summary>
        /// <param name="userProvider">The user provider.</param>
        /// <param name="socialMediaTypeService">The social media type service.</param>
        /// <param name="socialMediaService">The social media service.</param>
        public SocialMediasController(IUserProvider userProvider, ISocialMediaTypeService socialMediaTypeService, ISocialMediaService socialMediaService)
        {
            Contract.Requires(socialMediaTypeService != null, "The social media type service must not be null.");
            Contract.Requires(socialMediaService != null, "The social media service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.socialMediaTypeService = socialMediaTypeService;
            this.socialMediaService = socialMediaService;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Returns the social media types currently in the system.
        /// </summary>
        /// <returns>The social media types.</returns>
        [Route("Types")]
        [ResponseType(typeof(SocialMediaTypeDTO))]
        public async Task<IHttpActionResult> GetSocialMediaTypesAsync([FromUri]PagingQueryBindingModel<SocialMediaTypeDTO> model)
        {
            if (ModelState.IsValid)
            {
                var dtos = await socialMediaTypeService.GetAsync(model.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(dtos);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
