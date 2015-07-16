using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.WebApi.Models.Admin;
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

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The AddressesController is used to handle crud operations on addresses directly.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Addresses")]
    public class AddressesController : ApiController
    {
        private readonly ILocationService locationService;
        private readonly IUserProvider userProvider;

        /// <summary>
        /// Creates a new instance with the given ILocationService.
        /// </summary>
        /// <param name="locationService">The location serivce.</param>
        /// <param name="userProvider">The user provider.</param>
        public AddressesController(ILocationService locationService, IUserProvider userProvider)
        {
            Contract.Requires(locationService != null, "The location service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.locationService = locationService;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Updates the system's address with the given updated address.
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(AddressDTO))]
        public async Task<IHttpActionResult> PutUpdateAddressAsync(UpdatedAddressBindingModel updatedAddress)
        {
            if (ModelState.IsValid)
            {
                var currentUser = this.userProvider.GetCurrentUser();
                var businessUser = this.userProvider.GetBusinessUser(currentUser);
                await this.locationService.UpdateAsync(updatedAddress.ToUpdatedEcaAddress(businessUser));
                await this.locationService.SaveChangesAsync();
                var dto = await this.locationService.GetAddressByIdAsync(updatedAddress.AddressId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
