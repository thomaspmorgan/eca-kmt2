using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Lookup;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Settings;
using ECA.WebApi.Custom.Storage;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using System.Diagnostics.Contracts;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// Controller for dependents
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class DependentsController : ApiController
    {
        private static ExpressionSorter<DependentTypeDTO> DEFAULT_PERSON_TYPE_SORTER = new ExpressionSorter<DependentTypeDTO>(x => x.Name, SortDirection.Ascending);
        private static ExpressionSorter<BirthCountryReasonDTO> DEFAULT_BIRTH_COUNTRY_REASON_SORTER = new ExpressionSorter<BirthCountryReasonDTO>(x => x.Name, SortDirection.Ascending);

        private IPersonService service;
        private IDependentTypeService dependentTypeService;
        private IBirthCountryReasonService birthCountryReasonService;
        private IUserProvider userProvider;
        private IFileStorageHandler storageHandler;
        private AppSettings appSettings;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="dependentTypeService"></param>
        /// <param name="birthCountryReasonService">The birth country reason service</param>
        /// <param name="userProvider">The user provider.</param>
        /// <param name="appSettings">App settings.</param>
        /// <param name="storageHandler">The storage handler.</param>
        public DependentsController(IPersonService service, 
            IDependentTypeService dependentTypeService,
            IBirthCountryReasonService birthCountryReasonService,
            IFileStorageHandler storageHandler, 
            AppSettings appSettings,
            IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The participant service must not be null.");
            Contract.Requires(dependentTypeService != null, "The dependent type service must not be null.");
            Contract.Requires(birthCountryReasonService != null, "The birth country reason service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            this.service = service;
            this.dependentTypeService = dependentTypeService;
            this.birthCountryReasonService = birthCountryReasonService;
            this.userProvider = userProvider;
            this.appSettings = appSettings;
            this.storageHandler = storageHandler;
        }

        /// <summary>
        /// Returns data associated with person dependent
        /// </summary>
        /// <param name="dependentId">The dependent id to find</param>
        /// <returns></returns>
        [ResponseType(typeof(SimplePersonDependentDTO))]
        [Route("People/Dependent/{dependentId:int}")]
        public async Task<IHttpActionResult> GetPersonDependentByIdAsync(int dependentId)
        {
            var dependent = await service.GetPersonDependentByIdAsync(dependentId);
            if (dependent != null)
            {
                return Ok(dependent);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Gets the DS2019 file asyncronously
        /// </summary>
        /// <param name="dependentId">The person dependent id.</param>
        /// <returns>DS2019 file</returns>
        [Route("People/Dependent/{dependentId:int}/DS2019File")]
        public async Task<HttpResponseMessage> GetDS2019FileAsync(int dependentId)
        {
            var currentUser = userProvider.GetCurrentUser();
            var businessUser = userProvider.GetBusinessUser(currentUser);
            var fileName = await service.GetDS2019FileNameAsync(businessUser, dependentId);
            if (fileName != null)
            {
                var container = appSettings.DS2019FileStorageContainer;
                var message = await storageHandler.GetFileAsync(fileName, container);
                if (message != null)
                {
                    return message;
                }
            }

            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "File not found.");
        }

        /// <summary>
        /// Post method to create a person dependent
        /// </summary>
        /// <param name="model">The model to create</param>
        /// <returns></returns>
        [Route("Dependent")]
        [ResponseType(typeof(SimplePersonDependentDTO))]
        public async Task<IHttpActionResult> PostPersonDependentAsync([FromBody]DependentBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var person = await service.CreateDependentAsync(model.ToNewDependent(businessUser));
                await service.SaveChangesAsync();
                var dto = await this.service.GetPersonDependentByIdAsync(person.DependentId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Put method to update a person dependent
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("People/Dependent")]
        public async Task<IHttpActionResult> PutDependentAsync(UpdatedPersonDependentBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var dependent = await service.UpdatePersonDependentAsync(model.ToUpdatePersonDependent(businessUser));
                await service.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Delete a dependent permanently
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ResponseType(typeof(OkResult))]
        [Route("People/Dependent/{dependentId:int}")]
        public async Task<IHttpActionResult> PutDependentDeleteAsync(UpdatedPersonDependentBindingModel model)
        {
            var currentUser = userProvider.GetCurrentUser();
            var businessUser = userProvider.GetBusinessUser(currentUser);
            var updateDependent = model.ToUpdatePersonDependent(businessUser);
            await service.DeleteDependentAsync(updateDependent);
            await service.SaveChangesAsync();
            return Ok();
        }
        
        /// <summary>
        /// Returns the person types in the system.
        /// </summary>
        /// <returns>The person types.</returns>
        [ResponseType(typeof(PagingQueryBindingModel<DependentTypeDTO>))]
        [Route("Dependent/Types")]
        public async Task<IHttpActionResult> GetDependentTypesAsync([FromUri]PagingQueryBindingModel<DependentTypeDTO> model)
        {
            if (ModelState.IsValid)
            {
                var results = await this.dependentTypeService.GetAsync(model.ToQueryableOperator(DEFAULT_PERSON_TYPE_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Returns the birth country reasons in the system.
        /// </summary>
        /// <returns>The birth country reasons.</returns>
        [ResponseType(typeof(PagingQueryBindingModel<BirthCountryReasonDTO>))]
        [Route("Birthcountryreasons")]
        public async Task<IHttpActionResult> GetBirthCountryReasonsAsync([FromUri]PagingQueryBindingModel<BirthCountryReasonDTO> model)
        {
            if (ModelState.IsValid)
            {
                var results = await this.birthCountryReasonService.GetAsync(model.ToQueryableOperator(DEFAULT_BIRTH_COUNTRY_REASON_SORTER));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}