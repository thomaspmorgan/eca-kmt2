using CAM.Business.Queries.Models;
using CAM.Business.Service;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace ECA.WebApi.Controllers
{
    /// <summary>
    /// The UsersController is used to manage other user's access to resources.
    /// </summary>
    [Authorize]
    public class UsersController : ApiController
    {
        private static ExpressionSorter<UserDTO> DEFAULT_USER_SORTER = new ExpressionSorter<UserDTO>(x => x.Email, SortDirection.Ascending);

        private IUserService userService;

        /// <summary>
        /// Creates a new UsersController with the given user service.
        /// </summary>
        /// <param name="service">The user service.</param>
        public UsersController(IUserService service)
        {
            Contract.Requires(service != null, "The service must not be null.");
            this.userService = service;
        }

        /// <summary>
        /// Returns the users currently in the system.
        /// </summary>
        /// <param name="model">The paging, filtering, sorting parameters.</param>
        /// <returns>The users currently in the system.</returns>
        [ResponseType(typeof(PagedQueryResults<UserDTO>))]
        public async Task<IHttpActionResult> GetUsersAsync([FromUri]PagingQueryBindingModel<UserDTO> model)
        {
            if (ModelState.IsValid)
            {
                var users = await userService.GetUsersAsync(model.ToQueryableOperator(DEFAULT_USER_SORTER));
                return Ok(users);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
