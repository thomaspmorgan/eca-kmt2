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
    /// The EmailAddressHandler will be used to handle new email addresses to IEmailAddressable entities in the ECA system.
    /// </summary>
    public class EmailAddressHandler : ECA.WebApi.Models.Admin.IEmailAddressHandler
    {
        private readonly IEmailAddressService emailAddressService;
        private readonly IUserProvider userProvider;

        /// <summary>
        /// Creates a new AddressModelHandler
        /// </summary>
        /// <param name="emailAddressService">The email address service.</param>
        /// <param name="userProvider">The user provider to get current user information.</param>
        public EmailAddressHandler(IEmailAddressService emailAddressService, IUserProvider userProvider)
        {
            Contract.Requires(emailAddressService != null, "The email address service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.emailAddressService = emailAddressService;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Handles a controller action's request to add a email address to an entity.
        /// </summary>
        /// <typeparam name="T">The ECA Data entity type that will have a email address added.</typeparam>
        /// <param name="emailAddressModel">The email address to add.</param>
        /// <param name="controller">The controller handling the request.</param>
        /// <returns>The http action result.</returns>
        public async Task<IHttpActionResult> HandleEmailAddressAsync<T>(EmailAddressBindingModelBase<T> emailAddressModel, ApiController controller)
            where T : class, IEmailAddressable
        {
            if (controller.ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var emailAddress = await this.emailAddressService.CreateAsync(emailAddressModel.ToEmailAddress(businessUser));
                await this.emailAddressService.SaveChangesAsync();
                var dto = await this.emailAddressService.GetByIdAsync(emailAddress.EmailAddressId);
                var result = new OkNegotiatedContentResult<EmailAddressDTO>(dto, controller);
                return result;
            }
            else
            {
                return new InvalidModelStateResult(controller.ModelState, controller);
            }
        }

        /// <summary>
        /// Handles a controller action's request to update email address.
        /// </summary>
        /// <param name="updatedEmailAddress">The updated email address.</param>
        /// <param name="controller">The controller making the request.</param>
        /// <returns>The updated email address.</returns>
        public async Task<IHttpActionResult> HandleUpdateEmailAddressAsync(UpdatedEmailAddressBindingModel updatedEmailAddress, ApiController controller)
        {
            if (controller.ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(currentUser);
                await this.emailAddressService.UpdateAsync(updatedEmailAddress.ToUpdatedEmailAddress(businessUser));
                await this.emailAddressService.SaveChangesAsync();
                var dto = await this.emailAddressService.GetByIdAsync(updatedEmailAddress.Id);
                return new OkNegotiatedContentResult<EmailAddressDTO>(dto, controller);
            }
            else
            {
                return new InvalidModelStateResult(controller.ModelState, controller);
            }
        }

        /// <summary>
        /// Handles a controller action's request to delete a social media presence by id.
        /// </summary>
        /// <param name="id">The id of the social media.</param>
        /// <param name="controller">The controller making the request.</param>
        /// <returns>An ok result.</returns>
        public async Task<IHttpActionResult> HandleDeleteEmailAddressAsync(int id, ApiController controller)
        {
            await this.emailAddressService.DeleteAsync(id);
            await this.emailAddressService.SaveChangesAsync();
            return new OkResult(controller);
        }
    }
}