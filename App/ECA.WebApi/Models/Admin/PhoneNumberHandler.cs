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
    /// The PhoneNumberHandler will be used to handle new phone numbers to IPhoneNumberable entities in the ECA system.
    /// </summary>
    public class PhoneNumberHandler : IPhoneNumberHandler
    {
        private readonly IPhoneNumberService phoneNumberService;
        private readonly IUserProvider userProvider;

        /// <summary>
        /// Creates a new PhoneNumberHandler
        /// </summary>
        /// <param name="phoneNumberService">The phone number service.</param>
        /// <param name="userProvider">The user provider to get current user information.</param>
        public PhoneNumberHandler(IPhoneNumberService phoneNumberService, IUserProvider userProvider)
        {
            Contract.Requires(phoneNumberService != null, "The email address service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.phoneNumberService = phoneNumberService;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Handles a controller action's request to add a phone number to an entity.
        /// </summary>
        /// <typeparam name="T">The ECA Data entity type that will have a phone number added.</typeparam>
        /// <param name="phoneNumberModel">The phone number to add.</param>
        /// <param name="controller">The controller handling the request.</param>
        /// <returns>The http action result.</returns>
        public async Task<IHttpActionResult> HandlePhoneNumberAsync<T>(PhoneNumberBindingModelBase<T> phoneNumberModel, ApiController controller)
            where T : class, IPhoneNumberable
        {
            if (controller.ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var phoneNumber = await this.phoneNumberService.CreateAsync(phoneNumberModel.ToPhoneNumber(businessUser));
                await this.phoneNumberService.SaveChangesAsync();
                var dto = await this.phoneNumberService.GetByIdAsync(phoneNumber.PhoneNumberId);
                var result = new OkNegotiatedContentResult<PhoneNumberDTO>(dto, controller);
                return result;
            }
            else
            {
                return new InvalidModelStateResult(controller.ModelState, controller);
            }
        }

        /// <summary>
        /// Handles a controller action's request to update phone number.
        /// </summary>
        /// <param name="updatedPhoneNumber">The updated phone number.</param>
        /// <param name="controller">The controller making the request.</param>
        /// <returns>The updated phone number.</returns>
        public async Task<IHttpActionResult> HandleUpdatePhoneNumberAsync(UpdatedPhoneNumberBindingModel updatedPhoneNumber, ApiController controller)
        {
            if (controller.ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(currentUser);
                await this.phoneNumberService.UpdateAsync(updatedPhoneNumber.ToUpdatedPhoneNumber(businessUser));
                await this.phoneNumberService.SaveChangesAsync();
                var dto = await this.phoneNumberService.GetByIdAsync(updatedPhoneNumber.Id);
                return new OkNegotiatedContentResult<PhoneNumberDTO>(dto, controller);
            }
            else
            {
                return new InvalidModelStateResult(controller.ModelState, controller);
            }
        }

        /// <summary>
        /// Handles a controller action's request to delete a phone number by id.
        /// </summary>
        /// <param name="id">The id of the phone number.</param>
        /// <param name="controller">The controller making the request.</param>
        /// <returns>An ok result.</returns>
        public async Task<IHttpActionResult> HandleDeletePhoneNumberAsync(int id, ApiController controller)
        {
            await this.phoneNumberService.DeleteAsync(id);
            await this.phoneNumberService.SaveChangesAsync();
            return new OkResult(controller);
        }
    }
}