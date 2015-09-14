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
    /// The PhoneNumbersController is capable of handling updates on phone numbers within the ECA system.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/PhoneNumbers")]
    public class PhoneNumbersController : ApiController
    {
        private static ExpressionSorter<PhoneNumberTypeDTO> DEFAULT_SORTER = new ExpressionSorter<PhoneNumberTypeDTO>(x => x.Name, SortDirection.Ascending);
        private readonly IPhoneNumberTypeService phoneNumberTypeService;
        private readonly IPhoneNumberService phoneNumberService;
        private readonly IUserProvider userProvider;

        /// <summary>
        /// Creates a new instance of the controller.
        /// </summary>
        /// <param name="userProvider">The user provider.</param>
        /// <param name="phoneNumberTypeService">The social media type service.</param>
        /// <param name="phoneNumberService">The social media service.</param>
        public PhoneNumbersController(IUserProvider userProvider, IPhoneNumberTypeService phoneNumberTypeService, IPhoneNumberService phoneNumberService)
        {
            Contract.Requires(phoneNumberTypeService != null, "The phone number type service must not be null.");
            Contract.Requires(phoneNumberService != null, "The phone number service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.phoneNumberTypeService = phoneNumberTypeService;
            this.phoneNumberService = phoneNumberService;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Returns the email address types currently in the system.
        /// </summary>
        /// <returns>The email address types.</returns>
        [Route("Types")]
        [ResponseType(typeof(PhoneNumberTypeDTO))]
        public async Task<IHttpActionResult> GetPhoneNumberTypesAsync([FromUri]PagingQueryBindingModel<PhoneNumberTypeDTO> model)
        {
            if (ModelState.IsValid)
            {
                var dtos = await phoneNumberTypeService.GetAsync(model.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(dtos);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
