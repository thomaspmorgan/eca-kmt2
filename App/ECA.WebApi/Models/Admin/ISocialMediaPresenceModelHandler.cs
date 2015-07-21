using System;
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
        System.Threading.Tasks.Task<System.Web.Http.IHttpActionResult> HandleSocialMediaPresenceAsync<T>(SocialMediaBindingModelBase<T> socialMediaModel, System.Web.Http.ApiController controller) where T : class, ECA.Data.ISocialable;
    }
}
