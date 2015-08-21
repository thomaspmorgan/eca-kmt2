using System;
using System.Threading.Tasks;
using System.Web.Http;
namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The SocialMediaPresenceModelHandler will be used to handle new social media presences to ISocialable entities in the ECA system.
    /// </summary>
    public interface ISocialMediaPresenceModelHandler
    {
        /// <summary>
        /// Handles a controller action's request to add a social media presence to an entity.
        /// </summary>
        /// <typeparam name="T">The ECA Data entity type that will have a social media presence added.</typeparam>
        /// <param name="socialMediaModel">The social media presence to add.</param>
        /// <param name="controller">The controller handling the request.</param>
        /// <returns>The http action result.</returns>
        Task<IHttpActionResult> HandleSocialMediaPresenceAsync<T>(SocialMediaBindingModelBase<T> socialMediaModel, System.Web.Http.ApiController controller) where T : class, ECA.Data.ISocialable;

        /// <summary>
        /// Handles a controller action's request to update social media.
        /// </summary>
        /// <param name="updatedSocialMedia">The updated social media.</param>
        /// <param name="controller">The controller making the request.</param>
        /// <returns>The updated social media.</returns>
        Task<IHttpActionResult> HandleUpdateSocialMediaAsync(UpdatedSocialMediaBindingModel updatedSocialMedia, ApiController controller);

        /// <summary>
        /// Handles a controller action's request to delete a social media presence by id.
        /// </summary>
        /// <param name="id">The id of the social media.</param>
        /// <param name="controller">The controller making the request.</param>
        /// <returns>An ok result.</returns>
        Task<IHttpActionResult> HandleDeleteSocialMediaAsync(int id, ApiController controller);
    }
}
