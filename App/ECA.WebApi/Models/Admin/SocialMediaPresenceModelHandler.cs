using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Data;
using ECA.WebApi.Security;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The SocialMediaPresenceModelHandler will be used to handle new social media presences to ISocialable entities in the ECA system.
    /// </summary>
    public class SocialMediaPresenceModelHandler : ECA.WebApi.Models.Admin.ISocialMediaPresenceModelHandler
    {
        private readonly ISocialMediaService socialMediaService;
        private readonly IUserProvider userProvider;

        /// <summary>
        /// Creates a new AddressModelHandler
        /// </summary>
        /// <param name="socialMediaService">The social media service.</param>
        /// <param name="userProvider">The user provider to get current user information.</param>
        public SocialMediaPresenceModelHandler(ISocialMediaService socialMediaService, IUserProvider userProvider)
        {
            Contract.Requires(socialMediaService != null, "The social media service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.socialMediaService = socialMediaService;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Handles a controller action's request to add a social media presence to an entity.
        /// </summary>
        /// <typeparam name="T">The ECA Data entity type that will have a social media presence added.</typeparam>
        /// <param name="socialMediaModel">The social media presence to add.</param>
        /// <param name="controller">The controller handling the request.</param>
        /// <returns>The http action result.</returns>
        public async Task<IHttpActionResult> HandleSocialMediaPresenceAsync<T>(SocialMediaBindingModelBase<T> socialMediaModel, ApiController controller)
            where T : class, ISocialable
        {
            if (controller.ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var socialMedia = await this.socialMediaService.CreateAsync(socialMediaModel.ToSocialMediaPresence(businessUser));
                await this.socialMediaService.SaveChangesAsync();
                var dto = await this.socialMediaService.GetByIdAsync(socialMedia.SocialMediaId);
                var result = new OkNegotiatedContentResult<SocialMediaDTO>(dto, controller);
                return result;
            }
            else
            {
                return new InvalidModelStateResult(controller.ModelState, controller);
            }
        }
    }
}