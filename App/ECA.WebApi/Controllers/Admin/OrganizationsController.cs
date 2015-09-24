using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Lookup;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.Data;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace ECA.WebApi.Controllers.Admin
{
    /// <summary>
    /// The organizations controller
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class OrganizationsController : ApiController
    {
        private static readonly ExpressionSorter<SimpleOrganizationDTO> DEFAULT_SORTER = new ExpressionSorter<SimpleOrganizationDTO>(x => x.Name, SortDirection.Ascending);
        private static readonly ExpressionSorter<OrganizationTypeDTO> DEFAULT_ORGANIZATION_TYPE_SORTER = new ExpressionSorter<OrganizationTypeDTO>(x => x.Name, SortDirection.Ascending);
        private static readonly ExpressionSorter<SimpleLookupDTO> DEFAULT_ORGANIZATION_ROLE_SORTER = new ExpressionSorter<SimpleLookupDTO>(x => x.Value, SortDirection.Ascending);
        private IOrganizationService organizationService;
        private IOrganizationTypeService organizationTypeService;
        private IUserProvider userProvider;
        private IAddressModelHandler addressHandler;
        private ISocialMediaPresenceModelHandler socialMediaHandler;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">The service</param>
        /// <param name="organizationTypeService">The organization type service.</param>
        /// <param name="addressHandler">The address handler.</param>
        /// <param name="userProvider">The user provider.</param>
        /// <param name="socialMediaHandler">The social media handler.</param>
        public OrganizationsController(
            IOrganizationService service,
            IOrganizationTypeService organizationTypeService,
            IUserProvider userProvider,
            IAddressModelHandler addressHandler,
            ISocialMediaPresenceModelHandler socialMediaHandler)
        {
            Contract.Requires(service != null, "The organization service must not be null.");
            Contract.Requires(organizationTypeService != null, "The organization type service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            Contract.Requires(addressHandler != null, "The address handler must not be null.");
            Contract.Requires(socialMediaHandler != null, "The social media handler must not be null.");
            this.organizationService = service;
            this.organizationTypeService = organizationTypeService;
            this.userProvider = userProvider;
            this.addressHandler = addressHandler;
            this.socialMediaHandler = socialMediaHandler;
        }

        /// <summary>
        /// Returns the organizations in the system.
        /// </summary>
        /// <param name="queryModel">The query operator.</param>
        /// <returns>The organizations in the system.</returns>
        [ResponseType(typeof(PagedQueryResults<SimpleOrganizationDTO>))]
        [Route("Organizations")]
        public async Task<IHttpActionResult> GetOrganizationsAsync([FromUri]PagingQueryBindingModel<SimpleOrganizationDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var results = await organizationService.GetOrganizationsAsync(
                    queryModel.ToQueryableOperator(DEFAULT_SORTER, x => x.Name, x => x.Location, x => x.Status));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the organization with the given id.
        /// </summary>
        /// <param name="id">The id of the organization.</param>
        /// <returns>The organization.</returns>
        [ResponseType(typeof(OrganizationDTO))]
        [Route("Organizations/{id}")]
        public async Task<IHttpActionResult> GetOrganizationByIdAsync(int id)
        {
            var results = await organizationService.GetOrganizationByIdAsync(id);
            if (results != null)
            {
                return Ok(results);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Updates the organization with the given data.
        /// </summary>
        /// <param name="model">The updated model.</param>
        /// <returns>The updated organization.</returns>
        [ResponseType(typeof(OrganizationDTO))]
        [Route("Organizations")]
        public async Task<IHttpActionResult> PutUpdateOrganizationAsync([FromBody]UpdatedOrganizationBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await organizationService.UpdateAsync(model.ToEcaOrganization(businessUser));
                await organizationService.SaveChangesAsync();
                var updatedOrganization = await organizationService.GetOrganizationByIdAsync(model.OrganizationId);
                return Ok(updatedOrganization);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the organization types in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The organization types.</returns>
        [Route("Organizations/Types")]
        [ResponseType(typeof(PagedQueryResults<OrganizationTypeDTO>))]
        public async Task<IHttpActionResult> GetOrganizationTypesAsync([FromUri]PagingQueryBindingModel<OrganizationTypeDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                return Ok(await organizationTypeService.GetAsync(queryModel.ToQueryableOperator(DEFAULT_ORGANIZATION_TYPE_SORTER)));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the organization roles in the system
        /// </summary>
        /// <param name="queryModel">The query model</param>
        /// <returns>The organization roles</returns>
        [Route("Organizations/Roles")]
        [ResponseType(typeof(PagedQueryResults<SimpleLookupDTO>))]
        public async Task<IHttpActionResult> GetOrganizationRolesAsync([FromUri]PagingQueryBindingModel<SimpleLookupDTO> queryModel)
        {
            if (ModelState.IsValid)
            {
                var organizationRoles = await organizationService.GetOrganizationRolesAsync(queryModel.ToQueryableOperator(DEFAULT_ORGANIZATION_ROLE_SORTER));
                return Ok(organizationRoles);
            } else
            {
                return BadRequest(ModelState);
            }
        }

        #region Addresses
        /// <summary>
        /// Adds a new address to the organization.
        /// </summary>
        /// <param name="model">The new address.</param>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The saved address.</returns>
        [Route("Organizations/{organizationId:int}/Address")]
        [ResponseType(typeof(AddressDTO))]
        public Task<IHttpActionResult> PostAddressAsync(int organizationId, [FromBody]OrganizationAddressBindingModel model)
        {
            return addressHandler.HandleAdditionalAddressAsync<Organization>(model, this);
        }

        /// <summary>
        /// Updates the address for the organization.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="model">The updated address.</param>
        /// <returns>The saved address.</returns>
        [Route("Organizations/{organizationId:int}/Address")]
        [ResponseType(typeof(AddressDTO))]
        public Task<IHttpActionResult> PutAddressAsync(int organizationId, [FromBody]UpdatedAddressBindingModel model)
        {
            return addressHandler.HandleUpdateAddressAsync(model, this);
        }

        /// <summary>
        /// Deletes the address from the organization.
        /// </summary>
        /// <param name="organizationId">The organization id.</param>
        /// <param name="addressId">The address id.</param>
        /// <returns>An Ok result.</returns>
        [Route("Organizations/{organizationId:int}/Address/{addressId:int}")]
        [ResponseType(typeof(OkResult))]
        public Task<IHttpActionResult> DeleteAddressAsync(int organizationId, int addressId)
        {
            return addressHandler.HandleDeleteAddressAsync(addressId, this);
        }

        #endregion

        #region Social Media

        /// <summary>
        /// Adds a new social media to the person.
        /// </summary>
        /// <param name="model">The new social media.</param>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The saved social media.</returns>
        [Route("Organizations/{organizationId:int}/SocialMedia")]
        [ResponseType(typeof(SocialMediaDTO))]
        public Task<IHttpActionResult> PostSocialMediaAsync(int organizationId, [FromBody]OrganizationSocialMediaPresenceBindingModel model)
        {
            return socialMediaHandler.HandleSocialMediaPresenceAsync<Organization>(model, this);
        }

        /// <summary>
        /// Updates the new social media presence of the organization.
        /// </summary>
        /// <param name="model">The new social media.</param>
        /// <param name="organizationId">The organization id.</param>
        /// <returns>The saved social media.</returns>
        [Route("Organizations/{organizationId:int}/SocialMedia")]
        [ResponseType(typeof(SocialMediaDTO))]
        public Task<IHttpActionResult> PutUpdateSocialMediaAsync(int organizationId, [FromBody]UpdatedSocialMediaBindingModel model)
        {
            return socialMediaHandler.HandleUpdateSocialMediaAsync(model, this);
        }

        /// <summary>
        /// Deletes the social media presence from the organization.
        /// </summary>
        /// <param name="socialMediaId">The socialMediaId id.</param>
        /// <param name="organizationId">The person id.</param>
        /// <returns>An ok result.</returns>
        [Route("Organizations/{organizationId:int}/SocialMedia/{socialMediaId:int}")]
        [ResponseType(typeof(OkResult))]
        public Task<IHttpActionResult> DeleteSocialMediaAsync(int organizationId, int socialMediaId)
        {
            return socialMediaHandler.HandleDeleteSocialMediaAsync(socialMediaId, this);
        }

        #endregion
    }
}
