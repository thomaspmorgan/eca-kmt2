using System;
using System.Threading.Tasks;
using System.Web.Http;
namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The EmailAddresslHandler will be used to handle new email addresses to IEmailAddressable entities in the ECA system.
    /// </summary>
    public interface IEmailAddressHandler
    {
        /// <summary>
        /// Handles a controller action's request to add a email address to an entity.
        /// </summary>
        /// <typeparam name="T">The ECA Data entity type that will have a email address added.</typeparam>
        /// <param name="emailAddressModel">The email address to add.</param>
        /// <param name="controller">The controller handling the request.</param>
        /// <returns>The http action result.</returns>
        Task<IHttpActionResult> HandleEmailAddressAsync<T>(EmailAddressBindingModelBase<T> emailAddressModel, System.Web.Http.ApiController controller) where T : class, ECA.Data.IEmailAddressable;

        /// <summary>
        /// Handles a controller action's request to update email address.
        /// </summary>
        /// <param name="updatedEmailAddress">The updated email address.</param>
        /// <param name="controller">The controller making the request.</param>
        /// <returns>The updated email address.</returns>
        Task<IHttpActionResult> HandleUpdateEmailAddressAsync(UpdatedEmailAddressBindingModel updatedEmailAddress, ApiController controller);

        /// <summary>
        /// Handles a controller action's request to delete a email address by id.
        /// </summary>
        /// <param name="id">The id of the email address.</param>
        /// <param name="controller">The controller making the request.</param>
        /// <returns>An ok result.</returns>
        Task<IHttpActionResult> HandleDeleteEmailAddressAsync(int id, ApiController controller);
    }
}
