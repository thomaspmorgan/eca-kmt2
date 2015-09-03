using ECA.Business.Service.Persons;
using ECA.Business.Queries.Models.Persons;
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
    public class LanguageProficiencyController : ApiController
    {
        private static readonly ExpressionSorter<LanguageProficiencyDTO> DEFAULT_SORTER = new ExpressionSorter<LanguageProficiencyDTO>(x => x.LanguageName, SortDirection.Ascending);

        private readonly ILanguageProficiencyService service;
        private IUserProvider userProvider;

        /// <summary>
        /// Creates a new LanguageProficiencyController with the given service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="userProvider">The user provider service.</param>
        public LanguageProficiencyController(ILanguageProficiencyService service, IUserProvider userProvider)
        {
            Contract.Requires(service != null, "The service must not be null.");
            Contract.Requires(userProvider != null, "The userProvider must not be null.");
            this.service = service;
            this.userProvider = userProvider;
        }

        /// <summary>
        /// Returns the LanguageProficiencies in the system.
        /// </summary>
        /// <param name="queryModel">The query model.</param>
        /// <returns>The LanguageProficiencies.</returns>
        [ResponseType(typeof(PagedQueryResults<LanguageProficiencyDTO>))]
        public async Task<IHttpActionResult> GetAsync([FromUri]PagingQueryBindingModel<LanguageProficiencyDTO> queryModel)
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
        /// Adds a new LanguageProficiency to the person.
        /// </summary>
        /// <param name="model">The new languageProficiency.</param>
        /// <returns>The saved LanguageProficiency.</returns>
        [ResponseType(typeof(LanguageProficiencyDTO))]
        [Route("People/{personId:int}/LanguageProficiency")]
        public async Task<IHttpActionResult> PostLanguageProficiencyAsync(NewPersonLanguageProficiencyBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var languageProficiency = await service.CreateAsync(model.ToNewPersonLanguageProficiency(businessUser));
                await service.SaveChangesAsync();
                var dto = await service.GetByIdAsync(languageProficiency.LanguageId, languageProficiency.PersonId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Updates a LanguageProficiency to the person.
        /// </summary>
        /// <param name="model">The updated LanguageProficiency.</param>
        /// <returns>void</returns>
        [ResponseType(typeof(LanguageProficiencyDTO))]
        [Route("People/{personId:int}/LanguageProficiency")]
        public async Task<IHttpActionResult> PutLanguageProficiencyAsync(UpdatedPersonLanguageProficiencyBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                await service.UpdateAsync(model.ToUpdatedPersonLanguageProficiency(businessUser));
                await service.SaveChangesAsync();
                var dto = await service.GetByIdAsync(model.LanguageId, model.PersonId);
                return Ok(dto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Deletes the LanguageProficiency with the given id.
        /// </summary>
        /// <param name="languageId">The id of the person.</param>
        /// <param name="personId">The id of the person.</param>
        /// <returns>An ok response.</returns>
        [ResponseType(typeof(OkResult))]
        [Route("People/{personId:int}/LanguageProficiency/{languageId:int}")]
        public async Task<IHttpActionResult> DeleteLanguageProficiency(int languageId, int personId)
        {
            await service.DeleteAsync(languageId,personId);
            await service.SaveChangesAsync();
            return Ok();
        }
    }
}
