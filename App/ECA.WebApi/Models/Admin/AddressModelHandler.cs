using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service;
using ECA.Business.Service.Admin;
using ECA.Core.Service;
using ECA.Data;
using ECA.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace ECA.WebApi.Models.Admin
{
    /// <summary>
    /// The AddressModelHandler is used a single point to add or update addresses for entities that are addressable in the ECA Business
    /// library.  All controller's that handle saving addresses should wrap this class.
    /// </summary>
    public class AddressModelHandler : ECA.WebApi.Models.Admin.IAddressModelHandler
    {
        private readonly ILocationService locationService;
        private readonly IUserProvider userProvider;

        /// <summary>
        /// Creates a new AddressModelHandler
        /// </summary>
        /// <param name="locationService">The location service to handle the addresses.</param>
        /// <param name="userProvider">The user provider to get current user information.</param>
        public AddressModelHandler(ILocationService locationService, IUserProvider userProvider)
        {
            Contract.Requires(locationService != null, "The location service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.locationService = locationService;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Handles a controller action's request to add an address to an entity.
        /// </summary>
        /// <typeparam name="T">The ECA Data entity type that will have an address added.</typeparam>
        /// <param name="additionalAddress">The additional address.</param>
        /// <param name="controller">The controller handling the request.</param>
        /// <returns>The http action result.</returns>
        public async Task<IHttpActionResult> HandleAdditionalAddressAsync<T>(AddressBindingModelBase<T> additionalAddress, ApiController controller)
            where T : class, IAddressable
        {
            if (controller.ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var address = await this.locationService.CreateAsync(additionalAddress.ToAdditionalAddress(businessUser));
                await this.locationService.SaveChangesAsync();
                var dto = await this.locationService.GetAddressByIdAsync(address.AddressId);
                var result = new OkNegotiatedContentResult<AddressDTO>(dto, controller);
                return result;
            }
            else
            {
                return new InvalidModelStateResult(controller.ModelState, controller);
            }
        }
    }
}