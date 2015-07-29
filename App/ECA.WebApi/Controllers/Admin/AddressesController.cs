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
    /// The AddressesController is used to handle crud operations on addresses directly.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Addresses")]
    public class AddressesController : ApiController
    {
        private static ExpressionSorter<AddressTypeDTO> DEFAULT_SORTER = new ExpressionSorter<AddressTypeDTO>(x => x.Name, SortDirection.Ascending);
        private readonly ILocationService locationService;
        private readonly IUserProvider userProvider;
        private readonly IAddressTypeService addressTypeService;

        /// <summary>
        /// Creates a new instance with the given ILocationService.
        /// </summary>
        /// <param name="locationService">The location serivce.</param>
        /// <param name="userProvider">The user provider.</param>
        /// <param name="addressTypeService">The address type service.</param>
        public AddressesController(ILocationService locationService, IUserProvider userProvider, IAddressTypeService addressTypeService)
        {
            Contract.Requires(locationService != null, "The location service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            Contract.Requires(addressTypeService != null, "The address type service must not be null.");
            this.locationService = locationService;
            this.userProvider = userProvider;
            this.addressTypeService = addressTypeService;
        }

        /// <summary>
        /// Updates the system's address with the given updated address.
        /// </summary>
        /// <returns>The updated address.</returns>
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

        /// <summary>
        /// Deletes the system's address with the given id.
        /// </summary>
        /// <returns>The updated address.</returns>
        [ResponseType(typeof(OkResult))]
        public async Task<IHttpActionResult> DeleteAddressAsync(int id)
        {
            await this.locationService.DeleteAsync(id);
            await this.locationService.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Returns the address types currently in the system.
        /// </summary>
        /// <returns>The address types.</returns>
        [Route("Types")]
        [ResponseType(typeof(AddressDTO))]
        public async Task<IHttpActionResult> GetAddressTypesAsync([FromUri]PagingQueryBindingModel<AddressTypeDTO> model)
        {
            if (ModelState.IsValid)
            {
                var dtos = await addressTypeService.GetAsync(model.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(dtos);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
