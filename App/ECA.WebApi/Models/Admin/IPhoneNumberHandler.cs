using System;
using System.Threading.Tasks;
using System.Web.Http;
namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The PhoneNumberHandler will be used to handle new phone number to IPhoneNumberable entities in the ECA system.
    /// </summary>
    public interface IPhoneNumberHandler
    {
        /// <summary>
        /// Handles a controller action's request to add a phone number to an entity.
        /// </summary>
        /// <typeparam name="T">The ECA Data entity type that will have a phone number added.</typeparam>
        /// <param name="phoneNumberModel">The phone number to add.</param>
        /// <param name="controller">The controller handling the request.</param>
        /// <returns>The http action result.</returns>
        Task<IHttpActionResult> HandlePhoneNumberAsync<T>(PhoneNumberBindingModelBase<T> phoneNumberModel, System.Web.Http.ApiController controller) where T : class, ECA.Data.IPhoneNumberable;

        /// <summary>
        /// Handles a controller action's request to update phone number.
        /// </summary>
        /// <param name="updatedPhoneNumber">The updated phone number.</param>
        /// <param name="controller">The controller making the request.</param>
        /// <returns>The updated phone number.</returns>
        Task<IHttpActionResult> HandleUpdatePhoneNumberAsync(UpdatedPhoneNumberBindingModel updatedPhoneNumber, ApiController controller);

        /// <summary>
        /// Handles a controller action's request to delete a phone number by id.
        /// </summary>
        /// <param name="id">The id of the phone number.</param>
        /// <param name="controller">The controller making the request.</param>
        /// <returns>An ok result.</returns>
        Task<IHttpActionResult> HandleDeletePhoneNumberAsync(int id, ApiController controller);
    }
}
