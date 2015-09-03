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
    /// The EmailAddressesController is capapble of handling updates on email addresses within the ECA system.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/EmailAddresses")]
    public class EmailAddressesController : ApiController
    {
        private static ExpressionSorter<EmailAddressTypeDTO> DEFAULT_SORTER = new ExpressionSorter<EmailAddressTypeDTO>(x => x.Name, SortDirection.Ascending);
        private readonly IEmailAddressTypeService emailAddressTypeService;
        private readonly IEmailAddressService emailAddressService;
        private readonly IUserProvider userProvider;

        /// <summary>
        /// Creates a new instance of the controller.
        /// </summary>
        /// <param name="userProvider">The user provider.</param>
        /// <param name="emailAddressTypeService">The social media type service.</param>
        /// <param name="emailAddressService">The social media service.</param>
        public EmailAddressesController(IUserProvider userProvider, IEmailAddressTypeService emailAddressTypeService, IEmailAddressService emailAddressService)
        {
            Contract.Requires(emailAddressTypeService != null, "The social media type service must not be null.");
            Contract.Requires(emailAddressService != null, "The social media service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.emailAddressTypeService = emailAddressTypeService;
            this.emailAddressService = emailAddressService;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Returns the email address types currently in the system.
        /// </summary>
        /// <returns>The email address types.</returns>
        [Route("Types")]
        [ResponseType(typeof(SocialMediaTypeDTO))]
        public async Task<IHttpActionResult> GetEmailAddressTypesAsync([FromUri]PagingQueryBindingModel<EmailAddressTypeDTO> model)
        {
            if (ModelState.IsValid)
            {
                var dtos = await emailAddressTypeService.GetAsync(model.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(dtos);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
