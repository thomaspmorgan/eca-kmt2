using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Sorter;
using ECA.Core.Query;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ECA.Data;
using ECA.Business.Queries.Models.Admin;
using ECA.WebApi.Models.Admin;

namespace ECA.WebApi.Controllers.Persons
{
    /// <summary>
    /// Controller for people
    /// </summary>
    [RoutePrefix("api")]
    [Authorize]
    public class PeopleController : ApiController
    {
        private static ExpressionSorter<SimplePersonDTO> DEFAULT_PEOPLE_SORTER = new ExpressionSorter<SimplePersonDTO>(x => x.LastName, SortDirection.Ascending);

        private IPersonService service;
        private IUserProvider userProvider;
        private IAddressModelHandler addressHandler;
        private ISocialMediaPresenceModelHandler socialMediaHandler;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="service">The service to inject</param>
        /// <param name="userProvider">The user provider.</param>
        /// <param name="addressHandler">The address handler.</param>
        /// <param name="socialMediaHandler">The social media handler.</param>
        public PeopleController(
            IPersonService service, 
            IUserProvider userProvider,
            IAddressModelHandler addressHandler,
            ISocialMediaPresenceModelHandler socialMediaHandler)
        {
            Contract.Requires(service != null, "The participant service must not be null.");
            Contract.Requires(userProvider != null, "The user provider must not be null.");
            Contract.Requires(addressHandler != null, "The address handler must not be null.");
            Contract.Requires(socialMediaHandler != null, "The social media handler must not be null.");
            this.addressHandler = addressHandler;
            this.service = service;
            this.userProvider = userProvider;
            this.socialMediaHandler = socialMediaHandler;
        }

        /// <summary>
        /// Returns data associated with person 
        /// </summary>
        /// <param name="personId">The person id to find </param>
        /// <returns>data associated to person</returns>
        [ResponseType(typeof(SimplePersonDTO))]
        [Route("Person/{personId:int}")]
        public async Task<IHttpActionResult> GetPersonByIdAsync(int personId)
        {
            var person = await service.GetSimplePersonAsync(personId);
            if (person != null)
            {
                return Ok(person);
            }
            else
            {
                return NotFound();
            }
        }


        /// <summary>
        /// Returns pii associated with person 
        /// </summary>
        /// <param name="personId">The person id to find pii for</param>
        /// <returns>Pii associated to person</returns>
        [ResponseType(typeof(PiiDTO))]
        [Route("People/{personId:int}/Pii")]
        public async Task<IHttpActionResult> GetPiiByIdAsync(int personId)
        {
            var pii = await service.GetPiiByIdAsync(personId);
            if (pii != null)
            {
                return Ok(pii);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returns contact info associated with a person
        /// </summary>
        /// <param name="personId">The person id to find contact info for</param>
        /// <returns>Contact info associated with person</returns>
        [ResponseType(typeof(ContactInfoDTO))]
        [Route("People/{personId:int}/ContactInfo")]
        public async Task<IHttpActionResult> GetContactInfoByIdAsync(int personId)
        {
            var contactInfo = await service.GetContactInfoByIdAsync(personId);
            if (contactInfo != null)
            {
                return Ok(contactInfo);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returns contact info associated with a person
        /// </summary>
        /// <param name="personId">The person id to find contact info for</param>
        /// <returns>Contact info associated with person</returns>
        [ResponseType(typeof(GeneralDTO))]
        [Route("People/{personId:int}/General")]
        public async Task<IHttpActionResult> GetGeneralByIdAsync(int personId)
        {
            var general = await service.GetGeneralByIdAsync(personId);
            if (general != null)
            {
                return Ok(general);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returns educations info associated with a person
        /// </summary>
        /// <param name="personId">The person id to find educations info for</param>
        /// <returns>Educations info associated with person</returns>
        [ResponseType(typeof(IList<EducationEmploymentDTO>))]
        [Route("People/{personId:int}/Education")]
        public async Task<IHttpActionResult> GetEducationsByPersonIdAsync(int personId)
        {
            var educations = await service.GetEducationsByPersonIdAsync(personId);
            if (educations != null)
            {
                return Ok(educations);
            }
            else
            {
                return NotFound();
            }
        }


        /// <summary>
        /// Returns employments info associated with a person
        /// </summary>
        /// <param name="personId">The person id to find employments info for</param>
        /// <returns>Employents info associated with person</returns>
        [ResponseType(typeof(IList<EducationEmploymentDTO>))]
        [Route("People/{personId:int}/Employment")]
        public async Task<IHttpActionResult> GetEmploymentsByPersonIdAsync(int personId)
        {
            var employments = await service.GetEmploymentsByPersonIdAsync(personId);
            if (employments != null)
            {
                return Ok(employments);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returns Evaluation-Notes info associated with a person
        /// </summary>
        /// <param name="personId">The person id to find Evalution-Notes info for</param>
        /// <returns>Evaluation-Notes info associated with person</returns>
        [ResponseType(typeof(IList<EvaluationNoteDTO>))]
        [Route("People/{personId:int}/EvaluationNotes")]
        public async Task<IHttpActionResult> GetEvaluationNotesByPersonIdAsync(int personId)
        {
            var evaluationNotes = await service.GetEvaluationNotesByPersonIdAsync(personId);
            if (evaluationNotes != null)
            {
                return Ok(evaluationNotes);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Returns sorted, filtered, and paged people in the eca system.
        /// </summary>
        /// <param name="model">The filters, paging, and sorting details.</param>
        /// <returns>The people in the ssytem.</returns>
        [ResponseType(typeof(PagedQueryResults<SimplePersonDTO>))]
        public async Task<IHttpActionResult> GetPeopleAsync([FromUri]PagingQueryBindingModel<SimplePersonDTO> model)
        {
            if (ModelState.IsValid)
            {
                var results = await this.service.GetPeopleAsync(model.ToQueryableOperator(DEFAULT_PEOPLE_SORTER, x => x.FullName));
                return Ok(results);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Post method to create a person
        /// </summary>
        /// <param name="model">The model to create</param>
        /// <returns>Success or error</returns>
        public async Task<IHttpActionResult> PostPersonAsync(PersonBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var person = await service.CreateAsync(model.ToNewPerson(businessUser));
                await service.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Put method to update a person
        /// </summary>
        /// <param name="model">The model to update</param>
        /// <returns></returns>
        [Route("People/Pii")]
        public async Task<IHttpActionResult> PutPiiAsync(PiiBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var person = await service.UpdatePiiAsync(model.ToUpdatePii(businessUser));
                await service.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        /// <summary>
        /// Put method to update a person's General Info
        /// </summary>
        /// <param name="model">The model to update</param>
        /// <returns></returns>
        [Route("People/General")]
        public async Task<IHttpActionResult> PutGeneralAsync(GeneralBindingModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = userProvider.GetCurrentUser();
                var businessUser = userProvider.GetBusinessUser(currentUser);
                var person = await service.UpdateGeneralAsync(model.ToUpdateGeneral(businessUser));
                await service.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Adds a new address to the person.
        /// </summary>
        /// <param name="model">The new address.</param>
        /// <returns>The saved address.</returns>
        [Route("People/Address")]
        [ResponseType(typeof(AddressDTO))]
        public Task<IHttpActionResult> PostAddressAsync([FromBody]PersonAddressBindingModel model)
        {
            return addressHandler.HandleAdditionalAddressAsync<Person>(model, this);
        }

        /// <summary>
        /// Adds a new social media to the person.
        /// </summary>
        /// <param name="model">The new social media.</param>
        /// <returns>The saved social media.</returns>
        [Route("People/SocialMedia")]
        [ResponseType(typeof(SocialMediaDTO))]
        public Task<IHttpActionResult> PostSocialMediaAsync([FromBody]PersonSocialMediaPresenceBindingModel model)
        {
            return socialMediaHandler.HandleSocialMediaPresenceAsync<Person>(model, this);
        }

        /// <summary>
        /// Adds a new membership to the person.
        /// </summary>
        /// <param name="model">The new membership.</param>
        /// <returns>The saved membership.</returns>
        [Route("People/Membership")]
        [ResponseType(typeof(MembershipDTO))]
        public Task<IHttpActionResult> PostMembershipAsync([FromBody]PersonMembershipBindingModel model)
        {
            return membershipHandler.HandleMembershipAsync<Person>(model, this);
        }
    }
}
