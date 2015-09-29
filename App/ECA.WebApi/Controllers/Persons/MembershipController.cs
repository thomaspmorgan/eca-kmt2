using ECA.Business.Service.Persons;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Security;
using System.Web.Http.Results;

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// The MembershipController provides clients with the Memberships for a person in the eca system.
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class MembershipController : ApiController
    {
        private static readonly ExpressionSorter<MembershipDTO> DEFAULT_SORTER = new ExpressionSorter<MembershipDTO>(x => x.Name, SortDirection.Ascending);

        private readonly IMembershipService service;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new MembershipController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="userProvider"></param>
        public MembershipController(IMembershipService service, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The service must not be null.");
            Contract.Requires(userProvider != null, "The userProvider must not be null.");
            this.service = service;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Returns the Memberships in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The Memberships.</returns>
        [ResponseType(typeof(PagedQueryResults<MembershipDTO>))]
        public async Task<IHttpActionResult> GetAsync([FromUri]PagingQueryBindingModel<MembershipDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var dtos = await service.GetAsync(queryModel.ToQueryableOperator(DEFAULT_SORTER));
                return Ok(dtos);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Adds a new membership to the person.
        /// </summary>
        /// <param name="model">The new membership.</param>
        /// <returns>The saved membership.</returns>
        [ResponseType(typeof(MembershipDTO))]
        [Route("People/{personId:int}/Membership")]
        public async Task<IHttpActionResult> PostMembershipAsync(NewPersonMembershipBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var membership = await service.CreateAsync(model.ToPersonMembership(businessUser));
                await service.SaveChangesAsync();
                var dto = await service.GetByIdAsync(membership.MembershipId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Updates a membership to the person.
        /// </summary>
        /// <param name="model">The updated membership.</param>
        /// <returns>void</returns>
        [ResponseType(typeof(MembershipDTO))]
        [Route("People/{personId:int}/Membership")]
        public async Task<IHttpActionResult> PutMembershipAsync(UpdatedPersonMembershipBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await service.UpdateAsync(model.ToUpdatedPersonMembership(businessUser));
                await service.SaveChangesAsync();
                var dto = await service.GetByIdAsync(model.Id);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Deletes the membership with the given id.
        /// </summary>
        /// <param name="id">The id of the membership.</param>
        /// <returns>An ok response.</returns>
        [ResponseType(typeof(OkResult))]
        [Route("People/{personId:int}/Membership/{id:int}")]
        public async Task<IHttpActionResult> DeleteMembership(int id)
        {
            await service.DeleteAsync(id);
            await service.SaveChangesAsync();
            return Ok();
        }
    }
}
